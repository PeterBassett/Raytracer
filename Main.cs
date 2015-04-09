using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Raytracer.Extensions;
using Raytracer.Rendering.Cameras;
using Raytracer.Rendering.Core;
using Raytracer.Rendering.FileTypes.VBRayScene;
using Raytracer.Rendering.PixelSamplers;
using Raytracer.Rendering.Renderers;

namespace Raytracer
{
    public partial class Main : Form
    {
        private Scene m_scene = null;
        private string m_sceneFile;
        private bool m_isSceneDefinitionDirty = true;

        public Main()
        {
            InitializeComponent();

            var scenes = Directory.GetFiles(".", "*.ray", SearchOption.AllDirectories).ToList();

            foreach (var file in scenes)
            {
                var menuItem = new ToolStripMenuItem();
                menuItem.Name = "testToolStripMenuItem";
                menuItem.Text = Path.GetFileName(file);
                menuItem.Tag = Path.GetFullPath(file);
                menuItem.Click += new System.EventHandler(SceneFileMenuItem_Click);

                mnuAvailableFiles.DropDownItems.Add(menuItem);
            }      
        }        

        public void LoadSceneFromFile(string strSceneFile)
        {
            m_sceneFile = strSceneFile;

            var file = Path.GetFileName(strSceneFile);
            var folder = Path.GetDirectoryName(strSceneFile);
            Directory.SetCurrentDirectory(folder);

            txtSceneFile.Text = File.ReadAllText(file);
        }

        public void LoadSceneFromEditor()
        {
            if (!m_isSceneDefinitionDirty)
                return;
            
            LoadScene(txtSceneFile.Text);
            m_isSceneDefinitionDirty = false;
        }

        public void LoadScene(string strScene)
        {
            this.txtMessages.Text = "Reading scene\r\n";
            
            Stopwatch watch = new Stopwatch();
            watch.Start();

            var loader = new VBRaySceneLoader();

            using (var sceneStream = new MemoryStream(System.Text.Encoding.Default.GetBytes(strScene)))
                m_scene = loader.LoadScene(sceneStream);

            watch.Stop();

            this.txtMessages.Text += string.Format("Loaded: {0}ms\r\n", watch.ElapsedMilliseconds);                
        }

        public void RenderScene()
        {
            this.txtMessages.Clear();

            LoadSceneFromEditor();
            
            this.txtMessages.Text += "Rendering\r\n";
            
            int width = pictureBox1.Width;
            int height = pictureBox1.Height;
            bool blnMultiThreaded = multiThreadedToolStripMenuItem.Checked;
            bool traceShadows = mnuShadows.Checked;
            bool traceReflections = mnuReflections.Checked;
            bool traceRefractions = mnuRefractions.Checked;

            Action<bool,bool,bool,int> UpdateScreenRenderOptions = (shadows, reflections, refractions, renderDepth) =>
                {
                    this.UIThread(() =>
                    {
                        this.mnuShadows.Checked = shadows;
                        this.mnuReflections.Checked = reflections;
                        this.mnuRefractions.Checked = refractions;

                        SetSelectedRenderDepthMenuItem(renderDepth);
                    });
                };            

            var task = new Task<long>(() =>
            {
                Stopwatch watch = new Stopwatch();
                watch.Start();
                PictureBoxBmp bmp = new PictureBoxBmp(pictureBox1);
                
                Render(width, height, bmp, blnMultiThreaded, traceShadows, traceReflections, traceRefractions, UpdateScreenRenderOptions);

                bmp.Render();

                watch.Stop();
                return watch.ElapsedMilliseconds;
            });

            task.ContinueWith((time) =>
            {
                this.UIThread(() =>
                {
                    this.txtMessages.Text += string.Format("Done:{0}ms total\r\n", time.Result);
                });
            });

            task.Start();
        }

        private void SetSelectedRenderDepthMenuItem(int renderDepth)
        {
            foreach (ToolStripMenuItem item in mnuRenderDepth.DropDown.Items)
            {
                item.Checked = (item.Text == renderDepth.ToString());
            }
        }

        private void Render(int width, int height, IBmp bmp, 
            bool blnMultiThreaded, bool traceShadows, bool traceReflections, bool traceRefractions,
            Action<bool, bool, bool, int> UpdateScreenRenderOptions)
        {
            VBRaySceneLoader loader = new VBRaySceneLoader();

            Stopwatch watch = new Stopwatch();

            UpdateScreenRenderOptions(m_scene.TraceShadows, m_scene.TraceReflections, m_scene.TraceRefractions, m_scene.RecursionDepth);
                        
            watch.Start();
            
            var camera = new PinholeCamera(m_scene.EyePosition, 
                m_scene.ViewPointRotation,
                bmp.Size,
                m_scene.FieldOfView);

            IPixelSampler pixelSampler = null;
            if (GetAnitaliasingLevel() > 1)
                pixelSampler = new EdgeDetectionSampler(GetAnitaliasingLevel(), GetRenderAntialiasingSamples(), bmp.Size);
            else
                pixelSampler = new StandardPixelSampler();

            var renderer = new RayTracingRenderer(m_scene, camera, pixelSampler, (uint)m_scene.RecursionDepth, blnMultiThreaded, traceShadows, traceReflections, traceRefractions);
            renderer.RenderScene(bmp);
            
            watch.Stop();
            this.UIThread(() =>
            {
                this.txtMessages.Text += string.Format("Rendered :{0}ms\r\n", watch.ElapsedMilliseconds);
            });            
        }

        private void SceneFileMenuItem_Click(object sender, EventArgs e)
        {
            LoadSceneFromFile(((ToolStripMenuItem)sender).Tag.ToString());
        }

        /// <summary>
        /// function that takes a delegate and times its execution. 
        /// </summary>
        /// <param name="act">function to time</param>
        /// <returns>time in milliseconds.</returns>
        private static long Time(Action act)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            act();
            sw.Stop();
            return sw.ElapsedMilliseconds;
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dlgOpen.InitialDirectory = Path.GetDirectoryName(Application.ExecutablePath);
            if (dlgOpen.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                LoadSceneFromFile(dlgOpen.FileName);
            }
        }

        private void mnuSave_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image == null)
                return;

            dlgSaveBmp.InitialDirectory = Path.GetDirectoryName(Application.ExecutablePath);
            if (dlgSaveBmp.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                SaveCurrentImage(dlgSaveBmp.FileName);
            }
        }

        private void SaveCurrentImage(string strImageName)
        {
            pictureBox1.Image.Save(strImageName);
        }

        private void multiThreadedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            multiThreadedToolStripMenuItem.Checked = !multiThreadedToolStripMenuItem.Checked;
        }

        private void renderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (m_scene == null)
                return;

            RenderScene();
        }

        private void toolStripMenuItem6_Click(object sender, EventArgs e)
        {
            if (m_scene == null)
                return;

            foreach (ToolStripMenuItem item in ((ToolStripMenuItem)sender).GetCurrentParent().Items)
            {
                item.Checked = false;
            }

            ((ToolStripMenuItem)sender).Checked = true;
            m_scene.RecursionDepth = int.Parse(((ToolStripMenuItem)sender).Text);
        }

        private void mnuShadows_Click(object sender, EventArgs e)
        {
            ((ToolStripMenuItem)sender).Checked = !((ToolStripMenuItem)sender).Checked;
        }

        private void mnuReflections_Click(object sender, EventArgs e)
        {
            ((ToolStripMenuItem)sender).Checked = !((ToolStripMenuItem)sender).Checked;
        }

        private void mnuRefractions_Click(object sender, EventArgs e)
        {
            ((ToolStripMenuItem)sender).Checked = !((ToolStripMenuItem)sender).Checked;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            //Go();
        }

        private void btnRender_Click(object sender, EventArgs e)
        {
            RenderScene();
        }

        private void saveSceneToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveSceneToFile();
        }

        private void SaveSceneToFile()
        {
            dlgSaveRay.InitialDirectory = Path.GetDirectoryName(Application.ExecutablePath);
            if (dlgSaveRay.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                SaveSceneToFile(dlgSaveRay.FileName);
                m_sceneFile = dlgSaveRay.FileName;
            }
        }

        private void SaveSceneToFile(string fileName)
        {
            File.WriteAllText(fileName, txtSceneFile.Text);
        }

        private void saveSceneToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            SaveSceneToFile(m_sceneFile);
        }

        private void txtSceneFile_TextChanged(object sender, EventArgs e)
        {
            m_isSceneDefinitionDirty = true;
        }

        private void mnuSuperSampling_Click(object sender, EventArgs e)
        {
            var menu = (ToolStripMenuItem)sender;

            foreach (ToolStripMenuItem item in menu.GetCurrentParent().Items.Cast<ToolStripMenuItem>().Take(4))
                item.Checked = false;

            menu.Checked = true;
        }

        private uint GetAnitaliasingLevel()
        {
            return (from item in mnuSuperSampling.DropDownItems.Cast<ToolStripMenuItem>().Take(4)
                    where item.Checked
                    select uint.Parse(item.Tag.ToString())).First();
        }

        private uint GetRenderDepth()
        {
            return (from item in mnuRenderDepth.DropDownItems.Cast<ToolStripMenuItem>()
                    where item.Checked
                    select uint.Parse(item.Tag.ToString())).First();
        }
        
        private bool GetRenderAntialiasingSamples()
        {
            return renderAntialiasingSamplesToolStripMenuItem.Checked;
        }

        private void renderAntialiasingSamplesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            renderAntialiasingSamplesToolStripMenuItem.Checked = !renderAntialiasingSamplesToolStripMenuItem.Checked;            
        }
    }
}

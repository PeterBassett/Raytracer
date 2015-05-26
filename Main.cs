using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Raytracer.Extensions;
using Raytracer.FileTypes;
using Raytracer.MathTypes;
using Raytracer.Rendering.Cameras;
using Raytracer.Rendering.Core;
using Raytracer.Rendering.PixelSamplers;
using Raytracer.Rendering.Renderers;
using Raytracer.Rendering.RenderingStrategies;
//using System.Threading;
using Raytracer.FileTypes.XMLRayScene;
using System.Collections.Concurrent;
using Raytracer.FileTypes.XMLRayScene.Loaders;
using Raytracer.Rendering.Synchronisation;

namespace Raytracer
{
    public partial class Main : Form
    {
        private Scene m_scene;
        private IRenderer m_renderer;
        private ICamera m_camera;        
        private string m_sceneFile;
        private bool m_isSceneDefinitionDirty = true;
        private CancellationTokenSource _cancellationTokenSource;

        public Main()
        {
            InitializeComponent();
            txtSceneFile.Text = "";
            pixelPosition.Text = "";

            var scenes = Directory.GetFiles(".", "*.ray", SearchOption.AllDirectories).Concat(
                         Directory.GetFiles(".", "*.xml", SearchOption.AllDirectories)).OrderBy(e => e);

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

            UpdateScreenRenderOptions();
        }

        private void UpdateScreenRenderOptions()
        {
            this.UIThread(() =>
            {
                this.mnuShadows.Checked = m_renderer.Settings.TraceShadows;
                this.mnuReflections.Checked = m_renderer.Settings.TraceReflections;
                this.mnuRefractions.Checked = m_renderer.Settings.TraceRefractions;

                SetSelectedRenderDepthMenuItem(m_renderer.Settings.PathDepth);
            });
        }

        public void LoadScene(string strScene)
        {
            this.txtMessages.Text = "Reading scene\r\n";
            
            Stopwatch watch = new Stopwatch();
            watch.Start();

            m_scene = null;

            GC.Collect();

            ISceneLoader loader = new XmlRaySceneLoader();

            SystemComponents components = null;
            using (var sceneStream = new MemoryStream(System.Text.Encoding.Default.GetBytes(strScene)))
                components = loader.LoadScene(sceneStream);

            m_renderer = components.renderer;
            m_scene = components.scene;
            m_camera = components.camera;
            _cancellationTokenSource = components.CancellationTokenSource;

            watch.Stop();

            this.txtMessages.Text += string.Format("Loaded: {0}ms\r\n", watch.ElapsedMilliseconds);                
        }

        public void RenderPixel(int x, int y)
        {
            RenderScene(new Vector2(x, y));
        }

        public void RenderScene(Vector2? renderAt = null)
        {
            btnCancelRendering.Enabled = true;
            btnRender.Enabled = false;

            this.txtMessages.Clear();

            LoadSceneFromEditor();
            
            this.txtMessages.Text += "Rendering\r\n";
            
            int width = renderedImage.Width;
            int height = renderedImage.Height;
            
            var task = new Task<long>(() =>
            {
                Stopwatch watch = new Stopwatch();
                watch.Start();
                var bmp = new PictureBoxBmp(renderedImage);

                Render(width, height, bmp, _cancellationTokenSource.Token, renderAt);

                watch.Stop();
                return watch.ElapsedMilliseconds;
            });

            task.ContinueWith((time) =>
            {
                this.UIThread(() =>
                {
                    this.txtMessages.Text += string.Format("Done:{0}ms total\r\n", time.Result);
                    btnRender.Enabled = true;
                    btnCancelRendering.Enabled = false;
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

        private void Render(int width, int height, IBmp bmp, CancellationToken token, Vector2? renderAt)
        {
            renderingCurrentPercentage = 0;
            renderingCurrentTotal = 0;
            scanLinesCompleted = new ConcurrentDictionary<int, int>();

            Stopwatch watch = new Stopwatch();
            
            watch.Start();

            m_camera.OutputDimensions = bmp.Size;
            _cancellationTokenSource.Reset();

            IPixelSampler pixelSampler = new StandardPixelSampler();            
            IRenderingStrategy renderingStrategy;

            if (GetRenderingStrategy() == "Progressive")
            {
                renderingStrategy = new ProgressiveRenderingStrategy(pixelSampler, 64, GetMultiThreaded(), token);
            }
            else
            {
                switch (GetSampler())
                {
                    case "Jittered":
                        pixelSampler = new JitteredPixelSampler(GetAnitaliasingLevel());
                        break;
                    case "GreyscaleEdgeDetection":
                        pixelSampler = new EdgeDetectionSampler(GetAnitaliasingLevel(), GetRenderAntialiasingSamples());
                        break;
                    case "ComponentEdgeDetection":
                        pixelSampler = new EdgeDetectionPerComponentSampler(GetAnitaliasingLevel(), GetRenderAntialiasingSamples());
                        break;
                    default:
                        throw new ArgumentOutOfRangeException("Sampler");
                }

                renderingStrategy = new BasicRenderingStrategy(pixelSampler, GetMultiThreaded(), token);
            }

            m_renderer.RenderingStrategy.OnCompletedScanLine += RenderingStrategy_OnCompletedScanLine;

            IRenderer renderer = m_renderer;// new RayTracingRenderer(m_scene, m_camera, renderingStrategy, (uint)m_renderer.Settings.PathDepth, blnMultiThreaded, traceShadows, traceReflections, traceRefractions);

            if (renderAt.HasValue)
                renderer.ComputeSample(renderAt.Value);
            else
                renderer.RenderScene(bmp);
            
            watch.Stop();

            m_renderer.RenderingStrategy.OnCompletedScanLine -= RenderingStrategy_OnCompletedScanLine;

            this.UIThread(() =>
            {
                this.txtMessages.Text += string.Format("Rendered :{0}ms\r\n", watch.ElapsedMilliseconds);
            });            
        }

        int renderingCurrentPercentage = 0;
        int renderingCurrentTotal = 0;
        ConcurrentDictionary<int, int> scanLinesCompleted = new ConcurrentDictionary<int, int>();

        private void RenderingStrategy_OnCompletedScanLine(int completed, int total)
        {
            if (total != renderingCurrentTotal)
            {
                renderingCurrentTotal = total;
                scanLinesCompleted.Clear();
            }

            if (!scanLinesCompleted.ContainsKey(completed))
                scanLinesCompleted.AddOrUpdate(completed, completed, (a, b) => completed);

            var percentage = (int)((scanLinesCompleted.Count / (double)total) * 100);

            if (renderingCurrentPercentage != percentage)
            {
                renderingCurrentPercentage = percentage;

                this.UIThread(() =>
                {
                    lblPercent.Text = string.Format("Rendered {0}%\r\n", renderingCurrentPercentage);

                    Application.DoEvents();
                });
            }
        }

        private bool GetMultiThreaded()
        {
            return multiThreadedToolStripMenuItem.Checked;
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
            if (renderedImage.Image == null)
                return;

            dlgSaveBmp.InitialDirectory = Path.GetDirectoryName(Application.ExecutablePath);
            if (dlgSaveBmp.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                SaveCurrentImage(dlgSaveBmp.FileName);
            }
        }

        private void SaveCurrentImage(string strImageName)
        {
            renderedImage.Image.Save(strImageName);
        }

        private void multiThreadedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            multiThreadedToolStripMenuItem.Checked = !multiThreadedToolStripMenuItem.Checked;
            UpdateSettings();
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
            m_renderer.Settings.PathDepth = int.Parse(((ToolStripMenuItem)sender).Text);

            UpdateSettings();
        }

        private void mnuShadows_Click(object sender, EventArgs e)
        {
            ((ToolStripMenuItem)sender).Checked = !((ToolStripMenuItem)sender).Checked;
            UpdateSettings();
        }

        private void UpdateSettings()
        {
            if (m_renderer != null && m_renderer.Settings != null)
            {
                m_renderer.Settings.MultiThreaded = multiThreadedToolStripMenuItem.Checked;
                m_renderer.Settings.TraceShadows = mnuShadows.Checked;
                m_renderer.Settings.TraceReflections = mnuReflections.Checked;
                m_renderer.Settings.TraceRefractions = mnuRefractions.Checked;
            }
        }


        private void mnuReflections_Click(object sender, EventArgs e)
        {
            ((ToolStripMenuItem)sender).Checked = !((ToolStripMenuItem)sender).Checked;
            UpdateSettings();
        }

        private void mnuRefractions_Click(object sender, EventArgs e)
        {
            ((ToolStripMenuItem)sender).Checked = !((ToolStripMenuItem)sender).Checked;
            UpdateSettings();
        }

        private void renderedImage_Click(object sender, EventArgs e)
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

        private void btnCancelRendering_Click(object sender, EventArgs e)
        {
            if (_cancellationTokenSource != null)
                _cancellationTokenSource.Cancel();
            
            btnCancelRendering.Enabled = false;
            btnRender.Enabled = true;
        }

        private void progressiveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var menu = (ToolStripMenuItem)sender;

            foreach (ToolStripMenuItem item in menu.GetCurrentParent().Items.Cast<ToolStripMenuItem>())
                item.Checked = false;

            menu.Checked = true;
        }

        private string GetRenderingStrategy()
        {
            return (from item in mnuRenderingMode.DropDownItems.Cast<ToolStripMenuItem>()
                    where item.Checked
                    select item.Text).First();
        }

        private System.Drawing.Point _mouseCoordinatesOverImage;
        private void renderedImage_MouseMove(object sender, MouseEventArgs e)
        {
            _mouseCoordinatesOverImage = e.Location;
            pixelPosition.Text = string.Format("{0}:{1}", e.X, e.Y);
        }

        private void renderedImage_MouseLeave(object sender, EventArgs e)
        {
            pixelPosition.Text = "";
        }

        private void renderedImage_DoubleClick(object sender, EventArgs e)
        {
            var coordinateDisplay = new PixelCoordinates();
            coordinateDisplay.OnRenderRequested += coordinateDisplay_OnRenderRequested;
            coordinateDisplay.Display(_mouseCoordinatesOverImage.X, _mouseCoordinatesOverImage.Y);
        }

        void coordinateDisplay_OnRenderRequested(object sender, int x, int y)
        {
            RenderPixel(x, renderedImage.Height - y);
        }

        private void jitteredSamplerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var menu = (ToolStripMenuItem)sender;

            foreach (ToolStripMenuItem item in menu.GetCurrentParent().Items.Cast<ToolStripMenuItem>())
                item.Checked = false;

            menu.Checked = true;
        }

        private string GetSampler()
        {
            return (from item in jitteredSamplerToolStripMenuItem.GetCurrentParent().Items.Cast<ToolStripMenuItem>()
                    where item.Checked
                    select item.Tag.ToString()).First();
        }
    }
}

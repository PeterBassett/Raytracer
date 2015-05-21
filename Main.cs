using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Raytracer.Extensions;
using Raytracer.MathTypes;
using Raytracer.Rendering.Cameras;
using Raytracer.Rendering.Core;
using Raytracer.FileTypes.VBRayScene;
using Raytracer.Rendering.PixelSamplers;
using Raytracer.Rendering.Renderers;
using Raytracer.Rendering.RenderingStrategies;
using System.Threading;
using System.Text;
using System.Collections.Generic;

namespace Raytracer
{
    public partial class Main : Form
    {
        private Scene m_scene = null;
        private string m_sceneFile;
        private bool m_isSceneDefinitionDirty = true;
        private CancellationTokenSource _cancellationTokenSource;

        public Main()
        {
           // generatetorusgrid();


            InitializeComponent();
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

        private void generatetorusgrid()
        {
            var colours = GenerateColours();
            var colourNames = colours.Keys.ToArray();
            var builder = new StringBuilder();

            int maxX = 10;
            int maxY = 10;

            for (int x = -maxX/2; x < maxX/2; x++)
            {
                for (int y = 0; y < maxY; y++)
                {
                    AddDisc(x, y, 0, 0, builder, colourNames);
                }
            }

            var file = File.ReadAllText(@"Data/Small/DiscGrid.ray");

            var coloursTogether = string.Join("\r\n\r\n", colours.Values);
            file = file.Replace("%MATERIALS%", coloursTogether);
            file = file.Replace("%PRIMITIVES%", builder.ToString());

            File.WriteAllText(@"Data/Small/DiscGridGenerated.ray", file);
        }

        private void AddDisc(float x, float y, float xRotation, float yRotation, StringBuilder builder, string[] colors)
        {
            var colour = colors[rng.Next(colors.Length)];
            /*
             * Torus( 1,5, 
                  0, 0, 0,
                  0, 0, 0,
                  "red"
                )*/

            var outerRadius = 3.0 + 2.0*rng.NextDouble();
            var innerRadius =  rng.NextDouble();
            innerRadius = Math.Max(innerRadius, 0);
            builder.AppendFormat("Disc({0},{1},\r\n{2},{3},{4},\r\n{5},{6},{7},\r\n\"{8}\")\r\n\r\n",
                outerRadius, innerRadius, x * 12, y * 12, 0, 0, 0, 0, colour);
        }

        private Dictionary<string, string> GenerateColours()
        {
            var colours = new Dictionary<string, string>();

            Random rnd = new Random();
            var cols = (from prop in typeof(Color).GetProperties()
                        where prop.PropertyType == typeof(Color)
                        select (Color)prop.GetValue(null, null)).ToList();
            
            foreach (var colour in cols)
            {
                if (colour.IsNamedColor)
                {
                    var col = string.Format("{0}, {1}, {2}", colour.R / 255.0, colour.G / 255.0, colour.B / 255.0);

                    var reflected = 0;//rnd.NextDouble().ToString("F2");
                    var refracted = 0;//rnd.NextDouble().ToString("F2");
                    colours.Add(colour.Name, string.Format("ColourMaterial(\"{0}\", {1}, {1}, 20, 0.35, {2},{2},{2}, 1.33, {3},{3},{3} )", colour.Name, col, reflected, refracted));
                }
            }

            return colours;
        }

        private Random rng = new Random();
        private void AddTorus(float x, float y, float xRotation, float yRotation, StringBuilder builder, string [] colors)
        {
            var colour = colors[rng.Next(colors.Length)];
            /*
             * Torus( 1,5, 
                  0, 0, 0,
                  0, 0, 0,
                  "red"
                )*/

            builder.AppendFormat("Torus({0},{1},\r\n{2},{3},{4},\r\n{5},{6},{7},\r\n\"{8}\")\r\n\r\n",
                1, 5, x * 13, y * 13, 0, xRotation, yRotation, 0, colour);
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
                this.mnuShadows.Checked = m_scene.TraceShadows;
                this.mnuReflections.Checked = m_scene.TraceReflections;
                this.mnuRefractions.Checked = m_scene.TraceRefractions;

                SetSelectedRenderDepthMenuItem(m_scene.RecursionDepth);
            });
        }

        public void LoadScene(string strScene)
        {
            this.txtMessages.Text = "Reading scene\r\n";
            
            Stopwatch watch = new Stopwatch();
            watch.Start();

            m_scene = null;

            GC.Collect();

            var loader = new RaySceneLoader();

            using (var sceneStream = new MemoryStream(System.Text.Encoding.Default.GetBytes(strScene)))
                m_scene = loader.LoadScene(sceneStream);

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
            bool blnMultiThreaded = multiThreadedToolStripMenuItem.Checked;
            _cancellationTokenSource = new CancellationTokenSource();
            bool traceShadows = mnuShadows.Checked;
            bool traceReflections = mnuReflections.Checked;
            bool traceRefractions = mnuRefractions.Checked;
            
            var task = new Task<long>(() =>
            {
                Stopwatch watch = new Stopwatch();
                watch.Start();
                var bmp = new PictureBoxBmp(renderedImage);

                Render(width, height, bmp, blnMultiThreaded, traceShadows, traceReflections, traceRefractions, _cancellationTokenSource.Token, renderAt);

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

        private void Render(int width, int height, IBmp bmp, 
            bool blnMultiThreaded, bool traceShadows, bool traceReflections, bool traceRefractions, CancellationToken token, Vector2? renderAt)
        {
            var loader = new RaySceneLoader();

            Stopwatch watch = new Stopwatch();
            
            watch.Start();
            
            var camera = new PinholeCamera(m_scene.EyePosition, 
                m_scene.ViewPointRotation,
                bmp.Size,
                m_scene.FieldOfView);

            IPixelSampler pixelSampler = new StandardPixelSampler();            
            IRenderingStrategy renderingStrategy;

            if (GetRenderingStrategy() == "Progressive")
            {
                renderingStrategy = new ProgressiveRenderingStrategy(pixelSampler, 64, blnMultiThreaded, token);
            }
            else
            {
                switch (GetSampler())
                {
                    case "Jittered":
                        pixelSampler = new JitteredPixelSampler(GetAnitaliasingLevel());
                        break;
                    case "GreyscaleEdgeDetection":
                        pixelSampler = new EdgeDetectionSampler(GetAnitaliasingLevel(), GetRenderAntialiasingSamples(), bmp.Size);
                        break;
                    case "ComponentEdgeDetection":
                        pixelSampler = new EdgeDetectionPerComponentSampler(GetAnitaliasingLevel(), GetRenderAntialiasingSamples(), bmp.Size);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException("Sampler");
                }

                renderingStrategy = new BasicRenderingStrategy(pixelSampler, blnMultiThreaded, token);
            }

            int currentPercentage = 0;
            var scanLinesCompleted = new System.Collections.Concurrent.ConcurrentDictionary<int, int>();
            var currentTotal = 0;
            renderingStrategy.OnCompletedScanLine += (completed, total) =>
            {
                if (total != currentTotal)
                {
                    currentTotal = total;
                    scanLinesCompleted.Clear();
                }

                if (!scanLinesCompleted.ContainsKey(completed))
                    scanLinesCompleted.AddOrUpdate(completed, completed, (a, b) => completed);

                var percentage = (int)((scanLinesCompleted.Count / (double)total) * 100);

                if (currentPercentage != percentage)
                {
                    currentPercentage = percentage;

                    this.UIThread(() =>
                    {             
                        lblPercent.Text = string.Format("Rendered {0}%\r\n", currentPercentage);

                        Application.DoEvents();
                    });
                }
            };

            IRenderer renderer = new RayTracingRenderer(m_scene, camera, renderingStrategy, (uint)m_scene.RecursionDepth, blnMultiThreaded, traceShadows, traceReflections, traceRefractions);

            if (renderAt.HasValue)
                renderer.ComputeSample(renderAt.Value);
            else
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

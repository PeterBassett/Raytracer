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
using Raytracer.FileTypes.XMLRayScene;
using System.Collections.Concurrent;
using Raytracer.FileTypes.XMLRayScene.Loaders;
using Raytracer.Rendering.Synchronisation;
using Raytracer.Rendering.Distributions;

namespace Raytracer
{
    public partial class Main : Form
    {
        private Scene _scene;
        private IRenderer _renderer;
        private ICamera _camera;
        private CancellationTokenSource _cancellationTokenSource;
        private IRenderer _sceneDefinedRenderer;

        private string _sceneFile;
        private bool _isSceneDefinitionDirty = true;        

        public Main()
        {
            InitializeComponent();

            txtSceneFile.Text = "";
            pixelPosition.Text = "";

            var scenes = Directory.GetFiles(".", "*.xml", SearchOption.AllDirectories);

            foreach (var file in scenes)
            {
                var menuItem = new ToolStripMenuItem
                {
                    Name = "testToolStripMenuItem",
                    Text = Path.GetFileName(file),
                    Tag = Path.GetFullPath(file)
                };
                menuItem.Click += SceneFileMenuItem_OnClick;

                mnuAvailableFiles.DropDownItems.Add(menuItem);
            }      
        }

        private void LoadSceneFromFile(string strSceneFile)
        {
            _sceneFile = strSceneFile;

            var file = Path.GetFileName(strSceneFile);
            var folder = Path.GetDirectoryName(strSceneFile);
            Directory.SetCurrentDirectory(folder);

            txtSceneFile.Text = File.ReadAllText(file);
        }

        private void LoadSceneFromEditor()
        {
            if (!_isSceneDefinitionDirty)
                return;
            
            LoadScene(txtSceneFile.Text);
            _isSceneDefinitionDirty = false;
        }

        private void LoadScene(string strScene)
        {
            txtMessages.Text = "Reading scene\r\n";
            
            var watch = new Stopwatch();
            watch.Start();

            var existingScene = _scene;
            _scene = null;

            GC.Collect();

            ISceneLoader loader = new XmlRaySceneLoader();

            SystemComponents systemComponents;

            try
            {
                using (var sceneStream = new MemoryStream(System.Text.Encoding.Default.GetBytes(strScene)))
                    systemComponents = loader.LoadScene(sceneStream, existingScene);

                _renderer = systemComponents.Renderer;
                _scene = systemComponents.Scene;
                _camera = systemComponents.Camera;
                _cancellationTokenSource = systemComponents.CancellationTokenSource;
                _sceneDefinedRenderer = _renderer;

                txtMessages.Text += string.Format("Loaded: {0}ms\r\n", watch.ElapsedMilliseconds); 
            }
            catch (Exception ex)
            {
                txtMessages.Text += string.Format("Error Loading : {0}\r\n", ex.Message); 
            }
    
            watch.Stop();
        }

        private void RenderPixel(int x, int y)
        {
            RenderScene(new Vector2(x, y));
        }

        private void RenderScene(Vector2? renderAt = null)
        {
            btnCancelRendering.Enabled = true;
            btnRender.Enabled = false;

            txtMessages.Clear();

            LoadSceneFromEditor();

            OverrideRenderer();
            
            txtMessages.Text += "Rendering\r\n";
            
            var task = new Task<long>(() =>
            {
                var watch = new Stopwatch();
                watch.Start();

                var bmp = new PictureBoxBmp(renderedImage);

                Render(bmp, _cancellationTokenSource.Token, renderAt);

                watch.Stop();
                return watch.ElapsedMilliseconds;
            });

            task.ContinueWith(time => this.UIThread(() =>
            {
                txtMessages.Text += string.Format("Done:{0}ms total\r\n", time.Result);
                btnRender.Enabled = true;
                btnCancelRendering.Enabled = false;
            }));

            task.Start();
        }

        private void OverrideRenderer()
        {
            if (!overrideSceneDefaults.Checked)
                return;

            var multiThreaded = GetMultiThreaded();
            var antiAliasingLevel = GetAnitaliasingLevel();
            var antiAliasingSamples = GetRenderAntialiasingSamples();
            
            var distribution = GetSelectedDistributionSource();

            var renderingStrategy = GetSelectedRenderingStrategy(multiThreaded, antiAliasingLevel, antiAliasingSamples);

            var camera = _renderer.Camera;
            
            _renderer = new Raytracer.Rendering.Renderers.RayTracingRenderer();
            _renderer.Camera = _camera;
            _renderer.Scene = _scene;
            _renderer.Settings = new RenderSettings()
            {
                MultiThreaded = multiThreaded,
                PathDepth = (int)GetRenderDepth(),
                TraceReflections = mnuReflections.Checked,
                TraceRefractions = mnuRefractions.Checked,
                TraceShadows = mnuShadows.Checked
            };
            _renderer.RenderingStrategy = renderingStrategy;
            _renderer.Distribution = distribution; 
        }
        
        private Distribution GetSelectedDistributionSource()
        {
            var source = GetDistributionSource();

            switch (source)
            {
                case "Random":
                    return new RandomDistribution();
                case "Stratified":
                    return new StratifiedDistribution();
                default:
                    throw new ArgumentOutOfRangeException("Distribution");
            }
        }

        private string GetDistributionSource()
        {
            return GetSelectedMenuItemText(mnuDistributionSource);
        }

        private IRenderingStrategy GetSelectedRenderingStrategy(bool multiThreaded, uint antiAliasingLevel, bool antiAliasingSamples)
        {
            IRenderingStrategy renderingStrategy = null;
            var selectedRenderingStrategy = GetRenderingStrategy();
            if (selectedRenderingStrategy == "Progressive")
            {
                renderingStrategy = new ProgressiveRenderingStrategy(new StandardPixelSampler(),
                                                                     64,
                                                                     multiThreaded,
                                                                     _cancellationTokenSource.Token);
            }
            else if (selectedRenderingStrategy == "Thread Per Core" ||
                     selectedRenderingStrategy == "Grid")
            {
                IPixelSampler pixelSampler;

                switch (GetSampler())
                {
                    case "Jittered":
                        pixelSampler = new JitteredPixelSampler(antiAliasingLevel);
                        break;
                    case "GreyscaleEdgeDetection":
                        pixelSampler = new EdgeDetectionSampler(antiAliasingLevel, antiAliasingSamples);
                        break;
                    case "ComponentEdgeDetection":
                        pixelSampler = new EdgeDetectionPerComponentSampler(antiAliasingLevel, antiAliasingSamples);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException("Sampler");
                }

                if (selectedRenderingStrategy == "Thread Per Core")
                {
                    renderingStrategy = new BasicRenderingStrategy(pixelSampler,
                                                                   multiThreaded,
                                                                   _cancellationTokenSource.Token);
                }
                else if (selectedRenderingStrategy == "Grid")
                {
                    renderingStrategy = new GridRenderingStrategy(pixelSampler,
                                                                   multiThreaded,
                                                                   _cancellationTokenSource.Token);
                }
            }
            else
                throw new ArgumentOutOfRangeException();

            return renderingStrategy;
        }

        private void SetSelectedRenderDepthMenuItem(int renderDepth)
        {
            foreach (ToolStripMenuItem item in mnuRenderDepth.DropDown.Items)
            {
                item.Checked = (item.Text == renderDepth.ToString());
            }
        }

        private void Render(IBmp bmp, CancellationToken token, Vector2? renderAt)
        {
            _renderingStartedAt = DateTime.Now;
            _renderingCurrentPercentage = 0;
            _renderingPercentage = 0;

            var watch = new Stopwatch();
            
            watch.Start();

            _camera.OutputDimensions = bmp.Size;
            _cancellationTokenSource.Reset();

            _renderer.RenderingStrategy.OnRenderingStarted += RenderingStrategy_OnRenderingStarted;
            _renderer.RenderingStrategy.OnCompletedPercentageDelta += RenderingStrategy_OnCompletedAdditionalPercentage;
            _renderer.RenderingStrategy.OnRenderingComplete += RenderingStrategy_OnRenderingComplete;
            
            if (renderAt.HasValue)
                _renderer.ComputeSample(renderAt.Value);
            else
                _renderer.RenderScene(bmp);
            
            watch.Stop();

            _renderer.RenderingStrategy.OnRenderingStarted -= RenderingStrategy_OnRenderingStarted;
            _renderer.RenderingStrategy.OnCompletedPercentageDelta -= RenderingStrategy_OnCompletedAdditionalPercentage;
            _renderer.RenderingStrategy.OnRenderingComplete -= RenderingStrategy_OnRenderingComplete;

            this.UIThread(() =>
            {
                txtMessages.Text += string.Format("Rendered :{0}ms\r\n", watch.ElapsedMilliseconds);
            });            
        }

        DateTime _renderingStartedAt;
        int _renderingCurrentPercentage;
        double _renderingPercentage;
        
        private void SetRenderingCompletionPercentage(int percentage)
        {
            if (_renderingCurrentPercentage != percentage)
            {
                _renderingCurrentPercentage = percentage;

                this.UIThread(() =>
                {
                    lblPercent.Text = string.Format("Rendered {0}%\r\n{1}", _renderingCurrentPercentage, FormatElapsedTime(DateTime.Now - _renderingStartedAt));

                    Application.DoEvents();
                });
            }
        }

        public static double Add(ref double location1, double value)
        {
            double newCurrentValue = 0;
            while (true)
            {
                double currentValue = newCurrentValue;
                double newValue = currentValue + value;
                newCurrentValue = System.Threading.Interlocked.CompareExchange(ref location1, newValue, currentValue);
                if (newCurrentValue == currentValue)
                    return newValue;
            }
        }

        void RenderingStrategy_OnRenderingStarted()
        {
            _renderingCurrentPercentage = 0;
            _renderingPercentage = 0;
        }

        private void RenderingStrategy_OnCompletedAdditionalPercentage(double percentageDelta)
        {
            var percentage = (int)Add(ref _renderingPercentage, percentageDelta);
            SetRenderingCompletionPercentage(percentage);
        }

        void RenderingStrategy_OnRenderingComplete()
        {
            SetRenderingCompletionPercentage(100);
        }

        private string FormatElapsedTime(TimeSpan span)
        {
            return string.Format("{0:00}:{1:00}:{2:00}.{3}", (int)span.TotalHours, span.Minutes, span.Seconds, span.Milliseconds);
        }

        private bool GetMultiThreaded()
        {
            return multiThreadedToolStripMenuItem.Checked;
        }

        private void SceneFileMenuItem_OnClick(object sender, EventArgs e)
        {
            LoadSceneFromFile(((ToolStripMenuItem)sender).Tag.ToString());
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dlgOpen.InitialDirectory = Path.GetDirectoryName(Application.ExecutablePath);
            if (dlgOpen.ShowDialog() == DialogResult.OK)
            {
                LoadSceneFromFile(dlgOpen.FileName);
            }
        }

        private void mnuSave_Click(object sender, EventArgs e)
        {
            if (renderedImage.Image == null)
                return;

            dlgSaveBmp.InitialDirectory = Path.GetDirectoryName(Application.ExecutablePath);
            if (dlgSaveBmp.ShowDialog() == DialogResult.OK)
            {
                SaveCurrentImage(dlgSaveBmp.FileName);
            }
        }

        private void SaveCurrentImage(string strImageName)
        {
            renderedImage.Image.Save(strImageName);
        }

        private void renderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_scene == null)
                return;

            RenderScene();
        }

        private void mnuGeneralSettings_Click(object sender, EventArgs e)
        {
            ((ToolStripMenuItem)sender).Checked = !((ToolStripMenuItem)sender).Checked;
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
                _sceneFile = dlgSaveRay.FileName;
            }
        }

        private void SaveSceneToFile(string fileName)
        {
            File.WriteAllText(fileName, txtSceneFile.Text);
        }

        private void saveSceneToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            SaveSceneToFile(_sceneFile);
        }

        private void txtSceneFile_TextChanged(object sender, EventArgs e)
        {
            _isSceneDefinitionDirty = true;
        }

        private uint GetAnitaliasingLevel()
        {
            return (from item in mnuSuperSampling.DropDownItems.Cast<ToolStripMenuItem>().Take(4)
                    where item.Checked
                    select uint.Parse(item.Tag.ToString())).First();
        }

        private int GetRenderDepth()
        {
            return int.Parse(GetSelectedMenuItemText(mnuRenderDepth));
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

        private string GetRenderingStrategy()
        {
            return GetSelectedMenuItemText(mnuRenderingMode);
        }

        private string GetSelectedMenuItemText(ToolStripMenuItem menuItem)
        {            
            return (from item in menuItem.DropDownItems.Cast<ToolStripMenuItem>()
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

        private string GetSampler()
        {
            return (from item in jitteredSamplerToolStripMenuItem.GetCurrentParent().Items.Cast<ToolStripMenuItem>()
                    where item.Checked
                    select item.Tag.ToString()).First();
        }

        private void CheckSelectedToolMenuItem(object sender, EventArgs e)
        {
            var menu = (ToolStripMenuItem)sender;

            foreach (ToolStripMenuItem item in menu.GetCurrentParent().Items.Cast<ToolStripMenuItem>())
                item.Checked = false;

            menu.Checked = true;
        }

        void sceneDefaults_Click(object sender, EventArgs e)
        {
            var menuItems = new[] { useSceneDefaults, overrideSceneDefaults };

            foreach (var item in menuItems)
                item.Checked = item.Equals(sender);

            if (useSceneDefaults.Equals(sender))
                _renderer = _sceneDefinedRenderer;
        }
    }
}

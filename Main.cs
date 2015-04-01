using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Raytracer.Rendering.FileTypes.VBRayScene;
using Raytracer.Rendering.FileTypes;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.IO;
using System.Threading.Tasks;
using Raytracer.Extensions;
using Raytracer.Rendering;
using Raytracer.Rendering;
using Raytracer.Rendering.Primitives;
using Raytracer.Rendering.Materials;
using Raytracer.MathTypes;

namespace Raytracer
{
    using Vector = Vector3;
using Raytracer.Rendering.Antialiasing;
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

            //Go();

            /*
            List<long> elapsedTimes = new List<long>();
            Bmp bmp = new Bmp(500, 500);
            for (int i = 0; i < 10; i++)
            {            
                Stopwatch watch = new Stopwatch();
                watch.Start();
                Render("NoisyGlassBall.ray", bmp.Width, bmp.Height, bmp);
                watch.Stop();
                Console.WriteLine(watch.ElapsedMilliseconds);
                elapsedTimes.Add(watch.ElapsedMilliseconds);
            }
            Console.WriteLine("Avg {0}ms", elapsedTimes.Average());

            Application.Exit();*/

            //this.WindowState = FormWindowState.Maximized;            
        }

        PictureBoxBmp bmpCache = null;

        private void Go()
        {
            bmpCache = new PictureBoxBmp(pictureBox1);
            var strScene = "RedBallOnReflectiveCheck.ray";
            VBRaySceneLoader loader = new VBRaySceneLoader();

            using (var stream = File.OpenRead(strScene))
                m_scene = loader.LoadScene(stream);            

            m_scene.MultiThreaded = true;
            m_scene.Width = pictureBox1.Width;
            m_scene.Height = pictureBox1.Height;

            var centerPoint = m_scene.Primitives.First().Pos;
            var distance = m_scene.EyePosition.Z;
            double angle = 0;

            Stopwatch watch = new Stopwatch();
            watch.Start();
            int frame = 0;
            while (true)
            {
                m_scene.TraceScene(bmpCache);
                bmpCache.Render();
                Vector dir = m_scene.ViewPointRotation;
                m_scene.ViewPointRotation = new Vector(0,- angle, 0);

                double rads = (angle - 90) * (Math.PI / 180.0);
                //m_scene.Pos = m_scene.Pos + (m_scene.Dir * 0.1);
                m_scene.EyePosition = centerPoint - new Vector(
                    distance * Math.Cos(rads),
                    0,
                    distance * Math.Sin(rads)); 

                angle += 0.3;

                if (angle > 360)
                    angle = 0;

                frame++;
                if (frame % 100 == 0)
                {
                    watch.Stop();

                    var ms = (double)watch.ElapsedMilliseconds;

                    txtMessages.Text = Math.Round((1000.0 / (ms / frame))).ToString() + " fps";

                    frame = 0;

                    watch.Reset();
                    watch.Start();
                }

                Application.DoEvents();
            }
        }

        private void BuildAndRenderVistaScene()
        {
            Scene scene = new Scene();

            scene.Width = pictureBox1.Width;
            scene.Height = pictureBox1.Height;
            scene.FieldOfView = 90;

            scene.EyePosition = new Vector(50, 200, -1500);
            //scene.Dir = new MathTypes.Vector(0, -0.042612f, -1);
            scene.ViewPointRotation = new Vector(0,0, 0);

            scene.RecursionDepth = 5;
            scene.TraceReflections = true;
            scene.TraceRefractions = false;
            scene.TraceShadows = true;

            Vector Cen = new Vector(50, -20, -860);

            scene.AddLight(new Light()
            {                                
                Ambient = new Colour(0.1f)
            });

            scene.AddLight(new Light()
                {
                    Pos = Cen + new Vector(0, 100, 80),
                    Diffuse = new Colour(1, 0.4f, 0.1f),// * 5e-1,
                    Ambient = new Colour(0)
                });
            scene.AddLight(new Light()
            {
                Pos = Cen + new Vector(0, 100, 80),
                Diffuse = new Colour(1, 0.4f, 0.1f),// * 5e-1,
                Ambient = new Colour(0)
            });
            scene.AddLight(new Light()
            {
                Pos = Cen + new Vector(0, 100, 80),
                Diffuse = new Colour(1, 0.4f, 0.1f),// * 5e-1,
                Ambient = new Colour(0)
            });
            scene.AddLight(new Light()
            {
                Pos = Cen + new Vector(0, 100, 80),
                Diffuse = new Colour(1, 0.4f, 0.1f),// * 5e-1,
                Ambient = new Colour(0)
            });

            scene.AddLight(new Light()
            {
                Pos = Cen + new Vector(0, 600, 600),
                Diffuse = new Colour(1, 0.4f, 0.1f),// * 5e-1,
            });

            scene.AddLight(new Light()
            {
                Pos = Cen + new Vector(0, 600, 600),
                Diffuse = new Colour(1, 0.4f, 0.1f),// * 5e-1,
            });

            scene.AddLight(new Light()
            {
                Pos = Cen + new Vector(0, 600, 600),
                Diffuse = new Colour(1, 0.4f, 0.1f),// * 5e-1,
            });
            scene.AddLight(new Light()
            {
                Pos = Cen + new Vector(0, 600, 600),
                Diffuse = new Colour(1, 0.4f, 0.1f),// * 5e-1,
            });
            scene.AddLight(new Light()
            {
                Pos = Cen + new Vector(0, 600, 600),
                Diffuse = new Colour(1, 0.4f, 0.1f),// * 5e-1,
            });

            scene.AddLight(new Light()
            {
                Pos = Cen + new Vector(0, 600, 600),
                Diffuse = new Colour(1, 0.4f, 0.1f),// * 5e-1,
            });

            scene.AddLight(new Light()
            {
                Pos = Cen + new Vector(0, 600, 600),
                Diffuse = new Colour(1, 0.4f, 0.1f),// * 5e-1,
            });

            scene.AddLight(new Light()
            {
                Pos = Cen + new Vector(0, 600, 600),
                Diffuse = new Colour(1, 0.4f, 0.1f),// * 5e-1,
            });

            scene.AddLight(new Light()
            {
                Pos = Cen + new Vector(0, 600, 600),
                Diffuse = new Colour(1, 0.4f, 0.1f),// * 5e-1,
            });

            scene.AddLight(new Light()
            {
                Pos = Cen + new Vector(0, 600, 600),
                Diffuse = new Colour(1, 0.4f, 0.1f),// * 5e-1,
            });

            scene.AddLight(new Light()
            {
                Pos = Cen + new Vector(0, 600, 600),
                Diffuse = new Colour(1, 0.4f, 0.1f),// * 5e-1,
            });

            scene.AddLight(new Light()
            {
                Pos = Cen + new Vector(0, 600, 600),
                Diffuse = new Colour(1, 0.4f, 0.1f),// * 5e-1,
            });

            scene.AddLight(new Light()
            {
                Pos = Cen + new Vector(0, 600, 600),
                Diffuse = new Colour(1, 0.4f, 0.1f),// * 5e-1,
            });

            scene.AddLight(new Light()
            {
                Pos = Cen + new Vector(0, 600, 600),
                Diffuse = new Colour(1, 0.4f, 0.1f),// * 5e-1,
            });

            scene.AddLight(new Light()
            {
                Pos = Cen + new Vector(0, 600, 600),
                Diffuse = new Colour(1, 0.4f, 0.1f),// * 5e-1,
            });

            scene.AddMaterial(new Material()
            {
                Emissive = new Colour(0.631f, 0.753f, 1.00f) * 3e-1f,
                Diffuse = new Colour(1, 1, 1) * .5f,
                Ambient = new Colour(1),
                Specular = new Colour(0),
                Specularity = 0
            }, "Sky");

            scene.AddMaterial(new Material()
            {
                Diffuse = new Colour(1, 1, 1) * .3f,
                Ambient = new Colour(1),
                Specular = new Colour(0),
                Specularity = 0
            }, "mnt");

            scene.AddMaterial(new Material()
            {
                Diffuse = new Colour(1, 1, 1) * .8f,
                Ambient = new Colour(1),
                Specular = new Colour(0),
                Specularity = 0
            }, "snow");

            scene.AddMaterial(new Material()
            {
                Diffuse = new Colour(1, 1, 1) * .1f,
                Ambient = new Colour(1),
                Specular = new Colour(0),
                Specularity = 0
            }, "mnt base");

            scene.AddMaterial(new Material()
            {
                Diffuse = new Colour(.2f, .2f, 1),
                Reflective = new Colour(0.6f),
                Ambient = new Colour(1),
                Specular = new Colour(0),
                Specularity = 0
            }, "water");

            scene.AddMaterial(new Material()
            {
                Diffuse = new Colour(0, .3f, 0),
                Ambient = new Colour(1),
                Specular = new Colour(0),
                Specularity = 0
            }, "grass");

            scene.AddMaterial(new Material()
            {
                Diffuse = new Colour(1, 1, 1) * .996f,
                Ambient = new Colour(1),
                Specular = new Colour(0),
                Specularity = 0
            }, "ball");

            scene.AddMaterial(new Material()
            {
                Diffuse = new Colour(1, 1, 1) * .8f,
                Ambient = new Colour(1),
                Specular = new Colour(0),
                Specularity = 0
            }, "clouds");

            // ======================================================================
            // vista
            // ======================================================================

            Sphere[] spheres = {

              new Sphere()
                {
                    Radius = 1e4f,  
                    Pos = Cen+new Vector(), 
                    Material = scene.FindMaterial("Sky"),
                },

              new Sphere()
                {
                    Radius = 150,  
                    Pos = Cen+new Vector(-350,0, -100), 
                    Material = scene.FindMaterial("mnt"),
                },
              
                new Sphere()
                {
                    Radius = 200,  
                    Pos = Cen+new Vector(-210,0,-100), 
                    Material = scene.FindMaterial("mnt"),
                },
                new Sphere()
                {
                    Radius = 145,  
                    Pos = Cen+new Vector(-210,85,-100), 
                    Material = scene.FindMaterial("snow"),
                },
              new Sphere()
                {
                    Radius = 150,  
                    Pos = Cen+new Vector(-50,0,-100), 
                    Material = scene.FindMaterial("mnt"),
                },
              new Sphere()
                {
                    Radius = 150,  
                    Pos = Cen+new Vector(100,0,-100), 
                    Material = scene.FindMaterial("mnt"),
                },              

              new Sphere()
                {
                    Radius = 125,  
                    Pos = Cen+new Vector(250,0,-100), 
                    Material = scene.FindMaterial("mnt"),
                }, 
              new Sphere()
                {
                    Radius = 150,  
                    Pos = Cen+new Vector(375,0,-100), 
                    Material = scene.FindMaterial("mnt"),
                },     

              new Sphere()
                {
                    Radius = 2500,  
                    Pos = Cen+new Vector(0,-2400,-500), 
                    Material = scene.FindMaterial("mnt base"),
                }, 

              new Sphere()
                {
                    Radius = 8000,  
                    Pos = Cen+new Vector(0,-8000,200), 
                    Material = scene.FindMaterial("water"),
                }, 

              new Sphere()
                {
                    Radius = 8000,  
                    Pos = Cen+new Vector(0,-8000,1100), 
                    Material = scene.FindMaterial("grass"),
                }, 
              
                new Sphere()
                {
                    Radius = 8,  
                    Pos = Cen+new Vector(-75, -5, 850), 
                    Material = scene.FindMaterial("grass"),
                }, 

                new Sphere()
                {
                    Radius = 30,  
                    Pos = Cen+new Vector(0,   23, 825), 
                    Material = scene.FindMaterial("ball"),
                }, 
                            

              new Sphere()
            {
                Radius = 30,  
                Pos = Cen+new Vector(200,280,400),
                Material = scene.FindMaterial("clouds")
            },

              new Sphere()
            {
                Radius = 37,  
                Pos = Cen+new Vector(237,280,400),
                Material = scene.FindMaterial("clouds")
            },

              new Sphere()
            {
                Radius = 28,  
                Pos = Cen+new Vector(267,280,400),
                Material = scene.FindMaterial("clouds")
            },
                          new Sphere()
            {
                Radius = 40,  
                Pos = Cen+new Vector(150,280,1000),
                Material = scene.FindMaterial("clouds")
            },

            new Sphere()
            {
                Radius = 37,  
                Pos = Cen+new Vector(187,280,1000),
                Material = scene.FindMaterial("clouds")
            },
            
            new Sphere()
            {
                Radius = 40,  
                Pos = Cen+new Vector(600,280,1100),
                Material = scene.FindMaterial("clouds")
            },
            new Sphere()
            {
                Radius = 37,  
                Pos = Cen+new Vector(637,280,1100),
                Material = scene.FindMaterial("clouds")
            },
            new Sphere()
            {
                Radius = 37,  
                Pos = Cen+new Vector(-800,280,1400),
                Material = scene.FindMaterial("clouds")
            },
            new Sphere()
            {
                Radius = 37,  
                Pos = Cen+new Vector(0,280,1600),
                Material = scene.FindMaterial("clouds")
            },
            new Sphere()
            {
                Radius = 37,  
                Pos = Cen+new Vector( 537,280,1800),
                Material = scene.FindMaterial("clouds")
            },              
            
              
           

            };

            foreach (var prim in spheres)
            {
                scene.AddObject(prim);
            }
            VBRaySceneLoader loader = new VBRaySceneLoader();
            using (var sw = new StreamWriter("Vista.ray"))
                loader.SaveScene(sw, scene);

            PictureBoxBmp bmp = new PictureBoxBmp(pictureBox1);
            scene.TraceScene(bmp);
            bmp.Render();

            pictureBox1.Image.Save("Vista.bmp", ImageFormat.Bmp);
        }


        private void BuildAndRenderBallGridScene()
        {
            Scene scene = new Scene();

            scene.Width = pictureBox1.Width;
            scene.Height = pictureBox1.Height;
            scene.FieldOfView = 90;
            scene.EyePosition = new Vector(-4.5f, 7.3f, -9);
            scene.ViewPointRotation = new Vector(45, 30, 0);

            scene.RecursionDepth = 5;
            scene.TraceReflections = true;
            scene.TraceRefractions = false;
            scene.TraceShadows = true;

            scene.AddLight(new Light()
            {
                Pos = new Vector(20, 20, 20),
                Diffuse = new Colour(50 / 255f)
            });

            scene.AddLight(new Light()
            {
                Pos = new Vector(-20, 20, -20),
                Diffuse = new Colour(50 / 255f)
            });

            scene.AddLight(new Light()
            {   
                Pos = new Vector(10, 10, 10),
                Ambient = new Colour(50 / 255f, 0, 0)
            });

            Random rnd = new Random();
            var colours = (from prop in typeof(Color).GetProperties()
                           where prop.PropertyType == typeof(Color)
                           select (Color)prop.GetValue(null, null)).ToList();
            
            foreach (var colour in colours)
            {
                if (colour.IsNamedColor)
                {
                    scene.AddMaterial(new Material()
                    {
                        Name = colour.Name,
                        Diffuse = new Colour(colour.R / 255f, colour.G / 255f, colour.B / 255f),
                        Reflective = new Colour(colour.R / 255f, colour.G / 255f, colour.B / 255f),
                        Specular = new Colour((float)rnd.NextDouble(), (float)rnd.NextDouble(), (float)rnd.NextDouble()),                        
                        SpecularExponent = (float)rnd.NextDouble(),
                        Specularity = rnd.Next(1, 60)                        
                    }, colour.Name);
                }
            }                    

            if (scene.FindMaterial("lightgray") == null)
                scene.AddMaterial(new Material()
                {
                    Diffuse = new Colour(0.9f)
                }, "lightgray");

            if (scene.FindMaterial("darkgray") == null)
                scene.AddMaterial(new Material()
                {
                    Diffuse = new Colour(0.4f)
                }, "darkgray");

            scene.AddMaterial(new MaterialCheckerboard()
            {
                SubMaterial1 = scene.FindMaterial("lightgray"),
                SubMaterial2 = scene.FindMaterial("darkgray"),
            }, "check");
            
            var GetColour = new Func<int, int, int, string>( (x, y, z) =>
            {
                return colours[rnd.Next(colours.Count)].Name;
            });

            for (int x = 0; x < 10; x++)
            {
                for (int y = 0; y < 10; y++)
                {
                    for (int z = 0; z < 10; z++)
                    {
                        scene.AddObject(new Sphere()
                        {
                            Pos = new Vector(-10 + (x * 2), -10 + y * 2, -10 + z * 2),
                            Radius = 0.5f,
                            Material = scene.FindMaterial( GetColour(x, y, z) )
                        });
                    }
                }
            }
            
            scene.AddObject(new Plane()
            {
                Ori = new Vector(0,-6,0),
                Normal = new Vector(0,1,0),
                Material = scene.FindMaterial("check")
            });

            VBRaySceneLoader loader = new VBRaySceneLoader();
            using (var sw = new StreamWriter("BallGrid.ray"))
                loader.SaveScene(sw, scene);

            PictureBoxBmp bmp = new PictureBoxBmp(pictureBox1);
            scene.TraceScene(bmp);
            bmp.Render();

            pictureBox1.Image.Save("output.bmp", ImageFormat.Bmp);
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
                
                Render(width, height, bmp, blnMultiThreaded, UpdateScreenRenderOptions);

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

        private void Render(int width, int height, IBmp bmp, bool blnMultiThreaded, Action<bool, bool, bool, int> UpdateScreenRenderOptions)
        {
            VBRaySceneLoader loader = new VBRaySceneLoader();

            Stopwatch watch = new Stopwatch();

            UpdateScreenRenderOptions(m_scene.TraceShadows, m_scene.TraceReflections, m_scene.TraceRefractions, m_scene.RecursionDepth);

            m_scene.MultiThreaded = blnMultiThreaded;
            m_scene.Width = width;
            m_scene.Height = height;

            watch.Start();
            m_scene.TraceScene(bmp);
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

        private void Form1_Load(object sender, EventArgs e)
        {
            //BuidlCSGScene();
            //Go();
        }

        private void BuidlCSGScene()
        {
            Scene scene = new Scene();

            scene.Width = pictureBox1.Width;
            scene.Height = pictureBox1.Height;
            scene.FieldOfView = 90;

            scene.EyePosition = new Vector(2.2, 1.2, -1.4);
            scene.ViewPointRotation = new Vector(30, -55, 0);
            
            scene.RecursionDepth = 1;
            scene.TraceReflections = false;
            scene.TraceRefractions = false;
            scene.TraceShadows = true;

            scene.AddLight(new Light()
            {
                Pos = new Vector(10, 10, 10),
                Ambient = new Colour(50 / 255f)
            });

            scene.AddLight(new Light()
            {
                Pos = new Vector(-20, 20, 20),
                Diffuse = new Colour(50 / 255f)
            });

            scene.AddLight(new Light()
            {
                Pos = new Vector(20, 20, -20),
                Diffuse = new Colour(50 / 255f)
            });

            
            if (scene.FindMaterial("lightgray") == null)
                scene.AddMaterial(new Material()
                {
                    Diffuse = new Colour(0.9f)
                }, "lightgray");

            if (scene.FindMaterial("darkgray") == null)
                scene.AddMaterial(new Material()
                {
                    Diffuse = new Colour(0.4f)
                }, "darkgray");

            int slices = 10;
            int stacks = 10;

            var a = CSG.cube();
            var b = CSG.sphere(new Vector(), 1.35, slices, stacks);
            var c = CSG.cylinder(new Vector(-1, 0, 0), new Vector(1, 0, 0), 0.4, slices);
            var d = CSG.cylinder(new Vector(0, -1, 0), new Vector(0, 1, 0), 0.4, slices);
            var e = CSG.cylinder(new Vector(0, 0, -1), new Vector(0, 0, 1), 0.4, slices);
            var f = CSG.sphere(new Vector(), 1.45, slices, stacks);

            var polygons = a.subtract(b).union(c).union(d).union(e).intersect(f).toPolygons();
            
            var triangles = from poly in polygons
                            from tri in CreateTriangles(poly.vertices.Select( v => v.pos).ToList(), null)
                            select tri;

            Mesh mesh = new Mesh(triangles.ToList());
            mesh.Name = "CSG";

            scene.AddMeshes(mesh, mesh.Name);

            MeshInstance inst = new MeshInstance();
            inst.Mesh = mesh;
            
            scene.AddObject(inst);

            PictureBoxBmp bmp = new PictureBoxBmp(pictureBox1);
            scene.TraceScene(bmp);
            bmp.Render();

           // pictureBox1.Image.Save("CSG.bmp", ImageFormat.Bmp);
        }

        private static List<Triangle> CreateTriangles(List<Vector> verticies, Material currentMaterial)
        {
            List<Triangle> triangles = new List<Triangle>();

            Vector v1, v2, v3;

            v1 = verticies[0];

            for (int i = 0; i < verticies.Count - 2; ++i)
            {
                v2 = verticies[i + 1];
                v3 = verticies[i + 2];

                if (v1 == v2 || v1 == v3 || v2 == v3)
                    continue;

                Triangle tri = new Triangle();
                tri.Vertex[0] = v1;
                tri.Vertex[1] = v2;
                tri.Vertex[2] = v3;

                tri.Material = currentMaterial;
                triangles.Add(tri);
            }

            return triangles;
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
            if (m_scene == null)
                return;

            ((ToolStripMenuItem)sender).Checked = !((ToolStripMenuItem)sender).Checked;
            m_scene.TraceShadows = ((ToolStripMenuItem)sender).Checked;
        }

        private void mnuReflections_Click(object sender, EventArgs e)
        {
            if (m_scene == null)
                return;

            ((ToolStripMenuItem)sender).Checked = !((ToolStripMenuItem)sender).Checked;
            m_scene.TraceReflections = ((ToolStripMenuItem)sender).Checked;
        }

        private void mnuRefractions_Click(object sender, EventArgs e)
        {
            if (m_scene == null)
                return;

            ((ToolStripMenuItem)sender).Checked = !((ToolStripMenuItem)sender).Checked;
            m_scene.TraceRefractions = ((ToolStripMenuItem)sender).Checked;
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
            if (m_scene == null)
                return;

            var menu = (ToolStripMenuItem)sender;

            foreach (ToolStripMenuItem item in menu.GetCurrentParent().Items.Cast<ToolStripMenuItem>().Take(4))
                item.Checked = false;

            menu.Checked = true;

            m_scene.Antialiaser = GetAntialiaser();
        }

        private uint GetAnitaliasingLevel()
        {
            return (from item in mnuSuperSampling.DropDownItems.Cast<ToolStripMenuItem>().Take(4)
                    where item.Checked
                    select uint.Parse(item.Tag.ToString())).First();
        }

        private IAntialiaser GetAntialiaser()
        {
             //uint.Parse(((ToolStripMenuItem)sender).Tag.ToString());
            return new EdgeDetectionResampling(GetAnitaliasingLevel(), 
                                               renderAntialiasingSamplesToolStripMenuItem.Checked);
        }

        private void renderAntialiasingSamplesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            renderAntialiasingSamplesToolStripMenuItem.Checked = !renderAntialiasingSamplesToolStripMenuItem.Checked;
        }

        private void stochasticSampleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            stochasticSampleToolStripMenuItem.Checked = !stochasticSampleToolStripMenuItem.Checked;
        }
    }
}

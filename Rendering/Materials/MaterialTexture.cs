using System;
using System.Drawing;
using Raytracer.Rendering.Core;
using Raytracer.Rendering.FileTypes;

namespace Raytracer.Rendering.Materials
{
    class MaterialTexture : Material
    {
        private Bmp _diffuseMap;

        public double UScale { get; set; }

        public double VScale { get; set; }

        public MaterialTexture(Material other)
        {
            Material.CloneElements(this, other);
            this.Name = other.Name;
        }

        public MaterialTexture()
        {            
        }

        public override void SolidifyMaterial(IntersectionInfo info, Material output)
        {                        
            MaterialDispatcher dispatcher = new MaterialDispatcher();
            dispatcher.Solidify((dynamic)info.Primitive, this, info, output);
        }

        public void LoadDiffuseMap(string path)
        {
            using(var bmp = new Bitmap(path))
                _diffuseMap = ImageReader.Read(bmp);
        }

        public void LoadAmbientMap(string path)
        {
            // DOES NOTHING for NOW
        }

        internal Colour Sample(int x, int y)
        {
            return _diffuseMap.GetPixel(x, y);
        }

        internal Colour Sample(double u, double v)
        {
            try
            {
                u = u * _diffuseMap.Size.Width - 0.5;
                v = v * _diffuseMap.Size.Height - 0.5;

                int x = (int)Math.Floor(u);
                int y = (int)Math.Floor(v);

                double u_ratio = u - x;
                double v_ratio = v - y;
                double u_opposite = 1 - u_ratio;
                double v_opposite = 1 - v_ratio;

                x = x % _diffuseMap.Size.Width;
                y = y % _diffuseMap.Size.Height;

                if (x < 0)
                    x += _diffuseMap.Size.Width;

                if (y < 0)
                    y += _diffuseMap.Size.Height;

                return (GetPixelSafe(x, y)      * u_opposite + GetPixelSafe(x + 1, y)        * u_ratio) * v_opposite +
                       (GetPixelSafe(x, y + 1)  * u_opposite + GetPixelSafe(x + 1, y + 1)    * u_ratio) * v_ratio;
            }
            catch(Exception)
            {
                Console.WriteLine();
            }

            return new Colour(0, 0, 0);
        }

        public Colour GetPixelSafe(int x, int y)
        {
            int maxX = _diffuseMap.Size.Width - 1;
            int maxY = _diffuseMap.Size.Height - 1;

            x = Math.Min(maxX, Math.Max(0, x));
            y = Math.Min(maxY, Math.Max(0, y));

            return _diffuseMap.GetPixel(x, y);
        }

        public double Width { get { return _diffuseMap.Size.Width; } }

        public double Height { get { return _diffuseMap.Size.Height; } }
    }
}

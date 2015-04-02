﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
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
                _diffuseMap = new Bmp(bmp);
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
                u = u * _diffuseMap.Width - 0.5;
                v = v * _diffuseMap.Height - 0.5;

                int x = (int)Math.Floor(u);
                int y = (int)Math.Floor(v);

                double u_ratio = u - x;
                double v_ratio = v - y;
                double u_opposite = 1 - u_ratio;
                double v_opposite = 1 - v_ratio;

                return (GetPixelSafe(x, y)      * u_opposite + GetPixelSafe(x + 1, y)        * u_ratio) * v_opposite +
                       (GetPixelSafe(x, y + 1)  * u_opposite + GetPixelSafe(x + 1, y + 1)    * u_ratio) * v_ratio;
            }
            catch(Exception ex)
            {
                Console.WriteLine();
            }

            return new Colour(0, 0, 0);
        }

        public Colour GetPixelSafe(int x, int y)
        {
            int maxX = _diffuseMap.Width - 1;
            int maxY = _diffuseMap.Height - 1;

            x = Math.Min(maxX, Math.Max(0, x));
            y = Math.Min(maxY, Math.Max(0, y));

            return _diffuseMap.GetPixel(x, y);
        }

        public double Width { get { return _diffuseMap.Width; } }

        public double Height { get { return _diffuseMap.Height; } }
    }
}
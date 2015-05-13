using System;
using System.Drawing;
using Raytracer.Rendering.Core;
using Raytracer.Rendering.FileTypes;

namespace Raytracer.Rendering.Materials
{
    class MaterialTexture : Material
    {
        private Texture _diffuseMap;

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
            _diffuseMap = new Texture(ImageReader.Read(path));
        }

        public void LoadAmbientMap(string path)
        {
            // DOES NOTHING for NOW
        }

        internal Colour Sample(double u, double v)
        {
            return _diffuseMap.Sample(u, v);
        }
    }
}

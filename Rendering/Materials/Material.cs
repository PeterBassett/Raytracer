using Raytracer.Rendering.Core;

namespace Raytracer.Rendering.Materials
{    
    class Material
    {        
        public Material()
        {
            Refraction = 1.0f;
	        Specularity = 20.0f;
	        SpecularExponent = 0.3f;
            Ambient = new Colour();
            Diffuse = new Colour();
            Emissive = new Colour();
            Reflective = new Colour();
            Transmitted = new Colour();
            Specular = new Colour();
            Density = 0;
        }

        public Colour Ambient { get; set; }
        public Colour Diffuse { get; set; }
        public Colour Emissive { get; set; }
        public Colour Reflective { get; set; }
        public Colour Transmitted { get; set; }
        public float Refraction { get; set; }
        public Colour Specular { get; set; }
        public float Specularity { get; set; }
        public float SpecularExponent { get; set; }
        public float Density { get; set; }

        public string Name { get; set; }

        public virtual void SolidifyMaterial(IntersectionInfo info, Material output)
        {
            CloneElements(output, this);
        }

        public static void CloneElements(Material dest, Material source)
        {
	        dest.Specularity = source.Specularity;
            dest.SpecularExponent = source.SpecularExponent;
            dest.Ambient = new Colour(source.Ambient);
            dest.Diffuse = new Colour(source.Diffuse);
            dest.Specular = new Colour(source.Specular);
            dest.Emissive = new Colour(source.Emissive);
            dest.Reflective = new Colour(source.Reflective);
            dest.Transmitted = new Colour(source.Transmitted);
            dest.Refraction = source.Refraction;
            dest.Density = source.Density;
        }
    }
}

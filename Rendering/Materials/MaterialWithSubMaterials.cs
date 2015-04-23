using Raytracer.MathTypes;

namespace Raytracer.Rendering.Materials
{
    class MaterialWithSubMaterials : Material
    {
        public MaterialWithSubMaterials()
        {
            Size = new Vector(0.0f, 0.0f, 0.0f);
        }
        public Material SubMaterial1 { get; set; }
        public Material SubMaterial2 { get; set; }
        public Vector Size { get; set; }
    }
}

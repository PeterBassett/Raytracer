using Raytracer.MathTypes;

namespace Raytracer.Rendering.Materials
{
    class MaterialWithSubMaterials : Material
    {
        public MaterialWithSubMaterials()
        {
            Size = new Vector3(0.0f, 0.0f, 0.0f);
        }
        public Material SubMaterial1 { get; set; }
        public Material SubMaterial2 { get; set; }
        public Vector3 Size { get; set; }
    }
}

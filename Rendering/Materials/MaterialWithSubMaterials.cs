using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Raytracer.MathTypes;

namespace Raytracer.Rendering.Materials
{
    using Vector = Vector3D;

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

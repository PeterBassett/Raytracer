using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Raytracer.Rendering.FileTypes;

namespace Raytracer.Rendering.BackgroundMaterials
{
    class SolidColourBackground : IBackgroundMaterial
    {
        private Colour _colour;
        public SolidColourBackground(Colour colour)
        {
            _colour = colour;
        }

        public FileTypes.Colour Shade(Ray ray)
        {
            return _colour;
        }
    }
}

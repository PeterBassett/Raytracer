using System;
using Raytracer.Rendering.Core;

namespace Raytracer.Rendering.BackgroundMaterials
{
    class SolidColourBackground : IBackgroundMaterial
    {
        private readonly Colour _colour;

        public SolidColourBackground(Colour colour)
        {
            if (colour == null)
                throw new ArgumentNullException("colour");
            _colour = colour;
        }

        public Colour Shade(Ray ray)
        {
            return _colour;
        }
    }
}

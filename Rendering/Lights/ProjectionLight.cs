using System;
using Raytracer.MathTypes;
using Raytracer.Rendering.Core;

namespace Raytracer.Rendering.Lights
{
    class ProjectionLight : Light
    {
        private const double Hither = 1e-3f;

        private readonly Texture _texture;        
        private readonly double _screenX0;
        private readonly double _screenX1;
        private readonly double _screenY0;
        private readonly double _screenY1;
        private readonly Matrix _lightProjection;

        public ProjectionLight(Colour colourTint, IBmp textureBmp, float power, float totalWidthInDegrees, Transform transform)
            : base(colourTint, power, transform)
        {
            double fov = (float)MathLib.Deg2Rad(totalWidthInDegrees);
            _texture = new Texture(textureBmp);

            var aspect = textureBmp.Size.Width / textureBmp.Size.Height;
            if (aspect > 1)
            {
                _screenX0 = -aspect;
                _screenX1 = aspect;
                _screenY0 = -1;
                _screenY1 = 1;
            }
            else
            {
                _screenX0 = -1;
                _screenX1 = 1;
                _screenY0 = -1.0f / aspect;
                _screenY1 = 1.0f / aspect;
            }

            const double yon = 1e30f;

            _lightProjection = Matrix.CreatePerspectiveFieldOfView(fov, aspect, Hither, yon);

            // Compute cosine of cone surrounding projection directions
            var opposite = Math.Tan(fov / 2.0);
            var tanDiag = opposite * Math.Sqrt(1.0 + 1.0 / (aspect * aspect));
            Math.Cos(Math.Atan(tanDiag));
        }

        protected override Colour Sample(Point hitPoint, Normal normalAtHitPoint, ref Vector pointToLight, ref VisibilityTester visibilityTester)
        {
            pointToLight = Pos - hitPoint;

            var lengthSquared = pointToLight.LengthSquared;

            pointToLight = pointToLight.Normalize();

            visibilityTester.SetSegment(hitPoint, normalAtHitPoint, Pos);

            return Intensity * Projection(-pointToLight) / lengthSquared;
        }

        Colour Projection(Vector w) 
        {
            var wl = Transform.ToObjectSpace(w).Normalize();

            // Discard directions behind projection light
            if (wl.Z < Hither) 
                return new Colour(0);

            // Project point onto projection plane and compute light
            var Pl = _lightProjection.Transform((Point)wl);
            
            if (Pl.X < _screenX0 || Pl.X > _screenX1 ||
                Pl.Y < _screenY0 || Pl.Y > _screenY1)
                return new Colour(0);

            var s = (Pl.X - _screenX0) / (_screenX1 - _screenX0);
            var t = (Pl.Y - _screenY0) / (_screenY1 - _screenY0);
            
            return _texture.Sample(s, t);
        }
    }
}
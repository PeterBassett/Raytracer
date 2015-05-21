using System;
using Raytracer.MathTypes;
using Raytracer.Rendering.Core;
using Raytracer.FileTypes;

namespace Raytracer.Rendering.Lights
{
    class ProjectionLight : Light
    {
        public double CosTotalWidth;
        public Texture texture;
        double hither, yon;
        double fov;
        double screenX0, screenX1, screenY0, screenY1;
        Matrix lightProjection;

        public ProjectionLight(Colour colourTint, IBmp textureBmp, float power, float totalWidthInDegrees, Transform transform)
            : base(colourTint, transform)
        {
            Intensity *= power;
            fov = (float)MathLib.Deg2Rad(totalWidthInDegrees);
            texture = new Texture(textureBmp);

            var aspect = (float)textureBmp.Size.Width / (float)textureBmp.Size.Height;
            if (aspect > 1)
            {
                screenX0 = -aspect;
                screenX1 = aspect;
                screenY0 = -1;
                screenY1 = 1;
            }
            else
            {
                screenX0 = -1;
                screenX1 = 1;
                screenY0 = -1.0f / aspect;
                screenY1 = 1.0f / aspect;
            }
            hither = 1e-3f;
            yon = 1e30f;
            lightProjection = Matrix.CreatePerspectiveFieldOfView(fov, aspect, hither, yon);

            // Compute cosine of cone surrounding projection directions
            var opposite = Math.Tan(fov / 2.0);
            var tanDiag = opposite * Math.Sqrt(1.0 + 1.0 / (aspect * aspect));
            CosTotalWidth = Math.Cos(Math.Atan(tanDiag));
        }

        public override Colour Sample(Point hitPoint, Normal normalAtHitPoint, ref Vector pointToLight, ref VisibilityTester visibilityTester)
        {
            pointToLight = Pos - hitPoint;

            var lengthSquared = pointToLight.LengthSquared;

            pointToLight = pointToLight.Normalize();

            visibilityTester.SetSegment(hitPoint, normalAtHitPoint, Pos);

            return Intensity * Projection(-pointToLight) / lengthSquared;
        }

        Colour Projection(Vector w) 
        {
            var wl = _transform.ToObjectSpace(w).Normalize();

            // Discard directions behind projection light
            if (wl.Z < hither) 
                return new Colour(0);

            // Project point onto projection plane and compute light
            var Pl = lightProjection.Transform((Point)wl);
            
            if (Pl.X < screenX0 || Pl.X > screenX1 ||
                Pl.Y < screenY0 || Pl.Y > screenY1)
                return new Colour(0);

            var s = (Pl.X - screenX0) / (screenX1 - screenX0);
            var t = (Pl.Y - screenY0) / (screenY1 - screenY0);
            
            return texture.Sample(s, t);
        }
    }
}
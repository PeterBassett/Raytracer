using System;
using Raytracer.MathTypes;
using Raytracer.Rendering.Cameras;
using Raytracer.Rendering.Core;
using Raytracer.Rendering.Materials;
using Raytracer.Rendering.RenderingStrategies;
using Raytracer.Rendering.Distributions;

namespace Raytracer.Rendering.Renderers
{
    class RayTracingRenderer : TracingRendererBase
    {
        public RayTracingRenderer(int spp) : base(spp)
        {
        }

        protected override Colour TraceRay(Ray ray, Colour contribution, double curRefractionIndex, long depth, Vector eyeDirection, out double intersectionDistance)
        {
            var colour = new Colour(0.0f);

            var info = FindClosestIntersection(ray);

            if (info.Result == HitResult.Miss)
            {
                if (Scene.BackgroundMaterial != null)
                    colour = Scene.BackgroundMaterial.Shade(ray);

                intersectionDistance = 0;
                return colour;
            }

            intersectionDistance = info.T;
            
            // set the 
            var material = new Material();
            var objectMaterial = info.Material ?? Scene.DefaultMaterial;

            var materialDispatcher = new MaterialDispatcher();
            materialDispatcher.Solidify((dynamic)info.Primitive, (dynamic)objectMaterial, info, material);
                
            // get the shading due to lighting at this point
            colour = Shade(info.HitPoint, info.NormalAtHitPoint, material, eyeDirection) * contribution;

            if (depth == Settings.PathDepth)
            {
                colour.Clamp();
                return colour;
            }

            // if we are dealing with a reflective material
            if (material.Reflective.Sum() > 0.0f && Settings.TraceReflections)
            {
                Colour colReflectAmount = material.Reflective * contribution;

                if (colReflectAmount.Sum() > 0.01f)
                {
                    // calculate the new reflected direction
                    var reflectedRay = new Ray(info.HitPoint, CalculateReflectedRay(ray.Dir, info.NormalAtHitPoint));
                    // recursivly call trace ray
                    double t;
                    colour += TraceRay(reflectedRay, colReflectAmount, curRefractionIndex, depth + 1, eyeDirection, out t);
                }
            }

            // if we are dealing with a refractive material
            if (material.Transmitted.Sum() > 0.0f && Settings.TraceRefractions)
            {
                Colour colRefractiveAmount = material.Transmitted * contribution;

                if (colRefractiveAmount.Sum() > 0.01f)
                {

                    double outReflectionFactor;
                    Colour rcol = CalculateRefraction(
                        info,
                        ray.Dir,
                        curRefractionIndex,
                        colRefractiveAmount,
                        depth,
                        out outReflectionFactor);

                    colour += rcol;                    
                }
            }            

            colour.Clamp();
            return colour;
        }
    }
}

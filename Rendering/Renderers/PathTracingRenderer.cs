using System;
using Raytracer.MathTypes;
using Raytracer.Rendering.Cameras;
using Raytracer.Rendering.Core;
using Raytracer.Rendering.Materials;
using Raytracer.Rendering.RenderingStrategies;
using Raytracer.Rendering.Distributions;
using Raytracer.Rendering.Samplers;

namespace Raytracer.Rendering.Renderers
{
    class PathTracingRenderer : TracingRendererBase
    {       
        private int _spp;

        public PathTracingRenderer(int spp)
            : base(spp)
        {
        }

        protected override Colour TraceRay(Ray ray, Colour contribution, double curRefractionIndex, long depth, Vector eyeDirection, out double intersectionDistance)
        {
            /*
            if (depth == MaxDepth) {
              return Black;  // Bounced enough times.
            }

            r.FindNearestObject();
            if (r.hitSomething == false) {
              return Black;  // Nothing was hit.
            }

            Material m = r.thingHit->material;
            Color emittance = m.emittance;

            // Pick a random direction from here and keep going.
            Ray newRay;
            newRay.origin = r.pointWhereObjWasHit;
            newRay.direction = RandomUnitVectorInHemisphereOf(r.normalWhereObjWasHit);  // This is NOT a cosine-weighted distribution!

            // Compute the BRDF for this ray (assuming Lambertian reflection)
            float cos_theta = DotProduct(newRay.direction, r.normalWhereObjWasHit);
            Color BRDF = 2 * m.reflectance * cos_theta;
            Color reflected = TracePath(newRay, depth + 1);

            // Apply the Rendering Equation here.
            return emittance + (BRDF * reflected);
            */

            intersectionDistance = 0;
            var colour = new Colour(0.0f);

            if (depth == Settings.PathDepth)
                return colour;
            
            var info = FindClosestIntersection(ray);

            if (info.Result == HitResult.Miss)
            {
                if (Scene.BackgroundMaterial != null)
                    colour = Scene.BackgroundMaterial.Shade(ray);

                intersectionDistance = 0;
                return colour;
            }


            if (!(info.Primitive is Raytracer.Rendering.Primitives.Plane) && depth > 2)
                Console.WriteLine();

            if (info.Primitive is Raytracer.Rendering.Primitives.Sphere && info.Primitive.Material.Name == "whiteLight" && depth > 2)
                Console.WriteLine();

            intersectionDistance = info.T;
            
            // set the 
            var material = new Material();
            var objectMaterial = info.Material ?? Scene.DefaultMaterial;

            var materialDispatcher = new MaterialDispatcher();
            materialDispatcher.Solidify((dynamic)info.Primitive, (dynamic)objectMaterial, info, material);
                
            // get the shading due to lighting at this point
            //colour = Shade(info.HitPoint, info.NormalAtHitPoint, material, eyeDirection) * contribution;

            if (depth == Settings.PathDepth)
            {
                colour.Clamp();
                return colour;
            }

            Material m = material;// info.Material;
            Colour emittance = m.Emissive;

            // Pick a random direction from here and keep going.
            Ray newRay = new Ray(info.HitPoint, Sampler.RandomUnitVectorInHemisphereOf(info.NormalAtHitPoint));  // This is NOT a cosine-weighted distribution!

            // Compute the BRDF for this ray (assuming Lambertian reflection)
            var cos_theta = Vector.DotProduct(newRay.Dir, info.NormalAtHitPoint);
            Colour BRDF = 2 * m.Diffuse * cos_theta;
            double dist = 0;
            Colour reflected = TraceRay(newRay, Colour.White, 1, depth + 1, eyeDirection, out dist);

            // Apply the Rendering Equation here.
            return emittance + (BRDF * reflected);            
        }
    }
}

using System;
using Raytracer.MathTypes;
using Raytracer.Rendering.Cameras;
using Raytracer.Rendering.Core;
using Raytracer.Rendering.Materials;
using Raytracer.Rendering.RenderingStrategies;
using Raytracer.Rendering.Distributions;

namespace Raytracer.Rendering.Renderers
{
    class RayTracingRenderer : IRenderer
    {
        public RenderSettings Settings { get; set; }
        public IRenderingStrategy RenderingStrategy { get; set; }
        public ICamera Camera { get; set; }
        public Scene Scene { get; set; }
        public Distribution Distribution { get; set; }

        public void RenderScene(IBmp frameBuffer)
        {
            AssertDependancies();

            RenderingStrategy.RenderScene(this, frameBuffer);
        }

        private void AssertDependancies()
        {
            if (Scene == null)
                throw new ArgumentNullException("scene");

            if (Camera == null)
                throw new ArgumentNullException("camera");

            if (Settings == null)
                throw new ArgumentNullException("settings");

            if (RenderingStrategy == null)
                throw new ArgumentNullException("renderingStrategy");
        }

        public Colour ComputeSample(Vector2 pixelCoordinates)
        {
            return Trace(Camera.GenerateRayForPixel(pixelCoordinates));
        }

        private Colour Trace(Ray ray)
        {
            return TraceRay(ray, new Colour(1.0f), 1.0f, 1, ray.Dir);
        }
        
        public IntersectionInfo FindClosestIntersection(Ray ray)
        {
            var minimumIntersection = new IntersectionInfo(HitResult.Miss);

            foreach (var obj in Scene.GetCandiates(ray))
            {
                var result = obj.Intersect(ray);

                if (result.T < minimumIntersection.T && result.Result != HitResult.Miss)
                {
                    minimumIntersection = result;
                }
            }

            return minimumIntersection;
        }

        private Traceable FindObjectContainingPoint(Point point)
        {
            return Scene.FindObjectContainingPoint(point);
        }

        private Colour TraceRay(Ray ray, Colour contribution, double curRefractionIndex, long depth, Vector eyeDirection)
        {
            var colour = new Colour(0.0f);

            var info = FindClosestIntersection(ray);

            if (info.Result == HitResult.Miss)
            {
                if (Scene.BackgroundMaterial != null)
                    colour = Scene.BackgroundMaterial.Shade(ray);

                return colour;
            }
            
            // set the 
            var material = new Material();
            var objectMaterial = info.Primitive.Material ?? Scene.DefaultMaterial;

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
                    colour += TraceRay(reflectedRay, colReflectAmount, curRefractionIndex, depth + 1, eyeDirection);
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


        Colour CalculateRefraction(
            IntersectionInfo intersection, 
            Vector direction, 
            double sourceRefractiveIndex,
            Colour rayIntensity,
            long recursionDepth,
            out double outReflectionFactor)
        {
            // Convert direction to a unit vector so that
            // relation between angle and dot product is simpler.
            Vector dirUnit = direction;
            dirUnit = dirUnit.Normalize();

            var cosA1 = Vector.DotProduct(dirUnit, intersection.NormalAtHitPoint);
            double sinA1;
            if (cosA1 <= -1.0)
            {
                if (cosA1 < -1.0001)
                {
                    throw new Exception("Dot product too small.");
                }
                // The incident ray points in exactly the opposite
                // direction as the normal vector, so the ray
                // is entering the solid exactly perpendicular
                // to the surface at the intersection point.
                cosA1 = -1.0;  // clamp to lower limit
                sinA1 =  0.0;
            }
            else if (cosA1 >= +1.0)
            {
                if (cosA1 > +1.0001)
                {
                    throw new Exception("Dot product too large.");
                }
                // The incident ray points in exactly the same
                // direction as the normal vector, so the ray
                // is exiting the solid exactly perpendicular
                // to the surface at the intersection point.
                cosA1 = +1.0;  // clamp to upper limit
                sinA1 =  0.0;
            }
            else
            {
                // The ray is entering/exiting the solid at some
                // positive angle with respect to the normal vector.
                // We need to calculate the sine of that angle
                // using the trig identity cos^2 + sin^2 = 1.
                // The angle between any two vectors is always between
                // 0 and PI, so the sine of such an angle is never negative.
                sinA1 = Math.Sqrt(1.0 - cosA1*cosA1);
            }

            // The parameter sourceRefractiveIndex passed to this function
            // tells us the refractive index of the medium the light ray
            // was passing through before striking this intersection.
            // We need to figure out what the target refractive index is,
            // i.e., the refractive index of whatever substance the ray 
            // is about to pass into.  We determine this by pretending that
            // the ray continues traveling in the same direction a tiny
            // amount beyond the intersection point, then asking which
            // solid object (if any) contains that test point.
            // Ties are broken by insertion order: whichever solid was
            // inserted into the scene first that contains a point is 
            // considered the winner.  If a solid is found, its refractive
            // index is used as the target refractive index; otherwise,
            // we use the scene's ambient refraction, which defaults to 
            // vacuum (but that can be overridden by a call to 
            // Scene::SetAmbientRefraction).
            var testPoint = intersection.HitPoint + MathLib.IntersectionEpsilon * dirUnit;

            var container = FindObjectContainingPoint(testPoint);

            var material = new Material();
            double targetRefractiveIndex = Scene.DefaultMaterial.Refraction;

            if (container != null)
            {
                var objectMaterial = container.Material ?? Scene.DefaultMaterial;

                var materialDispatcher = new MaterialDispatcher();
                
                materialDispatcher.Solidify((dynamic)container, (dynamic)objectMaterial, intersection, material);

                targetRefractiveIndex = material.Refraction; 
            }

            double ratio = sourceRefractiveIndex / targetRefractiveIndex;

            // Snell's Law: the sine of the refracted ray's angle
            // with the normal is obtained by multiplying the
            // ratio of refractive indices by the sine of the
            // incident ray's angle with the normal.
            double sinA2 = ratio * sinA1;

            if (sinA2 <= -1.0 || sinA2 >= +1.0)
            {
                // Since sin_a2 is outside the bounds -1..+1, then
                // there is no such real angle a2, which in turn
                // means that the ray experiences total internal reflection,
                // so that no refracted ray exists.

                Vector reflectionVector = CalculateReflectedRay(dirUnit, intersection.NormalAtHitPoint);
                outReflectionFactor = 1;
                var reflectedRay = new Ray(intersection.HitPoint, reflectionVector);
                return TraceRay(reflectedRay, rayIntensity, sourceRefractiveIndex, recursionDepth + 1, reflectedRay.Dir);
            }

            // Getting here means there is at least a little bit of
            // refracted light in addition to reflected light.
            // Determine the direction of the refracted light.
            // We solve a quadratic equation to help us calculate
            // the vector direction of the refracted ray.

            var k = new double[2];
            int numSolutions = Algebra.SolveQuadraticEquation(
                1.0,
                2.0 * cosA1,
                1.0 - 1.0/(ratio*ratio),
                k);

            // There are generally 2 solutions for k, but only 
            // one of them is correct.  The right answer is the
            // value of k that causes the light ray to bend the
            // smallest angle when comparing the direction of the
            // refracted ray to the incident ray.  This is the 
            // same as finding the hypothetical refracted ray 
            // with the largest positive dot product.
            // In real refraction, the ray is always bent by less
            // than 90 degrees, so all valid dot products are 
            // positive numbers.
            double maxAlignment = -0.0001;  // any negative number works as a flag
            var refractDir = new Vector();
            for (int i=0; i < numSolutions; ++i)
            {
                Vector refractAttempt = dirUnit + k[i] * intersection.NormalAtHitPoint;
                double alignment = Vector.DotProduct(dirUnit, refractAttempt);
                if (alignment > maxAlignment)
                {
                    maxAlignment = alignment;
                    refractDir = refractAttempt;
                }
            }

            refractDir = refractDir.Normalize();
            if (maxAlignment <= 0.0)
            {
                // Getting here means there is something wrong with the math.
                // Either there were no solutions to the quadratic equation,
                // or all solutions caused the refracted ray to bend 90 degrees
                // or more, which is not possible.
                throw new Exception("Refraction failure.");
            }

            // Determine the cosine of the exit angle.
            double cosA2 = Math.Sqrt(1.0 - sinA2*sinA2);
            if (cosA1 < 0.0)
            {
                // Tricky bit: the polarity of cos_a2 must
                // match that of cos_a1.
                cosA2 = -cosA2;
            }

            // Determine what fraction of the light is
            // reflected at the interface.  The caller
            // needs to know this for calculating total
            // reflection, so it is saved in an output parameter.

            // We assume uniform polarization of light,
            // and therefore average the contributions of s-polarized
            // and p-polarized light.
            double Rs = PolarizedReflection(
                sourceRefractiveIndex,
                targetRefractiveIndex,
                cosA1,
                cosA2);

            double Rp = PolarizedReflection(
                sourceRefractiveIndex,
                targetRefractiveIndex,
                cosA2,
                cosA1);

            outReflectionFactor = (Rs + Rp) / 2.0;

            // Whatever fraction of the light is NOT reflected
            // goes into refraction.  The incoming ray intensity
            // is thus diminished by this fraction.
            Colour nextRayIntensity = (1.0 - outReflectionFactor) * rayIntensity;

            var ray = new Ray(testPoint, refractDir);
            return TraceRay(ray, nextRayIntensity, targetRefractiveIndex, recursionDepth + 1, ray.Dir);
        }

        double PolarizedReflection(
            double n1,              // source material's index of refraction
            double n2,              // target material's index of refraction
            double cosA1,          // incident or outgoing ray angle cosine
            double cosA2)          // outgoing or incident ray angle cosine
        {
            var left  = n1 * cosA1;
            var right = n2 * cosA2;
            var numer = left - right;
            var denom = left + right;
            denom *= denom;     // square the denominator

            if (denom < MathLib.Epsilon)
            {
                // Assume complete reflection.
                return 1.0;
            }

            var reflection = (numer*numer) / denom;
            
            return reflection > 1.0 ? 1.0 : reflection;
        }

        private Colour Shade(Point hitPoint, Normal normal, Material material, Vector eyeDirection)
        {
            // first assign the emmissive part of the color as the base color
            Colour colour = material.Emissive;

            // iterate through all the lights in the scene
            foreach (var light in Scene.Lights)
            {
                var visibilityTester = new VisibilityTester(this);
                var pointToLight = Vector.Zero;
                var lightColour = light.SampleLight(hitPoint, normal, ref pointToLight, ref visibilityTester);

                // get the angle between the light vector ad the surface normal
                var lightCos = 1.0;// Vector.DotProduct(pointToLight, normal);

                if (lightColour.Brightness > 0 && lightCos > 0.0 && (!Settings.TraceShadows || visibilityTester.Unoccluded()))
                {
                    colour += (material.Diffuse * lightColour) * lightCos;

                    if (material.Specularity > 0.0f && light.Specular())
                    {
                        // calculate specular highlights
                        var vReflect = CalculateReflectedRay(pointToLight, normal);

                        // normalise the vector
                        vReflect = vReflect.Normalize();

                        var fSpecular = Vector.DotProduct(vReflect, eyeDirection);

                        if (fSpecular > 0.0f)
                        {
                            var power = Math.Pow(fSpecular, material.Specularity);
                            colour += lightColour * material.SpecularExponent * power;
                        }
                    }
                }
            }

            return colour;
        }

        private Vector CalculateReflectedRay(Vector dir, Normal normal)
        {
            dir = (-dir).Normalize();

            normal = normal.Normalize();

            var tmp = (normal * (2.0f * Vector.DotProduct(normal, dir))) - dir;
            tmp.Normalize();
            return (Vector)tmp;
        }

        private Vector CalculateRefractedRay(Vector dir, Vector normal, double n_Out, double n_In)
        {
            var c = -Vector.DotProduct(normal, dir);

            var n = n_Out / n_In;

            return (n * dir) + (double)(n * c - Math.Sqrt(1 - n * n * (1 - c * c))) * normal;
        }
    }
}

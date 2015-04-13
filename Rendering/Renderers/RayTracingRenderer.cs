﻿using System;
using Raytracer.MathTypes;
using Raytracer.Rendering.Cameras;
using Raytracer.Rendering.Core;
using Raytracer.Rendering.FileTypes;
using Raytracer.Rendering.Materials;
using Raytracer.Rendering.Primitives;
using Raytracer.Rendering.RenderingStrategies;

namespace Raytracer.Rendering.Renderers
{
    class RayTracingRenderer : IRenderer
    {
        private Scene _scene;
        private IRenderingStrategy _renderingStrategy;
        private ICamera _camera;
        private bool _multiThreaded;
        private uint _maxDepth;
        private bool _traceReflections;
        private bool _traceRefractions;
        private bool _traceShadows;

        public RayTracingRenderer(Scene scene, ICamera camera, IRenderingStrategy renderingStrategy,
            uint maxDepth, bool multiThreaded, 
            bool traceShadows, bool traceReflections, bool traceRefractions)
        {
            _scene = scene;
            _camera = camera;
            _renderingStrategy = renderingStrategy;
            _multiThreaded = multiThreaded;
            _maxDepth = maxDepth;
            _traceShadows = traceShadows;
            _traceReflections = traceReflections;
            _traceRefractions = traceRefractions;
        }

        public void RenderScene(IBmp frameBuffer)
        {
            _renderingStrategy.RenderScene(this, frameBuffer);
        }

        public Colour ComputeSample(Vector2 pixelCoordinates)
        {
            return Trace(_camera.GenerateRayForPixel(pixelCoordinates));
        }

        public Colour Trace(Ray ray)
        {
            return TraceRay(ray, new Colour(1.0f), 1.0f, 1, ray.Dir);
        }
        
        private IntersectionInfo FindClosestIntersection(Ray ray)
        {
            IntersectionInfo minimumIntersection = new IntersectionInfo(HitResult.MISS);

            foreach (var obj in _scene.GetCandiates(ray))
            {
                var result = obj.Intersect(ray);

                if (result.T < minimumIntersection.T && result.Result != HitResult.MISS)
                {
                    minimumIntersection = result;
                }
            }

            return minimumIntersection;
        }

        private Traceable FindObjectContainingPoint(Vector3 point)
        {
            return _scene.GetCandiates(point);
        }

        private Colour TraceRay(Ray ray, Colour contribution, double curRefractionIndex, long depth, Vector3 eyeDirection)
        {
            const double EPSILON = 0.001f;

            Colour colour = new Colour(0.0f);

            IntersectionInfo info = FindClosestIntersection(ray);

            if (info.Result == HitResult.MISS)
            {
                if (_scene.BackgroundMaterial != null)
                    colour = _scene.BackgroundMaterial.Shade(ray);

                return colour;
            }
            
            // set the 
            Material material = new Material();
            Material objectMaterial = info.Primitive.Material != null ? info.Primitive.Material : _scene.DefaultMaterial;

            var materialDispatcher = new MaterialDispatcher();
            materialDispatcher.Solidify((dynamic)info.Primitive, (dynamic)objectMaterial, info, material);
                
            // get the shading due to lighting at this point
            colour = Shade(info.HitPoint, info.NormalAtHitPoint, material, eyeDirection) * contribution;

            if (depth == _maxDepth)
            {
                colour.Clamp();
                return colour;
            }

            // if we are dealing with a reflective material
            if (material.Reflective.Sum() > 0.0f && this._traceReflections)
            {
                Colour colReflectAmount = material.Reflective * contribution;

                if (colReflectAmount.Sum() > 0.01f)
                {
                    // calculate the new reflected direction
                    Ray reflectedRay = new Ray(info.HitPoint, CalculateReflectedRay(ray.Dir, info.NormalAtHitPoint));

                    // recursivly call trace ray
                    colour += TraceRay(reflectedRay, colReflectAmount, curRefractionIndex, depth + 1, eyeDirection);
                }
            }

            // if we are dealing with a refractive material
            if (material.Transmitted.Sum() > 0.0f && this._traceRefractions)
            {
                Colour colRefractiveAmount = material.Transmitted * contribution;

                if (colRefractiveAmount.Sum() > 0.01f)
                {

                    double outReflectionFactor = 0;
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
            Vector3 direction, 
            double sourceRefractiveIndex,
            Colour rayIntensity,
            long recursionDepth,
            out double outReflectionFactor)
        {
            // Convert direction to a unit vector so that
            // relation between angle and dot product is simpler.
            Vector3 dirUnit = direction;
            dirUnit.Normalize();

            double cos_a1 = Vector3.DotProduct(dirUnit, intersection.NormalAtHitPoint);
            double sin_a1;
            if (cos_a1 <= -1.0)
            {
                if (cos_a1 < -1.0001)
                {
                    throw new Exception("Dot product too small.");
                }
                // The incident ray points in exactly the opposite
                // direction as the normal vector, so the ray
                // is entering the solid exactly perpendicular
                // to the surface at the intersection point.
                cos_a1 = -1.0;  // clamp to lower limit
                sin_a1 =  0.0;
            }
            else if (cos_a1 >= +1.0)
            {
                if (cos_a1 > +1.0001)
                {
                    throw new Exception("Dot product too large.");
                }
                // The incident ray points in exactly the same
                // direction as the normal vector, so the ray
                // is exiting the solid exactly perpendicular
                // to the surface at the intersection point.
                cos_a1 = +1.0;  // clamp to upper limit
                sin_a1 =  0.0;
            }
            else
            {
                // The ray is entering/exiting the solid at some
                // positive angle with respect to the normal vector.
                // We need to calculate the sine of that angle
                // using the trig identity cos^2 + sin^2 = 1.
                // The angle between any two vectors is always between
                // 0 and PI, so the sine of such an angle is never negative.
                sin_a1 = Math.Sqrt(1.0 - cos_a1*cos_a1);
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
            Vector3 testPoint = intersection.HitPoint + MathLib.IntersectionEpsilon * dirUnit;

            var container = FindObjectContainingPoint(testPoint);

            Material material = new Material();
            double targetRefractiveIndex = this._scene.DefaultMaterial.Refraction;

            if (container != null)
            {
                Material objectMaterial = container.Material != null ? container.Material : _scene.DefaultMaterial;

                var materialDispatcher = new MaterialDispatcher();
                
                materialDispatcher.Solidify((dynamic)container, (dynamic)objectMaterial, intersection, material);

                targetRefractiveIndex = material.Refraction; 
            }

            double ratio = sourceRefractiveIndex / targetRefractiveIndex;

            // Snell's Law: the sine of the refracted ray's angle
            // with the normal is obtained by multiplying the
            // ratio of refractive indices by the sine of the
            // incident ray's angle with the normal.
            double sin_a2 = ratio * sin_a1;

            if (sin_a2 <= -1.0 || sin_a2 >= +1.0)
            {
                // Since sin_a2 is outside the bounds -1..+1, then
                // there is no such real angle a2, which in turn
                // means that the ray experiences total internal reflection,
                // so that no refracted ray exists.
                outReflectionFactor = 1.0;      // complete reflection
                return new Colour(0.0, 0.0, 0.0);    // no refraction at all
            }

            // Getting here means there is at least a little bit of
            // refracted light in addition to reflected light.
            // Determine the direction of the refracted light.
            // We solve a quadratic equation to help us calculate
            // the vector direction of the refracted ray.

            var k = new double[2];
            int numSolutions = Algebra.SolveQuadraticEquation(
                1.0,
                2.0 * cos_a1,
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
            Vector3 refractDir = new Vector3();
            for (int i=0; i < numSolutions; ++i)
            {
                Vector3 refractAttempt = dirUnit + k[i]*intersection.NormalAtHitPoint;
                double alignment = Vector3.DotProduct(dirUnit, refractAttempt);
                if (alignment > maxAlignment)
                {
                    maxAlignment = alignment;
                    refractDir = refractAttempt;
                }
            }

            if (maxAlignment <= 0.0)
            {
                // Getting here means there is something wrong with the math.
                // Either there were no solutions to the quadratic equation,
                // or all solutions caused the refracted ray to bend 90 degrees
                // or more, which is not possible.
                throw new Exception("Refraction failure.");
            }

            // Determine the cosine of the exit angle.
            double cos_a2 = Math.Sqrt(1.0 - sin_a2*sin_a2);
            if (cos_a1 < 0.0)
            {
                // Tricky bit: the polarity of cos_a2 must
                // match that of cos_a1.
                cos_a2 = -cos_a2;
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
                cos_a1,
                cos_a2);

            double Rp = PolarizedReflection(
                sourceRefractiveIndex,
                targetRefractiveIndex,
                cos_a2,
                cos_a1);

            outReflectionFactor = (Rs + Rp) / 2.0;

            // Whatever fraction of the light is NOT reflected
            // goes into refraction.  The incoming ray intensity
            // is thus diminished by this fraction.
            Colour nextRayIntensity = (1.0 - outReflectionFactor) * rayIntensity;

            var ray = new Ray(intersection.HitPoint, refractDir);
            return TraceRay(ray, nextRayIntensity, targetRefractiveIndex, recursionDepth + 1, ray.Dir);
        }

        double PolarizedReflection(
            double n1,              // source material's index of refraction
            double n2,              // target material's index of refraction
            double cos_a1,          // incident or outgoing ray angle cosine
            double cos_a2)          // outgoing or incident ray angle cosine
        {
            double left  = n1 * cos_a1;
            double right = n2 * cos_a2;
            double numer = left - right;
            double denom = left + right;
            denom *= denom;     // square the denominator
            if (denom < MathLib.Epsilon)
            {
                // Assume complete reflection.
                return 1.0;
            }
            double reflection = (numer*numer) / denom;
            if (reflection > 1.0)
            {
                // Clamp to actual upper limit.
                return 1.0;
            }
            return reflection;
        }

        private Colour Shade(Vector3 hitPoint, Vector3 normal, Material material, Vector3 eyeDirection)
        {
            Colour colour;
            //= pObj.Material;
            Material lightMaterial = null;

            // first assign the emmissive part of the color as the base color
            colour = material.Emissive;

            // iterate through all the lights in the scene
            foreach (var light in _scene.Lights)
            {
                // get the light material
                Colour lightAmbient = light.Ambient;
                Colour lightDiffuse = light.Diffuse;

                // assign the ambient term of the light
                colour += (material.Ambient * lightAmbient);

                // construct a vector from the point to the light
                Vector3 pointToLight;
                double lightVecLen = 0.0f;
                pointToLight = light.Pos - hitPoint;

                // save the lenght of the vector.
                lightVecLen = pointToLight.GetLength();

                // normalise the vector
                pointToLight.Normalize();

                // get the angle between the light vector ad the surface normal
                double lightCos = Vector3.DotProduct(pointToLight, normal);

                // is this point shadowed
                bool shadowed = false;

                // if we are tracing shadows
                if (this._traceShadows && lightDiffuse.Sum() > 0)
                    shadowed = ShadowTrace(hitPoint, light.Pos, normal, lightVecLen);

                // if the angle is greater than 0.0 
                //then the light falls directly on the surface
                if (lightCos > 0.0f && !shadowed)
                {
                    // compute the diffuse term
                    colour += (material.Diffuse * lightDiffuse) * lightCos * (1 / lightVecLen * 100);

                    if (material.Specularity > 0.0f)
                    {
                        // calculate specular highlights
                        Vector3 vReflect = CalculateReflectedRay(pointToLight, normal);

                        // normalise the vector
                        vReflect.Normalize();

                        double fSpecular = Vector3.DotProduct(vReflect, eyeDirection);

                        if (fSpecular > 0.0f)
                        {
                            double power;
                            power = (double)Math.Pow(fSpecular, material.Specularity);
                            colour += lightDiffuse * material.SpecularExponent * power;
                        }
                    }
                }
            }

            return colour;
        }

        private bool ShadowTrace(Vector3 hitPoint, Vector3 lightPosition, Vector3 surfaceNormal, double lightDistance)
        {
            Vector3 dir = lightPosition - hitPoint;
            dir.Normalize();

            Ray ray = new Ray(hitPoint + (surfaceNormal * 0.00001f), dir);

            foreach (var obj in _scene.GetCandiates(ray))
            {
                var result = obj.Intersect(ray);

                if (result.T > 0f && result.T <= lightDistance && result.Result == HitResult.HIT)
                    return true;
            }

            return false;
        }

        private Vector3 CalculateReflectedRay(Vector3 dir, Vector3 normal)
        {
            Vector3 tmp = new Vector3();

            dir = -dir;
            dir.Normalize();
            normal.Normalize();

            tmp = (normal * (2.0f * Vector3.DotProduct(normal, dir))) - dir;
            tmp.Normalize();
            return tmp;
        }

        private Vector3 CalculateRefractedRay(Vector3 dir, Vector3 normal, double n_Out, double n_In)
        {
            double c = -Vector3.DotProduct(normal, dir);
            double n1 = n_Out;
            double n2 = n_In;
            double n = n1 / n2;
            return (n * dir) + (double)(n * c - Math.Sqrt(1 - n * n * (1 - c * c))) * normal;
        }        
    }
}
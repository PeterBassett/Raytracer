using System;
using System.Threading.Tasks;
using Raytracer.MathTypes;
using Raytracer.Rendering.Cameras;
using Raytracer.Rendering.Core;
using Raytracer.Rendering.FileTypes;
using Raytracer.Rendering.Materials;
using Raytracer.Rendering.PixelSamplers;

namespace Raytracer.Rendering.Renderers
{
    class RayTracingRenderer : IRenderer
    {
        private Scene _scene;
        private IPixelSampler _pixelSampler;
        private ICamera _camera;
        private bool _multiThreaded;
        private uint _maxDepth;
        private bool _traceReflections;
        private bool _traceRefractions;
        private bool _traceShadows;

        public RayTracingRenderer(Scene scene, ICamera camera, IPixelSampler pixelSampler,
            uint maxDepth, bool multiThreaded, 
            bool traceShadows, bool traceReflections, bool traceRefractions)
        {
            _scene = scene;
            _camera = camera;
            _pixelSampler = pixelSampler;
            _multiThreaded = multiThreaded;
            _maxDepth = maxDepth;
            _traceShadows = traceShadows;
            _traceReflections = traceReflections;
            _traceRefractions = traceRefractions;
        }

        public void RenderScene(IBmp frameBuffer)
        {
            var options = new ParallelOptions();
            if (!_multiThreaded)
                options.MaxDegreeOfParallelism = 1;

            Parallel.For(0, frameBuffer.Size.Width, options, (x) =>
            {
                for (int y = 0; y < frameBuffer.Size.Height; y++)                    
                    frameBuffer.SetPixel(x, y, _pixelSampler.SamplePixel(this, x, y));
            });
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
                    // calculate refraction
                    double rindex = material.Refraction;
                    double n = curRefractionIndex / rindex;
                    //Vector N = primitive.GetNormal(info.HitPoint) * (double)info.Result;

                    Vector3 N = info.NormalAtHitPoint * (double)info.Result;
                    double cosI = -Vector3.DotProduct(N, ray.Dir);
                    double cosT2 = 1.0f - n * n * (1.0f - cosI * cosI);
                    if (cosT2 > 0.0f)
                    {
                        Vector3 T = -((n * ray.Dir) + (double)(n * cosI - Math.Sqrt(cosT2)) * N);

                        Colour rcol = TraceRay(new Ray(info.HitPoint + T * EPSILON, T), colRefractiveAmount, rindex, depth + 1, eyeDirection);

                        //Raytrace( Ray( pi + T * EPSILON, T ), rcol, a_Depth + 1, rindex, dist );
                        // apply Beer's law
                        //Colour absorbance = material.Transmitted * -dist;
                        //Colour transparency = new Colour((double)Math.Exp(absorbance.Red), (double)Math.Exp(absorbance.Green), (double)Math.Exp(absorbance.Blue));
                        colour += rcol;// *transparency;
                    }
                }
            }            

            colour.Clamp();
            return colour;
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

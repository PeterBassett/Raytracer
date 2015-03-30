using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Raytracer.MathTypes;
using Raytracer.Rendering.FileTypes;
using System.Threading.Tasks;
using System.Threading;
using Raytracer.Rendering.Accellerators;
using Raytracer.Rendering.Primitives;
using Raytracer.Rendering.Materials;

namespace Raytracer.Rendering
{
    using Vector = Vector3;
    using Real = System.Double;

    class Scene
    {
        List<Light> _lights = new List<Light>();
        List<Traceable> _primitives = new List<Traceable>();
        Dictionary<string, Mesh> _meshes = new Dictionary<string, Mesh>();

        Dictionary<string, Material> _materials = new Dictionary<string, Material>();

        BVH m_SceneGraph = null;

        Real _nearWidth;
        Real _nearHeight;

        int _maxDepth;
        bool _traceReflections;
        bool _traceRefractions;
        bool _traceShadows;
        Material _defaultMaterial;

        public bool MultiThreaded { get; set; }

        public Vector Pos { get; set; }

        public Vector Dir { get; set; }

        public Real FieldOfView { get; set; }

        public int Width { get; set; }
        public int Height { get; set; }

        public Scene()
        {
            MultiThreaded = true;
            _maxDepth = 5;
            _traceReflections = true;
            _traceRefractions = true;
            _traceShadows = true;
            _defaultMaterial = new Material()
            {
                Ambient = new Colour(1f),
                Diffuse = new Colour(1f),
                Specularity = 20,
                SpecularExponent = 0.35f
            };
        }

        public int RecursionDepth
        {
            get { return _maxDepth; }
            set { _maxDepth = value; }
        }

        public bool TraceShadows
        {
            get { return _traceShadows; }
            set { _traceShadows = value; }
        }

        public bool TraceReflections
        {
            get { return _traceReflections; }
            set { _traceReflections = value; }
        }

        public bool TraceRefractions
        {
            get { return _traceRefractions; }
            set { _traceRefractions = value; }
        }

        public void AddLight(Light pLight)
        {
            _lights.Add(pLight);
        }

        public void AddObject(Traceable pObject)
        {
            if (pObject.Material == null)
                pObject.Material = this._defaultMaterial;

            _primitives.Add(pObject);
        }

        public void AddMaterial(Material mat, string strName)
        {
            if (mat.Name != strName)
                mat.Name = strName;

            _materials.Add(strName, mat);
        }

        public void AddMeshes(Mesh mesh, string strName)
        {
            if (mesh.Name != strName)
                mesh.Name = strName;

            _meshes.Add(strName, mesh);
        }

        public void BuildAccellerationStructures()
        {
            //return;

            if (_primitives.Count < 10)
                return;

            List<Traceable> elements = new List<Traceable>();
            int iIndex = _primitives.Count - 1;

            while (iIndex >= 0)
            {
                var prim = _primitives[iIndex];

                if (!prim.GetAABB().IsEmpty)
                {
                    elements.Add(prim);
                    _primitives.RemoveAt(iIndex);
                }

                iIndex--;
            }
            /*
            var firstElement = elements.First();
            var firstElementAABB = firstElement.GetAABB();
            AABB bounds = new AABB(firstElementAABB);

            foreach (var item in elements)
            {
                bounds.InflateToEncapsulate(item.GetAABB());
            }
            */
            m_SceneGraph = new BVH(elements);
            /*
            foreach (var item in elements)
            {
                m_SceneGraph.Add(item);
            }

            m_SceneGraph.PruneEmptyNodes();*/
        }

        public IEnumerable<Light> Lights
        {
            get
            {
                return this._lights;
            }
        }

        public IEnumerable<Material> Materials
        {
            get
            {
                return this._materials.Values;
            }
        }

        public IEnumerable<Traceable> Primitives
        {
            get
            {
                return this._primitives;
            }
        }

        public Material FindMaterial(string strMaterial)
        {
            if (_materials.ContainsKey(strMaterial))
                return _materials[strMaterial];
            else
                return null;
        }

        public long TraceScene(IBmp bmp)
        {
            _nearWidth = 2.0f * (Real)Math.Tan(MathLib.Deg2Rad(FieldOfView) / 2.0f);
            _nearHeight = _nearWidth * ((Real)Height) / ((Real)Width);

            bmp.Init(Width, Height);

            long TotalRays = 0;
            ParallelOptions options = new ParallelOptions();

            if (!this.MultiThreaded)
                options.MaxDegreeOfParallelism = 1;

            Parallel.For<int>(0, Width, options, () => 0, (lX, loop, TotalTraceRayCalls) =>
            {
                Colour col = new Colour(1.0f);
                for (int lY = 0; lY < Height; lY++)
                {
                    try
                    {
                        var dir = new Vector();
                        dir.X = ((Real)lX - Width / 2) * _nearWidth / (Real)Width;
                        dir.Y = ((Real)lY - Height / 2) * _nearHeight / (Real)Height;
                        dir.Z = 1;

                        dir.RotateX(Dir.X, ref dir);
                        dir.RotateY(Dir.Y, ref dir);
                        dir.RotateZ(Dir.Z, ref dir);
                        dir.Normalize();

                        Ray cRay = new Ray(Pos, dir);

                        bmp.SetPixel((int)lX, (int)lY, TraceRay(cRay, col, 1.0f, 1, cRay.Dir, ref TotalTraceRayCalls));
                    }
                    catch (Exception ex)
                    {
                        bmp.SetPixel((int)lX, (int)lY, new Colour(1, 0, 0));
                        Console.WriteLine(ex);
                    }
                }

                return TotalTraceRayCalls;

            }, (x) => Interlocked.Add(ref TotalRays, x));

            return TotalRays;
        }

        IntersectionInfo FindClosestIntersection(Ray ray)
        {
            IntersectionInfo minimumIntersection = new IntersectionInfo(HitResult.MISS);

            foreach (var obj in GetCandiates(ray))
            {
                var result = obj.Intersect(ray);

                if (result.T < minimumIntersection.T && result.Result != HitResult.MISS)
                {
                    minimumIntersection = result;
                }
            }

            return minimumIntersection;
        }

        private IEnumerable<Traceable> GetCandiates(Ray ray)
        {
            foreach (var prim in _primitives)
                yield return prim;

            if (m_SceneGraph != null)
                foreach (var item in m_SceneGraph.Intersect(ray))
                    yield return item;
        }

        Colour TraceRay(Ray cRay, Colour contribution, Real curRefractionIndex, long depth, Vector eyeDirection, ref int TotalCalls)
        {
            const Real EPSILON = 0.001f;

            TotalCalls++;

            Colour colour = new Colour(0.0f);

            IntersectionInfo info = FindClosestIntersection(cRay);

            if (info.Result != HitResult.MISS)
            {
                // set the 
                Material material = new Material();
                Material objectMaterial = info.Primitive.Material != null ? info.Primitive.Material : this._defaultMaterial;

                var materialDispatcher = new MaterialDispatcher();
                materialDispatcher.Solidify((dynamic)info.Primitive, (dynamic)objectMaterial, info, material);

                //objectMaterial.SolidifyMaterial(cInfo, material);

                // get the shading due to lighting at this point
                colour = Shade(info.HitPoint, info.NormalAtHitPoint, material, eyeDirection) * contribution;

                if (depth == _maxDepth)
                {
                    colour.Clamp();
                    return colour;
                }

                // if we are dealing with a reflective material
                if (material.Reflective.Sum() > 0.0f && TraceReflections)
                {
                    Colour colReflectAmount = material.Reflective * contribution;

                    if (colReflectAmount.Sum() > 0.01f)
                    {
                        // calculate the new reflected direction
                        Ray reflectedRay = new Ray(info.HitPoint, CalculateReflectedRay(cRay.Dir, info.NormalAtHitPoint));

                        // recursivly call trace ray
                        colour += TraceRay(reflectedRay, colReflectAmount, curRefractionIndex, depth + 1, eyeDirection, ref TotalCalls);
                    }
                }

                // if we are dealing with a refractive material
                if (material.Transmitted.Sum() > 0.0f && TraceRefractions)
                {
                    Colour colRefractiveAmount = material.Transmitted * contribution;

                    if (colRefractiveAmount.Sum() > 0.01f)
                    {
                        // calculate refraction
                        Real rindex = material.Refraction;
                        Real n = curRefractionIndex / rindex;
                        //Vector N = primitive.GetNormal(info.HitPoint) * (Real)info.Result;

                        Vector N = info.NormalAtHitPoint * (Real)info.Result;
                        Real cosI = -Vector.DotProduct(N, cRay.Dir);
                        Real cosT2 = 1.0f - n * n * (1.0f - cosI * cosI);
                        if (cosT2 > 0.0f)
                        {
                            Vector T = -((n * cRay.Dir) + (Real)(n * cosI - Math.Sqrt(cosT2)) * N);
                            
                            Colour rcol = TraceRay(new Ray(info.HitPoint + T * EPSILON, T), colRefractiveAmount, rindex, depth + 1, eyeDirection, ref TotalCalls);

                            //Raytrace( Ray( pi + T * EPSILON, T ), rcol, a_Depth + 1, rindex, dist );
                            // apply Beer's law
                            //Colour absorbance = material.Transmitted * -dist;
                            //Colour transparency = new Colour((Real)Math.Exp(absorbance.Red), (Real)Math.Exp(absorbance.Green), (Real)Math.Exp(absorbance.Blue));
                            colour += rcol;// *transparency;
                        }
                    }
                }
            }

            colour.Clamp();
            return colour;
        }

        Colour Shade(Vector hitPoint, Vector normal, Material material, Vector eyeDirection)
        {

            Colour colour;
            //= pObj.Material;
            Material lightMaterial = null;

            // first assign the emmissive part of the color as the base color
            colour = material.Emissive;

            // iterate through all the lights in the scene
            foreach (var light in _lights)
            {
                // get the light material
                Colour lightAmbient = light.Ambient;
                Colour lightDiffuse = light.Diffuse;

                // assign the ambient term of the light
                colour += (material.Ambient * lightAmbient);

                // construct a vector from the point to the light
                Vector pointToLight;
                Real lightVecLen = 0.0f;
                pointToLight = light.Pos - hitPoint;

                // save the lenght of the vector.
                lightVecLen = pointToLight.GetLength();

                // normalise the vector
                pointToLight.Normalize();

                // get the angle between the light vector ad the surface normal
                Real lightCos = Vector.DotProduct(pointToLight, normal);

                // is this point shadowed
                bool shadowed = false;

                // if we are tracing shadows
                if (this.TraceShadows && lightDiffuse.Sum() > 0)
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
                        Vector vReflect = CalculateReflectedRay(pointToLight, normal);

                        // normalise the vector
                        vReflect.Normalize();

                        Real fSpecular = Vector.DotProduct(vReflect, eyeDirection);

                        if (fSpecular > 0.0f)
                        {
                            Real power;
                            power = (Real)Math.Pow(fSpecular, material.Specularity);
                            colour += lightDiffuse * material.SpecularExponent * power;
                        }
                    }
                }
            }

            return colour;
        }

        bool ShadowTrace(Vector hitPoint, Vector lightPosition, Vector surfaceNormal, Real lightDistance)
        {
            Vector dir = lightPosition - hitPoint;
            dir.Normalize();

            Ray ray = new Ray(hitPoint + (surfaceNormal * 0.00001f), dir);

            foreach (var obj in GetCandiates(ray))
            {
                var result = obj.Intersect(ray);

                if (result.T > 0f && result.T <= lightDistance && result.Result == HitResult.HIT)
                    return true;
            }

            return false;
        }


        Vector CalculateReflectedRay(Vector dir, Vector normal)
        {
            Vector tmp = new Vector();

            dir = -dir;
            dir.Normalize();
            normal.Normalize();

            tmp = (normal * (2.0f * Vector.DotProduct(normal, dir))) - dir;
            tmp.Normalize();
            return tmp;
        }

        Vector CalculateRefractedRay(Vector dir, Vector normal, Real n_Out, Real n_In)
        {
            Real c = -Vector.DotProduct(normal, dir);
            Real n1 = n_Out;
            Real n2 = n_In;
            Real n = n1 / n2;
            return (n * dir) + (Real)(n * c - Math.Sqrt(1 - n * n * (1 - c * c))) * normal;
        }


        internal void LoadComplete()
        {
            BuildAccellerationStructures();
        }

        internal Mesh FindMesh(string meshName)
        {
            if (_meshes.ContainsKey(meshName))
                return _meshes[meshName];
            else
                return null;
        }
    }
}
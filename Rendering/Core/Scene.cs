﻿using System.Collections.Generic;
using Raytracer.MathTypes;
using Raytracer.Rendering.Accellerators;
using Raytracer.Rendering.BackgroundMaterials;
using Raytracer.Rendering.FileTypes;
using Raytracer.Rendering.Materials;
using Raytracer.Rendering.Primitives;
    
namespace Raytracer.Rendering.Core
{
    using Real = System.Double;
    using Vector = Vector3;
    
    class Scene
    {
        List<Light> _lights = new List<Light>();
        List<Traceable> _primitives = new List<Traceable>();
        Dictionary<string, Mesh> _meshes = new Dictionary<string, Mesh>();
        Dictionary<string, Material> _materials = new Dictionary<string, Material>();
        IAccelerator m_SceneGraph = null;        
        
        Real _nearWidth;
        Real _nearHeight;

        public IBackgroundMaterial BackgroundMaterial { get; set; }
        public Material DefaultMaterial { get; set; }
        
        public Vector EyePosition { get; set; }
        public Vector ViewPointRotation { get; set; }
        public Real FieldOfView { get; set; }

        private int _width;
        private int _height;
        
        public Scene()
        {
            DefaultMaterial = new Material()
            {
                Ambient = new Colour(1f),
                Diffuse = new Colour(1f),
                Specularity = 20,
                SpecularExponent = 0.35f
            };            
        }

        public int RecursionDepth
        {
            get;
            set;
        }

        public bool TraceShadows
        {
            get;
            set;
        }

        public bool TraceReflections
        {
            get;
            set;
        }

        public bool TraceRefractions
        {
            get;
            set;
        }

        public void AddLight(Light pLight)
        {
            _lights.Add(pLight);
        }

        public void AddObject(Traceable pObject)
        {
            if (pObject.Material == null)
                pObject.Material = this.DefaultMaterial;

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
            if (_primitives.Count < 10)
                return;

            var elements = FilterUnboundedPrimitives();

            m_SceneGraph = new AABBHierarchy();
            m_SceneGraph.Build(elements);
        }

        private List<Traceable> FilterUnboundedPrimitives()
        {
            var elements = new List<Traceable>();

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

            return elements;
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

        public IEnumerable<Traceable> GetCandiates(Ray ray)
        {
            foreach (var prim in this._primitives)
                yield return prim;

            if (m_SceneGraph != null)
                foreach (var prim in m_SceneGraph.Intersect(ray))
                    yield return prim;
        }

        public Traceable GetCandiates(Vector3 point)
        {
            foreach (var prim in this._primitives)
            {
                if (prim.Contains(point))
                    return prim;
            }

            if (m_SceneGraph != null)
            {
                foreach (var prim in m_SceneGraph.Intersect(point))
                {
                    if (prim.Contains(point))
                        return prim;
                }
            }

            return null;
        }

        /*
        public Colour Trace(Ray ray)
        {
            return TraceRay(ray, new Colour(1.0f), 1.0f, 1, ray.Dir);
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

        Colour TraceRay(Ray ray, Colour contribution, Real curRefractionIndex, long depth, Vector eyeDirection)
        {
            const Real EPSILON = 0.001f;

            Colour colour = new Colour(0.0f);

            IntersectionInfo info = FindClosestIntersection(ray);

            if (info.Result == HitResult.MISS)
            {
                if (this.BackgroundMaterial != null)
                    colour = this.BackgroundMaterial.Shade(ray);

                return colour;
            }
            
            // set the 
            Material material = new Material();
            Material objectMaterial = info.Primitive.Material != null ? info.Primitive.Material : this._defaultMaterial;

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
            if (material.Reflective.Sum() > 0.0f && TraceReflections)
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
                    Real cosI = -Vector.DotProduct(N, ray.Dir);
                    Real cosT2 = 1.0f - n * n * (1.0f - cosI * cosI);
                    if (cosT2 > 0.0f)
                    {
                        Vector T = -((n * ray.Dir) + (Real)(n * cosI - Math.Sqrt(cosT2)) * N);

                        Colour rcol = TraceRay(new Ray(info.HitPoint + T * EPSILON, T), colRefractiveAmount, rindex, depth + 1, eyeDirection);

                        //Raytrace( Ray( pi + T * EPSILON, T ), rcol, a_Depth + 1, rindex, dist );
                        // apply Beer's law
                        //Colour absorbance = material.Transmitted * -dist;
                        //Colour transparency = new Colour((Real)Math.Exp(absorbance.Red), (Real)Math.Exp(absorbance.Green), (Real)Math.Exp(absorbance.Blue));
                        colour += rcol;// *transparency;
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
        */
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
using System;
using System.Collections.Generic;
using Raytracer.MathTypes;
using Raytracer.Rendering.Accellerators;
using Raytracer.Rendering.BackgroundMaterials;
using Raytracer.Rendering.Materials;
using Raytracer.Rendering.Primitives;
using Raytracer.Rendering.Accellerators.Partitioners;
    
namespace Raytracer.Rendering.Core
{
    class Scene
    {
        private readonly List<Light> _lights = new List<Light>();
        private readonly List<Traceable> _primitives = new List<Traceable>();
        private readonly Dictionary<string, Mesh> _meshes = new Dictionary<string, Mesh>();
        private readonly Dictionary<string, Material> _materials = new Dictionary<string, Material>();
        private IAccelerator _sceneGraph;

        public IBackgroundMaterial BackgroundMaterial { get; set; }
        public Material DefaultMaterial { get; private set; }
        /*
        public Point EyePosition { get; set; }
        public Vector ViewPointRotation { get; set; }
        public double FieldOfView { get; set; }
        */
        public Scene()
        {
            DefaultMaterial = new Material
            {
                Ambient = new Colour(1f),
                Diffuse = new Colour(1f),
                Specularity = 20,
                SpecularExponent = 0.35f
            };            
        }
        /*
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
        */
        public void AddLight(Light light)
        {
            if (light == null)
                throw new ArgumentNullException("light");

            _lights.Add(light);
        }

        public void AddObject(Traceable primitive)
        {
            if (primitive == null)
                throw new ArgumentNullException("primitive");

            if (primitive.Material == null)
                primitive.Material = DefaultMaterial;

            _primitives.Add(primitive);
        }

        public void AddMaterial(Material mat, string strName)
        {
            if(mat == null)
                throw new ArgumentNullException("mat");

            mat.Name = strName;

            _materials.Add(strName, mat);
        }

        public void AddMeshes(Mesh mesh, string strName)
        {
            if (mesh == null)
                throw new ArgumentNullException("mesh");

            mesh.Name = strName;

            _meshes.Add(strName, mesh);
        }

        private void BuildAccellerationStructures()
        {
            if (_primitives.Count < 10)
                return;

            var elements = FilterUnboundedPrimitives();

            _sceneGraph = new AABBHierarchy(new SahMutliAxisPrimitivePartitioner());
            _sceneGraph.Build(elements);
        }

        private IEnumerable<Traceable> FilterUnboundedPrimitives()
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
                return _lights;
            }
        }

        public Material FindMaterial(string strMaterial)
        {
            if (strMaterial != null && _materials.ContainsKey(strMaterial))
                return _materials[strMaterial];
            
            return null;
        }

        public IEnumerable<Traceable> GetCandiates(Ray ray)
        {
            foreach (var prim in _primitives)
                yield return prim;

            if (_sceneGraph == null) 
                yield break;

            foreach (var prim in _sceneGraph.Intersect(ray))
                yield return prim;
        }

        public Traceable FindObjectContainingPoint(Point point)
        {
            foreach (var prim in _primitives)
            {
                if (prim.Contains(point))
                    return prim;
            }

            if (_sceneGraph == null) 
                return null;

            foreach (var prim in _sceneGraph.Intersect(point))
            {
                if (prim.Contains(point))
                    return prim;
            }

            return null;
        }

        internal void LoadComplete()
        {
            BuildAccellerationStructures();
        }

        internal Mesh FindMesh(string meshName)
        {
            return _meshes.ContainsKey(meshName) ? _meshes[meshName] : null;
        }
    }
}
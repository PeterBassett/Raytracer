using System.Collections.Generic;
using Raytracer.MathTypes;
using Raytracer.Rendering.Accellerators;
using Raytracer.Rendering.BackgroundMaterials;
using Raytracer.FileTypes;
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
        public Material DefaultMaterial { get; set; }
        /*
        public Point EyePosition { get; set; }
        public Vector ViewPointRotation { get; set; }
        public double FieldOfView { get; set; }
        */
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

            _sceneGraph = new AABBHierarchy(new SAHMutliAxisPrimitivePartitioner());
            _sceneGraph.Build(elements);
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
            if (strMaterial != null && _materials.ContainsKey(strMaterial))
                return _materials[strMaterial];
            else
                return null;
        }

        public IEnumerable<Traceable> GetCandiates(Ray ray)
        {
            foreach (var prim in this._primitives)
                yield return prim;

            if (_sceneGraph == null) 
                yield break;

            foreach (var prim in _sceneGraph.Intersect(ray))
                yield return prim;
        }

        public Traceable FindObjectContainingPoint(Point point)
        {
            foreach (var prim in this._primitives)
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
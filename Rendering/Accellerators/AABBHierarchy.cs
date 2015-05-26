using System;
using System.Collections.Generic;
using System.Linq;
using Raytracer.MathTypes;
using Raytracer.Rendering.Core;
using Raytracer.Rendering.Accellerators.Partitioners;

namespace Raytracer.Rendering.Accellerators
{
    // ReSharper disable InconsistentNaming

    class AABBHierarchy : IAccelerator
    {
        protected class AABBHierarchyNode
        {
            internal readonly int _axis;
            internal readonly bool _isLeaf;
            internal readonly AABBHierarchyNode _left;
            internal readonly AABBHierarchyNode _right;
            internal readonly AABB _bounds;
            internal readonly Traceable[] _primitives;

            public AABBHierarchyNode(IList<Traceable> primitives, int depth, IPrimitivePartitioner partitioner)
            {
                if (!primitives.Any())
                    return;

                var leftPrims = new List<Traceable>();
                var rightPrims = new List<Traceable>();
                int chosenSplitAxis;
                if (partitioner.Partition(primitives, depth, ref _bounds, ref leftPrims, ref rightPrims, out chosenSplitAxis))
                {
                    _axis = chosenSplitAxis;
                    _left = new AABBHierarchyNode(leftPrims.ToArray(), depth + 1, partitioner);
                    _right = new AABBHierarchyNode(rightPrims.ToArray(), depth + 1, partitioner);
                }
                else
                {                    
                    _primitives = primitives.ToArray();

                    _isLeaf = true;

                    _bounds = AABB.Invalid();
                    for (long i = 0; i < _primitives.Length; i++)
                        _bounds = _primitives[i].GetAABB().InflateToEncapsulate(_bounds);
                }
            }

            internal IEnumerable<Traceable> Intersect(Ray ray)
            {
                if (!_bounds.Intersect(ray))
                    return Enumerable.Empty<Traceable>();                    

                if(_isLeaf)
                   return _primitives;
                
                var traceableObjects = new List<Traceable>();
                traceableObjects.AddRange(_left.Intersect(ray));
                traceableObjects.AddRange(_right.Intersect(ray));
                return traceableObjects;                 
            }

            internal IEnumerable<Traceable> Intersect(Point point)
            {
                if (!_bounds.Contains(point))
                    return Enumerable.Empty<Traceable>();

                if (_isLeaf)
                    return _primitives;

                var traceableObjects = new List<Traceable>();
                traceableObjects.AddRange(_left.Intersect(point));
                traceableObjects.AddRange(_right.Intersect(point));
                return traceableObjects;  
            }
        };

        protected AABBHierarchyNode _root;
        private readonly IPrimitivePartitioner _partitioner;

        public AABBHierarchy (IPrimitivePartitioner partitioner)
	    {
            if(partitioner == null)
                throw new ArgumentNullException("partitioner");
            _partitioner = partitioner;       
	    }

        public virtual void Build(IEnumerable<Traceable> primitives)
        {
            _root = new AABBHierarchyNode(primitives.ToArray(), 0, _partitioner);
        }

        public virtual IEnumerable<Traceable> Intersect(Ray ray)
        {
            return _root.Intersect(ray);
        }

        public virtual IEnumerable<Traceable> Intersect(Point point)
        {
            return _root.Intersect(point);
        }        
    }
}
       
using System;
using System.Collections.Generic;
using System.Linq;
using Raytracer.MathTypes;
using Raytracer.Rendering.Core;
using Raytracer.Rendering.Primitives;
using Raytracer.Rendering.Accellerators.Partitioners;

namespace Raytracer.Rendering.Accellerators
{
    // ReSharper disable InconsistentNaming

    class AABBHierarchy : IAccelerator
    {
        class AABBHierarchyNode
        {
            readonly bool _isLeaf;
            readonly AABBHierarchyNode _left;
            readonly AABBHierarchyNode _right;
            readonly AABB _bounds;
            readonly Traceable[] _primitives;

            public AABBHierarchyNode(IList<Traceable> primitives, int depth, IPrimitivePartitioner partitioner)
            {
                if (!primitives.Any())
                    return;

                List<Traceable> leftPrims = new List<Traceable>();
                List<Traceable> rightPrims = new List<Traceable>();

                if (partitioner.Partition(primitives, depth, ref _bounds, ref leftPrims, ref rightPrims))
                {
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

        private AABBHierarchyNode _root;
        private IPrimitivePartitioner _partitioner;

        public AABBHierarchy (IPrimitivePartitioner partitioner)
	    {
            if(partitioner == null)
                throw new ArgumentNullException("partitioner");
            _partitioner = partitioner;       
	    }

        public void Build(IEnumerable<Traceable> primitives)
        {
            _root = new AABBHierarchyNode(primitives.ToArray(), 0, _partitioner);
        }

        public IEnumerable<Traceable> Intersect(Ray ray)
        {
            return _root.Intersect(ray);
        }

        public IEnumerable<Traceable> Intersect(Point point)
        {
            return _root.Intersect(point);
        }
    }
}
       
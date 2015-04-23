using System;
using System.Collections.Generic;
using System.Linq;
using Raytracer.MathTypes;
using Raytracer.Rendering.Core;
using Raytracer.Rendering.Primitives;

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

            public AABBHierarchyNode(IList<Traceable> primitives, int depth)
            {
                _isLeaf = false;

                _bounds = primitives.First().GetAABB();

                if (!primitives.Any())
                    return;

                if (depth > 25 || primitives.Count() <= 6)
                {
                    _primitives = primitives.ToArray();

                    _isLeaf = true;

                    for (long i = 1; i < _primitives.Length; i++)
                    {
                        _bounds = _primitives[i].GetAABB().InflateToEncapsulate(_bounds);
                    }

                    return;
                }

                var midpt = new Point3();

                var trisRecp = 1.0 / primitives.Count();

                for (var i = 1; i < primitives.Count; i++)
                {
                    _bounds = primitives[i].GetAABB().InflateToEncapsulate(_bounds);
                    midpt = midpt + (primitives[i].Pos * trisRecp);
                }

                var bestAxis = 0;
                var bestRemainder = int.MaxValue;
                var partition = new bool[primitives.Count * 3];

                for (var axis = 0; axis < 3; axis++)
                {
                    var rightCount = 0;

                    for (var i = 0; i < primitives.Count; i++)
                    {                        
                        if (midpt[axis] >= primitives[i].Pos[axis])
                        {
                            rightCount++;
                            partition[axis * primitives.Count + i] = true;
                        }
                    }

                    rightCount = Math.Max(rightCount, 1);

                    var remainder = Math.Abs((primitives.Count / 2) - rightCount);

                    if (remainder < bestRemainder)
                    {
                        bestAxis = axis;
                        bestRemainder = remainder;
                    }

                    if (remainder == 0)
                        break;
                }

                var leftTris = new List<Traceable>(primitives.Count / 2);
                var rightTris = new List<Traceable>(primitives.Count / 2);

                for (var i = 0; i < primitives.Count; i++)
                {
                    if (partition[bestAxis * primitives.Count + i])
                        rightTris.Add(primitives[i]);
                    else
                        leftTris.Add(primitives[i]);
                }

                if (primitives.Count == leftTris.Count || primitives.Count == rightTris.Count)
                {
                    _primitives = primitives.ToArray();

                    _isLeaf = true;

                    for (var i = 1; i < _primitives.Length; i++)
                    {
                        _bounds = _primitives[i].GetAABB().InflateToEncapsulate(_bounds);
                    }

                    return;
                }

                _left = new AABBHierarchyNode(leftTris.ToArray(), depth + 1);
                _right = new AABBHierarchyNode(rightTris.ToArray(), depth + 1);
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

            internal IEnumerable<Traceable> Intersect(Point3 point)
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

        public void Build(IEnumerable<Traceable> primitives)
        {
            _root = new AABBHierarchyNode(primitives.ToArray(), 0);
        }

        public IEnumerable<Traceable> Intersect(Ray ray)
        {
            return _root.Intersect(ray);
        }

        public IEnumerable<Traceable> Intersect(Point3 point)
        {
            return _root.Intersect(point);
        }
    }
}
       
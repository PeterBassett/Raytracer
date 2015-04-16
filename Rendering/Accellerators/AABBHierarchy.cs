using System;
using System.Collections.Generic;
using System.Linq;
using Raytracer.MathTypes;
using Raytracer.Rendering.Core;
using Raytracer.Rendering.Primitives;

namespace Raytracer.Rendering.Accellerators
{
    class AABBHierarchy : IAccelerator
    {
        class AABBHierarchyNode
        {
            bool isLeaf;
            AABBHierarchyNode left;
            AABBHierarchyNode right;
            AABB bounds;
            Traceable[] primitives;
            int depth;
           
            public AABBHierarchyNode(Traceable[] primitives, int depth)
            {
                this.depth = depth;
                isLeaf = false;

                bounds = primitives.First().GetAABB();

                if (!primitives.Any())
                    return;

                if (depth > 25 || primitives.Count() <= 6)
                {
                    this.primitives = primitives.ToArray();

                    isLeaf = true;

                    for (long i = 1; i < this.primitives.Length; i++)
                    {
                        bounds.InflateToEncapsulate(this.primitives[i].GetAABB());
                    }

                    return;
                }

                Vector3 midpt = new Vector3();

                double tris_recp = 1.0 / primitives.Count();

                for (long i = 1; i < primitives.Length; i++)
                {
                    bounds.InflateToEncapsulate(primitives[i].GetAABB());
                    midpt = midpt + (primitives[i].Pos * tris_recp);
                }

                int bestAxis = 0;
                int bestRemainder = int.MaxValue;
                var partition = new bool[primitives.Length * 3];

                for (int axis = 0; axis < 3; axis++)
                {
                    int rightCount = 0;

                    for (long i = 0; i < primitives.Length; i++)
                    {                        
                        if (midpt[axis] >= primitives[i].Pos[axis])
                        {
                            rightCount++;
                            partition[axis * primitives.Length + i] = true;
                        }
                    }

                    rightCount = Math.Max(rightCount, 1);

                    var remainder = Math.Abs((primitives.Length / 2) - rightCount);

                    if (remainder < bestRemainder)
                    {
                        bestAxis = axis;
                        bestRemainder = remainder;
                    }

                    if (remainder == 0)
                        break;
                }

                var left_tris = new List<Traceable>(primitives.Length / 2);
                var right_tris = new List<Traceable>(primitives.Length / 2);

                for (long i = 0; i < primitives.Length; i++)
                {
                    if (partition[bestAxis * primitives.Length + i])
                        right_tris.Add(primitives[i]);
                    else
                        left_tris.Add(primitives[i]);
                }

                if (primitives.Length == left_tris.Count || primitives.Length == right_tris.Count)
                {
                    this.primitives = primitives.ToArray();

                    isLeaf = true;

                    for (long i = 1; i < this.primitives.Length; i++)
                    {
                        bounds.InflateToEncapsulate(this.primitives[i].GetAABB());
                    }

                    return;
                }

                this.left = new AABBHierarchyNode(left_tris.ToArray(), depth + 1);
                this.right = new AABBHierarchyNode(right_tris.ToArray(), depth + 1);
            }

            public AABBHierarchyNode Left
            {
                get { return left; }
            }

            public AABBHierarchyNode Right
            {
                get { return right; }
            }

            public bool IsLeaf()
            {
                return isLeaf;
            }

            internal IEnumerable<Traceable> Intersect(Ray ray)
            {
                if (!this.bounds.Intersect(ray))
                    return Enumerable.Empty<Traceable>();                    

                if(isLeaf)
                   return this.primitives;
                
                List<Traceable> traceableObjects = new List<Traceable>();
                traceableObjects.AddRange(this.left.Intersect(ray));
                traceableObjects.AddRange(this.right.Intersect(ray));
                return traceableObjects;                 
            }

            internal IEnumerable<Traceable> Intersect(Vector3 point)
            {
                if (!this.bounds.Contains(point))
                    return Enumerable.Empty<Traceable>();

                if (isLeaf)
                    return this.primitives;

                List<Traceable> traceableObjects = new List<Traceable>();
                traceableObjects.AddRange(this.left.Intersect(point));
                traceableObjects.AddRange(this.right.Intersect(point));
                return traceableObjects;  
            }
        };

        private AABBHierarchyNode root;

        public void Build(IEnumerable<Traceable> primitives)
        {
            root = new AABBHierarchyNode(primitives.ToArray(), 0);
        }

        public IEnumerable<Traceable> Intersect(Ray ray)
        {
            return this.root.Intersect(ray);
        }

        public IEnumerable<Traceable> Intersect(Vector3 point)
        {
            return this.root.Intersect(point);
        }
    }
}
       
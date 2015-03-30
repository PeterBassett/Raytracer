using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Raytracer.Rendering.Primitives;

namespace Raytracer.Rendering.Accellerators
{
    using Vector = Raytracer.MathTypes.Vector3D;

    class BVH
    {

        class BVHNode
        {
            bool isLeaf;
            BVHNode left;
            BVHNode right;
            AABB bounds;
            Traceable[] primitives;
            int depth;
            /*
            public BVHNode(Traceable [] primitives, int depth)
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
                    
                    for (long i = 1; i < this.primitives.Length; i++) {
                        bounds.InflateToEncapsulate(this.primitives[i].GetAABB());
                    }

                    return;
                }

                Vector midpt = new Vector();

                double tris_recp = 1.0 / primitives.Count();

                for (long i = 1; i < primitives.Length; i++) {
                    bounds.InflateToEncapsulate(primitives[i].GetAABB());
                    midpt = midpt + (primitives[i].Pos * tris_recp);
                 }

                List<Traceable> left_tris = new List<Traceable>(primitives.Length / 2);
                List<Traceable> right_tris = new List<Traceable>(primitives.Length / 2);
                
                int axis = bounds.GetLongestAxis();
                 
                for (long i=0; i < primitives.Length; i++) 
                {
                    bool right = true;

                    switch (axis) 
                    {
                        case 0:

                            right = (midpt.X >= primitives[i].Pos.X);

                            break;
                        case 1:

                            right = (midpt.Y >= primitives[i].Pos.Y);
                            
                            break;
                        case 2:

                            right = (midpt.Z >= primitives[i].Pos.Z);                    
                            break;
                    }

                    if(right)                    
                        right_tris.Add(primitives[i]);
                    else
                        left_tris.Add(primitives[i]);        
                }

                if ( primitives.Length == left_tris.Count || primitives.Length == right_tris.Count) 
                {
                    this.primitives = primitives.ToArray();

                    isLeaf = true;
                    
                    for (long i = 1; i < this.primitives.Length; i++) {
                        bounds.InflateToEncapsulate(this.primitives[i].GetAABB());
                    }

                    return;
                }

                this.left = new BVHNode(left_tris.ToArray(), depth+1);
                this.right = new BVHNode(right_tris.ToArray(), depth + 1);
            }
            */

            /*public BVHNode(Traceable[] primitives, int depth)
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

                Vector midpt = new Vector();

                double tris_recp = 1.0 / primitives.Count();

                for (long i = 1; i < primitives.Length; i++)
                {
                    bounds.InflateToEncapsulate(primitives[i].GetAABB());
                    midpt = midpt + (primitives[i].Pos * tris_recp);
                }



                int bestAxis = 0;
                int bestRemainder = int.MaxValue;
                int bestRightCount = 0;
                var bestPartition = new bool[primitives.Length];
                var currentPartition = new bool [primitives.Length];

                for (int axis = 0; axis < 3; axis++)
                {
                    int rightCount = 0;

                    for (long i = 0; i < primitives.Length; i++)
                    {
                        if (midpt[axis] >= primitives[i].Pos[axis])
                            rightCount++;
                    }

                    rightCount = Math.Max(rightCount, 1);

                    var remainder = Math.Abs((primitives.Length / 2) - rightCount);

                    if (remainder < bestRemainder)
                    {
                        bestAxis = axis;
                        bestRemainder = remainder;
                        bestRightCount = rightCount;
                        bestPartition = currentPartition;
                    }

                    if (remainder == 0)
                        break;
                }

                var left_tris = new List<Traceable>(primitives.Length / 2);
                var right_tris = new List<Traceable>(primitives.Length / 2);

                for (long i = 0; i < primitives.Length; i++)
                {
                    if (midpt[bestAxis] >= primitives[i].Pos[bestAxis])
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

                this.left = new BVHNode(left_tris.ToArray(), depth + 1);
                this.right = new BVHNode(right_tris.ToArray(), depth + 1);
            }*/
            /*
            public BVHNode(Traceable[] primitives, int depth)
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

                Vector midpt = new Vector();

                double tris_recp = 1.0 / primitives.Count();

                for (long i = 1; i < primitives.Length; i++)
                {
                    bounds.InflateToEncapsulate(primitives[i].GetAABB());
                    midpt = midpt + (primitives[i].Pos * tris_recp);
                }

                int bestAxis = 0;
                int bestRemainder = int.MaxValue;
                var bestPartition = new bool[primitives.Length];
                var currentPartition = new bool[primitives.Length];

                for (int axis = 0; axis < 3; axis++)
                {
                    int rightCount = 0;

                    for (long i = 0; i < primitives.Length; i++)
                    {
                        currentPartition[i] = false;
                        if (midpt[axis] >= primitives[i].Pos[axis])
                        {
                            rightCount++;
                            currentPartition[i] = true;
                        }
                    }

                    rightCount = Math.Max(rightCount, 1);

                    var remainder = Math.Abs((primitives.Length / 2) - rightCount);

                    if (remainder < bestRemainder)
                    {
                        bestAxis = axis;
                        bestRemainder = remainder;

                        bestPartition = currentPartition;
                        currentPartition = new bool[primitives.Length];
                    }

                    if (remainder == 0)
                        break;
                }

                var left_tris = new List<Traceable>(primitives.Length / 2);
                var right_tris = new List<Traceable>(primitives.Length / 2);

                for (long i = 0; i < primitives.Length; i++)
                {
                    if (bestPartition[i])
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

                this.left = new BVHNode(left_tris.ToArray(), depth + 1);
                this.right = new BVHNode(right_tris.ToArray(), depth + 1);
            }
            */

            public BVHNode(Traceable[] primitives, int depth)
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

                Vector midpt = new Vector();

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

                this.left = new BVHNode(left_tris.ToArray(), depth + 1);
                this.right = new BVHNode(right_tris.ToArray(), depth + 1);
            }

            public BVHNode Left
            {
                get { return left; }
            }

            public BVHNode Right
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
        };

        private BVHNode root;

        public BVH(IEnumerable<Traceable> primitives)
        {
            root = new BVHNode(primitives.ToArray(), 0);
        }

        public IEnumerable<Traceable> Intersect(Ray ray)
        {
            return this.root.Intersect(ray);
        }
    }
}
       
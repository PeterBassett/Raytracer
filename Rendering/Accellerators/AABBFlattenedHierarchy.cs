using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Runtime.InteropServices;
using Raytracer.MathTypes;
using Raytracer.Rendering.Core;
using Raytracer.Rendering.Primitives;
using Raytracer.Rendering.Accellerators.Partitioners;

namespace Raytracer.Rendering.Accellerators
{
    // ReSharper disable InconsistentNaming

    class AABBFlattenedHierarchy : AABBHierarchy
    {                
        public AABBFlattenedHierarchy(IPrimitivePartitioner partitioner) : base(partitioner)
        {
        
        }

        private List<Traceable> _primitives;
        private LinearAABBNode[] _nodes;
        private int _totalNodeCount;
        public override void Build(IEnumerable<Traceable> primitives)
        {
            base.Build(primitives);

            _totalNodeCount = GetTotalNodeCount(_root);

            _nodes = new LinearAABBNode[_totalNodeCount];
            _primitives = new List<Traceable>();

            int offset = 0;
            FlattenAABBTree(_root, ref offset);

            _root = null;
        }
        
        private int GetTotalNodeCount(AABBHierarchyNode node)
        {
            if (node == null) return 0;

            return 1 + GetTotalNodeCount(node._left) + GetTotalNodeCount(node._right);
        }

        [StructLayout(LayoutKind.Explicit)]
        struct LinearAABBNode
        {            
            //union
            [FieldOffset(0)]
            public int PrimitivesOffset;
            [FieldOffset(0)]
            public int SecondChildOffset;

            [FieldOffset(4)]
            public int PrimitiveCount;
            [FieldOffset(8)]
            public int Axis;
            [FieldOffset(12)]
            public AABB Bounds;
        };

        int FlattenAABBTree(AABBHierarchyNode node, ref int offset)
        {
            int nodeIndex = offset;
            offset++;

            var linearNode = _nodes[nodeIndex];

            linearNode.Bounds = node._bounds;                                                        

            if (node._primitives != null && node._primitives.Length > 0) 
            {
                Debug.Assert(node._left == null && node._right == null);

                linearNode.PrimitivesOffset = _primitives.Count;
                _primitives.AddRange(node._primitives);
                linearNode.PrimitiveCount = node._primitives.Length;
            }
            else 
            {
                // Creater interior flattened BVH node
                linearNode.Axis = node._axis;
                linearNode.PrimitiveCount = 0;

                FlattenAABBTree(node._left, ref offset);

                linearNode.SecondChildOffset = FlattenAABBTree(node._right, ref offset);
            }

            _nodes[nodeIndex] = linearNode;

            return nodeIndex;
        }

        public override IEnumerable<Traceable> Intersect(Ray ray)
        {
            var result = new IntersectionInfo(HitResult.MISS);

            if (!_nodes.Any()) 
                return null;            

            var invDir = new Vector(1.0 / ray.Dir.X, 1.0 / ray.Dir.Y, 1.0 / ray.Dir.Z);
            bool [] dirIsNeg = { invDir.X < 0, invDir.Y < 0, invDir.Z < 0 };

            // Follow ray through nodes to find primitive intersections
            int stackIndex = 0;
            int nodeIndex = 0;
            var nodeStack = new int[128];

            while (true)
            {
                var node = _nodes[nodeIndex];

                ///////////////////////////////////////
                // Check ray against BVH node
                if (!node.Bounds.Intersect(ray))
                {
                        if (stackIndex == 0)
                            break;

                    stackIndex--;
                    nodeIndex = nodeStack[stackIndex];
                    continue;
                }

                //////////////////////////////////////////////////////////////////
                // if we dont have any primitives, find the next node to check.
                if (node.PrimitiveCount == 0)
                {
                    // Put far BVH node on _todo_ stack, advance to near node
                    if (dirIsNeg[node.Axis])
                    {
                        nodeIndex ++;
                        nodeStack[stackIndex] = nodeIndex;
                        stackIndex++;
                        nodeIndex = node.SecondChildOffset;
                    }
                    else 
                    {
                        nodeStack[stackIndex] = node.SecondChildOffset;
                        stackIndex++;
                        nodeIndex++;
                    }

                    continue;
                }

                ///////////////////////////////////////////////////
                // Intersect ray with primitives in leaf BVH node
                for (int i = 0; i < node.PrimitiveCount; ++i)
                {
                    var info = _primitives[node.PrimitivesOffset + i].Intersect(ray);
                    if (info.Result == HitResult.HIT)
                    {                                
                        if (info.T < result.T)
                            result = info;
                    }
                }
                        
                if (stackIndex == 0) 
                    break;

                stackIndex--;
                nodeIndex = nodeStack[stackIndex];                
            }

            if (result.Primitive != null)
                return new[] { result.Primitive };
            else
                return Enumerable.Empty<Traceable>();
        }
    }
}
       
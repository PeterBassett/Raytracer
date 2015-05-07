using System;
using System.Collections.Generic;
using System.Linq;
using Raytracer.MathTypes;
using Raytracer.Rendering.Core;
using Raytracer.Rendering.Primitives;

namespace Raytracer.Rendering.Accellerators.Partitioners
{
    class SAHPrimitivePartioner : IPrimitivePartitioner
    {
        struct Bucket
        {
            public int Count;
            public AABB Bounds;
        }

        private const int _maxPrimsInNode = 4;
        private const int DefaultBucketCount = 12;

        public bool Partition(IList<Traceable> primitives, int depth, ref AABB bounds, ref List<Traceable> leftPrims, ref List<Traceable> rightPrims)
        {
            System.Diagnostics.Debug.Assert(depth < 500);

            AABB centroidBounds = AABB.Invalid();

            for (var i = 0; i < primitives.Count; i++)
            {
                var primitivePos = primitives[i].GetAABB();
                centroidBounds = primitivePos.InflateToEncapsulate(centroidBounds);
            }

            bounds = centroidBounds;

            // Partition primitives using approximate SAH
            if (primitives.Count <= _maxPrimsInNode) 
            {
                return false;
            }
            else 
            {
                int dim = centroidBounds.MaximumExtent();

                var bucketCount = DefaultBucketCount;
                var buckets = new Bucket[bucketCount];

                for (int i = 0; i < buckets.Length; i++)                
                {
                    buckets[i].Count = 0;
                    buckets[i].Bounds = AABB.Invalid();
                }

                for (var i = 0; i < primitives.Count; i++)
                {
                    int bucket = (int)( bucketCount * ((primitives[i].GetAABB().Center[dim] - centroidBounds.Min[dim]) / (centroidBounds.Max[dim] - centroidBounds.Min[dim])) );
                    
                    if (bucket == bucketCount) 
                        bucket = bucketCount-1;
                    
                    buckets[bucket].Count++;
                    buckets[bucket].Bounds = buckets[bucket].Bounds.InflateToEncapsulate( primitives[i].GetAABB() );
                }

                // Compute costs for splitting after each bucket
                var cost = new double[bucketCount-1];

                for (int i = 0; i < bucketCount-1; ++i) 
                {
                    var b0 = AABB.Invalid();
                    var b1 = AABB.Invalid();

                    int count0 = 0;
                    int count1 = 0;

                    for (int j = 0; j <= i; ++j) 
                    {
                        if (buckets[j].Bounds.IsInvalid)
                            continue;

                        b0 = b0.InflateToEncapsulate(buckets[j].Bounds);
                        count0 += buckets[j].Count;
                    }

                    for (int j = i + 1; j < bucketCount; ++j) 
                    {
                        if (buckets[j].Bounds.IsInvalid)
                            continue;

                        b1 = b1.InflateToEncapsulate(buckets[j].Bounds);
                        count1 += buckets[j].Count;
                    }

                    cost[i] = 0.125 + (count0*b0.SurfaceArea() + count1*b1.SurfaceArea()) / centroidBounds.SurfaceArea();
                }

                // Find bucket to split at that minimizes SAH metric
                var minCost = cost[0];
                int minCostSplit = 0;
                for (int i = 1; i < bucketCount-1; ++i) {
                    if (cost[i] < minCost) {
                        minCost = cost[i];
                        minCostSplit = i;
                    }
                }

                 // Either create leaf or split primitives at selected SAH bucket
                if (primitives.Count < _maxPrimsInNode || minCost > primitives.Count)
                    return false;

                leftPrims = new List<Traceable>();
                rightPrims = new List<Traceable>();
                
                var CompareToBucket = GetComparer(minCostSplit, bucketCount, dim, centroidBounds);

                foreach (var prim in primitives)
                {
                    if (CompareToBucket(prim))
                        rightPrims.Add(prim);
                    else
                        leftPrims.Add(prim);
                }
                
                return true;
            }
        }

        public Func<Traceable, bool> GetComparer(int splitBucket, int nBuckets, int dim, AABB centroidBounds)
        {
            return (Traceable p) =>
            {
                int b = (int)(nBuckets * ((p.GetAABB().Center[dim] - centroidBounds.Min[dim]) / (centroidBounds.Max[dim] - centroidBounds.Min[dim])));
                if (b == nBuckets) b = nBuckets - 1;

                return b <= splitBucket;
            };
        }
    }
}

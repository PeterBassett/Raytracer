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

        private const int DefaultBucketCount = 12;
        private int _bucketCount;
        private Bucket[] _buckets;

        public SAHPrimitivePartioner()
	    {
            _bucketCount = DefaultBucketCount;
            _buckets = new Bucket[_bucketCount];
	    }

        public bool Partition(IList<Traceable> primitives, ref AABB bounds, ref List<Traceable> leftPrims, ref List<Traceable> rightPrims)
        {
            AABB centroidBounds = primitives[0].GetAABB();

            for (var i = 1; i < primitives.Count; i++)
            {
                var primitivePos = primitives[i].GetAABB();
                centroidBounds = primitivePos.InflateToEncapsulate(centroidBounds);
            }

            bounds = centroidBounds;

            // Partition primitives using approximate SAH
            if (primitives.Count <= 4) 
            {
                return false;
            }
            else 
            {
                int dim = centroidBounds.MaximumExtent();

                for (var i = 1; i < primitives.Count; i++)
                {
                    int bucket = (int)( _bucketCount * ((primitives[i].GetAABB().Center[dim] - centroidBounds.Min[dim]) / (centroidBounds.Max[dim] - centroidBounds.Min[dim])) );
                    
                    if (bucket == _bucketCount) 
                        bucket = _bucketCount-1;
                    
                    _buckets[bucket].Count++;
                    _buckets[bucket].Bounds = _buckets[bucket].Bounds.InflateToEncapsulate( primitives[i].GetAABB() );
                }

                // Compute costs for splitting after each bucket
                var cost = new double[_bucketCount-1];

                for (int i = 0; i < _bucketCount-1; ++i) 
                {
                    var b0 = AABB.Invalid();
                    var b1 = AABB.Invalid();

                    int count0 = 0;
                    int count1 = 0;

                    for (int j = 0; j <= i; ++j) 
                    {
                        b0 = b0.InflateToEncapsulate(_buckets[j].Bounds);
                        count0 += _buckets[j].Count;
                    }

                    for (int j = i + 1; j < _bucketCount; ++j) 
                    {
                        b1 = b1.InflateToEncapsulate(_buckets[j].Bounds);
                        count1 += _buckets[j].Count;
                    }

                    cost[i] = 0.125 + (count0*b0.SurfaceArea() + count1*b1.SurfaceArea()) / centroidBounds.SurfaceArea();
                }

                // Find bucket to split at that minimizes SAH metric
                var minCost = cost[0];
                int minCostSplit = 0;
                for (int i = 1; i < _bucketCount-1; ++i) {
                    if (cost[i] < minCost) {
                        minCost = cost[i];
                        minCostSplit = i;
                    }
                }
                
                leftPrims = new List<Traceable>();
                rightPrims = new List<Traceable>();
                
                var CompareToBucket = GetComparer(minCostSplit, _bucketCount, dim, centroidBounds);

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

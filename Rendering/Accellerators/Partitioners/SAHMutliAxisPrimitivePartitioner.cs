using System;
using System.Collections.Generic;
using System.Linq;
using Raytracer.MathTypes;
using Raytracer.Rendering.Core;
using Raytracer.Rendering.Primitives;

namespace Raytracer.Rendering.Accellerators.Partitioners
{
    class SAHMutliAxisPrimitivePartitioner : IPrimitivePartitioner
    {
        struct Bucket
        {
            public int Index;
            public int Count;
            public AABB Bounds;
            public double Cost;
        }

        private const int _maxPrimsInNode = 4;
        private const int DefaultBucketCount = 12;

        public bool Partition(IList<Traceable> primitives, int depth, ref AABB bounds, ref List<Traceable> leftPrims, ref List<Traceable> rightPrims, out int chosenSplitAxis)
        {
            System.Diagnostics.Debug.Assert(depth < 500);
            chosenSplitAxis = 0;

            if (!primitives.Any())
            {
                bounds = AABB.Empty;
                return false;
            }

            bounds = primitives[0].GetAABB();

            for (var i = 1; i < primitives.Count; i++)
            {
                var primitivePos = primitives[i].GetAABB();
                bounds = primitivePos.InflateToEncapsulate(bounds);
            }

            // Partition primitives using approximate SAH
            if (primitives.Count <= _maxPrimsInNode) 
            {
                return false;
            }
            else 
            {
                int minAxis;
                var bestPartition = FindBestBucketAndAxisPartition(primitives, bounds, out minAxis);

                chosenSplitAxis = minAxis;

                // Either create leaf or split primitives at selected SAH bucket
                if (primitives.Count < _maxPrimsInNode || bestPartition.Cost > primitives.Count)
                    return false;

                PartitionPrimitivesByMinimumBucket(bestPartition.Index, minAxis, primitives, ref bounds, ref leftPrims, ref rightPrims);

                return true;
            }
        }

        private Bucket FindBestBucketAndAxisPartition(IList<Traceable> primitives, AABB bounds, out int minAxis)
        {
            Bucket[] axisBestPartition = new Bucket[3];

            for (int axis = 0; axis < 3; axis++)
            {
                if (bounds.Max[axis] - bounds.Min[axis] > 0)
                    axisBestPartition[axis] = GetBestPartitionForAxis(primitives, bounds, axis);
                else
                    axisBestPartition[axis].Cost = int.MaxValue;
            }

            minAxis = 0;

            for (int axis = 1; axis < 3; axis++)
                if (axisBestPartition[axis].Cost < axisBestPartition[minAxis].Cost)
                    minAxis = axis;

            return axisBestPartition[minAxis];
        }

        private void PartitionPrimitivesByMinimumBucket(int splitBucketIndex, int minAxis, IList<Traceable> primitives, ref AABB bounds, ref List<Traceable> leftPrims, ref List<Traceable> rightPrims)
        {
            leftPrims = new List<Traceable>();
            rightPrims = new List<Traceable>();

            var CompareToBucket = GetComparer(splitBucketIndex, minAxis, bounds);

            foreach (var prim in primitives)
            {
                if (CompareToBucket(prim))
                    rightPrims.Add(prim);
                else
                    leftPrims.Add(prim);
            }
        }

        private Bucket GetBestPartitionForAxis(IList<Traceable> primitives, AABB bounds, int dim)
        {
            Bucket[] buckets = InitialiseBuckets();

            CategorisePrimitivesIntoBucketsByAxis(primitives, buckets, bounds, dim);

            ComputeBucketSAHCosts(ref bounds, buckets);

            return GetMinimumCostBucket(buckets);                
        }

        private Bucket GetMinimumCostBucket(Bucket[] buckets)
        {
            var min = buckets[0];

            // ignore the last bucket
            for (int i = 1; i < buckets.Length - 1; ++i)
                if (buckets[i].Cost < min.Cost)
                    min = buckets[i];

            return min;
        }

        private static void ComputeBucketSAHCosts(ref AABB bounds, Bucket[] buckets)
        {
            int bucketCount = buckets.Length;

            for (int i = 0; i < bucketCount - 1; ++i)
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

                buckets[i].Cost = 0.125 + (count0 * b0.SurfaceArea() + count1 * b1.SurfaceArea()) / bounds.SurfaceArea();
            }
        }

        private static void CategorisePrimitivesIntoBucketsByAxis(IList<Traceable> primitives, Bucket[] buckets, AABB bounds, int dimension)
        {
            var bucketCount = buckets.Length;

            for (var i = 0; i < primitives.Count; i++)
            {
                int bucket = (int)(bucketCount * ((primitives[i].GetAABB().Center[dimension] - bounds.Min[dimension]) / (bounds.Max[dimension] - bounds.Min[dimension])));

                if (bucket == bucketCount)
                    bucket = bucketCount - 1;

                buckets[bucket].Count++;
                buckets[bucket].Bounds = buckets[bucket].Bounds.InflateToEncapsulate(primitives[i].GetAABB());
            }
        }

        private static Bucket[] InitialiseBuckets()
        {
            var buckets = new Bucket[DefaultBucketCount];

            for (int i = 0; i < buckets.Length; i++)
            {
                buckets[i].Index = i;
                buckets[i].Count = 0;
                buckets[i].Bounds = AABB.Invalid();
            }

            return buckets;
        }

        public Func<Traceable, bool> GetComparer(int splitByBucketIndex, int axis, AABB bounds)
        {
            return (Traceable p) =>
            {
                int bucket = (int)(DefaultBucketCount * ((p.GetAABB().Center[axis] - bounds.Min[axis]) / (bounds.Max[axis] - bounds.Min[axis])));

                if (bucket == DefaultBucketCount)
                    bucket = DefaultBucketCount - 1;

                return bucket <= splitByBucketIndex;
            };
        }
    }
}

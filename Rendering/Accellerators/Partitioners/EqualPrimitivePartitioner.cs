﻿using System;
using System.Collections.Generic;
using System.Linq;
using Raytracer.MathTypes;
using Raytracer.Rendering.Core;

namespace Raytracer.Rendering.Accellerators.Partitioners
{
    class EqualPrimitivePartioner : IPrimitivePartitioner
    {
        public bool Partition(IList<Traceable> primitives, int depth, ref AABB bounds, ref List<Traceable> leftPrims, ref List<Traceable> rightPrims, out int chosenSplitAxis)
        {
            chosenSplitAxis = 0;
            if (!primitives.Any())
            {
                bounds = AABB.Empty;
                return false;
            }

            bounds = primitives.First().GetAABB();

            var midpt = new Point();

            var trisRecp = 1.0 / primitives.Count();

            for (var i = 1; i < primitives.Count; i++)
            {
                var primitivePos = primitives[i].GetAABB();
                bounds = primitivePos.InflateToEncapsulate(bounds);
                midpt = midpt + (primitivePos.Center * trisRecp);
            }

            if (primitives.Count() <= 6)
                return false;

            if (depth > 25)
                return false;

            var bestAxis = 0;
            var bestRemainder = int.MaxValue;
            var partition = new bool[primitives.Count * 3];

            for (var axis = 0; axis < 3; axis++)
            {
                var rightCount = 0;

                for (var i = 0; i < primitives.Count; i++)
                {
                    var primitivePos = primitives[i].GetAABB().Center;

                    if (midpt[axis] >= primitivePos[axis])
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

            leftPrims = new List<Traceable>(primitives.Count / 2);
            rightPrims = new List<Traceable>(primitives.Count / 2);

            for (var i = 0; i < primitives.Count; i++)
            {
                if (partition[bestAxis * primitives.Count + i])
                    rightPrims.Add(primitives[i]);
                else
                    leftPrims.Add(primitives[i]);
            }

            chosenSplitAxis = bestAxis;

            if (primitives.Count == leftPrims.Count || primitives.Count == rightPrims.Count)
                return false;

            return true;
        }
    }
}

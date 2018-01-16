using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Raytracer.MathTypes;

namespace Raytracer.Rendering.Distributions
{
    abstract class Distribution
    {
        private static ThreadLocal<Random> _rnd;

        static Distribution()
        {
            _rnd = new ThreadLocal<Random>( () => new Random() );
        }

        public static double GetNextRandom()
        {
            return _rnd.Value.NextDouble();
        }

        public static double GetNextRandom(double minimum, double maximum)
        {
            return _rnd.Value.NextDouble() * (maximum - minimum) + minimum;
        }

        public abstract Vector2[] TwoD(uint samples, double x1, double y1, double x2, double y2);
    }
}
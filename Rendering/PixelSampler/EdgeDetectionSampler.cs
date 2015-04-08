using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Raytracer.Rendering.FileTypes;
using Raytracer.MathTypes;
using System.Collections.Concurrent;

namespace Raytracer.Rendering.PixelSamplers
{
    class EdgeDetectionSampler : JitteredPixelSampler
    {
        private ConcurrentDictionary<Tuple<int, int>, Colour> _bmp;
        private Size _dimensions;         
        private bool _renderEdgeDetectionResults;

        public EdgeDetectionSampler(uint subSamplingLevel, bool renderEdgeDetectionResults, Size dimensions) : base(subSamplingLevel)
        {
            _renderEdgeDetectionResults = renderEdgeDetectionResults;
            _dimensions = dimensions;
            _bmp = new ConcurrentDictionary<Tuple<int, int>, Colour>();
        }

        public override Colour SamplePixel(Renderer renderer, int x, int y)
        {
            if (_samples == 1)
                return renderer.ComputeSample(new Vector2(x, y));

            var difference = SobelOperator(renderer, x, y);
            var edgeFound = difference > 0.5;
 
            if (_renderEdgeDetectionResults)
            {
                return edgeFound ? new Colour(1, 0, 0) : new Colour(0, 0, 0);
            }
            else
            {
                if (edgeFound)
                    return base.SamplePixel(renderer, x, y);                 
                else
                    return GetPixel(renderer, x, y);
            }
        }

        private float SobelOperator(Renderer renderer, int x, int y)
        {
            var p1 = GetPixel(renderer, x - 1,  y - 1   ).Brightness;
            var p2 = GetPixel(renderer, x,      y - 1   ).Brightness;
            var p3 = GetPixel(renderer, x + 1,  y - 1   ).Brightness;
            var p4 = GetPixel(renderer, x - 1,  y       ).Brightness;
            var p5 = GetPixel(renderer, x,      y       ).Brightness;
            var p6 = GetPixel(renderer, x + 1,  y       ).Brightness;
            var p7 = GetPixel(renderer, x - 1,  y + 1   ).Brightness;
            var p8 = GetPixel(renderer, x,      y + 1   ).Brightness;
            var p9 = GetPixel(renderer, x + 1,  y + 1   ).Brightness;

            return Math.Abs((p1 + 2 * p2 + p3) - (p7 + 2 * p8 + p9))
                 + Math.Abs((p3 + 2 * p6 + p9) - (p1 + 2 * p4 + p7));
        }

        private Colour GetPixel(Renderer renderer, int x, int y)
        {
            ClampCoordinatesInBounds(ref x, ref y);

            var key = new Tuple<int,int>(x, y);

            Colour colour;

            if (_bmp.TryGetValue(key, out colour))
                return colour;

            colour = renderer.ComputeSample(new Vector2(x, y));

            _bmp.TryAdd(key, colour);
        
            return colour;
        }

        private void ClampCoordinatesInBounds(ref int x, ref int y)
        {
            if (x < 0)
                x = 0;
            if (x > _dimensions.Width - 1)
                x = _dimensions.Width - 1;

            if (y < 0)
                y = 0;
            if (y > _dimensions.Height - 1)
                y = _dimensions.Height - 1;
        }
    }
}

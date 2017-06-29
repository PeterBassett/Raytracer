using System;
using System.Collections.Concurrent;
using Raytracer.MathTypes;
using Raytracer.Rendering.Renderers;
using Raytracer.Rendering.Core;

namespace Raytracer.Rendering.PixelSamplers
{
    class EdgeDetectionSampler : JitteredPixelSampler
    {
        private ConcurrentDictionary<Tuple<int, int>, Colour> _bmp;
        private Size _dimensions;         
        private bool _renderEdgeDetectionResults;

        public EdgeDetectionSampler(uint subSamplingLevel, bool renderEdgeDetectionResults) : base(subSamplingLevel)
        {
            _renderEdgeDetectionResults = renderEdgeDetectionResults;
            _bmp = new ConcurrentDictionary<Tuple<int, int>, Colour>();
        }

        public override void SamplePixel(IRenderer renderer, int x, int y, Raytracer.Rendering.Core.Buffer buffer)
        {
            if (_samples == 1)
            {
                buffer.AddSample(x, y, GetPixel(renderer, x, y));
                return;
            }

            _dimensions = renderer.Camera.OutputDimensions;
            var difference = SobelOperator(renderer, x, y);
            var edgeFound = difference > 0.5;
 
            if (_renderEdgeDetectionResults)
            {
                buffer.AddSample(x, y, edgeFound ? new Colour(1, 0, 0) : new Colour(0, 0, 0));
            }
            else
            {
                if (edgeFound)
                    base.SamplePixel(renderer, x, y, buffer);                 
                else
                    buffer.AddSample(x, y, GetPixel(renderer, x, y));
            }
        }

        protected virtual float SobelOperator(IRenderer renderer, int x, int y)
        {
            var p1 = GetPixel(renderer, x - 1,  y - 1).Brightness;
            var p2 = GetPixel(renderer, x,      y - 1).Brightness;
            var p3 = GetPixel(renderer, x + 1,  y - 1).Brightness;
            var p4 = GetPixel(renderer, x - 1,  y    ).Brightness;
            var p5 = GetPixel(renderer, x,      y    ).Brightness;
            var p6 = GetPixel(renderer, x + 1,  y    ).Brightness;
            var p7 = GetPixel(renderer, x - 1,  y + 1).Brightness;
            var p8 = GetPixel(renderer, x,      y + 1).Brightness;
            var p9 = GetPixel(renderer, x + 1,  y + 1).Brightness;

            return SobolOperator(p1, p2, p3, p4, p6, p7, p8, p9);
        }

        protected virtual float SobolOperator(float p1, float p2, float p3, float p4, float p6, float p7, float p8, float p9)
        {
            return Math.Abs((p1 + 2 * p2 + p3) - (p7 + 2 * p8 + p9))
                 + Math.Abs((p3 + 2 * p6 + p9) - (p1 + 2 * p4 + p7));
        }

        protected Colour GetPixel(IRenderer renderer, int x, int y)
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

        public override void Initialise()
        {
            _bmp = new ConcurrentDictionary<Tuple<int, int>, Colour>();
        }
    }
}

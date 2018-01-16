using Raytracer.Rendering.Distributions;
using Raytracer.Rendering.Renderers;

namespace Raytracer.Rendering.PixelSamplers
{
    class EdgeDetectionPerComponentSampler : EdgeDetectionSampler
    {
        public EdgeDetectionPerComponentSampler(Distribution distribution, uint subSamplingLevel, bool renderEdgeDetectionResults)
            : base(distribution, subSamplingLevel, renderEdgeDetectionResults)
        {
        }

        protected override float SobelOperator(IRenderer renderer, int x, int y)
        {
            var p1 = GetPixel(renderer, x - 1,  y - 1);
            var p2 = GetPixel(renderer, x,      y - 1);
            var p3 = GetPixel(renderer, x + 1,  y - 1);
            var p4 = GetPixel(renderer, x - 1,  y    );
            var p5 = GetPixel(renderer, x,      y    );
            var p6 = GetPixel(renderer, x + 1,  y    );
            var p7 = GetPixel(renderer, x - 1,  y + 1);
            var p8 = GetPixel(renderer, x,      y + 1);
            var p9 = GetPixel(renderer, x + 1,  y + 1);

            float total = 0;

            for (int i = 0; i < 3; i++)
			{
                total += SobolOperator(p1[i], p2[i], p3[i], p4[i], p6[i], p7[i], p8[i], p9[i]);
			}

            return total;
        }
    }
}

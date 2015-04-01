using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Raytracer.Rendering.FileTypes;

namespace Raytracer.Rendering.Antialiasing
{
    class EdgeDetectionResampling : IAntialiaser
    {
        private HashSet<Tuple<int, int>> _rescanCoordinates;
        private IBmp _bmp;

        private uint _subSamplingLevel;
        private bool _renderEdgeDetectionResults;

        public EdgeDetectionResampling(uint subSamplingLevel, bool renderEdgeDetectionResults)
        {
            _subSamplingLevel = subSamplingLevel;
            _renderEdgeDetectionResults = renderEdgeDetectionResults;
        }

        public void Anitalias(Scene scene, IBmp bmp)
        {
            _rescanCoordinates = new HashSet<Tuple<int, int>>();
            _bmp = bmp;

            DetectStrongEdges(bmp);

            if (_renderEdgeDetectionResults)
                ClearBitmap(bmp);

            ResampleEdgePixels(scene, bmp);
        }

        private void DetectStrongEdges(IBmp bmp)
        {
            for (int x = 0; x < _bmp.Width; x++)
            {
                for (int y = 0; y < _bmp.Height; y++)
                {
                    var difference = SobelOperator(bmp, x, y);

                    if (difference > 0.5)
                        MarkPixelToRescan(x, y);
                }
            }
        }

        private void ClearBitmap(IBmp bmp)
        {
            var black = new Colour(0, 0, 0);
            for (int x = 0; x < _bmp.Width; x++)
            {
                for (int y = 0; y < _bmp.Height; y++)
                {
                    bmp.SetPixel(x, y, black);
                }
            }
        }

        private void ResampleEdgePixels(Scene scene, IBmp bmp)
        {
            var red = new Colour(1, 0, 0);
            foreach (var coordinate in _rescanCoordinates)
            {
                if (_renderEdgeDetectionResults)
                    bmp.SetPixel(coordinate.Item1, coordinate.Item2, red);
                else
                    scene.TracePixel(bmp, coordinate.Item1, coordinate.Item2, _subSamplingLevel);
            }
        }

        private float SobelOperator(IBmp bmp, int x, int y)
        {
            var p1 = GetPixel(x - 1, y - 1).Brightness;
            var p2 = GetPixel(x, y - 1).Brightness;
            var p3 = GetPixel(x + 1, y - 1).Brightness;
            var p4 = GetPixel(x - 1, y).Brightness;
            var p5 = GetPixel(x, y).Brightness;
            var p6 = GetPixel(x + 1, y).Brightness;
            var p7 = GetPixel(x - 1, y + 1).Brightness;
            var p8 = GetPixel(x, y + 1).Brightness;
            var p9 = GetPixel(x + 1, y + 1).Brightness;

            return Math.Abs((p1 + 2 * p2 + p3) - (p7 + 2 * p8 + p9))
                 + Math.Abs((p3 + 2 * p6 + p9) - (p1 + 2 * p4 + p7));            
        }

        private Colour GetPixel(int x, int y)
        {
            ClampCoordinatesInBounds(ref x, ref y);

            return _bmp.GetPixel(x, y);
        }
        
        private void MarkPixelToRescan(int x, int y)
        {
            ClampCoordinatesInBounds(ref x, ref y);

            _rescanCoordinates.Add(new Tuple<int, int>(x, y));
        }

        private void ClampCoordinatesInBounds(ref int x, ref int y)
        {
            if (x < 0)
                x = 0;
            if (x > _bmp.Width - 1)
                x = _bmp.Width - 1;

            if (y < 0)
                y = 0;
            if (y > _bmp.Height - 1)
                y = _bmp.Height - 1;
        }
    }
}

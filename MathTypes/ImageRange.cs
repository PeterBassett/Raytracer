using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Raytracer.MathTypes
{
    public struct ImageRange
    {
        public ImageRange(int id, int x1, int x2, int y1, int y2) : this()
        {
            ID = id;
            X1 = x1;
            Y1 = y1;
            X2 = x2;
            Y2 = y2;
        }

        public int ID { get; private set; }
        public int X1 { get; private set; }
        public int Y1 { get; private set; }
        public int X2 { get; private set; }
        public int Y2 { get; private set; }
        public int Pixels { get { return (X2 - X1) * (Y2 - Y1); } }
    }
}

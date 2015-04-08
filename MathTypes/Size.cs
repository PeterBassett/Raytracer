using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Raytracer.MathTypes
{
    public struct Size
    {
        public Size(uint width, uint height)
            : this((int)width, (int)height)
        {
        }

        public Size(int width, int height) : this()
        {
            if (width < 0)
                throw new ArgumentOutOfRangeException("width", "width must be positive");

            if (height < 0)
                throw new ArgumentOutOfRangeException("height", "height must be positive");

            this.Width = width;
            this.Height = height;
        }        

        public int Width { get; private set; }
        public int Height { get; private set; }
    }
}

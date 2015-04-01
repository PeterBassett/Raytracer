using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Raytracer.Rendering.Antialiasing
{
    interface IAntialiaser
    {
        void Anitalias(Scene scene, IBmp bmp);
    }
}

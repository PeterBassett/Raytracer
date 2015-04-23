using Raytracer.Rendering.FileTypes;
using Raytracer.MathTypes;

namespace Raytracer.Rendering
{
    class Light
    {
        public Light()
        {
            Ambient = new Colour();
            Diffuse = new Colour();
            Pos = new Point3(0.0f, 0.0f, 0.0f);
        }

        public Colour Ambient { get; set; }
        public Colour Diffuse { get; set; }
        public Point3 Pos { get; set; }
    }
}

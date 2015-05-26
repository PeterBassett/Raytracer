using Raytracer.MathTypes;

namespace Raytracer.Rendering.Core
{
    interface IBmp
    {
        Colour GetPixel(int lX, int lY);
        void SetPixel(int lX, int lY, Colour colour);
        Size Size { get; }
        void BeginWriting();
        void EndWriting();
    }
}

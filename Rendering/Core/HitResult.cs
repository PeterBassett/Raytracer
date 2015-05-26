namespace Raytracer.Rendering.Core
{
    public enum HitResult
    {
        Miss = 0, // Ray missed primitive
        Hit = 1, // Ray hit primitive
        InPrim = -1 // Ray started inside primitive        
    }
}

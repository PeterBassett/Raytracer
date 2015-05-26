namespace Raytracer.Rendering.Renderers
{
    public class RenderSettings
    {       
        public int PathDepth { get; set; }

        public bool TraceShadows { get; set; }

        public bool TraceReflections { get; set; }

        public bool TraceRefractions { get; set; }

        public bool MultiThreaded { get; set; }
    }
}

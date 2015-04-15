using System.Threading.Tasks;
using Raytracer.Rendering.Core;
using Raytracer.Rendering.PixelSamplers;
using Raytracer.Rendering.Renderers;
using System.Threading;

namespace Raytracer.Rendering.RenderingStrategies
{
    class ParallelOptionsBase
    {
        protected bool _multiThreaded;
        protected CancellationToken _cancellationToken;

        public ParallelOptionsBase(bool multiThreaded, CancellationToken cancellationToken)
        {
            _multiThreaded = multiThreaded;
            _cancellationToken = cancellationToken;
        }

        protected ParallelOptions GetThreadingOptions()
        {
            var options = new ParallelOptions()
            {
                MaxDegreeOfParallelism = 1
            };            
            
            if (_multiThreaded)
                options.MaxDegreeOfParallelism = System.Environment.ProcessorCount;
            
            return options;
        }
    }
}

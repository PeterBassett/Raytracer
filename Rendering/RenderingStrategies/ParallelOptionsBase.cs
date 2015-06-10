using System.Threading.Tasks;
using Raytracer.Rendering.Synchronisation;

namespace Raytracer.Rendering.RenderingStrategies
{
    class ParallelOptionsBase
    {
        protected readonly bool _multiThreaded;
        protected readonly CancellationToken _cancellationToken;

        public delegate void CompletedPercentageDelta(double percentageDelta);
        public event CompletedPercentageDelta OnCompletedPercentageDelta;

        public delegate void RenderingStarted();
        public event RenderingStarted OnRenderingStarted;

        public delegate void RenderingComplete();
        public event RenderingComplete OnRenderingComplete;

        protected ParallelOptionsBase(bool multiThreaded, CancellationToken cancellationToken)
        {
            _multiThreaded = multiThreaded;
            _cancellationToken = cancellationToken;
        }

        protected ParallelOptions GetThreadingOptions()
        {
            var options = new ParallelOptions
            {
                MaxDegreeOfParallelism = 1
            };            
            
            if (_multiThreaded)
                options.MaxDegreeOfParallelism = System.Environment.ProcessorCount;
            
            return options;
        }

        protected void RaiseOnCompletedPercentageDelta(double percentageDelta)
        {
            var handler = OnCompletedPercentageDelta;

            if (handler != null)
                handler(percentageDelta);
        }

        protected void RaiseRenderingStarted()
        {
            var handler = OnRenderingStarted;

            if (handler != null)
                handler();
        }

        protected void RaiseRenderingComplete()
        {
            var handler = OnRenderingComplete;

            if (handler != null)
                handler();
        }        
    }
}

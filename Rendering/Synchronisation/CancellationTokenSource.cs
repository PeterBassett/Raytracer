using System.Threading;

namespace Raytracer.Rendering.Synchronisation
{
    class CancellationTokenSource
    {
        private readonly ManualResetEventSlim _event;

        public CancellationTokenSource()
        {
            _event = new ManualResetEventSlim();
        }

        internal bool IsCancellationRequested
        {
            get { return _event.IsSet; }
        }

        public CancellationToken Token
        {
            get
            {
                return new CancellationToken(this);
            }
        }

        public void Cancel()
        {
            if (IsCancellationRequested)
                return;

            _event.Set();
        }

        public void Reset()
        {
            if (!IsCancellationRequested)
                return;

            _event.Reset();
        }
    }
}

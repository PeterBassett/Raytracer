namespace Raytracer.Rendering.Synchronisation
{
    internal class CancellationToken
    {
        private readonly CancellationTokenSource _source;
        public CancellationToken(CancellationTokenSource source)
        {
            _source = source;
        }

        public bool IsCancellationRequested
        {
            get { return _source.IsCancellationRequested; }
        }
    }
}

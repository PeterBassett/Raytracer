using System;
namespace Raytracer.Rendering.Core
{
    public enum Channel
    {
        ColorChannel,
        VarianceChannel,
        StandardDeviationChannel,
        SamplesChannel
    }

    interface IBuffer
    {
        void AddSample(int x, int y, Colour sample);
        Colour Colour(int x, int y);
        int Samples(int x, int y);
        Raytracer.MathTypes.Size Size { get; }
        Colour StandardDeviation(int x, int y);
        Colour Variance(int x, int y);
        void WriteToBmp(Channel channel, IBmp image);
        void BeginWriting();
        void EndWriting();        
    }
}

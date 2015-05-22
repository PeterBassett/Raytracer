﻿using System;
using System.IO;
using Raytracer.Rendering.Core;

namespace Raytracer.FileTypes
{
    interface ISceneLoader : IDisposable
    {
        bool CanLoadStream(Stream sceneStream);
        Scene LoadScene(Stream sceneStream);
    }
}

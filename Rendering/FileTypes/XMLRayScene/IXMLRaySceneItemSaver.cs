using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Raytracer.Rendering.FileTypes.XMLRayScene
{
    interface IXMLRaySceneItemSaver
    {
        Type SaverForType { get; }
        void SaveObject(System.IO.StreamWriter file, object ObjectToSave);
    }
}

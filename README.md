# Raytracer

A faily traditional raytracer.

Functionality implemented so far.

Primitives
* Spheres
* Triangles
* Planes
* Meshes loaded from wavefront obj files. Supports mtl files.
* Tori (Toruses, Donuts....Mmm Donuts!)

Effects
* Reflections
* Refractions

Various Materials
* Plain RGB materials defined with Ambient, Diffuse etc etc etc.
* Checker Board of two submaterials.
* Perlin Noise mix of two submaterials.
* Texture material. UV coordinate calculations are specific to each primitive type. All common image file formats are supported via the .net framework.
* Cubemaps, both vertical and horizontal crosses, are supported for scene backgrounds. When a ray misses all objects inthe scene we render from the cubemap.

File Types
* Home made human readable "ray" files.
* Wavefront obj files.
* Wavefront mtl files.

Antialiasing
* Multiple antialiasing algorithms possible. Currently implemented are :  
* Non anti aliased 
* NxN jittered samples per pixel 
* Greyscale Edge Detection Jittered Resampling.
* Per Component (RGB) Edge Detection Jittered Resampling.

Accelleration Structures
* Octree
* Bounding Volume Hierarchy, AABBs in my case.

Improvements To Come.

I need to get mesh instancing working correctly. At the moment rotation throws off various things, e.g. shadowing.
Change the Traceable interface. The main intersection routine should return all intersections with the object, not just the first.
With this we can implement CSG against arbitrary traceable implementations.

Examples
* Refraction needs some work in this one
![Flawed refraction](/OutputImages/RefractiveSphere.bmp?raw=true "Flawed Refraction")
* Reflective sphere and cubemap
![Flawed refraction](/OutputImages/ReflectiveCubemappedSphere.bmp?raw=true "Reflection of cubemap")
* Refractive sphere and cubemap
![Flawed refraction](/OutputImages/RefractiveCubemappedSphere.bmp?raw=true "Refraction of cubemap")
* Colourful room
![Flawed refraction](/OutputImages/Room.bmp?raw=true "Spangly")
* Mesh instances
![Flawed refraction](/OutputImages/Lamps.bmp?raw=true "Mesh instances")
* Texture mapped triangles
![Texture mapped triangles](/OutputImages/LegoCar.bmp?raw=true "Texture mapping on triangles")
* Texture mapped sphere
![Flawed refraction](/OutputImages/Earth.bmp?raw=true "Texture mapping on sphere")
* Perlin noise mixing base materials, red, yellow and texture.
![Flawed refraction](/OutputImages/BurningEarth.bmp?raw=true "Perlin noise mixing base materials")
* Mesh self shadowing.
![Flawed refraction](/OutputImages/LegoCarSelfShadowed.bmp?raw=true "Mesh self shadowing.")
* Torus.
![Mmm.Donuts](/OutputImages/Torus.jpeg?raw=true "Torus Primitive.")
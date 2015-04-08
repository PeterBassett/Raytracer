# Raytracer

A faily traditional raytracer.

Functionality implemented so far.

Primitives
* Spheres
* Triangles
* Planes
* Meshes loaded from wavefront obj files. Supports mtl files.

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
* Edge Detection jittered are implemented.

Accelleration Structures
* Octree
* Bounding Volume Hierarchy, AABBs in my case.

Improvements To Come.

I need to get mesh instancing working correctly. At the moment rotation throws off various things, e.g. shadowing.
Change the Traceable interface. The main intersection routine should return all intersections with the object, not just the first.
With this we can implement CSG against arbitrary traceable implementations.

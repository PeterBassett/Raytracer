# Raytracer

A faily traditional raytracer.

Functionality implemented so far.

Primitives
* Spheres
* Triangles
* Planes
* Meshes loaded from wavefront obj files. Supports mtl files.

Various Materials
* Plane RGB Materials defined with Ambient, Diffuse etc etc etc.
* Checkerboard of two submaterials.
* Perlin noise of two submaterials.
* Texture material. UV coordinate calculations specific to each primitive. All common image file formats are supported via the .net framework.

File Types
* Home made human readable "ray" files.
* Wavefront obj files.
* Wavefront mtl files.

Antialiasing
* After image generation you can run an edge detection routine. The edges are then retraced with a specified subsampling level. 
* By default the subsampling is randomly jittered within each subpixel.

Accelleration Structures
* Octree
* Bounding Volume Hierarchy, AABBs in my case.

Improvements To Come.

I need to get mesh instancing working correctly. At the moment rotation throws off various things, e.g. shadowing.
Change the Traceable interface. The main intersection routine should return all intersections with the object, not just the first.
With this we can implement CSG against arbitrary traceable implementations.

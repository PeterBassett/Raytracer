# Raytracer

A faily traditional raytracer.

Functionality implemented so far.

Primitives
* Spheres
* Triangles
* Planes
* Meshes loaded from wavefront obj files. Supports mtl files.
* Tori (Toruses, Donuts....Mmm Donuts!)
* Cylinders

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

I will eventually change the Traceable interface so that the main intersection routine returns all intersections with the object, not just the first.
With this I can implement CSG against arbitrary traceable implementations.

Examples
* Refraction needs some work in this one
![Flawed refraction](/OutputImages/RefractiveSphere.jpg?raw=true)
* Reflective sphere and cubemap
![Flawed refraction](/OutputImages/ReflectiveCubemappedSphere.jpg?raw=true "Reflection of cubemap")
* Refractive sphere and cubemap
![Flawed refraction](/OutputImages/RefractiveCubemappedSphere.jpg?raw=true "Refraction of cubemap")
* Colourful room
![Flawed refraction](/OutputImages/Room.jpg?raw=true "Spangly")
* Mesh instances
![Flawed refraction](/OutputImages/Lamps.jpg?raw=true "Mesh instances")
* Texture mapped triangles
![Texture mapped triangles](/OutputImages/LegoCar.jpg?raw=true "Texture mapping on triangles")
* Texture mapped sphere
![Flawed refraction](/OutputImages/Earth.jpg?raw=true "Texture mapping on sphere")
* Perlin noise mixing base materials, red, yellow and texture.
![Flawed refraction](/OutputImages/BurningEarth.jpg?raw=true "Perlin noise mixing base materials")
* Mesh self shadowing.
![Flawed refraction](/OutputImages/LegoCarSelfShadowed.jpg?raw=true "Mesh self shadowing.")
* Torus.
![Mmm.Donuts](/OutputImages/Torus.jpg?raw=true "Torus Primitive.")
* Refractive Torus.
![Mmm.Donuts](/OutputImages/RefractiveTorus.jpg?raw=true "Refractive Torus.")
* Transformed Tori.
![Mmm.Donuts](/OutputImages/TransformedTorusChain.jpg?raw=true "Object transformations.")
* Mesh instancing and transformations.
![Cars](/OutputImages/LegoCars.jpg?raw=true "Object transformations.")
* Mesh instancing and transformations.
![Companions](/OutputImages/Cubes.jpg?raw=true "Object transformations.")
* 280 interlinked tori with various levels of reflectivity and refraction.
![chain mail](/OutputImages/ToriGrid.jpg?raw=true ".")
* Cylinders. They support all the usual materials.
![cylinders](/OutputImages/Cylinders.jpg?raw=true ".")

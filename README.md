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
* Cubemaps, both vertical and horizontal crosses, are supported for scene backgrounds. When a ray misses all objects in the scene we render from the cubemap.

Lights
* Point Light - Simple light at a point in space casting light in all directions. Obeys the inverse square law and can cause shadows.
* Ambient Light - Has no position and contributes the same light to every point in space. Casts no shadows.
* Spot Light - Like a point light but has a direction and width in addtion to position. It casts a cone of light along its direction. The width determines the angle of the cone. Casts shadows.
* Distant Light - Has no position but does have a direction. Useful for simulating the sun for example. Casts shadows.
* Projection Light - Like a spot light but projects a texture into the scene. Think overhead projector and you'll not be far off.
* Area Lights. Currently just Spherical but I will expand to disc and quads. Custom sampling threshlold per light. Casts soft shadows.

File Types
* XML Based renderer and scene definition files.
* Wavefront obj files.
* Wavefront mtl files.

Antialiasing
* Multiple antialiasing algorithms possible. Currently implemented are :  
* Non anti aliased, 
* Progressive rendering. 64px square regions followed by 32px etc.
* NxN jittered samples per pixel 
* Greyscale Edge Detection Jittered Resampling.
* Per Component (RGB) Edge Detection Jittered Resampling.

Accelleration Structures
* Octree
* Bounding Volume Hierarchy, AABBs in my case.
* Multi Axis Midpoint Split Criteria 
* Multi Axis SAH Split Criteria

Recent improvements 

Multi axis SAH construction for my AABB BVH.
Fixed Texture UV coordinate calculation. I had them each mirrored.
Changed the lighting design to add more complex lighting. I'll have to change the file format to accomodate this more effectively. 

Still to come

I will also change the transformation capabilities of the primitives and lights so they all take a Transform object instead of explicit positions, rotations etc. The new file format can then have several method of producing a Transform. Explicit translations and rotations, lookat matricies, axis angles etc and they could all be used where ever appropriate. 

Then comes bump mapping, area lights and a path tracer implementation.

Examples

* Refraction needs some work in this one
![Flawed refraction](/OutputImages/RefractiveSphere.jpg?raw=true)
* Reflective sphere and cubemap
![Flawed refraction](/OutputImages/ReflectiveCubemappedSphere.jpg?raw=true)
* Refractive sphere and cubemap
![Flawed refraction](/OutputImages/RefractiveCubemappedSphere.jpg?raw=true)
* Colourful room
![Flawed refraction](/OutputImages/Room.jpg?raw=true)
* Mesh instances
![Flawed refraction](/OutputImages/Lamps.jpg?raw=true)
* Texture mapped triangles
![Texture mapped triangles](/OutputImages/LegoCar.jpg?raw=true)
* Texture mapped sphere
![Flawed refraction](/OutputImages/Earth.jpg?raw=true)
* Perlin noise mixing base materials, red, yellow and texture.
![Flawed refraction](/OutputImages/BurningEarth.jpg?raw=true)
* Mesh self shadowing.
![Flawed refraction](/OutputImages/LegoCarSelfShadowed.jpg?raw=true)
* Torus.
![Mmm.Donuts](/OutputImages/Torus.jpg?raw=true)
* Refractive Torus.
![Mmm.Donuts](/OutputImages/RefractiveTorus.jpg?raw=true)
* Transformed Tori.
![Mmm.Donuts](/OutputImages/TransformedTorusChain.jpg?raw=true)
* Mesh instancing and transformations.
![Cars](/OutputImages/LegoCars.jpg?raw=true)
* Mesh instancing and transformations.
![Companions](/OutputImages/Cubes.jpg?raw=true)
* 280 interlinked tori with various levels of reflectivity and refraction.
![chain mail](/OutputImages/ToriGrid.jpg?raw=true)
* Cylinders. They support all the usual materials.
![cylinders](/OutputImages/Cylinders.jpg?raw=true)
* Texture Coordinate Wrapping.
![texture coordinates](/OutputImages/Sponza.jpeg?raw=true)
* 1,087,474 triangle Happy Buddha.
![SAH BHV](/OutputImages/Buddha.jpeg?raw=true)
* Spot lights.
![spots](/OutputImages/SpotLight.jpeg?raw=true)
* Projector light.
![projector light](/OutputImages/ProjectionLightBuddha.jpeg?raw=true)
* Cryteks version of the Sponza Model. Still work to do
![projector light](/OutputImages/CrytekSponza.jpeg?raw=true)

* Four Spherical Area Lights. 256 shadow rays per light.
![sphere area light](/OutputImages/SphereLight.jpeg?raw=true)

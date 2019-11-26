# Earcut.NET

[![Actions Status](https://github.com/oberbichler/Rhino-Ibra/workflows/CI/badge.svg?branch=master)](https://github.com/oberbichler/Rhino-Ibra/actions) [![Codacy Badge](https://api.codacy.com/project/badge/Grade/9c8e55157c1c49c7876324793b58e248)](https://www.codacy.com/app/oberbichler/earcut.net?utm_source=github.com&amp;utm_medium=referral&amp;utm_content=oberbichler/earcut.net&amp;utm_campaign=Badge_Grade)

Earcut.NET is a port of [earcut.js by Mapbox](https://github.com/mapbox/earcut).

## Usage

Call `Earcut.Tessellate(vertices, holes)` where `vertices` is a flat list of the vertex coordinates and `holes`is the list of hole indices.

```csharp
var tessellation = Earcut.Tessellate(new double[] {
    0,   0,                 // Vertex 0 (outline)
  100,   0,                 // Vertex 1 (outline)
  100, 100,                 // Vertex 2 (outline)
    0, 100,                 // Vertex 3 (outline)
   20,  20,                 // Vertex 4 (hole)
   80,  20,                 // Vertex 5 (hole)
   80,  80,                 // Vertex 6 (hole)
   20,  80                  // Vertex 7 (hole)
}, new int[] {
   4                        // Index of the first Vertex of the hole
});
```

`tessellation` contains a flat list of vertex indices. Each group of three indices forms a triangle.

```csharp
// Triangle 0:
var a_0 = tessellation[0];  // = 3
var b_0 = tessellation[1];  // = 0
var c_0 = tessellation[2];  // = 4

// Triangle 1:
var a_1 = tessellation[3];  // = 5
var b_1 = tessellation[4];  // = 4
var c_1 = tessellation[5];  // = 0

// Triangle 2:
var a_2 = tessellation[3];  // = 3
// ...
```

Please check out the [earcut.js repository](https://github.com/mapbox/earcut) for more detailed information.

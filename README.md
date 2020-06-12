# IfcInfraToolkit

Create and modify IFC4x3 models in the scope of bSI InfraDeployment 

## Content 

The libraries provided in this repository can be used to create IFC4x3RC1 models demonstrating specific geometry concepts in the IFC data model. 

## Build the project

Most of the relevant Civil3D API dlls are shipped within the repository (see folder 'precompiledDLLs'). 
Reinstall all Dynamo-specific dependencies via the Nuget Manager. 
Integrate the library compiled by the source code by importing the dll into your Dynamo in Civil3D. 

## Planned Features

- [ ] Export alignments into IFC
- [ ] Support OpenCrossProfileDefs to represent road geometries
- [ ] Advanced spatial structure trees inside IFC
- [ ] Products, placements and geometries created in Civil3D and/or Dynamo

## Used Frameworks
- GeometryGymIFC: https://github.com/GeometryGym/GeometryGymIFC MIT Licence
- Autodesk Civil3D APIs: https://www.autodesk.de/products/civil-3d/overview 



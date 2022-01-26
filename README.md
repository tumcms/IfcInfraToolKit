# IfcInfraToolkit

[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)

Create and modify IFC4x3 models using Dynamo nodes. 

![00_Overview](https://user-images.githubusercontent.com/56266373/151181167-f33f0f92-f49e-4309-83ff-c6bc7fd500de.PNG)

## Content 

The libraries provided in this repository enable interactive composing mechanisms to create IFC4x3 models. 
The provided toolkits are prototypes to test specific concepts of recent IFC extensions and should not be used in any production critical environment. 

## Build the project

After cloning the code, you find a `*.sln` file. 
Please re-install all referenced nuget packages. 

Build `InfraToolKit_Common` first as the Dynamo-specific libs reference the build result of the `_Common` library.

## Sample scripts

Please refer to the `\sampleDymamoScripts` folder for a comprehensive overview of examples. 


## Frameworks
- GeometryGymIFC: https://github.com/GeometryGym/GeometryGymIFC MIT Licence
- Autodesk Civil3D APIs: https://www.autodesk.de/products/civil-3d/overview 
- Autodesk Revit API: https://www.revitapidocs.com/ 
- Dynamo ZeroTouchLibaries: https://github.com/DynamoDS/Dynamo 


# TroubleShooting

We have experienced issues with an outdated version of GeometryGymIFC library shipped in Revit 2021 and 2022. 
Please perform the following steps to figure out if your Revit installation is affected by this issue: 

- Open Revit
- In Revit, open Dynamo
- Import the built result `IfcInfraToolKit_DynamoCore.dll`, which is the result of building `IfcInfraToolKit_DynamoCore` of this solution
- Create a simple Dynamo flow consisting of `CreateIfcModel` and `SaveIfcModel` nodes
- open the result in a text editor and see the version given in the header of the IFC file

If it states `v0.0.20`, please go into `C:\Program Files\Autodesk\Revit 2022` and replace `GeometryGymIFC.dll` with a more recent version of this dll. 
You can find a suitable dll named similar in the build results located in `ifcinfratoolkit\IfcInfraToolKit_DynamoCore\bin\Debug` or `Release`


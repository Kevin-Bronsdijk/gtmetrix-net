// --------------------------------------------------------------------------------------
// FAKE build script 
// --------------------------------------------------------------------------------------

#r @"tools\FAKE\tools\FakeLib.dll"
open Fake
open Fake.AssemblyInfoFile

// --------------------------------------------------------------------------------------
// Information about the project to be used at NuGet and in AssemblyInfo files
// --------------------------------------------------------------------------------------

let project = "gtmetrix-net"
let authors = ["Kevin Bronsdijk"]
let summary = "GTmetrix-net .Net client"
let version = getBuildParam "version"
let description = "The GTmetrix-net .Net client interacts with the GTmetrix.com REST API allowing you to utilize GTmetrixs features using a .NET interface."
let notes = "Added the FullyLoadedTime property to the Results class. For more information and documentation, please visit the project site on GitHub."
let nugetVersion = getBuildParam "version"
let tags = "gtmetrix.com gtmetrix C# API web optimization"
let gitHome = "https://github.com/kevin-bronsdijk"
let gitName = "gtmetrix-net"

// --------------------------------------------------------------------------------------
// Build script 
// --------------------------------------------------------------------------------------

let buildDir = "./output/"
let packagingOutputPath = "./nuGet/"
let packagingWorkingDir = "./inputNuget/"
let nugetDependencies = getDependencies "./src/gtmetrix-net/packages.config"

// --------------------------------------------------------------------------------------

Target "Clean" (fun _ ->
 CleanDir buildDir
)

// --------------------------------------------------------------------------------------

Target "AssemblyInfo" (fun _ ->
    let attributes =
        [ 
            Attribute.Title project
            Attribute.Product project
            Attribute.Description summary
            Attribute.Version version
            Attribute.FileVersion version
        ]

    CreateCSharpAssemblyInfo "src/gtmetrix-net/Properties/AssemblyInfo.cs" attributes
)

// --------------------------------------------------------------------------------------

Target "Build" (fun _ ->
 !! "src/*.sln"
 |> MSBuildRelease buildDir "Build"
 |> Log "AppBuild-Output: "
)

// --------------------------------------------------------------------------------------

Target "CreatePackage" (fun _ ->

    CreateDir packagingWorkingDir
    CleanDir packagingWorkingDir
    CopyFile packagingWorkingDir "./output/gtmetrix.dll"

    NuGet (fun p -> 
        {p with
            Authors = authors
            Dependencies = nugetDependencies
            Files = [ (@"gtmetrix.dll", Some @"lib/net452", None);
                        (@"gtmetrix.dll", Some @"lib/net45", None) ] 
            Project = project
            Description = description
            OutputPath = packagingOutputPath
            Summary = summary
            WorkingDir = packagingWorkingDir
            Version = nugetVersion
            ReleaseNotes = notes
            Publish = false }) 
            "gtmetrix.nuspec"
            
    DeleteDir packagingWorkingDir
)

// --------------------------------------------------------------------------------------

Target "All" DoNothing

"Clean"
  ==> "AssemblyInfo"
  ==> "Build"
  ==> "CreatePackage"
  ==> "All"

RunTargetOrDefault "All"
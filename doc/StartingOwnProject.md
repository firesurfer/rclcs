# How to start your own project

This page explains how to create your own project using the __rclcs__. 

First see the [testing workspace](https://github.com/firesurfer/rclcs_testing_ws) for code examples and for build system examples. 
In the following I will explain the step how to create such a workspace.

## Create the workspace

Simply create a new workspace / package according to the ros2 manual.

Then inside your package you should have a source folder.
Inside this folder you want to create an `csproj` project either with visual studio or monodevelop.
This has two advantages:
    
    * You can write your code inside an ide that helps you write C# code
    * It makes the compilation much easier, because we can simply invoke `msbuild` with the csproj file as argument

## Edit cmake / package.xml

Open the `package.xml` in an editor of your choice. 

Insert the following lines:

```
 <build_depend>rclcs</build_depend>
 <exec_depend>rclcs</exec_depend>
```

Then open the `CMakeLists.txt`

Insert in the `find_package` section:

```
    find_package(dotnet_cmake_module REQUIRED)
    find_package(DotNETExtra MODULE)
```


Then add your created project as cmake target:

```
add_msbuild(<project name>
    EXECUTABLE
    CSPROJ
    ${CMAKE_CURRENT_SOURCE_DIR}/src/<your project>.csproj)
install_assemblies(<project name> COPY_DEPENDENCIES DESTINATION bin)
```



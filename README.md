# rclcs

__WARNING__ : The rcl api and the generated message types did change recently and I couldn't adapt yet. You will be able to compile everything but you will run into segfaults while execution!
These are usually some easy fixes by adapting the methods called in the rcl.

A C# wrapper for [ROS2](https://github.com/ros2). It is designed to provide an interface similar to the rclcpp.
The idea behind the wrapper is to reproduce the ros message as a C# struct with the same memory layout as their C counterparts.
So the messages (structs) can directly be passed to the `rcl` without any conversions. For easier usage these structs are wrapped into classes so you don't have the inconvenience of memory handling.

__Important__: I needed to force building 64bit assemblies. If you need 32bit support you have to add fitting targets in your csproj files and adapt the `add_msbuild` cmake function to build the 32bit target instead!

## How to use

These instructions are mostly the same on Windows and Linux. 

Get your copy of ROS and compile it according to the documentation:

For __Linux__ : https://github.com/ros2/ros2/wiki/Linux-Development-Setup

For __Windows__: https://github.com/ros2/ros2/wiki/Windows-Development-Setup

Make sure you've got mono/ .Net in a version installed that targets at least the framework version 4.5.1.

Then from your ros2 workspace base directory:
```
  cd src/ament
  git clone https://github.com/firesurfer/dotnet_cmake_module
  cd ../ros2
  git clone https://github.com/firesurfer/rclcs
  cd rosidl
  git clone https://github.com/firesurfer/rosidl_generator_cs
```
Then edit the file in `rosidl_typesupport/rosidl_default_generators/CMakeLists.txt` and add the following line after the other generators:
```
  ament_export_dependencies(rosidl_generator_cs)
```

Then go back to the ros2 workspace and do:


### Linux
```
rm -rf build/ install/
./src/ament/ament_tools/scripts/ament.py build

``` 

### Windows 
```
rmdir /S /Q build install
python src\ament\ament_tools\scripts\ament.py build
```


## Install to the GAC (Global assembly cache)

This is __optional__ but useful. But it just makes sense if the code is stable. By installing the assembly into the GAC you won't need the `WindowsAssemblyLoader` or the `MONO_PATH` any more (See examples for more information). But you are tied to the installed version of the rclcs.

1. Create a keyfile:
(sn is called sn.exe on windows)

```
sn -k <keypair name>
```

2. Tell the compiler to use it:

In your `CMakeLists.txt` in the `add_assemblies` section add to the `COMPILER_ARGS`
```
"-keyfile:<Path to the keypair>"

```

3. Install to the GAC:

```
gacutil -i <assembly name>
```


 
## Examples

You can have look at my [testing workspace](https://github.com/firesurfer/rclcs_testing_ws) which contains some [examples](https://github.com/firesurfer/rclcs_testing_ws/tree/master/src/test_cs/test_cs/Examples) (And is still a bit messy, but you should get ahead with it).

For windows you need to use the `WindowsAssemblyLoader` as main method and replace the line inside the `StartMain(string[] args)` function with your own start function.


## What works at the moment

* Init and deinit RCL
	* Errorhandling of return values, throwing exceptions where useful.
	* Implemented rmw errorhandling methods
* Create a Node
* Create a Publisher
* Create a Subscription
* Create a Service/Client
	* Get a request and answer it
* Publish and recieve a  message 
* Generate code for messages 
* Setting qos profiles
* Using the currently implemented graph functions
* Having intellisense comments

For a list of currently supported types see: [supported types](doc/SupportedTypes.md)
For further understanding of the what is happening behind the scenes see [memory handling](/doc/MemoryHandling.md)

## What doesn't work at the moment
(And I know that it doesn't work)

* ~~String arrays~~
* ~~Fixed Arrays - Coming soon (probably)~~
* Preinitialized value -> Not coming soon (This is because C# doesn't allow preinitialised members in structs) At the moment the preinit values are simply ignored
* Sometimes you might have to compile messages twice in order to have them properly compiled. (Or just remove the build and install folder)
* ~~Sending messages from a cpp program to a C# program: (See: https://github.com/eProsima/ROS-RMW-Fast-RTPS-cpp/pull/45)~~

## What is critical at the moment

* ~~I'm not sure if it's possible to reproduce more complicated messages in C# an directly pass them to the rcl without any conversion. A conversion would be possible but would be a waste of resources in the most cases.~~

* ~~Program crashes at exit due to a multithreading error~~ 
* Sometimes the program will crash at exit (But just sometimes....)
* ~~Memory handling has to be done manual: See [memory handling](/doc/MemoryHandling.md)~~
* Windows support. Codepaths for service and client are missing at the moment. Publishing and subscribing not tested at the moment.
* ~~Messages are somehow compiled twice. This isn't critical but it fails at the first time. So don't worry if you get any build warnings.~~

## What has to be done next

* ~~Generate xml documentation for better autocompletion~~
* ~~Cleanup of the message generator~~
   * ~~Use templating engine for message generation~~ I implemented the message generator in C# using the CodeDom API
   * The generated code would be better if I could directly create IL-code
   * Testing of nested types in the messages
   * Having a proper dependency resolving mechanism
* ~~Fix errors regarding arrays~~
* Free memory where needed - I should have covered all (or at least most cases) by now
* ~~Finish implementation of services~~
* Implement error handling where needed
	* There are some return type checks missing
	* There are some cases where some argument checks would be useful
	* In some cases it would be useful to trak the current initilisation state
* Make sure the api is consistent with the rclcpp
	* I think I'm going to break the consistency in some parts in favour to usability
	* Implement the various executors and spin methods
* Write tests (See Build and run tests section)
* Allow (easier) debugging the managed code
* ~~Integrate into ament (See [this](https://groups.google.com/d/msg/ros-sig-ng-ros/MN_N_SunrjA/wuEUYOXxEwAJ) mailinglist post)~~
    * Check integration for windows
* Implement code paths for windows (Infrastructure is already available)

## Build and run tests

Checkout my fork of the [system_tests](https://github.com/firesurfer/system_tests) repository of ros2.
The test are implemented via the [nunit framework](https://github.com/nunit/docs/wiki/NUnit-Documentation) version 3.2. 
This version is shipped via the system_tests repository. The nunit framework is shipped under the MIT license which is compatible to the 
Apache 2.0 license used for the system_test repository. 
You can build and run the tests via:

```
cd <ros2_ws>
ament build --only-package test_rclcs --build-tests
```

Unfortunatly invoking the nunit3-console test runner via cmake results in an uncoloured test output. Furthermore I need to copy the `test_rclcs.dll` into the 
`ros2_ws/install/lib` directory and run the test in this directory because otherwise nunit won't properly resolve the assembly dependencies. 
The test results can also be found in an xml file called "TestResults.txt" in the `install/lib` folder.


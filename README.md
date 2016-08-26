# rclcs

A C# wrapper for [ROS2](https://github.com/ros2). It is designed to provide an interface similar to the rclcpp.
The idea behind the wrapper is to reproduce the ros message as a C# struct with the same memory layout as their C counterparts.
So the messages (structs) can directly be passed to the `rcl` without any conversions. 

## How to use

Get your [copy of ROS](https://github.com/ros2/ros2/wiki/Linux-Development-Setup) and compile it according to the documentation.

Make sure you've got mono/ .Net installed (Not tested on windows yet).

Then from your ros2 workspace base directory:
```
  cd src/ros2
  git clone https://github.com/firesurfer/rclcs
  cd rosidl
  git clone https://github.com/firesurfer/rosidl_generator_cs
```
Then edit the file in `rosidl/rosidl_default_generators/CMakeLists.txt` and add the following line after the other generators:
```
  ament_export_dependencies(rosidl_generator_cs)
```

Then go back to the ros2 workspace and do an ament build. 

An example on how to use the rclcs in your own application is soon to follow.

## What works at the moment

* Init and deinit RCL
* Create a Node
* Create a Publisher
* Create a Subscription
* Publish and recieve a simple message (no nested types)
* Generate code for messages without nested types

For a list of currently supported types see: [supported types](doc/SupportedTypes.md)

## What doesn't work at the moment

* Everything else 
* Services
* Arrays - Working on it

## What is critical at the moment

* ~ ~I'm not sure if it's possible to reproduce more complicated messages in C# an directly pass them to the rcl without any conversion. A conversion would be possible but would be a waste of resources in the most cases.~ ~

## What has to be done next

* Support for nested types - this includes a cleanup of the message generator
* Fix errors regarding arrays
* Free memory where needed
* Finish implementation of services
* Implement error handling where needed
* Make sure the api is consistent with the rclcpp

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

## Example
You can have look at my [testing workspace](https://github.com/firesurfer/rclcs_testing_ws) ~~which is quite messy.~~ which contains some [examples](https://github.com/firesurfer/rclcs_testing_ws/tree/master/src/test_cs/test_cs/Examples) (And is still a bit messy).


## What works at the moment

* Init and deinit RCL
* Create a Node
* Create a Publisher
* Create a Subscription
* Create a Service/Client
	* Get a request and answer it
* Publish and recieve a  message (nested types might be a bit buggy at the moment)
	* Arrays will work soon without any patches to the rmw (See: https://github.com/eProsima/ROS-RMW-Fast-RTPS-cpp/pull/45)
* Generate code for messages 
* Setting qos profile for subscription and publisher

For a list of currently supported types see: [supported types](doc/SupportedTypes.md)
For further understanding of the what is happeding behind the scenes see [memory handling](/doc/MemoryHandling.md)

## What doesn't work at the moment
(And I know that it doesn't work)

* String arrays
* Fixed Arrays - Coming soon (probably)
* Preinitialized value -> Not coming soon (This is because C# doesn't allow preinitialised members in structs)

## What is critical at the moment

* ~~I'm not sure if it's possible to reproduce more complicated messages in C# an directly pass them to the rcl without any conversion. A conversion would be possible but would be a waste of resources in the most cases.~~

* Program crashes at exit due to a multithreading error
* ~~Memory handling has to be done manual: See [memory handling](/doc/MemoryHandling.md)~~

## What has to be done next

* Generate xml documentation for better autocompletion
* Cleanup of the message generator
   * Use templating engine for message generation
* ~~Fix errors regarding arrays~~
* Free memory where needed
* ~~Finish implementation of services~~
* Implement error handling where needed
* Make sure the api is consistent with the rclcpp
	* I think I'm going to break the consistency in some parts in favour to usability
* Write tests
* Use templating engine for message generation
* Allow (easier) debugging the managed code
* Integrate into ament



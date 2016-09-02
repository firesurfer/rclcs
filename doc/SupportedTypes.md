# Supported types

Because it's kind of difficult to automatically reproduce all types which are defined in [ROS wiki]( http://wiki.ros.org/msg ) in C# / managed code, I can only provide a limited subset at the moment:

* bool
* int8
* uint8
* int16
* uint16
* int32
* uint32
* int64
* uint64
* float32
* float64
* string 

* arrays 

* nested message are going to be supported as soon a I'm doing a clean up of the rosidl_generator_cs - the implementation should be fairly easy because nested structs in c# can directly be used in c. 

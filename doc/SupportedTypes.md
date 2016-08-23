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
* string (got some errors at the moment)

* arrays of the types above are soon to follow

## Types I'm not sure that they will ever work

* Nested message. For example you define a message A and want an array of elements of message A in a message B

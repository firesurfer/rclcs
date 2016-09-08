# Memory handling

See the last paragraph "Solution"

When using a managed language like C# one usually has the advantage not to be bothered with memory management, because the garbage collector keeps track of allocated memory und releases the memory in case 
there are no more references pointing at it. 

So why do we have to worry about memory management: Because the rclcs does native calls into the rcl. 

## What you don't have to worry about

(If you are using it correctly)
Simple cases where the user doesn't have care about the memory are the creation/deletion of a node/publisher/subscription/service/client. Let's take for example the Node class:

```cs
public class Node:Executable
	{
		private bool disposed = false;
		private rcl_node InternalNode;

//Some more code

	protected override void Dispose(bool disposing)
		{
			if (disposed)
				return; 

			if (disposing) {
				// Free any other managed objects here.
				//
				InternalNode.Dispose ();
			}

			// Free any unmanaged objects here.
			//

			disposed = true;
			// Call base class implementation.
			base.Dispose(disposing);
		}
```

As you can see the class implements the [IDisposable pattern](https://msdn.microsoft.com/de-de/library/fs2xkftw%28v=vs.110%29.aspx).
This means you can safely use an instance of Node with a using directive:

```csharp
using (Node testNode = new Node ("BasicNodeExample")) {
	//Do some stuff with your code
}

``` 
or dispose it by hand:

```cs
Node testNode = new Node("BasicNodeExample");

testNode.Dispose();
```

The point in the code where the native handle is released can be found in the rcl\_node class which wraps the native functions. The Node class simply calls dispose on it's instance of the rcl\_node class in its dispose method. 

```cs
	public class rcl_node:IDisposable
	{
		private bool disposed = false;
		private rcl_node_t nativ_handle;
	//Some more code


	public void Dispose()
	{ 
		Dispose(true);
		GC.SuppressFinalize(this);           
	}
	protected virtual void Dispose(bool disposing)
	{
		if (disposed)
			return; 

		if (disposing) {
			// Free any other managed objects here.
				//
		}
		if(rcl_node_is_valid(ref nativ_handle))
			rcl_node_fini (ref nativ_handle);
		// Free any unmanaged objects here.
		//
		disposed = true;
	}
```

## What you have to worry about

Now we get down the stuff which make ros interessting: the messages. The messages are automatically created by the [rosidl_generator_cs](https://github.com/firesurfer/rosidl_generator_cs) and can also be used like in c++ or c. For example:

```cs
using (Publisher<test_msgs.msg.Dummy> testPublisher = testNode.CreatePublisher<test_msgs.msg.Dummy> ("TestTopic")) {
		//Create a message //TODO let message implement IDisposable
		test_msgs.msg.Dummy testMsg = new test_msgs.msg.Dummy (); 
		//Fill the message fields
		testMsg.thisafloat32 = 0.4f;
		//Fill a string 
		testMsg.thisisastring = new rosidl_generator_c__String ("TestString");
		//Fill an array
		testMsg.thisafloat32array = new rosidl_generator_c__primitive_array_float32 (new float[]{ 1.3f, 100000.4f });

		//And now publish the message
		testPublisher.Publish (testMsg);
		//Free unmanaged memory
		testMsg.Free ();
}
```

As you can see you can create and safely delete a publisher the same way like a node simply by using a using statement. The code for the message also doesn't look that exiting aside from the last line:
```cs
//Free unmanaged memory
testMsg.Free ();
```

So let's see why it's necessary to free the memory by hand (this is only needed if you are using arrays - you could get around by using fixed size arrays).
First let's have look at the generated code:
```cs
using System;
using rclcs;
using System.Runtime.InteropServices;
namespace test_msgs
{
    namespace msg
    {
    [StructLayout (LayoutKind.Sequential)]
    public struct Dummy:IRosMessage
    {
        [DllImport ("libtest_msgs__rosidl_typesupport_introspection_c.so")]
        public static extern IntPtr rosidl_typesupport_introspection_c_get_message__test_msgs__msg__Dummy();

        public void Free(){
          foreach (var item in this.GetType().GetFields()) {
            if (typeof(IRosTransportItem).IsAssignableFrom (item.FieldType)) {
               IRosTransportItem ros_transport_item = (IRosTransportItem)item.GetValue(this);
               ros_transport_item.Free();
            }
          }
        }
        public byte thisisabool;
        public System.Byte thisisaint8;
        public System.SByte thisiauint8;
        public System.Int16 thisisaint16;
        public System.UInt16 thisisauint16;
        public System.Int32 thisisaint32;
        public System.UInt32 thisisauint32;
        public System.Int64 thisisaint64;
        public System.UInt64 thisisauint64;
        public float thisafloat32;
        public double thisisfloat64;
        public rosidl_generator_c__String thisisastring;
        public rosidl_generator_c__String thisisanotherstring;
        public rosidl_generator_c__primitive_array_bool thisisaboolarray;
        public rosidl_generator_c__primitive_array_int8 thisisaint8array;
        public rosidl_generator_c__primitive_array_uint8 thisiauint8array;
        public rosidl_generator_c__primitive_array_int16 thisisaint16array;
        public rosidl_generator_c__primitive_array_uint16 thisisauint16array;
        public rosidl_generator_c__primitive_array_int32 thisisaint32array;
        public rosidl_generator_c__primitive_array_uint32 thisisauint32array;
        public rosidl_generator_c__primitive_array_int64 thisisaint64array;
        public rosidl_generator_c__primitive_array_uint64 thisisauint64array;
        public rosidl_generator_c__primitive_array_float32 thisafloat32array;
        public rosidl_generator_c__primitive_array_float64 thisisfloat64array;
        public rosidl_generator_c__primitive_array_string thisisastringarray;
        public builtin_interfaces.msg.Time thisisatime;
    }
    }
}
```
As you can see the primitive types like int32 and float are just part of the message. In C# they also will be arranged in the memory in this order (thanks to: [StructLayout (LayoutKind.Sequential)]). 
More interesting are the array and strings types. They are themselfs just structs. They have to be structs, because otherwise we would have, in order to publish a message, create a piece of memory with the same size the message would have in c and copy all necessary parts at the correct position. This would be catastrophic from a performance point of view. 
Let's have look at the code for the int8 array:
```cs
[StructLayout(LayoutKind.Sequential)]
	public struct rosidl_generator_c__primitive_array_int8:IRosTransportItem
	{	
		public IntPtr Data ;
		UIntPtr Size;
		UIntPtr Capacity;


		public rosidl_generator_c__primitive_array_int8(byte[] _Data)
		{
			
			Data = Marshal.AllocHGlobal (Marshal.SizeOf<byte>()* _Data.Length);
			Size = (UIntPtr) _Data.Length;
			Capacity = Size;
			Marshal.Copy (_Data, 0, Data, _Data.Length);

		}
		public byte[] Array
		{
			get{ 
				byte[] tempArray = new byte[(int)Size]; 
				Marshal.Copy (Data, tempArray, 0, (int)Size);
				return tempArray;
			}
		}
		public int ArraySize
		{
			get{return (int)Size;}
		}
		public int ArrayCapacity
		{
			get{ return (int)Capacity;}
		}
		public void Free()
		{
			if(Data != IntPtr.Zero)
				Marshal.FreeHGlobal (Data);

		}
	}

```
As already shown in the example it would be used like that:
```cs
testMsg.thisaint8array = new rosidl_generator_c__primitive_array_int8 (new byte[]{ 1, 10 });
```
So this is what we are doing here: We got data for the array so we are creating an instance of the struct and pass a byte array to it. In the constructor we allocate a piece of unmanaged memory with the given size: `Data = Marshal.AllocHGlobal (Marshal.SizeOf<byte>()* _Data.Length);` and copy the array data into the unmanaged memory. In order to retrieve it we copy it back into a managed array and return this array.
The problem is: When shall we free the memory (Calling `Marshal.FreeHGlobal (Data);`) ? The obvious answer would be, let's add an destructor to the struct. 
Well C# doesn't allow destructors in structs (for a good reason). Furthermore would a destructor in the struct not solve the problem, it would even make it more problematic. Structs in C# are valuetypes. This means that every time you do something like:
```cs
struct A{}

//Now we use the struct
A instance1 = new A();
A instance2 = instance1;

```
we do a copy of the whole struct. This means we do a copy of the IntPtr, so if we destroy one copy and release the memory in this copy all other copies will suddenly have invalid data behind the Data pointer. 

### Solutions to this dilemma

Perhaps it would be an idea to wrap the message into a message container that is a class. A class is a reference type which means that all users will use the same struct afterwards. Problems with this solution might be that you could still create copies of the struct itsself.

## The (in the end) implemented solution

I solved this problem by creating a wrapper class for each generated message struct. The wrapper class contains one instance of the struct which can be obtained and set by reference. The wrapper class inherits the MessageWrapper base class which defines basic functions for accessing the data. This still leaves a main problem that a struct is still a valuetype that will be copied in many cases you would'n expect it to be copied. Therefore the message generator creates getter and setter methods that directly access the struct fields (and create some magic for accessing arrays). Furthermore the wrapper itself implements IDisposable and calls the `IRosTransportItem.Free()` method where and when ever needed. In most cases at the point of its lifecyle where it gets destroyed. 

//TODO talk more about the solution

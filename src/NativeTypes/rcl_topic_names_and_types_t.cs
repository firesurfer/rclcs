using System;
using System.Runtime.InteropServices;
using System.Collections.Generic;

namespace rclcs
{
	/// <summary>
	/// Rcl topic names and types t.
	/// Native version 
	/// </summary>
	internal struct rcl_topic_names_and_types_t
	{
		/*
         *  size_t topic_count;
            char ** topic_names;
            char ** type_names;
         */
		public IntPtr topic_count;
		public IntPtr topic_names;
		public IntPtr type_names;
	}
	/// <summary>
	/// RCL topic names and types.
	/// Managed version of the rcl_topic_names_and_types_t
	/// </summary>
	public class RCLTopicNamesAndTypes
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="rclcs.RCLTopicNamesAndTypes"/> class.
		/// Takes the native_struct and marshalls the char** for topic_names and type_names
		/// </summary>
		/// <param name="native_struct">Native struct.</param>
		internal RCLTopicNamesAndTypes(rcl_topic_names_and_types_t native_struct)
		{
			string[] topics = MarshallingHelpers.PtrToStringArray(native_struct.topic_names);
			string[] types = MarshallingHelpers.PtrToStringArray(native_struct.type_names);
			TopicNames = new List<string>(topics);
			TypeNames = new List<string>(types);

		}
		public List<string> TopicNames { get; private set; }
		public List<string> TypeNames { get; private set; }



	}

}
//#define RMW_GID_STORAGE_SIZE 24
using System;


namespace rclcs
{
	public  unsafe struct rmw_gid_t
	{
		public string implementation_identifier;
		public fixed byte data [24] ;
	}
}


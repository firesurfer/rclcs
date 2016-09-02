using System;

namespace rclcs
{
	public unsafe struct rmw_request_id_t
	{
		public fixed byte write_guid[16];
		public Int64 sequence_number;
	}
}


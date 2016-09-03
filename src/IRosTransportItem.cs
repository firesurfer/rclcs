using System;

namespace rclcs
{
	public interface IRosTransportItem
	{
		//TODO IDisposable would be better
		void Free();
	}
}


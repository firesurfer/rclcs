using System;

namespace rclcs
{
	public interface IRosTransportItem
	{
		//TODO IDisposable would be better ? Perhaps not: https://stackoverflow.com/questions/7914423/struct-and-idisposable
		void Free();
	}
}


using System;

namespace rclcs
{
	/// <summary>
	/// Basic interface every message has to implement.
	/// It provides a free method which should be used in order to free unmanaged, manually allocated memory.
	/// </summary>
	public interface IRosTransportItem
	{
		//TODO IDisposable would be better ? Perhaps not: https://stackoverflow.com/questions/7914423/struct-and-idisposable
		void Free();
	}
}


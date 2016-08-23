using System;

namespace ROS2Sharp
{
	public class RCLErrorException:Exception
	{
		public RCLReturnValues RCLReturnValue{ get; private set;}
		public RCLErrorException()
		{
			RCLReturnValue = RCLReturnValues.RCL_RET_ERROR;
		}
		public RCLErrorException(string message):base(message)
		{
			RCLReturnValue = RCLReturnValues.RCL_RET_ERROR;
		}
		public RCLErrorException(string message,Exception inner):base(message,inner)
		{
			RCLReturnValue = RCLReturnValues.RCL_RET_ERROR;
		}
	}
	public class RCLTimeoutException:Exception
	{
		public RCLReturnValues RCLReturnValue{ get; private set;}
		public RCLTimeoutException()
		{
			RCLReturnValue = RCLReturnValues.RCL_RET_TIMEOUT;
		}
		public RCLTimeoutException(string message):base(message)
		{
			RCLReturnValue = RCLReturnValues.RCL_RET_TIMEOUT;
		}
		public RCLTimeoutException(string message,Exception inner):base(message,inner)
		{
			RCLReturnValue = RCLReturnValues.RCL_RET_TIMEOUT;
		}
	}
	public class RCLNotInitException:Exception
	{
		public RCLReturnValues RCLReturnValue{ get; private set;}
		public RCLNotInitException()
		{
			RCLReturnValue = RCLReturnValues.RCL_REG_NOT_INIT;
		}
		public RCLNotInitException(string message):base(message)
		{
			RCLReturnValue = RCLReturnValues.RCL_REG_NOT_INIT;
		}
		public RCLNotInitException(string message,Exception inner):base(message,inner)
		{
			RCLReturnValue = RCLReturnValues.RCL_REG_NOT_INIT;
		}
	}
	public class RCLBadAllocException:Exception
	{
		public RCLReturnValues RCLReturnValue{ get; private set;}
		public RCLBadAllocException()
		{
			RCLReturnValue = RCLReturnValues.RCL_RET_BAD_ALLOC;
		}
		public RCLBadAllocException(string message):base(message)
		{
			RCLReturnValue = RCLReturnValues.RCL_RET_BAD_ALLOC;
		}
		public RCLBadAllocException(string message,Exception inner):base(message,inner)
		{
			RCLReturnValue = RCLReturnValues.RCL_RET_BAD_ALLOC;
		}
	}
	public class RCLInvalidArgumentException:Exception
	{
		public RCLReturnValues RCLReturnValue{ get; private set;}
		public RCLInvalidArgumentException()
		{
			RCLReturnValue = RCLReturnValues.RCL_RET_INVALID_ARGUMENT;
		}
		public RCLInvalidArgumentException(string message):base(message)
		{
			RCLReturnValue = RCLReturnValues.RCL_RET_INVALID_ARGUMENT;
		}
		public RCLInvalidArgumentException(string message,Exception inner):base(message,inner)
		{
			RCLReturnValue = RCLReturnValues.RCL_RET_INVALID_ARGUMENT;
		}
	}
	public class RCLPublisherInvalidException:Exception
	{
		public RCLReturnValues RCLReturnValue{ get; private set;}
		public RCLPublisherInvalidException()
		{
			RCLReturnValue = RCLReturnValues.RCL_RET_PUBLISHER_INVALID;
		}
		public RCLPublisherInvalidException(string message):base(message)
		{
			RCLReturnValue = RCLReturnValues.RCL_RET_PUBLISHER_INVALID;
		}
		public RCLPublisherInvalidException(string message,Exception inner):base(message,inner)
		{
			RCLReturnValue = RCLReturnValues.RCL_RET_PUBLISHER_INVALID;
		}
	}
	public class RCLSubscriptionInvalidException:Exception
	{
		public RCLReturnValues RCLReturnValue{ get; private set;}
		public RCLSubscriptionInvalidException()
		{
			RCLReturnValue = RCLReturnValues.RCL_RET_SUBSCRIPTION_INVALID;
		}
		public RCLSubscriptionInvalidException(string message):base(message)
		{
			RCLReturnValue = RCLReturnValues.RCL_RET_SUBSCRIPTION_INVALID;
		}
		public RCLSubscriptionInvalidException(string message,Exception inner):base(message,inner)
		{
			RCLReturnValue = RCLReturnValues.RCL_RET_SUBSCRIPTION_INVALID;
		}
	}
	public class RCLSubscriptonTakeFailedException:Exception
	{
		public RCLReturnValues RCLReturnValue{ get; private set;}
		public RCLSubscriptonTakeFailedException()
		{
			RCLReturnValue = RCLReturnValues.RCL_RET_SUBSCRIPTION_TAKE_FAILED;
		}
		public RCLSubscriptonTakeFailedException(string message):base(message)
		{
			RCLReturnValue = RCLReturnValues.RCL_RET_SUBSCRIPTION_TAKE_FAILED;
		}
		public RCLSubscriptonTakeFailedException(string message,Exception inner):base(message,inner)
		{
			RCLReturnValue = RCLReturnValues.RCL_RET_SUBSCRIPTION_TAKE_FAILED;
		}
	}
}


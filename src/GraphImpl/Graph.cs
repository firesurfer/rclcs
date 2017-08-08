using System;
using System.Runtime.InteropServices;
using System.Collections.Generic;
namespace rclcs
{

    public abstract class GraphBase: IDisposable
    {
        protected bool disposed = false;

        public GraphBase()
        {
            
        }
        public void Dispose()
		{
			// Dispose of unmanaged resources.
			Dispose(true);
			// Suppress finalization.
			GC.SuppressFinalize(this);
		}
		protected virtual void Dispose(bool disposing)
		{
			if (disposed)
				return;
			if (disposing)
			{

				// Free any other managed objects here.
			}
			// Free any unmanaged objects here.


			disposed = true;
		}

        public abstract UInt64 CountPublishers(Node RosNode, string TopicName);
        public abstract UInt64 CountSubscriptions(Node RosNode, string TopicName);
        public abstract RCLTopicNamesAndTypes GetTopicNamesAndTypes(Node RosNode);
        public abstract bool ServiceServerAvailable<T, U>(Node RosNode, Client<T, U> ServiceClient)
            where T : MessageWrapper, new()
            where U : MessageWrapper, new();
    }

    public class Graph:IDisposable
    {
        GraphBase Impl;
		protected bool disposed = false;

        public Graph()
        {
			if (Environment.OSVersion.Platform == PlatformID.Win32NT)
			{
				//TODO codepath for windows
			}
			else if (Environment.OSVersion.Platform == PlatformID.Unix)
			{
                Impl = new GraphLinux();
			}
			else
			{
				throw new Exception("Operating system: " + Environment.OSVersion.Platform.ToString() + " not supported");
			}
        }

		public void Dispose()
		{
			// Dispose of unmanaged resources.
			Dispose(true);
			// Suppress finalization.
			GC.SuppressFinalize(this);
		}
		protected virtual void Dispose(bool disposing)
		{
			if (disposed)
				return;
			if (disposing)
			{

				// Free any other managed objects here.
			}
			// Free any unmanaged objects here.


			disposed = true;
		}

        public UInt64 CountPublishers(Node RosNode, string TopicName)
        {
            return Impl.CountPublishers(RosNode, TopicName);
        }

		
		public  UInt64 CountSubscriptions(Node RosNode, string TopicName)
        {
            return Impl.CountSubscriptions(RosNode, TopicName);
        }
		public  RCLTopicNamesAndTypes GetTopicNamesAndTypes(Node RosNode)
        {
            return Impl.GetTopicNamesAndTypes(RosNode);
        }
		public  bool ServiceServerAvailable<T, U>(Node RosNode, Client<T, U> ServiceClient)
			where T : MessageWrapper, new()
			where U : MessageWrapper, new()
        {
            return Impl.ServiceServerAvailable< T, U > (RosNode, ServiceClient);
        }

    }
	
	

}


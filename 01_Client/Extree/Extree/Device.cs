using System;
using System.IO.Ports;
using System.Linq;

namespace Extree
{
	public class Device : IDisposable
	{
		public const int NUM_LED = 16;
		private SerialPort port;
		public bool IsDisposed { get; private set; }

		public Device(string portname)
		{
			port = new SerialPort(portname, 115200);
			port.Open();
		}

		public void ExecuteCommand(ICommand cmd) {
			if(IsDisposed)
				throw new InvalidOperationException("Device has been disposed");

			byte[] bytes = cmd.ToBytes();
			port.Write(bytes, 0, bytes.Length);
		}

		public void Dispose ()
		{
			port.Close();
			IsDisposed = true;
		}

	}
}


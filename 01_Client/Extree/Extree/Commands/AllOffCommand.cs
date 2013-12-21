using System;

namespace Extree
{
	public class AllOffCommand : ICommand
	{
		public byte[] ToBytes ()
		{
			return new byte[] { (byte)'o' };
		}

		public override string ToString ()
		{
			return string.Format ("[AllOffCommand]");
		}

	}
}


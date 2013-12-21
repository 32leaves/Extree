using System;

namespace Extree
{
	public class SetColorCommand : ICommand
	{
		public static SetColorCommand FromString(string[] args)
		{
			return new SetColorCommand(int.Parse(args[0]), new Color(args[1]));
		}

		private readonly int led;
		private readonly Color color;

		public SetColorCommand (int led, Color color)
		{
			this.led = led;
			this.color = color;
		}		

		public byte[] ToBytes ()
		{
			byte[] result = new byte[5];
			result[0] = (byte)'c';
			color.ToBytes().CopyTo(result, 1);
			result[4] = (byte)'!';
			return result;
		}

		public override string ToString ()
		{
			return string.Format ("[SetColorCommand] led={0} color={1}", led, color);
		}

	}
}


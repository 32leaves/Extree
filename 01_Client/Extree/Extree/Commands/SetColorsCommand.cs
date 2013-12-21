using System;
using System.Linq;

namespace Extree
{
	public class SetColorsCommand : ICommand
	{
		public static SetColorsCommand FromString (string[] colors)
		{
			if (colors.Length < Device.NUM_LED && colors.Length != 1)
				throw new ArgumentException (String.Format ("At least {0} colors are required.", Device.NUM_LED));

			return new SetColorsCommand(Enumerable.Range(0, Device.NUM_LED).Select((idx) => new Color(colors[colors.Length > 1 ? idx : 0])).ToArray());
		}

		private readonly Color[] colors;

		public SetColorsCommand (Color[] colors)
		{
			if (colors.Length < Device.NUM_LED)
				throw new ArgumentException (String.Format ("At least {0} colors are required.", Device.NUM_LED));

			this.colors = colors;
		}

		public byte[] ToBytes ()
		{
			byte[] result = new byte[Device.NUM_LED * 3 + 2];
			result[0] = (byte)'s';
			Enumerable.Range(0, Device.NUM_LED).SelectMany((idx) => colors[idx].ToBytes()).ToArray().CopyTo(result, 1);
			result[Device.NUM_LED * 3 + 1] = (byte)'!';
			return result;
		}

		public override string ToString ()
		{
			return string.Format ("[SetColorsCommand] {0}", colors.Select(c => c.ToString()).Aggregate((a, b) => a + " " + b));
		}

	}
}


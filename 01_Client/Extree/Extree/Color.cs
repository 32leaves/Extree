using System;

namespace Extree
{
	public class Color
	{
		public readonly byte R, G, B;

		public Color (byte r, byte g, byte b)
		{
			this.R = r;
			this.G = g;
			this.B = b;
		}

		public Color (string hex) : this(Convert.ToUInt32(hex.Substring(1), 16))
		{
		}

		public Color (UInt32 color) : this((byte)((color >> 16) & 0xFF), (byte)((color >> 8) & 0xFF), (byte)((color >> 0) & 0xFF))
		{
		}

		public byte[] ToBytes ()
		{
			return new byte[] { R, G, B };
		}

	}
}


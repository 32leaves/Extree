using System;

namespace Extree
{
	public interface ICommand
	{

		string ToString();

		byte[] ToBytes();

	}
}


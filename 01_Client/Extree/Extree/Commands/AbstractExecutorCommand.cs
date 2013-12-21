using System;

namespace Extree
{
	public abstract class AbstractExecutorCommand : ICommand
	{

		public abstract void Run(CommandExecutor executor);

		#region ICommand implementation
		public byte[] ToBytes ()
		{
			return new byte[0];
		}
		#endregion

	}
}


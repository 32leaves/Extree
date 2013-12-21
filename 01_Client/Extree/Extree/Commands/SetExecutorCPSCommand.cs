using System;

namespace Extree
{
	public class SetExecutorCPSCommand : AbstractExecutorCommand
	{
		public static SetExecutorCPSCommand FromString(string[] args) {
			return new SetExecutorCPSCommand(int.Parse(args[0]));
		}

		private readonly int cps;

		public SetExecutorCPSCommand (int cps)
		{
			if(cps < 0 || cps > 300)
				throw new ArgumentOutOfRangeException("cps", "Commands per seconds must be between 1 and 300");

			this.cps = cps;
		}

		public override void Run (CommandExecutor executor)
		{
			executor.CommandsPerSecond = cps;
		}

	}
}


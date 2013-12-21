using System;

namespace Extree
{
	public class RepeatExecutionCommand : AbstractExecutorCommand
	{
		public static RepeatExecutionCommand FromString(string[] args) {
			return new RepeatExecutionCommand(args.Length == 0 ? -42 : int.Parse(args[0]));
		}

		private int repeatCount;

		public RepeatExecutionCommand(int repeatCount) {
			this.repeatCount = repeatCount;
		}

		#region implemented abstract members of Extree.AbstractExecutorCommand
		public override void Run (CommandExecutor executor)
		{
			if (repeatCount == -42) {
				executor.ShouldRepeat = true;
			} else if (repeatCount > 1) {
				executor.ShouldRepeat = true;
				repeatCount--;
			} else {
				executor.ShouldRepeat = false;
			}

			Console.WriteLine();
		}
		#endregion


	}
}


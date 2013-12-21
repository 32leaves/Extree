using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Extree
{
	public class CommandExecutor
	{
		public delegate void ExecutionFinishedCallback();
		public delegate void CommandFinishedCallback(ICommand cmd);

		private readonly Device device;

		public int CommandsPerSecond { get; set; }
		public bool ShouldRepeat { get; set; }

		public CommandExecutor (Device device)
		{
			this.device = device;
			CommandsPerSecond = 50;
		}

		public event CommandFinishedCallback OnCommandFinished;
		public event ExecutionFinishedCallback OnExecutionFinished;

		public void Execute (IEnumerable<ICommand> commands)
		{
			new Thread(() => {
				ShouldRepeat = true;
				while(ShouldRepeat) {
					ShouldRepeat = false;
					foreach (ICommand cmd in commands) {
						if(cmd is AbstractExecutorCommand) {
							((AbstractExecutorCommand) cmd).Run(this);
						} else {
							device.ExecuteCommand(cmd);
							Thread.Sleep((int) (1000 / CommandsPerSecond));
						}
						if(OnCommandFinished != null) OnCommandFinished(cmd);
					}
				}

				if(OnExecutionFinished != null) OnExecutionFinished();
			}).Start();
		}

	}
}


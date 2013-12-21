using System;
using System.Linq;
using System.Collections.Generic;

namespace Extree
{
	public class CommandParser
	{
		public delegate ICommand CommandFactory(string[] args);

		private CommandParser ()
		{
		}

		private static IDictionary<string, CommandFactory> REGISTRY = new Dictionary<string, CommandFactory>()
		{
			{ "color", SetColorCommand.FromString },
			{ "colors", SetColorsCommand.FromString },
			{ "off", (a) => new AllOffCommand() },
			{ "#cps", SetExecutorCPSCommand.FromString },
			{ "#repeat", RepeatExecutionCommand.FromString }
		};

		public static ICommand ParseOne (string singleCommand)
		{
			string[] parts = singleCommand.Split (' ');
			if (parts.Length < 1)
				throw new ArgumentException ("Command must have at least one token");

			string id = parts [0];
			if (!REGISTRY.ContainsKey (id)) {
				throw new ArgumentOutOfRangeException ("singleCommand", string.Format ("Unknown command: {0}", id));
			} else {
				return REGISTRY[id](parts.Skip(1).ToArray ());
			}
		}

		public static IEnumerable<ICommand> Parse (string[] commands)
		{
			return commands.Where((cmd) => cmd.Trim().Length > 0).Select((cmd) => ParseOne(cmd));
		}

	}
}


using System;
using Mono.Options;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Extree
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			string serialPort = null;
			string command = null;
			bool showHelp = false;
			bool serverMode = false;

			var options = new OptionSet () {
				{ "p|port=", "the serial port Extree is connected to", v => serialPort = v },
				{ "c|cmd=", "the command to execute", v => command = v },
				{ "s|server", "go into server mode", v => serverMode = true },
				{ "h|help", "print this help", v => showHelp = true },
			};
			List<string> scripts;
			try {
				scripts = options.Parse (args);

				if(serialPort == null) {
					throw new OptionException("Port option is required", "port");
				}
			} catch (OptionException e) {
				Console.Write ("extree: ");
				Console.WriteLine (e.Message);
				Console.WriteLine ("Try --help for more information");
				return;
			}

			if (showHelp) {
				Console.WriteLine ("Usage: extree [OPTIONS]+ <script>?");
				Console.WriteLine ("Controls an ExTree");
				Console.WriteLine ("If no script or command is specified, STDIN is read.");
				Console.WriteLine ();
				Console.WriteLine ("Options:");
				options.WriteOptionDescriptions (Console.Out);
				return;
			}

			List<ICommand> commands = new List<ICommand> ();
			if (command != null) {
				commands.Add (CommandParser.ParseOne (command));
			}
			if (scripts.Count > 0) {
				commands.AddRange (CommandParser.Parse (File.ReadAllLines (scripts [0])));
			}

			if (commands.Count > 0) {
				ExecuteCommands (serialPort, commands);
			} else if(serverMode) {
				StartServerMode(serialPort);
			} else {
				ExecuteFromSTDIN(serialPort);
			}
		}

		static void ExecuteCommands (string serialPort, List<ICommand> commands)
		{
			using(Device device = new Device(serialPort)) {
				Thread.Sleep(500);
				if(commands.Count > 0) Thread.Sleep(500);

				SemaphoreSlim lck = new SemaphoreSlim(0);
				CommandExecutor executor = new CommandExecutor(device);
				executor.OnCommandFinished   += (cmd) => Console.Write(".");
				executor.OnExecutionFinished += ()    => lck.Release();
				executor.Execute(commands);
				lck.Wait(-1);
				Console.WriteLine();
			}
		}	

		static void StartServerMode (string serialPort)
		{
			using (Device device = new Device(serialPort)) {
				Thread.Sleep (500);

				CommandExecutor executor = new CommandExecutor (device);
				executor.OnCommandFinished += (cmd) => Console.Write (".");

				IPAddress ipAddress = Dns.GetHostAddresses(Dns.GetHostName())[0];
				Socket listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp );
				listener.Bind(new IPEndPoint(ipAddress, 11000));
            	listener.Listen(10);

				while (true) {
					Console.WriteLine("Waiting for a connection on {0}:11000 ...", ipAddress);
	                // Program is suspended while waiting for an incoming connection.
	                Socket handler = listener.Accept();
	                string data = "";

	                // An incoming connection needs to be processed.
	                while (true) {
	                    byte[] bytes = new byte[1024];
	                    int bytesRec = handler.Receive(bytes);
	                    data += Encoding.ASCII.GetString(bytes,0,bytesRec);
	                    if (bytesRec == 0) {
	                        break;
	                    }
	                }

					string[] lines = data.Split('\n');
					try {
						executor.Execute(CommandParser.Parse (lines));
						handler.Send(Encoding.ASCII.GetBytes("Done"));
					} catch(ArgumentException e) {
						handler.Send(Encoding.ASCII.GetBytes(string.Format("Error: ", e.Message)));
					}

	                handler.Shutdown(SocketShutdown.Both);
	                handler.Close();
	            }
			}
		}

		static void ExecuteFromSTDIN (string serialPort)
		{
			using (Device device = new Device(serialPort)) {
				Thread.Sleep (500);

				CommandExecutor executor = new CommandExecutor(device);
				executor.OnCommandFinished += (cmd) => Console.Write(".");
				for (string line = Console.ReadLine(); line != null; line = Console.ReadLine()) {
					executor.Execute(new ICommand[] { CommandParser.ParseOne(line) });
				}
			}
		}

	}
}

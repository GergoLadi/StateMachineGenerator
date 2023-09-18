namespace StateMachineGenerator
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var checkedArguments = CheckArgs(args);
            var stateMachine = StateMachineGenerator.Generate((uint) checkedArguments["numStates"],
                                                              (uint) checkedArguments["pctSelfLoop"],
                                                              (uint) checkedArguments["pctRandomEdges"],
                                                              (uint) checkedArguments["pctReuseInputChar"],
                                                              (uint) checkedArguments["pctReuseOutputChar"]);

            var fsmData = stateMachine.Dump();
            if (checkedArguments.ContainsKey("outputFileName"))
            {
                string fileName = (string) checkedArguments["outputFileName"];
                File.WriteAllText(fileName, fsmData);
            } else
            {
                Console.Write(fsmData);
            }
        }

        private static void PrintUsage()
        {
            var appName = typeof(Program).Assembly.GetName().Name;
            Console.WriteLine($"Usage: {appName} <numStates> <pctSelfLoop> <pctRandomEdges> " +
                              $"<pctReuseInputChar> <pctReuseOutputChar> " +
                              $"[outputFileName]");
            Console.WriteLine();
            Console.WriteLine("Parameters: ");
            Console.WriteLine("\tnumStates (integer, 2+, required): The number of states in the state machine.");
            Console.WriteLine("\tpctSelfLoop (integer, 0-100, required): The probability (in %) that a state will have a self loop.");
            Console.WriteLine("\tpctRandomEdges (integer, 0-100, required): The probability (in %) that two states will be connected.");
            Console.WriteLine("\tpctReuseInputChar (integer, 0-100, required): The probability (in %) that a previously used \r\n\t\t" +
                              "input character will be reused in a transition.");
            Console.WriteLine("\tpctReuseOutputChar (integer, 0-100, required): The probability (in %) that a previously used \r\n\t\t" +
                              "output character will be reused in a transition.");
            Console.WriteLine("\toutputFileName (string, optional): If provided, the output will be written to this file instead of \r\n\t\t" +
                              "being printed to the console.");
        }

        private static Dictionary<string, object> CheckArgs(string[] args)
        {
            if (args.Length < 5)
            {
                PrintUsage();
                Environment.Exit(-1);
            }

            var parsedArgs = new Dictionary<string, object>();
            UInt32 tempUInt;
            int currentArgIndex = 0;

            if (!UInt32.TryParse(args[currentArgIndex], out tempUInt) || tempUInt < 2)
            {
                Console.WriteLine("Error: numStates must be a positive integer and greater than or equal to 2.");
                Environment.Exit(-2);
            }
            parsedArgs["numStates"] = tempUInt;
            currentArgIndex++;

            if (!UInt32.TryParse(args[currentArgIndex], out tempUInt) || tempUInt < 0 || tempUInt > 100)
            {
                Console.WriteLine("Error: pctSelfLoop must be an integer between 0 and 100.");
                Environment.Exit(-3);
            }
            parsedArgs["pctSelfLoop"] = tempUInt;
            currentArgIndex++;

            if (!UInt32.TryParse(args[currentArgIndex], out tempUInt) || tempUInt < 0 || tempUInt > 100)
            {
                Console.WriteLine("Error: pctRandomEdges must be an integer between 0 and 100.");
                Environment.Exit(-4);
            }
            parsedArgs["pctRandomEdges"] = tempUInt;
            currentArgIndex++;

            if (!UInt32.TryParse(args[currentArgIndex], out tempUInt) || tempUInt < 0 || tempUInt > 100)
            {
                Console.WriteLine("Error: pctReuseInputChar must be an integer between 0 and 100.");
                Environment.Exit(-5);
            }
            parsedArgs["pctReuseInputChar"] = tempUInt;
            currentArgIndex++;

            if (!UInt32.TryParse(args[currentArgIndex], out tempUInt) || tempUInt < 0 || tempUInt > 100)
            {
                Console.WriteLine("Error: pctReuseOutputChar must be an integer between 0 and 100.");
                Environment.Exit(-6);
            }
            parsedArgs["pctReuseOutputChar"] = tempUInt;
            currentArgIndex++;

            if (args.Length > currentArgIndex)
            {
                string filePath = args[currentArgIndex];
                var directory = new FileInfo(filePath).Directory;
                if (directory == null || !directory.Exists)
                {
                    Console.WriteLine("Error: the path specified for outputFileName does not exist.");
                    Environment.Exit(-7);
                }

                parsedArgs["outputFileName"] = filePath;
                currentArgIndex++;
            }

            return parsedArgs;
        }
    }
}

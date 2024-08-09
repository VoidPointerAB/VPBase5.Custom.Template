using BundlerMinifier;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace VPBase.Bundler
{
    class Program
    {
        static int Main(params string[] args)
        {
            int readConfigsUntilIndex = args.Length;
            string configPath;
            if (GetConfigFileFromArgs(args, out configPath))
            {
                --readConfigsUntilIndex;
            }

            var arguments = new List<string>();
            foreach(var arg in args)
            {
                arguments.Add(arg);
            }

            return ProgramHandler.Execute(arguments, configPath);
        }

        private static bool GetConfigFileFromArgs(string[] args, out string configPath)
        {
            Console.WriteLine("GetConfigFileFromArgs. Found arguments Count: " + args.Length);

            int index = args.Length - 1;
            IEnumerable<Bundle> bundles;
            bool fileExists = false;
            bool fallbackExists = fileExists = File.Exists(ProgramHandler.DefaultConfigFileName);

            if (index > -1)
            {
                var argument = args[index];

                Console.WriteLine("Fist argument is: " + argument);

                fileExists = File.Exists(argument);

                Console.WriteLine("File Exists: " + fileExists);

                if (BundleHandler.TryGetBundles(argument, out bundles))
                {
                    configPath = argument;

                    Console.WriteLine("Try get bundles succeeded! Num of bundles found: " + bundles.Count());

                    return true;
                }
                else
                {
                    Console.WriteLine("Try get bundles failed!");
                }
            }

            if (BundleHandler.TryGetBundles(ProgramHandler.DefaultConfigFileName, out bundles))
            {
                configPath = new FileInfo(ProgramHandler.DefaultConfigFileName).FullName;
                return false;
            }

            if (args.Length > 0)
            {
                if (!fileExists)
                {
                    Console.WriteLine($"A configuration file called {args[index]} could not be found".Red().Bright());
                }
                else
                {
                    Console.WriteLine($"Configuration file {args[index]} has errors".Red().Bright());
                }
            }

            if (!fallbackExists)
            {
                Console.WriteLine($"A configuration file called {ProgramHandler.DefaultConfigFileName} could not be found".Red().Bright());
            }
            else
            {
                Console.WriteLine($"Configuration file {ProgramHandler.DefaultConfigFileName} has errors".Red().Bright());
            }

            configPath = null;
            return false;
        }
    }
}

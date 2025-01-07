//-----------------------------------------------------------------------
// <copyright file="MicroBenchmark.cs" company="Lifeprojects.de">
//     Class: MicroBenchmark
//     Copyright © Gerhard Ahrens, 2019
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>development@lifeprojects.de</email>
// <date>11.09.2019</date>
//
// <summary>Class for MicroBenchmark</summary>
//-----------------------------------------------------------------------

namespace ModernBaseLibrary.Core
{
    using System;
    using System.Collections;
    using System.Diagnostics;
    using System.Reflection;

    public class MicroBenchmark
    {
        private static int runIterations = 1;
        private static Stopwatch stopwatch = null;

        public static void Run(string[] args)
        {
            args = ParseCommandLine(args);

            if (args == null)
            {
                args = new string[0];
            }

            if (stopwatch == null)
            {
                stopwatch = new Stopwatch();
            }

            BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.IgnoreCase | BindingFlags.Static | BindingFlags.Instance;

            foreach (Type type in Assembly.GetCallingAssembly().GetTypes())
            {
                // Find an Init method taking string[], if any
                MethodInfo initMethod = type.GetMethod("Init", bindingFlags, null,
                                                      new Type[] { typeof(string[]) },
                                                      null);

                // Find a parameterless Reset method, if any
                MethodInfo resetMethod = type.GetMethod("Reset", bindingFlags,
                                                       null, new Type[0],
                                                       null);

                // Find a parameterless Check method, if any
                MethodInfo checkMethod = type.GetMethod("Check", bindingFlags,
                                                      null, new Type[0],
                                                      null);

                // Find all parameterless methods with the [Benchmark] attribute
                ArrayList benchmarkMethods = new ArrayList();
                foreach (MethodInfo method in type.GetMethods(bindingFlags))
                {
                    ParameterInfo[] parameters = method.GetParameters();
                    if (parameters != null && parameters.Length != 0)
                    {
                        continue;
                    }

                    if (method.GetCustomAttributes
                        (typeof(BenchmarkAttribute), false).Length != 0)
                    {
                        benchmarkMethods.Add(method);
                    }
                }

                if (benchmarkMethods.Count == 0)
                {
                    continue;
                }

                Console.WriteLine($"Benchmarking type {type.Name}");

                try
                {
                    if (initMethod != null)
                    {
                        initMethod.Invoke(null, new object[] { args });
                    }
                }
                catch (TargetInvocationException e)
                {
                    Exception inner = e.InnerException;
                    string message = (inner == null ? null : inner.Message);
                    if (message == null)
                    {
                        message = "(No message)";
                    }

                    Console.WriteLine($"Init failed ({message})");
                    continue; // Next type
                }

                for (int i = 0; i < runIterations; i++)
                {
                    if (runIterations != 1)
                    {
                        Console.WriteLine($"Run #{i + 1}");
                    }

                    foreach (MethodInfo method in benchmarkMethods)
                    {
                        try
                        {
                            if (resetMethod != null)
                            {
                                resetMethod.Invoke(null, null);
                            }

                            GC.Collect();
                            GC.WaitForPendingFinalizers();
                            GC.Collect();

                            stopwatch.Start();
                            method.Invoke(null, null);
                            stopwatch.Stop();

                            if (checkMethod != null)
                            {
                                checkMethod.Invoke(null, null);
                            }

                            Console.WriteLine("  {0,-20} {1}", method.Name, stopwatch.Elapsed.ToString("c"));
                        }
                        catch (TargetInvocationException e)
                        {
                            Exception inner = e.InnerException;
                            string message = (inner == null ? null : inner.Message);
                            if (message == null)
                            {
                                message = "(No message)";
                            }

                            Console.WriteLine($"  {method.Name}: Failed ({message})");
                        }
                    }
                }
            }

            stopwatch = null;
        }

        public static void Run(string name, int iterations, Action action)
        {
            Stopwatch watch = null;
            try
            {
                Console.Write($"Running benchmark '{name}' for {iterations} iterations... ");

                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect();

                action.Invoke();

                watch = Stopwatch.StartNew();
                for (int i = 0; i < iterations; i++)
                {
                    action.Invoke();
                }

                watch.Stop();

                Console.WriteLine($"Elapsed time {watch.ElapsedMilliseconds / iterations} ms");
            }
            catch (OutOfMemoryException)
            {
                Console.WriteLine($"Out of memory!");
            }
            finally
            {
                watch = null;
            }
        }
        private static string[] ParseCommandLine(string[] args)
        {
            if (args == null)
            {
                return new string[0];
            }

            for (int i = 0; i < args.Length; i++)
            {
                switch (args[i])
                {
                    case "-runtwice":
                        runIterations = 2;
                        break;

                    case "-version":
                        PrintEnvironment();
                        break;

                    case "-endoptions":
                        {
                            string[] ret = new string[args.Length - i - 1];
                            Array.Copy(args, i + 1, ret, 0, ret.Length);
                            return ret;
                        }

                    default:
                        {
                            string[] ret = new string[args.Length - i];
                            Array.Copy(args, i, ret, 0, ret.Length);
                            return ret;
                        }
                }
            }

            return new string[0];
        }

        /// <summary>
        /// Prints out information about the operating environment.
        /// </summary>
        static void PrintEnvironment()
        {
            Console.WriteLine($"Operating System: {Environment.OSVersion}");
            Console.WriteLine($"Runtime version: {Environment.Version}");
            Console.WriteLine($"Startdate: {DateTime.Now}");
        }
    }
}

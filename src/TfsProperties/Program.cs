namespace TfsProperties
{
    using System;
    using ManyConsole;

    internal class Program
    {
        internal static void Main(string[] args)
        {
            var commands = ConsoleCommandDispatcher.FindCommandsInSameAssemblyAs(typeof(Program));
            ConsoleCommandDispatcher.DispatchCommand(commands, args, Console.Out);
        }
    }
}

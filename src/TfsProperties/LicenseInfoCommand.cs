namespace TfsProperties
{
    using System;
    using ManyConsole;

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses", Justification = "Lazy Loading")]
    internal class LicenseInfoCommand : ConsoleCommand
    {
        public LicenseInfoCommand()
        {
            IsCommand("License", "Displays licensing information.");
        }

        public override int Run(string[] remainingArguments)
        {
            // Write our own license
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(Properties.Resources.TfsPropertiesLicenseIntro);
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine(Properties.Resources.TfsPropertiesLicense);

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(Properties.Resources.ManyConsoleLicenseIntro);
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine(Properties.Resources.ManyConsoleLicense);
            
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(Properties.Resources.NDeskOptionsLicenseIntro);
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine(Properties.Resources.NDeskOptionsLicense);
            return StatusCode.Success;
        }
    }
}

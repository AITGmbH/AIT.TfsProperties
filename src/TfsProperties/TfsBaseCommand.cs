namespace TfsProperties
{
    using System;
    using ManyConsole;
    using Microsoft.TeamFoundation.Client;

    /// <summary>
    /// Class for the basic command of TfsProperties.
    /// </summary>
    public abstract class TfsBaseCommand : ConsoleCommand
    {
        private string collectionUrl;
        private string projectName;

        /// <summary>
        /// Defines the structure of the basic command of TfsProperties. Required Options are "c|collection=" and "p|project=".
        /// </summary>
        protected TfsBaseCommand()
        {
            HasRequiredOption("c|collection=", "The Url of the team project collection.", x => collectionUrl = x);
            HasRequiredOption("p|project=", "The name of the team project.", x => projectName = x);
        }

        /// <summary>
        /// Gets the url of the collection.
        /// </summary>
        protected string CollectionUrl
        {
            get
            {
                return collectionUrl;
            }
        }

        /// <summary>
        /// Gets the name of the team project.
        /// </summary>
        protected string ProjectName
        {
            get
            {
                return projectName;
            }
        }

        /// <summary>
        /// Determines the behaviour of the basic command of TfsProperties.
        /// </summary>
        /// <param name="remainingArguments"></param>
        /// <returns>The status code for success.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1303:Do not pass literals as localized parameters", MessageId = "System.Console.WriteLine(System.String,System.Object)", Justification = "Application not localized")]
        public override int Run(string[] remainingArguments)
        {
            var startForegroundColor = Console.ForegroundColor;
            var startBackgroundColor = Console.BackgroundColor;

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Connecting to TFS '{0}'", CollectionUrl);
            int result = StatusCode.Success;
            using (var collection = TfsTeamProjectCollectionFactory.GetTeamProjectCollection(new Uri(CollectionUrl)))
            {
                Console.ForegroundColor = startForegroundColor;
                Console.BackgroundColor = startBackgroundColor;
                collection.EnsureAuthenticated();
                result = RunTfsCommand(collection, remainingArguments);
                Console.ForegroundColor = startForegroundColor;
                Console.BackgroundColor = startBackgroundColor;
            }

            Console.ForegroundColor = startForegroundColor;
            Console.BackgroundColor = startBackgroundColor;
            return result;
        }

        /// <summary>
        /// Base method for the bahviour of the single commands.
        /// </summary>
        /// <param name="collection">Collection in which the team project, for which the properties should be listed, is located. </param>
        /// <param name="remainingArguments"></param>
        /// <returns></returns>
        protected abstract int RunTfsCommand(TfsTeamProjectCollection collection, string[] remainingArguments);
    }
}

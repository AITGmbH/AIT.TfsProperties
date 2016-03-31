namespace TfsProperties
{
    using System;
    using Microsoft.TeamFoundation.Client;
    using Microsoft.TeamFoundation.Server;

    /// <summary>
    /// Class for the command to list all properties of a team project
    /// </summary>
    public class ListPropertiesCommand : TfsBaseCommand
    {
        /// <summary>
        /// Command to list all properties of a team project.
        /// </summary>
        public ListPropertiesCommand()
        {
            IsCommand("List", "Lists the properties of the project");
        }

        /// <summary>
        /// Determines the behaviour of the command to list all properties of a team project.
        /// </summary>
        /// <param name="collection">Collection in which the team project, for which the properties should be listed, is located. </param>
        /// <param name="remainingArguments"></param>
        /// <returns>The status code for success.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1303:Do not pass literals as localized parameters", MessageId = "System.Console.WriteLine(System.String,System.Object,System.Object)", Justification = "Application is not localized")]
        protected override int RunTfsCommand(TfsTeamProjectCollection collection, string[] remainingArguments)
        {
            if (collection == null)
            { 
                throw new ArgumentNullException("collection", "The specified collection is not valid.");
            }

            var structureService = collection.GetService<ICommonStructureService>();
            var project = structureService.GetProjectFromName(ProjectName);
            string name;
            string state;
            int templateId;
            ProjectProperty[] properties;
            structureService.GetProjectProperties(project.Uri, out name, out state, out templateId, out properties);
            foreach (var projectProperty in properties)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("{0} = {1}", projectProperty.Name, projectProperty.Value);
            }

            return StatusCode.Success;
        }
    }
}
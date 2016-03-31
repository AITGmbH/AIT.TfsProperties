namespace TfsProperties
{
    using System;
    using System.Linq;
    using Microsoft.TeamFoundation.Client;
    using Microsoft.TeamFoundation.Server;

    /// <summary>
    /// Class for the command to edit or delete a property of a team project.
    /// </summary>
    public class UpdatePropertyCommand : TfsBaseCommand
    {
        private string propertyName;
        private string propertyValue;
        private bool force;
        private bool deleteProperty;

        /// <summary>
        /// Command to edit or delete a property of a team project.
        /// </summary>
        public UpdatePropertyCommand()
        {
            IsCommand("Set", "Sets a property on the team project.");
            HasRequiredOption("n|name=", "The name of the property.", x => propertyName = x);
            HasOption("v|value=", "The value of the property.", x => propertyValue = x);
            HasOption("d|delete", "Delete the property.", x => deleteProperty = true);
            HasOption("f|force", "Force to write the property even if it did not exist before.", x => force = true);
        }

        /// <summary>
        /// Gets the name of a team project property.
        /// </summary>
        protected string PropertyName
        {
            get
            {
                return propertyName;
            }
        }

        /// <summary>
        /// Gets the value of a team project property.
        /// </summary>
        protected string PropertyValue
        {
            get
            {
                return propertyValue;
            }
        }

        /// <summary>
        /// Gets the indicator whether to write the property even if it did not exist before in the team project.
        /// </summary>
        protected bool Force
        {
            get
            {
                return force;
            }
        }

        /// <summary>
        /// Gets the indicator whether to delete a property of a team project.
        /// </summary>
        protected bool DeleteProperty
        {
            get
            {
                return deleteProperty;
            }
        }

        /// <summary>
        /// Determines the behaviour of the command to to edit or delete a property of a team project.
        /// </summary>
        /// <param name="collection">Collection in which the team project, for which the property should be edit or deleted, is located. </param>
        /// <param name="remainingArguments"></param>
        /// <returns>The status code for success.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1303:Do not pass literals as localized parameters", MessageId = "System.Console.WriteLine(System.String,System.Object)", Justification = "Application not localized")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1303:Do not pass literals as localized parameters", MessageId = "System.Console.WriteLine(System.String,System.Object,System.Object)", Justification = "Application not localized")]
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
            Console.WriteLine();
            structureService.GetProjectProperties(project.Uri, out name, out state, out templateId, out properties);
            var projectProperty = properties.FirstOrDefault(x => x.Name == PropertyName);
            if (projectProperty == null)
            {
                if (Force)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Property '{0}' was not found. Adding a new one with value '{1}'", PropertyName, PropertyValue);
                    var newProperties = new ProjectProperty[properties.Length + 1];
                    properties.CopyTo(newProperties, 0);
                    newProperties[newProperties.Length - 1] = new ProjectProperty(PropertyName, PropertyValue);
                    structureService.UpdateProjectProperties(project.Uri, state, newProperties.ToArray());
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(
                        "Property '{0}' was not found. use the -force option in order to create it. Only the following properties exist: {1}",
                        PropertyName,
                        string.Concat(properties.Select(x => "'" + x.Name + "',")));
                }
            }
            else
            {
                if (DeleteProperty)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("Deleting property '{0}'", PropertyName);

                    // Select all but the specified property
                    var relevantProperties = from property in properties
                                             where property.Name != PropertyName
                                             select property;
                    structureService.UpdateProjectProperties(project.Uri, state, relevantProperties.ToArray());
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Setting property '{0}' to value '{1}'", PropertyName, PropertyValue);
                    projectProperty.Value = PropertyValue;
                    structureService.UpdateProjectProperties(project.Uri, state, properties);
                }
            }

            return StatusCode.Success;
        }
    }
}
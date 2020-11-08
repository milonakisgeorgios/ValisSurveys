using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

[assembly: log4net.Config.XmlConfigurator(Watch = true)]

[assembly: InternalsVisibleTo("Valis.Core.SqlServer")]
[assembly: InternalsVisibleTo("Valis.Core.UnitTests")]
[assembly: InternalsVisibleTo("ValisManager")]
[assembly: InternalsVisibleTo("ValisServer")]
[assembly: InternalsVisibleTo("ValisReporter")]
[assembly: InternalsVisibleTo("ValisApplicationService")]

// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle("Valis.Core")]
[assembly: AssemblyDescription("Valis Survey Core")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("George Milonakis")]
[assembly: AssemblyProduct("An ASP.NET (WebForms) Survey System by George Milonakis")]
[assembly: AssemblyCopyright("Copyright © George Milonakis")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

// Setting ComVisible to false makes the types in this assembly not visible 
// to COM components.  If you need to access a type in this assembly from 
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("462bdbbd-ce90-4040-8360-614867871331")]


[assembly: AssemblyVersion("2014.12.29.0")]
[assembly: AssemblyFileVersion("2014.12.29.0")]

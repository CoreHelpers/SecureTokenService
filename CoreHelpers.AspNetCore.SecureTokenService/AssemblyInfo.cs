using System;
using System.Reflection;

[assembly: AssemblyCompany("Core Helpers")]
[assembly: AssemblyProduct("SecureTokenService")]
[assembly: AssemblyTitle("CoreHelpers Secure Token Service")]

[assembly: AssemblyFileVersion("1.1.0.0")]
[assembly: AssemblyVersion("1.1.0.0")]

#if (DEBUG)
[assembly: AssemblyConfiguration("Debug")]
#else
[assembly: AssemblyConfiguration("Release")]
#endif
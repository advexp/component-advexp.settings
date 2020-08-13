using System.Reflection;

// General assembly properties
[assembly: AssemblyCompany("Advexp")]
#if __TDD__
[assembly: AssemblyProduct("Advexp.Settings, TDD version")]
#else
[assembly: AssemblyProduct("Advexp.Settings")]
#endif // __TDD__

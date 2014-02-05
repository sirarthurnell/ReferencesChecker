using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mono.Cecil;
using AssemblyCheck.Token;

namespace AssemblyCheck
{
    /// <summary>
    /// Contains defferent methods for reading assemblies
    /// from different sources.
    /// </summary>
    public static class AssemblyReader
    {
        /// <summary>
        /// Read the specified assembly and returns info
        /// about it.
        /// </summary>
        /// <param name="pathToAssembly">The assembly's path.</param>
        /// <returns>Info about the assembly.</returns>
        public static AssemblyInfo ReadAssembly(string pathToAssembly)
        {
            var assembly = AssemblyDefinition.ReadAssembly(pathToAssembly);
            var references = from module in assembly.Modules
                             from reference in module.AssemblyReferences
                             select reference;

            var info = CreateInfo(assembly);
            foreach (var reference in references)
            {
                info.References.Add(CreateInfo(reference));
            }

            return info;
        }

        /// <summary>
        /// Obtains info from a name reference of Mono.Cecil.
        /// </summary>
        /// <param name="reference">Name reference.</param>
        /// <returns>Info about the assembly.</returns>
        private static AssemblyInfo CreateInfo(AssemblyNameReference reference)
        {
            AssemblyInfo info = new AssemblyInfo
            {
                Name = reference.Name,
                FullName = reference.FullName,
                Version = new AssemblyVersion(reference.Version.ToString()),
                PublicKeyToken = reference.PublicKeyToken.TranslateToken(),
                Culture = reference.Culture
            };

            return info;
        }

        /// <summary>
        /// Obtains info from an assembly definition of Mono.Cecil.
        /// </summary>
        /// <param name="assembly">Assembly definition.</param>
        /// <returns>Info about the assembly.</returns>
        private static AssemblyInfo CreateInfo(AssemblyDefinition assembly)
        {
            AssemblyNameDefinition name = assembly.Name;
            AssemblyInfo info = new AssemblyInfo
            {
                Name = name.Name,
                FullName = name.FullName,
                Version = new AssemblyVersion(name.Version.ToString()),
                PublicKeyToken = name.PublicKeyToken.TranslateToken(),
                Culture = name.Culture
            };

            return info;
        }
    }
}

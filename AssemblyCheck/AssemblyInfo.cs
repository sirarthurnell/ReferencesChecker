using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mono.Cecil;

namespace AssemblyCheck
{
    /// <summary>
    /// Contains info about an assembly.
    /// </summary>
    public class AssemblyInfo
    {
        public string Path { get; private set; }
        public string Name { get; private set; }
        public string FullName { get; private set; }
        public AssemblyVersion Version { get; private set; }
        public string Culture { get; private set; }
        public string PublicKeyToken { get; private set; }

        private List<AssemblyInfo> _references = new List<AssemblyInfo>();
        public List<AssemblyInfo> References
        { 
            get
            {
                return _references;
            }
        }

        /// <summary>
        /// Gets the full name of the assembly.
        /// </summary>
        /// <returns>Full name of the assembly.</returns>
        public override string ToString()
        {
            return FullName;
        }

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
            info.Path = pathToAssembly;

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
                PublicKeyToken = TokenUtils.TranslateToken(reference.PublicKeyToken),
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
                PublicKeyToken = TokenUtils.TranslateToken(name.PublicKeyToken),
                Culture = name.Culture
            };

            return info;
        }        
    }
}

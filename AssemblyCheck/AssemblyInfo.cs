using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AssemblyCheck
{
    /// <summary>
    /// Contains info about an assembly.
    /// </summary>
    public class AssemblyInfo
    {
        public string Name { get; internal set; }
        public string FullName { get; internal set; }
        public AssemblyVersion Version { get; internal set; }
        public string Culture { get; internal set; }
        public string PublicKeyToken { get; internal set; }

        private List<AssemblyInfo> _references = new List<AssemblyInfo>();
        public List<AssemblyInfo> References
        { 
            get
            {
                return _references;
            }
        }

        /// <summary>
        /// Creates a new empty instance of AssemblyInfo.
        /// </summary>
        internal AssemblyInfo()
        {
        }

        /// <summary>
        /// Creates a new instance of AssemblyInfo based
        /// on the specified full name of assembly.
        /// </summary>
        /// <param name="fullName">Full name of assembly.</param>
        public AssemblyInfo(string fullName)
        {
            string[] parts = fullName.Split(',');
            if (parts.Length < 4)
            {
                throw new ArgumentException("Full name specified without full information.", "fullName");
            }

            string nameToApply = string.Empty,
                   versionToApply = string.Empty,
                   cultureToApply = string.Empty;

            nameToApply = parts[0].Trim();
            if (nameToApply.Contains("="))
            {
                throw new ArgumentException("The name of the assembly has an invalid format.", "fullName");
            }
            else
            {
                Name = nameToApply;
            }
            
            for (int i = 1; i < parts.Length; i++)
            {
                string[] currentParts = parts[i].Split('=');
                string key = currentParts[0].Trim();
                string value = currentParts[1].Trim();

                switch (key)
                {
                    case "Culture":
                        cultureToApply = value;
                        Culture = String.Compare(cultureToApply, "neutral", true) == 0 ? "" : value;
                        break;

                    case "PublicKeyToken":
                        PublicKeyToken = value;
                        break;

                    case "Version":
                        versionToApply = value;
                        break;
                }
            }

            if (versionToApply == string.Empty)
            {
                throw new ArgumentException("The name of the assembly doesn't have version.", "fullName");
            }
            else
            {
                Version = new AssemblyVersion(versionToApply);
            }

            FullName = String.Format("{0}, Version={1}, Culture={2}, PublicKeyToken={3}", nameToApply, Version, cultureToApply, PublicKeyToken);
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
        /// Compounds the full name of the assembly
        /// without version number.
        /// </summary>
        /// <returns>Full name of the assembly
        /// without version number.</returns>
        public string GetFullNameWithoutVersion()
        {
            string cultureToApply = Culture == "" ? "neutral" : Culture;
            return String.Format("{0}, Culture={1}, PublicKeyToken={2}",
                Name, cultureToApply, PublicKeyToken);
        }
    }
}

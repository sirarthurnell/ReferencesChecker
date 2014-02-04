using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AssemblyCheck
{
    /// <summary>
    /// Represents the number version of an assembly.
    /// </summary>
    public struct AssemblyVersion : IComparable<AssemblyVersion>
    {
        private List<int> _version;

        /// <summary>
        /// Creates a new instance of AssemblyVersion.
        /// </summary>
        /// <param name="numbers">Numbers that will
        /// compound the version number.</param>
        public AssemblyVersion(IEnumerable<int> numbers)
        {
            _version = new List<int>(numbers);
        }

        /// <summary>
        /// Creates a new instance of AssemblyVersion.
        /// </summary>
        /// <param name="version">Version number
        /// specified by numbers separated by dots.</param>
        public AssemblyVersion(string version)
        {
            _version = new List<int>();
            ParseAndFillNumbers(version);
        }

        /// <summary>
        /// Given a version number as string of numbers
        /// separated by dots, fills the current version
        /// number.
        /// </summary>
        /// <param name="version">Version number as string.</param>
        private void ParseAndFillNumbers(string version)
        {
            string[] numbers;

            if (String.IsNullOrEmpty(version))
            {
                throw new ArgumentException("Version number can't be a null or empty string.", "version");
            }

            try
            {
                numbers = version.Split('.');
                foreach (string number in numbers)
                {
                    _version.Add(Convert.ToInt32(number));
                }
            }
            catch (Exception ex)
            {
                throw new ArgumentException("Incorrect version format.", "version", ex);
            }
        }

        /// <summary>
        /// Obtains the components of the version number.
        /// </summary>
        public IEnumerable<int> Parts
        {
            get
            {
                List<int> copy = new List<int>(_version);
                return copy;
            }
        }

        /// <summary>
        /// Gets a string representation of
        /// the version number.
        /// </summary>
        /// <returns>Version number as string.</returns>
        public override string ToString()
        {
            return String.Join(".", _version);
        }

        public static bool operator >(AssemblyVersion version1, AssemblyVersion version2)
        {
            int comparation = version1.CompareTo(version2);
            return comparation > 0 ? true : false;
        }

        public static bool operator <=(AssemblyVersion version1, AssemblyVersion version2)
        {
            return !(version1 > version2);
        }

        public static bool operator >=(AssemblyVersion version1, AssemblyVersion version2)
        {
            int comparation = version1.CompareTo(version2);
            return comparation >= 0 ? true : false;
        }

        public static bool operator <(AssemblyVersion version1, AssemblyVersion version2)
        {
            return !(version1 >= version2);
        }

        public static bool operator ==(AssemblyVersion version1, AssemblyVersion version2)
        {
            return version1.Equals(version2);
        }

        public static bool operator !=(AssemblyVersion version1, AssemblyVersion version2)
        {
            return !(version1 == version2);
        }

        /// <summary>
        /// Compares this instance with another instance
        /// of AssemblyVersion.
        /// </summary>
        /// <param name="other">Another AssemblyVersion
        /// instance.</param>
        /// <returns>Zero if both instances are equal.
        /// Number greater than zero if the current
        /// instance is greater than the another instance.
        /// Number less than zero if the current instance
        /// is less than the another instace.</returns>
        public int CompareTo(AssemblyVersion other)
        {            
            int totalChunks = Math.Min(_version.Count, other._version.Count);
            for (int i = 0; i < totalChunks; i++)
            {
                if (_version[i] == other._version[i])
                {
                    continue;
                }
                else
                {
                    return _version[i] - other._version[i];
                }
            }

            if (_version.Count > other._version.Count)
            {
                return 1;
            }
            else if (_version.Count < other._version.Count)
            {
                return -1;
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// Checks if the current instace is equal
        /// to other instance.
        /// </summary>
        /// <param name="obj">Object to compare against.</param>
        /// <returns>True if they both are equal. False
        /// if not.</returns>
        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            if (GetHashCode() != obj.GetHashCode())
            {
                return false;
            }

            if(!(obj is AssemblyVersion))
            {
                return false;
            }

            AssemblyVersion other = (AssemblyVersion)obj;
            if(_version.Count != other._version.Count)
            {
                return false;
            }

            for (int i = 0; i < _version.Count; i++)
            {
                if (_version[i] != other._version[i])
                {
                    return false;
                }
            }

            return true;
        }

        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using AssemblyCheck;
using System.IO;

namespace ReferencesChecker
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {      
        private string layoutAlgorithmType;
        private AnalysisResult graph;

        /// <summary>
        /// Creates the references graph between assemblies
        /// contained in a directory.
        /// </summary>
        public void CreateGraphFromAssembliesDirectory(string pathToDirectory)
        {
            DirectoryInfo directory = new DirectoryInfo(pathToDirectory);
            var files = directory.EnumerateFiles("*.dll", SearchOption.AllDirectories);
            var assemblies = new List<AssemblyInfo>();

            foreach (FileInfo file in files)
            {
                try
                {
                    var assembly = AssemblyReader.ReadAssembly(file.FullName);
                    assemblies.Add(assembly);
                }
                catch { }
            }

            var analyzer = new ReferencesAnalyzer();
            analyzer.Initialize(assemblies);
            var analyzerResults = analyzer.Analyze();
            Graph = analyzerResults;
        }

        public string LayoutAlgorithmType
        {
            get { return layoutAlgorithmType; }
            set
            {
                layoutAlgorithmType = value;
                NotifyPropertyChanged("LayoutAlgorithmType");
            }
        }

        public AnalysisResult Graph
        {
            get { return graph; }
            set
            {
                graph = value;
                NotifyPropertyChanged("Graph");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }
    }
}

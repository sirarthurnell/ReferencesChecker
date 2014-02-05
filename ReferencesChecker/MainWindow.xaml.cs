using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using QuickGraph;
using AssemblyCheck;
using System.IO;

namespace ReferencesChecker
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private AnalysisResult _graph;
        public AnalysisResult Graph
        {
            get
            {
                return _graph;
            }
        }

        public MainWindow()
        {
            CreateGraphFromAssembliesDirectory(@"C:\Archivos de programa\Archivos comunes\Mityc\Sigetel\Assemblies\Frmwk2");
            InitializeComponent();
        }

        /// <summary>
        /// Creates the references graph between assemblies
        /// contained in a directory.
        /// </summary>
        private void CreateGraphFromAssembliesDirectory(string pathToDirectory)
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
            _graph = analyzerResults;
        }
    }
}

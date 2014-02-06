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
        private MainWindowViewModel _viewModel;

        public MainWindow()
        {
            _viewModel = new MainWindowViewModel();
            this.DataContext = _viewModel;
            InitializeComponent();
        }

        private void LoadAssembliesDirectory(string pathToDirectory)
        {
            _viewModel.CreateGraphFromAssembliesDirectory(pathToDirectory);
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new System.Windows.Forms.FolderBrowserDialog();
            dlg.Description = "Select the folder containing the assemblies you want to analyze.";
            if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                LoadAssembliesDirectory(dlg.SelectedPath);
            }
        }

        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}

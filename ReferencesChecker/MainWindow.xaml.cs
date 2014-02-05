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

namespace ReferencesChecker
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private IBidirectionalGraph<object, IEdge<object>> _graph;

        public IBidirectionalGraph<object, IEdge<object>> Graph
        {
            get
            {
                return _graph;
            }
        }

        public MainWindow()
        {            
            CreateMinimalGraph();
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            
        }

        private void CreateMinimalGraph()
        {
            int value1 = 1;
            int value2 = 2;

            var graph = new BidirectionalGraph<object, IEdge<object>>();
            graph.AddVertex(value1);
            graph.AddVertex(value2);

            var edge1 = new Edge<object>(value1, value2);
            graph.AddEdge(edge1);

            _graph = graph;
        }
    }
}

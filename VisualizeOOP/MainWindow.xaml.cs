using Microsoft.Msagl.Drawing;
using Microsoft.Msagl.GraphViewerGdi;
using System.Windows;
using System.Windows.Controls;

namespace VisualizeOOP
{
    public partial class MainWindow : Window
    {
        private readonly MainViewModel _viewModel = new();
        private readonly GViewer _gViewer = new();
        private Node? _highlightedNode;

        public MainWindow()
        {
            //Initalize
            InitializeComponent();
            DataContext = _viewModel;
            wfHost.Child = _gViewer;
        }

        private async void BtnAnalyze_Click(object sender, RoutedEventArgs e)
        {
            //Read the provided folder path
            string folderToAnalyze = txtFolderPath.Text;

            //Checks if the path is not empty
            if (string.IsNullOrWhiteSpace(folderToAnalyze))
                //If it it empty
                return;

            //Explores the folder and create the diagram
            await _viewModel.ProcessAndLoadGraphAsync(folderToAnalyze);
            _highlightedNode = null;
            _gViewer.Graph = _viewModel.CurrentGraph;
        }

        private void LstClasses_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lstClasses.SelectedItem is not ClassModel selectedClass || _gViewer.Graph == null)
                return;

            Node? node = _gViewer.Graph.FindNode(selectedClass.Name);
            if (node == null)
                return;

            //Un-highlight the previous node
            if (_highlightedNode != null)
            {
                _highlightedNode.Attr.Color = new Color(86, 156, 214);
                _highlightedNode.Attr.LineWidth = 1;
            }

            //Highlight the new one
            node.Attr.Color = new Color(255, 200, 0);
            node.Attr.LineWidth = 3;
            _highlightedNode = node;

            //Center the viewport on the node
            _gViewer.CenterToPoint(node.GeometryNode.Center);

            _gViewer.Invalidate();
        }
    }
}
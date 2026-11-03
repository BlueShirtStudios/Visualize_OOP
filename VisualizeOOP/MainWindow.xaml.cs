using Microsoft.Msagl.Drawing;
using Microsoft.Msagl.GraphViewerGdi;
using System.Windows;
using System.Windows.Controls;
using Color = Microsoft.Msagl.Drawing.Color;

namespace VisualizeOOP
{
    public partial class MainWindow : Window
    {
        private static readonly Color NormalBorder = new Color(86, 156, 214);
        private static readonly Color HighlightBorder = new Color(255, 200, 0);

        private readonly MainViewModel _viewModel = new();
        private readonly GViewer _gViewer = new();
        private Node? _highlightedNode;

        public MainWindow()
        {
            InitializeComponent();
            DataContext = _viewModel;
            wfHost.Child = _gViewer;

            // Fires after loading a folder AND after switching display mode
            _viewModel.GraphChanged += OnGraphChanged;
        }

        private void OnGraphChanged()
        {
            // Old node objects no longer exist in the new graph
            _highlightedNode = null;
            _gViewer.Graph = _viewModel.CurrentGraph;

            // Keep the selected class highlighted after a rebuild
            if (lstClasses.SelectedItem is ClassModel selected)
                HighlightClass(selected, centerView: false);
        }

        private void Mode_Checked(object sender, RoutedEventArgs e)
        {
            // Checked fires during InitializeComponent, before rbDetailed exists
            if (rbDetailed == null) return;

            _viewModel.DisplayMode = rbDetailed.IsChecked == true
                ? DiagramModes.Detailed
                : DiagramModes.Summary;
        }

        private async void BtnAnalyze_Click(object sender, RoutedEventArgs e)
        {
            string folderToAnalyze = txtFolderPath.Text;

            //Check for whitespace
            if (string.IsNullOrWhiteSpace(folderToAnalyze))
                return;

            //Check for "" to make life easier
            folderToAnalyze = folderToAnalyze.Trim('"');

            //Build the graph with the mode selected
            await _viewModel.ProcessAndLoadGraphAsync(folderToAnalyze);
        }

        private void LstClasses_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lstClasses.SelectedItem is ClassModel selectedClass)
                HighlightClass(selectedClass, centerView: true);
        }

        private void HighlightClass(ClassModel cls, bool centerView)
        {
            if (_gViewer.Graph == null) return;

            Node? node = _gViewer.Graph.FindNode(cls.Name);
            if (node == null) return;

            if (_highlightedNode != null)
            {
                _highlightedNode.Attr.Color = NormalBorder;
                _highlightedNode.Attr.LineWidth = 1;
            }

            node.Attr.Color = HighlightBorder;
            node.Attr.LineWidth = 3;
            _highlightedNode = node;

            if (centerView)
                _gViewer.CenterToPoint(node.GeometryNode.Center);

            _gViewer.Invalidate();
        }
    }
}
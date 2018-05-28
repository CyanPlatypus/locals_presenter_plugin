using System.Collections.Generic;
using System.Linq;
using Microsoft.Msagl.Drawing;
using Microsoft.Msagl.WpfGraphControl;

namespace WindowVSIX
{
    using System.Diagnostics.CodeAnalysis;
    using System.Windows;
    using System.Windows.Controls;

    /// <summary>
    /// Interaction logic for windowDemoControl.
    /// </summary>
    public partial class windowDemoControl : UserControl
    {
        DockPanel graphViewerPanel = new DockPanel();
        //System.Windows.Controls.ToolBar toolBar = new System.Windows.Controls.ToolBar();
        GraphViewer graphViewer = new GraphViewer();

        /// <summary>
        /// Initializes a new instance of the <see cref="windowDemoControl"/> class.
        /// </summary>
        public windowDemoControl()
        {
            this.InitializeComponent();

            graphViewerPanel.ClipToBounds = true;
            //mainGrid.Height = 300;
            //mainGrid.Width = 300;
            mainGrid.Children.Add(graphViewerPanel);
            graphViewer.BindToPanel(graphViewerPanel);

            //graphViewer.MouseDown += WpfApplicationSample_MouseDown;
            //this.myGrid.Children.Add(mainGrid);

        }

        /// <summary>
        /// Handles click on the button by displaying a message box.
        /// </summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event args.</param>
        [SuppressMessage("Microsoft.Globalization", "CA1300:SpecifyMessageBoxOptions", Justification = "Sample code")]
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1300:ElementMustBeginWithUpperCaseLetter", Justification = "Default event handler naming pattern")]
        private void button1_Click(object sender, RoutedEventArgs e)
        {
            
            //MessageBox.Show(string.Format(System.Globalization.CultureInfo.CurrentUICulture, "Invoked '{0}'", this.ToString()),
            //    "windowDemo");
        }

        public void AddLog(string str)
        {
            tbMain.AppendText(str);
        }

        public void CleanLog()
        {
            tbMain.Document.Blocks.Clear();
        }

        //public void DrawNodes(List<Node> nodes)
        //{
        //    Graph graph = new Graph();

        //    nodes.ForEach(n=>DrawNode(n, graph));

        //    graph.Attr.LayerDirection = LayerDirection.LR;
        //    graph.Attr.SimpleStretch = false;
        //    graph.Attr.AspectRatio = 1;

        //    graphViewer.Graph = graph;
        //}

        public void DrawGraph(Graph graph)
        {
            graphViewer.Graph = graph;
        }

        //protected void DrawNode(Node n, Graph graph)
        //{
        //    if (n == null) return;
        //    //do not draw what already was drawn
        //    if (graph.Nodes.Any(nn => nn.Id == n.Id))
        //        return;

        //    Microsoft.Msagl.Drawing.Node
        //        newNode = new Microsoft.Msagl.Drawing.Node(n.Id) {LabelText = n.Caption ?? "?"};
        //    //newNode.Attr.FillColor = Color.LightGray;
        //    //font font size
        //    //newNode.Label.FontSize = 11;
        //    graph.AddNode(newNode);
        //    //if (parent != null)
        //    //    graph.AddEdge(parent.Id, n.Id);

        //    n.ChildNodes.ForEach(chN=>DrawNode(chN, graph));
        //    n.ChildNodes.ForEach(chN => graph.AddEdge(n.Id, chN.Id));
        //}
    }
}
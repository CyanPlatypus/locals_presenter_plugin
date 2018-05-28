using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Msagl.Drawing;

namespace WindowVSIX.Logic
{
    class DrawManager
    {
        public static Graph CreadeDrawableNoeds(List<Node> nodes)
        {
            Graph graph = new Graph();

            nodes.ForEach(n => DrawNode(n, graph));

            graph.Attr.LayerDirection = LayerDirection.LR;
            graph.Attr.SimpleStretch = false;
            graph.Attr.AspectRatio = 1;

            return graph;
        }

        protected static void DrawNode(Node n, Graph graph)
        {
            if (n == null) return;
            //do not draw what already was drawn
            if (graph.Nodes.Any(nn => nn.Id == n.Id))
                return;

            Microsoft.Msagl.Drawing.Node
                newNode = new Microsoft.Msagl.Drawing.Node(n.Id) { LabelText = n.Caption ?? "?" };
            //newNode.Attr.FillColor = Color.LightGray;
            //font font size
            //newNode.Label.FontSize = 11;
            graph.AddNode(newNode);
            //if (parent != null)
            //    graph.AddEdge(parent.Id, n.Id);

            n.ChildNodes.ForEach(chN => DrawNode(chN, graph));
            n.ChildNodes.ForEach(chN => graph.AddEdge(n.Id, chN.Id));
        }
    }
}

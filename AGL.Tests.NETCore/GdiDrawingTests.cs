using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using System.DrawingCore;
using System.DrawingCore.Imaging;
using Xunit;


using Microsoft.Msagl.Core.Geometry.Curves;
using Microsoft.Msagl.Core.Layout;
using Microsoft.Msagl.Core.Routing;
using Microsoft.Msagl.Layout.Layered;
using Microsoft.Msagl.Drawing;
using Node = Microsoft.Msagl.Drawing.Node;
using Edge = Microsoft.Msagl.Drawing.Edge;
using GeometryNode = Microsoft.Msagl.Core.Layout.Node;
using GeometryEdge = Microsoft.Msagl.Core.Layout.Edge;
using GeometryPoint = Microsoft.Msagl.Core.Geometry.Point;

using AGL.Drawing.Gdi;

namespace AGL.Tests
{
    public class GdiDrawingTests
    {
        [Fact]
        public void CanDrawGeometryGraph()
        {
            Graph graph = new Graph();
            GeometryGraph ge = new GeometryGraph();
            graph.GeometryGraph = ge;
            SugiyamaLayoutSettings sugiyamaSettings = new SugiyamaLayoutSettings
            {
                Transformation = PlaneTransformation.Rotation(Math.PI / 2),
                EdgeRoutingSettings = { EdgeRoutingMode = EdgeRoutingMode.Spline },
                MinNodeHeight = 10,
                MinNodeWidth = 20,
              
            };
            sugiyamaSettings.NodeSeparation *= 2;
            graph.LayoutAlgorithmSettings = sugiyamaSettings;
            graph.AddNode("A");
            graph.AddNode("B");
            graph.AddNode("C");
            graph.AddNode("D");
            graph.AddEdge("A", "B");
            
            foreach (Node n in graph.Nodes)
            {
                graph.GeometryGraph.Nodes.Add(new GeometryNode(CurveFactory.CreateRectangle(20, 10, new GeometryPoint()), n));
            }
            
            foreach (Edge e in graph.Edges)
            {
                GeometryNode source = graph.FindGeometryNode(e.SourceNode.Id);
                GeometryNode target = graph.FindGeometryNode(e.TargetNode.Id);
                graph.GeometryGraph.Edges.Add(new GeometryEdge(source, target));
            }
            ge.UpdateBoundingBox();
            graph.GeometryGraph = ge;
            using (Bitmap bmp = new Bitmap(400, 400, PixelFormat.Format32bppPArgb))
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.Clear(System.DrawingCore.Color.White);
                Rectangle rect = new Rectangle(0, 0, 400, 400);
                //GdiUtils.SetGraphTransform(graph, rect, g);
                LayeredLayout layout = new LayeredLayout(graph.GeometryGraph, sugiyamaSettings);
                layout.Run();
                GdiUtils.DrawFromGraph(rect, graph.GeometryGraph, g);
                bmp.Save("graph.bmp", ImageFormat.Bmp);
            }
                
        }
    }
}

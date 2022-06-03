using System.Linq;
using System.Collections.Generic;
using System.Text;

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

namespace AGL.Drawing
{
    public class Drawing
    {
        public static SugiyamaLayoutSettings GetSugiyamaLayout(int minNodeWidth = 2000, int minNodeHeight = 1000, double rotateBy = 0.0)
        {
            SugiyamaLayoutSettings sugiyamaSettings = new SugiyamaLayoutSettings
            {
                Transformation = PlaneTransformation.Rotation(rotateBy),
                EdgeRoutingSettings = { EdgeRoutingMode = EdgeRoutingMode.SugiyamaSplines },
                MinNodeHeight = minNodeHeight,
                MinNodeWidth = minNodeWidth

            };
            return sugiyamaSettings;
        }

        public static string DrawSvg(Graph graph, int width = 2000, int height = 2000, double rotateBy = 0.0)
        {
            graph.GeometryGraph = new GeometryGraph();
            var layout = GetSugiyamaLayout(5, 10, rotateBy);
            graph.LayoutAlgorithmSettings = layout;
            int nodeWidth = graph.Nodes.Max(n => n.LabelText.Length) * 9;
            foreach (Node n in graph.Nodes)
            {
                var gn = new GeometryNode(CurveFactory.CreateRectangle(nodeWidth, 40, new GeometryPoint()), n);
                graph.GeometryGraph.Nodes.Add(gn);
                n.GeometryNode = gn;
            }
            foreach (Edge edge in graph.Edges)
            {
                var sn = graph.FindGeometryNode(edge.Source);
                var tn = graph.FindGeometryNode(edge.Target);
                var ge = new GeometryEdge(sn, tn);
                ge.UserData = edge;
                ge.EdgeGeometry.TargetArrowhead = new Arrowhead();
                graph.GeometryGraph.Edges.Add(ge);
                edge.GeometryEdge = ge;
            }
            graph.GeometryGraph.UpdateBoundingBox();
            new LayeredLayout(graph.GeometryGraph, layout).Run();

            var sb = new StringBuilder();
            var svgWriter = new SvgGraphWriter(sb, graph);
            svgWriter.Write();
            return sb.ToString();
        }

    }
}

namespace Microsoft.Msagl.Drawing
{
	using System;
	using System.IO;
	using System.Linq;
	using System.Text;
	using System.Xml;
	public static class DGMLWriter
	{
		public static string Write(Graph g)
		{
			using (var stringWriter = new StringWriter())
			using (var xmlWriter = new XmlTextWriter(stringWriter))
			{
				xmlWriter.Formatting = Formatting.Indented;
				xmlWriter.WriteStartElement("DirectedGraph");
				xmlWriter.WriteAttributeString("xmlns", "http://schemas.microsoft.com/vs/2009/dgml");
				xmlWriter.WriteStartElement("Nodes");

				foreach (var node in g.Nodes)
				{
					var nodeId = Convert.ToString(node.Id);
					var label = node.LabelText;

					xmlWriter.WriteStartElement("Node");
					xmlWriter.WriteAttributeString("Id", nodeId);
					xmlWriter.WriteAttributeString("Label", label);

					if (node.OutEdges.Count() == 0 || node.OutEdges.Count() == 0)
					{
						xmlWriter.WriteAttributeString("Background", "Yellow");
					}
					xmlWriter.WriteEndElement();
				}

				xmlWriter.WriteEndElement();
				xmlWriter.WriteStartElement("Links");

				foreach (var edge in g.Edges)
				{
					xmlWriter.WriteStartElement("Link");
					xmlWriter.WriteAttributeString("Source", edge.Source);
					xmlWriter.WriteAttributeString("Target", edge.Target);
					xmlWriter.WriteEndElement();
				}
				
				xmlWriter.WriteEndElement();
				xmlWriter.WriteStartElement("Styles");
				xmlWriter.WriteStartElement("Style");
				xmlWriter.WriteAttributeString("TargetType", "Node");

				xmlWriter.WriteStartElement("Setter");
				xmlWriter.WriteAttributeString("Property", "FontFamily");
				xmlWriter.WriteAttributeString("Value", "Consolas");
				xmlWriter.WriteEndElement();

				xmlWriter.WriteStartElement("Setter");
				xmlWriter.WriteAttributeString("Property", "NodeRadius");
				xmlWriter.WriteAttributeString("Value", "5");
				xmlWriter.WriteEndElement();

				xmlWriter.WriteStartElement("Setter");
				xmlWriter.WriteAttributeString("Property", "MinWidth");
				xmlWriter.WriteAttributeString("Value", "0");
				xmlWriter.WriteEndElement();

				xmlWriter.WriteEndElement();
				xmlWriter.WriteEndElement();
				xmlWriter.WriteEndElement();
				xmlWriter.Flush();
				return stringWriter.ToString();
			}
		}

	}
}

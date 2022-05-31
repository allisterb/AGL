namespace Microsoft.Msagl.Drawing
{
	using System.Drawing;
	using System.IO;
	using System.Linq;
	using System.Text;
	using System.Xml;
	public static class DGMLWriter
	{
		public static void Write(Stream stream, Graph g)
		{
			using (var writer = new StreamWriter(stream))
            {
				Write(writer, g);
            }
			
		}

		public static void Write(TextWriter writer, Graph g)
        {
			using (var xmlWriter = new XmlTextWriter(writer))
			{
				xmlWriter.Formatting = Formatting.Indented;
				xmlWriter.WriteStartElement("DirectedGraph");
				xmlWriter.WriteAttributeString("xmlns", "http://schemas.microsoft.com/vs/2009/dgml");
				xmlWriter.WriteStartElement("Nodes");

				foreach (var node in g.Nodes)
				{
					var nodeId = node.Id;
					var label = node.LabelText;
					xmlWriter.WriteStartElement("Node");
					xmlWriter.WriteAttributeString("Id", nodeId);
					xmlWriter.WriteAttributeString("Label", label);
					xmlWriter.WriteAttributeString("Background", node.Attr.FillColor.ToString().Trim('"'));

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
			}
		}
	}
}

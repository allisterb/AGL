namespace Microsoft.Msagl.Drawing
{
	using System.Text;
	public static class DOTWriter
	{
		public static string Write(Graph g)
		{
			var sb = new StringBuilder();
			sb.AppendLine("digraph \n{");
			sb.AppendLine("\tnode[shape=\"rect\"];");

			foreach (var node in g.Nodes)
			{
				sb.AppendFormat("\t{0}[label=\"{1}\"];\n", node.Id, node.LabelText);

				foreach (var edge in g.Edges)
				{
					sb.AppendFormat("\t{0} -> {1};\n", edge.Source, edge.Target);
				}
			}
			sb.AppendLine("}");
			return sb.ToString();
		}
	}
}

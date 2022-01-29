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
				sb.AppendFormat("\t{0}[label=\"{1}\"];\n", node.Id.Replace(".", "_"), node.LabelText);

				foreach (var edge in g.Edges)
				{
					sb.AppendFormat("\t{0} -> {1};\n", edge.Source.Replace(".", "_"), edge.Target.Replace(".", "_"));
				}
			}
			sb.AppendLine("}");
			return sb.ToString();
		}
	}
}

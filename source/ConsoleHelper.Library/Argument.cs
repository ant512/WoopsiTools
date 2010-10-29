using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PrettyConsole.Library
{
	public class Argument
	{
		public bool IsOptional
		{
			get;
			private set;
		}

		public string Name
		{
			get;
			private set;
		}

		public string Type
		{
			get;
			private set;
		}

		public string Purpose
		{
			get;
			private set;
		}

		private List<ArgumentOption> Options
		{
			get;
			set;
		}

		public Argument(string name, string type, string purpose, bool isOptional)
		{
			Name = name;
			Type = type;
			Purpose = purpose;
			IsOptional = isOptional;
			Options = new List<ArgumentOption>();
		}

		public void AddOption(string name, string description)
		{
			Options.Add(new ArgumentOption(name, description));
		}

		public override string ToString()
		{
			StringBuilder output = new StringBuilder();

			if (IsOptional) output.Append("[");
			output.Append("/");
			output.Append(Name.ToUpper());
			output.Append(" ");
			output.Append(Type.ToLower());
			if (IsOptional) output.Append("]");

			return output.ToString();
		}

		public string FormatNameAndDescription(int leftIndentWidth, int maxWidth)
		{
			StringBuilder output = new StringBuilder();

			foreach (string line in ConsolePrinter.IndentedWrap(String.Format("/{0} {1}\n", Name.PadRight(leftIndentWidth, ' '), Purpose), leftIndentWidth, maxWidth, false)) {
				output.Append(String.Format("{0}\n", line));
			}

			if (Options.Count > 0)
			{
				output.Append("\n");
			}

			foreach (ArgumentOption option in Options)
			{
				foreach (string line in ConsolePrinter.IndentedWrap(option.ToString(), leftIndentWidth + 2, maxWidth, true)) {
					output.Append(String.Format("{0}\n", line));
				}
			}

			if (Options.Count > 0)
			{
				output.Append("\n");
			}

			return output.ToString();
		}
	}
}

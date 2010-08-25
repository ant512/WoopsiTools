using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace PrettyConsole.Library
{
	public class ConsolePrinter
	{
		private const int MAX_ROW_LEN = 79;

		public string Name { get; set; }
		public string Version { get; set; }
		public string Description { get; set; }

		public List<Argument> ArgumentList
		{
			get;
			private set;
		}

		public List<string> ParagraphList
		{
			get;
			private set;
		}

		public ConsolePrinter()
		{
			ArgumentList = new List<Argument>();
			ParagraphList = new List<string>();
		}

		public void AddArgument(Argument argument)
		{
			ArgumentList.Add(argument);
		}

		public void AddParagraph(string text)
		{
			ParagraphList.Add(text.Replace('\n', ' '));
		}

		public string Title
		{
			get
			{
				return String.Format("{0} {1}\n", Name, Version);
			}
		}

		public string HelpText
		{
			get
			{
				StringBuilder output = new StringBuilder();

				// Description
				foreach (string line in Wrap(Description, MAX_ROW_LEN))
				{
					output.Append(String.Format("{0}\n", line));
				}

				output.Append("\n");

				// Arguments
				foreach (string line in IndentedWrap(GetSignature(), Name.Length - 1, MAX_ROW_LEN, false))
				{
					output.Append(String.Format("{0}\n", line.Replace('\\', ' ')));
				}

				output.Append("\n");
				output.Append(GetFormattedArgumentDescriptions());

				if (ParagraphList.Count > 0)
				{
					output.Append("\n");

					foreach (string paragraph in ParagraphList)
					{
						foreach (string line in Wrap(paragraph, MAX_ROW_LEN))
						{
							output.Append(String.Format("{0}\n", line));
						}

						output.Append("\n");
					}
				}

				return output.ToString();
			}
		}

		private string GetSignature()
		{
			StringBuilder output = new StringBuilder();
			output.Append(String.Format("{0} ", Name));

			foreach (Argument argument in ArgumentList)
			{
				output.Append(String.Format("{0} ", argument.ToString().Replace(' ', '\\')));
			}

			return output.ToString();
		}

		private string GetFormattedArgumentDescriptions()
		{
			StringBuilder output = new StringBuilder();

			// Get the longest argument name or the name of the app; use this
			// to indent the descriptions
			int longestName = Name.Length;

			foreach (Argument argument in ArgumentList)
			{
				if (argument.Name.Length > longestName) longestName = argument.Name.Length;
			}

			// Add formatted argument to the output
			foreach (Argument argument in ArgumentList)
			{
				output.Append(argument.FormatNameAndDescription(longestName, MAX_ROW_LEN));
			}

			return output.ToString();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="text"></param>
		/// <param name="margin"></param>
		/// <returns></returns>
		/// <remarks>Borrowed from http://blueonionsoftware.com/blog.aspx?p=6091173d-6bdb-498c-9d57-c0da43319839 and altered to return an
		/// iterator instead of a list.</remarks>
		public static IEnumerable<string> Wrap(string text, int margin)
		{
			int start = 0, end;
			var lines = new List<string>();
			text = Regex.Replace(text, @"\s", " ").Trim();

			while ((end = start + margin) < text.Length)
			{
				while (text[end] != ' ' && end > start)
					end -= 1;

				if (end == start)
					end = start + margin;

				yield return text.Substring(start, end - start);
				start = end + 1;
			}

			if (start < text.Length)
				yield return text.Substring(start);
		}

		public static IEnumerable<string> IndentedWrap(string text, int leftIndentWidth, int margin, bool indentFirstLine)
		{
			StringBuilder output = new StringBuilder();
			bool firstLine = !indentFirstLine;
			StringBuilder wrapped = new StringBuilder();

			foreach (string line in ConsolePrinter.Wrap(text, margin))
			{
				if (firstLine)
				{
					firstLine = false;
					yield return String.Format("{0}", line);
				}
				else
				{
					wrapped.Append(String.Format("{0} ", line));
				}
			}

			foreach (string line in ConsolePrinter.Wrap(wrapped.ToString(), margin - leftIndentWidth - 2))
			{
				yield return String.Format("{0}{1}", String.Empty.PadLeft(leftIndentWidth + 2, ' '), line);
			}
		}
	}
}

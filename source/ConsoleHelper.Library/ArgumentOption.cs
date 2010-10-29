using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PrettyConsole.Library
{
	public sealed class ArgumentOption
	{
		public string Name
		{
			get;
			private set;
		}

		public string Description
		{
			get;
			private set;
		}

		public ArgumentOption(string name, string description)
		{
			Name = name;
			Description = description;
		}

		public override string ToString()
		{
			return String.Format("{0} - {1}", Name, Description);
		}
	}
}

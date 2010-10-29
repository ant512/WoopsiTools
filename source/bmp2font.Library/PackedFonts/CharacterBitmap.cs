using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bmp2Font.Library.PackedFonts
{
	class CharacterBitmap
	{
		public int Width
		{
			get;
			set;
		}

		public int CharTop
		{
			get;
			set;
		}

		public List<int> Data
		{
			get;
			private set;
		}

		public int Offset
		{
			get;
			set;
		}

		public CharacterBitmap()
		{
			Data = new List<int>();
		}
	}
}

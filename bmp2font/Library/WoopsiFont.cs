using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Woopsi.Bmp2Font
{
	/// <summary>
	/// Container for various information that describes a Woopsi font.  The output of the conversion
	/// process is stored in the font's HeaderContent and BodyContent members.
	/// </summary>
	public class WoopsiFont
	{
		/// <summary>
		/// The font's class name.  This is the name given to the C++ font class.
		/// </summary>
		private string mClassName;

		/// <summary>
		/// The font's class name.  This is the name given to the C++ font class.
		/// </summary>
		public string ClassName
		{
			get { return mClassName; }
			private set
			{
				// The .h and .cpp files are given the same name as the class
				mClassName = value;
				HeaderFileName = String.Format("{0}.h", mClassName.ToLower());
				BodyFileName = String.Format("{0}.cpp", mClassName.ToLower());
			}
		}

		/// <summary>
		/// The width of the font.  Should be the width of the widest glyph in the font.
		/// </summary>
		public int Width
		{
			get;
			private set;
		}

		/// <summary>
		/// The height of the font.  Should be the height of the tallest glyph in the font.
		/// </summary>
		public int Height
		{
			get;
			private set;
		}

		/// <summary>
		/// The complete .h file content.
		/// </summary>
		public string HeaderContent
		{
			get;
			set;
		}

		/// <summary>
		/// The complete .cpp file content.
		/// </summary>
		public string BodyContent
		{
			get;
			set;
		}

		/// <summary>
		/// The filename of the .h file.
		/// </summary>
		public string HeaderFileName
		{
			get;
			private set;
		}

		/// <summary>
		/// The filename of the .cpp file.
		/// </summary>
		public string BodyFileName
		{
			get;
			private set;
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="width">The width of the widest glyph in the font.</param>
		/// <param name="height">The height of the tallest glyph in the font.</param>
		/// <param name="className">The name of the font's C++ class.</param>
		public WoopsiFont(int width, int height, string className)
		{
			Width = width;
			Height = height;
			ClassName = className;
		}
	}
}

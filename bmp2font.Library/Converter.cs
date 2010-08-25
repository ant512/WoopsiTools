using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Drawing;

namespace Bmp2Font.Library
{
	/// <summary>
	/// Converts fonts based on input parameters.
	/// </summary>
	public class Converter
	{
		#region Constants

		/// <summary>
		/// Number of characters per row in the bitmap.
		/// </summary>
		const int CHARS_PER_ROW = 32;

		/// <summary>
		/// Number of rows in the bitmap.
		/// </summary>
		const int ROWS_PER_FONT = 8;

		#endregion

		#region Methods

		/// <summary>
		/// Converts the supplied bitmap to a Woopsi font using the specified parameters.
		/// </summary>
		/// <param name="className">The C++ classname for the font.</param>
		/// <param name="bitmapName">The name for the u16 array that stores the font bitmap data.</param>
		/// <param name="fontType">The type of font to produce.  Options are packedfont1 and packedfont16.</param>
		/// <param name="bitmap">The bitmap to convert.</param>
		/// <param name="fontWidth">The width of a glyph in the font.  If set to -1, the width is calculated
		/// from the width of the bitmap.  In that case, the bitmap must be a multiple of 32 wide.</param>
		/// <param name="fontHeight">The height of a glyph in the font.  If set to -1, the height is calculated
		/// from the height of the bitmap.  In that case, the bitmap must be a multiple of 8 tall.</param>
		/// <param name="backgroundR">The red component of the background colour.  If any component is set to
		/// -1, the colour is retrieved automatically from the top-left pixel of the bitmap.</param>
		/// <param name="backgroundG">The green component of the background colour.  If any component is set to
		/// -1, the colour is retrieved automatically from the top-left pixel of the bitmap.</param>
		/// <param name="backgroundB">The blue component of the background colour.  If any component is set to
		/// -1, the colour is retrieved automatically from the top-left pixel of the bitmap.</param>
		/// <returns>The converted font.</returns>
		public static WoopsiFont Convert(string className, string bitmapName, string fontType, Bitmap bitmap, int fontWidth, int fontHeight, int backgroundR, int backgroundG, int backgroundB)
		{
			fontWidth = GetFontWidth(fontWidth, bitmap);
			fontHeight = GetFontHeight(fontHeight, bitmap);

			WoopsiFont font = new WoopsiFont(fontWidth, fontHeight, className);
			Color backgroundColour = GetBackgroundColour(backgroundR, backgroundG, backgroundB, bitmap);

			FontConverterBase converter = null;
			switch (fontType.ToLower())
			{
				case "packedfont16":
					converter = new PackedFonts.PackedFontConverter(font, bitmap, backgroundColour, false);
					break;
				case "packedfont1":
					converter = new PackedFonts.PackedFontConverter(font, bitmap, backgroundColour,  true);
					break;
			}

			font.BodyContent = converter.OutputCPP;
			font.HeaderContent = converter.OutputH;

			return font;
		}

		/// <summary>
		/// Get the background colour of the bitmap.  If a colour is specified, that colour is returned.  If
		/// any component is set to -1, the top-left pixel of the bitmap is used as the background colour instead.
		/// </summary>
		/// <param name="r">The red component if the colour.</param>
		/// <param name="g">The green component if the colour.</param>
		/// <param name="b">The blue component if the colour.</param>
		/// <param name="bitmap">The bitmap being converted.</param>
		/// <returns>The background colour.</returns>
		private static Color GetBackgroundColour(int r, int g, int b, Bitmap bitmap)
		{
			if ((r == -1) || (g == -1) || (b == -1))
			{
				// Get background colour from bitmap
				return bitmap.GetPixel(0, 0);
			}
			else
			{
				// Get background colour from supplied parameters
				return Color.FromArgb(r, g, b);
			}
		}

		/// <summary>
		/// Gets the width of the font.  If the supplied value is -1, the width is calculated from the
		/// width of the bitmap.  In this case, the bitmap must be a multiple of 32 wide.  If a value >-1
		/// is specified, that value is returned.
		/// </summary>
		/// <param name="width">Desired width.</param>
		/// <param name="bitmap">Bitmap being converted.</param>
		/// <returns>The width of the font.</returns>
		static int GetFontWidth(int width, Bitmap bitmap)
		{
			if (width > -1) return width;

			if (bitmap.Width % CHARS_PER_ROW != 0)
			{
				throw new ArgumentException(String.Format("Bitmap width is not a multiple of {0}.", CHARS_PER_ROW));
			}

			return bitmap.Width / CHARS_PER_ROW;
		}

		/// <summary>
		/// Gets the height of the font.  If the supplied value is -1, the height is calculated from the
		/// height of the bitmap.  In this case, the bitmap must be a multiple of 8 tall.  If a value >-1
		/// is specified, that value is returned.
		/// </summary>
		/// <param name="height">Desired height.</param>
		/// <param name="bitmap">Bitmap being converted.</param>
		/// <returns>The height of the font.</returns>
		static int GetFontHeight(int height, Bitmap bitmap)
		{
			if (height > -1) return height;

			if (bitmap.Height % ROWS_PER_FONT != 0)
			{
				throw new ArgumentException(String.Format("Bitmap width is not a multiple of {0}.", ROWS_PER_FONT));
			}

			return bitmap.Height / ROWS_PER_FONT;
		}

		#endregion
	}
}

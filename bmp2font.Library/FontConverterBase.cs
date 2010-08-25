using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace Bmp2Font.Library
{
	/// <summary>
	/// Abstract class defining the basic behaviour of a font converter.
	/// </summary>
	abstract class FontConverterBase
	{
		#region Members

		/// <summary>
		/// The font being converted.
		/// </summary>
		protected WoopsiFont mFont;

		/// <summary>
		/// The bitmap to be converted.
		/// </summary>
		protected Bitmap mBitmap;

		/// <summary>
		/// The background colour of the bitmap.
		/// </summary>
		protected Color mBackgroundColour = Color.Black;

		/// <summary>
		/// The .cpp file content.
		/// </summary>
		protected StringBuilder mOutputCPP = new StringBuilder();

		/// <summary>
		/// The .h file content.
		/// </summary>
		protected StringBuilder mOutputH = new StringBuilder();

		#endregion

		#region Properties

		/// <summary>
		/// Get a string representation of the .cpp file content.
		/// </summary>
		public string OutputCPP {
			get { return mOutputCPP.ToString(); }
		}

		/// <summary>
		/// Get a string representation of the .h file content.
		/// </summary>
		public string OutputH
		{
			get { return mOutputH.ToString(); }
		}

		#endregion

		#region Constructors

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="font">The font object that will store the converted data.</param>
		/// <param name="bitmap">The bitmap to convert.</param>
		/// <param name="backgroundColour">The background colour of the bitmap.</param>
		public FontConverterBase(WoopsiFont font, Bitmap bitmap, Color backgroundColour)
		{
			mFont = font;
			mBitmap = bitmap;
			mBackgroundColour = backgroundColour;
		}

		#endregion

		#region Methods

		#region Abstract Methods

		/// <summary>
		/// Builds the .cpp output.  Does the bulk of the font conversion.
		/// </summary>
		protected abstract void BuildCPPOutput();

		/// <summary>
		/// Builds the .h output.
		/// </summary>
		protected abstract void BuildHOutput();

		#endregion

		/// <summary>
		/// Convert the supplied colour to an 16-bit integer.
		/// </summary>
		/// <param name="colour">The colour to convert.</param>
		/// <returns>The converted colour.</returns>
		protected int ColorToRGBA16(Color colour)
		{
			int r = (colour.R >> 3);
			int g = (colour.G >> 3);
			int b = (colour.B >> 3);
			return r | (g << 5) | (b << 10) | (1 << 15);
		}

		/// <summary>
		/// Convert the suplied 16-bit integer to a Color object.
		/// </summary>
		/// <param name="colour">The colour to convert.</param>
		/// <returns>The converted colour.</returns>
		protected Color RGB16ToColor(int colour)
		{
			int r = (colour & 31) << 3;
			int g = ((colour >> 5) & 31) << 3;
			int b = ((colour >> 10) & 31) << 3;

			return Color.FromArgb(r, g, b);
		}

		/// <summary>
		/// Convert an integer to a hex string.
		/// </summary>
		/// <param name="value">The integer to convert.</param>
		/// <returns>The hex string.</returns>
		protected string IntToHexString(int value)
		{
			return String.Format("0x{0:X4}", value);
		}

		#endregion
	}
}

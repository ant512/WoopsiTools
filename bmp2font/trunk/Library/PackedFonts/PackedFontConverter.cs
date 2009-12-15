using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace Woopsi.Bmp2Font.PackedFonts
{
	/// <summary>
	/// Converter that will convert from bitmaps to PackedFont1 and PackedFont16 classes.
	/// </summary>
	class PackedFontConverter : FontConverterBase
	{
		#region Members

		/// <summary>
		/// If true, fonts will be converted to PackedFont1 classes.  If false, fonts will be converted
		/// to PackedFont16 classes.
		/// </summary>
		private bool mIsMonochrome = false;

		/// <summary>
		/// The ASCII code of the first character that appears in the font.
		/// </summary>
		private int mFirstChar = -1;

		/// <summary>
		/// The ASCII code of the last character that appears in the font.
		/// </summary>
		private int mLastChar = 256;

		/// <summary>
		/// The width of a blank space in the font.
		/// </summary>
		private int mSpaceWidth = 0;

		/// <summary>
		/// The maximum width of a glyph in the font.
		/// </summary>
		private int mMaximumObservedWidth = 0;

		/// <summary>
		/// The superclass of the resultant font; will be either PackedFont1 or PackedFont16.
		/// </summary>
		private string mSuperClassName = "";

		/// <summary>
		/// List of converted glyph data.
		/// </summary>
		private List<CharacterBitmap> mCharacters = new List<CharacterBitmap>();

		#endregion

		#region Constructors

		/// <summary>
		/// Constructor.  Automatically converts the font when an object is instantiated.
		/// </summary>
		/// <param name="font">Font being converted.</param>
		/// <param name="bitmap">Bitmap to convert.</param>
		/// <param name="backgroundColour">Background colour of the bitmap.</param>
		/// <param name="isMonochrome">True to convert to a PackedFont1; false to convert to a PackedFont16.</param>
		public PackedFontConverter(WoopsiFont font, Bitmap bitmap, Color backgroundColour, bool isMonochrome) : base(font, bitmap, backgroundColour)
		{
			mIsMonochrome = isMonochrome;
			mSuperClassName = isMonochrome ? "PackedFont1" : "PackedFont16";

			BuildCPPOutput();
			BuildHOutput();
		}

		#endregion

		#region Methods

		/// <summary>
		/// Create the .cpp output file.
		/// </summary>
		protected override void BuildCPPOutput()
		{
			// Create an array of character pixel data
			BuildCharacterData();

			// Pack data if font is monochrome
			if (mIsMonochrome) PackCharacters();

			// Append data to output string
			AppendHeader();
			AppendGlyphs();
			AppendOffsets();
			AppendWidths();
			AppendConstructor();
		}

		/// <summary>
		/// Builds an array of converted glyphs from the source bitmap.
		/// </summary>
		protected void BuildCharacterData() {

			// Variables for loop
			int charX = 0;
			int charY = 0;
			bool gotData = false;
			CharacterBitmap character = null;
			Color col;

			// Width of space is 1/4th height of font, rounded up
			mSpaceWidth = (mFont.Height + 3) / 4;

			// Loop through all characters in the font
			for (int i = 0; i < 256; ++i)
			{
				// Create a new character object to store this character
				character = new CharacterBitmap();
				character.Width = 0;
				gotData = false;

				// Loop through all pixels in this character's bitmap
				for (int y = 0; y < mFont.Height; ++y)
				{
					for (int x = 0; x < mFont.Width; ++x)
					{
						// Store the pixel in the character object in 16-bit RGBA 1555 format
						col = mBitmap.GetPixel(x + charX, y + charY);

						// Did we find any data in this character?
						if (!col.Equals(mBackgroundColour))
						{
							// Add the colour to the character's bitmap
							character.Data.Add(ColorToRGBA16(col));

							gotData = true;

							// Update the width of this character
							if (x + 1 > character.Width)
							{
								character.Width = x + 1;

								// Is this the largest width yet seen?
								if (character.Width > mMaximumObservedWidth) mMaximumObservedWidth = character.Width;
							}
						}
						else
						{
							// No data; add 0 to the character's bitmap
							character.Data.Add(0);
						}
					}
				}

				// Add the character to the character array
				mCharacters.Add(character);

				// Move to the next character in the bitmap
				charX += mFont.Width;

				// Wrap to the next row of characters if we've moved past the end
				// of the bitmap
				if (charX >= mBitmap.Width)
				{
					charX = 0;
					charY += mFont.Height;
				}

				// If we found data and this is the first character to contain data, remember this character
				if ((mFirstChar < 0) && (gotData)) mFirstChar = i;

				// If we found data, this is the last character so far to contain it
				if (gotData) mLastChar = i;
			}
		}

		/// <summary>
		/// Get a printable representation of the supplied char.  Returns the character itself
		/// if it is in the standard ASCII range, or a hex code if it falls outside that range.
		/// </summary>
		/// <param name="ch">The character to print.</param>
		/// <returns>A printable representation of the character.</returns>
		private string PrintableChar(char ch)
		{
			if (ch < 32) return String.Format("0x{0:X2}", ch);
			if (ch < 127) return String.Format("'{0}'", ch);
			return String.Format("0x{0:X2}", (int)ch);
		}

		/// <summary>
		/// Pack a single character so that each pixel is represented by a single bit.
		/// </summary>
		/// <param name="character">Character to pack.</param>
		/// <returns>The packed character.</returns>
		private CharacterBitmap PackCharacter(CharacterBitmap character)
		{
			CharacterBitmap packedCharacter = new CharacterBitmap();
			int packedValue = 0;
			int bits = 16;

			packedCharacter.Offset = character.Offset;
			packedCharacter.Width = character.Width;

			// Loop through all pixels, condensing them into single bits
			for (int y = 0; y < mFont.Height; ++y)
			{
				for (int x = 0; x < character.Width; ++x)
				{
					// Set bit if pixel is not transparent
					if (character.Data[x + (y * mFont.Width)] != 0)
					{
						packedValue += (1 << (bits - 1));
					}

					bits--;

					// Append packed value to data if processed 16 bits
					if (bits == 0)
					{
						packedCharacter.Data.Add(packedValue);
						bits = 16;
						packedValue = 0;
					}
				}
			}

			// Add any trailing bits as a new value
			if ((bits != 16) || (character.Width == 0)) packedCharacter.Data.Add(packedValue);

			return packedCharacter;
		}

		/// <summary>
		/// Pack all characters in the supplied array so that each pixel is
		/// represented by a single bit.
		/// </summary>
		protected void PackCharacters()
		{
			// Loop through all characters, packing each one
			for (int i = mFirstChar; i <= mLastChar; ++i)
			{
				// Replace unpacked character with packed character
				mCharacters[i] = PackCharacter(mCharacters[i]);
			}
		}

		/// <summary>
		/// Creates the .h output string.
		/// </summary>
		protected override void BuildHOutput()
		{
			mOutputH.Append(String.Format("#ifndef _{0}_H_\n", mFont.ClassName.ToUpper()));
			mOutputH.Append(String.Format("#define _{0}_H_\n", mFont.ClassName.ToUpper()));
            mOutputH.Append("\n");
            mOutputH.Append(String.Format("#include \"{0}.h\"\n", mSuperClassName.ToLower()));
            mOutputH.Append("\n");
            mOutputH.Append("namespace WoopsiUI {\n");
			mOutputH.Append(String.Format("\tclass {0} : public {1}", mFont.ClassName, mSuperClassName));
            mOutputH.Append(" {\n");
            mOutputH.Append("\tpublic:\n");
			mOutputH.Append(String.Format("\t\t{0}(u8 fixedWidth = 0);\n", mFont.ClassName));
            mOutputH.Append("\t};\n");
            mOutputH.Append("}\n");
            mOutputH.Append("\n");
            mOutputH.Append("#endif\n");
		}

		/// <summary>
		/// Appends the header to the .cpp output string.
		/// </summary>
		private void AppendHeader()
		{
			mOutputCPP.Append(String.Format("#include \"{0}\"\n", mFont.HeaderFileName));
			mOutputCPP.Append("#include <nds.h>\n");
			mOutputCPP.Append("\n");
			mOutputCPP.Append("using namespace WoopsiUI;\n");
			mOutputCPP.Append("\n");
		}

		/// <summary>
		/// Gets the number of shorts in the glyph data.  Chooses whether
		/// to treat the data as 16-bit or monochrome automatically.
		/// </summary>
		/// <returns>The number of shorts in the glyph data.</returns>
		private int GetShortCount() {
			return (mIsMonochrome ? GetShortCountMono() : GetShortCount16());
		}

		/// <summary>
		/// Gets the number of shorts in the monochrome glyph data.
		/// </summary>
		/// <returns>The number of shorts in the glyph data.</returns>
		private int GetShortCountMono()
		{
			int shortCount = 0;

			foreach (CharacterBitmap item in mCharacters)
			{
				shortCount += (item.Data.Count);
			}

			return shortCount;
		}

		/// <summary>
		/// Gets the number of shorts in the 16-bit glyph data.
		/// </summary>
		/// <returns>The number of shorts in the glyph data.</returns>
		private int GetShortCount16()
		{
			int shortCount = 0;

			foreach (CharacterBitmap item in mCharacters)
			{
				shortCount += (item.Width * mFont.Height);
			}

			return shortCount;
		}

		/// <summary>
		/// Appends the glyph data to the .cpp output string.
		/// </summary>
		private void AppendGlyphs()
		{
			CharacterBitmap character = null;

			// Calculate the number of shorts in the data
			int shortCount = GetShortCount();

			mOutputCPP.Append(String.Format("static const u16 {0}_glyphdata[{1}] = ", mFont.ClassName, shortCount));
			mOutputCPP.Append("{\n");

			// Append glyph data
			int pos = 0;

			for (int i = mFirstChar; i <= mLastChar; ++i)
			{
				character = mCharacters[i];
				character.Offset = pos;


				if (mIsMonochrome)
				{
					// Append the character itself
					mOutputCPP.Append(String.Format("/* {0} */\t", PrintableChar((char)i)));

					// Append all pixels
					foreach (int value in character.Data)
					{
						// Append a hex-formatted version of the data
						mOutputCPP.Append(String.Format("{0},", IntToHexString(value)));

						pos++;
					}
				}
				else
				{
					// Append the character itself
					mOutputCPP.Append(String.Format("\t// {0}\n", PrintableChar((char)i)));

					// Append all pixels
					for (int y = 0; y < mFont.Height; ++y)
					{
						for (int x = 0; x < character.Width; ++x)
						{
							// Append a hex-formatted version of the data
							mOutputCPP.Append(String.Format("\t{0},", IntToHexString(character.Data[x + (y * mFont.Width)])));

							pos++;
						}

						// Line break at the end of the row
						mOutputCPP.Append("\n");
					}
				}

				// Line break between chars
				mOutputCPP.Append("\n");
			}

			// Append closing bracket
			mOutputCPP.Append("};\n\n");
		}

		/// <summary>
		/// Append glyph offsets to the .cpp output string.
		/// </summary>
		private void AppendOffsets()
		{
			int charCount = (mLastChar - mFirstChar) + 1;

			mOutputCPP.Append(String.Format("static const u16 {0}_offset[{1}] = ", mFont.ClassName, charCount));
			mOutputCPP.Append("{\n");

			int pos = 0;
			for (int i = mFirstChar; i <= mLastChar; ++i)
			{
				// Append offset
				mOutputCPP.Append(mCharacters[i].Offset.ToString().PadLeft(5));

				// Append comma if more offsets to be appended
				if (i < mLastChar) mOutputCPP.Append(",");

				pos++;

				if (pos % 16 == 0) mOutputCPP.Append("\n");
			}

			// Append closing bracket
			mOutputCPP.Append("\n};\n\n");
		}

		/// <summary>
		/// Append character widths to the .cpp output string.
		/// </summary>
		private void AppendWidths()
		{
			int charCount = (mLastChar - mFirstChar) + 1;

			mOutputCPP.Append(String.Format("static const u8 {0}_width[{1}] = ", mFont.ClassName, charCount));
			mOutputCPP.Append("{\n");

			int pos = 0;
			for (int i = mFirstChar; i <= mLastChar; ++i)
			{
				// Append offset
				mOutputCPP.Append(mCharacters[i].Width.ToString().PadLeft(2));

				// Append comma if more offsets to be appended
				if (i < mLastChar) mOutputCPP.Append(",");

				pos++;

				if (pos % 16 == 0) mOutputCPP.Append("\n");
			}

			// Append closing bracket
			mOutputCPP.Append("\n};\n\n");
		}

		/// <summary>
		/// Appends the font constructor to the .cpp output string.
		/// </summary>
		private void AppendConstructor()
		{
			mOutputCPP.Append(String.Format("{0}::{0}(u8 fixedWidth) : {1} (\n", mFont.ClassName, mSuperClassName));
			mOutputCPP.Append(String.Format("\t{0},\n", mFirstChar));
			mOutputCPP.Append(String.Format("\t{0},\n", mLastChar));
			mOutputCPP.Append(String.Format("\t{0}_glyphdata,\n", mFont.ClassName));
			mOutputCPP.Append(String.Format("\t{0}_offset,\n", mFont.ClassName));
			mOutputCPP.Append(String.Format("\t{0}_width,\n", mFont.ClassName));
			mOutputCPP.Append(String.Format("\t{0},\n", mFont.Height));
			mOutputCPP.Append(String.Format("\t{0},\n", mSpaceWidth));
			mOutputCPP.Append(String.Format("\t{0}", mMaximumObservedWidth));
			mOutputCPP.Append(") {\n");
			mOutputCPP.Append("\tif (fixedWidth) setFontWidth(fixedWidth);\n");
			mOutputCPP.Append("};\n");
		}

		#endregion
	}
}

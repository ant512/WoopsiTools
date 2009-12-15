using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace Woopsi.Bmp2Font.MonoFonts
{
	/// <summary>
	/// Converter that will convert to MonoFont classes.
	/// </summary>
	class MonoFontConverter : FontConverterBase
	{
		#region Constructors

		/// <summary>
		/// Constructor.  Automatically converts the font when an object is instantiated.
		/// </summary>
		/// <param name="font">Font being converted.</param>
		/// <param name="bitmap">Bitmap to convert.</param>
		/// <param name="backgroundColour">Background colour of the bitmap.</param>
		public MonoFontConverter(WoopsiFont font, Bitmap bitmap, Color backgroundColour)
			: base(font, bitmap, backgroundColour)
		{
			BuildCPPOutput();
			BuildHOutput();
		}

		#endregion

		#region Methods

		/// <summary>
		/// Create the .cpp output string.
		/// </summary>
		protected override void BuildCPPOutput()
		{
			Console.WriteLine("Packing data");

			// Open array
			mOutputCPP.Append("] __attribute__ ((aligned (4))) = {\n");

			// Start packing bits
			int bits = 0;
			int packed = 0;
			int values = 0;

			// Scan through all image pixels
			for (int y = 0; y < mBitmap.Height; y++)
			{
				for (int x = 0; x < mBitmap.Width; x++)
				{
					if (!mBitmap.GetPixel(x, y).Equals(mBackgroundColour))
					{
						// Colour not the background colour; pack bit
						packed |= (1 << (15 - bits));
					}

					bits++;

					// Do we need to output the value?
					if (bits == 16)
					{
						mOutputCPP.Append(packed.ToString());
						mOutputCPP.Append(", ");

						values++;

						if (values % 8 == 0)
						{
							mOutputCPP.Append("\n");
						}

						// Reset values
						packed = 0;
						bits = 0;
					}
				}
			}

			// Strip the trailing comma and space
			int lastCommaPos = mOutputCPP.ToString().LastIndexOf(',');
			mOutputCPP.Remove(lastCommaPos, mOutputCPP.Length - lastCommaPos);

			// Append closing bracket and semi-colon
			mOutputCPP.Append("\n};\n\n");

			// Append font constructor
			mOutputCPP.Append(String.Format("{0}::{0}(u16 drawColour) : MonoFont({1}_Bitmap, {2}, {3}, {4}, {5}, drawColour)",
				mFont.ClassName,
				mFont.ClassName.ToLower(),
				mBitmap.Width.ToString(),
				mBitmap.Height.ToString(),
				mFont.Width.ToString(),
				mFont.Height.ToString()));

			mOutputCPP.Append(" { };");

			// Insert opening data in reverse order
			mOutputCPP.Insert(0, values.ToString());
			mOutputCPP.Insert(0, "_Bitmap[");
			mOutputCPP.Insert(0, mFont.ClassName.ToLower());
			mOutputCPP.Insert(0, "static const u16 ");
			mOutputCPP.Insert(0, "\n");
			mOutputCPP.Insert(0, "using namespace WoopsiUI;\n");
			mOutputCPP.Insert(0, "\n");
			mOutputCPP.Insert(0, String.Format("#include \"{0}\"\n", mFont.HeaderFileName));
			mOutputCPP.Insert(0, "#include <nds.h>\n");

			Console.WriteLine("Pack OK");
		}

		/// <summary>
		/// Creates the .h output string.
		/// </summary>
		protected override void BuildHOutput()
		{
			mOutputH.Append(String.Format("#ifndef _{0}_H_\n", mFont.ClassName.ToUpper()));
			mOutputH.Append(String.Format("#define _{0}_H_\n", mFont.ClassName.ToUpper()));
			mOutputH.Append("\n");
			mOutputH.Append("#include \"monofont.h\"\n");
			mOutputH.Append("\n");
			mOutputH.Append("namespace WoopsiUI {\n");
			mOutputH.Append(String.Format("\tclass {0} : public MonoFont", mFont.ClassName));
			mOutputH.Append(" {\n");
			mOutputH.Append("\tpublic:\n");
			mOutputH.Append(String.Format("\t\t{0}(u16 drawColour = 32768);\n", mFont.ClassName));
			mOutputH.Append("\t};\n");
			mOutputH.Append("}\n");
			mOutputH.Append("\n");
			mOutputH.Append("#endif\n");
		}

		#endregion
	}
}

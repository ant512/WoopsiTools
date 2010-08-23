using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Drawing;
using Woopsi.Bmp2Font;

namespace Bmp2FontApp
{
	class Program
	{
		#region Constants

		const string APP_VERSION = "V1.1";
		const string APP_NAME = "bmp2font";
		const int CHARS_PER_ROW = 32;
		const int ROWS_PER_FONT = 8;

		#endregion

		#region Members

		static int mBackgroundR = -1;
		static int mBackgroundG = -1;
		static int mBackgroundB = -1;
		static string mFileNameIn = "";
		static string mClassName = "";
		static string mBitmapName = "";
		static Bitmap mBitmap = null;
		static string mFontType = "";
		static int mFontWidth = -1;
		static int mFontHeight = -1;

		#endregion

		#region Methods

		/// <summary>
		/// Main loop.
		/// </summary>
		/// <param name="args">Command line arguments</param>
		static void Main(string[] args)
		{
			// Output title
			WriteTitle();

			// Fetch arguments
			if (!ParseArgs(args)) return;

			// Load original file
			if (!LoadFile()) return;

			// Perform the conversion
			try
			{
				WoopsiFont font = Converter.Convert(mClassName, mBitmapName, mFontType, mBitmap, mFontWidth, mFontHeight, mBackgroundR, mBackgroundG, mBackgroundB);

				// Output files
				WriteFile(font.HeaderFileName, font.HeaderContent);
				WriteFile(font.BodyFileName, font.BodyContent);

				Console.WriteLine("All done!");
			}
			catch (Exception)
			{
				Error("Unable to parse and pack data");
			}
		}

		/// <summary>
		/// Write the program's title to the console.
		/// </summary>
		static void WriteTitle()
		{
			Console.WriteLine(String.Format("{0} {1}", APP_NAME, APP_VERSION));
			Console.WriteLine("");
		}

		/// <summary>
		/// Load source bitmap
		/// </summary>
		static bool LoadFile()
		{
			Console.WriteLine("Loading bitmap");

			try
			{
				mBitmap = Bitmap.FromFile(mFileNameIn) as Bitmap;
				return true;
			}
			catch
			{
				Error("Unable to load bitmap");
			}

			return false;
		}

		/// <summary>
		/// Output data to disk.
		/// </summary>
		static void WriteFile(string filename, string data)
		{
			Console.WriteLine(String.Format("Writing file: {0}", filename));

			StreamWriter file = null;

			try
			{
				file = new StreamWriter(filename);
			}
			catch
			{
				Error("Unable to write file");
				return;
			}

			try
			{
				file.Write(data);

				Console.WriteLine("Write OK");
			}
			catch
			{
				Error("Unable to write file");
			}
			finally
			{
				file.Close();
				file.Dispose();
			}
		}

		/// <summary>
		/// Show an error message.
		/// </summary>
		/// <param name="msg">Message to show.</param>
		static void Error(string msg)
		{
			Console.WriteLine(String.Format("Error: {0}", msg));
		}

		static void PrintHelp()
		{
			Console.WriteLine("Converts a BMP font image into a bit-packed monochrome font class");
			Console.WriteLine("for use with Woopsi.");
			Console.WriteLine("");
			Console.WriteLine("bmp2font /INFILE string /CLASSNAME string /FONTTYPE string");
			Console.WriteLine("         [/WIDTH int] [/HEIGHT int] [/R int] [/G int] [/B int]");
			Console.WriteLine("");
			Console.WriteLine("/INFILE        Path and filename of the BMP file to convert");
			Console.WriteLine("/CLASSNAME     Name of the resultant font class");
			Console.WriteLine("/FONTTYPE      Type of font to produce.  Options are:");
			Console.WriteLine("");
			Console.WriteLine("                 packedfont1  - Monochrome packed proportional font");
			Console.WriteLine("                 packedfont16 - 16-bit packed proportional font");
			Console.WriteLine("");
			Console.WriteLine("/WIDTH         Width of a single character in the font");
			Console.WriteLine("/HEIGHT        Height of a single character in the font");
			Console.WriteLine("/R             Red component of the background colour");
			Console.WriteLine("/G             Green component of the background colour");
			Console.WriteLine("/B             Blue component of the background colour");
			Console.WriteLine("");
			Console.WriteLine("WIDTH and HEIGHT represent the width and height of a single character");
			Console.WriteLine("in the font.  Fonts must be fixed width.  If WIDTH and HEIGHT are not");
			Console.WriteLine("specified, they will be calculated from the dimensions of the bitmap.");
			Console.WriteLine("In that case, the bitmap must be a multiple of 32 wide (32 chars per line)");
			Console.WriteLine("and a multiple of 8 high (32 * 8 = 256, the number of chars in a font)");
			Console.WriteLine("");
			Console.WriteLine("If the RGB background colour is not specified, it is automatically");
			Console.WriteLine("determined from the colour of the top-left pixel.  If this pixel is not");
			Console.WriteLine("part of the background, the resultant font class will be corrupt.");
		}

		/// <summary>
		/// Parse the command line arguments
		/// </summary>
		/// <param name="args">Command line arguments.</param>
		static bool ParseArgs(string[] args)
		{
			try
			{
				for (int i = 0; i < args.Length; i++)
				{
					string item = args[i];

					if (item[0] == '/')
					{
						switch (item.ToLower())
						{
							case "/r":
								// Set the background red colour
								mBackgroundR = Int32.Parse(args[++i]);
								break;
							case "/g":
								// Set the background green colour
								mBackgroundG = Int32.Parse(args[++i]);
								break;
							case "/b":
								// Set the background blue colour
								mBackgroundB = Int32.Parse(args[++i]);
								break;
							case "/width":
								// Set the width
								mFontWidth = Int32.Parse(args[++i]);
								break;
							case "/height":
								// Set the height
								mFontHeight = Int32.Parse(args[++i]);
								break;
							case "/infile":
								// Set the input file
								mFileNameIn = args[++i];

								// Set the bitmap file name
								mBitmapName = mFileNameIn.Substring(mFileNameIn.LastIndexOf('\\') + 1, (mFileNameIn.LastIndexOf('.') - (mFileNameIn.LastIndexOf('\\') + 1)));
								break;
							case "/classname":
								// Set the class name
								mClassName = args[++i];
								break;
							case "/fonttype":
								// Set the font type
								mFontType = args[++i];
								break;
							case "/?":
								// Help
								PrintHelp();
								break;
						}
					}
				}

				// Validate input
				if ((mFileNameIn != "") &&
					(mClassName != "") &&
					(mBitmapName != "") &&
					(mFontType != ""))
				{
					return true;
				}
				else
				{
					Error("Missing arguments.  Use /? to see the help text.");
				}
			}
			catch
			{
				Error("Unable to parse arguments");
			}

			return false;
		}

		#endregion
	}
}

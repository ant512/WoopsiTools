using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Drawing;
using Bmp2Font.Library;
using PrettyConsole.Library;

namespace Bmp2FontApp
{
	class Program
	{
		#region Constants

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
		static string mFontType = "packedfont1";
		static int mFontWidth = -1;
		static int mFontHeight = -1;

		#endregion

		#region Properties

		private static ConsolePrinter Helper { get; set; }

		#endregion

		#region Methods

		/// <summary>
		/// Main loop.
		/// </summary>
		/// <param name="args">Command line arguments</param>
		static void Main(string[] args)
		{
			Helper = new ConsolePrinter();

			Helper.Name = "Bmp2Font";
			Helper.Description = "Converts a BMP font image into a bit-packed font class for use with Woopsi.";
			Helper.Version = "V1.2";
			Helper.AddArgument(new Argument("INFILE", "string", "Path and filename of the BMP file to convert", false));
			Helper.AddArgument(new Argument("CLASSNAME", "string", "Name of the resultant font class", false));

			Argument arg = new Argument("FONTTYPE", "string", "Type of font to produce.  Options are:", true);
			arg.AddOption("packedfont1", "Monochrome packed proportional font");
			arg.AddOption("packedfont16", "16-bit packed proportional font");

			Helper.AddArgument(arg);

			Helper.AddArgument(new Argument("WIDTH", "int", "Width of a single character in the font", true));
			Helper.AddArgument(new Argument("HEIGHT", "int", "Height of a single character in the font", true));
			Helper.AddArgument(new Argument("R", "int", "Red component of the background colour", true));
			Helper.AddArgument(new Argument("G", "int", "Green component of the background colour", true));
			Helper.AddArgument(new Argument("B", "int", "Blue component of the background colour", true));

			Helper.AddParagraph(@"WIDTH and HEIGHT represent the width and height of a single character in the font.
Fonts must be fixed width.  If WIDTH and HEIGHT are not specified, they will be calculated from the dimensions of the
bitmap.  In that case, the bitmap must be a multiple of 32 wide (32 chars per line) and a multiple of 8 high
(32 * 8 = 256,the number of chars in a font).");
	
			Helper.AddParagraph(@"If the RGB background colour is not specified, it is automatically determined from
the colour of the top-left pixel.  If this pixel is not part of the background, the resultant font class will be corrupt.");

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
								Console.WriteLine(Helper.HelpText);
								break;
						}
					}
				}

				// Validate input
				if ((mFileNameIn != "") &&
					(mClassName != "") &&
					(mBitmapName != ""))
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

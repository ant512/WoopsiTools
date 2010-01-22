using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Drawing.Text;
using Woopsi.Font2Bmp;

namespace Font2BitmapApp
{
	class Program
	{
		#region Constants

		const string APP_VERSION = "V1.0";
		const string APP_NAME = "font2bmp";

		#endregion

		#region Members

		static string mFontName = "";
		static float mFontSize = 10f;
		static FontStyle mFontStyle = FontStyle.Regular;
		static int mBackgroundR = 255;
		static int mBackgroundG = 255;
		static int mBackgroundB = 255;
		static int mTextR = 0;
		static int mTextG = 0;
		static int mTextB = 0;
		static string mFileName = "";

		#endregion

		#region Methods

		/// <summary>
		/// Main program.
		/// </summary>
		/// <param name="args">Command line arguments.</param>
		static void Main(string[] args)
		{
			// Output title
			WriteTitle();

			// Fetch arguments
			ParseArgs(args);

			// Get the font to convert
			Font font = GetFont(mFontName, mFontSize, mFontStyle);

			// Convert the font to a bitmap
			Console.WriteLine("Converting font to bitmap...");
			Bitmap bmp = BitmapCreator.CreateFontBitmap(font, mBackgroundR, mBackgroundG, mBackgroundB, mTextR, mTextG, mTextB);
			Console.WriteLine("Font converted.");
			Console.WriteLine("");

			// Save the bitmap
			Console.WriteLine(String.Format("Saving bitmap to {0}...", mFileName));
			bmp.Save(mFileName, System.Drawing.Imaging.ImageFormat.Bmp);
			Console.WriteLine("All done!");
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
		/// Attempt to get the named font.
		/// </summary>
		/// <param name="fontName">The name of the font to retrieve.</param>
		/// <param name="fontSize">The size of the font to retrieve.</param>
		/// <param name="fontStyle">The style of the font to retrieve.</param>
		/// <returns></returns>
		static Font GetFont(string fontName, float fontSize, FontStyle fontStyle)
		{
			Console.WriteLine("Attempting to retrieve font...");

			// Create font based on font name/size arguments
			Font font = new Font(fontName, fontSize, fontStyle, GraphicsUnit.Pixel);

			// Does the font exist?
			if (!font.Name.ToLower().Equals(fontName.ToLower()))
			{
				Console.WriteLine("Font does not exist!");
				Console.ReadLine();
				Environment.Exit(0);
			}

			Console.WriteLine("Font found!");
			Console.WriteLine("");

			return font;
		}

		/// <summary>
		/// Print a list of available fonts.
		/// </summary>
		static void PrintFontList()
		{
			Console.WriteLine("Available fonts:");
			Console.WriteLine("");
			foreach (FontFamily item in FontFamily.Families)
			{
				Console.WriteLine(String.Format(" - {0}", item.Name));
			}
		}

		/// <summary>
		/// Display an error and quit.
		/// </summary>
		/// <param name="err">Error message to display.</param>
		static void Error(string err)
		{
			Console.WriteLine(String.Format("Error: {0}", err));
			Environment.Exit(1);
		}

		/// <summary>
		/// Print the help text.
		/// </summary>
		static void PrintHelp()
		{
			Console.WriteLine("Converts a Windows font to a BMP file.");
			Console.WriteLine("");
			Console.WriteLine("font2bmp /FONT string /SIZE int [/STYLE string] /FILE string");
			Console.WriteLine("         [/BACKR int] [/BACKG int] [/BACKB int]");
			Console.WriteLine("         [/TEXTR int] [/TEXTG int] [/TEXTB int]");
			Console.WriteLine("         [/LIST]");
			Console.WriteLine("");
			Console.WriteLine("/FONT          Name of the Windows font to convert");
			Console.WriteLine("/SIZE          Size of the font in pixels");
			Console.WriteLine("/STYLE         Style of the font.  Options are:");
			Console.WriteLine("");
			Console.WriteLine("                 regular  - Standard font");
			Console.WriteLine("                 bold     - Bold font");
			Console.WriteLine("                 italic   - Italic font");
			Console.WriteLine("");
			Console.WriteLine("/FILE          Output path and filename");
			Console.WriteLine("");
			Console.WriteLine("/BACKR         Red component of the background colour");
			Console.WriteLine("/BACKG         Green component of the background colour");
			Console.WriteLine("/BACKB         Blue component of the background colour");
			Console.WriteLine("");
			Console.WriteLine("/TEXTR         Red component of the text colour");
			Console.WriteLine("/TEXTG         Green component of the text colour");
			Console.WriteLine("/TEXTB         Blue component of the text colour");
			Console.WriteLine("");
			Console.WriteLine("/LIST          Lists all available Windows font names");
			Console.WriteLine("");
			Console.WriteLine("If the background colour is not specified it defaults to white.");
			Console.WriteLine("If the text colour is not specified it defaults to black.");
			Console.WriteLine("If the style is not specified it defaults to regular.");
		}

		/// <summary>
		/// Parse the command line arguments
		/// </summary>
		/// <param name="args">Command line arguments.</param>
		static void ParseArgs(string[] args)
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
							case "/backr":
								// Set the background red colour
								mBackgroundR = Int32.Parse(args[++i]);
								break;
							case "/backg":
								// Set the background green colour
								mBackgroundG = Int32.Parse(args[++i]);
								break;
							case "/backb":
								// Set the background blue colour
								mBackgroundB = Int32.Parse(args[++i]);
								break;
							case "/textr":
								// Set the text red colour
								mTextR = Int32.Parse(args[++i]);
								break;
							case "/textg":
								// Set the text green colour
								mTextG = Int32.Parse(args[++i]);
								break;
							case "/textb":
								// Set the text blue colour
								mTextB = Int32.Parse(args[++i]);
								break;
							case "/size":
								// Set the size
								mFontSize = Int32.Parse(args[++i]);
								break;
							case "/file":
								// Set the output filename
								mFileName = args[++i];
								break;
							case "/font":
								// Set the font name
								mFontName = args[++i];
								break;
							case "/style":
								// Set the font style
								string style = args[++i].ToLower();

								switch (style)
								{
									case "regular":
										mFontStyle = FontStyle.Regular;
										break;
									case "bold":
										mFontStyle = FontStyle.Bold;
										break;
									case "italic":
										mFontStyle = FontStyle.Italic;
										break;
								}
								break;
							case "/?":
								// Help
								PrintHelp();
								Environment.Exit(0);
								break;
							case "/list":
								// List of fonts
								PrintFontList();
								Environment.Exit(0);
								break;
						}
					}
				}

				// Prevent colours being partially defined
				if ((mBackgroundR == -1) || (mBackgroundG == -1) || (mBackgroundB == -1))
				{
					mBackgroundR = 255;
					mBackgroundG = 255;
					mBackgroundB = 255;
				}

				if ((mTextR == -1) || (mTextG == -1) || (mTextB == -1))
				{
					mTextR = 0;
					mTextG = 0;
					mTextB = 0;
				}

				// Validate input
				if ((mFileName == "") ||
					(mFontName == "") ||
					(Math.Floor(mFontSize) == 0f)) {
					Error("Missing arguments.  Use /? to see the help text.");
				}
			}
			catch
			{
				Error("Unable to parse arguments");
			}
		}

		#endregion
	}
}
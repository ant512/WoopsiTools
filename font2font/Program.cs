using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.IO;
using System.Drawing.Text;
using Font2Bmp.Library;
using Bmp2Font.Library;
using PrettyConsole.Library;

namespace font2font
{
	class Program
	{
		#region Constants

		const int CHARS_PER_ROW = 32;
		const int ROWS_PER_FONT = 8;

		#endregion

		#region Members

		static string mFontName = "";
		static float mFontSize = 0f;
		static FontStyle mFontStyle = FontStyle.Regular;
		static int mTextR = 0;
		static int mTextG = 0;
		static int mTextB = 0;
		static string mFontType = "packedfont1";
		static string mOutputPath = "";

		#endregion

		#region Properties

		private static ConsolePrinter Helper { get; set; }

		#endregion

		/// <summary>
		/// Main program.
		/// </summary>
		/// <param name="args">Command line arguments.</param>
		static void Main(string[] args)
		{
			Helper = new ConsolePrinter();

			Helper.Name = "Font2Font";
			Helper.Description = "Converts a Windows font to a Woopsi font.";
			Helper.Version = "V1.2";
			Helper.AddArgument(new Argument("FONT", "string", "Name of the Windows font to convert", false));
			Helper.AddArgument(new Argument("SIZE", "int", "Size of the font in pixels", false));
			Argument arg = new Argument("STYLE", "int", "Style of the font.  Options are:", true);
			arg.AddOption("regular", "Standard font");
			arg.AddOption("bold", "Bold font");
			arg.AddOption("italic", "Italic font");

			Helper.AddArgument(arg);

			arg = new Argument("FONTTYPE", "string", "Type of font to produce.  Options are:", true);
			arg.AddOption("packedfont1", "Monochrome packed proportional font");
			arg.AddOption("packedfont16", "16-bit packed proportional font");

			Helper.AddArgument(arg);

			Helper.AddArgument(new Argument("PATH", "string", "Output path", false));
			Helper.AddArgument(new Argument("R", "int", "Red component of the text colour", true));
			Helper.AddArgument(new Argument("G", "int", "Green component of the text colour", true));
			Helper.AddArgument(new Argument("B", "int", "Blue component of the text colour", true));
			Helper.AddArgument(new Argument("LIST", "int", "Lists all available Windows font names", true));

			Helper.AddParagraph("If the text colour is not specified it defaults to black.");
			Helper.AddParagraph("If the style is not specified it defaults to regular.");
			Helper.AddParagraph("If the path is not specified it defaults to the current path.");
			Helper.AddParagraph("If the font type is not specified it defaults to packedfont1.");

			Console.WriteLine(Helper.Title);

			Console.WriteLine(Helper.HelpText);

			// Fetch arguments
			ParseArgs(args);

			// Use background colour that is opposite of text colour
			int backgroundR = (mTextR ^ 255) & 255;
			int backgroundG = (mTextG ^ 255) & 255;
			int backgroundB = (mTextB ^ 255) & 255;

			// Get the font
			Font font = GetFont(mFontName, mFontSize, mFontStyle);

			// Convert the font to a bitmap
			Bitmap bmp = BitmapCreator.CreateFontBitmap(font, backgroundR, backgroundG, backgroundB, mTextR, mTextG, mTextB);

			// Convert bitmap to Woopsi font
			WoopsiFont woopsiFont = Converter.Convert(mFontName, mFontName, mFontType, bmp, bmp.Width / CHARS_PER_ROW, bmp.Height / ROWS_PER_FONT, backgroundR, backgroundG, backgroundB);

			// Output files
			WriteFile(mOutputPath + "\\" + woopsiFont.HeaderFileName, woopsiFont.HeaderContent);
			WriteFile(mOutputPath + "\\" + woopsiFont.BodyFileName, woopsiFont.BodyContent);

			Console.WriteLine("All done!");
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
							case "/r":
								// Set the text red colour
								mTextR = Int32.Parse(args[++i]);
								break;
							case "/g":
								// Set the text green colour
								mTextG = Int32.Parse(args[++i]);
								break;
							case "/b":
								// Set the text blue colour
								mTextB = Int32.Parse(args[++i]);
								break;
							case "/size":
								// Set the size
								mFontSize = Int32.Parse(args[++i]);
								break;
							case "/path":
								// Set the output filename
								mOutputPath = args[++i];
								break;
							case "/font":
								// Set the font name
								mFontName = args[++i];
								break;
							case "/fonttype":
								// Set the font type
								mFontType = args[++i];
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
								Console.WriteLine(Helper.HelpText);
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
				if ((mTextR == -1) || (mTextG == -1) || (mTextB == -1))
				{
					mTextR = 0;
					mTextG = 0;
					mTextB = 0;
				}

				// Validate input
				if ((mFontName == "") ||
					(Math.Floor(mFontSize) == 0f))
				{
					Error("Missing arguments.  Use /? to see the help text.");
				}
			}
			catch
			{
				Error("Unable to parse arguments");
			}
		}

		/// <summary>
		/// Perform the conversion.
		/// </summary>
		static void ConvertFont()
		{
			// Use background colour that is opposite of text colour
			int backgroundR = mTextR ^ 255;
			int backgroundG = mTextG ^ 255;
			int backgroundB = mTextB ^ 255;

			// Get the font
			Font font = GetFont(mFontName, mFontSize, mFontStyle);

			// Convert the font to a bitmap
			Bitmap bmp = BitmapCreator.CreateFontBitmap(font, backgroundR, backgroundG, backgroundB, mTextR, mTextG, mTextB);

			// Convert bitmap to Woopsi font
			WoopsiFont woopsiFont = Converter.Convert(mFontName, mFontName, mFontType, bmp, bmp.Width / CHARS_PER_ROW, bmp.Height / ROWS_PER_FONT, backgroundR, backgroundG, backgroundB);

			// Output files
			WriteFile(mOutputPath + "\\" + woopsiFont.HeaderFileName, woopsiFont.HeaderContent);
			WriteFile(mOutputPath + "\\" + woopsiFont.BodyFileName, woopsiFont.BodyContent);
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
		/// Show an error message and quit.
		/// </summary>
		/// <param name="msg">Message to show.</param>
		static void Error(string msg)
		{
			Console.WriteLine(String.Format("Error: {0}", msg));
			Environment.Exit(1);
		}
	}
}

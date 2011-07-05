using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.IO;
using PrettyConsole.Library;

namespace Bmp2Bitmap
{
	class Converter
	{
		#region Members

		string mFileNameIn = "";
		string mFileNameOutCPP = "";
		string mFileNameOutH = "";
		string mClassName = "";
		bool mError = false;
		Bitmap mBitmap = null;
		protected StringBuilder mOutputCPP = new StringBuilder();
		protected StringBuilder mOutputH = new StringBuilder();

		#endregion

		#region Properties

		private ConsolePrinter Helper { get; set; }

		#endregion

		/// <summary>
		/// Convert the font.
		/// </summary>
		/// <param name="args">Command line arguments</param>
		public void Convert(string[] args)
		{
			Helper = new ConsolePrinter();

			Helper.Name = "Bmp2Bitmap";
			Helper.Description = "Converts a BMP image into a bitmap class for use with Woopsi.";
			Helper.Version = "V1.2";
			Helper.AddArgument(new Argument("INFILE", "string", "Path and filename of the BMP file to convert", false));
			Helper.AddArgument(new Argument("CLASSNAME", "string", "Name of the resultant bitmap class", false));

			Console.WriteLine(String.Format("{0}\n", Helper.Title));

			// Fetch arguments
			ParseArgs(args);

			if (mError) return;

			// Check arguments are valid
			if (ValidateArgs())
			{
				// Load original file
				LoadFile();

				if (mError) return;

				// Automatically choose an output name if not set in arguments
				GenerateOutputNames();

				if (mError) return;

				// Perform the conversion
				BuildCPPOutput();
				BuildHOutput();

				// Output files
				WriteFile(mFileNameOutCPP, mOutputCPP);
				WriteFile(mFileNameOutH, mOutputH);

				Console.WriteLine("All done!");
			}
		}

		/// <summary>
		/// Create the output file.
		/// </summary>
		void BuildCPPOutput()
		{
			Console.WriteLine("Packing data");

			// Open array
			mOutputCPP.Append("] __attribute__ ((aligned (4))) = {\n");

			// Scan through all image pixels
			int values = 0;
			for (int y = 0; y < mBitmap.Height; y++)
			{
				for (int x = 0; x < mBitmap.Width; x++)
				{
					Color col = mBitmap.GetPixel(x, y);

					mOutputCPP.Append(ColorToRGBA16(col).ToString());
					mOutputCPP.Append(", ");

					values++;

					if (values % 11 == 0) mOutputCPP.Append("\n");
				}
			}

			// Strip the trailing comma and space
			int lastCommaPos = mOutputCPP.ToString().LastIndexOf(',');
			mOutputCPP.Remove(lastCommaPos, mOutputCPP.Length - lastCommaPos);

			// Append closing bracket and semi-colon
			mOutputCPP.Append("\n};\n\n");

			// Append font constructor
			mOutputCPP.Append(String.Format("{0}::{0}() : WoopsiGfx::BitmapWrapper({1}_Bitmap, {2}, {3})",
				mClassName,
				mClassName.ToLower(),
				mBitmap.Width.ToString(),
				mBitmap.Height.ToString()));

			mOutputCPP.Append(" { };");

			// Insert opening data in reverse order
			mOutputCPP.Insert(0, "\n");
			mOutputCPP.Insert(0, values.ToString());
			mOutputCPP.Insert(0, "_Bitmap[");
			mOutputCPP.Insert(0, mClassName.ToLower());
			mOutputCPP.Insert(0, "static const u16 ");
			mOutputCPP.Insert(0, "\n");
			mOutputCPP.Insert(0, String.Format("#include \"{0}\"\n", mFileNameOutH));
			mOutputCPP.Insert(0, "#include <nds.h>\n");

			Console.WriteLine("Pack OK");
		}

		/// <summary>
		/// Create a .h file to accompany the .cpp file.
		/// </summary>
		void BuildHOutput()
		{
			mOutputH.Append(String.Format("#ifndef _{0}_H_\n", mClassName.ToUpper()));
			mOutputH.Append(String.Format("#define _{0}_H_\n", mClassName.ToUpper()));
			mOutputH.Append("\n");
			mOutputH.Append("#include <bitmapwrapper.h>\n");
			mOutputH.Append("\n");
			mOutputH.Append(String.Format("class {0} : public WoopsiGfx::BitmapWrapper", mClassName));
			mOutputH.Append(" {\n");
			mOutputH.Append("public:\n");
			mOutputH.Append(String.Format("\t{0}();\n", mClassName));
			mOutputH.Append("};\n");
			mOutputH.Append("\n");
			mOutputH.Append("#endif\n");
		}

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
		/// If the output filename has not been specified, use the input name sans
		/// extension.
		/// </summary>
		void GenerateOutputNames()
		{
			// Ensure output is lower-case
			string name = mClassName.ToLower();

			// Ensure output does not contain a file extension
			if (name.IndexOf('.') > -1)
			{
				name = name.Substring(0, name.IndexOf('.'));
			}

			mFileNameOutCPP = name + ".cpp";
			mFileNameOutH = name + ".h";
		}

		/// <summary>
		/// Validate the command line arguments.
		/// </summary>
		/// <returns>True if the arguments are valid; false if not</returns>
		bool ValidateArgs()
		{
			// Validate input
			if ((mFileNameIn != "") && (mClassName != "")) return true;

			// Validation failed
			Error("Missing arguments.  Use /? to see the help text.");
			return false;
		}

		/// <summary>
		/// Parse the command line arguments
		/// </summary>
		/// <param name="args">Command line arguments.</param>
		void ParseArgs(string[] args)
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
							case "/infile":
								// Set the input file
								mFileNameIn = args[++i];
								break;
							case "/classname":
								// Set the class name
								mClassName = args[++i];
								break;
							case "/?":
								// Help
								Console.WriteLine(Helper.HelpText);
								break;
						}
					}
				}
			}
			catch
			{
				Error("Unable to parse arguments");
			}
		}

		/// <summary>
		/// Load source bitmap
		/// </summary>
		void LoadFile()
		{
			Console.WriteLine("Loading bitmap");

			try
			{
				mBitmap = Bitmap.FromFile(mFileNameIn) as Bitmap;
				Console.WriteLine("Load OK");
			}
			catch
			{
				Error("Unable to load bitmap");
				return;
			}
		}

		/// <summary>
		/// Output data to disk.
		/// </summary>
		void WriteFile(string filename, StringBuilder data)
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
		/// Show an error message and set the error flag to true.
		/// </summary>
		/// <param name="msg">Message to show.</param>
		void Error(string msg)
		{
			Console.WriteLine(String.Format("Error: {0}", msg));
			mError = true;
		}
	}
}

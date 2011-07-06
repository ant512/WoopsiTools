using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace FontConverter
{
	public partial class Form1 : Form
	{
		#region Constants

		const int CHARS_PER_ROW = 32;
		const int ROWS_PER_FONT = 8;

		#endregion

		private Bitmap mBitmap = null;
		private Color mTextColour = Color.FromArgb(255, 255, 255, 255);
		private Color mBackgroundColour = Color.FromArgb(255, 0, 0, 0);

		public Form1()
		{
			InitializeComponent();

			FontListBox.DataSource = FontFamily.Families;
			FontListBox.DisplayMember = "Name";
			FontListBox.ValueMember = "Name";

			List<FontStyle> styles = new List<FontStyle> { FontStyle.Regular, FontStyle.Bold, FontStyle.Italic };
			StyleListBox.DataSource = styles;

			List<string> fontTypes = new List<string>{ "PackedFont1", "PackedFont16" };
			FontTypeListBox.DataSource = fontTypes;

			List<int> sizes = new List<int>();
			for (int i = 5; i < 21; ++i)
			{
				sizes.Add(i);
			}
			SizeListBox.DataSource = sizes;
		}

		private void FontListBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (StyleListBox.SelectedValue == null) return;

			CreateFontBitmap();
		}

		private void CreateFontBitmap()
		{
			Font font = null;

			try
			{
				font = new Font((FontFamily)FontListBox.SelectedItem, Convert.ToInt32(SizeListBox.SelectedValue), (FontStyle)StyleListBox.SelectedValue, GraphicsUnit.Pixel);
			}
			catch (ArgumentException)
			{
				// Really don't care that the font doesn't support the style
			}

			if (font != null)
			{
				mBitmap = Font2Bmp.Library.BitmapCreator.CreateFontBitmap(font, mBackgroundColour, mTextColour);
				FontPictureBox.Image = mBitmap;
			}
		}

		private void StyleListBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			CreateFontBitmap();
		}

		private void SaveButton_Click(object sender, EventArgs e)
		{
			if (ClassNameTextBox.Text == String.Empty)
			{
				MessageBox.Show("Please enter a class name.");
				return;
			}

			FolderBrowserDialog dialog = new FolderBrowserDialog();
			DialogResult result = dialog.ShowDialog();

			if (result == System.Windows.Forms.DialogResult.OK) {
				Bmp2Font.Library.WoopsiFont woopsiFont = Bmp2Font.Library.Converter.Convert(ClassNameTextBox.Text, ClassNameTextBox.Text.ToLower(), FontTypeListBox.SelectedValue.ToString().ToLower(), mBitmap, mBitmap.Width / CHARS_PER_ROW, mBitmap.Height / ROWS_PER_FONT, 0, 0, 0);

				WriteFile(dialog.SelectedPath + "\\" + woopsiFont.HeaderFileName, woopsiFont.HeaderContent);
				WriteFile(dialog.SelectedPath + "\\" + woopsiFont.BodyFileName, woopsiFont.BodyContent);
			}
		}

		private void WriteFile(string filename, string data)
		{
			Console.WriteLine(String.Format("Writing file: {0}", filename));

			StreamWriter file = null;

			try
			{
				file = new StreamWriter(filename);
			}
			catch
			{
				MessageBox.Show("Unable to write file");
				return;
			}

			try
			{
				file.Write(data);
			}
			catch
			{
				MessageBox.Show("Unable to write file");
			}
			finally
			{
				file.Close();
				file.Dispose();
			}
		}

		private void TextColourButton_Click(object sender, EventArgs e)
		{
			ColorDialog dialog = new ColorDialog();

			dialog.Color = mTextColour;

			if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
			{
				mTextColour = dialog.Color;
				CreateFontBitmap();
			}
		}

		private void BackgroundColourButton_Click(object sender, EventArgs e)
		{
			ColorDialog dialog = new ColorDialog();

			dialog.Color = mBackgroundColour;

			if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
			{
				mBackgroundColour = dialog.Color;
				CreateFontBitmap();
			}
		}

		private void SizeListBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			CreateFontBitmap();
		}
	}
}

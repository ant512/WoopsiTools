using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Text;
using System.Text;

namespace Font2Bmp.Library
{
	/// <summary>
	/// Creates a bitmap from a Windows font.
	/// </summary>
	public class BitmapCreator
	{
		#region Methods

		/// <summary>
		/// Creates a bitmap from a Windows font.
		/// </summary>
		/// <param name="font">The font to convert.</param>
		/// <param name="backgroundR">The red component of the background colour.</param>
		/// <param name="backgroundG">The green component of the background colour.</param>
		/// <param name="backgroundB">The blue component of the background colour.</param>
		/// <param name="textR">The red component of the text colour.</param>
		/// <param name="textG">The green component of the text colour.</param>
		/// <param name="textB">The blue component of the text colour.</param>
		/// <returns></returns>
		static public Bitmap CreateFontBitmap(Font font, int backgroundR, int backgroundG, int backgroundB, int textR, int textG, int textB)
		{
			return CreateFontBitmap(font, Color.FromArgb(backgroundR, backgroundG, backgroundB), Color.FromArgb(textR, textG, textB));
		}

		/// <summary>
		/// Creates a bitmap from a Windows font.
		/// </summary>
		/// <param name="backgroundColour">The background colour.</param>
		/// <param name="textColour">The text colour.</param>
		/// <param name="font">The font to convert.</param>
		/// <returns></returns>
		static public Bitmap CreateFontBitmap(Font font, Color backgroundColour, Color textColour)
		{
			// Ensure the font will be drawn cleanly without anti-aliasing
			TextRenderingHint renderingHint = TextRenderingHint.SingleBitPerPixelGridFit;

			List<Bitmap> glyphBitmaps = new List<Bitmap>();
			List<Rectangle> glyphRects = new List<Rectangle>();

			int fontWidth = 0;
			int fontHeight = 0;
			int fontYOffset = font.Height;

			Brush backgroundBrush = new SolidBrush(backgroundColour);
			Brush textBrush = new SolidBrush(textColour);

			string output;

			// Get dimensions of each glyph based on actual drawn pixels, rather
			// than rely .NET's dimensions.  At the same time we create a separate
			// bitmap for each glyph that we will use later to create the final composite
			// of the font
			for (int i = 32; i < 127; ++i)
			{
				output = ((char)i).ToString();

				// Draw glyph to temporary bitmap
				Bitmap glyphBmp = new Bitmap((int)font.Size * 2, (int)font.Size * 2);
				glyphBitmaps.Add(glyphBmp);

				Graphics glyphGfx = Graphics.FromImage(glyphBmp);
				glyphGfx.TextRenderingHint = renderingHint;
				glyphGfx.FillRectangle(backgroundBrush, new Rectangle(0, 0, glyphBmp.Width, glyphBmp.Height));
				glyphGfx.DrawString(output, font, textBrush, new PointF(0, 0));

				// Get the dimensions of the glyph
				Rectangle rect = GetGlyphDimensions(glyphBmp, textColour);
				glyphRects.Add(rect);

				// Remember the dimensions of the glyph if this is the largest yet observed - 
				// this will give us the dimensions of the font
				if (rect.Width > fontWidth) fontWidth = rect.Width;
				if (rect.Height + rect.Y > fontHeight) fontHeight = rect.Height + rect.Y;

				// The fontYOffset value stores the y co-ordinate of the minimum drawn
				// pixel from the top of the glyph.  It tells us how much wasted space
				// there is between the top of what .NET considers to be the glyph
				// and the actual pixel data.  At the end of the loop we have a value
				// that tells us amount of wasted space that is valid for every glyph
				// in the font
				if (fontYOffset > rect.Y) fontYOffset = rect.Y;
			}

			// Ensure that our value for the height of the font is not made incorrect
			// by the amount of wasted space above the glyphs
			fontHeight -= fontYOffset;

			// Now that we know exactly how big the font is, we can create
			// a bitmap to store the font composite
			int bmpWidth = (int)(fontWidth * 32);
			int bmpHeight = (int)(fontHeight * 8);

			Bitmap bmp = new Bitmap(bmpWidth, bmpHeight, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

			// Create and prepare graphics object to draw with
			Graphics gfx = Graphics.FromImage(bmp);
			gfx.FillRectangle(backgroundBrush, new Rectangle(0, 0, bmp.Width, bmp.Height));

			// Copy all font glyphs to the bitmap
			int glyph = 0;
			for (int y = 1; y < 4; ++y)
			{
				for (int x = 0; x < 32; ++x)
				{
					// Stop if we have no more glyphs to draw
					if (glyph >= glyphRects.Count) break;

					// Only draw the glyph if it has any content
					if (glyphRects[glyph].Width > 0)
					{
						// Calculate the area to copy from.  We only copy the glyph itself,
						// not the surrounding padding, so that when we paste it to the composite
						// the glyph is left-aligned
						int sourceX = glyphRects[glyph].X;
						int sourceY = fontYOffset;
						int sourceWidth = glyphRects[glyph].Width;
						int sourceHeight = fontHeight;

						// Calculate the area to copy to.  All glyphs are drawn in a regular grid
						int destX = x * fontWidth;
						int destY = y * fontHeight;
						int destWidth = sourceWidth;
						int destHeight = sourceHeight;

						Rectangle sourceRect = new Rectangle(sourceX, sourceY, sourceWidth, sourceHeight);
						Rectangle destRect = new Rectangle(destX, destY, destWidth, destHeight);

						// Perform the copy
						gfx.DrawImage(glyphBitmaps[glyph], destRect, sourceRect, GraphicsUnit.Pixel);
					}

					glyph++;
				}
			}

			return bmp;
		}

		/// <summary>
		/// Get the dimensions of the glyph that is drawn on the supplied bitmap.  Loops
		/// through all pixels and identifies the co-ordinates of the top, left, bottom and
		/// rightmost pixels that contain the supplied colour.
		/// </summary>
		/// <param name="bmp">The bitmap to analyse.</param>
		/// <param name="colour">The colour that should be treated as foreground.</param>
		/// <returns>A rectangle describing the used region of the bitmap.</returns>
		static Rectangle GetGlyphDimensions(Bitmap bmp, Color colour)
		{
			int left = bmp.Width;
			int top = bmp.Height;
			int bottom = -1;
			int right = -1;

			for (int x = 0; x < bmp.Width; ++x)
			{
				for (int y = 0; y < bmp.Height; ++y)
				{
					if (bmp.GetPixel(x, y).Equals(colour))
					{
						if (left > x) left = x;
						if (top > y) top = y;
						if (bottom < y) bottom = y;
						if (right < x) right = x;
					}
				}
			}

			return new Rectangle(left, top, (right - left) + 1, (bottom - top) + 1);
		}

		#endregion
	}
}

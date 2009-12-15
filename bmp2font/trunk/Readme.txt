* Copyright (c) 2008-2009, Antony Dzeryn
* All rights reserved.
*
* Redistribution and use in source and binary forms, with or without
* modification, are permitted provided that the following conditions are met:
*     * Redistributions of source code must retain the above copyright
*       notice, this list of conditions and the following disclaimer.
*     * Redistributions in binary form must reproduce the above copyright
*       notice, this list of conditions and the following disclaimer in the
*       documentation and/or other materials provided with the distribution.
*     * Neither the names "FontBitPacker", "Woopsi", "Simian Zombie" nor the
*       names of its contributors may be used to endorse or promote products
*       derived from this software without specific prior written permission.
*
* THIS SOFTWARE IS PROVIDED BY Antony Dzeryn ``AS IS'' AND ANY
* EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
* WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
* DISCLAIMED. IN NO EVENT SHALL Antony Dzeryn BE LIABLE FOR ANY
* DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES
* (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES;
* LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND
* ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
* (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
* SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.

  
bmp2font
--------

  bmp2font is a utility designed to convert BMP files containing font glyphs
  into Woopsi-compatible C++ classes.  It can convert files to Font, MonoFont,
  PackedFont1 and PackedFont16 classes.

  
Usage
-----

  bmp2font is a command line tool.  To use it, open a DOS prompt and type:
  
  bmp2font /infile <in> /classname <class> /fonttype /r <red> /g <green> /b
  <blue> /width <width> /height <height>
  
  Replace the text in brackets like this:

   - infile   - This should be the full path and filename for the BMP file.
   - outfile  - This is the full path and filename of the file you want to
                create.
   - fonttype - Type of font to produce.  Options are monofont, font,
                packedfont1 and packedfont16.
   - r        - This is the red component of the background colour used in the
                font.  This will be made transparent.
   - g        - This is the green component of the background colour used in the
                font.  This will be made transparent.
   - b        - This is the blue component of the background colour used in the
                font.  This will be made transparent.
   - width    - This is the width of a single character in the font.  Fonts are
                fixed width.
   - height   - This is the height of a single character in the font.

  If the RGB background colour is not specified, the utility uses the top-left
  pixel of the bitmap instead.
  
  If the width and height of the font are not specified, the utility bases its
  dimensions on the width and height of the bitmap.  In this situation, the
  bitmap must be 32 characters wide and 8 characters tall.  Not all glyphs need
  to be present in the bitmap.


Requirements
------------

  To run the utility you will need Windows and the .NET3.5 redistributable.  To
  compile the sourcecode you need Visual Studio 2008.
   

Links
-----

 - http://ant.simianzombie.com                    - My main site
 - http://www.sourceforge.net/projects/woopsi     - Woopsi SourceForge page


Email
-----

  Contact me at spam_mail250@yahoo.com.
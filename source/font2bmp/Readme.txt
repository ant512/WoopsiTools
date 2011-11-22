* Copyright (c) 2009-2010, Antony Dzeryn
* All rights reserved.
*
* Redistribution and use in source and binary forms, with or without
* modification, are permitted provided that the following conditions are met:
*     * Redistributions of source code must retain the above copyright
*       notice, this list of conditions and the following disclaimer.
*     * Redistributions in binary form must reproduce the above copyright
*       notice, this list of conditions and the following disclaimer in the
*       documentation and/or other materials provided with the distribution.
*     * Neither the names "font2bmp", "Woopsi", "Simian Zombie" nor the
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

  
font2bmp
--------

  font2bmp is a utility designed to convert Windows fonts into a BMP file.  The
  BMP will contain the standard ASCII set laid out in a regular grid of 8 rows
  by 32 columns.  The bitmap can be converted to a Woopsi font class by using
  the bmp2font program.  Alternatively, that step can be omitted by using the
  font2font program, which will convert straight from a Windows font to a Woopsi
  font class.


Usage
-----

  font2bmp is a command line tool.  To use it, open a DOS prompt and type:
  
  font2bmp /font <font> /size <size> /style <style> /file <file> /backr <backr>
  /backg <backg> /backb <backb> /textr <textr> /textg <textg> /textb <textb>
  /list
  
   - font       - The name of the font to convert.
   - size       - The size of the font, in pixels.
   - style      - The style of the font, which can be either regular, bold or
                  italic.
   - file       - The output file and path.
   - backr      - The red component of the background colour.
   - backg      - The green component of the background colour.
   - backb      - The blue component of the background colour.
   - textr      - The red component of the text colour.
   - textb      - The blue component of the text colour.
   - textg      - The green component of the text colour.
  
  The "list" option overrides all others.  If set, this option causes the
  program to print a list of all available Windows font names, which can be used
  as the parameter for the "font" option.

  If no background colour is specified, or the colour is partially specified,
  it defaults to white.  If the text colour is not specified it defaults to
  black.  If the style is not specified it defaults to regular.
   

Requirements
------------

  To run the utility you will need Windows and the .NET3.5 redistributable.  To
  compile the sourcecode you need Visual Studio 2008.
   

Links
-----

 - http://simianzombie.com                    - My main site
 - http://bitbucket.org/ant512/woopsi         - Bitbucket page


Email
-----

  Contact me at ant@simianzombie.com.
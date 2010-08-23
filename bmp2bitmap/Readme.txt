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
*     * Neither the names "bmp2bitmap", "Woopsi", "Simian Zombie" nor the
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

  
bmp2bitmap
----------

  bmp2bitmap is a utility designed to convert BMP files into Woopsi-compatible
  C++ classes.  These classes can then be embedded into NDS ROM files, removing
  the need to load the BMP files from disk or manually wrap the raw bitmap data
  in a struct describing the original bitmap.

  
Usage
-----

  bmp2bitmap is a command line tool.  To use it, open a DOS prompt and type:
  
  bmp2bitmap /infile <in> /classname <class>
  
   - infile     - This should be the full path and filename for the BMP file.
   - classname  - This is the name of the class to be create.  The resultant
                  file with have the same name, albeit in lower-case.


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
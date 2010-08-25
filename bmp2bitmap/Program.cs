using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bmp2Bitmap
{
	class Program
	{
		static void Main(string[] args)
		{
			Converter converter = new Converter();
			converter.Convert(args);
		}
	}
}

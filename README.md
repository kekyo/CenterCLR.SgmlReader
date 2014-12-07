# SgmlReader for Portable Class Library

## What is this?

* SgmlReader is "SGML" markup language parser, and derived from System.Xml.XmlReader in .NET CLR.
* But, most popular usage the "HTML" parser. (It's scraper!!)

* SgmlReader based Mindtouch SgmlReader 1.8.11 (https://github.com/mindtouch/sgmlreader)
* Changed to Portable class library, target platforms:
 * .NET Framework 4.0 or upper.
 * Silverlight 4 or upper.
 * Windows phone 7 or upper.
 * Windows store apps (Windows 8 or upper).
 * Xbox 360.

* More easy usage: HTML4 DTD included. - HTML scraping ready!
* Unit test included.
* Class interface little bit changed from original implementation.

* NuGet package ID: SgmlReaderPortable (https://www.nuget.org/packages/SgmlReaderPortable/)
* GitHub: https://github.com/kekyo/CenterCLR.SgmlReader.git

* Build requirements:
 * Visual Studio 2012
 * NuBuild Project System (https://visualstudiogallery.msdn.microsoft.com/3efbfdea-7d51-4d45-a954-74a2df51c5d0)

* Enjoy!

## Standard usage
```
// Open stream
using (var stream = new FileStream("target.html", FileMode.Open, FileAccess.Read, FileShare.Read))
{
	var tr = new StreamReader(stream, Encoding.UTF8);

	// Setup SgmlReader
	var sgmlReader = new SgmlReader(stream)
		{
			DocType = "HTML",				// Use internal HTML4 DTD (No external access)
			WhitespaceHandling = true,		// Means WhitespaceHandling.All
			CaseFolding = CaseFolding.ToLower
		};

	// create document
	var document = new XmlDocument();
	document.PreserveWhitespace = true;
	document.XmlResolver = null;
	document.Load(sgmlReader);
}
```

## External reference capability usage (SGML handling, not tested :-)
```
// Open stream
using (var stream = new FileStream("target.sgml", FileMode.Open, FileAccess.Read, FileShare.Read))
{
	var tr = new StreamReader(stream, Encoding.UTF8);

	// Define base uri
	var baseUri = new Uri("http://www.example.com/");

	// Setup SgmlReader
	var sgmlReader = new SgmlReader(stream, baseUri,

		// Stream opener delegate (Separate physical resource access)
		uri => new StreamInformation
			{
				Stream = WebRequest.Create(uri).GetResponse().GetResponseStream(),
				DefaultEncoding = Encoding.UTF8
			})

		{
			WhitespaceHandling = true,
			CaseFolding = CaseFolding.ToLower
		};

	// create document
	var document = new XmlDocument();
	document.PreserveWhitespace = true;
	document.XmlResolver = null;
	document.Load(sgmlReader);
}
```

## Versions

* 2014.12.7.1:
 * Namespace changed "CenterCLR.Sgml".
 * More easy usage, HTML parse is default mode.
 * Native store app library included.
* 1.8.11.2014:
 * Initial release.

## Derived original copyrights
```
/*
 * 
 * An XmlReader implementation for loading SGML (including HTML) converting it
 * to well formed XML, by adding missing quotes, empty attribute values, ignoring
 * duplicate attributes, case folding on tag names, adding missing closing tags
 * based on SGML DTD information, and so on.
 *
 * Copyright (c) 2002 Microsoft Corporation. All rights reserved. (Chris Lovett)
 *
 */

/*
 * 
 * Copyright (c) 2007-2013 MindTouch. All rights reserved.
 * www.mindtouch.com  oss@mindtouch.com
 *
 * For community documentation and downloads visit wiki.developer.mindtouch.com;
 * please review the licensing section.
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * 
 *     http://www.apache.org/licenses/LICENSE-2.0
 * 
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 *
 */
```

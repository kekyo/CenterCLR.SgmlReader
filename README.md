# SgmlReader for Portable Class Library

![SgmlReader for Portable Class Library](https://raw.githubusercontent.com/kekyo/CenterCLR.SgmlReader/master/CenterCLR.SgmlReader.128.png)

## Status
* NuGet Package: [![NuGet SgmlReader](https://img.shields.io/nuget/v/CenterCLR.SgmlReader.svg?style=flat)](https://www.nuget.org/packages/CenterCLR.SgmlReader)
* NuGet Package (Older PCLs): [![NuGet SgmlReader (Older PCLs)](https://img.shields.io/badge/nuget-v2017.6.12-blue.svg?style=flat)](https://www.nuget.org/packages/CenterCLR.SgmlReader/2017.6.12)
* Continuous integration: [![AppVeyor SgmlReader](https://img.shields.io/appveyor/ci/kekyo/centerclr-sgmlreader.svg?style=flat)](https://ci.appveyor.com/project/kekyo/centerclr-sgmlreader)

* Currently CI tests are broken. (These tests passed by running manually)

## What is this?

* SgmlReader is "SGML" markup language parser, and derived from System.Xml.XmlReader in .NET CLR.
* But, most popular usage the "HTML" parser. (It's scraper!!)

## Easy usage for HTML parse and get truly XDocument.
``` csharp
// Open from stream
using (var stream = new FileStream("target.html", FileMode.Open, FileAccess.Read, FileShare.Read))
{
    // Parse Html mode (Easy usage)
    XDocument document = SgmlReader.Parse(stream);

    // Manipulate XDocument anything...
}
```

## Details

* SgmlReader based Mindtouch SgmlReader 1.8.11 (https://github.com/mindtouch/sgmlreader)
* Supported Portable class library, target platforms:
  * .NET Framework 4.0 (not net40-Client) or upper.
  * .NET Standard 1.0 or upper.
  * .NET Core 2.0 or upper.

* More easy usage: HTML4 DTD included. - HTML scraping ready!
* Unit test included.
* Class interface little bit changed from original implementation.

* NuGet package ID: CenterCLR.SgmlReader (https://www.nuget.org/packages/CenterCLR.SgmlReader/)
* GitHub: https://github.com/kekyo/CenterCLR.SgmlReader.git

* Build requirements:
  * Visual Studio 2017

* Advent calendar-driven project :-)  (http://www.kekyo.net/2014/12/08/4441  in Japanese)

* Enjoy!

## External reference capability usage (SGML handling, not tested :-)
``` csharp
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
* 2018.8.30:
  * Support for .NET Framework 4.0, .NET Standard 1.0/2.0 and .NET Core 2.0.
  * Obsoleted all PCLs and net40-Client. If you use these platforms, try to fixed nuget package version at older.
  * Obsoleted key-signed.
  * Switched to new MSBuild format.
* 2017.6.12:
  * Support .NET Standard 1.0.
* 2016.3.27.2:
  * Add .NET 3.5-Client/4.0-Client assembly (with serializable exception type).
* 2016.3.27.1:
  * Refactor by target platforms.
  * Add PCL3 (dnxcore50)
* 2014.12.7.3:
  * Add 1 line parse method.
* 2014.12.7.2:
  * Direct handling the Stream class.
  * Initial parameter is set of Html parse mode.
* 2014.12.7.1:
  * Namespace changed "CenterCLR.Sgml".
  * More easy usage, HTML parse is default mode.
  * Native store app library included.
* 1.8.11.2014:
  * Initial release.

## Derived original copyrights
``` csharp
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

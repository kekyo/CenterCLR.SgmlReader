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

/*
 * SgmlReader for Portable 
 * Copyright (c) 2014 Kouji Matsui, All rights reserved.
 * https://github.com/kekyo/CenterCLR.SgmlReader.git
 * Old project site : http://sgmlreaderportable.codeplex.com
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

using System;
using System.IO;
using System.Text;
using System.Xml;
using CenterCLR.Sgml;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CenterCLR.SgmlReaderTests
{
	public partial class Tests
	{

		//--- Types ---
		private delegate void XmlReaderTestCallback(XmlReader reader, XmlWriter xmlWriter);

		private enum XmlRender
		{
			Doc,
			DocClone,
			Passthrough
		}

		//--- Class Fields ---
		private static bool _debug = true;

		//--- Class Methods ---
		private static void Test(string name, XmlRender xmlRender, CaseFolding caseFolding, string doctype, bool format)
		{
			string source;
			string expected;
			ReadTest(name, out source, out expected);
			expected = expected.Trim().Replace("\r", "");
			string actual;

			// determine how the document should be written back
			XmlReaderTestCallback callback;
			switch (xmlRender)
			{
				case XmlRender.Doc:

					// test writing sgml reader using xml document load
					callback = (reader, writer) =>
					{
						var doc = new XmlDocument();
						doc.Load(reader);
						doc.WriteTo(writer);
					};
					break;
				case XmlRender.DocClone:

					// test writing sgml reader using xml document load and clone
					callback = (reader, writer) =>
					{
						var doc = new XmlDocument();
						doc.Load(reader);
						var clone = doc.Clone();
						clone.WriteTo(writer);
					};
					break;
				case XmlRender.Passthrough:

					// test writing sgml reader directly to xml writer
					callback = (reader, writer) =>
					{
						reader.Read();
						while (!reader.EOF)
						{
							writer.WriteNode(reader, true);
						}
					};
					break;
				default:
					throw new ArgumentException("unknown value", "xmlRender");
			}
			actual = RunTest(caseFolding, doctype, format, source, callback);
			Assert.AreEqual(expected, actual);
		}

		private static void ReadTest(string name, out string before, out string after)
		{
			var type = typeof(Tests);
			using (var stream = type.Assembly.GetManifestResourceStream(type.Namespace + ".Resources." + name))
			{
				var sr = new StreamReader(stream);
				var test = sr.ReadToEnd().Split('`');
				before = test[0];
				after = test[1];
			}
		}

		private static string RunTest(CaseFolding caseFolding, string doctype, bool format, string source, XmlReaderTestCallback callback)
		{

			var pseudoBaseUri = new Uri(string.Format(
				"rsrc://{0}/{1}/",
				typeof(Tests).Assembly.FullName.Split(',')[0],
				typeof(Tests).Namespace));

			// initialize sgml reader
			XmlReader reader = new SgmlReader(
				new StringReader(source),
				pseudoBaseUri,
				pseudoUri =>	// Stream opener (Uri --> Stream callback)
				{
					var resourcePath = string.Join(
						".",
						pseudoUri.PathAndQuery.Split(
							new[] { '/' },
							StringSplitOptions.RemoveEmptyEntries));
					return new StreamInformation
						{
							Stream = typeof(Tests).Assembly.GetManifestResourceStream(resourcePath),
							DefaultEncoding = Encoding.UTF8
						};
				})
				{
					CaseFolding = caseFolding,
					DocType = doctype,
					WhitespaceHandling = format == false
				};

			// check if we need to use the LoggingXmlReader
			if (_debug)
			{
				reader = new LoggingXmlReader(reader, Console.Out);
			}

			// initialize xml writer
			var stringWriter = new StringWriter();
			var xmlTextWriter = new XmlTextWriter(stringWriter);
			if (format)
			{
				xmlTextWriter.Formatting = Formatting.Indented;
			}
			callback(reader, xmlTextWriter);
			xmlTextWriter.Close();

			// reproduce the parsed document
			var actual = stringWriter.ToString();

			// ensure that output can be parsed again
			try
			{
				using (var stringReader = new StringReader(actual))
				{
					var doc = new XmlDocument();
					doc.Load(stringReader);
				}
			}
			catch (Exception)
			{
				Assert.Fail("unable to parse sgml reader output:\n{0}", actual);
			}
			return actual.Trim().Replace("\r", "");
		}
	}
}

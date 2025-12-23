using System;
using System.IO;
using System.Text.Json;
using System.Xml;
using System.Xml.Serialization;
using Newtonsoft.Json;
using Xunit;

namespace Peeveen.Test {
	/// <summary>
	/// Extension functions for making JSON or XML serialization tests easier to write.
	/// </summary>
	public static class SerializationTests {
		private static readonly XmlSerializerNamespaces emptyNamespaces = new XmlSerializerNamespaces();
		static SerializationTests() {
			emptyNamespaces.Add(string.Empty, string.Empty);
		}

		/// <summary>
		/// Deserializes the given string, passes the result through a validation function,
		/// then serializes it and deserializes it again, then passes it through the validation
		/// function again. Finally, it serializes the object once again, and compares the
		/// results of both serializations for equality.
		/// </summary>
		/// <typeparam name="T">Type of object to serialize/deserialize.</typeparam>
		/// <param name="serializedObject">The serialized text to test.</param>
		/// <param name="serializerFn">Function to serialize an object.</param>
		/// <param name="deserializerFn">Function to deserialize an obbject.</param>
		/// <param name="compareOriginalString">Whether to compare serialization results
		/// against the original passed-in serialized string.</param>
		/// <param name="testFn">The test function.</param>
		/// <exception cref="Exception">Thrown if the deserialization ever returns null.</exception>
		private static void TestSerialization<T>(string serializedObject, Action<T> testFn, Func<T, string> serializerFn, Func<string, T> deserializerFn, bool compareOriginalString) {
			T DeserializeAndTest(string serObj) {
				var deserItem = deserializerFn(serObj);
				Assert.NotNull(deserItem);
				testFn(deserItem);
				return deserItem;
			}

			var deserializedItem = DeserializeAndTest(serializedObject);
			var serializedAgainObject = serializerFn(deserializedItem);
			if (compareOriginalString)
				Assert.Equal(serializedAgainObject.Trim(), serializedObject.Trim());
			deserializedItem = DeserializeAndTest(serializedAgainObject);
			Assert.NotNull(deserializedItem);
			var serializedOnceAgainObject = serializerFn(deserializedItem);
			Assert.Equal(serializedOnceAgainObject, serializedAgainObject);
		}

		/// <summary>
		/// Deserializes the given XML, passes the result through a validation function,
		/// then serializes it and deserializes it again, and passes it through the
		/// validation function again.
		/// </summary>
		/// <typeparam name="T">Type of object to serialize/deserialize.</typeparam>
		/// <param name="xml">The XML to test.</param>
		/// <param name="testFn">The test function.</param>
		/// <param name="readerSettings">XML reader settings.</param>
		/// <param name="writerSettings">XML writer settings.</param>
		/// <param name="serializerNamespaces">Namespaces to use.</param>
		/// <param name="compareOriginalString">If true, an additional check will be performed
		/// to compare the passed-in serialized data string against the result of the first
		/// serialization. Do not pass true if your original data contains comments, non-uniform
		/// indentation, or any other features that cannot be recreated with the various
		/// "serialization options" that you can provide.</param>
		public static void TestXmlSerialization<T>(string xml, Action<T> testFn, XmlReaderSettings readerSettings = null, XmlWriterSettings writerSettings = null, XmlSerializerNamespaces serializerNamespaces = null, bool compareOriginalString = false) {
			XmlSerializer serializer = new XmlSerializer(typeof(T));
			TestSerialization(xml, testFn,
				(obj) => {
					var stringWriter = new StringWriter();
					var xmlWriter = writerSettings == null ? XmlWriter.Create(stringWriter) : XmlWriter.Create(stringWriter, writerSettings);
					serializer.Serialize(xmlWriter, obj, serializerNamespaces ?? emptyNamespaces);
					return stringWriter.ToString();
				},
				(xmlIn) => {
					var stringReader = new StringReader(xmlIn);
					var xmlReader = readerSettings == null ? XmlReader.Create(stringReader) : XmlReader.Create(stringReader, readerSettings);
					return (T)serializer.Deserialize(xmlReader);
				},
				compareOriginalString
			);
		}

		/// <summary>
		/// Deserializes the given JSON, passes the result through a validation function,
		/// then serializes it and deserializes it again, and passes it through the
		/// validation function again.
		///
		/// Uses Newtonsoft JSON.NET for JSON serialization/deserialization.
		/// </summary>
		/// <typeparam name="T">Type of object to serialize/deserialize.</typeparam>
		/// <param name="json">The JSON to test.</param>
		/// <param name="testFn">The test function.</param>
		/// <param name="formatting">JSON formatting option.</param>
		/// <param name="compareOriginalString">If true, an additional check will be performed
		/// to compare the passed-in serialized data string against the result of the first
		/// serialization. Do not pass true if your original data contains comments, non-uniform
		/// indentation, or any other features that cannot be recreated with the various
		/// "serialization options" that you can provide.</param>
		public static void TestNewtonsoftJsonSerialization<T>(string json, Action<T> testFn, Newtonsoft.Json.Formatting formatting = Newtonsoft.Json.Formatting.None, bool compareOriginalString = false) {
			TestSerialization(json, testFn,
				(obj) => JsonConvert.SerializeObject(obj, formatting),
				(jsonIn) => JsonConvert.DeserializeObject<T>(jsonIn),
				compareOriginalString
			);
		}

		/// <summary>
		/// Deserializes the given JSON, passes the result through a validation function,
		/// then serializes it and deserializes it again, and passes it through the
		/// validation function again.
		///
		/// Uses Newtonsoft JSON.NET for JSON serialization/deserialization.
		/// </summary>
		/// <typeparam name="T">Type of object to serialize/deserialize.</typeparam>
		/// <param name="json">The JSON to test.</param>
		/// <param name="testFn">The test function.</param>
		/// <param name="serializerOptions">Serializer options.</param>
		/// <param name="compareOriginalString">If true, an additional check will be performed
		/// to compare the passed-in serialized data string against the result of the first
		/// serialization. Do not pass true if your original data contains comments, non-uniform
		/// indentation, or any other features that cannot be recreated with the various
		/// "serialization options" that you can provide.</param>
		public static void TestSystemTextJsonSerialization<T>(string json, Action<T> testFn, JsonSerializerOptions serializerOptions = null, bool compareOriginalString = false) {
			TestSerialization(json, testFn,
				(obj) => System.Text.Json.JsonSerializer.Serialize(obj, serializerOptions),
				(jsonIn) => System.Text.Json.JsonSerializer.Deserialize<T>(jsonIn, serializerOptions),
				compareOriginalString
			);
		}
	}
}
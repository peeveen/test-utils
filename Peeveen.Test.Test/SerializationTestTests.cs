using System.Xml;
namespace Peeveen.Test.Test;

public class SerializationTestTests {
	public class TestObject {
		public string StringValue { get; set; } = string.Empty;
		public int IntValue { get; set; }
	}

	[Fact]
	public void TestSerializationTests() {
		var xml = TextFileUtils.ReadFileAsString("data", "testXml.xml");
		SerializationTests.TestXmlSerialization<TestObject>(xml, obj => {
			Assert.Equal("Good", obj.StringValue);
			Assert.Equal(123, obj.IntValue);
		}, writerSettings: new XmlWriterSettings {
			Indent = true,
			IndentChars = "\t"
		});

		var json = TextFileUtils.ReadFileAsString("data", "testJson.json");
		SerializationTests.TestNewtonsoftJsonSerialization<TestObject>(json, obj => {
			Assert.Multiple(() => {
				Assert.Equal("Good", obj.StringValue);
				Assert.Equal(123, obj.IntValue);
			});
		});
		SerializationTests.TestSystemTextJsonSerialization<TestObject>(json, obj => {
			Assert.Multiple(() => {
				Assert.Equal("Good", obj.StringValue);
				Assert.Equal(123, obj.IntValue);
			});
		});
	}
}
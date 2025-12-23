using System.IO;
using System.Threading.Tasks;

namespace Peeveen.Test {
	/// <summary>
	/// Extension functions for making JSON or XML serialization tests easier to write.
	/// </summary>
	public static class TextFileUtils {
		private const int DefaultParentDirDepth = 3;

		/// <summary>
		/// Reads a test text file as one string.
		/// </summary>
		/// <param name="pathParts">Path parts that will be combined.</param>
		/// <param name="parentDirDepth">How many ".." elements to prepend the pathParts with.</param>
		/// <returns></returns>
		public static string ReadFileAsString(int parentDirDepth = 3, params string[] pathParts) => File.ReadAllText(TestUtils.BuildFilePath(parentDirDepth, pathParts));

		/// <summary>
		/// Reads a test text file as one string.
		/// </summary>
		/// <param name="pathParts">Path parts that will be combined. This will be prepended with
		/// "../../.." to cover the "bin/Debug/net6.0" part of the execution path. To use a shallower
		/// or deeper path, use the overloaded method where you can specify the parent dir depth.</param>
		/// <returns></returns>
		public static string ReadFileAsString(params string[] pathParts) => ReadFileAsString(DefaultParentDirDepth, pathParts);

		/// <summary>
		/// Reads a test text file as one string.
		/// </summary>
		/// <param name="pathParts">Path parts that will be combined.</param>
		/// <param name="parentDirDepth">How many ".." elements to prepend the pathParts with.</param>
		/// <returns></returns>
		public static string[] ReadFileAsStrings(int parentDirDepth = 3, params string[] pathParts) => File.ReadAllLines(TestUtils.BuildFilePath(parentDirDepth, pathParts));

		/// <summary>
		/// Reads a test text file as one string.
		/// </summary>
		/// <param name="pathParts">Path parts that will be combined. This will be prepended with
		/// "../../.." to cover the "bin/Debug/net6.0" part of the execution path. To use a shallower
		/// or deeper path, use the overloaded method where you can specify the parent dir depth.</param>
		/// <returns></returns>
		public static string[] ReadFileAsStrings(params string[] pathParts) => ReadFileAsStrings(DefaultParentDirDepth, pathParts);
	}
}
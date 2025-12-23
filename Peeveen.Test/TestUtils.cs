using System.IO;

namespace Peeveen.Test {
	/// <summary>
	/// Useful test functions.
	/// </summary>
	public static class TestUtils {
		internal static string BuildFilePath(int parentDirDepth = 3, params string[] pathParts) {
			string[] parts = new string[parentDirDepth + pathParts.Length];
			for (int f = 0; f < parentDirDepth; ++f)
				parts[f] = "..";
			for (int f = 0; f < pathParts.Length; ++f)
				parts[f + parentDirDepth] = pathParts[f];
			return Path.Combine(parts);
		}

		/// <summary>
		/// Code starts in (e.g.) "bin/Debug/net7.0" ... so need to drop down three folders
		/// to find test files. This returns a path of three ".." strings, joined.
		/// </summary>
		/// <returns>Three .. path components, joined.</returns>
		public static readonly string ProjectFolderPath = BuildFilePath(3);

		/// <summary>
		/// Appends the name of the given test file to ProjectFolderPath.
		/// </summary>
		/// <param name="testFilename">Name of file.</param>
		/// <returns>Path to test file.</returns>
		public static string GetTestFilePath(string testFilename) => Path.Combine(ProjectFolderPath, testFilename);
	}
}
using System.Collections.Generic;
using System.IO;

namespace Inversion.Data {
	/// <summary>
	/// Describes basic functionality for reading resources
	/// external to the application.
	/// </summary>
	public interface IResourceAdapter {
		/// <summary>
		/// Determines whether or not the relative path
		/// specified exists.
		/// </summary>
		/// <param name="path">The relative path to check for.</param>
		/// <returns>
		/// Returns true if the resource exists; otherwise, returns false.
		/// </returns>
		bool Exists(string path);
		/// <summary>
		/// Opens a stream on the resource specified
		/// by the relative path.
		/// </summary>
		/// <param name="path">The relative path to the resource.</param>
		/// <returns>Returns a stream to the specified resource.</returns>
		Stream Open(string path);
		/// <summary>
		/// Opens a binary resources, copies the contents to a byte array
		/// and then closes the resource.
		/// </summary>
		/// <param name="path">The relative path to the resource.</param>
		/// <returns>Returns a byte array of the resources contents.</returns>
		byte[] ReadAllBytes(string path);
		/// <summary>
		/// Reads the lines of the specified resource as an enumerable.
		/// </summary>
		/// <param name="path">The relative path to the resource.</param>
		/// <returns>Returns an enumerable of the resources lines.</returns>
		IEnumerable<string> ReadLines(string path);
		/// <summary>
		/// Reads all the lines the the specified resource into
		/// and array.
		/// </summary>
		/// <param name="path">The relative path to the resource.</param>
		/// <returns>Returns a string array with all the lines of the resource.</returns>
		string[] ReadAllLines(string path);
		/// <summary>
		/// Opens the specified resource, reads its contents, and
		/// then closes the resource.
		/// </summary>
		/// <param name="path">The relative path to the resource.</param>
		/// <returns>Returns the contents of the resource as text.</returns>
		string ReadAllText(string path);
	}
}
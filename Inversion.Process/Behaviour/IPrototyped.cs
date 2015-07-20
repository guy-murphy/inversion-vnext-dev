﻿namespace Inversion.Process.Behaviour {
	/// <summary>
	/// Describes an object that possess a prototype specification.
	/// </summary>
	public interface IPrototyped: IConfigured {
		/// <summary>
		/// Provides access to a prototype specification.
		/// </summary>
		IPrototype Prototype { get; }
	}
}

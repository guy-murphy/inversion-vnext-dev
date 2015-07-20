namespace Inversion.Process {
	/// <summary>
	/// Represents a context with a mutable state model.
	/// </summary>
	/// <typeparam name="TState"></typeparam>
	public interface IStatefulContext<out TState>: ISimpleProcessContext {
		/// <summary>
		/// State model for the context.
		/// </summary>
		TState State { get; }
	}
}
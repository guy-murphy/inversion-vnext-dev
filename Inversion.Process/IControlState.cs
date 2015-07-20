using Inversion.Collections;

namespace Inversion.Process {
	/// <summary>
	/// Implements a common working control state.
	/// </summary>
	public interface IControlState: IDataDictionary<object> {
		/// <summary>
		/// Messages intended for user feedback.
		/// </summary>
		/// <remarks>
		/// This is a poor mechanism for localisation,
		/// and may need to be treated as tokens
		/// by the front end to localise.
		/// </remarks>
		IDataCollection<string> Messages { get; }

		/// <summary>
		/// Error messages intended for user feedback.
		/// </summary>
		/// <remarks>
		/// This is a poor mechanism for localisation,
		/// and may need to be treated as tokens
		/// by the front end to localise.
		/// </remarks>
		IDataCollection<ErrorMessage> Errors { get; }

		/// <summary>
		/// A dictionary of named timers.
		/// </summary>
		/// <remarks>
		/// `ProcessTimer` is only intended
		/// for informal timings, and it not intended
		/// for proper metrics.
		/// </remarks>
		ProcessTimerDictionary Timers { get; }

		/// <summary>
		/// Flags for the context available to behaviours as shared state.
		/// </summary>
		IDataCollection<string> Flags { get; }

		/// <summary>
		/// The parameters of the contexts execution available
		/// to behaviours as shared state.
		/// </summary>
		IDataDictionary<string> Params { get; }
	}
}
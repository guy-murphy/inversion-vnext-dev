using Inversion.Collections;

namespace Inversion.Process {
	/// <summary>
	/// Implements a common working control state.
	/// </summary>
	public class ControlState : DataDictionary<object>, IControlState {
		/// <summary>
		/// Messages intended for user feedback.
		/// </summary>
		/// <remarks>
		/// This is a poor mechanism for localisation,
		/// and may need to be treated as tokens
		/// by the front end to localise.
		/// </remarks>
		public IDataCollection<string> Messages { get; } = new DataCollection<string>();

		/// <summary>
		/// Error messages intended for user feedback.
		/// </summary>
		/// <remarks>
		/// This is a poor mechanism for localisation,
		/// and may need to be treated as tokens
		/// by the front end to localise.
		/// </remarks>
		public IDataCollection<ErrorMessage> Errors { get; } = new DataCollection<ErrorMessage>();

		/// <summary>
		/// A dictionary of named timers.
		/// </summary>
		/// <remarks>
		/// `ProcessTimer` is only intended
		/// for informal timings, and it not intended
		/// for proper metrics.
		/// </remarks>
		public ProcessTimerDictionary Timers { get; } = new ProcessTimerDictionary();

		/// <summary>
		/// Flags for the context available to behaviours as shared state.
		/// </summary>
		public IDataCollection<string> Flags { get; } = new DataCollection<string>();

		/// <summary>
		/// The parameters of the contexts execution available
		/// to behaviours as shared state.
		/// </summary>
		public IDataDictionary<string> Params { get; } = new ConcurrentDataDictionary<string>();
	}
}

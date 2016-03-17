using System.Runtime.Caching;
using Inversion.Collections;

namespace Inversion.Process {
	/// <summary>
	/// Provides a processing context as a self-contained and sufficient
	/// channel of application execution. The context manages a set of
	/// behaviours and mediates between them and the outside world.
	/// </summary>
	/// <remarks>
	/// The process context along with the `IBehaviour` objects registered
	/// on its bus *are* Inversion. Everything else is chosen convention about
	/// how those behaviours interact with each other via the context.
	/// </remarks>
	public interface IProcessContext : IContextFor<IControlState> {

		/// <summary>
		/// Provsion of a simple object cache for the context.
		/// </summary>
		/// <remarks>
		/// This really needs replaced with our own interface
		/// that we control. This isn't portable.
		/// </remarks>
		ObjectCache ObjectCache { get; }

		/// <summary>
		/// Gives access to a collection of view steps
		/// that will be used to control the render
		/// pipeline for this context.
		/// </summary>
		ViewSteps ViewSteps { get; }

		/// <summary>
		/// Fires an event on the context. Each behaviour registered with context
		/// is consulted in no particular order, and for each behaviour that has a condition
		/// that returns true when applied to the event, that behaviours action is executed.
		/// </summary>
		/// <param name="ev">The event to fire on this context.</param>
		/// <returns></returns>
		IEvent Fire(IEvent ev);
		
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
		/// Flags for the context available to behaviours as shared state.
		/// </summary>
		IDataCollection<string> Flags { get; }

		/// <summary>
		/// The parameters of the contexts execution available
		/// to behaviours as shared state.
		/// </summary>
		IDataDictionary<string> Params { get; }

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

	}
}
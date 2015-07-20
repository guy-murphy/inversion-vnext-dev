using System;
using System.Collections.Generic;
using System.Runtime.Caching;

using Inversion.Collections;
using Inversion.Data;
using Inversion.Process.Behaviour;

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
	public interface IProcessContext : IStatefulContext<IControlState> {

		/// <summary>
		/// Exposes resources available to the context.
		/// </summary>
		IResourceAdapter Resources { get; }

		/// <summary>
		/// Provsion of a simple object cache for the context.
		/// </summary>
		/// <remarks>
		/// This really needs replaced with our own interface
		/// that we control. This isn't portable.
		/// </remarks>
		ObjectCache ObjectCache { get; }

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
		/// Gives access to a collection of view steps
		/// that will be used to control the render
		/// pipeline for this context.
		/// </summary>
		ViewSteps ViewSteps { get; }

		/// <summary>
		/// Gives access to the current control state of the context.
		/// This is the common state that behaviours share and that
		/// provides the end state or result of a contexts running process.
		/// </summary>
		IDataDictionary<object> ControlState { get; }

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
		/// Contructs an event with the message specified, using the supplied
		/// parameter keys to copy parameters from the context to the constructed event.
		/// This event is then fired on this context.
		/// </summary>
		/// <param name="message">The message to assign to the event.</param>
		/// <param name="parms">The parameters to copy from the context.</param>
		/// <returns>Returns the event that was constructed and fired on this context.</returns>
		IEvent FireWith(string message, params string[] parms);
	}
}
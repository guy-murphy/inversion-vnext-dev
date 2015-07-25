using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading;

using Inversion.Collections;
using Inversion.Data;

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
	public class ProcessContext : ContextFor<IControlState>, IProcessContext {
		
		/// <summary>
		/// Provsion of a simple object cache for the context.
		/// </summary>
		/// <remarks>
		/// This really needs replaced with our own interface
		/// that we control. This isn't portable.
		/// </remarks>
		public ObjectCache ObjectCache { get; } = MemoryCache.Default;

		/// <summary>
		/// Gives access to a collection of view steps
		/// that will be used to control the render
		/// pipeline for this context.
		/// </summary>
		public ViewSteps ViewSteps { get; } = new ViewSteps();

		/// <summary>
		/// Gives access to the current control state of the context.
		/// This is the common state that behaviours share and that
		/// provides the end state or result of a contexts running process.
		/// </summary>
		public IDataDictionary<object> ControlState => this.State;

		/// <summary>
		/// Messages intended for user feedback.
		/// </summary>
		/// <remarks>
		/// This is a poor mechanism for localisation,
		/// and may need to be treated as tokens
		/// by the front end to localise.
		/// </remarks>
		public IDataCollection<string> Messages => this.State.Messages;

		/// <summary>
		/// Flags for the context available to behaviours as shared state.
		/// </summary>
		public IDataCollection<string> Flags => this.State.Flags;

		/// <summary>
		/// The parameters of the contexts execution available
		/// to behaviours as shared state.
		/// </summary>
		public IDataDictionary<string> Params => this.State.Params;

		/// <summary>
		/// Error messages intended for user feedback.
		/// </summary>
		/// <remarks>
		/// This is a poor mechanism for localisation,
		/// and may need to be treated as tokens
		/// by the front end to localise.
		/// </remarks>
		public IDataCollection<ErrorMessage> Errors => this.State.Errors;

		/// <summary>
		/// A dictionary of named timers.
		/// </summary>
		/// <remarks>
		/// `ProcessTimer` is only intended
		/// for informal timings, and it not intended
		/// for proper metrics.
		/// </remarks>
		public ProcessTimerDictionary Timers => this.State.Timers;

		/// <summary>
		/// Instantiates a new process contrext for inversion.
		/// </summary>
		/// <remarks>You can think of this type here as "being Inversion". This is the thing.</remarks>
		/// <param name="services">The service container the context will use.</param>
		/// <param name="resources">The resources available to the context.</param>
		public ProcessContext(IServiceContainer services, IResourceAdapter resources): base(services, resources, new ControlState()) {}

		/// <summary>
		/// Fires an event on the context. Each behaviour registered with context
		/// is consulted in no particular order, and for each behaviour that has a condition
		/// that returns true when applied to the event, that behaviours action is executed.
		/// </summary>
		/// <param name="ev">The event to fire on this context.</param>
		/// <returns></returns>
		public override IEventFor<IControlState> Fire(IEventFor<IControlState> ev) {
			if (ev.Context != this) throw new ProcessException("The event has a different context that the one on which it has been fired.");
			try {
				this.Bus.OnNext(ev);
			} catch (ThreadAbortException) {
				// we have probably been redirected
			} catch (Exception err) {
				this.Errors.Add(new ErrorMessage($"A problem was encountered firing '{ev.Message}'", err));
			}
			return ev;
		}

		/// <summary>
		/// Fires an event on the context. Each behaviour registered with context
		/// is consulted in no particular order, and for each behaviour that has a condition
		/// that returns true when applied to the event, that behaviours action is executed.
		/// </summary>
		/// <param name="ev">The event to fire on this context.</param>
		/// <returns></returns>
		public IEvent Fire(IEvent ev) {
			return this.Fire(ev as IEventFor<IControlState>) as IEvent;
		}

		/// <summary>
		/// Constructs an event using the message specified, and using the dictionary
		/// provided to populate the parameters of the event. This event is then
		/// fired on this context.
		/// </summary>
		/// <param name="message">The message to assign to the event.</param>
		/// <param name="parms">The parameters to populate the event with.</param>
		/// <returns>Returns the event that was constructed and fired on this context.</returns>
		public override IEventFor<IControlState> Fire(string message, IDictionary<string, string> parms) {
			IEvent ev = new Event(this, message, parms);
			this.Fire(ev);
			return ev;
		}

		/// <summary>
		/// Contructs an event with the message specified, using the supplied
		/// parameter keys to copy parameters from the context to the constructed event.
		/// This event is then fired on this context.
		/// </summary>
		/// <param name="message">The message to assign to the event.</param>
		/// <param name="parms">The parameters to copy from the context.</param>
		/// <returns>Returns the event that was constructed and fired on this context.</returns>
		public IEvent FireWith(string message, params string[] parms) {
			IDictionary<string,string> copy = new Dictionary<string, string>();
			foreach (string parm in parms) {
				if (this.Params.ContainsKey(parm)) {
					copy.Add(parm, this.Params[parm]);
				}
			}
			IEvent ev = new Event(this, message, copy);
			this.Fire(ev);
			return ev;
		}

		/// <summary>
		/// Provides a string representation of the context and its current state.
		/// </summary>
		/// <returns>Returns a string representation of the context.</returns>
		public override string ToString() {
			return this.State.ToJson();
		}
	}
}

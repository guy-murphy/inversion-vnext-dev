using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Inversion.Process {

	/// <summary>
	/// Represents an event occuring in the system.
	/// </summary>
	/// <remarks>
	/// Exactly what "event" means is application specific
	/// and can range from imperative to reactive.
	/// </remarks>
	public class Event : EventFor<IControlState>, IEvent {
		
		/// <summary>
		/// The context upon which this event is being fired.
		/// </summary>
		/// <remarks>
		/// And event always belongs to a context.
		/// </remarks>
		public new IProcessContext Context => base.Context as IProcessContext;

		IContextFor<IControlState> IEventFor<IControlState>.Context => this.Context;
		
		/// <summary>
		/// Instantiates a new event bound  to a context.
		/// </summary>
		/// <param name="context">The context to which the event is bound.</param>
		/// <param name="message">The simple message the event represents.</param>
		/// <param name="parameters">The parameters of the event.</param>
		public Event(IProcessContext context, string message, IDictionary<string, string> parameters) : this(context, message, null, parameters) { }

		/// <summary>
		/// Instantiates a new event bound  to a context.
		/// </summary>
		/// <param name="context">The context to which the event is bound.</param>
		/// <param name="message">The simple message the event represents.</param>
		/// <param name="obj">An object being carried by the event.</param>
		/// <param name="parameters">The parameters of the event.</param>
		public Event(IProcessContext context, string message, IData obj, IDictionary<string, string> parameters): base(context, message, obj, parameters) { }

		/// <summary>
		/// Instantiates a new event bound  to a context.
		/// </summary>
		/// <param name="context">The context to which the event is bound.</param>
		/// <param name="message">The simple message the event represents.</param>
		public Event(IProcessContext context, string message) : this(context, message, null) {}

		/// <summary>
		/// Instantiates a new event as a copy of the event provided.
		/// </summary>
		/// <param name="ev">The event to copy for this new instance.</param>
		public Event(IEvent ev): base(ev) { }

		/// <summary>
		/// Fires the event on the context to which it is bound.
		/// </summary>
		/// <returns>
		/// Returns the event that has just been fired.
		/// </returns>
		public new IEvent Fire() {
			return this.Context.Fire(this);
		}

	}
}

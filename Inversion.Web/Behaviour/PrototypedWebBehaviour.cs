using System;
using System.Collections.Generic;

using Inversion.Process;
using Inversion.Process.Behaviour;

namespace Inversion.Web.Behaviour {
	/// <summary>
	/// A web behaviour that can be configured with a prototype specification
	/// of selection criteria used to drive the behaviours condition.
	/// </summary>
	public abstract class PrototypedWebBehaviour: PrototypedBehaviour, IWebBehaviour {
		/// <summary>
		/// Creates a new instance of the behaviour.
		/// </summary>
		/// <param name="respondsTo">The message the behaviour responds to.</param>
		protected PrototypedWebBehaviour(string respondsTo) : this(respondsTo, new WebPrototype()) {}

		/// <summary>
		/// Creates a new instance of the behaviour.
		/// </summary>
		/// <param name="respondsTo">The message the behaviour responds to.</param>
		/// <param name="prototype">The prototype to use in configuring this behaviour.</param>
		protected PrototypedWebBehaviour(string respondsTo, IPrototype prototype) : base(respondsTo, prototype) { }

		/// <summary>
		/// Creates a new instance of the behaviour.
		/// </summary>
		/// <param name="respondsTo">The message the behaviour responds to.</param>
		/// <param name="config">The configuration elements to use in configuring this behaviour.</param>
		protected PrototypedWebBehaviour(string respondsTo, IEnumerable<IConfigurationElement> config): base(respondsTo, new WebPrototype(config)) {
			
		}

		/// <summary>
		/// Determines if this behaviours action should be executed in
		/// response to the provided event.
		/// </summary>
		/// <param name="ev">The event to consider.</param>
		/// <returns>Returns true if this behaviours action to execute in response to this event; otherwise returns  false.</returns>
		public override bool Condition(IEvent ev) {
			return this.Condition(ev, (IWebContext)ev.Context);
		}

		/// <summary>
		/// Determines if each of the behaviours selection criteria match.
		/// </summary>
		/// <param name="ev">The event to consult.</param>
		/// <param name="context">The context to consult.</param>
		/// <returns>
		/// Returns true if the selection criteria for this behaviour each return true.
		///  </returns>
		public virtual bool Condition(IEvent ev, IWebContext context) {
			return base.Condition(ev);
		}

		/// <summary>
		/// The action to perform if this behaviours condition is met.
		/// </summary>
		/// <param name="ev">The event to consult.</param>
		public override void Action(IEvent ev) {
			this.Action(ev, (IWebContext)ev.Context);
		}
		
		/// <summary>
		/// The action to perform if this behaviours condition is met.
		/// </summary>
		/// <param name="ev">The event to consult.</param>
		/// <param name="context">The context upon which to perform any action.</param>
		public abstract void Action(IEvent ev, IWebContext context);

		/// <summary>
		/// Provide recovery from failures.
		/// </summary>
		/// <param name="ev">The event to process.</param>
		/// <param name="err">The exception raised by the behaviours actions.</param>
		public override void Rescue (IEvent ev, Exception err) {
			this.Rescue(ev, (IWebContext)ev.Context, err);
		}

		/// <summary>
		/// Provide recovery from failures.
		/// </summary>
		/// <param name="ev">The event to process.</param>
		/// <param name="ctx">The context to consider.</param>
		/// <param name="err">The exception raised by the behaviours actions.</param>
		public virtual void Rescue (IEvent ev, IWebContext ctx, Exception err) {
			base.Rescue(ev, err);
		}
	}
}

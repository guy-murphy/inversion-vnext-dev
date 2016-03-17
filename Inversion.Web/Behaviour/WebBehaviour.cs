using System;
using Inversion.Process;
using Inversion.Process.Behaviour;

namespace Inversion.Web.Behaviour {
	/// <summary>
	/// An abstract provision of basic web-centric features for process behaviours
	/// being used in a web application.
	/// </summary>
	public abstract class WebBehaviour : ProcessBehaviour, IWebBehaviour {
		/// <summary>
		/// Creates a new instance of the behaviour.
		/// </summary>
		/// <param name="respondsTo">The message the behaviour responds to.</param>
		protected WebBehaviour(string respondsTo) : base(respondsTo) {}

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
		/// Determines if this behaviours action should be executed in
		/// response to the provided event and context.
		/// </summary>
		/// <param name="ev">The event to consider.</param>
		/// <param name="ctx">The context to consider.</param>
		/// <returns></returns>
		public virtual bool Condition(IEvent ev, IWebContext ctx) {
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
		/// Implementors should impliment this behaviour with the desired action
		/// for their behaviour.
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
			this.Rescue(ev, (IWebContext) ev.Context, err);
		}

		/// <summary>
		/// Provide recovery from failures.
		/// </summary>
		/// <param name="ev">The event to process.</param>
		/// <param name="ctx">The context to consider.</param>
		/// <param name="err">The exception raised by the behaviours actions.</param>
		public virtual void Rescue(IEvent ev, IWebContext ctx, Exception err) {
			base.Rescue(ev, err);
		}

	}
}

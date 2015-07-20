using System;

namespace Inversion.Process.Behaviour {

	/// <summary>
	/// A simple named behaviour with a default condition
	/// matching that name againts <see cref="IEvent.Message"/>.
	/// </summary>
	public abstract class ProcessBehaviour : SimpleProcessBehaviour, IProcessBehaviour {

		
		/// <summary>
		/// Creates a new instance of the behaviour.
		/// </summary>
		/// <param name="respondsTo">The message the behaviour responds to.</param>
		protected ProcessBehaviour(string respondsTo): base(respondsTo) {
		}

		/// <summary>
		/// Determines if the event specifies the behaviour by name.
		/// </summary>
		/// <param name="ev">The event to consult.</param>
		/// <returns>
		/// Returns true if true if `ev.Message` is the same as `this.Message`
		///  </returns>
		/// <remarks>
		/// The intent is to override for bespoke conditions.
		/// </remarks>
		public override bool Condition(IEvent ev) {
			return this.Condition(ev, (IProcessContext)ev.Context);
		}

		/// <summary>
		/// Determines if the event specifies the behaviour by name.
		/// </summary>
		/// <param name="ev">The event to consult.</param>
		/// <param name="context">The context to consult.</param>
		/// <returns>
		/// Returns true if true if `ev.Message` is the same as `this.Message`
		///  </returns>
		/// <remarks>
		/// The intent is to override for bespoke conditions.
		/// </remarks>
		public virtual bool Condition(IEvent ev, IProcessContext context) {
			// check the base condition
			// and then either there are no roles specified
			// or the user is in any of the roles defined
			return this.RespondsTo == "*" || ev.Message == this.RespondsTo;
		}
		
		public override void Action(IEvent ev, ISimpleProcessContext context) {
			this.Action(ev, (IProcessContext)context);
		}

		/// <summary>
		/// The action to perform when the `Condition(IEvent)` is met.
		/// </summary>
		/// <param name="ev">The event to consult.</param>
		/// <param name="context">The context upon which to perform any action.</param>
		public abstract void Action(IEvent ev, IProcessContext context);
		
		public override void Rescue(IEvent ev, Exception err, ISimpleProcessContext context) {
			this.Rescue(ev, err, (IProcessContext)context);
		}

		/// <summary>
		/// Provide recovery from failures.
		/// </summary>
		/// <param name="ev">The event to process.</param>
		/// <param name="err">The exception raised by the behaviours actions.</param>
		/// <param name="context">The context to consult.</param>
		public virtual void Rescue(IEvent ev, Exception err, IProcessContext context) {
			context.State.Errors.Add(new ErrorMessage(err.Message, err));
		}

	}
}

using System;

namespace Inversion.Process.Behaviour {
	/// <summary>
	/// Provides a base class for implementing behaviours.
	/// </summary>
	/// <typeparam name="TEvent">Type of event which the behaviour will consume.</typeparam>
	/// <typeparam name="TContext">Type of context which the behaviour will use.</typeparam>
	public abstract class BehaviourFor<TEvent, TContext>: IBehaviourFor<TEvent, TContext> where TEvent: IEventFor<TContext> {

		/// <summary>
		/// The name the behaviour is known by to the system.
		/// </summary>
		public string RespondsTo { get; }

		/// <summary>
		/// Creates a new instance of the behaviour.
		/// </summary>
		/// <param name="respondsTo">The message the behaviour responds to.</param>
		protected BehaviourFor(string respondsTo) {
			this.RespondsTo = respondsTo;
		}

		/// <summary>
		/// Determines if the event specifies the behaviour by name.
		/// </summary>
		/// <param name="ev">The event to consult.</param>
		/// <returns>
		/// Returns true if the condition is met; otherwise, returns false.
		/// </returns>
		/// <remarks>
		/// The intent is to override for bespoke conditions.
		/// </remarks>
		public virtual bool Condition(TEvent ev) {
			return this.Condition(ev, ev.Context);
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
		public virtual bool Condition(TEvent ev, TContext context) {
			// check the base condition
			// and then either there are no roles specified
			// or the user is in any of the roles defined
			return this.RespondsTo == "*" || ev.Message == this.RespondsTo;
		}
		
		/// <summary>
		/// The action to perform when the `Condition(IEvent)` is met.
		/// </summary>
		/// <param name="ev">The event to consult.</param>
		public virtual void Action(TEvent ev) {
			this.Action(ev, ev.Context);
		}

		/// <summary>
		/// The action to perform when the `Condition(IEvent)` is met.
		/// </summary>
		/// <param name="ev">The event to consult.</param>
		/// <param name="context">The context upon which to perform any action.</param>
		public abstract void Action(TEvent ev, TContext context);

		/// <summary>
		/// Provide recovery from failures.
		/// </summary>
		/// <param name="ev">The event to process.</param>
		/// <param name="err">The exception raised by the behaviours actions.</param>
		public virtual void Rescue(TEvent ev, Exception err) {
			this.Rescue(ev, err, ev.Context);
		}

		/// <summary>
		/// Provide recovery from failures.
		/// </summary>
		/// <param name="ev">The event to process.</param>
		/// <param name="err">The exception raised by the behaviours actions.</param>
		/// <param name="context">The context upon which to perform any action.</param>
		public abstract void Rescue(TEvent ev, Exception err, TContext context);

	}
}

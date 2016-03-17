using System;

namespace Inversion.Process.Behaviour {
	/// <summary>
	/// Provides a base class for implementing behaviours.
	/// </summary>
	/// <typeparam name="TState">The type of context state.</typeparam>
	public abstract class BehaviourFor<TState>: IBehaviourFor<TState> {

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
		public virtual bool Condition(IEventFor<TState> ev) {
			// check the base condition
			// and then either there are no roles specified
			// or the user is in any of the roles defined
			return this.RespondsTo == "*" || ev.Message == this.RespondsTo;
		}

		/// <summary>
		/// The action to perform when the `Condition(IEvent)` is met.
		/// </summary>
		/// <param name="ev">The event to consult.</param>
		public abstract void Action(IEventFor<TState> ev);
		
		/// <summary>
		/// Provide recovery from failures.
		/// </summary>
		/// <param name="ev">The event to process.</param>
		/// <param name="err">The exception raised by the behaviours actions.</param>
		public abstract void Rescue(IEventFor<TState> ev, Exception err);
		
	}
}

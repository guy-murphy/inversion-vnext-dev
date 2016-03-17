using System;

namespace Inversion.Process.Behaviour {
	/// <summary>
	/// Describes a basic behaviour.
	/// </summary>
	/// <typeparam name="TState">The type of context state.</typeparam>
	public interface IBehaviourFor<TState> {
		/// <summary>
		/// Gets the message that the behaviour will respond to.
		/// </summary>
		/// <value>A `string` value.</value>
		string RespondsTo { get; }

		/// <summary>
		/// Process an action for the provided <see cref="IEvent"/>.
		/// </summary>
		/// <param name="ev">The event to be processed. </param>
		void Action(IEventFor<TState> ev);

		/// <summary>
		/// Provide recovery from failures.
		/// </summary>
		/// <param name="ev">The event to process.</param>
		/// <param name="err">The exception raised by the behaviours actions.</param>
		void Rescue(IEventFor<TState> ev, Exception err);

		/// <summary>
		/// The considtion that determines whether of not the behaviours action
		/// is valid to run.
		/// </summary>
		/// <param name="ev">The event to consider with the condition.</param>
		/// <returns>
		/// `true` if the condition is met; otherwise,  returns  `false`.
		/// </returns>
		bool Condition(IEventFor<TState> ev);
		
	}
}

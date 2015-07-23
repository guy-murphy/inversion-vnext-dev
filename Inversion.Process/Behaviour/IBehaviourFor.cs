using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inversion.Process.Behaviour {
	/// <summary>
	/// Describes a basic behaviour.
	/// </summary>
	public interface IBehaviourFor<in TEvent, in TContext> {
		/// <summary>
		/// Gets the message that the behaviour will respond to.
		/// </summary>
		/// <value>A `string` value.</value>
		string RespondsTo { get; }

		/// <summary>
		/// Process an action for the provided <see cref="IEvent"/>.
		/// </summary>
		/// <param name="ev">The event to be processed. </param>
		void Action(TEvent ev);
		
		/// <summary>
		/// Process the action in response to the provided <see cref="IEvent"/>
		/// with the <see cref="IProcessContext"/> provided.
		/// </summary>
		/// <param name="ev">The event to process.</param>
		/// <param name="context">The context to use.</param>
		void Action(TEvent ev, TContext context);

		/// <summary>
		/// Provide recovery from failures.
		/// </summary>
		/// <param name="ev">The event to process.</param>
		/// <param name="err">The exception raised by the behaviours actions.</param>
		void Rescue(TEvent ev, Exception err);

		/// <summary>
		/// Provide recovery from failures.
		/// </summary>
		/// <param name="ev">The event to process.</param>
		/// <param name="err">The exception raised by the behaviours actions.</param>
		/// <param name="context">The context to use.</param>
		void Rescue(TEvent ev, Exception err, TContext context);

		/// <summary>
		/// The considtion that determines whether of not the behaviours action
		/// is valid to run.
		/// </summary>
		/// <param name="ev">The event to consider with the condition.</param>
		/// <returns>
		/// `true` if the condition is met; otherwise,  returns  `false`.
		/// </returns>
		bool Condition(TEvent ev);

		/// <summary>
		/// The considtion that determines whether of not the behaviours action
		/// is valid to run.
		/// </summary>
		/// <param name="ev">The event to consider with the condition.</param>
		/// <param name="context">The context to use.</param>
		/// <returns>
		/// `true` if the condition is met; otherwise,  returns  `false`.
		/// </returns>
		bool Condition(TEvent ev, TContext context);
	}
}

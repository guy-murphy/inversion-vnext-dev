using System;
using System.Collections.Generic;
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
	/// <typeparam name="TState">The type of context state.</typeparam>
	public interface IContextFor<TState>: IDisposable {

		/// <summary>
		/// Exposes the processes service container.
		/// </summary>
		IServiceContainer Services { get; }

		/// <summary>
		/// Exposes resources available to the context.
		/// </summary>
		IResourceAdapter Resources { get; }

		/// <summary>
		/// State model for the context.
		/// </summary>
		TState State { get; }

		/// <summary>
		/// Registers a behaviour with the context ensuring
		/// it is consulted for each event fired on this context.
		/// </summary>
		/// <param name="behaviour">The behaviour to register with this context.</param>
		void Register(IBehaviourFor<TState> behaviour);

		/// <summary>
		/// Registers a whole bunch of behaviours with this context ensuring
		/// each one is consulted when an event is fired on this context.
		/// </summary>
		/// <param name="behaviours">The behaviours to register with this context.</param>
		void Register(IEnumerable<IBehaviourFor<TState>> behaviours);

		/// <summary>
		/// Creates and registers a runtime behaviour with this context constructed 
		/// from a predicate representing the behaviours condition, and an action
		/// representing the behaviours action. This behaviour will be consulted for
		/// any event fired on this context.
		/// </summary>
		/// <param name="condition">The predicate to use as the behaviours condition.</param>
		/// <param name="action">The action to use as the behaviours action.</param>
		void Register(Predicate<IEventFor<TState>> condition, Action<IEventFor<TState>, IContextFor<TState>> action);

		/// <summary>
		/// Fires an event on the context. Each behaviour registered with context
		/// is consulted in no particular order, and for each behaviour that has a condition
		/// that returns true when applied to the event, that behaviours action is executed.
		/// </summary>
		/// <param name="ev">The event to fire on this context.</param>
		/// <returns></returns>
		IEventFor<TState> Fire(IEventFor<TState> ev);

		/// <summary>
		/// Constructs a simple event, with a simple string message
		/// and fires it on this context.
		/// </summary>
		/// <param name="message">The message to assign to the event.</param>
		/// <returns>Returns the event that was constructed and fired on this context.</returns>
		IEventFor<TState> Fire(string message);

		/// <summary>
		/// Constructs an event using the message specified, and using the dictionary
		/// provided to populate the parameters of the event. This event is then
		/// fired on this context.
		/// </summary>
		/// <param name="message">The message to assign to the event.</param>
		/// <param name="parms">The parameters to populate the event with.</param>
		/// <returns>Returns the event that was constructed and fired on this context.</returns>
		IEventFor<TState> Fire(string message, IDictionary<string, string> parms);

		/// <summary>
		/// Instructs the context that operations have finished, and that while it
		/// may still be consulted no further events will be fired.
		/// </summary>
		void Completed();

	}
}

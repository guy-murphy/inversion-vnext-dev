using System;

namespace Inversion.Process.Behaviour {

	/// <summary>
	/// A behaviour that facilitates creating behaviours whose conditions and actions
	/// are assigned at runtime not compile time.
	/// </summary>
	public class RuntimeBehaviourFor<TState> : BehaviourFor<TState> {

		private readonly Predicate<IEventFor<TState>> _condition;
		private readonly Action<IEventFor<TState>> _action;
		private readonly Action<IEventFor<TState>, Exception> _rescue;

		/// <summary>
		/// Instantiates a new runtime behaviour.
		/// </summary>
		/// <param name="respondsTo">The name by which the behaviour is known to the system.</param>
		protected RuntimeBehaviourFor(string respondsTo) : base(respondsTo) { }

		/// <summary>
		/// Instantiates a new runtime behaviour.
		/// </summary>
		/// <param name="respondsTo">The name by which the behaviour is known to the system.</param>
		/// <param name="condition">The predicate that will determine if this behaviours action should be executed.</param>
		/// <param name="action">The action that should be performed if this behaviours conditions are met.</param>
		/// <param name="rescue">The action that should be performed to recover from failure.</param>
		public RuntimeBehaviourFor (string respondsTo, Predicate<IEventFor<TState>> condition, Action<IEventFor<TState>> action, Action<IEventFor<TState>, Exception> rescue)
			: base(respondsTo) {
			_condition = condition;
			_action = action;
			_rescue = rescue;
		}

		/// <summary>
		/// Determines if this behaviours action should be executed in
		/// response to the provided event.
		/// </summary>
		/// <param name="ev">The event to consider.</param>
		/// <returns>Returns true if this behaviours action to execute in response to this event; otherwise returns  false.</returns>
		public override bool Condition(IEventFor<TState> ev) {
			return _condition(ev);
		}

		/// <summary>
		/// The action to perform if this behaviours condition is met.
		/// </summary>
		/// <param name="ev">The event to consult.</param>
		public override void Action(IEventFor<TState> ev) {
			_action(ev);
		}

		/// <summary>
		/// Provide recovery from failures.
		/// </summary>
		/// <param name="ev">The event to process.</param>
		/// <param name="err">The exception raised by the behaviours actions.</param>
		public override void Rescue (IEventFor<TState> ev, Exception err) {
			_rescue(ev, err);
		}
	}
}

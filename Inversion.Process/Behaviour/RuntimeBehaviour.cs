using System;

namespace Inversion.Process.Behaviour {

	/// <summary>
	/// A behaviour that facilitates creating behaviours whose conditions and actions
	/// are assigned at runtime not compile time.
	/// </summary>
	public class RuntimeBehaviourFor<TState> : BehaviourFor<TState> {

		private readonly Predicate<IEventFor<TState>> _condition;
		private readonly Action<IEventFor<TState>, IContextFor<TState>> _action;

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
		public RuntimeBehaviourFor(string respondsTo, Predicate<IEventFor<TState>> condition, Action<IEventFor<TState>, IContextFor<TState>> action)
			: base(respondsTo) {
			_condition = condition;
			_action = action;
		}

		public override void Rescue(IEventFor<TState> ev, Exception err, IContextFor<TState> context) {
		}

		public override bool Condition(IEventFor<TState> ev) {
			return _condition(ev);
		}

		public override void Action(IEventFor<TState> ev, IContextFor<TState> context) {
			_action(ev, context);
		}
	}
}

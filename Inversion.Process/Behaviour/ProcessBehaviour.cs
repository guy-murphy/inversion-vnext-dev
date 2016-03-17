using System;

namespace Inversion.Process.Behaviour {

	/// <summary>
	/// A simple named behaviour.
	/// </summary>
	public abstract class ProcessBehaviour : BehaviourFor<IControlState>, IProcessBehaviour {

		/// <summary>
		/// Creates a new instance of the behaviour.
		/// </summary>
		/// <param name="respondsTo">The message the behaviour responds to.</param>
		protected ProcessBehaviour (string respondsTo) : base(respondsTo) { }

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
		public override bool Condition(IEventFor<IControlState> ev) {
			return this.Condition((IEvent)ev);
		}

		/// <summary>
		/// The considtion that determines whether of not the behaviours action
		/// is valid to run.
		/// </summary>
		/// <param name="ev">The event to consider with the condition.</param>
		/// <returns>
		/// `true` if the condition is met; otherwise,  returns  `false`.
		/// </returns>
		public virtual bool Condition (IEvent ev) {
			return base.Condition(ev);
		}

		/// <summary>
		/// Process an action for the provided <see cref="IEventFor{IControlState}"/>.
		/// </summary>
		/// <param name="ev">The event to be processed. </param>
		public override void Action (IEventFor<IControlState> ev) {
			this.Action((IEvent)ev);
		}

		/// <summary>
		/// Process an action for the provided <see cref="IEvent"/>.
		/// </summary>
		/// <param name="ev">The event to be processed. </param>
		public abstract void Action(IEvent ev);

		/// <summary>
		/// Provide recovery from failures.
		/// </summary>
		/// <param name="ev">The event to process.</param>
		/// <param name="err">The exception raised by the behaviours actions.</param>
		public override void Rescue (IEventFor<IControlState> ev, Exception err) {
			this.Rescue((IEvent)ev, err);
		}

		/// <summary>
		/// Provide recovery from failures.
		/// </summary>
		/// <param name="ev">The event to process.</param>
		/// <param name="err">The exception raised by the behaviours actions.</param>
		/// <remarks>
		/// By default we add a new <see cref="ErrorMessage"/> to the contexts errors collection.
		/// </remarks>
		public virtual void Rescue(IEvent ev, Exception err) {
			ev.Context.Errors.Add(new ErrorMessage(err.Message, err));
		}
	}
}

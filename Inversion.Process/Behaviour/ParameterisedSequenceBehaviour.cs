using System.Collections.Generic;

namespace Inversion.Process.Behaviour {
	/// <summary>
	/// A behaviour concerned with driving the processing of a
	/// sequence of messages.
	/// </summary>
	public class ParameterisedSequenceBehaviour : PrototypedBehaviour {
		/// <summary>
		/// Creates a new instance of the behaviour.
		/// </summary>
		/// <param name="respondsTo">The message the behaviour will respond to.</param>
		/// <param name="config">Configuration for the behaviour.</param>
		public ParameterisedSequenceBehaviour(string respondsTo, IConfiguration config) : base(respondsTo, config.Elements) {}

		/// <summary>
		/// Creates a new instance of the behaviour.
		/// </summary>
		/// <param name="respondsTo">The message the behaviour will respond to.</param>
		/// <param name="config">Configuration for the behaviour.</param>
		public ParameterisedSequenceBehaviour(string respondsTo, IEnumerable<IConfigurationElement> config) : base(respondsTo, config) {}

		/// <summary>
		/// The action to perform when the `Condition(IEvent)` is met.
		/// </summary>
		/// <param name="ev">The event to consult.</param>
		public override void Action(IEvent ev) {
			this.FireSequenceFromConfig(ev.Context, "fire");
		}
	}
}

using System;
using System.Collections.Generic;
using Inversion.Process.Behaviour;

namespace Inversion.Process.Tests.Behaviour {
	public class TestBehaviour: PrototypedBehaviour {

		private readonly Action<IEvent> _action;

		public TestBehaviour(string respondsTo) : base(respondsTo) {}

		public TestBehaviour(string respondsTo, IEnumerable<IConfigurationElement> config) : this(respondsTo, config, null) { }

		public TestBehaviour(string respondsTo, Action<IEvent> action) : base(respondsTo) {
			_action = action;
		}

		public TestBehaviour(string respondsTo, IEnumerable<IConfigurationElement> config, Action<IEvent> perform) 
			: base(respondsTo, config) {
				_action = perform;
		}

		public override void Action(IEvent ev) {
			_action?.Invoke(ev);
		}
	}
}

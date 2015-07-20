﻿using System.Collections.Generic;
using System.Linq;

namespace Inversion.Process.Behaviour {
	/// <summary>
	/// A behaviour that can be configured with a prototype specification
	/// of selection criteria used to drive the behaviours condition.
	/// </summary>
	public abstract class PrototypedBehaviour: ProcessBehaviour, IPrototypedBehaviour {

		private readonly IPrototype _prototype;

		/// <summary>
		/// Provides access to a prototype specification.
		/// </summary>
		public IPrototype Prototype {
			get { return _prototype; }
		}
		/// <summary>
		/// Provices access to component configuration stuiable for querying.
		/// </summary>
		public IConfiguration Configuration {
			get { return _prototype; }
		}

		/// <summary>
		/// Creates a new instance of the behaviour.
		/// </summary>
		/// <param name="respondsTo">The message the behaviour responds to.</param>
		protected PrototypedBehaviour(string respondsTo) : this(respondsTo, new Prototype()) {}
		/// <summary>
		/// Creates a new instance of the behaviour.
		/// </summary>
		/// <param name="respondsTo">The message the behaviour responds to.</param>
		/// <param name="prototype">The prototype to use in configuring this behaviour.</param>
		protected PrototypedBehaviour(string respondsTo, IPrototype prototype): base(respondsTo) {
			_prototype = prototype;
		}
		/// <summary>
		/// Creates a new instance of the behaviour.
		/// </summary>
		/// <param name="respondsTo">The message the behaviour responds to.</param>
		/// <param name="config">The configuration elements to use in configuring this behaviour.</param>
		protected PrototypedBehaviour(string respondsTo, IEnumerable<IConfigurationElement> config): base(respondsTo) {
			_prototype = new Prototype(config);
		}
		
		/// <summary>
		/// Determines if each of the behaviours selection criteria match.
		/// </summary>
		/// <param name="ev">The event to consult.</param>
		/// <param name="context">The context to consult.</param>
		/// <returns>
		/// Returns true if the selection criteria for this behaviour each return true.
		///  </returns>
		public override bool Condition(IEvent ev, IProcessContext context) {
			return base.Condition(ev, context) && this.Prototype.Criteria.All(criteria => criteria(this.Configuration, ev));
		}

	}
}

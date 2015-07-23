using System.Collections.Generic;

namespace Inversion.Process {

	/// <summary>
	/// Represents an event occuring in the system.
	/// </summary>
	/// <remarks>
	/// Exactly what "event" means is application specific
	/// and can range from imperative to reactive.
	/// </remarks>
	public interface IEvent : IEventFor<IProcessContext> {}
}

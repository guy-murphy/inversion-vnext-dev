using System;

namespace Inversion.Process.Behaviour {

	/// <summary>
	/// The base type for behaviours in Conclave. Behaviours are intended
	/// to be registered against a context such as <see cref="ProcessContext"/>
	/// using `ProcessContext.Register(behaviour)`.
	/// </summary>
	/// <remarks>
	/// <para>
	/// When events are fired
	/// against that context, each behaviour registered will apply it's condition
	/// to the <see cref="IEvent"/> being fired. If this condition returns `true`,
	/// then the context will apply the behaviours `Action` against
	/// the event.
	/// </para>
	/// <para>
	/// Care should be taken to ensure behaviours are well behaved. To this
	/// end the following contract is implied by use of `IProcessBehaviour`:-
	/// </para>
	/// </remarks>
	/// <example>
	/// <code> <![CDATA[
	///		context.Register(behaviours);
	///		context.Fire("set-up");
	///		context.Fire("process-request");	
	///		context.Fire("tear-down");
	///		context.Completed();
	///		context.Response.ContentType = "text/xml";
	///		context.Response.Write(context.ControlState.ToXml());
	/// ]]> </code>
	/// </example>
	public interface IProcessBehaviour: IBehaviourFor<IControlState> {
		void Action(IEvent ev);
	}
}

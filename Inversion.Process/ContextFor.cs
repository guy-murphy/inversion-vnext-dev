﻿using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Reactive.Subjects;
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
	public abstract class ContextFor<TState> :  IContextFor<TState> {

		private bool _isDisposed;

		/// <summary>
		/// The event bus of the process.
		/// </summary>
		protected ISubject<IEventFor<TState>> Bus { get; } = new Subject<IEventFor<TState>>();
		/// <summary>
		/// Exposes the processes service container.
		/// </summary>
		public IServiceContainer Services { get; }

		/// <summary>
		/// Exposes resources external to the process.
		/// </summary>
		public IResourceAdapter Resources { get; }

		/// <summary>
		/// Models the mutable state of the context.
		/// </summary>
		public TState State { get; }

		/// <summary>
		/// Instantiates a new simple process contrext for inversion.
		/// </summary>
		/// <remarks>You can think of this type here as "being Inversion". This is the thing.</remarks>
		/// <param name="services">The service container the context will use.</param>
		/// <param name="resources">The resources available to the context.</param>
		/// <param name="state">The mutable state of the context.</param>
		protected ContextFor(IServiceContainer services, IResourceAdapter resources, TState state) {
			this.Services = services;
			this.Resources = resources;
			this.State = state;
		}

		/// <summary>
		/// Desrtructor for the type.
		/// </summary>
		~ContextFor() {
			// ensure unmanaged resources are cleaned up
			// this might all be a bit conceipted, I'm not sure
			// we've run into a real use-case requiring this since day one.
			this.Dispose(false);
		}

		/// <summary>
		/// Releases all resources maintained by the current context instance.
		/// </summary>
		public void Dispose() {
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		/// <summary>
		/// Disposal that allows for partitioning of 
		/// clean-up of managed and unmanaged resources.
		/// </summary>
		/// <param name="disposing"></param>
		/// <remarks>
		/// This is looking conceited and should probably be removed.
		/// I'm not even sure I can explain a use case for it in terms
		/// of an Inversion context.
		/// </remarks>
		protected virtual void Dispose(bool disposing) {
			if (!_isDisposed) {
				if (disposing) {
					// managed resource clean-up
					
				}
				// unmanaged resource clean-up
				// ... nothing to do
				// call dispose on base class, and clear data
				// base.Dispose(disposing);
				// mark disposing as done
				_isDisposed = true;
			}
		}

		/// <summary>
		/// Registers a behaviour with the context ensuring
		/// it is consulted for each event fired on this context.
		/// </summary>
		/// <param name="behaviour">The behaviour to register with this context.</param>
		public void Register(IBehaviourFor<TState> behaviour) {
			this.Bus.Where(ev => behaviour.Condition(ev)).Subscribe(
				(IEventFor<TState> ev) => {
					try {
						behaviour.Action(ev);
					} catch (Exception err) {
						behaviour.Rescue(ev, err);
					}
				}
			);
		}

		/// <summary>
		/// Registers a whole bunch of behaviours with this context ensuring
		/// each one is consulted when an event is fired on this context.
		/// </summary>
		/// <param name="behaviours">The behaviours to register with this context.</param>
		public void Register(IEnumerable<IBehaviourFor<TState>> behaviours) {
			foreach (IBehaviourFor<TState> behaviour in behaviours) {
				this.Register(behaviour);
			}
		}

		/// <summary>
		/// Creates and registers a runtime behaviour with this context constructed 
		/// from a predicate representing the behaviours condition, and an action
		/// representing the behaviours action. This behaviour will be consulted for
		/// any event fired on this context.
		/// </summary>
		/// <param name="condition">The predicate to use as the behaviours condition.</param>
		/// <param name="action">The action to use as the behaviours action.</param>
		public void Register (Predicate<IEventFor<TState>> condition, Action<IEventFor<TState>> action) {
			this.Register(new RuntimeBehaviourFor<TState>(String.Empty, condition, action, (ev,err) => {/*nop*/}));
		}

		/// <summary>
		/// Creates and registers a runtime behaviour with this context constructed 
		/// from a predicate representing the behaviours condition, and an action
		/// representing the behaviours action. This behaviour will be consulted for
		/// any event fired on this context.
		/// </summary>
		/// <param name="condition">The predicate to use as the behaviours condition.</param>
		/// <param name="action">The action to use as the behaviours action.</param>
		/// <param name="rescue">The action to perform to recover from errors.</param>
		public void Register(Predicate<IEventFor<TState>> condition, Action<IEventFor<TState>> action, Action<IEventFor<TState>, Exception> rescue) {
			this.Register(new RuntimeBehaviourFor<TState>(String.Empty, condition, action, rescue));
		}
		

		/// <summary>
		/// Fires an event on the context. Each behaviour registered with context
		/// is consulted in no particular order, and for each behaviour that has a condition
		/// that returns true when applied to the event, that behaviours action is executed.
		/// </summary>
		/// <param name="ev">The event to fire on this context.</param>
		/// <returns></returns>
		public virtual IEventFor<TState> Fire(IEventFor<TState> ev) {
			if (ev.Context != this) throw new ProcessException("The event has a different context that the one on which it has been fired.");
			this.Bus.OnNext(ev);
			return ev;
		}

		/// <summary>
		/// Constructs a simple event, with a simple string message
		/// and fires it on this context.
		/// </summary>
		/// <param name="message">The message to assign to the event.</param>
		/// <returns>Returns the event that was constructed and fired on this context.</returns>
		public IEventFor<TState> Fire(string message) {
			return this.Fire(message, null);
		}

		/// <summary>
		/// Constructs an event using the message specified, and using the dictionary
		/// provided to populate the parameters of the event. This event is then
		/// fired on this context.
		/// </summary>
		/// <param name="message">The message to assign to the event.</param>
		/// <param name="parms">The parameters to populate the event with.</param>
		/// <returns>Returns the event that was constructed and fired on this context.</returns>
		public abstract IEventFor<TState> Fire(string message, IDictionary<string, string> parms);

		/// <summary>
		/// Instructs the context that operations have finished, and that while it
		/// may still be consulted no further events will be fired.
		/// </summary>
		public void Completed() {
			this.Bus.OnCompleted();
		}


	}
	
}

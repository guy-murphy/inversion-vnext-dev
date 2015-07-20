using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Inversion.Data;

namespace Inversion.Process {
	public class StatefulContext<TState>: SimpleProcessContext, IStatefulContext<TState> where TState : new() {
		public TState State { get; } = new TState();
		public StatefulContext(IServiceContainer services) : base(services) {}
	}
}

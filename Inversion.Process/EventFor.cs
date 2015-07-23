using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Inversion.Process {
	/// <summary>
	/// Represents an event occuring in the system.
	/// </summary>
	/// <typeparam name="TContext">Type of the context which this event is for.</typeparam>
	/// <typeparam name="TBehaviour">Type of the behaviour that will respond to this event.</typeparam>
	public class EventFor<TContext, TBehaviour>: IEventFor<TContext> where TContext: IContextFor<IEventFor<TContext>, TBehaviour>  {
		
		private IData _object;

		/// <summary>
		/// Provides indexed access to the events parameters.
		/// </summary>
		/// <param name="key">The key of the parameter to look up.</param>
		/// <returns>Returns the parameter indexed by the key.</returns>
		public string this[string key] => this.Params.ContainsKey(key) ? this.Params[key] : null;

		/// <summary>
		/// The simple message the event represents.
		/// </summary>
		/// <remarks>
		/// Again, exactly what this means is application specific.
		/// </remarks>
		public string Message { get; }

		/// <summary>
		/// The parameters for this event represented
		/// as key-value pairs.
		/// </summary>
		public IDictionary<string, string> Params { get; }

		/// <summary>
		/// The context upon which this event is being fired.
		/// </summary>
		/// <remarks>
		/// And event always belongs to a context.
		/// </remarks>
		public TContext Context { get; }

		/// <summary>
		/// Any object that the event may be carrying.
		/// </summary>
		/// <remarks>
		/// This is a dirty escape hatch, and
		/// can even be used as a "return" value
		/// for the event.
		/// </remarks>
		public IData Object {
			get { return _object; }
			set {
				if (_object == null) {
					_object = value;
				} else {
					throw new InvalidOperationException("You may only set the Object value of an event the once. Thereafter it is readonly.");
				}
			}
		}

		/// <summary>
		/// Provides an abstract representation
		/// of the objects data expressed as a JSON object.
		/// </summary>
		/// <remarks>
		/// For this type the json object is created each time
		/// it is accessed.
		/// </remarks>
		public JObject Data => this.ToJsonObject();

		/// <summary>
		/// Instantiates a new event bound  to a context.
		/// </summary>
		/// <param name="context">The context to which the event is bound.</param>
		/// <param name="message">The simple message the event represents.</param>
		/// <param name="parameters">The parameters of the event.</param>
		public EventFor(TContext context, string message, IDictionary<string, string> parameters) : this(context, message, null, parameters) { }

		/// <summary>
		/// Instantiates a new event bound  to a context.
		/// </summary>
		/// <param name="context">The context to which the event is bound.</param>
		/// <param name="message">The simple message the event represents.</param>
		/// <param name="obj">An object being carried by the event.</param>
		/// <param name="parameters">The parameters of the event.</param>
		public EventFor(TContext context, string message, IData obj, IDictionary<string, string> parameters) {
			this.Context = context;
			this.Message = message;
			this.Params = (parameters == null) ? new Dictionary<string, string>() : new Dictionary<string, string>(parameters);
			_object = obj;
		}

		/// <summary>
		/// Instantiates a new event bound  to a context.
		/// </summary>
		/// <param name="context">The context to which the event is bound.</param>
		/// <param name="message">The simple message the event represents.</param>
		public EventFor(TContext context, string message) : this(context, message, null) { }

		/// <summary>
		/// Instantiates a new event as a copy of the event provided.
		/// </summary>
		/// <param name="ev">The event to copy for this new instance.</param>
		public EventFor(IEventFor<TContext> ev) {
			this.Context = ev.Context;
			this.Message = ev.Message;
			this.Params = new Dictionary<string, string>(ev.Params);
			_object = ev.Object;
		}

		/// <summary>
		/// Creates a clone of this event by copying
		/// it into a new instance.
		/// </summary>
		/// <returns>The newly cloned event.</returns>
		object ICloneable.Clone() {
			return new EventFor<TContext, TBehaviour>(this);
		}

		/// <summary>
		/// Creates a clone of this event by copying
		/// it into a new instance.
		/// </summary>
		/// <returns>The newly cloned event.</returns>
		public virtual EventFor<TContext, TBehaviour> Clone() {
			return new EventFor<TContext, TBehaviour>(this);
		}

		/// <summary>
		/// Adds a key-value pair as a parameter to the event.
		/// </summary>
		/// <param name="key">The key of the parameter.</param>
		/// <param name="value">The value of the parameter.</param>
		public void Add(string key, string value) {
			this.Params.Add(key, value);
		}

		/// <summary>
		/// Fires the event on the context to which it is bound.
		/// </summary>
		/// <returns>
		/// Returns the event that has just been fired.
		/// </returns>
		public IEventFor<TContext> Fire() {
			return this.Context.Fire(this);
		}

		/// <summary>
		/// Determines whether or not the parameters 
		/// specified exist in the event.
		/// </summary>
		/// <param name="parms">The parameters to check for.</param>
		/// <returns>Returns true if all the parameters exist; otherwise return false.</returns>
		public bool HasParams(params string[] parms) {
			return parms.Length > 0 && parms.All(parm => this.Params.ContainsKey(parm));
		}

		/// <summary>
		/// Determines whether or not the parameters 
		/// specified exist in the event.
		/// </summary>
		/// <param name="parms">The parameters to check for.</param>
		/// <returns>Returns true if all the parameters exist; otherwise return false.</returns>
		public bool HasParams(IEnumerable<string> parms) {
			return parms != null && parms.All(parm => this.Params.ContainsKey(parm));
		}

		/// <summary>
		/// Determines whether or not all the key-value pairs
		/// provided exist in the events parameters.
		/// </summary>
		/// <param name="match">The key-value pairs to check for.</param>
		/// <returns>
		/// Returns true if all the key-value pairs specified exists in the events
		/// parameters; otherwise returns false.
		/// </returns>
		public bool HasParamValues(IEnumerable<KeyValuePair<string, string>> match) {
			return match.All(entry => this.Params.Contains(entry));
		}

		/// <summary>
		/// Determines whether or not each of the prameters specified
		/// exist on the event, and creates an error for each one that
		/// does not.
		/// </summary>
		/// <param name="parms">The paramter names to check for.</param>
		/// <returns>
		/// Returns true if each of the parameters exist on the event;
		/// otherwise returns false.
		/// </returns>
		public bool HasRequiredParams(params string[] parms) {
			bool has = parms.Length > 0;
			foreach (string parm in parms) {
				if (!this.Params.ContainsKey(parm)) {
					has = false;
				}
			}
			return has;
		}

		/// <summary>
		/// Creates a string representation of the event.
		/// </summary>
		/// <returns>Returns a new string representing the event.</returns>
		public override string ToString() {
			StringBuilder sb = new StringBuilder();

			sb.AppendFormat("(event @message {0}\n", this.Message);
			foreach (KeyValuePair<string, string> entry in this.Params) {
				sb.AppendFormat("   ({0} -{1})\n", entry.Key, entry.Value);
			}
			sb.AppendLine(")");

			return sb.ToString();
		}

		/// <summary>
		/// Obtains an enumerator for the events parameters.
		/// </summary>
		/// <returns>Returns an enumerator suitable for iterating through the events parameters.</returns>
		public IEnumerator<KeyValuePair<string, string>> GetEnumerator() {
			return this.Params.GetEnumerator();
		}

		/// <summary>
		/// Obtains an enumerator for the events parameters.
		/// </summary>
		/// <returns>Returns an enumerator suitable for iterating through the events parameters.</returns>
		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
			return this.Params.GetEnumerator();
		}

		/// <summary>
		/// Produces an xml representation of the model.
		/// </summary>
		/// <param name="xml">The writer to used to write the xml to. </param>
		public virtual void ToXml(XmlWriter xml) {
			xml.WriteStartElement("event");
			xml.WriteAttributeString("message", this.Message);
			xml.WriteStartElement("params");
			foreach (KeyValuePair<string, string> entry in this.Params) {
				xml.WriteStartElement("item");
				xml.WriteAttributeString("name", entry.Key);
				xml.WriteAttributeString("value", entry.Value);
				xml.WriteEndElement();
			}
			xml.WriteEndElement();
			xml.WriteEndElement();
		}

		/// <summary>
		/// Produces a json respresentation of the model.
		/// </summary>
		/// <param name="json">The writer to use for producing json.</param>
		public virtual void ToJson(JsonWriter json) {
			json.WriteStartObject();
			json.WritePropertyName("_type");
			json.WriteValue("event");
			json.WritePropertyName("message");
			json.WriteValue(this.Message);
			json.WritePropertyName("params");
			json.WriteStartObject();
			foreach (KeyValuePair<string, string> kvp in this.Params) {
				json.WritePropertyName(kvp.Key);
				json.WriteValue(kvp.Value);
			}
			json.WriteEndObject();
			json.WriteEndObject();
		}

		/// <summary>
		/// Creates a new event from an xml representation.
		/// </summary>
		/// <param name="context">The context to which the new event will be bound.</param>
		/// <param name="xml">The xml representation of an event.</param>
		/// <returns>Returns a new event.</returns>
		public static Event FromXml(ProcessContext context, string xml) {
			try {
				XElement ev = XElement.Parse(xml);
				if (ev.Name == "event") {
					return new Event(
						context,
						ev.Attribute("message").Value,
						ev.Elements().ToDictionary(el => el.Attribute("name").Value, el => el.Attribute("value").Value)
					);
				} else {
					throw new Event.ParseException("The expressed type of the json provided does not appear to be an event.");
				}
			} catch (Exception err) {
				throw new Event.ParseException("An unexpected error was encoutered parsing the provided json into an event object.", err);
			}
		}

		/// <summary>
		/// Creates a new event from an json representation.
		/// </summary>
		/// <param name="context">The context to which the new event will be bound.</param>
		/// <param name="json">The json representation of an event.</param>
		/// <returns>Returns a new event.</returns>
		public static Event FromJson(ProcessContext context, string json) {
			try {
				JObject job = JObject.Parse(json);
				if (job.Value<string>("_type") == "event") {
					return new Event(
						context,
						job.Value<string>("message"),
						job.Value<JObject>("params").Properties().ToDictionary(p => p.Name, p => p.Value.ToString())
					);
				} else {
					throw new Event.ParseException("The expressed type of the json provided does not appear to be an event.");
				}
			} catch (Exception err) {
				throw new Event.ParseException("An unexpected error was encoutered parsing the provided json into an event object.", err);
			}
		}

		/// <summary>
		/// An exception thrown when unable to parse the xml or json representations
		/// for creating a new event.
		/// </summary>
		public class ParseException : InvalidOperationException {
			/// <summary>
			/// Instantiates a new parse exception with a human readable message.
			/// </summary>
			/// <param name="message">The human readable message for the exception.</param>
			public ParseException(string message) : base(message) { }
			/// <summary>
			/// instantiates a new exception wrapping a provided inner exception,
			/// with a human readable message.
			/// </summary>
			/// <param name="message">The human readable message for the exception.</param>
			/// <param name="err">The inner exception to wrap.</param>
			public ParseException(string message, Exception err) : base(message, err) { }
		}
	}
}

﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Caching;
using System.Text;
using System.Xml;
using System.Xml.Xsl;

using Inversion.Data;
using Inversion.Process;
using Inversion.Process.Behaviour;

namespace Inversion.Web.Behaviour.View {

	/// <summary>
	/// A behaviour that will transform the last view step by attempting to find
	/// an appropriate XSL style sheet, based upon the context params
	/// of *area*, *concern*, and *action*. 
	/// </summary>
	/// <remarks>
	/// This is intended for use in Web application, not as a general
	/// purpose XSL transform.
	/// </remarks>

	public class XsltViewBehaviour : ProcessBehaviour {

		// This is a piece of voodoo I was handed by a friend who had some
		//		similar occasional encoding problems which apparently are
		//		due to an underlying problem in the BCL.
		// Find out if its still relevant and what exactly its doing.

		private class StringWriterWithEncoding : StringWriter {
			public StringWriterWithEncoding(StringBuilder builder, Encoding encoding)
				: base(builder) {
				this.Encoding = encoding;
			}

			public override Encoding Encoding { get; }
		}

		private readonly string _contentType;
		private readonly bool _enableCache;


		/// <summary>
		/// Instantiates a new xslt view behaviour used to provide xslt templating
		/// primarily for web applications.
		/// </summary>
		/// <param name="respondsTo">The message the behaviour has set as responding to.</param>
		/// <remarks>
		/// Defaults to caching compiled xslt, to a content type of "text/xml".
		/// </remarks>
		public XsltViewBehaviour(string respondsTo) : this(respondsTo, "text/xml") { }

		/// <summary>
		/// Instantiates a new xslt view behaviour used to provide xslt templating
		/// primarily for web applications.
		/// </summary>
		/// <param name="respondsTo">The message the behaviour has set as responding to.</param>
		/// <param name="contentType">The content type of the view step produced from this behaviour.</param>
		/// <remarks>
		/// Defaults to caching compiled xslt.
		/// </remarks>
		public XsltViewBehaviour(string respondsTo, string contentType)
			: base(respondsTo) {
			_contentType = contentType;
			_enableCache = true;
		}

		/// <summary>
		/// Instantiates a new xslt view behaviour used to provide xslt templating
		/// primarily for web applications.
		/// </summary>
		/// <param name="respondsTo">The message the behaviour has set as responding to.</param>
		/// <param name="contentType">The content type of the view step produced from this behaviour.</param>
		/// <param name="enableCache">Specifies whether or not the xslt compilation should be cached.</param>
		public XsltViewBehaviour(string respondsTo, string contentType, bool enableCache)
			: this(respondsTo, contentType) {
			_enableCache = enableCache;
		}

		/// <summary>
		/// Instantiates a new xslt view behaviour used to provide xslt templating
		/// primarily for web applications.
		/// </summary>
		/// <param name="respondsTo">The message the behaviour has set as responding to.</param>
		/// <param name="enableCache">Specifies whether or not the xslt compilation should be cached.</param>
		/// <remarks>Defaults to a content type of "text/xml".</remarks>
		public XsltViewBehaviour(string respondsTo, bool enableCache)
			: this(respondsTo) {
			_enableCache = enableCache;
		}

		// The iterator generated for this should be
		//		ThreadLocal and therefore safe to use
		//		in this manner on a singleton, would be
		//		nice to fonfirm this.
		// At some point this will need to move to being
		// and injected strategy.
		private IEnumerable<string> _possibleTemplates(IProcessContext context) {
			string area = context.Params["area"];
			string concern = context.Params["concern"];
			string action = $"{context.Params["action"]}.xslt";

			// area/concern/action
			yield return Path.Combine(area, concern, action);
			// area/concern/default
			yield return Path.Combine(area, concern, "default.xslt");
			// area/action
			yield return Path.Combine(area, action);
			// area/default
			yield return Path.Combine(area, "default.xslt");
			// concern/action
			yield return Path.Combine(concern, action);
			// concern/default
			yield return Path.Combine(concern, "default.xslt");
			// action
			yield return action;
			// default
			yield return "default.xslt";
		}

		/// <summary>
		/// Takes the content of the last view-step and transforms it with the xslt with the location
		/// that best matches the path of the url. 
		/// </summary>
		/// <param name="ev">The event that gave rise to this action.</param>
		public override void Action(IEvent ev) {
			if (ev.Context.ViewSteps.HasSteps && ev.Context.ViewSteps.Last.HasContent || ev.Context.ViewSteps.Last.HasModel) {

				foreach (string templateName in _possibleTemplates(ev.Context)) {

					// check if we have the template cached
					string cacheKey = String.Concat("xsl::", templateName);
					XslCompiledTransform xsl = (!_enableCache || ev.Context.IsFlagged("nocache")) ? null : ev.Context.ObjectCache.Get(cacheKey) as XslCompiledTransform;
					if (xsl == null) {
						// we dont have it cached
						// does the file exist?					
						string templatePath = Path.Combine("Resources", "Views", "Xslt", templateName);
						if (ev.Context.Resources.Exists(templatePath)){
							xsl = ev.Context.Resources.Open(templatePath).AsXslDocument();
							if (_enableCache) {
								CacheItemPolicy policy = new CacheItemPolicy {
									AbsoluteExpiration = ObjectCache.InfiniteAbsoluteExpiration
								};
								ev.Context.ObjectCache.Add(cacheKey, xsl, policy);
							}
						}
					}
					// do the actual transform
					if (xsl != null) {
						// copy forward the parameters from the context
						// to the xsl stylesheet, with no namespace
						XsltArgumentList args = new XsltArgumentList();
						foreach (KeyValuePair<string, string> parm in ev.Context.Params) {
							args.AddParam(parm.Key, "", parm.Value);
						}

						StringBuilder result = new StringBuilder();
						XmlDocument input = new XmlDocument();
						
						string inputText = ev.Context.ViewSteps.Last.Content ?? ev.Context.ViewSteps.Last.Model.ToXml();
						input.LoadXml(inputText);
						xsl.Transform(input, args, new StringWriterWithEncoding(result, Encoding.UTF8));
						ev.Context.ViewSteps.CreateStep(templateName, _contentType, result.ToString());
						break; // we've found and processed our template, no need to keep looking
					}
				}

			}
		}
	}
}

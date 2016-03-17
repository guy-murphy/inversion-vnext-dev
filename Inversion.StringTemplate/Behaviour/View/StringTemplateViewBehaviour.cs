using System.IO;

using Antlr4.StringTemplate;

using Inversion.Process;
using Inversion.Web.Behaviour.View;

namespace Inversion.StringTemplate.Behaviour.View {
	/// <summary>
	/// A behaviour that will transform the last view step by attempting to find
	/// an appropriate ST4 template, based upon the context params
	/// of *area*, *concern*, and *action*. 
	/// </summary>
	public class StringTemplateViewBehaviour : ViewBehaviour {

		/// <summary>
		/// Creates a new instance of the behaviour, with the default
		/// content type of "text/html".
		/// </summary>
		/// <param name="respondsTo">The message the behaviour responds to.</param>
		public StringTemplateViewBehaviour(string respondsTo) : this(respondsTo, "text/html") { }
		/// <summary>
		/// Creates a new instance of the behaviour.
		/// </summary>
		/// <param name="respondsTo">The message the behaviour responds to.</param>
		/// <param name="contentType">The content type of the view step produced from this behaviour.</param>
		public StringTemplateViewBehaviour(string respondsTo, string contentType)
			: base(respondsTo, contentType) {}
		
		/// <summary>
		/// Implementors should impliment this behaviour with the desired action
		/// for their behaviour.
		/// </summary>
		/// <param name="ev">The event to consult.</param>
		public override void Action(IEvent ev) {
			if (ev.Context.ViewSteps.HasSteps && ev.Context.ViewSteps.Last.HasModel) {
				foreach (string templateName in this.GetPossibleTemplates(ev.Context, "st")) {
					string templatePath = Path.Combine("Resources", "Views", "ST", templateName);
					if (ev.Context.Resources.Exists(templatePath)) {
						string src = ev.Context.Resources.ReadAllText(templatePath);
						Template template = new Template(src, '`', '`');
						template.Add("ctx", ev.Context);
						template.Add("model", ev.Context.ViewSteps.Last.Model);
						string result = template.Render();
						ev.Context.ViewSteps.CreateStep(templateName, this.ContentType, result);
						break; // we've found and processed our template, no need to keep looking
					}
				}
			}
		}
	}
}

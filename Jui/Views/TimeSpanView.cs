using System;
using System.Text;
using HomeSeer.Jui.Types;
using Newtonsoft.Json;

namespace HomeSeer.Jui.Views {

	/// <inheritdoc cref="AbstractView"/>
	/// <summary>
	/// A time span allows user to enter a time interval in days, hours, minutes and seconds
	/// </summary>
	public sealed class TimeSpanView : AbstractView 
	{	
		/// <summary>
		/// The current value of the field
		/// </summary>
		[JsonProperty("value")]
		public TimeSpan Value { get; set; }

		/// <summary>
		/// Wether a number of days can be entered
		/// </summary>
		[JsonProperty("show_days")]
		public bool ShowDays { get; set; } = true;

		/// <summary>
		/// Wether a number of seconds can be entered
		/// </summary>
		[JsonProperty("show_seconds")]
		public bool ShowSeconds { get; set; } = true;

		/// <inheritdoc cref="AbstractView"/>
		/// <summary>
		/// Create a new instance of an TimeSpanView with an ID and  a Name
		/// </summary>
		/// <param name="id">The unique ID for the View</param>
		/// <param name="name">The name of the View</param>
		[JsonConstructor]
		public TimeSpanView(string id, string name) : base(id, name) 
		{	
			Type = EViewType.TimeSpan;
		}

		/// <inheritdoc cref="AbstractView"/>
		/// <summary>
		/// Create a new instance of an InputView with an ID, a Name, and a value.
		/// </summary>
		/// <param name="id">The unique ID for the View</param>
		/// <param name="name">The name of the View</param>
		/// <param name="value">The time span value</param>
		public TimeSpanView(string id, string name, TimeSpan value) : base(id, name) 
		{
			Type = EViewType.TimeSpan;
			Value = value;
		}

		/// <inheritdoc cref="AbstractView.Update"/>
		/// <summary>
		/// Update the view to the new state.  This will change the time span value 
		/// </summary>
		/// <exception cref="ViewTypeMismatchException">Thrown when the new view's class doesn't match the calling view</exception>
		public override void Update(AbstractView newViewState) 
		{
			base.Update(newViewState);

			if (!(newViewState is TimeSpanView updatedTimeSpanView)) 
			{
				throw new ViewTypeMismatchException("The original view type does not match the type in the update");
			}
			Value = updatedTimeSpanView.Value;
		}

		/// <inheritdoc cref="AbstractView.UpdateValue"/>
		public override void UpdateValue(string value) 
		{
			if (TimeSpan.TryParse(value, out TimeSpan parsedTimeSpan))
			{
				Value = parsedTimeSpan;
			}
			else
            {
				throw new FormatException($"Time span {value} is not in the correct format");
			}
		}

		/// <inheritdoc cref="AbstractView.GetStringValue"/>
		/// <remarks>
		/// The same as <see cref="Value"/>
		/// </remarks>
		public override string GetStringValue() {
			return Value.ToString();
		}

		/// <inheritdoc cref="AbstractView.ToHtml"/>
		public override string ToHtml(int indent = 0) {

			var sb = new StringBuilder();
			sb.Append(GetIndentStringFromNumber(indent));
			//Open the containing div
			sb.Append($"<div id=\"{Id}\" class=\"jui-view\">");
			sb.Append(Environment.NewLine);
			//Add the title
			sb.Append(GetIndentStringFromNumber(indent + 1));
			sb.Append("<div id=\"");
			sb.Append(Id);
			sb.Append(".title\" class=\"jui-title\">");
			sb.Append("<small>");
			sb.Append(Name);
			sb.Append("</small>");
			sb.Append("</div>");
			sb.Append(Environment.NewLine);
			if(ShowDays)
            {
				//Add the input for days
				sb.Append(GetIndentStringFromNumber(indent + 1));
				sb.Append("<span class=\"md-form\">");
				sb.Append(Environment.NewLine);
				sb.Append(GetIndentStringFromNumber(indent + 2));
				sb.Append($"<input type=\"number\" step=\"1\" min=\"0\"  id=\"{Id}-days\" class=\"jui-input jui-timespan\" value=\"{Value.Days}\" style=\"width: 56px;\">");
				sb.Append(Environment.NewLine);
				sb.Append(GetIndentStringFromNumber(indent + 2));
				sb.Append($"<label for=\"{Id}-days\" id=\"{Id}-days-hint\">Days</label>");
				sb.Append(Environment.NewLine);
				sb.Append(GetIndentStringFromNumber(indent + 1));
				sb.Append($"</span>");
				sb.Append(Environment.NewLine);
			}
			//Add the input for hours
			sb.Append(GetIndentStringFromNumber(indent+1));
			sb.Append("<span class=\"md-form\">");
			sb.Append(Environment.NewLine);
			sb.Append(GetIndentStringFromNumber(indent + 2));
			sb.Append($"<input type=\"number\" step=\"1\" min=\"0\" max=\"23\"  id=\"{Id}-hours\" class=\"jui-input jui-timespan\" value=\"{Value.Hours}\">");
            sb.Append(Environment.NewLine);
            sb.Append(GetIndentStringFromNumber(indent+2));
            sb.Append($"<label for=\"{Id}-hours\" id=\"{Id}-hours-hint\">Hours</label>");
			sb.Append(Environment.NewLine);
			sb.Append(GetIndentStringFromNumber(indent + 1));
			sb.Append($"</span>");
			sb.Append(Environment.NewLine);
			//Add the input for minutes
			sb.Append(GetIndentStringFromNumber(indent + 1));
			sb.Append("<span class=\"md-form\">");
			sb.Append(Environment.NewLine);
			sb.Append(GetIndentStringFromNumber(indent + 2));
			sb.Append($"<input type=\"number\" step=\"1\" min=\"0\" max=\"59\" id=\"{Id}-minutes\" class=\"jui-input jui-timespan\" value=\"{Value.Minutes}\">");
			sb.Append(Environment.NewLine);
			sb.Append(GetIndentStringFromNumber(indent + 2));
			sb.Append($"<label for=\"{Id}-minutes\" id=\"{Id}-minutes-hint\">Mins</label>");
			sb.Append(Environment.NewLine);
			sb.Append(GetIndentStringFromNumber(indent + 1));
			sb.Append($"</span>");
			sb.Append(Environment.NewLine);
			if (ShowSeconds)
			{
				//Add the input for seconds
				sb.Append(GetIndentStringFromNumber(indent + 1));
				sb.Append("<span class=\"md-form\">");
				sb.Append(Environment.NewLine);
				sb.Append(GetIndentStringFromNumber(indent + 2));
				sb.Append($"<input type=\"number\" step=\"1\" min=\"0\" max=\"59\" id=\"{Id}-seconds\" class=\"jui-input jui-timespan\" value=\"{Value.Seconds}\">");
				sb.Append(Environment.NewLine);
				sb.Append(GetIndentStringFromNumber(indent + 2));
				sb.Append($"<label for=\"{Id}-seconds\" id=\"{Id}-seconds-hint\">Secs</label>");
				sb.Append(Environment.NewLine);
				sb.Append(GetIndentStringFromNumber(indent + 1));
				sb.Append($"</span>");
				sb.Append(Environment.NewLine);
			}
			//Close the containing div
			sb.Append(GetIndentStringFromNumber(indent));
            sb.Append("</div>");
            sb.Append(Environment.NewLine);
			
			return sb.ToString();
		}

	}

}
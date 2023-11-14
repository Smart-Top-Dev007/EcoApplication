using System;
using System.Text;
using System.Web.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace EcoCentre.Models.Infrastructure.Serialization
{
	public class CamelCaseJsonResult : ActionResult
	{
		public Encoding ContentEncoding { get; set; }
		public string ContentType { get; set; }
		public object Data { get; set; }
		
		public Formatting Formatting { get; set; }

		public CamelCaseJsonResult(object data, Formatting formatting) : this(data)
		{
			Formatting = formatting;
		}

		public CamelCaseJsonResult(object data) : this()
		{
			Data = data;
		}

		public CamelCaseJsonResult()
		{
			Formatting = Formatting.None;
		}

		public override void ExecuteResult(ControllerContext context)
		{
			if (context == null)
				throw new ArgumentNullException(nameof(context));
			var response = context.HttpContext.Response;
			response.ContentType = !string.IsNullOrEmpty(ContentType)
				? ContentType
				: "application/json";
			if (ContentEncoding != null)
				response.ContentEncoding = ContentEncoding;

			if (Data == null) return;

			var writer = new JsonTextWriter(response.Output) { Formatting = Formatting };
			var serializerSettings = new JsonSerializerSettings();
			var header = context.RequestContext.HttpContext.Request.Headers["X-Camel-Case-Json"];
			if (header?.ToLower() == "true")
			{
				serializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
			}
			var serializer = JsonSerializer.Create(serializerSettings);
			serializer.Serialize(writer, Data);
			writer.Flush();
		}
	}
}
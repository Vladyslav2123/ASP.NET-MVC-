using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace HW_11.Areas.Auth.Models;

public static class NotifyExtensions
{
	public static void AddToast(this Controller controller, string type, string content)
	{
		var data = controller.TempData["Toasts"];
		List<Toast> toasts = data == null ? new List<Toast>() : JsonSerializer.Deserialize<List<Toast>>(data.ToString());
		toasts.Add(new Toast { Type = type, Content = content });
		controller.TempData["Toasts"] = JsonSerializer.Serialize(toasts);
	}
}

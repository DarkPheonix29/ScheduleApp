using Microsoft.AspNetCore.Mvc;
using System.Net.WebSockets;
using System.Text;
using static UserManager;

namespace ScheduleApp.Server.Controllers;

[Route("api/[controller]")]
[ApiController]
public class NotificationController : ControllerBase
{
	private WebSocket? _webSocket;

	[HttpGet("connect")]
	public async Task<ActionResult> OpenWebSocketAsync()
	{
		if (!HttpContext.WebSockets.IsWebSocketRequest)
		{
			return BadRequest("WebSocket request expected.");
		}

		// Accept the WebSocket connection
		_webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync();
		Console.WriteLine("WebSocket connection established.");

		OnAdminMessage += SendNotificationAsync;

		// Keep listening for WebSocket state changes
		while (_webSocket.State is WebSocketState.Connecting or WebSocketState.Open)
		{
			await Task.Delay(1000); // Prevent high CPU usage
		}

		OnAdminMessage -= SendNotificationAsync;

		Console.WriteLine("WebSocket connection closed.");
		return Ok();
	}

	private void SendNotificationAsync(object? obj, AdminMessageEventArgs args)
	{
		Task.Run(async () =>
		{
			if (_webSocket is null)
			{
				return;
			}

			var message = Encoding.UTF8.GetBytes(args.Message);
			await _webSocket.SendAsync(new ArraySegment<byte>(message), WebSocketMessageType.Text, true, CancellationToken.None);
		});
	}

	/*public async Task SendNotificationAsync(string message)
	{
		if (_webSocket != null && _webSocket.State == WebSocketState.Open)
		{
			var messageBytes = System.Text.Encoding.UTF8.GetBytes(message);
			await _webSocket.SendAsync(
				new ArraySegment<byte>(messageBytes),
				WebSocketMessageType.Text,
				true,
				CancellationToken.None
			);
			Console.WriteLine("Notification sent: " + message);
		}
	}*/
}

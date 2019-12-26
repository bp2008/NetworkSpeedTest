function WebSocketStreamer(packetReceived)
{
	var self = this;
	var socket;
	var ws_is_ready = false;
	this.onStateChanged = null;

	this.Connect = function ()
	{
		socket = new WebSocket("ws" + (location.protocol === "https:" ? "s" : "") + "://" + location.hostname + ":" + location.port + "/nstws");
		raiseStateChanged();
		socket.binaryType = "arraybuffer";
		socket.onopen = function (event)
		{
			console.info("WebSocket Open");
			ws_is_ready = true;
			raiseStateChanged();
		};
		socket.onclose = function (event)
		{
			var codeTranslation = TranslateWebSocketCloseCode(event.code);
			var errmsg = "Code " + EscapeHTML(event.code + (event.reason ? " " + event.reason : "")) + "<br/>" + codeTranslation[0] + "<br/>" + codeTranslation[1];
			console.info("WebSocket Closed", errmsg);
			raiseStateChanged();
		};
		socket.onerror = function (event)
		{
			// We can't find out what the error was.  Yay web standards.
			console.error("WebSocket Error");
		};
		socket.onmessage = function (event)
		{
			HandleWSMessage(event.data);
		};
	};
	this.Disconnect = function ()
	{
		socket.close();
		socket = null;
	};
	var HandleWSMessage = function (data)
	{
		packetReceived(data);
		//var cmdView = new Uint8Array(data, 0, 1);
		//if (cmdView.length === 0)
		//{
		//	toaster.Warning("empty message from server");
		//	return;
		//}
		//switch (cmdView[0])
		//{
		//	case Command.StartStreaming:
		//		currentStreamId = Util.ReadByte(data, { offset: 1 });
		//		console.log("Streaming Started: " + currentStreamId);
		//		break;
		//	case Command.StopStreaming:
		//		toaster.Info("StopStreaming message received from server.");
		//		break;
		//	case Command.GetScreenCapture:
		//		var streamId = Util.ReadByte(data, { offset: 1 });
		//		if (streamId !== currentStreamId)
		//		{
		//			console.info("dropped frame from stream " + streamId + " because current stream is " + currentStreamId);
		//			break;
		//		}
		//		if (typeof self.onFrameReceived === "function")
		//			self.onFrameReceived(data);
		//		//mainMenu.bytesThisSecond += data.byteLength;
		//		acknowledgeFrame(streamId);
		//		break;
		//	case Command.ReproduceUserInput:
		//		toaster.Warning("ReproduceUserInput message received from server. This should not ever happen.");
		//		break;
		//	case Command.GetDesktopInfo:
		//		toaster.Info("GetDesktopInfo message received from server.");
		//		self.currentDesktopInfo = new DesktopInfo(data, { offset: 1 });
		//		console.log("GetDesktopInfo", self.currentDesktopInfo);
		//		break;
		//	case Command.Error_SyntaxError:
		//		toaster.Warning("Error_SyntaxError message received from server");
		//		break;
		//	case Command.Error_CommandCodeUnknown:
		//		toaster.Warning("Error_CommandCodeUnknown message received from server");
		//		break;
		//	case Command.Error_Unspecified:
		//		toaster.Warning("Error_Unspecified message received from server");
		//		break;
		//	default:
		//		toaster.Warning("Unidentifiable message received from server, starting with byte: " + cmdView[0]);
		//		break;
		//}
	};
	//this.setStreamSettings = function (clientSettings)
	//{
	//	var arg = new Uint8Array(5);
	//	arg[0] = Command.SetStreamSettings;
	//	arg[1] = GetImageColorFlags(clientSettings.colorDetail);
	//	arg[2] = Util.Clamp(parseInt(clientSettings.quality), 1, 100);
	//	arg[3] = Util.Clamp(parseInt(clientSettings.maxFps), 1, 60);
	//	arg[4] = Util.Clamp(parseInt(clientSettings.maxFramesInTransit), 1, 60);
	//	SendToWebSocket(arg);
	//};
	//this.startStreaming = function ()
	//{
	//	console.log("StartStreaming");
	//	var arg = new Uint8Array(3);
	//	arg[0] = Command.StartStreaming;
	//	arg[1] = StreamType.JPEG; // Stream type (JPEG / H.264). H.264 not yet implemented.
	//	arg[2] = 0; // 0 = Primary display

	//	SendToWebSocket(arg);
	//};
	//this.stopStreaming = function ()
	//{
	//	console.log("StopStreaming");
	//	var arg = new Uint8Array(2);
	//	arg[0] = Command.StopStreaming;
	//	SendToWebSocket(arg);
	//};
	//var acknowledgeFrame = function (streamId)
	//{
	//	var arg = new Uint8Array(2);
	//	arg[0] = Command.AcknowledgeFrame;
	//	arg[1] = streamId;
	//	SendToWebSocket(arg);
	//};
	//this.reproduceKeyAction = function (keyDown, keyCode, modifiers)
	//{
	//	var arg = new Uint8Array(10);
	//	arg[0] = Command.ReproduceUserInput;
	//	arg[1] = keyDown ? InputType.KeyDown : InputType.KeyUp;
	//	var offsetWrapper = { offset: 2 };
	//	Util.WriteInt32(arg, offsetWrapper, keyCode);
	//	Util.WriteUInt32(arg, offsetWrapper, modifiers);
	//	SendToWebSocket(arg);
	//};
	//this.reproduceMouseMoveAction = function (x, y)
	//{
	//	var arg = new Uint8Array(10);
	//	arg[0] = Command.ReproduceUserInput;
	//	arg[1] = InputType.MouseMove;
	//	var offsetWrapper = { offset: 2 };
	//	Util.WriteFloat(arg, offsetWrapper, x);
	//	Util.WriteFloat(arg, offsetWrapper, y);
	//	SendToWebSocket(arg);
	//};
	//this.reproduceMouseButtonAction = function (buttonDown, buttonCode)
	//{
	//	var arg = new Uint8Array(3);
	//	arg[0] = Command.ReproduceUserInput;
	//	arg[1] = buttonDown ? InputType.MouseButtonDown : InputType.MouseButtonUp;
	//	arg[2] = buttonCode;
	//	SendToWebSocket(arg);
	//};
	//this.reproduceMouseWheelAction = function (deltaX, deltaY)
	//{
	//	var arg = new Uint8Array(6);
	//	arg[0] = Command.ReproduceUserInput;
	//	arg[1] = InputType.MouseWheel;
	//	var offsetWrapper = { offset: 2 };
	//	Util.WriteInt16(arg, offsetWrapper, deltaX);
	//	Util.WriteInt16(arg, offsetWrapper, deltaY);
	//	SendToWebSocket(arg);
	//};
	//var GetImageColorFlags = function (colorDetail)
	//{
	//	if (colorDetail === 1)
	//		return ImgFlags.Color420;
	//	if (colorDetail === 2)
	//		return ImgFlags.Color440;
	//	if (colorDetail === 3)
	//		return ImgFlags.Color444;
	//	return ImgFlags.Grayscale;
	//};
	//var getDesktopInfo = function ()
	//{
	//	var arg = new Uint8Array(1);
	//	arg[0] = Command.GetDesktopInfo;
	//	SendToWebSocket(arg);
	//};
	this.SendToWebSocket = function (message)
	{
		if (typeof socket === "undefined" || !socket)
		{
			console.debug("Outgoing websocket message suppressed because socket is not open");
			return;
		}
		switch (socket.readyState)
		{
			case WebSocketState.Connecting:
				console.debug("WebSocket is still connecting.");
				break;
			case WebSocketState.Open:
				if (ws_is_ready)
					socket.send(message);
				else
					console.error("Authentication error");
				break;
			case WebSocketState.Closing:
				console.debug("WebSocket is closing.");
				break;
			case WebSocketState.Closed:
				console.debug("WebSocket is closed.");
				break;
		}
	};

	///////////////////////////////////////////////////////////////
	// Simple Public Getters //////////////////////////////////////
	///////////////////////////////////////////////////////////////
	this.getReadyState = function ()
	{
		return socket ? socket.readyState : WebSocketState.Closed;
	};
	///////////////////////////////////////////////////////////////
	// Private Helper Methods /////////////////////////////////////
	///////////////////////////////////////////////////////////////
	var raiseStateChanged = function ()
	{
		if (socket && typeof self.onStateChanged === "function")
			self.onStateChanged(socket.readyState);
	};
}

function TranslateWebSocketCloseCode(code)
{
	return WebSocketCloseCode.Translate(code);
}
var WebSocketCloseCode = new (function ()
{
	this.Translate = function (code)
	{
		if (code >= 0 && code <= 999)
			return ["Unknown code", "Error code is reserved and not used."];
		else if (code >= 1000 && code <= 1015)
			return [ws_code_map_name[code], ws_code_map_desc[code]];
		else if (code >= 1016 && code <= 1999)
			return ["Unknown code", "Error code is reserved for future use by the WebSocket standard."];
		else if (code >= 2000 && code <= 2999)
			return ["Unknown code", "Error code is reserved for use by WebSocket extensions."];
		else if (code >= 3000 && code <= 3999)
			return ["Unknown code", "Error code is available for use by libraries and frameworks. May not be used by applications. Available for registration at the IANA via first-come, first-serve."];
		else if (code >= 4000 && code <= 4999)
			return ["Unknown code", "Error code is available for use by applications."];
		else
			return ["Unknown code", "Unknown code"];
	};
	var ws_code_map_name = {};
	var ws_code_map_desc = {};
	ws_code_map_name[1000] = "CLOSE_NORMAL";
	ws_code_map_desc[1000] = "Normal closure; the connection successfully completed whatever purpose for which it was created.";
	ws_code_map_name[1001] = "CLOSE_GOING_AWAY";
	ws_code_map_desc[1001] = "The endpoint is going away, either because of a server failure or because the browser is navigating away from the page that opened the connection.";
	ws_code_map_name[1002] = "CLOSE_PROTOCOL_ERROR";
	ws_code_map_desc[1002] = "The endpoint is terminating the connection due to a protocol error.";
	ws_code_map_name[1003] = "CLOSE_UNSUPPORTED";
	ws_code_map_desc[1003] = "The connection is being terminated because the endpoint received data of a type it cannot accept (for example, a text-only endpoint received binary data).";
	ws_code_map_name[1004] = " ";
	ws_code_map_desc[1004] = "Reserved. A meaning might be defined in the future.";
	ws_code_map_name[1005] = "CLOSE_NO_STATUS";
	ws_code_map_desc[1005] = "Reserved.  Indicates that no status code was provided even though one was expected.";
	ws_code_map_name[1006] = "CLOSE_ABNORMAL";
	ws_code_map_desc[1006] = "Reserved. Used to indicate that a connection was closed abnormally (that is, with no close frame being sent) when a status code is expected.";
	ws_code_map_name[1007] = "Unsupported Data";
	ws_code_map_desc[1007] = "The endpoint is terminating the connection because a message was received that contained inconsistent data (e.g., non-UTF-8 data within a text message).";
	ws_code_map_name[1008] = "Policy Violation";
	ws_code_map_desc[1008] = "The endpoint is terminating the connection because it received a message that violates its policy. This is a generic status code, used when codes 1003 and 1009 are not suitable.";
	ws_code_map_name[1009] = "CLOSE_TOO_LARGE";
	ws_code_map_desc[1009] = "The endpoint is terminating the connection because a data frame was received that is too large.";
	ws_code_map_name[1010] = "Missing Extension";
	ws_code_map_desc[1010] = "The client is terminating the connection because it expected the server to negotiate one or more extension, but the server didn't.";
	ws_code_map_name[1011] = "Internal Error";
	ws_code_map_desc[1011] = "The server is terminating the connection because it encountered an unexpected condition that prevented it from fulfilling the request.";
	ws_code_map_name[1012] = "Service Restart";
	ws_code_map_desc[1012] = "The server is terminating the connection because it is restarting. [Ref]";
	ws_code_map_name[1013] = "Try Again Later";
	ws_code_map_desc[1013] = "The server is terminating the connection due to a temporary condition, e.g. it is overloaded and is casting off some of its clients. [Ref]";
	ws_code_map_name[1014] = " ";
	ws_code_map_desc[1014] = "Reserved for future use by the WebSocket standard.";
	ws_code_map_name[1015] = "TLS Handshake";
	ws_code_map_desc[1015] = "Reserved. Indicates that the connection was closed due to a failure to perform a TLS handshake (e.g., the server certificate can't be verified).";
})();
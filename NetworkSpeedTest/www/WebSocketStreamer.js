function WebSocketStreamer(endpoint, packetReceived, onClose)
{
	if (!endpoint)
		throw new Error("Undefined endpoint given to WebSocketStreamer");
	var self = this;
	var socket;
	var ws_is_ready = false;
	var totalBytesSent = 0;
	var totalBytesReceived = 0;

	this.Connect = function ()
	{
		return new Promise((resolve, reject) =>
		{
			socket = new WebSocket("ws" + (location.protocol === "https:" ? "s" : "") + "://" + location.hostname + (location.port ? (":" + location.port) : "") + endpoint);
			socket.binaryType = "arraybuffer";
			socket.onopen = function (event)
			{
				console.log("WebSocket Open");
				ws_is_ready = true;
				resolve();
			};
			socket.onclose = function (event)
			{
				var codeTranslation = TranslateWebSocketCloseCode(event.code);
				var errmsg = "Code " + EscapeHTML(event.code + (event.reason ? " " + event.reason : "")) + "\n"
					+ codeTranslation[0] + "\n"
					+ codeTranslation[1];
				console.log("WebSocket Closed", errmsg);
				if (typeof onClose === "function")
					onClose(event);
				reject(new Error(errmsg));
			};
			socket.onerror = function (event)
			{
				console.error("WebSocket Error");
				reject(new Error("WebSocket Error"));
			};
			socket.onmessage = function (event)
			{
				HandleWSMessage(event.data);
			};
		});
	};

	this.Disconnect = function ()
	{
		socket.close();
		socket = null;
	};
	var HandleWSMessage = function (data)
	{
		var totalDataLength = data.byteLength;
		if (totalDataLength <= 125)
			totalDataLength += 6;
		else if (totalDataLength <= 65535)
			totalDataLength += 8;
		else
			totalDataLength += 14;
		totalBytesReceived += totalDataLength;

		if (typeof packetReceived === "function")
			packetReceived(data, totalDataLength);
	};
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
				{
					socket.send(message);
					var totalDataLength = message.length;
					if (totalDataLength <= 125)
						totalDataLength += 6;
					else if (totalDataLength <= 65535)
						totalDataLength += 8;
					else
						totalDataLength += 14;
					totalBytesSent += totalDataLength
				}
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
	this.getBufferedAmount = function ()
	{
		return socket ? socket.bufferedAmount : 0;
	};
	this.getBytesSent = function ()
	{
		return totalBytesSent - self.getBufferedAmount();
	};
	this.getBytesReceived = function ()
	{
		return totalBytesReceived;
	};
	///////////////////////////////////////////////////////////////
	// Private Helper Methods /////////////////////////////////////
	///////////////////////////////////////////////////////////////
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
var WebSocketState = {
	Connecting: 0,
	Open: 1,
	Closing: 2,
	Closed: 3
};
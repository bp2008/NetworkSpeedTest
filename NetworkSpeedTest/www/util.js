function FetchDataStreamer(url, dataCallback, streamEnded, ShowError)
{
	var self = this;
	var cancel_streaming = false;
	var stopCalledByApp = false;
	var reader = null;

	var abort_controller = null;
	var responseError = null;

	this.StopStreaming = function ()
	{
		stopCalledByApp = true;
		stopStreaming_Internal();
	}
	var stopStreaming_Internal = function ()
	{
		cancel_streaming = true;
		// Aborting the AbortController must happen first, or else some versions of MS Edge leave the stream open in the background.
		if (abort_controller)
		{
			abort_controller.abort();
			abort_controller = null;
		}
		if (reader)
		{
			var cancelPromise = reader.cancel("Streaming canceled");
			if (cancelPromise && cancelPromise["catch"])
				cancelPromise["catch"](function (e)
				{
					if (DOMException && DOMException.ABORT_ERR && e && e.code === DOMException.ABORT_ERR)
					{
						// Expected result. Don't spam console.
					}
					else if (DOMException && DOMException.INVALID_STATE_ERR && e && e.code === DOMException.INVALID_STATE_ERR)
					{
						// Expected result in MS Edge.
					}
					else
						console.error(e);
				});
			reader = null;
		}
	}
	var Start = function ()
	{
		var fetchArgs = { credentials: "same-origin" };
		if (typeof AbortController === "function")
		{
			// FF 57+, Edge 16+ (in theory)
			// Broken in Edge 17.x and 18.x (connection stays open)
			// Unknown when it will be fixed
			abort_controller = new AbortController();
			fetchArgs.signal = abort_controller.signal;
		}
		fetch(url, fetchArgs).then(function (res)
		{
			try
			{
				if (res.headers.get("Content-Type") === "application/x-binary")
				{
					if (!res.ok)
						responseError = res.status + " " + res.statusText;
					// Do NOT return before the first reader.read() or the fetch can be left in a bad state!
					reader = res.body.getReader();
					return pump(reader);
				}
			}
			catch (e)
			{
				ShowError(e);
			}
		})["catch"](function (e)
		{
			try
			{
				CallStreamEnded(e);
			}
			catch (e)
			{
				ShowError(e);
			}
		});
	}
	function CallStreamEnded(message)
	{
		if (typeof streamEnded === "function")
		{
			try
			{
				streamEnded(message, stopCalledByApp, responseError);
			}
			catch (e)
			{
				ShowError(e);
			}
		}
		streamEnded = null;
	}
	function CallDataCallback()
	{
		try
		{
			dataCallback.apply(this, arguments);
		}
		catch (e)
		{
			ShowError(e);
		}
	}

	function pump()
	{
		// Do NOT return before the first reader.read() or the fetch can be left in a bad state!
		// Except if reader is null of course.
		if (reader == null)
			return;
		reader.read().then(function (result)
		{
			try
			{
				if (result.done)
				{
					CallStreamEnded("fetch graceful exit (type 1)");
					return;
				}
				else if (cancel_streaming)
				{
					stopStreaming_Internal();
					CallStreamEnded("fetch graceful exit (type 2)");
					return;
				}

				CallDataCallback(result.value);
				return pump();
			}
			catch (e)
			{
				ShowError(e);
			}
		}
		)["catch"](function (e)
		{
			try
			{
				stopStreaming_Internal();
				CallStreamEnded(e);
			}
			catch (e)
			{
				ShowError(e);
			}
		});
	}

	try
	{
		Start();
	}
	catch (e)
	{
		ShowError(e);
	}
}
// HELPER METHODS //
String.prototype.toFloat = function (digits)
{
	return parseFloat(this.toFixed(digits));
};
Number.prototype.toFloat = function (digits)
{
	return parseFloat(this.toFixed(digits));
};
function formatBytes(bytes, decimals)
{
	if (bytes == 0) return '0B';
	var negative = bytes < 0;
	if (negative)
		bytes = -bytes;
	var k = 1024,
		dm = typeof decimals != "undefined" ? decimals : 2,
		sizes = ['B', 'KB', 'MB', 'GB', 'TB', 'PB', 'EB', 'ZB', 'YB'],
		i = Math.floor(Math.log(bytes) / Math.log(k));
	return (negative ? '-' : '') + (bytes / Math.pow(k, i)).toFloat(dm) + sizes[i];
}
function formatBitsPerSecond(bits)
{
	if (bits == 0) return '0 bps';
	var negative = bits < 0;
	if (negative)
		bits = -bits;
	var k = 1000,
		dm = typeof decimals != "undefined" ? decimals : 2,
		sizes = ['bps', 'Kbps', 'Mbps', 'Gbps', 'Tbps', 'Pbps', 'Ebps', 'Zbps', 'Ybps'],
		decimals = [0, 0, 1, 2, 2, 2, 2, 2, 2],
		i = Math.floor(Math.log(bits) / Math.log(k));
	return (negative ? '-' : '') + (bits / Math.pow(k, i)).toFloat(decimals[i]) + ' ' + sizes[i];
}
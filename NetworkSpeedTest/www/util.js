function Queue()
{
	var e = this, a = [], c = 0; this.getLength = function () { return a.length - c }; this.isEmpty = function () { return a.length === c }; this.enqueue = function (d) { a.push(d) }; this.dequeue = function () { if (!e.isEmpty()) { var d = a[c]; a[c] = void 0; c++; 16 < a.length && 2 * c >= a.length && (a = a.slice(c), c = 0); return d } }; this.peek = function () { return e.isEmpty() ? void 0 : a[c] }; this.replaceFront = function (d) { e.isEmpty() ? e.enqueue(d) : a[c] = d }; this.toArray = function () { for (var d = Array(e.getLength()), b = c, f = 0; b < a.length; b++, f++)d[f] = a[b]; return d };
	this.find = function (d) { for (var b = c; b < a.length; b++)if (d(a[b])) return a[b] }; this.removeAll = function (d) { for (var b = c; b < a.length; b++)d(a[b]) && (a.splice(b, 1), b--) }
};
// HELPER METHODS //
String.prototype.toFloat = function (digits)
{
	return parseFloat(this.toFixed(digits));
};
Number.prototype.toFloat = function (digits)
{
	return parseFloat(this.toFixed(digits));
};
function formatBytes2(bytes, decimals)
{
	if (bytes == 0) return '0B';
	var negative = bytes < 0;
	if (negative)
		bytes = -bytes;
	var k = 1024,
		dm = typeof decimals != "undefined" ? decimals : 2,
		sizes = ['B', 'KiB', 'MiB', 'GiB', 'TiB', 'PiB', 'EiB', 'ZiB', 'YiB'],
		i = Math.floor(Math.log(bytes) / Math.log(k));
	return (negative ? '-' : '') + (bytes / Math.pow(k, i)).toFloat(dm) + sizes[i];
}
function formatBytes10(bytes, decimals)
{
	if (bytes == 0) return '0B';
	var negative = bytes < 0;
	if (negative)
		bytes = -bytes;
	var k = 1000,
		dm = typeof decimals != "undefined" ? decimals : 2,
		sizes = ['B', 'KB', 'MB', 'GB', 'TB', 'PB', 'EB', 'ZB', 'YB'],
		i = Math.floor(Math.log(bytes) / Math.log(k));
	return (negative ? '-' : '') + (bytes / Math.pow(k, i)).toFloat(dm) + sizes[i];
}
function formatBits(bits)
{
	if (bits == 0) return '0 b';
	var negative = bits < 0;
	if (negative)
		bits = -bits;
	var k = 1000,
		sizes = ['b', 'Kb', 'Mb', 'Gb', 'Tb', 'Pb', 'Eb', 'Zb', 'Yb'],
		decimals = [0, 0, 1, 2, 2, 2, 2, 2, 2],
		i = Math.floor(Math.log(bits) / Math.log(k));
	return (negative ? '-' : '') + (bits / Math.pow(k, i)).toFloat(decimals[i]) + ' ' + sizes[i];
}
function formatBitsDec(bits, decimals)
{
	if (bits == 0) return '0 b';
	var negative = bits < 0;
	if (negative)
		bits = -bits;
	var k = 1000,
		dm = typeof decimals != "undefined" ? decimals : 2,
		sizes = ['b', 'Kb', 'Mb', 'Gb', 'Tb', 'Pb', 'Eb', 'Zb', 'Yb'],
		i = Math.floor(Math.log(bits) / Math.log(k));
	return (negative ? '-' : '') + (bits / Math.pow(k, i)).toFloat(dm) + ' ' + sizes[i];
}
var escape = document.createElement('textarea');
function EscapeHTML(html)
{
	escape.textContent = html;
	return escape.innerHTML;
}
function UnescapeHTML(html)
{
	escape.innerHTML = html;
	return escape.textContent;
}
/**
 * Reads a 32-bit unsigned integer (big-endian) from the buffer at the specified offset.
 * @param {ArrayBuffer} buf - The buffer to read from.
 * @param {Object} offsetWrapper - An object containing the current offset. The offset will be updated after reading.
 * @param {number} offsetWrapper.offset - The current offset in the buffer.
 * @returns {number} The 32-bit unsigned integer read from the buffer.
 */
function ReadUInt32(buf, offsetWrapper)
{
	if (!offsetWrapper || typeof offsetWrapper.offset !== "number")
		offsetWrapper = { offset: 0 };
	var v = new DataView(buf, offsetWrapper.offset, 4).getUint32(0, false);
	offsetWrapper.offset += 4;
	return v;
}
/**
 * Reads a 64-bit unsigned integer (big-endian) from the buffer at the specified offset.
 * This returns BigInt because ordinary JavaScript numbers are natively 64-bit doubles with only 53-bit integer precision.
 * @param {ArrayBuffer} buf - The buffer to read from.
 * @param {Object} offsetWrapper - An object containing the current offset. The offset will be updated after reading.
 * @param {number} offsetWrapper.offset - The current offset in the buffer.
 * @returns {BigInt} The 64-bit unsigned integer read from the buffer.
 */
function ReadUInt64(buf, offsetWrapper)
{
	if (!offsetWrapper || typeof offsetWrapper.offset !== "number")
		offsetWrapper = { offset: 0 };
	var mostSignificant = BigInt(ReadUInt32(buf, offsetWrapper)) << 32n;
	var leastSignificant = BigInt(ReadUInt32(buf, offsetWrapper));
	return mostSignificant + leastSignificant;
}
/**
 * Writes a 32-bit unsigned integer to a new buffer (big-endian).
 * @param {number} value - The 32-bit unsigned integer to write.
 * @returns {Uint8Array} A new Uint8Array containing the written value.
 */
function UInt32ToUint8Array(value)
{
	var buf = new ArrayBuffer(4);
	var view = new DataView(buf);
	view.setUint32(0, value, false);
	return new Uint8Array(buf);
}
/**
 * Writes a 64-bit unsigned integer to a new buffer (big-endian).
 * @param {BigInt} value - The 64-bit unsigned integer to write.
 * @returns {Uint8Array} A new Uint8Array containing the written value.
 */
function UInt64ToUint8Array(value)
{
	value = BigInt(value);
	var buf = new ArrayBuffer(8);
	var view = new DataView(buf);
	var mostSignificant = Number(value >> 32n);
	var leastSignificant = Number(value & 0xFFFFFFFFn);
	view.setUint32(0, mostSignificant, false);
	view.setUint32(4, leastSignificant, false);
	return new Uint8Array(buf);
}
function InjectStyleBlock(cssText)
{
	var styleBlock = document.createElement('style');
	styleBlock.setAttribute('type', 'text/css');
	styleBlock.innerText = cssText;
	document.head.appendChild(styleBlock);
	return function (newCssText) { styleBlock.innerText = newCssText; };
}
// Vue Speedometer //	
Vue.component('speedometer', {
	props: {
		uploadSpeed: {
			type: Number,
			required: true
		},
		downloadSpeed: {
			type: Number,
			required: true
		},
		maxSpeed: {
			type: Number,
			default: 100
		},
		size: {
			type: Number,
			default: 200
		}
	},
	computed: {
		uploadNeedleX()
		{
			return 100 + 90 * Math.cos(this.angle(this.uploadSpeed) - Math.PI / 2);
		},
		uploadNeedleY()
		{
			return 100 + 90 * Math.sin(this.angle(this.uploadSpeed) - Math.PI / 2);
		},
		downloadNeedleX()
		{
			return 100 + 90 * Math.cos(this.angle(this.downloadSpeed) - Math.PI / 2);
		},
		downloadNeedleY()
		{
			return 100 + 90 * Math.sin(this.angle(this.downloadSpeed) - Math.PI / 2);
		}
	},
	methods: {
		angle(speed)
		{
			return (speed / this.maxSpeed) * Math.PI - (Math.PI / 2);
		},
		formatBits(bits)
		{
			return formatBits(bits);
		}
	},
	template: `
	<div class="speedometer" style="display: flex; align-items: flex-end;">
		<div>0</div>
		<svg :width="size" :height="(size/2)+3" viewBox="0 0 200 100">
			<circle cx="100" cy="100" r="90" stroke="black" stroke-width="2" fill="none" />
			<line :x1="0" :y1="size/2" :x2="size" :y2="size/2" stroke="#999999" stroke-width="2" />
			<line :x1="size/2" :y1="(size/2)-2" :x2="uploadNeedleX" :y2="uploadNeedleY" stroke="#FF8000" stroke-width="4" />
			<line :x1="size/2" :y1="(size/2)-2" :x2="downloadNeedleX" :y2="downloadNeedleY" stroke="#0000FF" stroke-width="4" />
		</svg>
		<div>{{formatBits(maxSpeed)}}ps</div>
	</div>
	`
});
InjectStyleBlock(`
.TestResult
{
	display: flex;
    flex-direction: column;
    align-items: center;
	padding: 20px;
	border: 1px solid black;
}
.TestResultLabel
{
	font-weight: bold;
}
.TestResultValue
{
}
`);
Vue.component('testresult', {
	props:
	{
		label: {
			type: String,
			required: true
		},
		value: null,
		color: {
			type: String,
			default: "#000000"
		},
	},
	template: `
	<div class="TestResult">
		<div class="TestResultLabel" :style="{ color: color }">{{label}}</div>
		<div class="TestResultValue">{{value}}</div>
	</div>
	`
});
Vue.component('cfgoption', {
	props:
	{
		value: {
			required: true
		},
		v: {
			required: true
		},
		labelfn: {
			type: Function,
			default: arg => arg
		}
	},
	template: `
    <input type="button"
      :style="{ opacity: value == v ? 1 : 0.48 }"
	  :value="labelfn(v)"
      @click="updateValue"
    ></input>
  `,
	methods:
	{
		updateValue()
		{
			this.$emit('input', this.v);
		}
	}
});

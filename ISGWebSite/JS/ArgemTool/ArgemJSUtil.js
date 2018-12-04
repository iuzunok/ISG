/// <reference path="Globalization/tr-TR.js" />

// debugger;

if (__argemCultureInfo === undefined)
{
    var __argemCultureInfo = { name: "tr-TR" };
}

var ArgemJSUtil = {
    Theme: 'W7',

    CultureInfo: __argemCultureInfo,

    CI: __argemCultureInfo,

    aryRenkPalet: new Array("#5B90BF", "#96b5b4", "#a3be8c", "#ab7967", "#d08770", "#b48ead", "#000000", "#000033", "#000066", "#000099", "#0000cc", "#003300", "#003333", "#003366", "#003399", "#0033cc", "#006600", "#006633", "#006666", "#006699", "#0066cc", "#009900", "#009933", "#009966", "#009999", "#0099cc", "#00cc00", "#00cc33", "#00cc66", "#00cc99", "#00cccc", "#330000", "#330033", "#330066", "#330099", "#3300cc", "#333300", "#333333", "#333366", "#333399", "#3333cc", "#336600", "#336633", "#336666", "#336699", "#3366cc", "#339900", "#339933", "#339966", "#339999", "#3399cc", "#33cc00", "#33cc33", "#33cc66", "#33cc99", "#33cccc", "#660000", "#660033", "#660066", "#660099", "#6600cc", "#663300", "#663333", "#663366", "#663399", "#6633cc", "#666600", "#666633", "#666666", "#666699", "#6666cc", "#669900", "#669933", "#669966", "#669999", "#6699cc", "#66cc00", "#66cc33", "#66cc66", "#66cc99", "#66cccc", "#990000", "#990033", "#990066", "#990099", "#9900cc", "#993300", "#993333", "#993366", "#993399", "#9933cc", "#996600", "#996633", "#996666", "#996699", "#9966cc", "#999900", "#999933", "#999966", "#999999", "#9999cc", "#99cc00", "#99cc33", "#99cc66", "#99cc99", "#99cccc", "#cc0000", "#cc0033", "#cc0066", "#cc0099", "#cc00cc", "#cc3300", "#cc3333", "#cc3366", "#cc3399", "#cc33cc", "#cc6600", "#cc6633", "#cc6666", "#cc6699", "#cc66cc", "#cc9900", "#cc9933", "#cc9966", "#cc9999", "#cc99cc", "#cccc00", "#cccc33", "#cccc66", "#cccc99", "#cccccc"),

    SayiMi: function (chrKarakter)
    {
        return /^[0-9]*$/.test(chrKarakter);
    },

    HarfMi: function (chrKarakter)
    {
        return /^[a-zA-Z]*$/.test(chrKarakter);
    },

    KeyCodes: {
        BACKSPACE: 8,
        TAB: 9,
        ENTER: 13,
        ESC: 27,
        SPACE: 32,
        END: 35,
        HOME: 36,
        LEFT: 37,
        UP: 38,
        RIGHT: 39,
        DOWN: 40,
        DELETE: 46,
        DECIMAL_POINT: 110,
        F8: 119,
        F9: 120,
        DOT: 190,
        COMMA: 188
    },

    SetCursorPosition: function (oInput, oCursorPosition)
    {
        if (oInput.createTextRange)
        {
            var range = oInput.createTextRange();
            range.move('character', oCursorPosition);
            range.select();
        }
        else if (oInput.selectionStart != null)
        {
            oInput.focus();
            oInput.setSelectionRange(oCursorPosition, oCursorPosition);
        }
        else
        {
            oInput.focus();
        }
    },

    GetCursorPosition: function (oInput)
    {
        if (window.getSelection)
        {
            var i = $(oInput).attr("selectionStart");
            return i;
        }
        else
        {
            if (oInput.createTextRange)
            {
                var r = document.selection.createRange().duplicate();
                r.moveStart('character', -oInput.value.length);
                return r.text.length;
            }
            else
                return oInput.selectionEnd;
        }
    },

    Trim: function (sString)
    {
        while (sString.substring(0, 1) == ' ')
            sString = sString.substring(1, sString.length);
        while (sString.substring(sString.length - 1, sString.length) == ' ')
            sString = sString.substring(0, sString.length - 1);
        return sString;
    },

    ZorunluCssAyarla: function (oInput, isValid)
    {
        if (isValid)
        {
            if (typeof (oInput.InvalidValueCSS) === 'undefined' || oInput.InvalidValueCSS == '')
            {
                $(oInput).css('background-color', $(oInput).data('EskibgColor'));
            }
            else
            {
                $(oInput).removeClass(oInput.InvalidValueCSS);
            }
        }
        else
        {
            if (typeof (oInput.InvalidValueCSS) === 'undefined' || oInput.InvalidValueCSS == '')
            {
                $(oInput).data('EskibgColor', $(oInput).css('background-color'));
                $(oInput).css('background-color', 'Red');
            }
            else
            {
                $(oInput).addClass(oInput.InvalidValueCSS);
            }
        }
    },

    JSonToString: function (o)
    {
        if (typeof (JSON) == 'object' && JSON.stringify)
            return JSON.stringify(o);

        var type = typeof (o);

        if (o === null)
            return "null";

        if (type == "undefined")
            return undefined;

        if (type == "number" || type == "boolean")
            return o + "";

        if (type == "string")
            return ArgemJSUtil.quoteString(o);

        if (type == 'object')
        {
            if (o.constructor === Date)
            {
                var month = o.getUTCMonth() + 1;
                if (month < 10) month = '0' + month;

                var day = o.getUTCDate();
                if (day < 10) day = '0' + day;

                var year = o.getUTCFullYear();

                var hours = o.getUTCHours();
                if (hours < 10) hours = '0' + hours;

                var minutes = o.getUTCMinutes();
                if (minutes < 10) minutes = '0' + minutes;

                var seconds = o.getUTCSeconds();
                if (seconds < 10) seconds = '0' + seconds;

                var milli = o.getUTCMilliseconds();
                if (milli < 100) milli = '0' + milli;
                if (milli < 10) milli = '0' + milli;

                return '"' + year + '-' + month + '-' + day + 'T' +
                    hours + ':' + minutes + ':' + seconds +
                    '.' + milli + 'Z"';
            }

            if (o.constructor === Array)
            {
                var ret = [];
                for (var i = 0; i < o.length; i++)
                    ret.push(ArgemJSUtil.JSonToString(o[i]) || "null");

                return "[" + ret.join(",") + "]";
            }

            var pairs = [];
            for (var k in o)
            {
                var name;
                var _type = typeof k;

                if (_type == "number")
                    name = '"' + k + '"';
                else if (_type == "string")
                    name = ArgemJSUtil.quoteString(k);
                else
                    continue;  //skip non-string or number keys

                if (typeof o[k] == "function")
                    continue;  //skip pairs where the value is a function.

                var val = ArgemJSUtil.JSonToString(o[k]);

                pairs.push(name + ":" + val);
            }

            return "{" + pairs.join(", ") + "}";
        }
    },

    myEval: function (code)
    {
        return eval(code);
    },

    quoteString: function (string)
    {
        var _escapeable = /["\\\x00-\x1f\x7f-\x9f]/g;
        if (string.match(_escapeable))
        {
            var _meta = { '\b': '\\b', '\t': '\\t', '\n': '\\n', '\f': '\\f', '\r': '\\r', '"': '\\"', '\\': '\\\\' };
            return '"' + string.replace(_escapeable, function (a)
            {
                var c = _meta[a];
                if (typeof c === 'string') return c;
                c = a.charCodeAt();
                return '\\u00' + Math.floor(c / 16).toString(16) + (c % 16).toString(16);
            }) + '"';
        }
        return '"' + string + '"';
    },

    BrowserVersiyonBul: function ()
    {
        var ua = navigator.userAgent, tem, M = ua.match(/(opera|chrome|safari|firefox|msie|trident(?=\/))\/?\s*(\d+)/i) || [];
        if (/trident/i.test(M[1]))
        {
            tem = /\brv[ :]+(\d+)/g.exec(ua) || [];
            return { name: 'IE', version: (tem[1] || '') };
        }
        if (M[1] === 'Chrome')
        {
            tem = ua.match(/\bOPR|Edge\/(\d+)/)
            if (tem != null)
            {
                return { name: 'Opera', version: tem[1] };
            }
        }
        M = M[2] ? [M[1], M[2]] : [navigator.appName, navigator.appVersion, '-?'];
        if ((tem = ua.match(/version\/(\d+)/i)) != null)
        {
            M.splice(1, 1, tem[1]);
        }
        return { name: M[0], version: M[1] };
    },

    Init: function (options)
    {
        this.Theme = options.Theme;
    }
};
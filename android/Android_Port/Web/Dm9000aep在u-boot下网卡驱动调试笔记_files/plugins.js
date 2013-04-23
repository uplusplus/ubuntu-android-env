;(function($){
	$.fn.extend({
		selection: function () {
			var selectedValue = '';
			var me = this[0];
			if (document.selection) {
				var range = document.selection.createRange();
				selectedValue = range.text;
			}
			else if (typeof (me.selectionStart) == 'number') {
				var start = me.selectionStart;
				var end = me.selectionEnd;
				if (start != end) {
					selectedValue = me.value.substring(start, end);
				}
			}
			return $.trim(selectedValue);
		},
		parseHtml: function (val) {
			var me = this[0];
			var value = $(this).val();
			if (document.selection) {
				var range = document.selection.createRange();
				if(range.text){
					range.text = val;
				}else{
					$(this).val(value + val);
				}
			}else if (typeof (me.selectionStart) == 'number') {
				var start = me.selectionStart;
				var end = me.selectionEnd;

				var startVal = value.substring(0, start);
				var endVal = value.substring(end);
				$(this).val(startVal + val + endVal);
			}
			else
				$(this).val(value + val);

			me.selectionStart = me.selectionEnd = $(this).val().length;
			me.focus();
		}
	});
})(jQuery);
jQuery.cookie = function(name, value, options) {
    if (typeof value != 'undefined') { // name and value given, set cookie
        options = options || {};
        if (value === null) {
            value = '';
            options.expires = -1;
        }
        var expires = '';
        if (options.expires && (typeof options.expires == 'number' || options.expires.toUTCString)) {
            var date;
            if (typeof options.expires == 'number') {
                date = new Date();
                date.setTime(date.getTime() + (options.expires * 24 * 60 * 60 * 1000));
            } else {
                date = options.expires;
            }
            expires = '; expires=' + date.toUTCString(); // use expires attribute, max-age is not supported by IE
        }
        // CAUTION: Needed to parenthesize options.path and options.domain
        // in the following expressions, otherwise they evaluate to undefined
        // in the packed version for some reason...
        var path = options.path ? '; path=' + (options.path) : '';
        var domain = options.domain ? '; domain=' + (options.domain) : '';
        var secure = options.secure ? '; secure' : '';
        document.cookie = [name, '=', encodeURIComponent(value), expires, path, domain, secure].join('');
    } else { // only name given, get cookie
        var cookieValue = null;
        if (document.cookie && document.cookie != '') {
            var cookies = document.cookie.split(';');
            for (var i = 0; i < cookies.length; i++) {
                var cookie = jQuery.trim(cookies[i]);
                // Does this cookie string begin with the name we want?
                if (cookie.substring(0, name.length + 1) == (name + '=')) {
                    cookieValue = decodeURIComponent(cookie.substring(name.length + 1));
                    break;
                }
            }
        }
        return cookieValue;
    }
};

//创建遮罩层
jQuery.createMask = function () {
    var height = document.documentElement.clientHeight;
    var width = document.documentElement.clientWidth;
    var bodyHeight = $("body").height();

    if (bodyHeight > height) {
        height = bodyHeight;
    }

    var mask = {};
    if ($("#mask_div").length == 0) {
        $("body").append('<div id="mask_div" style="position:absolute;top:0;left:0;filter:alpha(opacity=20);-moz-opacity:0.2;opacity:.2;"></div>')
    }
    mask = $("#mask_div");
    mask.css({ "width": width, "height": height, "background": "#000" });
};

//移除遮罩层
jQuery.removeMask = function () {
    $("#mask_div").remove();
};

Array.prototype.contain = function(val){
	for(var i=0;i<this.length;i++){
		if(this[i] === val){
			return true;
		}
	}
	return false;
}
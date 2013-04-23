var __mm_arr = null;
function printMedal(m_arr) {
    __mm_arr = m_arr;
    var s = "<div id='bms_box'>";
    for (var i = 0; i < m_arr.length; i++) {
        s += "<a href='http://medal.blog.csdn.net/allmedal.aspx' target=_blank>";
        s += "<img src='" + m_arr[i].src + "'";
        s += " onmouseover='m_over_m(this," + i + ")'";
        s += " onmouseout='m_out_m()'";
        s += " alt='' />";
        s += "</a>";
    }
    s += "</div>";
    $('#blog_medal').append($(s));
}
var __mm_over = false;
function m_over_m(e, i) {
    __mm_over = true;
    showMedalInfo(e, i);
}
function m_out_m() {
    __mm_over = false;
    hideMedalInfo();
}
var __mm_intro = null;
function showMedalInfo(e, i) {
    if (__mm_intro) {
        document.body.removeChild(__mm_intro);
        __mm_intro = null;
    }
    var m = __mm_arr[i];
    var s = "";
    s += "<dl onmouseover='__mm_over=true;' onmouseout='m_out_m();'>";
    s += "<dt><img src='" + m.src.replace("_s.gif", "_b.png").replace("_s2.gif", "_b.png") + "' /></dt>";
    s += "<dd><strong>" + m.title + "</strong>" + m.desc + "</dd>";
    s += "</dl>";
    __mm_intro = document.createElement("div");
    __mm_intro.className = "medal_intro";
    var pos = $(e).position();
    var left = pos.left - (306 - 32) / 2;
    if (left < 0) left = 0;
    __mm_intro.style.left = left + 'px';
    __mm_intro.style.top = pos.top + 32 + 4 + 'px';
    __mm_intro.innerHTML = s;
    document.body.appendChild(__mm_intro);
}
function hideMedalInfo() {
    setTimeout(function () {
        if (!__mm_over && __mm_intro) {
            document.body.removeChild(__mm_intro);
            __mm_intro = null;
        }
    }, 500);
}
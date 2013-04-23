/*文章中的插入代码*/
$(document).ready(function () {
    $(".article_content pre").each(function () {
        var $this = $(this);
        if ($this.attr("class").indexOf("brush:") != -1) {
            var lang = $this.attr("class").split(';')[0].split(':')[1];
            $this.attr('name', 'code');
            $this.attr('class', lang);
        }
        if ($this.attr("class")) {
            $this.attr('name', 'code');
        }
    });
    $('.article_content textarea[name=code]').each(function () {
        var $this = $(this);
        if ($this.attr("class").indexOf(":") != -1) {
            $this.attr("class", $this.attr("class").split(':')[0]);
        }
    });
    dp.SyntaxHighlighter.HighlightAll('code');
    //修复旧文章中的高亮
    $('.highlighter').addClass('dp-highlighter');

    /*如果浏览器不支持访问剪切板，就用flash实现*/
    if (!window.clipboardData) {
        setTimeout("setCopyBtn()", 1000);
    }
});
/*使用flash实现复制到剪切板*/
function setCopyBtn() {
    $('.CopyToClipboard').each(function () {

        var clip = new ZeroClipboard.Client();
        clip.setHandCursor(true);
        clip.addEventListener('load', function (client) { });
        clip.addEventListener('mouseOver', function (client) {
            var _c_ode = client.movie.parentNode.parentNode.parentNode.parentNode.nextSibling.innerHTML;
            _c_ode = _c_ode.replace(/&lt;/g, "<").replace(/&gt;/g, ">").replace(/&amp;/g, "&");
            client.setText(_c_ode);
        });
        clip.addEventListener('complete', function (client, text) {
            alert("代码已经复制到你的剪贴板。");
        });
        clip.glue(this, this.parentNode);
    });
}
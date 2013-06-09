/*
 * 【全站通知】全站通知类。
 *  by: zhaoxin@csdn.net
 *  2012-11-30 AM
 */
(function(window) {

  // indexOf for IE8, etc..
  if (Array.prototype.indexOf === undefined) {
    Array.prototype.indexOf = function (e, i) {
      for (i = i || 0; i < this.length; i++) {
        if (this[i] === e) {
          return i;
        }
      }
      return -1;
    };
  }
  // isArray for IE8, etc..
  if (Array.isArray === undefined) {
    Array.isArray = function (o) {
      if (o) {
        return o instanceof Array || Object.prototype.toString.call(o) === '[object Array]' ||
          (o.length === 0 || typeof o.length === 'number' &&  o.length > 0 && (o.length - 1) in o);
      }
      return false;
    };
  }
  // bind for IE8, etc..
  if (Function.prototype.bind === undefined) {
    Function.prototype.bind = function (ctx) {
      var self = this;
      return function () {
        self.apply(ctx, [].slice(arguments, 0));
      };
    };
  }
  
  
    //登录用户对象
  var currUser = {username:''},

    //默认消息服务端后台接口地址
    SERVERHOST = ~location.host.indexOf('local.csdn.net') ?
      'http://local.csdn.net:8081' :
      'http://notify.csdn.net',

    //csdn个人空间地址
    SPACE = "http://my.csdn.net/",
    
    //默认js/css等静态文件地址
    BASEPATH = ~location.host.indexOf('local.csdn.net') ?
      'http://local.csdnimg.cn/rabbit/note/' :
      "http://static.csdn.net/rabbit/note/",

    //存储设置为已读消息的id
    setReadedIds = [],

    //未读消息数量
    unReadedCount = 0,

    //List最多显示条数
    MAXCOUNT = 5,

    //动画中。。。
    inProcess = false,

    //点击消息图标是否触发setReaded,------true：再次点击不再触发setReaded
    isSetReaded = false,

    //是否是从getUnreadedCount接口获得的未读消息数量
    hasNoticeCount = false,
    
    //全局变量
    csdn = window.csdn || {},

    //debug on/off false: 显示样式
    //isDebug = ~location.host.indexOf('local.csdn.net') || location.search=='?smsg=1' ? false : true,
    isDebug = false;
    //在现代浏览器输出debug信息
    log = function() {
      var args = [].slice.call(arguments);
      if(typeof console != 'undefined') {
        //console.log.apply(console, [].slice.call(arguments));
        if(arguments.length>1){
          console.log(arguments[0],arguments[1]);
        }else{
          console.log(arguments[0]);
        }
      }
    },
    
    //socket唯一性控制开关 false=关闭唯一性控制，默认是true
    socketSwitch = false,
    
    /*
     * icons list
     * rev_tick :回帖
     * rev_conn :社交
     * rev_blog ：博客
     * rev_message ：留言
     * re_upres ：系统信息
     */
     icons = ['rev_tick', 'rev_conn', 'rev_blog', 'rev_message', 're_upres'];  
     
  csdn.note = function(conf) {

    //配置项
    this.conf = conf;

    //Dom节点
    this.Dom = {};

    //存储notice内容数组
    this.currData = [];

    //初始化
    this.init.apply(this, []);
  };

  csdn.note.prototype = {
    /*
     * 【初始化入口】
     */
    init: function() {  
      var self = this;
      self.conf.basePath = self.conf.basePath || BASEPATH;
      self.conf.serverHost = self.conf.serverHost || SERVERHOST;
      //检查用户登录  
      self.checkLogin(function(data) {
        log('checkLogin', data);
        //加载socket.io连接处理资源
        self.getSocket(self.conf.basePath + self.conf.multiWinUrl , function(){
          log('getSocket', self.conf.basePath + self.conf.multiWinUrl);
          //初始化消息列表
          self.getDoms(function(){
            //加载样式与事件
            if(!isDebug){
              self.loadCss(self.conf.basePath + self.conf.cssUrl, 'head', function(){});
              self.addEvent();
            }
          });
          
          //建立socket.io连接
          self.keepAlive(function(isMaster){
              //页面初始化获取未读消息数量
              self.getNoticeCount(isMaster,function(data) {
                self.openTip(data.data.count);
              });
          });
        }); 
      });
    },

    /*
    * 异步加载CSS
    */
    loadCss : function(src, target, callback){
        var node = document.createElement('link'),outEl;
        switch (target) {
            case 'body':
                outEl = document.body;
                break;
            case 'head':
                outEl = document.getElementsByTagName('head')[0];
                break;
            default:
                outEl = document.getElementsByTagName('head')[0];
        }
        node.rel = "stylesheet";
        node.type = 'text/css';
        if (node.addEventListener) {
            node.addEventListener('load', callback, false);
            node.addEventListener('error', function () {

            }, false);
        }
        else { // for IE6-8
            node.onreadystatechange = function () {
                var rs = node.readyState;
                if (rs === 'loaded' || rs === 'complete') {
                    node.onreadystatechange = null;
                    callback();
                }
            };
        }
        node.href = src;
        outEl.appendChild(node);
    },

    /*
     * 【UI】获取所有DomHook
     * @param <Object>
     * @param <Object>
     */
    getDoms: function(callback) {
      this.Dom.wrap = this.conf.wrap;
      this.Dom.btn = this.conf.btn;
      var btnLeft = this.Dom.btn.offset().left;
      var btnTop = this.Dom.btn.offset().top;
      this.conf.wrap.hide();
      this.renderPos(btnLeft,btnTop);
      this.Dom.wrap.css('zIndex','999');
      $('<iframe src="about:blank" frameborder="0" style="z-index:-1;position:absolute;top:0;left:0;width:100%;height:100%;background:transparent"></iframe>').appendTo(this.Dom.wrap);
      callback && callback();
      return this;
      
    },

    /*
     * 【UI】获取Dom.btn的位置,渲染Dom.wrap && csdn_notice_tip 的位置
     * @param btnLeft <String> btn可视范围左边距
     * @param btnTop <String> btn可视范围上边距
     */
     renderPos : function(btnLeft,btnTop){ 
        //var t = $.browser.msie ? 5 : 15;       
        var t = 23;
        this.Dom.wrap.css({
          position : "absolute",
          left : btnLeft -215 + 'px',
          top : btnTop + t + 'px',
          zIndex : 9999,
          width : "440px",
        });        
        $('.csdn_notice_tip').css({
          position : "absolute",
          left : btnLeft - 72 + 'px',
          top : btnTop + 22 + 'px'
        });
     },

    /*
     * 【logic】加载socket.io.js
     * @param <Object>
     * @param <Object>
     */
     getSocket : function(url,callback){
        var that = this;
        $.getScript(url,function(){
          if(typeof callback === "function"){
            callback();
          }
        });   

     },

    /*
     * 【UI】
     * @param <Object>
     * @param <Object>
     */
     showNoticeBox : function(){
        var that = this;
        if(!isSetReaded){
          this.getData(function(data){
            that.toggleShow(function() {
              var _wrap = that.Dom.wrap;
              that.Dom.list = _wrap.find(".list");
              that.Dom.content = _wrap.find(".notice_content");
              that.initList();
              that.initDetail();
              that.slideReset();
              if($('.csdn_notice_tip strong').html()*1){

                that.setReaded(that.currData,function(data){
                 $('.csdn_notice_tip strong').html(0);
                });
              } 
              isSetReaded = true;  
            });
          });
        }else{
          if($('.csdn_notice_tip strong').html()*1){
            that.setReaded(that.currData,function(data){
             $('.csdn_notice_tip strong').html(0);
            });
          } 
          this.toggleShow();
        }
     },

    /*
     * 【UI】显示详细项
     * @param <Object>
     * @param <Object>
     */
    addEvent: function() {
      var that = this;
      this.Dom.btn.bind("click", function() {
        if(!isSetReaded){
          that.showNoticeBox();
        }else{
          that.toggleShow(function(){
            var _content = $(that.Dom.wrap).find(".notifications");
            if($(_content[1]).is(':visible')){
              $(_content[1]).css('display','none'), 
              $(_content[0]).css('display','block');
              $('.remove').css("display","block");
            }            
          });
        }
        that.Dom.wrap.find('.unread').removeClass('unread');    
        return false;
      });
      $(window).bind("resize",function(e){
        var btnLeft = that.Dom.btn.offset().left;
        var btnTop = that.Dom.btn.offset().top;
        that.renderPos(btnLeft,btnTop);
        return false;
      });
      $(document).bind("click", function(e) {
        var target = e.target;
        if($(target).parents().filter(that.Dom.wrap).length <= 0) {
          that.Dom.wrap.hide();
          that.Dom.wrap.find('.remove').css('display','');
        }
      });
      this.Dom.wrap.bind("mouseup", function(e) {
        if ((e.which || e.button) === 1) {
          e.preventDefault();
          that.eventHandler(e);
          return false;
        }
      });
      $('.csdn_notice_tip').bind("click",function(e){
        e.preventDefault();
        that.eventHandler(e);
        return false;
      });
      return this;
    },

    
    /*
     * 【UI】事件处理
     * @param <Object>
     * @param <Object>
     */
    eventHandler: function(e) {
      var that = this;
      var el = e.target;
      e.stopPropagation();

      //点击list
      if(wrapList = $(el).hasClass("list")[0] || $(el).parents().filter(".list")[0]) {
        var children = $(wrapList.parentNode).children();
        that.currIndex = 0;
        for(var i = 0, len = children.length; i < len; i++) {
          if(children[i] == wrapList) {
            that.currIndex = i;
            break;
          }
        }
        var i = that.currIndex;

        if($('.csdn_notice_tip strong').html()*1){
          that.setReaded(that.currData, function(data) {
            $(wrapList).removeClass("unread");
            $('.csdn_notice_tip strong').html($('.csdn_notice_tip strong').html()*1-1);
            that.initDetail(i);
            that.setNavBtn(i);
            var _content = $(that.Dom.wrap).find(".notifications");
            that.goSlide($(_content[0]), $(_content[1]));
          });
        }else{
          $(wrapList).removeClass("unread");
          that.initDetail(i);
          that.setNavBtn(i);
          var _content = $(that.Dom.wrap).find(".notifications");
          that.goSlide($(_content[0]), $(_content[1]));
        }
        
        $('.remove').css("display","none");
        return;
      }

      //点击关闭
      if($(el).hasClass("remove")) {
        this.doClose();
        return;
      }

      //点击返回
      if($(el).hasClass("go_back")) {
        var _content = $(that.Dom.wrap).find(".notifications");
        this.goSlide($(_content[1]), $(_content[0]));
        $('.remove').css("display","block");
        return;
      }

      //点击上一条
      if($(el).hasClass("prvnote") && !($(el).hasClass("disabled"))) {
        that.prv();
        return;
      }

      //点击下一条
      if($(el).hasClass("nextnote") && !$(el).hasClass("disabled")) {
        that.next();
        return;
      }

      // 关闭消息提示层
      if($(el).hasClass("remove2")){
        $('.csdn_notice_tip').css('display','none');
        that.getData(function(data){
          that.setReaded(that.currData,function(){

          });
        });
        return;
      }

      // 点击消息提示层，打开通知列表
      if($(el).hasClass("tip_text")){
        isSetReaded = false;
        this.Dom.btn[0].click();
        return;
      }

    },
    goSlide: function(from, to) {
      var that = this;
      var _wrap = this.conf.wrap;
      var fwidth = $(from).width();
      var twidth = $(to).width();
      var fromH = from.height();
      var posLeft = "-" + fwidth;
      var posCurr = 0;
      var posRight = fwidth;

      _wrap.height(to.height());
      var isNext = false;
      from.css({
        position: "relative",
        top: 0,
        left: posCurr
      });

      //查找from以后的节点，看看是否包含from,判断方向

      function setPos(el, pos, width) {
        if(inProcess) return;
        var _pos = {
          left: function() {
            el.css({
              position: "absolute",
              width: fwidth,
              top: 0,
              left: "-" + width + "px"
            });
          },
          center: function() {
            el.css({
              position: "relative",
              width: fwidth,
              top: 0,
              left: 0
            });
          },
          right: function() {
            el.css({
              position: "absolute",
              width: fwidth,
              top: 0,
              left: width
            });
          }
        }
        _pos[pos]();
      }

      //向下一个滚动
      if($(from).nextAll().filter(to).length) {
        setPos(from, "center", fwidth);
        setPos(to, "right", fwidth);
        var targetPos = posLeft + "px";
      }

      //向上一个滚动
      else {
        setPos(from, "center", fwidth);
        setPos(to, "left", fwidth);
        var targetPos = posRight + "px";
      }
      from.parent().children().removeClass("curr");
      from.show();
      to.addClass("curr");
      to.show("",function(){
        _wrap.height(to.height());
      });
      _wrap.height(to.height());
      inProcess = true;
      var speed = 110;
      from.animate({
        left: targetPos
      }, speed, function() {
        from.removeAttr("style");
        from.hide();
        to.animate({
          left: 0
        }, speed, function() {
          to.siblings().hide();
          to.removeAttr("style");
          _wrap.height("");
          inProcess = false;
        });
      });


      to.animate({
        left: 0
      }, speed, function() {
        to.siblings().hide();
        to.removeAttr("style");
        _wrap.height("");
        inProcess = false;
      });
      inProcess = false;
    },

    slideReset: function() {
      inProcess = false;
      var curr = $(this.conf.wrap.find(".notifications")[0]);
      curr.addClass("curr");
      curr.nextAll().hide();
      curr.show();
      this.conf.wrap.scrollLeft(0);
      this.setOverFlow();
    },
    /*
     * 【UI】提供主模板
     * @param <Object>  是否是列表容器
     * @param <Object>  数据对象
     * @param <STRING>  内容列表
     * @param <STRING>  顶部导航
     */
    renderTpl: function(islist, data, list, nav) {
      var _data = data;
      nav = nav || '<a href="' + this.conf.serverHost + '/dashboard" target="_blank" class="go_all">查看所有通知</a>';
      var wrapClass = (islist) ? "notice_list_con" : "detail_con";

      list = list || this.renderListTpl(_data);
      //页面模板
      var tpl = '\
          <div class="notifications ' + wrapClass + '">\
          <div class="menu_title">\
                <span class="title">\
                ' + nav + '\
                </span>\
          </div>\
          <div class="notice_content">\
          ' + list + '\
          </div>\
        </div>';

      return tpl;
    },

    /*
     * 【UI】提供list模板
     * @param <Object>
     * @param <Object>
     */
    renderListTpl: function(data) {
      //list模板

      if(!data.length){
        return '<div class="nothing">暂没有新通知</div>';
      }
      function getTpl(item) {
        var title = item.title,
          time = item.time,
          type = item.type,
          id = item.id;
        var unreadClass = "";
        unreadClass = (!item.details.isReaded || item.details.isReaded == '0') ? " unread " : "";
        var tpl = '\
            <dl class="list ' + (icons[type] || 'rev_type' + type) + unreadClass + '">\
                <dt>\
                <i></i>\
                <a href="javascript:void(0);"><span class="count_down">' + time + '</span>' + title + '</a>\
                </dt>\
            </dl>';
        return tpl;
      }
      var tpl = "";
      var i = 0,
        len = data.length,
        item;
      while(i < len) {
        item = data[i];
        tpl += getTpl(item);
        i++;
      }
      return tpl;
    },

    /*
     * 【UI】提供item模板
     *
     *1.没有title的同时没有图片的，不显示主标题的时间
     *2.没有imgUrl且没有body的，只显示一条合并后的消息
     *（p.s:蛋疼的规则，我也不想这么写，产品线说，"这样使用起来方便"，于是暂时迁就，忍不了再改吧。）
     * @param <Object>
     * @param <Object>
     */
    renderItemTpl: function(data, index, curr) {
      var _this = this;
      var tpl = "";
      var i = 0,
        _data = data.items,
        len = _data.length,
        item;
      
      while(i < len) {
        item = _data[i];
        tpl += getTpl(item);
        i++;
      }

      var type = data.type;
      
      /*
       没有title的同时没有图片的，不显示主标题的时间
      （p.s:蛋疼的规则，我也不想这么写，但产品线说，"这样使用起来方便"，于是暂时迁就，忍不了再改吧。）
      */
      var _time = (!data.items[0].title&&!data.items[0].imgurl)?data.items[0].time:"";
      var header = '<dt><i></i><span class="item_title">' + data.title + '</span><span class="count_down">'+_time+'</span></dt>';
      var remain = data.remain || "0";
      var url = data.url || "javascript:void(0)";
      var isCurr = curr == index ? 'class="curr"' : '';
      var more = "";
      if(!data.items[0].content || !data.remain  || !data.type) {
        more = '';
      }else{
        more = '<a class="notifications_moer" target="_blank" href="' + url + '">查看其它 ' + remain + ' 条</a>';
      }
      
      tpl = '<div ' + isCurr + '><dl class="' + (icons[type] || 'rev_type' + type) + '">' + header + tpl + '</dl>' + more + '</div>';

      //list模板
      function getTpl(item) {
        
        //没有imgUrl且没有body的，只显示一条合并后的消息
        if(!data.imgUrl&&!data.items[0].content){
          return '';
        }
        
        var fromuser = item.fromuser;
        var fromuserLink = '<a href="' + SPACE + fromuser + '" target="_blank" class="usrlink">' + fromuser + '</a>';
        var content = item.content,
          time = item.time,
          id = item.id,
          tpl="";
        
        //如果有imgurl，则模板为图文显示
        if(item.imgurl){
          var img = !item.imglink?'<img src="'+item.imgurl+'" height="32" width="32"/>':'<a href="'+item.imglink+'" target="_blank"><img src="'+item.imgurl+'" height="32" width="32"/></a>';
          var imgtitle = item.imgtitle || "";
          tpl = '\
            <dd class="item" lang = "' + id + '">\
                <span class="count_down">' + time + '</span>' + img + imgtitle + '\
            </dd>';
            return tpl;
        }
        // 对BBCODE语法进行转换
        content = _this.ubbDecode(content);
        tpl = '\
          <dd class="item" lang = "' + id + '">\
              <div class="notice_txt">' + fromuserLink + '<span class="fenge">:</span>' + content + '</div><span class="count_down">' + time + '</span>\
          </dd>';
        return tpl;
      }

      return tpl;
    },

    /*
     * 【UI】设置导航
     * @param <Object>
     * @param <Object>
     */
    renderNav: function(isDetail) {
      isDetail = isDetail || "true";
      var _details;
      if(isDetail) {
        _details = '\
          <a class="go_back" href="javascript:void(0);">返回通知列表</a>\
          <a class=" notifications_page_none nextnote" href="javascript:void(0);">下一条</a>\
          <a class=" notifications_page prvnote" href="javascript:void(0);">上一条</a>\
          ';

      } else {
        _details = '<a href="javascript:void(0)">查看所有通知</a></span>';
      }
      return _details;
    },

    /*
     * 【UI】点击关闭
     * @param <Object>
     * @param <Object>
     */
    doClose: function() {
      this.Dom.wrap.css('display','none');
    },

    /*
     * 【UI】点击 显示/关闭
     * @param <Object>
     * @param <Object>
     */
    toggleShow: function(callback) {
      var that = this;
      if($('.csdn_notice_tip').is(':visible')){
        $('.csdn_notice_tip').hide();
      } 
      this.Dom.wrap.toggle();
      if(typeof callback == 'function') {
        callback();
      }
      return false;
    },

    /*
     * 【UI】设置上一条，下一条按钮
     * @param <Object>
     * @param <Object>
     */
    setMainBtn: function() {
      var num = 0;
      var len = this.currData.length;
      while(len--) {
        if(this.currData[len].details.isReaded === 0) {
          num++;
        }
      }
        
      this.openTip(num);
      
      
    },

    /*
     * 【UI】渲染新消息弹出提示层
     * @param <number> 新收到的消息数量
     * 
     */

    openTip : function(num){
      //num += $('.csdn_notice_tip strong').html()*1 ;
      $('.csdn_notice_tip strong').html(num);
      if(this.conf.wrap.is(':visible')){
        return;
      }
      if(num && !isDebug){
        $('.csdn_notice_tip').css('display','block');
      }else{
        $('.csdn_notice_tip').css('display','none');
      }
      
      

    },

    /*
     * 【UI】设置上一条，下一条按钮
     * @param <Object>
     * @param <Object>
     */
    setNavBtn: function(i) {
      var index = this.currIndex = i;
      var prv = this.Dom.wrap.find(".prvnote");
      var next = this.Dom.wrap.find(".nextnote");
      this.isEnd(index, {
        top: function() {
          //禁用上一条
          prv.addClass("disabled");
          next.removeClass("disabled");
        },
        middle: function() {
          //解除禁用
          next.removeClass("disabled");
          prv.removeClass("disabled");
        },
        end: function() {
          //禁用下一条
          next.addClass("disabled");
          prv.removeClass("disabled");
        },
        only: function() {
          // 只有一条
          next.addClass("disabled");
          prv.addClass("disabled");
        }
      });
    },

    /*
     * 【UI】超出n条后，出滚动条
     * @param <NUMBER> index
     * @param <Object> 回调对象
     */
    setOverFlow: function(n) {
       n = n || MAXCOUNT;
       var oContent = $(this.Dom.wrap.find(".notice_content")[0]);
       var len = oContent.children().length;
       var Jsoncss = {},isOverFolw = len>n-1;
          Jsoncss = {
            "overflow":isOverFolw?"auto":"",
            "height":isOverFolw? (oContent.height()? oContent.height() :"255px"):""

          }       
       oContent.css(Jsoncss);
    },

    /*
     * 【LOGIC】判断是否是上一条并执行相应回调
     * @param <NUMBER> index
     * @param <Object> 回调对象
     */
    isEnd: function(i, conf) {
      var _data = this.currData;
      this.isEndTop = false;

      if((i == 0) && (i == _data.length - 1)) {
        conf.only();
        this.isEndTop = true;
      } else if(i == 0) {
        conf.top();
        this.isEndTop = true;
      } else if(i == _data.length - 1) {
        conf.end();
        this.isEndTop = true;
      } else {
        conf.middle();
        this.isEndTop = false;
      }
    },

    /*
     * 【LOGIC】上一条
     * @param <Object>
     * @param <Object>
     */
    prv: function() {
      if(inProcess) {
        return
      }
      var _currIndex = this.currIndex;
      var i = this.currIndex - 1 ;
      this.setNavBtn(i);
      if(this.Dom.wrap.find('.notice_list_con  dl:nth-child('+(i+1)+')').hasClass('unread')){
        this.Dom.wrap.find('.notice_list_con  dl:nth-child('+(i+1)+')').removeClass('unread');
      }  
      
      var _content = this.conf.wrap.find(".detail_con .notice_content");
      _content.css({
        position: "relative"
      });
      var _list = _content.children();
      var from = $(_list[_currIndex]);
      var to = $(_list[i]);
      this.goSlide(from, to);
    },

    /*
     * 【LOGIC】下一条
     * @param <Object>
     * @param <Object>
     */
    next: function() {
      if(inProcess) {
        return
      };
      var _currIndex = this.currIndex;
      var i = this.currIndex + 1;
      this.setNavBtn(i);
      if(this.Dom.wrap.find('.notice_list_con  dl:nth-child('+(i+1)+')').hasClass('unread')){
        this.Dom.wrap.find('.notice_list_con  dl:nth-child('+(i+1)+')').removeClass('unread');
      }  
            
      var _content = this.conf.wrap.find(".detail_con .notice_content");
      _content.css({
        position: "relative"
      });
      var _list = _content.children();
      var from = $(_list[_currIndex]);
      var to = $(_list[i]);
      this.goSlide(from, to);
    },

    /*
     * 【LOGIC】显示详细项
     * @param <Object>
     * @param <Object>
     */
    initList: function(i) {
      var data = this.currData;
      var wrap = this.conf.wrap;
      var listCon = wrap.find(".notice_list_con");
      if(listCon.length > 0) {
        $(this.renderTpl(1, data)).replaceAll(listCon);
      } else {
        wrap.find('.box').append($(this.renderTpl(1, data)));
      }
    },

    /*
     * 【LOGIC】显示详细项
     * @param <Object>
     * @param <Object>
     */
    initDetail: function(i) {
      i = i || "0";
      i *= 1;
      var data = this.currData,
        el = this.conf.wrap.find('.notifications'),
        nav = this.renderNav();
      var wrap = this.conf.wrap;
      var j = 0,
        len = data.length,
        detail = "";
      while(j < len) {
        detail += this.renderItemTpl(data[j].details, j, i);
        j++;
      }
      var tpl = $(this.renderTpl('', data, detail, nav));
      var detailCon = wrap.find(".detail_con");
      if(detailCon.length > 0) {
        tpl.replaceAll(detailCon);
      } else {
        wrap.find('.box').append($(this.renderTpl('', data, detail, nav)));
      }

      content = this.conf.wrap.find(".notice_content");
      wrap.find(".detail_con .curr").siblings().hide();
      
    },

    /*
     * 【LOGIC】添加一条信息
     * @param <array> data 消息数组
     * @param <Object>
     */
    addItem: function(data) {
        this.currData = data.concat(this.currData);

    },


    /*
     * 【LOGIC】显示详细项
     * @param <Object>
     * @param <Object>
     */
    addMoreItemsBtn: function() {

    },

    /*
     * 【LOGIC】检查登录
     */
    checkLogin: function(callback) {
      var that = this;
      var url = "http://ptcms.csdn.net/comment/js/login.js";
      $.getScript(url, function(data) {
        currUser.username = csdn.getCookie("UserName") || currUser.username;
        currUser.userInfo = csdn.getCookie("UserInfo") || currUser.userInfo;
        var data = currUser.username ? true : false;
        that.setLogin(currUser.username,function(){
          if(data&& typeof callback ==="function"){
            callback(data);
          }
          
        });
      });
    },

    /*
     * 【LOGIC】设置登录
     */
    setLogin: function(nickname, callback) {
      nickname = nickname || currUser.username;
      if(typeof callback === "function"){
        callback();
      }
      
    },

    /*
     * 【DATA】先打开的页面建立真正的socket.io连接，并作为其他页面即时消息中转的服务端
     */
    openRealSocket : function() {
      var socket = io.connect(this.conf.serverHost, {
          'force new connection': true
          , reconnect: false
          , transports: ["xhr-polling", "jsonp-polling"]
        })
        , nickname = currUser.username
        , _this = this
        , callee = arguments.callee.bind(this)
        ;

      log('connect');
      socket.on('connect', function() {
        log('connected');
      });
       
      socketSwitch && _this.multiSocket.setMsg('&&' + new Date().getTime() + '&&' + unReadedCount);

      socket.on(nickname, function (msg) {

        log('nickname',msg);
        var socketData = "";
        if (typeof msg === 'string') {
           socketData = msg+'&&'+new Date().getTime();
            
            msg =  $.parseJSON(msg);
            if(msg.setReaded){
              _this.onSetReaded(msg.setReaded);
              socketData += '&&'+ unReadedCount;
            }else{
              socketData += '&&'+(++unReadedCount);
              _this.onNickname(msg);
              _this.openTip(unReadedCount);
            }
            _this.multiSocket.setMsg(socketData);
        } 
      }).on('error', function (err) {
        log('error: ' + err);
      }).on('disconnect', function () {
        log('disconnect');
      });        
    },

     /*
     * 【LOGIC】打开消息列表，设置即时消息为已读
     */
     onSetReaded : function(msg){
        //var unReadedCount = $('.csdn_notice_tip strong').html()*1;
        if(!msg || msg.length == 0){
          unReadedCount = 0;
          $('.csdn_notice_tip').css('display','none');
          $('.csdn_notice_tip strong').html(unReadedCount);
          return;
        }
        var data = this.currData;
        var len = data.length;
        var arrIds = [];
        var _this = this;

        $( data ).each( function ( i, obj ) {
          if ( obj.details.isReaded == undefined || obj.details.isReaded == 0)
          {
            $( obj.details.items ).each( function ( k, item ) {
              ( item.status == undefined || item.status == 0 ) && arrIds.push( item.id );
            } )
          }
        } );

        $.each(msg,function(i,value){
          var n = $.inArray(value,setReadedIds);
          if(n>-1){
            setReadedIds.splice(n,1);
          }
          unReadedCount--;
        });
        unReadedCount = unReadedCount <0 ? 0 : unReadedCount;
        if(!unReadedCount){
          $('.csdn_notice_tip').css('display','none');
        }
        $('.csdn_notice_tip strong').html(unReadedCount);

        //set cookie unReaded
        _this.multiSocket.setMsg('&&'+new Date().getTime()+'&&'+unReadedCount);
     },


     /*
     * 【LOGIC】处理即时消息
     */
     onNickname : function(msg,cbUnReadCount){
        var _this = this;
        _this.addItem(msg);
        log(unReadedCount);
        _this.Dom.wrap.find('.nothing').remove();
        var listItemHTML = $(_this.renderListTpl(msg));
        var oContent = $(_this.Dom.wrap.find(".notice_content")[0]);
        if(!oContent.children().length){
          oContent.append(listItemHTML);
        }else{
          listItemHTML.insertBefore(oContent.children()[0]);
        }
        
        listItemHTML.hide().slideDown("fast",function(){
          if(_this.Dom.wrap.is(":visible")){
            _this.setReaded(_this.currData,function(){
              var num = unReadedCount-1;
              $('.csdn_notice_tip strong').html(num);
            });
            
          }
        });
        _this.setOverFlow();
     },

     /*
     * 【DATA】非当前页面从cookie获取即时消息
     */
     openVirtulSocket : function(msg,unreadcount){
        if(!msg){
            unReadedCount = unreadcount*1|| 0;
            /*if(!unReadedCount){
              $('.csdn_notice_tip').css('display','none');
            }*/
            //$('.csdn_notice_tip strong').html(unReadedCount);
            this.openTip();
            return;
        }
        if(msg.setReaded){
          this.onSetReaded(msg.setReaded);
        }else{
          unReadedCount = unreadcount*1|| 0;
          this.onNickname(msg);
          this.openTip(unReadedCount);
        }
     },


    /*
     * 【DATA】建立长连接
     */
    keepAlive :  function(callback){
      var _this = this;
      //是否建立socket.io连接的主页面
      var isMaster = false;
      var nickname = currUser.username;
      log('keepAlive');
      this.multiSocket = new csdn.multiSocket({
        url: _this.conf.basePath + _this.conf.socketUrl,
        serverHost : this.conf.serverHost,
        socketSwitch : socketSwitch,
        masterEvent : function(){
          isMaster = true;
          log('I\'m maseter ----------');
          _this.openRealSocket.apply(_this);
          callback && callback(isMaster);
        },
        slaveEvent : function(msg,unreadcount){
          log("I'm  slave ----------");
          log("slaveEvent---msg-------------",msg);
          isMaster = false;
          _this.openVirtulSocket.apply(_this,[msg,unreadcount]);
          callback && callback(isMaster); 
        },
        setReadedEvent: function(){
          $.each(_this.currData, function() { this["details"]["isReaded"] = 1; });
          _this.openTip(0);
        }
      });
       
    },

    /*
     * 【DATA】获取全部未读数据的具体内容
     * @param <Object>
     * @param <Object>
     */
    getData: function(callback) {
      var that = this,
        username = currUser.username,
        userinfo = currUser.userInfo,
        count = this.conf.count || 5,
        subCount = this.conf.subCount || 5,
        _url = this.conf.serverHost + "/get_all?username=" + username + "&count=" + count + "&subcount=" + subCount;
      $.ajax({
        url: _url,
        dataType: "jsonp",
        jsonp: "jsonpcallback",
        scriptCharset: "utf-8",
        success: function(data) {
          that.currData = data.data;
          var arrIds = [];
          var len = data.data.length;
          if(typeof callback == "function" && data.status == 200) {
            callback(data);
          }
        }
      });
    },

    /*
     * 【DATA】获取全部未读数据的条数
     * @param <Object>
     * @param <Object>
     */

     getNoticeCount : function(isMaster,callback){
       if(!isMaster || unReadedCount){
        //ToDo : get from cookie
        callback({"status":200,"data":{"count":unReadedCount}});
        return;
       }
        var  that = this,
          username = currUser.username,
          userinfo = currUser.userinfo,
          _url = this.conf.serverHost + '/get_unread_count?username=' + username;
        $.ajax({
          url : _url,
          dataType: "jsonp",
          jsonp: "jsonpcallback",
          scriptCharset: "utf-8",
          success : function(data){
            if(typeof callback == "function" && data.status == 200){
              log('getNoticeCount_callback');
              
              unReadedCount = data.data.count;
              if(unReadedCount){
                hasNoticeCount = true;
              }
              //socket唯一控制开关
              socketSwitch&&that.multiSocket.setMsg('&&'+new Date().getTime()+'&&'+unReadedCount);
              callback(data);
            }
          }
        });
     },

    /*
     * 【DATA】设置已读
     * @param <Object>
     * @param <Object>
     */
    setReaded: function(data, callback,allSetReaded) {
      //todo
      var that = this,
        username = currUser.username,
        userinfo = currUser.userInfo,
        arrIds = [],
        allSetReaded = allSetReaded || false,
        len = data.length;

      $( data ).each( function ( i, obj ) {
          if ( obj.details.isReaded == undefined || obj.details.isReaded == 0)
          {
            $( obj.details.items ).each( function ( k, item ) {
              ( item.status == undefined || item.status == 0 ) && arrIds.push( item.id );
            } )
          }
        } );

        $.each(arrIds,function(i,value){
          if($.inArray(value,setReadedIds)<0){
            setReadedIds.push(value);
          }
        });

      var ids = allSetReaded ? setReadedIds.join(',') : '';     
      var _url = this.conf.serverHost + "/set_readed?username=" + username + "&ids=" + ids;
      $.ajax({
        url: _url,
        dataType: "jsonp",
        jsonp: "jsonpcallback",
        scriptCharset: "utf-8",
        success: function(data) {
          if(typeof callback == "function" && data.status == 200) {
            $.each(that.currData, function(i,k) { 
              this["details"]["isReaded"] = 1;
              //unReadedCount--;
            });
            callback && callback(data);
            that.multiSocket.setMsg('&&'+new Date().getTime()+'&&'+unReadedCount);
          }
        }
      });

    },

    /*
    * UBB转义
    */
    ubbDecode : function(content){
    var _this = this;
    content = $.trim(content);
      var re = /\[code=([\w#\.]+)\]([\s\S]*?)\[\/code\]/ig;

      function replaceQuote(str) {
        var m = /\[quote=([^\]]+)]([\s\S]*)\[\/quote\]/gi.exec(str);
        if (m) {
            return str.replace(m[0], '<fieldset><legend>引用“' + m[1] + '”的评论：</legend>' + replaceQuote(m[2]) + '</fieldset>');
        } else {
            return str;
        }
      }
      var codelist = [];
      while ((mc = re.exec(content)) != null) {
          codelist.push(mc[0]);
          content = content.replace(mc[0], "--code--");
      }
      content = replaceQuote(content);
      content = content.replace(/\[reply]([\s\S]*?)\[\/reply\][\r\n]{0,2}/gi, "回复$1：");
      content = content.replace(/\[url=([^\]]+)]([\s\S]*?)\[\/url\]/gi, '<a href="$1" target="_blank">$2</a>');
      content = content.replace(/\[img(=([^\]]+))?]([\s\S]*?)\[\/img\]/gi, '<img src="$3" style="max-width:200px;max-height:100px;" border="0" title="$2" />');
      content = content.replace(/\r?\n/ig, "<br />");

      if (codelist.length > 0) {
          var re1 = /--code--/ig;
          var i = 0;
          while ((mc = re1.exec(content)) != null) {
              content = content.replace(mc[0], codelist[i]);
              i++;
          }
      }
      content = content.replace(/\[code=([\w#\.]+)\]([\s\S]*?)\[\/code\]/ig, function (m0, m1, m2) {
          if ($.trim(m2) == "") return '';
          return '<pre name="code2" class="' + m1 + '">' + _this.HTMLEncode(m2) + '</pre>';
      });
      
      content = content.replace(/(<br\s\S*>|<br>)/ig, function (m0) {
          if ($.trim(m0) == "") return '';
          //return _this.HTMLEncode(m0);
          return "&nbsp;&nbsp;";
      });
      //针对转义的"做处理
      content = content.replace(/(\\&quote\;|\&quote\;)/ig, function (m0) {
          if ($.trim(m0) == "") return '';
          //return _this.HTMLEncode(m0);
          return "";
      });   
      return content;
    },
    HTMLEncode : function(str) {
      var s = "";
      if(str.length == 0) return "";
      s = str.replace(/&/g, "&amp;");
      s = s.replace(/</g, "&lt;");
      s = s.replace(/>/g, "&gt;");
      s = s.replace(/\'/g, "&#39;");
      s = s.replace(/\"/g, "&quot;");
      return s;
   }
  };

  //公开CSDN 对象
  window["csdn"] = csdn;
})(window);

(function($, window, undefined) {
  if ($ === undefined) {
    // 按需加载jQuery
    var done = false
      , callback = arguments.callee
      , script = document.createElement('script')
      , head = document.getElementsByTagName('head')[0] || document.documentElement
      ;
    script.src = 'http://csdnimg.cn/www/js/jquery-1.4.2.min.js';
    script.charset = 'utf-8';
    script.onload = script.onreadystatechange = function () {
      if(!done && (!this.readyState || this.readyState === 'loaded' || this.readyState === 'complete')) {
        done = true;
        try {
          callback(window.jQuery, window);
        } catch(e) {
          window.console && window.console.log(e);
        }

        script.onload = script.onreadystatechange = null;
        if(head && script.parentNode) {
          head.removeChild(script);
        }
      }
    };
    head.insertBefore(script, head.firstChild);
    return;
  }

  var script = $("#noticeScript")
    , btnId =  script.attr("btnId")||"header_notice_num"
    ;
  if(!$("#" + btnId).length){
    return false;
  }

  var instance = script.attr("instance") || "csdn_note"
    , wrapId = script.attr("wrapId") || "note1"
    , count = script.attr("count") || 5
    , subCount = script.attr("subcount") || 5
    , basePath = script.attr("basepath")
    , serverHost = script.attr("serverhost")
    , stamp = Math.floor(new Date()/120000).toString(36) + '=';
    ;
  
  if(instance === "csdn_note") {
    // <caoyu@csdn.net>
    // 在 body 标签未解析结束前使用 appendTo('body') 在IE6下会发生操作被终止的错误（页面会转到错误页面）
    // 换成 insertBefore 可以避免这个现象，下同
    $('<div id="note1" class="csdn_note"><span class="notice_top_arrow"><span class="inner"></span></span><a href="javascript:void(0)" class="remove"></a><div class="box"></div></div>').insertBefore(script);
  }
  $('<div class="csdn_notice_tip" style="z-index:999"><iframe src="about:blank" frameborder="0" scrolling="no" style="z-index:-1;position:absolute;top:0;left:0;width:100%;height:100%;background:transparent"></iframe><div class="tip_text">您有<strong>0</strong>条新通知</div><a href="javascript:void(0)" class="remove2"></a></div>').insertBefore(script).hide();
  
  window[instance] = new csdn.note({
    btn: $("#"+btnId)
    , wrap: $("#"+wrapId)
    , count: count
    , subCount: subCount
    , basePath: basePath
    , serverHost: serverHost
    , cssUrl: 'css/style.css?' + stamp
    , socketUrl: 'js/socket.io.js?' + Math.floor(new Date()/(24 * 60 * 60 * 1000)).toString(36) + '='
    , multiWinUrl: 'js/mutilwin_socket.js?' + stamp
  });
})(window.jQuery, window);

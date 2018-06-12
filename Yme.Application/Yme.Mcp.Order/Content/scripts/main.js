(function (window, $) {
    /**
 	 * 清空表单值
 	 * @return {[type]} [description]
 	 */
    ! function () {
        $(".clear_input").on("click", function (ev) {
            ev.preventDefault();
            var id = $(this).attr("data-for");
            $("#" + id).val("");
        });
    }();

    /**
 	 * 获取验证码
 	 * @return {[type]} [description]
 	 */
    ! function () {
        $("#get_verify_code").on("click", function (ev) {
            ev.preventDefault();
            if ($(this).attr("data-loading") === "0") {
                $(this).attr("data-loading", "1");
                var url = $(this).attr("data-url");
                var phone = $(this).attr("data-phone");
                var imgcode = $(this).attr("data-imgcode");
                var phone_number = $("#" + phone).val();
                var img_code = imgcode ? $("#" + imgcode).val() : true;
                var self = this;
                var time = 120;
                var intval_id = 0;
                var r = /^1[3-9][\d]{9}$/;
                var _r = true;
                if (!r.test(phone_number)) {
                    $("#" + phone + "_err").removeClass("hide");
                    $(this).attr("data-loading", "0");
                    _r = false;
                } else {
                    $("#" + phone + "_err").addClass("hide");
                }
                if (!img_code) {
                    $("#" + imgcode).val("").attr("placeholder", "请填写图形验证码");
                    $(this).attr("data-loading", "0");
                    _r = false;
                }
                if (_r) {
                    $.ajax({
                        "url": url,
                        type: "post",
                        dataType: "json",
                        data: {
                            mobile: phone_number,
                            verifyCode: img_code
                        },
                        success: function (res) {
                            if (res.status == "1") {
                                $(self).text(time + "后重新获取");
                                $(self).addClass("active");
                                intval_id = setInterval(function () {
                                    resetText();
                                }, 1000);
                            } else {
                                $(self).attr("data-loading", "0");
                                _r = false;
                                alert(res.data);
                            }
                        }
                    });
                }

                function resetText() {
                    time--;
                    $(self).text(time + "后重新获取");
                    if (time === 0) {
                        $(self).removeClass("active");
                        $(self).text("获得验证码");
                        $(self).attr("data-loading", 0);
                        clearInterval(intval_id);
                    }
                }
            }
        });
    }();

    /**
 	 * 同意协议
 	 * @return {[type]} [description]
 	 */
    ! function () {
        $("#argeement").on("click", function (ev) {
            var for_id = $(this).attr("data-for");
            var icon_id = $(this).attr("data-icon");
            var check = $(this).attr("data-agree");
            $("#" + for_id).val(check);
            if (check === "1") {
                $(this).attr("data-agree", 0);
                $("#" + icon_id).removeClass("icon-refuse").addClass("icon-agree");
            } else {
                $(this).attr("data-agree", 1);
                $("#" + icon_id).removeClass("icon-agree").addClass("icon-refuse");
            }
        });
    }();

    /**
 	 * 倒计时
 	 * @return {[type]} [description]
 	 */
    ! function () {
        function _countDownAction(starttime, endtime, elem, i) {
            var endtime = endtime;
            var time = endtime - starttime;
            var elem = elem;
            var i = i;
            this.callback = function () {
                time--;
                var t = "剩余时间 ";
                if (time >= 0) {
                    var h = Math.floor((time / 3600));
                    var m = Math.floor((time - (h * 3600)) / 60);
                    var s = time - ((h * 3600) + (m * 60));
                    var d = Math.floor(h / 24);

                    t += d > 0 ? d + ":" + (h - (d * 24)) + ":" + m + ":" + s : h + ":" + m + ":" + s;
                } else {
                    t = "结束了";
                    _countDown.fors[i].over = 1;
                    _countDown.elems[i].over = 1;
                    $(_countDown.fors[i]).addClass("btn-disabled");
                }
                $(elem).text(t);
                return time;
            }
        }

        var _countDown = {
            intvalid: 0,
            items: [],
            elems: $(".count_down"),
            fors: $(".count_down_disable"),
            init: function () {
                $(".count_down").forEach(function (item, i) {
                    if (item.over) {
                        return false;
                    }
                    var time = $(item).attr("data-time");
                    _countDown.items.push(new _countDownAction(Math.round(Date.now() / 1000), parseInt(time), item, i));
                });
                this.start();
            },
            start: function () {
                _countDown.intvalid = setInterval(function () {
                    var i = 0;
                    var t = 0;
                    for (; i < _countDown.items.length; i++) {
                        t = _countDown.items[i].callback();
                        if (t <= 0) {
                            _countDown.items.splice(i, 1);
                        }
                    }
                    if (!_countDown.items.length) {
                        _countDown.end();
                    }
                }, 1000);
            },
            end: function () {
                clearInterval(this.intervalid);
            }
        }
        _countDown.init();
        window.countDown = {
            reset: function () {
                _countDown.init();
            }
        }
    }();

    var _ajaxLoading = {
        elem: document.querySelector("#text_loading"),
        options: {
            url: "",
            page: 0,
            type: "get",
            dataType: "html",
            data: {},
            success: function (res) { }
        },
        init: function (config) {
            var k;
            for (k in config) {
                if (this.options.hasOwnProperty(k)) {
                    this.options[k] = config[k];
                }
            }
            this.elem.addEventListener("click", this.ajaxLoading, false);
        },
        ajaxLoading: function (ev) {
            ev.preventDefault();
            if (this.getAttribute("data-loading") === "0") {
                this.textContent = "等待加载";
                this.setAttribute("data-loading", "1");
                var self = this;
                var content = document.querySelector("#content");
                $.ajax({
                    url: _ajaxLoading.options.url + _ajaxLoading.options.page,
                    type: _ajaxLoading.options.type,
                    dataType: _ajaxLoading.options.dataType,
                    data: _ajaxLoading.options.data,
                    success: function (res) {
                        self.textContent = "加载更多";
                        self.setAttribute("data-loading", "0");
                        if (_ajaxLoading.options.success(res) === 0) {
                            self.textContent = "没有更多数据了";
                            self.setAttribute("data-loading", "1");
                        }
                    }
                });
            }
        }
    }

    var _scrollLoad = {
        on: false, //请求开关
        last: 0, //记录触发加载数据的滚动条距离
        // 冒充success函数的对象
        obj: {
            setPage: function (p) {
                if (p === false) {
                    _scrollLoad.on = true;
                } else {
                    _scrollLoad.options.page = p;
                }
            }
        },
        options: {
            url: "",
            page: 0,
            type: "get",
            dataType: "html",
            data: {},
            beforeSend: function () { },
            success: function (res) { }
        },
        init: function (config) {
            var k;
            for (k in config) {
                if (this.options.hasOwnProperty(k)) {
                    this.options[k] = config[k];
                }
            }
            document.addEventListener("scroll", this.load, false);
        },
        load: function (e) {
            var scroll_top = document.body.scrollTop || document.documentElement.scrollTop;
            var client_height = window.innerHeight;
            var scroll_height = document.body.scrollHeight;
            var last = scroll_height - (scroll_top + client_height);
            if (last < 100) {
                _scrollLoad.last = last;
                if (_scrollLoad.on) {
                    return false;
                }
                _scrollLoad.on = true;
                $.ajax({
                    url: _scrollLoad.options.url + _scrollLoad.options.page,
                    type: _scrollLoad.options.type,
                    dataType: _scrollLoad.options.dataType,
                    data: _scrollLoad.options.data,
                    beforeSend: _scrollLoad.options.beforeSend,
                    success: function (res) {
                        _scrollLoad.on = false;
                        _scrollLoad.last = 0;
                        _scrollLoad.options.success.call(_scrollLoad.obj, res);
                    }
                });
            }
        }
    }
    /**
 	 * Km.cookie.set(key, value);
 	 * @type {Object}
 	 */
    var _COOKIE = {
        get: function (key) {
            var r = "";
            var cookies = document.cookie.split(";");
            var data = {};
            for (var i = 0; i < cookies.length; i++) {
                var arr = cookies[i].split("=");
                arr[0] = arr[0].replace(/^\s*/, "");
                data[arr[0]] = arr[1];
            }
            if (data[key] != undefined) {
                r = data[key];
            }
            return r;
        },
        set: function (key, value) {
            var expires = 0;
            var d = new Date();
            if (arguments.length === 3) {
                expires = arguments[2];
                d = new Date((Date.now() / 1000 + expires) * 1000);
            }
            var cookie_str = key + "=" + value;
            cookie_str += expires ? ";expires=" + d.toUTCString() : "";
            document.cookie = cookie_str;
        }
    }

    window.Km = {
        ajaxLoad: function (config) {
            _ajaxLoading.init(config);
        },
        scrollLoad: function (config) {
            _scrollLoad.init(config);
            return {
                init: function () {
                    $.ajax({
                        url: _scrollLoad.options.url + _scrollLoad.options.page,
                        type: _scrollLoad.options.type,
                        dataType: _scrollLoad.options.dataType,
                        data: _scrollLoad.options.data,
                        beforeSend: _scrollLoad.options.beforeSend,
                        success: function (res) {
                            _scrollLoad.on = false;
                            _scrollLoad.last = 0;
                            _scrollLoad.options.success.call(_scrollLoad.obj, res);
                        }
                    });
                }
            }
        },
        cookie: _COOKIE
    }
}(window, $));

//
! function (window) {
    function removeClass(item, name) {
        var class_name = item.className.split(" ");
        if (class_name.indexOf(name) !== -1) {
            class_name.splice(class_name.indexOf(name), 1);
            item.className = class_name.join(" ");
        }
        return item;
    }

    function addClass(item, name) {
        var class_name = item.className.split(" ");
        if (class_name.indexOf(name) === -1) {
            class_name.push(name);
            item.className = class_name.join(" ");
        }
        return item;
    }

    function scrollTo(ev) {
        var elem = ev.target;
        if (elem.nodeName.toLowerCase() !== "input" && elem.className.indexOf("btn") == "-1") {
            ev.preventDefault();
            window.scrollTo(0, 0);
            return false;
        }
    }
    /**
 	 * 下拉弹出框
 	 * @type {Object}
 	 */
    var _pullSheet = {
        "elem": "",
        "options": {
            "elem": "#pull-sheet",
            "action": function () { },
            "cancel": function () { }
        },
        "init": function (options) {
            var k;
            for (k in options) {
                this.options[k] = options[k];
            }
            var elem = document.querySelector(this.options.elem);
            var save_elem = elem.querySelector(".btn-save");
            var cancel_elem = elem.querySelector(".btn-cancel");
            var group_elem = elem.querySelector(".pull-group");

            save_elem.addEventListener("click", this.action, false);
            cancel_elem.addEventListener("click", this.cancel, false);

            group_elem.style.animation = "pull_sheet 200ms linear";
            group_elem.style.animationFillMode = "forwards";
            group_elem.style.webkitAnimation = "pull_sheet 200ms linear";
            group_elem.style.webkitAnimationFillMode = "forwards";

            elem.className = "";
            this.elem = elem;
        },
        "action": function (ev) {
            ev.preventDefault();
            _pullSheet.options.action();
            _pullSheet.elem.className = "hide";
        },
        "cancel": function (ev) {
            ev.preventDefault();
            _pullSheet.elem.className = "hide";
            _pullSheet.options.cancel();
        }
    }

    /**
 	 * 模态框
 	 * @type {Object}
 	 */
    var _modal = {
        "elem": "",
        "options": {
            "elem": "#modal",
            "action": function () { },
            "cancel": function () { },
            "show": function () { },
            "close": function () {
                removeClass(_modal.elem, "hide");
                document.removeEventListener("touchstart", scrollTo, false);
            }
        },
        "init": function (options) {
            var k;
            for (k in options) {
                this.options[k] = options[k];
            }
            var elem = document.querySelector(this.options.elem);
            var save_elem = elem.querySelector(".btn-action");
            var cancel_elem = elem.querySelector(".btn-cancel");
            var content_elem = elem.querySelector(".modal-content");
            var close_elem = elem.querySelector(".btn-close");

            if (save_elem) {
                save_elem.addEventListener("click", this.action, false);
            }

            if (cancel_elem) {
                cancel_elem.addEventListener("click", this.cancel, false);
            }

            if (close_elem) {
                close_elem.addEventListener("click", this.close, false);
            }

            this.elem = elem;
            this.show();
        },
        "show": function () {
            removeClass(this.elem, "hide");
            // document.addEventListener("touchstart", scrollTo, false);
            this.options.show();
        },
        "action": function (ev) {
            ev.preventDefault();
            _modal.options.action({
                show: function () {
                    removeClass(_modal.elem, "hide");
                },
                hide: function () {
                    addClass(_modal.elem, "hide");
                }
            });
            // document.removeEventListener("touchstart", scrollTo, false);
        },
        "cancel": function (ev) {
            ev.preventDefault();
            _modal.elem.className = "hide";
            _modal.options.cancel();
            // document.removeEventListener("touchstart", scrollTo, false);
        },
        "close": function (ev) {
            ev.preventDefault();
            _modal.elem.className = "hide";
            // document.removeEventListener("touchstart", scrollTo, false);
        }
    }

    var _loading = {
        elem: "",
        init: function (selector) {
            this.elem = document.querySelector(selector);
            var show = this.show;
            var hide = this.hide;
            return {
                show: show,
                hide: hide
            }
        },
        show: function () {
            removeClass(_loading.elem, "hide");
        },
        hide: function () {
            addClass(_loading.elem, "hide");
        }
    }

    var _mess = {
        elem: document.querySelector(".dialog-mess"),
        init: function (mes) {
            removeClass(this.elem, "hide");
            this.elem.firstChild.textContent = mes;
            var timeid = setTimeout(function () {
                addClass(_mess.elem, "hide");
            }, 2000);
            this.elem.addEventListener("click", function () {
                addClass(this, "hide");
                clearInterval(timeid);
            }, false);
            return this;
        },
        faild: function () {
            var lastchild = this.elem.lastChild;
            while (lastchild.nodeType !== 1) {
                lastchild = lastchild.previousSibling;

            }
            addClass(lastchild, "icon-failed");
            removeClass(lastchild, "icon-success");
        }
    }

    window.Dialog = {
        pullSheet: function (options) {
            _pullSheet.init(options);
        },
        modal: function (options) {
            _modal.init(options);
        },
        mess: function (mes) {
            _mess.init(mes);
        },
        loading: function (selector) {
            return _loading.init(selector);
        },
        confirm: function () {

        }
    }
    window.Dialog.mess.faild = function (mes) {
        _mess.init(mes).faild();
    };
}(window);

// dialog 对话框集成jq
(function (window, $) {
    var _dialog = {
        elem: "",
        init: function (mess, elem) {
            var mess_elem = elem[0].querySelector(".dialog-body");
            mess_elem.innerHTML = mess;
            this.elem = elem[0];
        },
        show: function () {
            $(this.elem).removeClass("dialog-hide");
        },
        hide: function () {
            $(this.elem).addClass("dialog-hide");
        },
        confirm: function (func) {
            var action_elem = this.elem.querySelector(".dialog-btn-action");
            var cancel_elem = this.elem.querySelector(".dialog-btn-cancel");
            this.show();
            this.func = func;
            action_elem.removeEventListener("click", action, false);
            action_elem.addEventListener("click", action, false);
            cancel_elem.removeEventListener("click", cancel, false);
            cancel_elem.addEventListener("click", cancel, false);
        },
        notice: function (func) {
            var func = typeof func === "undefined" ? function () { } : func;
            var action_elem = this.elem.querySelector(".dialog-btn-action");
            var obj = this;
            this.show();
            this.func = func;
            action_elem.removeEventListener("click", action, false);
            action_elem.addEventListener("click", action, false);
        },
        mess: function (sec) {
            var obj = this;
            this.show();
            var sec = sec ? sec[0] : 5000;
            setTimeout(function () {
                obj.hide();
            }, sec);
        }
    }

    function action(e) {
        e.preventDefault();
        _dialog.hide();
        _dialog.func(true);
    }

    function cancel(e) {
        e.preventDefault();
        _dialog.hide();
        _dialog.func(false);
    }

    $.fn.dialog = function (conf) {
        var back = typeof conf.callback === "undefined" ? false : conf.callback;
        _dialog.init(conf.mess, this);
        if (back) {
            _dialog[conf.method](back);
        } else {
            if (typeof conf.method === "object") {
                _dialog[conf.method.name](conf.method.param);
            } else {
                _dialog[conf.method]();
            }
        }
    }
})(window, $);

// sidebar
! function (window, $) {
    function animationEnd() {
        $(_sidebar.elem).addClass("hide");
        $(this).off("webkitAnimationEnd", animationEnd);
        $(this).off("animationend", animationEnd);
    }
    var _sidebar = {
        elem: "",
        init: function () {
            _sidebar.run.call(this);
            _sidebar.elem = this;
            var sidebar_cancel = this[0].querySelector(".sidebar_cancel");
            $(sidebar_cancel).on("click", _sidebar.close);
            $(this).children(".sidebar-mask-transparent").on("click", _sidebar.close);
        },
        run: function () {
            $(this).removeClass("hide");
            var body = $(this).children(".sidebar-body");
            var bg = $(this).children(".sidebar-mask-transparent");
            body.css("-webkit-animation", "sidebar 450ms ease");
            body.css("animation", "sidebar 450ms ease");
            body.css("-webkit-animation-fill-mode", "forwards");
            body.css("animation-fill-mode", "forwards");

            bg.css("-webkit-animation", "slider-transparent 200ms ease");
            bg.css("animation", "slider-transparent 200ms ease");
            bg.css("-webkit-animation-fill-mode", "forwards");
            bg.css("animation-fill-mode", "forwards");
        },
        close: function (e) {
            var bg = $(_sidebar.elem).children(".sidebar-mask-transparent");
            var body = $(_sidebar.elem).children(".sidebar-body");
            body.css("-webkit-animation", "sidebar-close 450ms ease");
            body.css("animation", "sidebar-close 450ms ease");
            body.css("-webkit-animation-fill-mode", "forwards");
            body.css("animation-fill-mode", "forwards");
            body.on("webkitAnimationEnd", animationEnd);
            body.on("animationend", animationEnd);

            bg.css("-webkit-animation", "slider-transparent-close 400ms ease");
            bg.css("animation", "slider-transparent-close 400ms ease");
            bg.css("-webkit-animation-fill-mode", "forwards");
            bg.css("animation-fill-mode", "forwards");
        }
    }
    $.fn.sidebar = function () {
        _sidebar.init.call(this);
        return {
            close: _sidebar.close
        }
    }
}(window, $);
! function (window, $) {
    $.fn.radio = function () {
        var callback = arguments.length > 0 ? arguments[0] : function () { };
        var elems = [];
        for (var i = 0; i < this.length; i++) {
            elems.push(this[i]);
        }
        $(this).on("click", function (e) {
            switch (this.type) {
                case "radio":
                    if (this.checked && this._check) {
                        this.checked = false;
                        this._check = false;
                    } else {
                        for (var l = 0; l < elems.length; l++) {
                            elems[l]._check = false;
                        }
                        this._check = true;
                    }
                    break;
                case "checkbox":
                    if (this._check) {
                        this.checked = false;
                        this._check = false;
                    } else {
                        for (var l = 0; l < elems.length; l++) {
                            elems[l]._check = false;
                            elems[l].checked = false;
                        }
                        this._check = true;
                        this.checked = true;
                    }
                    break;
            }
            callback.call(this, (this._check));
        });
    }
}(window, $);
! function (window, $) {
    var _option = {
        type: "down",//down or up
        mess: "这是一个气泡",
        class_name: {
            left: "bottom-left",
            right: "bottom-right"
        }
    };
    $.fn.bubble = function (option) {
        for (k in option) {
            if (_option.hasOwnProperty(k)) {
                _option[k] = option[k];
            }
        }
        var to_top = this[0].offsetTop;
        var to_left = this[0].offsetLeft;
        var to_height = this[0].offsetHeight;
        var offset_parent = this[0].offsetParent;

        var offset_parent_left = offset_parent.offsetLeft + to_left;
        while (offset_parent.nodeName.toLowerCase() !== "body") {
            if (!offset_parent.hasOwnProperty("offsetParent")) { break; }
            offset_parent = offset_parent.offsetParent;
            offset_parent_left += offset_parent.offsetLeft;

        }
        var body_width = offset_parent.offsetWidth;

        var elem = document.createElement("div");

        if (_option.type === "up") {
            elem.style.top = "-" + (to_height + 10) + "px";
        } else {
            elem.style.top = (to_top + to_height + 10) + "px";
        }


        var html = '<div class="bubble-mark"></div><div class="bubble-body">' + _option["mess"] + '</div>';
        elem.innerHTML = html;
        this.parent().append(elem);
        if ((offset_parent_left + elem.offsetWidth) > body_width) {
            elem.className = "bubble bubble-default bubble-" + _option["class_name"]["right"];
            elem.style.left = (to_left - elem.offsetWidth + this[0].offsetWidth) + "px";
        } else {
            elem.style.left = to_left + "px";
            elem.className = "bubble bubble-default bubble-" + _option["class_name"]["left"];
        }

        setTimeout(function () {
            elem.parentNode.removeChild(elem);
        }, 1500);
    }
}(window, $);

! function (window) {
    var hideScroll = {
        hide: function () {
            document.body.style.overflow = "hidden";
        },
        show: function () {
            document.body.style.overflow = "auto";
        }
    }
    window.hideScroll = hideScroll;
}(window);

function lockScroll() {
    function scrollTo(ev) {
        var elem = ev.target;
        if (elem.nodeName.toLowerCase() !== "input" && elem.className.indexOf("btn") == "-1") {
            ev.preventDefault();
            return false;
        }
    }
    return {
        lock: function () {
            document.addEventListener("touchstart", scrollTo, false);

        },
        release: function () {
            document.removeEventListener("touchstart", scrollTo, false);

        }
    }
}
(function (window) {
    if (!window.KM) {
        window.KM = {};
        KM.extend = function (conf) {
            for (var k in conf) {
                if (!this.hasOwnProperty(k)) {
                    this[k] = conf[k];
                }
            }
        }
    }
})(window);

/*if (!NodeList[Symbol.iterator]) {
	NodeList.prototype[Symbol.iterator] = function(){
		var i = 0;
		var that = this;
		return {
			next:function(){
				var r;
				if (i<that.length) {
					r = {value:that[i],done:false};
				} else {
					r = {done:true}
				}
				i++;
				return r;
			}
		}
	}
}*/

(function (window) {
    function slideDelete(options) {
        this.data = options.data ? options.data : false;
        this.url = options.url;
        this.button = options.button;
        this.init(options.elem);
    }
    slideDelete.prototype.init = function (classname) {
        this.width = Math.ceil(window.innerWidth * (20 / 100));
        this.lock_scroll = false;
        this.lock_slide = false;
        this.elem = false;
        var elems = document.querySelectorAll(classname);
        var that = this;
        for (var elem_k = 0; elem_k < elems.length; elem_k++) {
            var elem = elems[elem_k];
            elem.addEventListener("touchstart", function (evt) {
                if (that.elem && that.elem != this) {
                    that.elem.style.webkitTransform = "translate(0px)";
                    that.elem.style.transform = "translate(0px)";
                    that.action_elem.style.transform = "translate(0px)";
                    that.action_elem.style.webkitTransform = "translate(0px)";
                    that.lock_slide = false;
                }
                that.elem = this;
                this.style.position = "relative";
                this.style.zIndex = "2";
                that.action_elem.style.display = "block";
                that.action_elem.style.width = that.width + "px";
                that.action_elem.style.height = this.offsetHeight + "px";
                that.action_elem.style.top = this.offsetTop + "px";
                that.action_elem.style.right = "-" + that.width + "px";

                this._client_x = evt.changedTouches[0].clientX;
                that.action_elem.setAttribute("data-value", this.getAttribute("data-value"));

            }, false);
            elem.addEventListener("touchmove", function (evt) {
                var x = evt.changedTouches[0].clientX;
                if (!this.lock_scroll && this._client_x > x) {
                    this.lock_scroll = true;
                }
                if (this.lock_scroll) {
                    evt.preventDefault();
                }
                var _x = this._client_x - x;

                if (_x > 0) {
                    if (that.lock_slide) { return false; }
                    if (_x > that.width) {
                        this.style.webkitTransform = "translate(-" + (that.width) + "px)";
                        that.action_elem.style.webkitTransform = "translate(-" + (that.width) + "px)";
                        this.style.transform = "translate(-" + (that.width) + "px)";
                        that.action_elem.style.transform = "translate(-" + (that.width) + "px)";
                        that.lock_slide = true;
                        return false;
                    }
                    this.style.webkitTransform = "translate(-" + (_x) + "px)";
                    that.action_elem.style.webkitTransform = "translate(-" + (_x) + "px)";
                    this.style.transform = "translate(-" + (_x) + "px)";
                    that.action_elem.style.transform = "translate(-" + (_x) + "px)";
                } else {
                    if (that.lock_slide) {
                        if (that.width + _x < 0) {
                            this.style.webkitTransform = "translate(0px)";
                            that.action_elem.style.webkitTransform = "translate(0px)";
                            this.style.transform = "translate(0px)";
                            that.action_elem.style.transform = "translate(0px)";
                            that.lock_slide = false;
                            return false;
                        } else {
                            this.style.webkitTransform = "translate(-" + (that.width + _x) + "px)";
                            that.action_elem.style.webkitTransform = "translate(-" + (that.width + _x) + "px)";
                            this.style.transform = "translate(-" + (that.width + _x) + "px)";
                            that.action_elem.style.transform = "translate(-" + (that.width + _x) + "px)";
                        }
                    } else {
                        this.style.webkitTransform = "translate(0px)";
                        that.action_elem.style.webkitTransform = "translate(0px)";
                        this.style.transform = "translate(0px)";
                        that.action_elem.style.transform = "translate(0px)";
                        that.lock_slide = false;
                    }

                }
            }, false);
            elem.addEventListener("touchend", function (evt) {
                var x = evt.changedTouches[0].clientX;
                var _x = this._client_x - x;

                if (_x > 0 && _x > that.width / 2) {
                    this.style.webkitTransform = "translate(-" + (that.width) + "px)";
                    that.action_elem.style.webkitTransform = "translate(-" + (that.width) + "px)";
                    this.style.transform = "translate(-" + (that.width) + "px)";
                    that.action_elem.style.transform = "translate(-" + (that.width) + "px)";
                    that.lock_slide = true;
                } else {
                    if (Math.abs(_x) > that.width / 2) {
                        this.style.webkitTransform = "translate(0px)";
                        that.action_elem.style.webkitTransform = "translate(0px)";
                        this.style.transform = "translate(0px)";
                        that.action_elem.style.transform = "translate(0px)";
                        that.lock_slide = false;
                    } else {
                        if (that.lock_slide) {
                            this.style.webkitTransform = "translate(-" + (that.width) + "px)";
                            that.action_elem.style.webkitTransform = "translate(-" + (that.width) + "px)";
                            this.style.transform = "translate(-" + (that.width) + "px)";
                            that.action_elem.style.transform = "translate(-" + (that.width) + "px)";
                        } else {
                            this.style.webkitTransform = "translate(0px)";
                            that.action_elem.style.webkitTransform = "translate(0px)";
                            this.style.transform = "translate(0px)";
                            that.action_elem.style.transform = "translate(0px)";
                        }
                    }
                }

            }, false);
        }
        this.createButton();
    }
    slideDelete.prototype.createButton = function () {
        var that = this;
        var action_elem = document.createElement("button");
        action_elem.innerText = this.button.text;
        action_elem.className = this.button.class;
        action_elem.style.position = "absolute";
        action_elem.style.zIndex = "3";
        action_elem.style.display = "none";
        document.body.appendChild(action_elem);
        action_elem.addEventListener("click", function (evt) {
            that.bindAction(this.getAttribute("data-value"), this);
        }, false);
        this.action_elem = action_elem;
    }
    slideDelete.prototype.bindAction = function (value, obj) {
        var that = this;
        var data = this.data ? this.data[value] : { id: value };
        this.bind_start();
        $.post(this.url, data, function (res) {
            that.action_elem.style.display = "none";
            that.elem.parentNode.removeChild(that.elem);
            that.bind_action(res);
        }, "json");
    }
    slideDelete.prototype.run = function (options) {
        this.bind_start = options.start;
        this.bind_action = options.done;
    }
    window.slideDelete = slideDelete;
})(window);

// 筛选城市
(function (window) {

    /**
	 * 筛选数据
	 * @param  {[type]} data [description]
	 * @return {[type]}      [description]
	 */
    function handleSearchCity(data) {
        this.data = data;
    }
    /**
	 * 按首字母
	 * @param  {[type]} key [description]
	 * @return {[type]}     [description]
	 */
    handleSearchCity.prototype.letter = function (key) {
        var data = false;
        for (var item_k in this.data) {
            var item = this["data"][item_k];
            if (item["key"].toUpperCase() === key) {
                data = item["city"];
                break;
            }
        }
        return data;
    }
    /**
	 * 按拼音
	 * @param  {[type]} key [description]
	 * @return {[type]}     [description]
	 */
    handleSearchCity.prototype.word = function (key) {

    }
    /**
	 * 按字符
	 * @param  {[type]} key [description]
	 * @return {[type]}     [description]
	 */
    handleSearchCity.prototype.chars = function (key) {
        var data = [];
        for (var item_k in this.data) {
            var item = this["data"][item_k];
            for (var _item_k in item["city"]) {
                var _item = item["city"][_item_k];
                var reg = new RegExp("^" + key);
                _item["name"].match(reg) && data.push(_item);
            }
        }

        return data.length ? data : false;
    }

    function searchCity() {
        this.init = function (options) {
            var search_list_elem = document.querySelector(options.search_list_elem);
            var input_elem = document.querySelector(options.input_elem);
            var city_elem = document.querySelector(options.city_elem);
            var search_city = new handleSearchCity(options.data);

            search_list_elem.addEventListener("click", function (evt) {
                var elem = evt.target;
                if (elem.getAttribute("data-i") !== null) {
                    var i = elem.getAttribute("data-i");
                    options.func(this._city_list[i]);
                }
            }, false);

            input_elem.addEventListener("input", function (evt) {
                var _key = this.value;
                var result;
                var str = "";
                if (_key) {
                    city_elem.classList.add("hide");
                    if (_key.length === 1 && _key.match(/[a-zA-Z]/)) {
                        _key = _key.toUpperCase();
                        result = search_city.letter(_key);
                    } else if (_key.match(/^[a-zA-Z]+[a-zA-Z]$/g)) {

                    } else {
                        result = search_city.chars(_key);
                    }
                    if (result) {
                        for (var k in result) {
                            str += '<div class="cell" data-i="' + k + '"><div class="cell-item" data-i="' + k + '">' + result[k]["name"] + '</div></div>';
                        }

                    } else {
                        str += '<div class="cell"><div class="cell-item tc">没有找到</div></div>';
                    }
                    search_list_elem.classList.remove("hide");
                    search_list_elem.innerHTML = str;
                    search_list_elem._city_list = result;
                } else {
                    city_elem.classList.remove("hide");
                    search_list_elem.classList.add("hide");
                }
            }, false);
        }
    }

    KM.extend({
        searchCity: function (options) {
            var search_city = new searchCity();
            search_city.init(options);
        }
    })
})(window);

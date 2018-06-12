/**
 * 封装系统功能性类
 * @param  {[type]} window [description]
 * @return {[type]}        [description]
 */
(function(window){
	var _G = {
		registers: {},
		extend: function(v){
			if (typeof v === "object") {
				for (k in v) {
					if (!this.registers.hasOwnProperty(k)) {
						this.registers[k] = v[k];
					}
				}
			}
		}
	}
	window.G = _G.registers;

	_G.extend({
		extend: function(a){
			if (typeof a === "object") {
				for (var b in a) {
					if (!this.hasOwnProperty(b)) {
						this[b] = a[b];
					}
				}
			}
		}
	});

	/**
	 * GET 
	 * @return {[type]} [description]
	 */
	var _Get = function(){
		this.data = {};
		this.init();
	}
	_Get.prototype.init = function(){
		var search = location.search;
		var that = this;
		if (search) {
			var path = search.replace("?","");
			var a = path.split("&");
			
			a.forEach(function(item){
				var b = item.split("=");
				var k = b[0];
				that.data[k] = b[1];
			});
		}
	}
	_Get.prototype.getValue = function(k){
		return k?this.data.hasOwnProperty(k)?decodeURIComponent(this.data[k]):"":this.data;
	}
	_Get.prototype.setValue = function(k, v){
		this.data[k] = v;
		return true;
	}
	var __Get = new _Get();
	_G.extend({
		get: function(k){
			if (arguments.length == 2) {
				return __Get.setValue(k, arguments[1]);
			} else {
				return __Get.getValue(k);
			}
		}
	})

	var _COOKIE = function(){
		this.get = function(a){
 			var r = "";
 			var cookies = document.cookie.split(";");
 			var data = {};
 			for (var i = 0; i < cookies.length; i++) {
 				var arr = cookies[i].split("=");
 				arr[0] = arr[0].replace(/^\s*/, "");
 				data[arr[0]] = arr[1];
 			}
 			if (data[a] != undefined) {
 				r = data[a];
 			}
		    return r;

		    
		}
		this.set = function(a, b){
			var expires = 0;
			var d = new Date();

			if (arguments.length === 3) {
			    expires = Number(arguments[2]);
				var time = (Date.now() / 1000) > expires ? (Date.now() / 1000) + expires: expires;
				d = new Date(time * 1000);
			}

			var cookie_str = a + "=" + b;
				cookie_str+= expires ? ";expires="+ d.toUTCString(): "";
			document.cookie = cookie_str;
		}
	}
	// var cookie = new _COOKIE();
	_G.extend({
		cookie: new _COOKIE()
	});

	/**
	 * loader && send 
	 * @param  {[type]} options [description]
	 * @return {[type]}         [description]
	 */
	var _Loader = function(options){
		this.mode = 1;// 0 调试 1 线上
		this.base_path = "/api";
		this.app = "canyin"; // "" 线上 "值" 调试
		this.url = "/api/index.php"; //调试配置
		this.config = {
			url : "",
			method: "GET",
			async: true,
			param: "",
			responseType: "json",
			headers: [],
			before: function(){},
			response: function(){},
			error: function(){
				location.href = "404.html";
			}
		}

		for (var ok in options) {
			if (this.config.hasOwnProperty(ok)) {
				this.config[ok] = options[ok];
			}
		}
		this.init();
		this.load();
	}
	_Loader.prototype = {
		init: function(){
			this.handle_mode();
			this.create_xhr();
			this.handle_param();
		},
		handle_mode: function(){
			if (!this.mode) {
				var uri = "",search = "";
				if (this.config.url.indexOf("?") !== -1) {
					uri = this.config.url.substr(0, this.config.url.indexOf("?"));
					search = "&" + this.config.url.substr(this.config.url.indexOf("?")+1);
				} else {
					uri = this.config.url;
				}
				uri = uri.replace("/", "");
				uri = uri.split("/");
				var url = "?m=" + this.app + "&c=" + uri[0] + "&a=" + uri[1];
				url += search;
				this.config.url = this.url + url;
			} else {
				this.config.url = this.base_path + this.config.url;
			}
		},
		handle_param: function(){
			var param = "";
			if (this.config.param) {
				if (typeof this.config.param === "object") {
					for (var pk in this.config.param) {
						param += pk + "=" + encodeURIComponent(this.config.param[pk]) + "&";
					}
					param = param.substr(0, param.lastIndexOf("&"));
				} else {
					param = this.config.param;
				}

				if (this.config.method.toUpperCase() === "POST") {
					this.config.param = param;
				} else {
					this.config.param = null;
					this.config.url += this.config.url.indexOf("?") !== -1 ? "&" + param: "?" + param;
				}
			}
			
		},
		create_xhr: function(){
			var that = this;
			this.xhr = new XMLHttpRequest();
			this.xhr.onreadystatechange = function(){
				if (that.xhr.readyState === XMLHttpRequest.DONE && that.xhr.status === 200) {
					var res = "";
					/*try{
						switch (that.config.responseType.toUpperCase()) {
							case "XML":
								res = that.xhr.responseXml;
							break;
							case "TEXT":
							case "HTML":
								res = that.xhr.responseText;
							break;
							case "JSON":
								res = eval("("+that.xhr.responseText+")");
							break;
						}
						that.config.response.call(this, res);
					} catch(e) {
						throw new SyntaxError(e);
					}*/
					switch (that.config.responseType.toUpperCase()) {
						case "XML":
							res = that.xhr.responseXml;
						break;
						case "TEXT":
						case "HTML":
							res = that.xhr.responseText;
						break;
						case "JSON":
							res = eval("("+that.xhr.responseText+")");
						break;
					}
					setTimeout(function(){
						that.config.response.call(this, res);
					}, 200);
					
				} else if(that.xhr.status === 404) {
					location.href = "404.html";
				}
			}
			this.xhr.onerror = this.xhr.onabort = function(e){
				that.config.error.call(this, e);
				throw new Error("request failed '"+that.config.url+"'");
			}
		},
		load: function(){
			var that = this;
			this.xhr.open(this.config.method.toUpperCase(), this.config.url);
			if (this.config.method.toUpperCase() === "POST") {
				this.xhr.setRequestHeader("Content-Type", "application/x-www-form-urlencoded");
			}
			if (this.config.headers.length) {
				this.config.headers.forEach(function(item){
					for (var hk in item) {
						that.xhr.setRequestHeader(hk, item[hk]);
					}
				});
			}
			if (this.config.responseType.toUpperCase() === "XML") {
				this.xhr.overrideMimeType("application/xml;charset=UTF-8")
			}
			this.config.before();
			this.xhr.send(this.config.param);
		}
	}
	_G.extend({
		loader: function(options){
			new _Loader(options);
		}
	});

	var _Loadding = {
		init: function(){},
		
	}

	_G.extend({
		helper: {
			extend: function(a){
				if (typeof a === "object") {
					for (var b in  a) {
						if (!this.hasOwnProperty(b)) {
							this[b] = a[b];
						}
					}
				}
			}
		}
	});

 	var _scrollLoad = {
 		on: false, //请求开关
 		last: 0, //记录触发加载数据的滚动条距离
 		options: {
 			page: 1,
 			page_count: 20,
 			load: function(){}
 		},
 		init: function(config) {
 			var k;
 			for (k in config) {
 				if (this.options.hasOwnProperty(k)) {
 					this.options[k] = config[k];
 				}
 			}
 			this.on = false;
 			document.addEventListener("scroll", this.load, false);
 		},
 		load: function(e) {
 			var scroll_top = document.body.scrollTop || document.documentElement.scrollTop;
 			var client_height = window.innerHeight;
 			var scroll_height = document.body.scrollHeight;
 			var last = scroll_height - (scroll_top + client_height);
 			if (last < 100) {
// 				_scrollLoad.on = false;
 				if(_scrollLoad.options.page>_scrollLoad.options.page_count){
 					return false;
 				}
 				_scrollLoad.last = last;
 				if (_scrollLoad.on) {
 					return false;
 				}

 				_scrollLoad.on = true;
 				_scrollLoad.options.load.call(_scrollLoad.options,function(s){
 					_scrollLoad.options.page++;
 					_scrollLoad.on = s;
 					_scrollLoad.last = 0;
 				});
 			}
 		}
 	}
 	_G.extend({
 		scrollload: function(options){
 			_scrollLoad.init(options);
 		}
 	});

 	/**
     * 注入helper类,并且让helper类具有扩展功能
     * @param  {[type]} a [description]
     * @return {[type]}   [description]
     */
 	_G.extend({
 	    helper: {
 	        extend: function (a) {
 	            if (typeof a === "object") {
 	                for (var b in a) {
 	                    if (!this.hasOwnProperty(b)) {
 	                        this[b] = a[b];
 	                    }
 	                }
 	            }
 	        }
 	    }
 	});
 	function _Dialog(options) {
 	    this.opt = {
 	        title: '提示',
 	        text: '',
 	        btns: ['取消', '确定'],
 	        callback: function () { }
 	    }
 	    for (var key in options) {
 	        if (options.hasOwnProperty(key)) {
 	            this.opt[key] = options[key];
 	        }
 	    }
 	    this.init();
 	}

 	_Dialog.prototype.init = function () {
 	    if (document.querySelector('#_Ym-dialog_')) { document.querySelector('#_Ym-dialog_').remove() }
 	    var _this = this;
 	    var head_tpl = _this.opt.title ? '<div class="Ym-dialog-hd"><div class="Ym-dialog-title">' + _this.opt.title + '</div></div>' : ''
 	    var btns_tpl = '';
 	    var btns_count = this.opt.btns.length;
 	    if (btns_count == 1) {
 	        btns_tpl = '<div class="Ym-dialog-ft">\
 	                            <a href="javascript:;" class="Ym-dialog-btn Ym-dialog-btn-confirm">' + this.opt.btns[0] + '</a>\
 	                        </div>';
 	    }
 	    if (btns_count == 2) {
 	        btns_tpl = '<div class="Ym-dialog-ft">\
 	                            <div class="Ym-dialog-btn Ym-dialog-btn-cancel">' + this.opt.btns[0] + '</div>\
 	                            <div class="Ym-dialog-btn Ym-dialog-btn-confirm">' + this.opt.btns[1] + '</div>\
 	                        </div>';
 	    }
 	    if (document.querySelectorAll("#_dialog_").length > 0) document.querySelectorAll("#_dialog_")[0].remove();
 	    var dialog_tpl = '<div id="_Ym-dialog_" class="Ym-dialog-fixed">\
                             <div class="Ym-mask"></div>\
                             <div class="Ym-dialog ">\
                                 '+ head_tpl + '\
                                 <div class="Ym-dialog-bd">' + _this.opt.text + '</div>\
                                ' + btns_tpl + '\
                             </div>\
                         </div>'
 	    document.body.insertAdjacentHTML('beforeend', dialog_tpl);

 	   
 	    document.querySelector('#_Ym-dialog_').addEventListener('touchstart', function () { return false; }, false)
 	    var dialogButtons = document.querySelectorAll(".Ym-dialog-btn");
 	    if (dialogButtons && dialogButtons.length > 0) {
 	        for (var i = 0; i < dialogButtons.length; i++) {
 	            dialogButtons[i].addEventListener('touchstart', function () {
 	                if (_this.opt.callback) {
 	                    var flag = _this.opt.callback.call(_this, this.classList.contains('Ym-dialog-btn-confirm'))
 	                    if (flag == false || flag == 'undefined') {
 	                        return
 	                    }
 	                };
 	                _this.hide();
 	                return;
 	            }, false)
 	        }
 	    }

 	}
 	_Dialog.prototype.hide = function () {
 	    document.querySelector('#_Ym-dialog_').classList.remove('in')
 	    setTimeout(function () { document.querySelector('#_Ym-dialog_').style.display = 'none' }, 150)
 	    return this;
 	}
 	_Dialog.prototype.show = function () {
 	    document.querySelector('#_Ym-dialog_').style.display = 'block'
 	    setTimeout(function () { document.querySelector('#_Ym-dialog_').classList.add('in'); }, 100);
 	    return this;
 	}
 	_G.extend({
 	    dialog: function (options) {
 	        return new _Dialog(options).show();
 	    }
 	});



 	


function _Loading(){
    this.init();
}
_Loading.prototype.init = function () {
    if (document.querySelector('.Ym-loading-fixed')) { console.info('已存在 Ym-loading-fixed 元素') }
    var LoadElemnt = document.createElement('div');
    this.LoadElemnt = LoadElemnt;
    LoadElemnt.className = "Ym-loading-fixed";
    LoadElemnt.innerHTML = ' <div class="Ym-loading"><div class="Ym-mask-transparent"></div>\
		 	          <div class="Ym-loading-img">\
				         <i class="Ym-loading-ele"></i>\
			          </div>\
			          <p class="Ym-loading-text"></p></div>\
		           ';
    document.body.appendChild(LoadElemnt);
}
_Loading.prototype.show = function(t){
    this.LoadElemnt.querySelector('.Ym-loading-text').innerHTML = t || '数据加载中'
    this.LoadElemnt.classList.add('in');
}
_Loading.prototype.hide = function(){
    this.LoadElemnt.classList.remove('in');
}

_G.extend({
    Loading: new _Loading()
});

















})(window);
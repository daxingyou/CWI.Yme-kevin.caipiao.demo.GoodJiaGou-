(function (win) {
    function PullRefresh(params) {
        this.params = {
            container: document.querySelector('.KaMo-refresh-content'),
            friction: 2.5,
            triggerDistance: 50,
            callback: function () { }
        }
        this.params = $.extend.call(this, this.params, params);
        this.touchYDelta;
        this.isLoading = false;
        this.docElem = window.document.documentElement;
        this.loadWrapH;
        this.translateVal;
        this.isMoved = false;
        this.firstTouchY;
        this.dpr = 1
        this.params.triggerDistance = this.params.triggerDistance * this.dpr;
        this.initialScroll;
        this._init();
    }
    PullRefresh.prototype = {

        _init: function () {

            var self = this;
            var loadingHtml = '<div class="KaMo-refresh-load"><div class="KaMo-refresh-pull-arrow">下拉刷新...</div></div>';

            self.params.container.insertAdjacentHTML('afterbegin', loadingHtml);
            self.params.container.addEventListener('touchstart', function (ev) {
                self.touchStart(ev)
            });
            self.params.container.addEventListener('touchmove', function (ev) {
                self.touchMove(ev)
            });
            self.params.container.addEventListener('touchend', function (ev) {
                self.touchEnd(ev, self.params.callback);

            });
            self.ele = document.querySelector('body').getElementsByClassName('KaMo-refresh-pull-arrow')[0]
        },
        touchStart: function (ev) {
            if (this.isLoading) {
                ev.preventDefault();
                return false;
            }
            this.isMoved = false;
            this.params.container.style.webkitTransitionDuration = '0ms';
            this.params.container.style.transitionDuration = '0ms';
            this.touchYDelta = '';
            var touchobj = ev.changedTouches[0];
            this.firstTouchY = parseInt(touchobj.clientY);
            this.initialScroll = this.scrollY();
        },
        touchMove: function (ev) {
            if (this.isLoading) {
                ev.preventDefault();
                return false;
            }
            var self = this;
            var moving = function () {

                var touchobj = ev.changedTouches[0],
					touchY = parseInt(touchobj.clientY);
                self.touchYDelta = touchY - self.firstTouchY;
                if (self.scrollY() === 0 && self.touchYDelta > 0) {
                    ev.preventDefault();
                }
                if (self.initialScroll > 0 || self.scrollY() > 0 || self.scrollY() === 0 && self.touchYDelta < 0) {
                    self.firstTouchY = touchY;
                    return;
                }
                self.translateVal = Math.pow(self.touchYDelta, 0.85);
                self.params.container.style.webkitTransform = self.params.container.style.transform = 'translate3d(0, ' + self.translateVal + 'px, 0)';
                self.isMoved = true;
                if (self.touchYDelta > self.params.triggerDistance) {
                    //self.ele.innerHTML = '松手刷新...';
                    self.ele.innerHTML = '松手刷新...';
                    self.params.container.classList.add("KaMo-refresh-pull-up");
                    self.params.container.classList.remove("KaMo-refresh-pull-down");
                } else {

                    self.params.container.classList.add("KaMo-refresh-pull-down");
                    self.params.container.classList.remove("KaMo-refresh-pull-up");
                }
            };
            moving();
        },
        touchEnd: function (ev, callback) {
            var self = this;
            if (this.isLoading || !this.isMoved) {
                this.isMoved = false;
                return;
            }

            // 根据下拉高度判断是否加载
            if (this.touchYDelta >= this.params.triggerDistance) {
                this.isLoading = true; //正在加载中
                //				ev.preventDefault();
                //self.ele.innerHTML = '刷新中...';
                self.ele.innerHTML = '';
                this.params.container.style.webkitTransitionDuration = '300ms';
                this.params.container.style.transitionDuration = '300ms';
                this.params.container.style.webkitTransform = 'translate3d(0,' + (50) + 'px,0)';
                this.params.container.style.transform = 'translate3d(0,' + (50) + 'px,0)';
                document.querySelector(".KaMo-refresh-pull-arrow").style.webkitTransitionDuration =
		    	document.querySelector(".KaMo-refresh-pull-arrow").style.transitionDuration = '0ms';
                self.params.container.classList.add("KaMo-refreshing");
                if (callback) {
                    callback({
                        status: "success"
                    });
                }
            } else {
                this.params.container.style.webkitTransitionDuration = '300ms';
                this.params.container.style.transitionDuration = '300ms';
                this.params.container.style.webkitTransform = 'translate3d(0,0,0)';
                this.params.container.style.transform = 'translate3d(0,0,0)';
                if (callback) {
                    callback({
                        status: "fail"
                    });
                }
            }
            this.isMoved = false;
            return;
        },
        cancelLoading: function () {
            var self = this;
            this.isLoading = false;

            setTimeout(function () { self.ele.innerHTML = '下拉刷新...'; }, 350)
            self.params.container.classList.remove("KaMo-refreshing");
            document.querySelector(".KaMo-refresh-pull-arrow").style.webkitTransitionDuration = '300ms';
            document.querySelector(".KaMo-refresh-pull-arrow").style.transitionDuration = '300ms';
            self.params.container.style.webkitTransform = 'translate3d(0,0,0)';
            self.params.container.style.transform = 'translate3d(0,0,0)';
            self.params.container.classList.remove("KaMo-refresh-pull-up");
            self.params.container.classList.add("KaMo-refresh-pull-down");
            return;
        },
        scrollY: function () {
            return window.pageYOffset || this.docElem.scrollTop;
        }
    }
    win.PullRefresh = PullRefresh;
})(window)
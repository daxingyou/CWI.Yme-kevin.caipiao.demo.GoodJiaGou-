/**
 * 获取一个1-33的随机数
 * @returns {number} 返回一个随机函数
 */
function randomMath(max) {
    var random = Math.floor(Math.random() * (max - 1))
    if (random < 1) return randomMath(max)
    return random
}

/**
 * 递归函数
 * @param arry
 * @param num
 * @returns {Number}
 */
function recursion(arry, num) {
    if (arry.indexOf(num) != -1) {
        return recursion(arry, randomMath(33))
    }
    return arry.push(num)
}

/**
 * 获取随机数组2
 * @returns {Array}
 */
function randomArry() {
    var selectBall = [];
    for (var i = 0; i < 6; i++) {
        recursion(selectBall, randomMath(33))
    }
    selectBall = selectBall.sort(function (a, b) {
        return a - b
    })
    selectBall[6] = randomMath(16)
    return selectBall
}

function GetQueryString2(name) {
    var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)");
    var r = window.location.search.substr(1).match(reg);
    if (r != null) return unescape(r[2]); return null;
}

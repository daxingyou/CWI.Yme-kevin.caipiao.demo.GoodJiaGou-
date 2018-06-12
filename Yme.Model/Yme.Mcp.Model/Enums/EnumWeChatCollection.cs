using System;
using Yme.Util.Attributes;
using Yme.Util.Extension;

namespace Yme.Mcp.Model.Enums
{
    /// <summary>
    ///  微信消息类型枚举
    /// </summary>
    public enum EnumWeChatMessageType
    {
        /// <summary>
        ///  文本
        /// </summary>
        [EnumAttribute("text")]
        text = 1,

        /// <summary>
        /// 图片
        /// </summary>
        [EnumAttribute("image")]
        image = 2,

        /// <summary>
        /// 语音
        /// </summary>
        [EnumAttribute("voice")]
        voice = 3,

        /// <summary>
        /// 视频
        /// </summary>
        [EnumAttribute("video")]
        video = 4,

        /// <summary>
        /// 地理位置
        /// </summary>
        [EnumAttribute("location")]
        location = 5,

        /// <summary>
        /// 链接信息
        /// </summary>
        [EnumAttribute("link")]
        link = 6,

        /// <summary>
        /// 音乐
        /// </summary>
        [EnumAttribute("music")]
        music = 7,

        /// <summary>
        /// 图文消息
        /// </summary>
        [EnumAttribute("news")]
        news = 8,

        /// <summary>
        /// 事件
        /// </summary>
        [EnumAttribute("events")]
        events = 9,

        /// <summary>
        ///  缩略图
        /// </summary>
        [EnumAttribute("thumb")]
        thumb = 10
    }

    /// <summary>
    /// 事件枚举
    /// </summary>
    public enum EnumWeChatEventType
    {
        /// <summary>
        /// 关注
        /// </summary>
        [EnumAttribute("关注")]
        subscribe = 1,

        /// <summary>
        /// 取消关注
        /// </summary>
        [EnumAttribute("取消关注")]
        unsubscribe = 2,

        /// <summary>
        /// 地理位置
        /// </summary>
        [EnumAttribute("地理位置")]
        location = 3,

        /// <summary>
        /// 扫码带提示
        /// </summary>
        [EnumAttribute("扫码带提示")]
        scancode_waitmsg = 4,

        /// <summary>
        /// 扫码推事件
        /// </summary>
        [EnumAttribute("扫码推事件")]
        SCAN = 5,

        /// <summary>
        /// 系统拍照发图
        /// </summary>
        [EnumAttribute("系统拍照发图")]
        pic_sysphoto = 6,

        /// <summary>
        /// 拍照或者相册发图
        /// </summary>
        [EnumAttribute("拍照或者相册发图")]
        pic_photo_or_album = 7,

        /// <summary>
        /// 微信相册发图
        /// </summary>
        [EnumAttribute("微信相册发图")]
        pic_weixin = 8,

        /// <summary>
        /// 发送位置
        /// </summary>
        [EnumAttribute("发送位置")]
        location_select = 9,

        /// <summary>
        /// 图片
        /// </summary>
        [EnumAttribute("图片")]
        media_id = 10,

        /// <summary>
        /// 图文消息
        /// </summary>
        [EnumAttribute("图文消息")]
        view_limited = 11,

        /// <summary>
        /// 绑定设备
        /// </summary>
        [EnumAttribute("绑定设备")]
        bind = 12,

        /// <summary>
        /// 解除绑定
        /// </summary>
        [EnumAttribute("解除绑定")]
        unbind = 13,

        /// <summary>
        /// 自定义菜单按钮
        /// </summary>
        [EnumAttribute("自定义菜单按钮")]
        click = 99
    }

    /// <summary>
    /// 自动回复消息序号
    /// </summary>
    public enum EnumWeChatAutoMsgKey
    {
        /// <summary>
        /// Wi-Fi配置指南
        /// </summary>
        [EnumAttribute("Wi-Fi配置指南")]
        WiFi = 1,

        /// <summary>
        /// 平台使用指南
        /// </summary>
        [EnumAttribute("平台使用指南")]
        Guide
    }

    /// <summary>
    /// 微信类型
    /// </summary>
    public enum EnumWeChatType
    {
        /// <summary>
        /// 门店端
        /// </summary>
        [EnumAttribute("门店端")]
        Client = 0,

        /// <summary>
        /// 商家端
        /// </summary>
        [EnumAttribute("商家端")]
        Merchant = 1
    }
}


using System;
using Yme.Util.Attributes;
using Yme.Util.Extension;

namespace Yme.Mcp.Model.Enums
{
    #region Api接口

    /// <summary>
    /// 响应状态
    /// </summary>
    public enum ActionResultCode
    {
        /// <summary>
        /// 执行失败
        /// </summary>
        [EnumAttribute("执行失败")]
        Failed = 0,

        /// <summary>
        /// 执行成功
        /// </summary>
        [EnumAttribute("执行成功")]
        Success = 1,

        /// <summary>
        /// 内部错误，当前未使用
        /// </summary>
        [EnumAttribute("内部错误，当前未使用")]
        InnerError = 2,

        /// <summary>
        /// 未登录
        /// </summary>
        [EnumAttribute("未登录")]
        Unauthorized = 3,

        /// <summary>
        /// 需要客户端确认
        /// </summary>
        [EnumAttribute("需要客户端确认")]
        Confirm = 4,

        /// <summary>
        /// 系统未知异常
        /// </summary>
        [EnumAttribute("系统未知异常")]
        UnknownError = 5,

        /// <summary>
        /// 重复提交
        /// </summary>
        [EnumAttribute("重复提交")]
        RepeatSubmit = 99
    }

    /// <summary>
    /// 终端类型
    /// </summary>
    public enum TerminalType
    {
        /// <summary>
        /// 其他设备
        /// </summary>
        [EnumAttribute("其他")]
        Other = 0,

        /// <summary>
        /// Android
        /// </summary>
        [EnumAttribute("Android")]
        Android = 1,

        /// <summary>
        /// IOS
        /// </summary>
        [EnumAttribute("IOS")]
        IOS = 2,

        /// <summary>
        /// PC
        /// </summary>
        [EnumAttribute("PC")]
        PC = 3,

        /// <summary>
        /// 公众号
        /// </summary>
        [EnumAttribute("公众号")]
        Wx = 4
    }

    #endregion

    #region 验证码

    /// <summary>
    /// 验证码类型
    /// </summary>
    public enum VerifyCodeType
    {
        /// <summary>
        /// 登录
        /// </summary>
        [EnumAttribute("登录")]
        Login = 1,

        /// <summary>
        /// 绑定手机
        /// </summary>
        [EnumAttribute("绑定手机")]
        BindMobile = 2
    }

    /// <summary>
    /// 验证码状态类型
    /// </summary>
    public enum VerifyCodeStatusType
    {
        /// <summary>
        /// 验证正常
        /// </summary>
        [EnumAttribute("验证正常")]
        Normal = 0,

        /// <summary>
        /// 验证码错误
        /// </summary>
        [EnumAttribute("短信验证码错误，请重新输入")]
        Fail,

        /// <summary>
        /// 验证码已被使用
        /// </summary>
        [EnumAttribute("短信验证码已被使用")]
        Used,

        /// <summary>
        /// 验证码已过期
        /// </summary>
        [EnumAttribute("短信验证码已过期")]
        Expired
    }

    #endregion

    #region 状态

    /// <summary>
    /// 布尔类型：是/否
    /// </summary>
    public enum BoolType
    {
        /// <summary>
        /// 否
        /// </summary>
        [EnumAttribute("否")]
        No = 0,

        /// <summary>
        /// 是
        /// </summary>
        [EnumAttribute("是")]
        Yes = 1
    }

    /// <summary>
    /// 有效标记类型
    /// </summary>
    public enum EnabledFlagType
    {
        /// <summary>
        /// 停用状态
        /// </summary>
        [EnumAttribute("停用")]
        Disabled = 0,

        /// <summary>
        /// 启用状态
        /// </summary>
        [EnumAttribute("启用")]
        Valid = 1
    }

    /// <summary>
    /// 删除标记类型
    /// </summary>
    public enum DeleteFlagType
    {
        /// <summary>
        /// 删除状态
        /// </summary>
        [EnumAttribute("删除")]
        Disabled = 1,

        /// <summary>
        /// 正常状态
        /// </summary>
        [EnumAttribute("正常")]
        Valid = 0
    }

    /// <summary>
    /// 订单状态
    /// </summary>
    public enum OrderStatus
    {
        /// <summary>
        /// 待接单
        /// </summary>
        [EnumAttribute("待接单")]
        WaitConfirm = 1,

        /// <summary>
        /// 已接单--同时更新打印状态为【待打印】
        /// </summary>
        [EnumAttribute("进行中")]
        Confirmed,

        /// <summary>
        /// 配送中
        /// </summary>
        [EnumAttribute("进行中")]
        Deliverying,

        /// <summary>
        /// 已完成
        /// </summary>
        [EnumAttribute("已完成")]
        Completed,

        /// <summary>
        /// 已取消
        /// </summary>
        [EnumAttribute("已取消")]
        Canceled
    }

    /// <summary>
    /// 打印状态
    /// </summary>
    public enum PrintStatus
    {
        /// <summary>
        /// 待打印
        /// </summary>
        [EnumAttribute("待打印")]
        WaitPrint = 1,

        /// <summary>
        /// 已打印
        /// </summary>
        [EnumAttribute("已打印")]
        Printed,

        /// <summary>
        /// 打印失败
        /// </summary>
        [EnumAttribute("打印失败")]
        PrintFail,

        /// <summary>
        /// 超时未打
        /// </summary>
        [EnumAttribute("超时未打")]
        TimeOutUnPrint
    }

    /// <summary>
    /// 订单类型
    /// </summary>
    public enum OrderType
    {
        /// <summary>
        /// 即时订单
        /// </summary>
        [EnumAttribute("即时订单")]
        ImmediatelyOrder = 1,

        /// <summary>
        /// 预订单
        /// </summary>
        [EnumAttribute("预订单")]
        PreOrder
    }

    /// <summary>
    /// 订单查询类型
    /// </summary>
    public enum OrderQueryType
    {
        /// <summary>
        /// 今日订单
        /// </summary>
        [EnumAttribute("今日订单")]
        DayOrders = 1,

        /// <summary>
        /// 预订单
        /// </summary>
        [EnumAttribute("预订单")]
        PreOrders,

        /// <summary>
        /// 补打订单
        /// </summary>
        [EnumAttribute("补打订单")]
        PrintOrders,

        /// <summary>
        /// 查询订单
        /// </summary>
        [EnumAttribute("查询订单")]
        SearchOrders
    }

    /// <summary>
    /// 统计区间类型
    /// </summary>
    public enum StatisticsType
    {
        /// <summary>
        /// 近7日
        /// </summary>
        [EnumAttribute("近7日")]
        OneWeek = 1,

        /// <summary>
        /// 近15日
        /// </summary>
        [EnumAttribute("近15日")]
        HalfMonth,

        /// <summary>
        /// 近1月
        /// </summary>
        [EnumAttribute("近30日")]
        OneMonth,

        /// <summary>
        /// 近3月
        /// </summary>
        [EnumAttribute("近90日")]
        ThreeMonths
    }

    /// <summary>
    /// 操作类型枚举
    /// </summary>
    public enum OperateType
    {
        /// <summary>
        /// 登录
        /// </summary>
        [EnumAttribute("登录")]
        DoLogin = 1,

        /// <summary>
        /// 登出
        /// </summary>
        [EnumAttribute("登出")]
        Dologout = 2,

        /// <summary>
        /// 获取信息
        /// </summary>
        [EnumAttribute("获取信息")]
        GetInfo = 3
    }

    #endregion

    #region 短信

    /// <summary>
    /// 短信类型
    /// </summary>
    public enum SmsType
    {
        /// <summary>
        /// 未知
        /// </summary>
        [EnumAttribute("未知")]
        UnKnown = 0,

        /// <summary>
        /// 验证码
        /// </summary>
        [EnumAttribute("验证码")]
        ValidateCode = 1
    }

    #endregion

    #region 单据

    /// <summary>
    /// 单据类型
    /// </summary>
    public enum BillType
    {
        /// <summary>
        /// 小票
        /// </summary>
        [EnumAttribute("小票")]
        SmallBill = 1,

        /// <summary>
        /// 快递面单
        /// </summary>
        [EnumAttribute("快递面单")]
        ExpressBill
    }

    /// <summary>
    /// 小票类型
    /// </summary>
    public enum SmallBillType
    {
        /// <summary>
        /// 厨房小票
        /// </summary>
        [EnumAttribute("厨房小票")]
        Kitchen = 1,

        /// <summary>
        /// 商家小票
        /// </summary>
        [EnumAttribute("商家小票")]
        Merchant,

        /// <summary>
        /// 配送小票
        /// </summary>
        [EnumAttribute("配送小票")]
        Delivery,

        /// <summary>
        /// 顾客小票
        /// </summary>
        [EnumAttribute("顾客小票")]
        Customer,

        /// <summary>
        /// 取消小票
        /// </summary>
        [EnumAttribute("取消小票")]
        Cancel
    }

    /// <summary>
    /// 小票类别
    /// </summary>
    public enum SmallBillCategory
    {
        // <summary>
        /// 营业类小票
        /// </summary>
        [EnumAttribute("营业类小票")]
        Business = 1,

        /// <summary>
        /// 取消类小票
        /// </summary>
        [EnumAttribute("取消类小票")]
        Cancel,
    }

    #endregion

    #region 集成平台

    /// <summary>
    /// 业务类型
    /// </summary>
    public enum BusinessType
    {
        /// <summary>
        /// 外卖
        /// </summary>
        [EnumAttribute("外卖")]
        Waimai = 1,

        /// <summary>
        /// 餐饮
        /// </summary>
        [EnumAttribute("餐饮")]
        Canyin,

        /// <summary>
        /// 商城
        /// </summary>
        [EnumAttribute("商城")]
        Shangcheng,

        /// <summary>
        /// 物流
        /// </summary>
        [EnumAttribute("物流")]
        Wuliu
    }

    /// <summary>
    /// 平台类型
    /// </summary>
    public enum PlatformType
    {
        /// <summary>
        /// 美团外卖
        /// </summary>
        [EnumAttribute("美团外卖")]
        Meituan = 1,

        /// <summary>
        /// 饿了么
        /// </summary>
        [EnumAttribute("饿了么")]
        Eleme,

        /// <summary>
        /// 百度外卖
        /// </summary>
        [EnumAttribute("百度外卖")]
        Baidu
    }

    /// <summary>
    /// 授权业务类型
    /// </summary>
    public enum AuthBussinessType
    {
        /// <summary>
        /// 团购/闪惠
        /// </summary>
        [EnumAttribute("团购/闪惠")]
        Coupon = 1,

        /// <summary>
        /// 外卖
        /// </summary>
        [EnumAttribute("外卖")]
        Waimai
    }

    /// <summary>
    /// 美团推送消息类型
    /// </summary>
    public enum MeiPushMsgType
    {
        /// <summary>
        /// 新订单
        /// </summary>
        [EnumAttribute("新订单")]
        NewOrder = 1,

        /// <summary>
        /// 配送中
        /// </summary>
        [EnumAttribute("配送中")]
        Delivering,

        /// <summary>
        /// 完成订单
        /// </summary>
        [EnumAttribute("完成订单")]
        CompleteOrder,

        /// <summary>
        /// 取消订单
        /// </summary>
        [EnumAttribute("取消订单")]
        CancelOrder
    }

    /// <summary>
    /// 美团订单状态码
    /// </summary>
    public enum MeiStatusCode
    {
        /// <summary>
        /// 用户已提交订单
        /// </summary>
        [EnumAttribute("用户已提交订单")]
        Submited = 1,

        /// <summary>
        /// 可推送到商家
        /// </summary>
        [EnumAttribute("可推送到商家")]
        EnablePush = 2,

        /// <summary>
        /// 商家已收到
        /// </summary>
        [EnumAttribute("商家已收到")]
        MerchantReceived = 3,

        /// <summary>
        /// 商家已确认
        /// </summary>
        [EnumAttribute("商家已确认")]
        MerchantConfirmed = 4,

        /// <summary>
        /// 已配送
        /// </summary>
        [EnumAttribute("已配送")]
        Deliveried = 6,

        /// <summary>
        /// 已完成
        /// </summary>
        [EnumAttribute("已完成")]
        Completed = 8,

        /// <summary>
        /// 已取消
        /// </summary>
        [EnumAttribute("已取消")]
        Canceled = 9
    }

    /// <summary>
    /// 美团配送代码
    /// </summary>
    public enum MeilogisticsCode
    {
        /// <summary>
        /// 商家自配送
        /// </summary>
        [EnumAttribute("商家自配送")]
        Sjzps = 0,

        /// <summary>
        /// 趣活
        /// </summary>
        [EnumAttribute("趣活")]
        Quhuo = 2,

        /// <summary>
        /// 达达
        /// </summary>
        [EnumAttribute("达达")]
        Dada = 16,

        /// <summary>
        /// E代送
        /// </summary>
        [EnumAttribute("E代送")]
        Eds = 33,

        /// <summary>
        /// 美团专送-加盟
        /// </summary>
        [EnumAttribute("美团专送-加盟")]
        Mtjm = 1001,

        /// <summary>
        /// 美团专送-自建
        /// </summary>
        [EnumAttribute("美团专送-自建")]
        Mtzj = 1002,

        /// <summary>
        /// 美团专送-众包
        /// </summary>
        [EnumAttribute("美团配送-众包")]
        Mtzb = 1003,

        /// <summary>
        /// 美团专送-城市代理
        /// </summary>
        [EnumAttribute("美团专送-城市代理")]
        Mtcsdl = 1004,

        /// <summary>
        /// 角马
        /// </summary>
        [EnumAttribute("角马")]
        Jm = 2001,

        /// <summary>
        /// 快送
        /// </summary>
        [EnumAttribute("快送")]
        Ks = 2002
    }

    /// <summary>
    /// 饿了么推送消息类型
    /// </summary>
    public enum ElemePushMsgType
    {
        /// <summary>
        /// 新订单
        /// </summary>
        [EnumAttribute("新订单")]
        NewOrder = 10,

        /// <summary>
        /// 取消订单
        /// </summary>
        [EnumAttribute("取消订单")]
        CancelOrder = 14,

        /// <summary>
        /// 无效订单
        /// </summary>
        [EnumAttribute("无效订单")]
        InvalidOrder = 15,

        /// <summary>
        /// 订单强制无效
        /// </summary>
        [EnumAttribute("强制无效")]
        ForceInvalid = 17,

        /// <summary>
        /// 完成订单
        /// </summary>
        [EnumAttribute("完成订单")]
        CompleteOrder = 18,

        /// <summary>
        /// 配送中
        /// </summary>
        [EnumAttribute("配送中")]
        Delivering = 55
    }

    /// <summary>
    /// 饿了么角色类型
    /// </summary>
    public enum ElemeRoleType
    {
        /// <summary>
        /// 用户
        /// </summary>
        [EnumAttribute("用户")]
        OrderUser = 1,

        /// <summary>
        /// 系统
        /// </summary>
        [EnumAttribute("系统")]
        ElemeSys = 2,

        /// <summary>
        /// 商户
        /// </summary>
        [EnumAttribute("商户")]
        Merchant = 3,

        /// <summary>
        /// 客服
        /// </summary>
        [EnumAttribute("客服")]
        Kefu = 4,

        /// <summary>
        /// 开放平台系统
        /// </summary>
        [EnumAttribute("开放平台系统")]
        OpenSys = 5,

        /// <summary>
        /// 短信系统
        /// </summary>
        [EnumAttribute("短信系统")]
        SmsSys = 6,

        /// <summary>
        /// 无线打印机系统
        /// </summary>
        [EnumAttribute("无线打印机系统")]
        WxdyjSys = 7,

        /// <summary>
        /// 风控系统
        /// </summary>
        [EnumAttribute("风控系统")]
        FkSys = 8
    }

    /// <summary>
    /// 百度外卖订单状态类型
    /// </summary>
    public enum BaiduOrderStatusType
    {
        /// <summary>
        /// 待确认
        /// </summary>
        [EnumAttribute("待确认")]
        WaitConfirm = 1,

        /// <summary>
        /// 已确认
        /// </summary>
        [EnumAttribute("已确认")]
        Confirmed = 5,

        /// <summary>
        /// 骑士已接单
        /// </summary>
        [EnumAttribute("已接单")]
        Received = 7,

        /// <summary>
        /// 骑士已取餐
        /// </summary>
        [EnumAttribute("配送中")]
        Delivering = 8,

        /// <summary>
        /// 已完成
        /// </summary>
        [EnumAttribute("已完成")]
        Completed = 9,

        /// <summary>
        /// 已取消
        /// </summary>
        [EnumAttribute("已取消")]
        Canceled = 10
    }

    /// <summary>
    /// 百度外卖订单取消类型
    /// </summary>
    public enum BaiduOrderCancelType
    {
        /// <summary>
        /// 不在配送范围内
        /// </summary>
        [EnumAttribute("不在配送范围内")]
        Bzpsfw = 1,

        /// <summary>
        /// 餐厅已打烊
        /// </summary>
        [EnumAttribute("餐厅已打烊")]
        Ctydy = 2,

        /// <summary>
        /// 美食已售完
        /// </summary>
        [EnumAttribute("美食已售完")]
        Msysw = 3,

        /// <summary>
        /// 菜品价格发生变化
        /// </summary>
        [EnumAttribute("菜品价格发生变化")]
        Cpjgbh = 4,

        /// <summary>
        /// 用户取消订单
        /// </summary>
        [EnumAttribute("用户取消订单")]
        Yhqxdd = 5,

        /// <summary>
        /// 重复订单
        /// </summary>
        [EnumAttribute("重复订单")]
        Cfdd = 6,

        /// <summary>
        /// 餐厅太忙
        /// </summary>
        [EnumAttribute("餐厅太忙")]
        Cttm = 7,

        /// <summary>
        /// 联系不上用户
        /// </summary>
        [EnumAttribute("联系不上用户")]
        Lxbsyh = 8,

        /// <summary>
        /// 假订单
        /// </summary>
        [EnumAttribute("假订单")]
        Jdd = 9,

        /// <summary>
        /// API商户系统向门店推送订单失败
        /// </summary>
        [EnumAttribute("API商户系统向门店推送订单失败")]
        Tsddsb = 52,

        /// <summary>
        /// 自定义输入
        /// </summary>
        [EnumAttribute("自定义输入")]
        Zdysr = -1
    }

    /// <summary>
    /// 百度外卖配送代码
    /// </summary>
    public enum BaidulogisticsCode
    {
        /// <summary>
        /// 百度专送
        /// </summary>
        [EnumAttribute("百度专送")]
        Bdzs = 1,

        /// <summary>
        /// 商家自配送
        /// </summary>
        [EnumAttribute("商家自配送")]
        Sjzps = 2
    }

    /// <summary>
    /// 打印结果编码
    /// </summary>
    public enum PrintResultType
    {
        /// <summary>
        /// 未配置
        /// </summary>
        [EnumAttribute("未配置")]
        UnConfig = 10000,

        /// <summary>
        /// 打印成功
        /// </summary>
        [EnumAttribute("打印成功")]
        Success = 10001,

        /// <summary>
        /// 打印失败
        /// </summary>
        [EnumAttribute("打印失败")]
        Fail = 10002,

        /// <summary>
        /// 打印机故障
        /// </summary>
        [EnumAttribute("打印机故障")]
        Fault = 10003,

        /// <summary>
        /// 打印机缺纸
        /// </summary>
        [EnumAttribute("打印机缺纸")]
        NeedPaper = 10004,

        /// <summary>
        /// 打印机网络故障
        /// </summary>
        [EnumAttribute("打印机网络故障")]
        NetworkFault = 10005,

        /// <summary>
        /// 打印机未关联
        /// </summary>
        [EnumAttribute("打印机未关联")]
        NotBind = 10006,

        /// <summary>
        /// 未找到打印机
        /// </summary>
        [EnumAttribute("未找到打印机")]
        NotFind = 10007,

        /// <summary>
        /// 超时未打印
        /// </summary>
        [EnumAttribute("超时未打印")]
        TimeOutUnPrint = 10008,

        /// <summary>
        /// 解绑清理
        /// </summary>
        [EnumAttribute("解绑清理")]
        UnBindClear = 10009,

        /// <summary>
        /// 其他原因
        /// </summary>
        [EnumAttribute("其他原因")]
        UnKnow = 99999
    }

    /// <summary>
    /// 打印机异常类型
    /// </summary>
    public enum PrinterFaultType
    {
        /// <summary>
        /// 正常
        /// </summary>
        [EnumAttribute("正常")]
        Enable = 1,

        /// <summary>
        /// 打印机缺纸
        /// </summary>
        [EnumAttribute("打印机缺纸")]
        NeedPaper,

        /// <summary>
        /// 打印机故障
        /// </summary>
        [EnumAttribute("打印机故障")]
        Fault,

        /// <summary>
        /// 打印机网络故障
        /// </summary>
        [EnumAttribute("打印机网络故障")]
        NetworkFault,

        /// <summary>
        /// 打印机未关联
        /// </summary>
        [EnumAttribute("打印机未关联")]
        NotBind,

        /// <summary>
        /// 未找到打印机
        /// </summary>
        [EnumAttribute("未找到打印机")]
        NotFind,

        /// <summary>
        /// 未知异常
        /// </summary>
        [EnumAttribute("未知异常")]
        UnKnow = 99
    }

    /// <summary>
    /// 推送消息类型
    /// </summary>
    public enum PushMsgType
    {
        [EnumAttribute("打印机异常消息")]
        PrinterFaultMsg = 1,

        [EnumAttribute("打印失败消息")]
        PrintFailMsg = 2
    }

    /// <summary>
    /// 发票类型
    /// </summary>
    public enum InvoiceType
    {
        [EnumAttribute("个人")]
        Personal = 1,

        [EnumAttribute("企业")]
        Company = 2
    }

    #endregion

    #region 微云打

    /// <summary>
    /// 设备状态
    /// </summary>
    public enum DeviceStatus
    {
        /// <summary>
        /// 正常
        /// </summary>
        [EnumAttribute("正常")]
        Enable = 1,

        /// <summary>
        /// 缺纸
        /// </summary>
        [EnumAttribute("缺纸")]
        NeedPage,

        /// <summary>
        /// 故障
        /// </summary>
        [EnumAttribute("故障")]
        Fault
    }

    // <summary>
    /// 任务状态
    /// </summary>
    public enum TaskStatus
    {
        /// <summary>
        /// 待打印
        /// </summary>
        [EnumAttribute("待打印")]
        WaitPrint = 0,

        /// <summary>
        /// 正常
        /// </summary>
        [EnumAttribute("打印成功")]
        PrintSuccess,

        /// <summary>
        /// 缺纸
        /// </summary>
        [EnumAttribute("打印机缺纸")]
        PrinterNeedPage,

        /// <summary>
        /// 故障
        /// </summary>
        [EnumAttribute("打印机故障")]
        PrinterFault
    }

    /// <summary>
    /// 任务数据类型
    /// </summary>
    public enum TaskDataType
    {
        /// <summary>
        /// JSON模版
        /// </summary>
        [EnumAttribute("TEMPLATE")]
        TEMPLATE = 1,

        /// <summary>
        /// URL地址
        /// </summary>
        [EnumAttribute("HTML")]
        HTML,

        /// <summary>
        /// ESC命令
        /// </summary>
        [EnumAttribute("ESC")]
        ESC,

        /// <summary>
        /// JSON数据
        /// </summary>
        [EnumAttribute("JSON")]
        JSON
    }

    /// <summary>
    /// 返回状态编码
    /// </summary>
    public enum ErrorCodeType
    {
        /// <summary>
        /// 调用成功
        /// </summary>
        [EnumAttribute("调用成功")]
        Success = 0,

        /// <summary>
        /// 参数错误
        /// </summary>
        [EnumAttribute("参数错误")]
        ParamError,

        /// <summary>
        /// 安全校验失败
        /// </summary>
        [EnumAttribute("安全校验失败")]
        Invalid,
    }

    /// <summary>
    /// 请求相应类型
    /// </summary>
    public enum ResponseType
    {
        /// <summary>
        /// 无打印任务
        /// </summary>
        [EnumAttribute("无打印任务")]
        NoPrintTask = 0,

        /// <summary>
        /// 参数错误
        /// </summary>
        [EnumAttribute("新打印任务")]
        NewPrintTask,
    }

    /// <summary>
    /// 微云打接口操作类型
    /// </summary>
    public enum McpOpType
    {
        /// <summary>
        /// 查询打印任务
        /// </summary>
        [EnumAttribute("查询打印任务")]
        GetTask = 0,

        /// <summary>
        /// 同步任务状态
        /// </summary>
        [EnumAttribute("同步任务状态")]
        SyncTaskStatus,

        /// <summary>
        /// 同步打印机状态
        /// </summary>
        [EnumAttribute("同步打印机状态")]
        SyncDeviceStatus,
    }

    #endregion
}

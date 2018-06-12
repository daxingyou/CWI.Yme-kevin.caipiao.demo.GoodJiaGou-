using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yme.Mcp.Model.ViewModels.Waimai.Eleme
{
    public class ElemeOrderModel
    {
        /// <summary>
        /// 【扩展】微云打平台门店Id
        /// </summary>
        public long mShopId { get; set; }

        /// <summary>
        /// 订单Id
        /// </summary>
        public string id { get; set; }

        /// <summary>
        /// 订单Id
        /// </summary>
        public string orderId { get; set; }

        /// <summary>
        /// 顾客送餐地址
        /// </summary>
        public string address { get; set; }

        /// <summary>
        /// 下单时间
        /// </summary>
        public DateTime? createdAt { get; set; }

        /// <summary>
        /// 订单生效时间
        /// </summary>
        public DateTime? activeAt { get; set; }

        /// <summary>
        /// 配送费
        /// </summary>
        public double deliverFee { get; set; }

        /// <summary>
        /// 预计送达时间
        /// </summary>
        public DateTime? deliverTime { get; set; }

        /// <summary>
        /// 订单备注
        /// </summary>
        public string description { get; set; }

        /// <summary>
        /// 订单详细类目的列表List<OGoodsGroup>
        /// </summary>
        public object groups { get; set; }

        /// <summary>
        /// 发票抬头
        /// </summary>
        public string invoice { get; set; }

        /// <summary>
        /// 是否预订单
        /// </summary>
        public bool book { get; set; }

        /// <summary>
        /// 是否在线支付
        /// </summary>
        public bool onlinePaid { get; set; }

        public string railwayAddress { get; set; }

        /// <summary>
        /// 顾客联系电话
        /// </summary>
        public List<string> phoneList { get; set; }

        /// <summary>
        /// 饿了么门店Id
        /// </summary>
        public string shopId { get; set; }

        /// <summary>
        /// 饿了么门店名称
        /// </summary>
        public string shopName { get; set; }

        /// <summary>
        /// 店铺当日订单流水号
        /// </summary>
        public int daySn { get; set; }

        /// <summary>
        /// 订单状态[OOrderStatus枚举]
        /// </summary>
        public string status { get; set; }

        /// <summary>
        /// 退单状态[OOrderRefundStatus枚举]
        /// </summary>
        public string refundStatus { get; set; }

        /// <summary>
        /// 下单用户的Id
        /// </summary>
        public int userId { get; set; }

        /// <summary>
        /// 订单总价，用户实际支付的金额，单位：元
        /// </summary>
        public double totalPrice { get; set; }

        /// <summary>
        /// 订单原始价格
        /// </summary>
        public double originalPrice { get; set; }

        /// <summary>
        /// 订单收货人姓名
        /// </summary>
        public string consignee { get; set; }

        /// <summary>
        /// 订单收货地址经纬度
        /// </summary>
        public string deliveryGeo { get; set; }

        /// <summary>
        /// 送餐地址
        /// </summary>
        public string deliveryPoiAddress { get; set; }

        /// <summary>
        /// 顾客是否需要发票
        /// </summary>
        public bool invoiced { get; set; }

        /// <summary>
        /// 店铺实收
        /// </summary>
        public double income { get; set; }

        /// <summary>
        /// 饿了么服务费率
        /// </summary>
        public double serviceRate { get; set; }

        /// <summary>
        /// 饿了么服务费
        /// </summary>
        public double serviceFee { get; set; }

        /// <summary>
        /// 订单中红包金额
        /// </summary>
        public double hongbao { get; set; }

        /// <summary>
        /// 餐盒费
        /// </summary>
        public double packageFee { get; set; }

        /// <summary>
        /// 订单活动总额
        /// </summary>
        public double activityTotal { get; set; }

        /// <summary>
        /// 店铺承担活动费用
        /// </summary>
        public double shopPart { get; set; }

        /// <summary>
        /// 饿了么承担活动费用
        /// </summary>
        public double elemePart { get; set; }

        /// <summary>
        /// 降级标识
        /// </summary>
        public bool downgraded { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public double vipDeliveryFeeDiscount { get; set; }

        /// <summary>
        /// 店铺绑定的外部ID
        /// </summary>
        public string openId { get; set; }

        /// <summary>
        /// 保护小号失效时间
        /// </summary>
        public DateTime? secretPhoneExpireTime { get; set; }

        /// <summary>
        /// 订单参加活动信息List<OActivity>
        /// </summary>
        public object orderActivities { get; set; }

        /// <summary>
        /// 发票类型
        /// </summary>
        public string invoiceType { get; set; }

        /// <summary>
        /// 纳税人识别号
        /// </summary>
        public string taxpayerId { get; set; }

        /// <summary>
        /// 冷链加价费
        /// </summary>
        public double coldBoxFee { get; set; }
    }
    
    /// <summary>
    /// 订单篮子列表
    /// </summary>
    public class EleOrderGroupsModel
    {
        /// <summary>
        /// 分组名称(例如：1号篮子)
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// 分组类型(normal: 普通商品, discount: 赠品)
        /// </summary>
        public string type { get; set; }

        /// <summary>
        /// 商品信息列表
        /// </summary>
        public List<EleOrderItemsModel> items { get; set; }
    }

    /// <summary>
    /// 订单篮子明细
    /// </summary>
    public class EleOrderItemsModel
    {
        /// <summary>
        /// 食物id
        /// </summary>
        public long id { get; set; }

        /// <summary>
        /// 规格Id（根据篮子的类型取不同的值）
        /// </summary>
        public long skuId { get; set; }

        /// <summary>
        /// 商品名称
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// 订单中商品项的标识(注意，此处不是商品分类Id)
        /// </summary>
        public int categoryId { get; set; }

        /// <summary>
        /// 商品单价
        /// </summary>
        public double price { get; set; }

        /// <summary>
        /// 商品数量
        /// </summary>
        public int? quantity { get; set; }

        /// <summary>
        /// 总价
        /// </summary>
        public double? total { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public object additions { get; set; }

        /// <summary>
        /// 多规格(Json串,name-value)
        /// </summary>
        public object newSpecs { get; set; }

        /// <summary>
        /// 多属性(Json串,name-value)
        /// </summary>
        public object attributes { get; set; }

        /// <summary>
        /// 商品扩展码(商户内部使用的SKU码)
        /// </summary>
        public string extendCode { get; set; }

        /// <summary>
        /// 商品条形码
        /// </summary>
        public string barCode { get; set; }

        /// <summary>
        /// 商品重量(单位克)
        /// </summary>
        public double? weight { get; set; }

        public double? userPrice { get; set; }

        public double? shopPrice { get; set; }
    }

    /// <summary>
    /// 订单参加活动信息
    /// </summary>
    public class EleOrderActiveModel
    {
        /// <summary>
        /// 活动id
        /// </summary>
        public long id { get; set; }

        /// <summary>
        /// 活动名称
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// 活动类别
        /// </summary>
        public int? categoryId { get; set; }

        /// <summary>
        /// 金额
        /// </summary>
        public double? amount { get; set; }
    }

    /// <summary>
    /// 商品属性信息
    /// </summary>
    public class OrderProductAttrModel
    {
        /// <summary>
        /// 属性名称
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// 属性值
        /// </summary>
        public string value { get; set; }
    }
}

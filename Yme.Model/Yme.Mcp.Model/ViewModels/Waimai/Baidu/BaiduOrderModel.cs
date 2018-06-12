using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yme.Mcp.Model.ViewModels.Waimai.Baidu
{
    public class BaiduOrderModel
    {
        ///// <summary>
        ///// 【扩展】微云打平台门店Id
        ///// </summary>
        //public long mShopId { get; set; }

        /// <summary>
        /// 百度外卖门店Id
        /// </summary>
        public string source { get; set; }

        /// <summary>
        /// 订单信息
        /// </summary>
        public BaiduOrderDetailModel order { get; set; }

        /// <summary>
        /// 客户信息
        /// </summary>
        public BaiduOrderUserModel user { get; set; }

        /// <summary>
        /// 门店信息
        /// </summary>
        public BaiduOrderShopModel shop { get; set; }

        /// <summary>
        /// 订单商品信息
        /// </summary>
        public object products { get; set; }

        /// <summary>
        /// 优惠信息
        /// </summary>
        public object discount { get; set; }

        /// <summary>
        /// 部分退款
        /// </summary>
        public object part_refund_info { get; set; }
    }

    /// <summary>
    /// 订单详细信息
    /// </summary>
    public class BaiduOrderDetailModel
    {
        /// <summary>
        /// 订单Id
        /// </summary>
        public string order_id { get; set; }

        /// <summary>
        /// 是否立即送餐:1-是,2-否
        /// </summary>
        public int send_immediately { get; set; }

        /// <summary>
        /// 订单当日流水号
        /// </summary>
        public string order_index { get; set; }

        /// <summary>
        /// 订单状态
        /// </summary>
        public int status { get; set; }

        /// <summary>
        /// 送达时间类型:1-定时达,2-限时达（错峰配送）
        /// </summary>
        public int expect_time_mode { get; set; }

        /// <summary>
        /// 期望送达时间
        /// </summary>
        public string send_time { get; set; }

        /// <summary>
        /// 取餐时间
        /// </summary>
        public int pickup_time { get; set; }

        /// <summary>
        /// 到店时间
        /// </summary>
        public int atshop_time { get; set; }

        /// <summary>
        /// 送餐时间
        /// </summary>
        public int delivery_time { get; set; }

        /// <summary>
        /// 骑士手机号
        /// </summary>
        public string delivery_phone { get; set; }

        /// <summary>
        /// 完成时间
        /// </summary>
        public string finished_time { get; set; }

        /// <summary>
        /// 确认时间
        /// </summary>
        public string confirm_time { get; set; }

        /// <summary>
        /// 取消时间
        /// </summary>
        public string cancel_time { get; set; }

        /// <summary>
        /// 配送费，单位：分
        /// </summary>
        public int send_fee { get; set; }

        /// <summary>
        /// 餐盒费，单位：分
        /// </summary>
        public int package_fee { get; set; }

        /// <summary>
        /// 优惠总金额，单位：分
        /// </summary>
        public int discount_fee { get; set; }

        /// <summary>
        /// 商户应收金额（百度物流），单位：分（自配送为用户实付）
        /// </summary>
        public int shop_fee { get; set; }

        /// <summary>
        /// 订单总金额，单位：分
        /// </summary>
        public int total_fee { get; set; }

        /// <summary>
        /// 用户实付金额，单位：分
        /// </summary>
        public int user_fee { get; set; }

        /// <summary>
        /// 付款类型: 1-线下,2-在线
        /// </summary>
        public int pay_type { get; set; }

        /// <summary>
        /// 付款状态: 1-未付,2-已付
        /// </summary>
        public int pay_status { get; set; }

        /// <summary>
        /// 是否需要发票: 1-是,2-否
        /// </summary>
        public int need_invoice { get; set; }

        /// <summary>
        /// 发票抬头
        /// </summary>
        public string invoice_title { get; set; }

        /// <summary>
        /// 纳税人识别号
        /// </summary>
        public string taxer_id { get; set; }

        /// <summary>
        /// 订单备注
        /// </summary>
        public string remark { get; set; }

        /// <summary>
        /// 物流类型：1-百度,2-自配送
        /// </summary>
        public int delivery_party { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public string create_time { get; set; }

        /// <summary>
        /// 餐具数量
        /// </summary>
        public string meal_num { get; set; }

        /// <summary>
        /// 取消订单责任承担方
        /// </summary>
        public string responsible_party { get; set; }

        /// <summary>
        /// 佣金，单位：分
        /// </summary>
        public int commission { get; set; }
    }

    /// <summary>
    /// 订单客户信息
    /// </summary>
    public class BaiduOrderUserModel
    {
        /// <summary>
        /// 客户名称
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// 客户所在省份
        /// </summary>
        public string province { get; set; }

        /// <summary>
        /// 客户所在城市
        /// </summary>
        public string city { get; set; }

        /// <summary>
        /// 客户所在区
        /// </summary>
        public string district { get; set; }

        /// <summary>
        /// 客户地址
        /// </summary>
        public string address { get; set; }

        /// <summary>
        /// 客户电话
        /// </summary>
        public string phone { get; set; }

        /// <summary>
        /// 顾客性别： 1-男,2-女
        /// </summary>
        public int gender { get; set; }

        /// <summary>
        /// 客户经纬度
        /// </summary>
        public object coord { get; set; }
    }

    /// <summary>
    /// 订单门店信息
    /// </summary>
    public class BaiduOrderShopModel
    {
        /// <summary>
        /// 商户ID
        /// </summary>
        public string id { get; set; }

        /// <summary>
        /// 百度商户名称
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// 百度商户ID
        /// </summary>
        public string baidu_shop_id { get; set; }
    }

    /// <summary>
    /// 订单商品-菜品信息
    /// </summary>
    public class BaiduOrderProductDishModel
    {
        /// <summary>
        /// 百度商品ID
        /// </summary>
        public string baidu_product_id { get; set; }

        /// <summary>
        /// 第三方菜品ID【可选】
        /// </summary>
        public string other_dish_id { get; set; }

        /// <summary>
        /// 商品UPC【可选】
        /// </summary>
        public string upc { get; set; }

        /// <summary>
        /// 菜品类型,1-单品,2-套餐,3-配料
        /// </summary>
        public int product_type { get; set; }

        /// <summary>
        /// 商品名称
        /// </summary>
        public string product_name { get; set; }

        /// <summary>
        /// 商品份数
        /// </summary>
        public int product_amount { get; set; }

        /// <summary>
        /// 价格
        /// </summary>
        public int product_price { get; set; }

        /// <summary>
        /// 商品规格
        /// </summary>
        public object product_attr { get; set; }

        /// <summary>
        /// 商品属性
        /// </summary>
        public object product_features { get; set; }

        /// <summary>
        /// 商品总价，单位：分
        /// </summary>
        public int product_fee { get; set; }

        /// <summary>
        /// 餐盒总价，单位：分
        /// </summary>
        public int package_fee { get; set; }

        /// <summary>
        /// 餐盒单价，单位：分
        /// </summary>
        public int package_price { get; set; }

        /// <summary>
        /// 餐盒数量
        /// </summary>
        public int package_amount { get; set; }

        /// <summary>
        /// 商品总价，单位：分
        /// </summary>
        public int total_fee { get; set; }

        /// <summary>
        /// 商品唯一串
        /// </summary>
        public string product_custom_index { get; set; }
    }

    /// <summary>
    /// 订单优惠信息
    /// </summary>
    public class BaiduOrderDiscountModel
    {
        /// <summary>
        /// 优惠类型
        /// </summary>
        public string type { get; set; }

        /// <summary>
        /// 优惠金额 单位：分
        /// </summary>
        public string fee { get; set; }

        /// <summary>
        /// 活动ID
        /// </summary>
        public string activity_id { get; set; }

        /// <summary>
        /// 百度承担金额
        /// </summary>
        public string baidu_rate { get; set; }

        /// <summary>
        /// 商户承担金额
        /// </summary>
        public string shop_rate { get; set; }

        /// <summary>
        /// 代理商承担金额
        /// </summary>
        public string agent_rate { get; set; }

        /// <summary>
        /// 物流承担金额
        /// </summary>
        public string logistics_rate { get; set; }

        /// <summary>
        /// 优惠描述
        /// </summary>
        public string desc { get; set; }

        /// <summary>
        /// 特价菜(单品)
        /// </summary>
        public object products { get; set; }
    }

    /// <summary>
    /// 订单退款信息
    /// </summary>
    public class BaiduOrderRefundModel
    {
        /// <summary>
        /// 部分退款状态(订单部分退款状态：10-商户发起申请,20-用户同意,50-用户/平台拒绝)
        /// </summary>
        public int status { get; set; }

        /// <summary>
        /// 部分退款后用户实际支付价 单位：分
        /// </summary>
        public int total_price { get; set; }

        /// <summary>
        /// 部分退款后商户应收 单位：分
        /// </summary>
        public int shop_fee { get; set; }

        /// <summary>
        /// 部分退款后订单总价 单位：分
        /// </summary>
        public int order_price { get; set; }

        /// <summary>
        /// 部分退款后餐盒费 单位：分
        /// </summary>
        public int package_fee { get; set; }

        /// <summary>
        /// 部分退款后配送费 单位：分
        /// </summary>
        public int send_fee { get; set; }

        /// <summary>
        /// 部分退款后优惠总金额 单位：分
        /// </summary>
        public int discount_fee { get; set; }

        /// <summary>
        /// 部分退款后佣金 单位：分
        /// </summary>
        public int commision { get; set; }

        /// <summary>
        /// 部分退款后骑士结算价 单位：分
        /// </summary>
        public int shop_show_price { get; set; }

        /// <summary>
        /// 部分退款总额 单位：分
        /// </summary>
        public int refund_price { get; set; }

        /// <summary>
        /// 部分退回餐盒金额 单位：分
        /// </summary>
        public int refund_box_price { get; set; }

        /// <summary>
        /// 退回配送费金额 单位：分
        /// </summary>
        public int refund_send_price { get; set; }

        /// <summary>
        /// 由于退款原订单减少优惠金额 单位：分
        /// </summary>
        public int refund_discount_price { get; set; }

        /// <summary>
        /// 部分退款拒绝操作者
        /// </summary>
        public int refuse_platform { get; set; }

        /// <summary>
        /// 部分退回餐盒金额 单位：分
        /// </summary>
        public object order_detail { get; set; }

        /// <summary>
        /// 退回配送费金额 单位：分
        /// </summary>
        public object refund_detail { get; set; }

        /// <summary>
        /// 由于退款原订单减少优惠金额 单位：分
        /// </summary>
        public object discount { get; set; }
    }

    /// <summary>
    /// 订单商品-套餐信息
    /// </summary>
    public class BaiduOrderProductComboModel
    {
        /// <summary>
        /// 百度商品ID
        /// </summary>
        public string baidu_product_id { get; set; }

        /// <summary>
        /// 第三方套餐ID
        /// </summary>
        public string product_id { get; set; }

        /// <summary>
        /// 菜品类型,1-单品,2-套餐,3-配料
        /// </summary>
        public string product_type { get; set; }

        /// <summary>
        /// 套餐名称
        /// </summary>
        public string product_name { get; set; }

        /// <summary>
        /// 套餐份数
        /// </summary>
        public string product_amount { get; set; }

        /// <summary>
        /// 价格
        /// </summary>
        public string product_price { get; set; }

        /// <summary>
        /// 套餐总价，单位：分
        /// </summary>
        public int product_fee { get; set; }

        /// <summary>
        /// 餐盒总价，单位：分
        /// </summary>
        public int package_fee { get; set; }

        /// <summary>
        /// 餐盒单价，单位：分
        /// </summary>
        public int package_price { get; set; }

        /// <summary>
        /// 餐盒数量
        /// </summary>
        public int package_amount { get; set; }

        /// <summary>
        /// 商品总价，单位：分
        /// </summary>
        public int total_fee { get; set; }

        /// <summary>
        /// 商品唯一串
        /// </summary>
        public string product_custom_index { get; set; }

        /// <summary>
        /// 是否固定价格:1-是,2-否
        /// </summary>
        public int is_fixed_price { get; set; }

        /// <summary>
        /// 套餐内组合
        /// </summary>
        public object group { get; set; }
    }

    /// <summary>
    /// 订单商品-套餐组合信息
    /// </summary>
    public class BaiduOrderComboGroupModel
    {
        /// <summary>
        /// 组合名称
        /// </summary>
        public string group_name { get; set; }

        /// <summary>
        /// 百度组合ID
        /// </summary>
        public string baidu_group_id { get; set; }

        /// <summary>
        /// 组合内单品，菜品和单菜一致，下面以配料为例
        /// </summary>
        public object product { get; set; }
    }

    /// <summary>
    /// 订单商品-套餐组合内单品信息
    /// </summary>
    public class BaiduOrderComboDishModel
    {
        /// <summary>
        /// 百度配料ID
        /// </summary>
        public string baidu_product_id { get; set; }

        /// <summary>
        /// 第三方配料ID
        /// </summary>
        public string product_id { get; set; }

        /// <summary>
        /// 配料名称
        /// </summary>
        public string product_name { get; set; }

        /// <summary>
        /// 配料类型
        /// </summary>
        public string product_type { get; set; }

        /// <summary>
        /// 配料数量
        /// </summary>
        public int product_amount { get; set; }

        /// <summary>
        /// 配料总价格
        /// </summary>
        public int product_fee { get; set; }

        /// <summary>
        /// 配料价格
        /// </summary>
        public int product_price { get; set; }
    }

    /// <summary>
    /// 商品规格信息
    /// </summary>
    public class OrderProductAttrModel
    {
        /// <summary>
        /// 规格【主要】
        /// </summary>
        public string option { get; set; }

        /// <summary>
        /// 规格名称
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// 百度规格ID
        /// </summary>
        public string baidu_attr_id { get; set; }

        /// <summary>
        /// 第三方规格ID
        /// </summary>
        public string attr_id { get; set; }
    }

    /// <summary>
    /// 商品属性信息
    /// </summary>
    public class OrderProductFeaturesModel
    {
        /// <summary>
        /// 属性【主要】
        /// </summary>
        public string option { get; set; }

        /// <summary>
        /// 属性名称
        /// </summary>
        public object name { get; set; }

        /// <summary>
        /// 百度属性ID
        /// </summary>
        public string baidu_feature_id { get; set; }
    }

    /// <summary>
    /// 优惠列表
    /// </summary>
    public class OrderDiscountProductsModel
    {
        /// <summary>
        /// 百度菜品ID
        /// </summary>
        public string baidu_product_id { get; set; }

        /// <summary>
        /// 原始价
        /// </summary>
        public string orig_price { get; set; }

        /// <summary>
        /// 优惠金额
        /// </summary>
        public string save_price { get; set; }

        /// <summary>
        /// 优惠后金额
        /// </summary>
        public string now_price { get; set; }
    }
}

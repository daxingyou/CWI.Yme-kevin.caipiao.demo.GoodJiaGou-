
namespace Yme.Mcp.Model.Waimai.Meituan
{
    /// <summary>
    /// 美团订单菜品详情
    /// </summary>
    public class MeituanOrderDetailModel
    {
        /// <summary>
        /// erp方菜品id（等价于eDishCode）
        /// </summary>
        public string app_food_code { get; set; }

        /// <summary>
        /// 餐盒数量（例如一份菜A需要1个餐盒，订单中点了2份菜A，餐盒数量为2）
        /// </summary>
        public int box_num { get; set; }

        /// <summary>
        /// 餐盒单价
        /// </summary>
        public float box_price { get; set; }

        /// <summary>
        /// 菜品名
        /// </summary>
        public string food_name { get; set; }

        /// <summary>
        /// 价格（菜品原价）
        /// </summary>
        public float price { get; set; }

        /// <summary>
        /// erp方菜品sku
        /// </summary>
        public string sku_id { get; set; }

        /// <summary>
        /// 菜品份数
        /// </summary>
        public int quantity { get; set; }

        /// <summary>
        /// 单位（如：份）
        /// </summary>
        public string unit { get; set; }

        /// <summary>
        /// 菜品折扣(如：0.8，只是美团商家、APP方配送的门店才会设置，默认为1。折扣值不参与总价计算。)
        /// </summary>
        public float food_discount { get; set; }

        /// <summary>
        /// 菜品规格
        /// </summary>
        public string spec { get; set; }

        /// <summary>
        /// 菜品属性,如："中辣,微甜",多个属性用英文逗号隔开
        /// </summary>
        public string food_property { get; set; }

        /// <summary>
        /// 商品所在的口袋(0-1号口袋，1-2号口袋，以此类推)
        /// </summary>
        public int cart_id { get; set; }
    }
}

namespace Yme.Util
{
    /// <summary>
    /// 正则常量
    /// </summary>
    public class RegexConsts
    {
        /// <summary>
        /// 全数字
        /// </summary>
        public const string ALL_NUM_PARTTERN = @"^[0-9]*$";

        /// <summary>
        /// 用户登录帐号名称表达式
        /// </summary>
        public const string USERACCOUNT_PATTERN = @"^[a-zA-Z0-9_\-]+$";

        /// <summary>
        /// 用户登录密码表达式
        /// </summary>
        public const string USERPASSWORD_PATTERN = @"^[a-zA-Z0-9_\-]+$";

        /// <summary>
        /// 多个整数用逗号组成的字符串
        /// </summary>
        public const string INT_PATTERN_LIST = @"^\d+(,\d+)*$";

        /// <summary>
        /// GUID
        /// </summary>
        public const string GUID = @"^(0|([a-zA-Z0-9\-]{36}))$";

        /// <summary>
        /// 多个GUID用逗号组成的字符串
        /// </summary>
        public const string GUID_PATTERN_LIST = @"^[a-zA-Z0-9\-]{36}(\,[a-zA-Z0-9\-]{36})*$";

        /// <summary>
        /// 四位增长的全数字编号(比如：资讯分类ID,城市ID)
        /// </summary>
        public const string CODE_WITH_DIGIT_FOR_FOUR_INCREASE = @"^(0|([0-9]{4})+)$";

        /// <summary>
        /// 评论分数(1.0-5.0)
        /// </summary>
        public const string COMMENT_SCORE = @"^[1-4]{1}(.[0-9]{1})?$|^[5]{1}(.[0]{1})?$";

        /// <summary>
        /// 区域ID
        /// </summary>
        public const string DISTRICT_ID = @"^(0|(\d{4})+)$";

        /// <summary>
        /// 排序值
        /// </summary>
        public const string ORDER_FIELD = @"^[0-6]{1}$";

        /// <summary>
        /// 排序类型
        /// </summary>
        public const string ORDER_TYPE = @"^[0,1]{1}$";

        /// <summary>
        /// 布尔类型 0，1
        /// </summary>
        public const string BOOL_TYPE = @"^[0,1]{1}$";

        /// <summary>
        /// 字符串默认
        /// </summary>
        public const string STRING_DEFAULT = @"^[0]{1}$";

        /// <summary>
        /// 金额（整数位最多十位，小数为最多为两位，可以无小数位）
        /// </summary>
        public const string AMOUNT_PATTERN = @"^(([0-9]|([1-9][0-9]{0,9}))((\.[0-9]{1,2})?))$";

        /// <summary>
        /// 百分比不含百分号（整数位最多2位，小数为最多为两位，可以无小数位）
        /// </summary>
        public const string PERCENTAGE_PATTERN = @"^(([0-9]|([1-9][0-9]{0,2}))((\.[0-9]{1,2})?))$";

        /// <summary>
        /// 打印类型第一二位01代表文档，02代表图片，第三四位01代表黑白，02代表彩色，第五六位表示尺寸
        /// </summary>
        public const string PRINTER_TYPE_PATTERN = @"^[0-9]{4}[a-zA-Z0-9#]{2}$";

        /// <summary>
        /// 店铺名称正则（数字字母和汉字）
        /// </summary>
        public const string SHOP_NAME_PATTERN = @"^[a-zA-Z0-9\u4e00-\u9fa5]+$";

        /// <summary>
        /// 16进制组成
        /// </summary>
        public const string BINARY_16_PATTERN = @"^[a-fA-F0-9]{12}$";

        /// <summary>
        /// 支付宝帐号（手机号或者Emall）
        /// </summary>
        public const string ALIPAY_PATTERN = @"^(1[3,4,5,7,8]{1}[0-9]{1}[0-9]{8})|([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";

        /// <summary>
        /// 图片验证码
        /// </summary>
        public const string IMAGE_CODE_PATTERN = @"[a-zA-Z0-9]{4}$";

        /// <summary>
        /// 版本号
        /// </summary>
        public const string VERSION_PATTERN = @"^[0-9]+(\.[0-9]){2}$";

        /// <summary>
        /// 手机号表达式
        /// </summary>
        public const string MOBILE_PATTERN = @"^1[3,4,5,7,8]{1}[0-9]{1}[0-9]{8}$";

        /// <summary>
        /// 邮箱地址【[\w!#$%&'*+/=?^_`{|}~-]+(?:\.[\w!#$%&'*+/=?^_`{|}~-]+)*@(?:[\w](?:[\w-]*[\w])?\.)+[\w](?:[\w-]*[\w])?】
        /// </summary>
        public const string IS_EMAIL = @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";

        /// <summary>
        /// 中文字符
        /// </summary>
        public const string IS_CHINESS_CHAR = @"[\u4e00-\u9fa5]";

        /// <summary>
        /// 双字节字符【包括汉字】
        /// </summary>
        public const string IS_DOUBLE_BYTE_CHAR = @"^[^\x00-\xff]+$";

        /// <summary>
        /// 空白行
        /// </summary>
        public const string IS_SPACE = @"\n\s*\r";

        /// <summary>
        /// 网址URL
        /// </summary>
        public const string IS_URL = @"[a-zA-z]+://[^\s]*";

        /// <summary>
        /// 中国大陆固话
        /// </summary>
        public const string IS_CHINESS_TELPHONE = @"\d{3,4}-\d{8}|\d{4}-\{7,8}";

        /// <summary>
        /// 腾讯QQ
        /// </summary>
        public const string IS_QQ = @"[1-9][0-9]{4,}";

        /// <summary>
        /// 中国大陆邮编
        /// </summary>
        public const string IS_CHINESS_ZIPCODE = @"[1-9]\d{5}(?!\d)";

        /// <summary>
        /// 中国公民18位身份证号
        /// </summary>
        public const string IS_CHINESS_ID = @"^(\d{6})(\d{4})(\d{2})(\d{2})(\d{3})([0-9]|X)$";

        #region 日期时间

        /// <summary>
        /// 年月
        /// </summary>
        public const string YEAR_MONTH = @"^[12]\d{3}((0[1-9])|1[0-2])$";

        /// <summary>
        /// 年月日
        /// </summary>
        public const string YEAR_MONTH_DAY = @"^[12]\d{3}-(0[1-9])|1[0-2]-(0[1-9])|[1-3][0-1]$";

        /// <summary>
        /// 日期格式【年月日】
        /// </summary>
        public const string IS_DATE = @"([0-9]{3}[1-9]|[0-9]{2}[1-9][0-9]{1}|[0-9]{1}[1-9][0-9]{2}|[1-9][0-9]{3})-(((0[13578]|1[02])-(0[1-9]|[12][0-9]|3[01]))|((0[469]|11)-(0[1-9]|[12][0-9]|30))|(02-(0[1-9]|[1][0-9]|2[0-8])))";

        #endregion

        #region 数字

        /// <summary>
        /// 真假值：0或1
        /// </summary>
        public const string NUM_BOOL = @"[0,1]";

        /// <summary>
        /// 整数：1或2
        /// </summary>
        public const string NUM_ONE_TWO = @"[1,2]";

        /// <summary>
        /// 正整数
        /// </summary>
        public const string INT_FOR_GREAT_ZERO = @"^[1-9]\d*$";

        /// <summary>
        /// 负整数
        /// </summary>
        public const string INT_FOR_LESS_ZERO = @"^-[1-9]\d*$";

        /// <summary>
        /// 大于一的整数
        /// </summary>
        public const string INT_FOR_GREAT_ONE = @"^[2-9]{1}[0-9]*$";

        /// <summary>
        /// 整数
        /// </summary>
        public const string INT = @"^-?[1-9]\d*$";

        /// <summary>
        /// 非负整数
        /// </summary>
        public const string INT_FOR_NOLESS_ZERO = @"^[1-9]\d*|0$";

        /// <summary>
        /// 非正整数
        /// </summary>
        public const string INT_FOR_NOGREAT_ZERO = @"^-[1-9]\d*|0$";

        /// <summary>
        /// 正浮点数
        /// </summary>
        public const string FLOAT_FOR_GRATE_ZERO = @"^[1-9]\d*\.\d*|0\.\d*[1-9]\d*$";

        /// <summary>
        /// 负浮点数
        /// </summary>
        public const string FLOAT_FOR_LESS_ZERO = @"^-[1-9]\d*\.\d*|-0\.\d*[1-9]\d*$";

        #endregion

    }
}

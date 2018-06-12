using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;

namespace Yme.Util.Extension
{
    /// <summary>
    /// 获取实体类Attribute自定义属性
    /// </summary>
    /// </summary>
    [Serializable]
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = true, Inherited = true)]
    public class EnumAttribute : Attribute
    {
        // <summary>
        /// 描述
        /// </summary>
        public string Description { set; get; }

        /// <summary>
        /// 锁变量
        /// </summary>
        private static object locker = new object();

        /// <summary>
        /// 缓存数据
        /// </summary>
        private static Dictionary<string, string> enumDescriptions = new Dictionary<string, string>();

        /// <summary>
        /// 描述
        /// </summary>
        /// <param name="des">描述内容</param>
        public EnumAttribute(string des)
        {
            this.Description = des;
        }

        /// <summary>
        /// 根据动作得到描述
        /// </summary>
        /// <param name="a">枚举</param>
        /// <returns>返回枚举描述</returns>
        public static string GetDescription(Enum a)
        {
            Type type = a.GetType();
            return GetDescription(type, a);
        }

        /// <summary>
        /// 获取枚举描述信息
        /// </summary>
        /// <param name="enumType"></param>
        /// <returns></returns>
        public static string GetDescription(Type enumType, Enum enumValue)
        {
            Type type = enumType;
            string key = type.ToString() + "." + enumValue.ToString();
            if (enumDescriptions.ContainsKey(key))
                return enumDescriptions[key];

            FieldInfo f = type.GetField(enumValue.ToString());
            string result = string.Empty;
            if (f != null)
            {
                EnumAttribute attr = Attribute.GetCustomAttribute(f, typeof(EnumAttribute)) as EnumAttribute;
                if (attr != null)
                    result = attr.Description;
                lock (locker)
                {
                    enumDescriptions.Add(key, result);
                }
                return result;
            }

            return result;
        }

        /// <summary>
        /// 返回枚举项的描述信息。
        /// </summary>
        /// <param name="value">要获取描述信息的枚举项。</param>
        /// <returns>枚举想的描述信息。</returns>
        public static string GetDescription1(Enum value)
        {
            Type enumType = value.GetType();
            // 获取枚举常数名称。
            string name = Enum.GetName(enumType, value);
            if (name != null)
            {
                // 获取枚举字段。
                FieldInfo fieldInfo = enumType.GetField(name);
                if (fieldInfo != null)
                {
                    // 获取描述的属性。
                    DescriptionAttribute attr = Attribute.GetCustomAttribute(fieldInfo,
                        typeof(DescriptionAttribute), false) as DescriptionAttribute;
                    if (attr != null)
                    {
                        return attr.Description;
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// 枚举值转换为键值对列表
        /// </summary>
        /// <param name="enumType"></param>
        /// <returns></returns>
        public static Dictionary<int, string> GetDictionary(Enum enumType)
        {
            Dictionary<int, string> result = new Dictionary<int, string>();
            foreach (var item in Enum.GetValues(enumType.GetType()))
            {
                result.Add(item.GetHashCode(), item.ToString());
            }
            return result;
        }
    }
}

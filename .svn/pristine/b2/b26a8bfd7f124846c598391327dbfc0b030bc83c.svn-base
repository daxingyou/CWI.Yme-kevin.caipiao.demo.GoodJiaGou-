using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace Yme.Util.Extension
{
    /// <summary>
    /// 枚举扩展
    /// </summary>
    public static partial class Extensions
    {
        /// <summary>
        /// 获取枚举描述
        /// </summary>
        /// <param name="type">枚举值</param>
        /// <returns>枚举描述</returns>
        public static string GetRemark<T>(this T type) where T : struct
        {
            if (type is Enum)
            {
                var t = type as Enum;
                return GetDescription(t);
            }
            return string.Empty;
        }

        /// <summary>
        /// 获取对应枚举的所有值
        /// </summary>
        /// <param name="type">枚举值</param>
        /// <returns>枚举的所有值</returns>
        public static List<T> List<T>(this T type) where T : struct
        {
            if (type is Enum)
            {
                return Enum.GetValues(type.GetType()).Cast<T>().ToList();
            }
            return new List<T>();
        }

        /// <summary>
        /// 返回枚举项的描述信息。
        /// </summary>
        /// <param name="value">要获取描述信息的枚举项。</param>
        /// <returns>枚举想的描述信息。</returns>
        private static string GetDescription(Enum value)
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
                    EnumAttribute attr = Attribute.GetCustomAttribute(fieldInfo, typeof(EnumAttribute)) as EnumAttribute;
                    if (attr != null)
                    {
                        return attr.Description;
                    }
                }
            }
            return null;
        }
    }
}

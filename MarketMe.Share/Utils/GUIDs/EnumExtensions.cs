

//using System;
//using System.ComponentModel;

//namespace MarketMe.Share.Utils.GUIDs
//{
//    public static class EnumExtensions
//    {
//        public const int OK = 200;

//        public static T Parse<T>(this string value)
//        {
//            try
//            {
//                if (string.IsNullOrEmpty(value)) return default;
//                return (T)Enum.Parse(typeof(T), value, true);
//            }
//            catch (Exception)
//            {
//                return default;
//            }
//        }
//        public static T ToEnum<T>(this string value) where T : struct
//        {
//            if (Enum.TryParse(value, out T result))
//            {
//                return result;
//            }
//            else
//            {
//                return default;
//            }
//        }
//        public static string GetName(this Enum value)
//        {
//            return Enum.GetName(value.GetType(), value);
//        }
//        public static T ToEnum<T>(this int value) where T : struct
//        {
//            return value.ToString().ToEnum<T>();
//        }

//        public static T GetDefaultValue<T>(this Enum value)
//        {
//            var fieldInfo = value.GetType().GetField(value.GetName());
//            var defaultValueAttribute = fieldInfo.GetCustomAttributes(typeof(DefaultValueAttribute), false).FirstOrDefault() as DefaultValueAttribute;
//            return defaultValueAttribute == null ? default : (T)defaultValueAttribute.Value;
//        }

//        public static string GetDescription(this Enum value)
//        {
//            var fieldInfo = value.GetType().GetField(value.GetName());
//            var descriptionAttribute = fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false).FirstOrDefault() as DescriptionAttribute;
//            return descriptionAttribute == null ? value.GetName() : descriptionAttribute.Description;
//        }

//        public static T GetValueFromDescription<T>(string description)
//        {
//            var type = typeof(T);
//            if (!type.IsEnum) throw new InvalidOperationException();
//            foreach (var field in type.GetFields())
//            {
//                var attribute = Attribute.GetCustomAttribute(field,
//                    typeof(DescriptionAttribute)) as DescriptionAttribute;
//                if (attribute != null)
//                {
//                    if (attribute.Description == description)
//                        return (T)field.GetValue(null);
//                }
//                else
//                {
//                    if (field.Name == description)
//                        return (T)field.GetValue(null);
//                }
//            }
//            throw new ArgumentException("Not found.", "description");
//            // or return default(T);
//        }
//    }
//}

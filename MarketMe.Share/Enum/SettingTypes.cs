using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Shared.Enums
{
    public enum SettingTypes
    {
        [DefaultValue(7.5), Description("VAT TAX")]
        TAX,
        [DefaultValue(0.0), Description("Extra Charges")]
        EXTRA_CHARGES,
        [DefaultValue("NGN"), Description("Currency Code")]
        CURRENCY_CODE,
        [DefaultValue("₦"), Description("Currency Symbol")]
        CURRENCY_SYMBOL,
    }

    public static class SettingsDataTypeConverter
    {

        public static object GetKeyDefaultValue(this SettingTypes dataType)
        {
            return dataType switch
            {
                SettingTypes.TAX => SettingTypes.TAX.GetDefaultValues<double>(),
                SettingTypes.EXTRA_CHARGES => SettingTypes.EXTRA_CHARGES.GetDefaultValues<double>(),
                SettingTypes.CURRENCY_CODE => SettingTypes.CURRENCY_CODE.GetDefaultValues<string>(),
                SettingTypes.CURRENCY_SYMBOL => SettingTypes.CURRENCY_SYMBOL.GetDefaultValues<string>(),
                _ => typeof(string),
            };
        }

        public static object TryConvertToObject(object obj, SettingTypes key)
        {
            if (obj == null)
            {
                return key.GetKeyDefaultValue();
            }

            try
            {
                return key switch
                {
                    SettingTypes.TAX => Convert.ChangeType(obj, typeof(double)),
                    SettingTypes.EXTRA_CHARGES => Convert.ChangeType(obj, typeof(double)),
                    SettingTypes.CURRENCY_CODE => Convert.ChangeType(obj, typeof(string)),
                    SettingTypes.CURRENCY_SYMBOL => Convert.ChangeType(obj, typeof(string)),
                    _ => null,
                };
            }
            catch (Exception)
            {
                return null;
            }
        }
        public const int OK = 200;

        public static T Parse<T>(this string value)
        {
            try
            {
                if (string.IsNullOrEmpty(value)) return default(T);
                return (T)Enum.Parse(typeof(T), value, true);
            }
            catch (Exception)
            {
                return default(T);
            }
        }
        public static T ToEnum<T>(this string value) where T : struct
        {
            if (Enum.TryParse(value, out T result))
            {
                return result;
            }
            else
            {
                return default;
            }
        }
        public static string GetName(this Enum value)
        {
            return Enum.GetName(value.GetType(), value);
        }
        public static T ToEnum<T>(this int value) where T : struct
        {
            return value.ToString().ToEnum<T>();
        }

        public static T GetDefaultValues<T>(this Enum value)
        {
            var fieldInfo = value.GetType().GetField(value.GetName());
            var defaultValueAttribute = fieldInfo.GetCustomAttributes(typeof(DefaultValueAttribute), false).FirstOrDefault() as DefaultValueAttribute;
            return defaultValueAttribute == null ? default(T) : (T)defaultValueAttribute.Value;
        }

        public static string GetDescription(this Enum value)
        {
            var fieldInfo = value.GetType().GetField(value.GetName());
            var descriptionAttribute = fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false).FirstOrDefault() as DescriptionAttribute;
            return descriptionAttribute == null ? value.GetName() : descriptionAttribute.Description;
        }

        public static T GetValueFromDescription<T>(string description)
        {
            var type = typeof(T);
            if (!type.IsEnum) throw new InvalidOperationException();
            foreach (var field in type.GetFields())
            {
                var attribute = Attribute.GetCustomAttribute(field,
                    typeof(DescriptionAttribute)) as DescriptionAttribute;
                if (attribute != null)
                {
                    if (attribute.Description == description)
                        return (T)field.GetValue(null);
                }
                else
                {
                    if (field.Name == description)
                        return (T)field.GetValue(null);
                }
            }
            throw new ArgumentException("Not found.", "description");
            // or return default(T);
        }
    }
}

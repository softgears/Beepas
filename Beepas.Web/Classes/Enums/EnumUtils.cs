using System.Linq;
using System.Reflection;

namespace Beepas.Web.Classes.Enums
{
    /// <summary>
    /// ��������������� ����� ��� ������ � ��������������
    /// </summary>
    public static class EnumUtils
    {
        /// <summary>
        /// ��������� ������� Enum Description �� ����� ������������ ��� ��������� ������
        /// </summary>
        /// <typeparam name="TEnum">��� ������������</typeparam>
        /// <param name="enumValue">������� �� ������������</param>
        /// <param name="locale">������, �� ��������� ru-RU</param>
        /// <returns>������</returns>
        public static string GetEnumMemberName<TEnum>(this TEnum enumValue, string locale = "ru-RU")
        {
            var ti = enumValue.GetType();
            var result = enumValue.ToString();
            foreach (var attr in from fieldInfo in ti.GetFields(BindingFlags.Public | BindingFlags.Static)
                                 where fieldInfo.Name.Equals(enumValue.ToString())
                                 select (EnumDescriptionAttribute[])fieldInfo.GetCustomAttributes(typeof(EnumDescriptionAttribute), false)
                                     into attrs
                                     where attrs.Length > 0
                                     select Enumerable.FirstOrDefault(attrs, r => r.Locale == locale))
            {
                result = attr.Name;
            }
            return result;
        }
    }
}
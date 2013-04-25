using System;

namespace Beepas.Web.Classes.Enums
{
    /// <summary>
    /// �������� ���������� ������� ������������ � ������� �����
    /// </summary>
    public class EnumDescriptionAttribute: Attribute
    {
        /// <summary>
        /// �������� �������� � ������������
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// ������ � ������� ������ ��� ������ ��������������
        /// </summary>
        public string Locale { get; set; }

        /// <summary>
        /// ������� ����� ������� ���������� ������������
        /// </summary>
        /// <param name="name"></param>
        public EnumDescriptionAttribute(string name)
        {
            Name = name;
            Locale = "ru-RU";
        }
    }
}
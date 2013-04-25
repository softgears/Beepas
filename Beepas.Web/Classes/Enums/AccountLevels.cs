namespace Beepas.Web.Classes.Enums
{
    /// <summary>
    /// ������ ��������
    /// </summary>
    public enum AccountLevels: short
    {
        /// <summary>
        /// ������� ������������
        /// </summary>
        [EnumDescription("�������")]
        Common = 0,

        /// <summary>
        /// ��� ������������
        /// </summary>
        [EnumDescription("VIP")]
        VIP = 1
    }
}
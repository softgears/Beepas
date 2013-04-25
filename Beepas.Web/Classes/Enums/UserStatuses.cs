namespace Beepas.Web.Classes.Enums
{
    /// <summary>
    /// ������� �������������
    /// </summary>
    public enum UserStatuses: short
    {
        /// <summary>
        /// ������������ �������
        /// </summary>
        [EnumDescription("�������")]
        Active = 0,

        /// <summary>
        /// ������������ ������������
        /// </summary>
        [EnumDescription("������������")]
        Blocked = 1
    }
}
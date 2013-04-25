namespace Beepas.Web.Classes.Enums
{
    /// <summary>
    /// ����������� ��������
    /// </summary>
    public enum PaymentDirection: short
    {
        /// <summary>
        /// ����������
        /// </summary>
        [EnumDescription("����������")]
        Income = 1,

        /// <summary>
        /// ��������
        /// </summary>
        [EnumDescription("��������")]
        Outcome = 2
    }
}
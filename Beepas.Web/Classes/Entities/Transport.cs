using System;

namespace Beepas.Web.Classes.Entities
{
    /// <summary>
    /// Транспортное средство, перевозящее мед на биржу и обратно
    /// </summary>
    public partial class Transport
    {
        /// <summary>
        /// Процент заполненности транспортного средства
        /// </summary>
        public string FillPercentage
        {
            get { return String.Format("{0:0}%", Math.Round(CurrentHoney / TransportType.Capacity * 100)); }
        }
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DataAccessLibrary.Models
{
    [Table("FinalOrder")]
    public class Order //Модель заказов
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime DeliveryDate { get; set; }
        public string ClientName { get; set; }
        public string PickupCode { get; set; }
        public string Status { get; set; }
    }
}

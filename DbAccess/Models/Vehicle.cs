using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbAccess.Models
{
    public class Vehicle
    {
        [Key]
        [Required]
        public string SalesID { get; set; }

        public string SalesName { get; set; }

        public string HP_ModelID { get; set; }

        public string HP_VIN { get; set; }

        public string HP_ColorName { get; set; }

        public string HP_DeliveryTime { get; set; }

        public string SalesStatus { get; set; }

        public string HP_DealerOrderStatus { get; set; }

        public DateTime ReceiptDateConfirmed { get; set; }
    }
}

using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApiAzureAcitveDirectoryB2C.Models
{
    public class OrderModel
    {
        public string OrderID { get; set; }
        public string ShipperName { get; set; }
        public string ShipperCity { get; set; }
        public DateTimeOffset TS { get; set; }
    }

    public class OrderEntity : TableEntity
    {
        public OrderEntity(string userId)
        {
            this.PartitionKey = userId;
            this.RowKey = Guid.NewGuid().ToString("N");
        }

        public OrderEntity() { }

        public string ShipperName { get; set; }

        public string ShipperCity { get; set; }

    }
}
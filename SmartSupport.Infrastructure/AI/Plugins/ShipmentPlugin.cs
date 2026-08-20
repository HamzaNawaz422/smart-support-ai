using Microsoft.SemanticKernel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace SmartSupport.Infrastructure.AI.Plugins
{
    public class ShipmentPlugin
    {
        [KernelFunction]
        [Description("Gets the current status of shipment using its tracking number.")]
        public string getShipmentStatus([Description("the shipment tracking number")] string trackingNumber)
        {

            return trackingNumber switch
            {
                "SKY123" => "Shipment is out for delivery.",
                "SKY456" => "Shipment is currently in transit.",
                _=>"Shipment was not found."
            };
        }


        [KernelFunction]
        [Description("Gets all shipments that have the specified shipment status.")]
        public List<string> GetShipmentsByStatus(
        [Description("The shipment status, for example: in transit, out for delivery, or delivered")]
        string status)
        {
            var shipments = new Dictionary<string, string>
        {
            { "SKY123", "Out for delivery" },
            { "SKY456", "In transit" },
            { "SKY789", "Delivered" },
            { "SKY999", "Out for delivery" }
        };

            return shipments
                .Where(x => x.Value.Equals(
                    status,
                    StringComparison.OrdinalIgnoreCase))
                .Select(x => x.Key)
                .ToList();
        }
    }
}

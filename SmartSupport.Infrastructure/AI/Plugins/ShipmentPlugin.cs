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
    }
}

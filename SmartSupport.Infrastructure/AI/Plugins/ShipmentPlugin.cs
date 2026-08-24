using Microsoft.SemanticKernel;
using SmartSupport.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace SmartSupport.Infrastructure.AI.Plugins
{
    public class ShipmentPlugin
    {
        private readonly IShipmentService _shipmentService;
        public ShipmentPlugin(IShipmentService shipmentService)
        {
            _shipmentService = shipmentService;
        }
        [KernelFunction]
        [Description("Gets the current status of shipment using its tracking number.")]
        public Task<string> getShipmentStatus([Description("the shipment tracking number")] string trackingNumber)
        {
            return _shipmentService.GetShipmentStatusAsync(trackingNumber);


        }


        [KernelFunction]
        [Description("Gets all shipments that have the specified shipment status.")]
        public Task<List<string>> GetShipmentsByStatus(
        [Description("The shipment status, for example: in transit, out for delivery, or delivered")]
        string status)
        {
            return _shipmentService.GetShipmentsByStatusAsync(status);
        }
    }
}

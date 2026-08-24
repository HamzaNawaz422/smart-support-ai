using Microsoft.SemanticKernel;
using SmartSupport.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace SmartSupport.Infrastructure.AI
{
    public class ShipmentService : IShipmentService
    {
        private readonly Dictionary<string, string> _shipment = new Dictionary<string, string>
        {
            { "SKY123", "Out for delivery" },
            { "SKY456", "In transit" },
            { "SKY789", "Delivered" },
            { "SKY999", "Out for delivery" }
        };


        public Task<string> GetShipmentStatusAsync(string trackingNumber)
        {
            var response = _shipment.TryGetValue(trackingNumber, out var value) ? value : "Shipment was not fount";
            return Task.FromResult(response);
        }

        public Task<List<string>> GetShipmentsByStatusAsync(string status)
        {
            //var response = _shipment.Where(x => x.Value.Equals(status, StringComparison.OrdinalIgnoreCase))
            //    .Select(x => x.Key)
            //    .ToList();

            var shipments = _shipment
            .Where(x => x.Value.Equals(
                status,
                StringComparison.OrdinalIgnoreCase))
            .Select(x => x.Key)
            .ToList();

            return Task.FromResult(shipments);

        }
    }
}

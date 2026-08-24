using System;
using System.Collections.Generic;
using System.Text;

namespace SmartSupport.Application.Interfaces
{
    public interface IShipmentService
    {
        Task<string> GetShipmentStatusAsync(string trackingNumber);

        Task<List<string>> GetShipmentsByStatusAsync(string status);
    }
}

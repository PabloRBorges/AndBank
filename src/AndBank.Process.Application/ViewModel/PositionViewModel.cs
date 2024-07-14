using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AndBank.Process.Application.ViewModel
{
    public class PositionViewModel
    {
        public string? PositionId { get; set; }
        public string? ProductId { get; set; }
        public string? ClientId { get; set; }
        public DateTime Date { get; set; }
        public decimal Value { get; set; }
        public decimal Quantity { get; set; }

    }

    public class SummaryViewModel
    {
        public string ProductId { get; set; }
        public decimal TotalValue { get; set; }
    }
}

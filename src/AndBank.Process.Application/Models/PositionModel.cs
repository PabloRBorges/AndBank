using AndBank.Core.DomainObjects;
using AndBank.Core.Models;

namespace AndBank.Processs.Aplication
{
    public class PositionModel : Entity, IAggregateRoot
    {
        public string PositionId { get; set; }
        public string ProductId { get; set; }
        public string ClientId { get; set; }
        public DateTime Date { get; set; }
        public decimal Value { get; set; }
        public decimal Quantity { get; set; }
    }
}

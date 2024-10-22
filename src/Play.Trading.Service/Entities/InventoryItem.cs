using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Play.Common.Service.Entities;

namespace Play.Trading.Service.Entities
{
    public class InventoryItem : IEntity
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }

        public Guid CatalogItemId { get; set; }

        public int Quantity { get; set; }
    }
}
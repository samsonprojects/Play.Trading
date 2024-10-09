using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Play.Trading.Service.Exceptions
{
    [Serializable]
    internal class UnknownItemException : Exception
    {
        private Guid catalogItemId;


        public UnknownItemException(Guid itemId) : base($"Unknown item '{itemId}' ")
        {
            this.catalogItemId = itemId;
        }

        public Guid ItemId { get; }

    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Play.Trading.Service.Settings
{
    public class QueueSettings
    {
        public string GrantItemsQueueAddress { get; init; }
        public string DebitGilQueueAddress { get; init; }
        public string SubtractItemsQueueAddress { get; init; }

    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MassTransit;
using Play.Common;
using Play.Common.Service.Repositories;
using Play.Trading.Service.Entities;
using static Play.Catalog.Contracts.Contracts;

namespace Play.Trading.Service.Consumers
{
    public class CatalogItemUpdatedConsumer : IConsumer<CatalogItemUpdated>
    {
        private IRepository<CatalogItem> _repository;

        public CatalogItemUpdatedConsumer(IRepository<CatalogItem> repository)
        {
            _repository = repository;
        }
        public async Task Consume(ConsumeContext<CatalogItemUpdated> context)
        {
            var message = context.Message;

            var item = await _repository.GetAsync(message.ItemId);
            if (item == null)
            {
                item = new CatalogItem
                {
                    Id = message.ItemId,
                    Description = message.Description,
                    Name = message.Name,
                    Price = message.Price
                };

                await _repository.CreateAsync(item);
                return;
            }
            else
            {
                item.Description = message.Description;
                item.Name = message.Name;
                item.Price = message.Price;
                await _repository.UpdateAsync(item);
            }


        }
    }
}
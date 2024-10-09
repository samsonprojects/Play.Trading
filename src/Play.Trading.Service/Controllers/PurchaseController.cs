using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Play.Trading.Service.Dtos;
using Play.Trading.Service.StateMachines;

namespace Play.Trading.Service.Controllers
{
    [ApiController]
    [Route("purchase")]
    [Authorize]
    public class PurchaseController : ControllerBase
    {

        private readonly IPublishEndpoint publishEndpoint;
        private readonly IRequestClient<GetPurchaseState> purchaseClient;

        public PurchaseController(IPublishEndpoint publishEndpoint, IRequestClient<GetPurchaseState> purchaseClient)
        {
            this.publishEndpoint = publishEndpoint;
            this.purchaseClient = purchaseClient;
        }

        [HttpGet("status/{idempotencyId}")]
        public async Task<ActionResult<PurchaseDto>> GetStatusAsync(Guid idempotencyId)
        {
            var response = await purchaseClient
                .GetResponse<PurchaseState>(new GetPurchaseState(idempotencyId));

            var purchaseState = response.Message;
            var purchase = new PurchaseDto(
                purchaseState.UserId,
                purchaseState.ItemId,
                purchaseState.PurchaseTotal,
                purchaseState.Quantity,
                purchaseState.CurrentState,
                purchaseState.ErrorMessage,
                purchaseState.Received,
                purchaseState.LastUpdated

            );

            return Ok(purchase);
        }

        [HttpPost]
        public async Task<IActionResult> PostAsync(SubmitPurchaseDto purchaseDto)
        {
            var userId = User.FindFirstValue("sub");

            var message = new PurchaseRequested(
                Guid.Parse(userId),
                purchaseDto.ItemId.Value,
                purchaseDto.Quantity,
                purchaseDto.IdempotencyId
            );

            await publishEndpoint.Publish(message);
            return AcceptedAtAction(nameof(GetStatusAsync),
            new { purchaseDto.IdempotencyId },
            new { purchaseDto.IdempotencyId });
        }
    }
}
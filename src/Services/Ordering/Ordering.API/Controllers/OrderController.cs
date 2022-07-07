using MediatR;
using Microsoft.AspNetCore.Mvc;
using Ordering.Application.Features.Orders.Commands.CheckoutOrder;
using Ordering.Application.Features.Orders.Commands.DeleteOrder;
using Ordering.Application.Features.Orders.Commands.UpdateOrder;
using Ordering.Application.Features.Orders.Queries.GetOrderList;

namespace Ordering.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IMediator _mediator;

        public OrderController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("{userName}")]
        [ProducesResponseType(typeof(IEnumerable<OrdersVm>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<OrdersVm>>> Get(string userName)
        {
            GetOrderListQuery query = new(userName);

            var orders = await _mediator.Send(query);

            return orders.ToList();
        }
        
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<int>> Post(CheckoutOrderCommand command)
        {
            return await _mediator.Send(command);
        }
        
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Put(UpdateOrderCommand command)
        {
            await _mediator.Send(command);

            return NoContent();
        }

        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(DeleteOrderCommand command)
        {
            await _mediator.Send(command);

            return NoContent();
        }
    }
}
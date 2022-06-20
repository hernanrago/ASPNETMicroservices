using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Application.Contracts.Infrastructure;
using Ordering.Application.Models;
using Ordering.Application.Contracts.Persistence;
using Ordering.Domain.Entities;

namespace Ordering.Application.Features.Orders.Commands.CheckoutOrder
{
    public class CheckoutOrderCommandHandler : IRequestHandler<CheckoutOrderCommand, int>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;
        private readonly IEmailService _emailService;
        private readonly ILogger<CheckoutOrderCommandHandler> _logger;

        public CheckoutOrderCommandHandler(IOrderRepository orderRepository, IMapper mapper, IEmailService emailService, ILogger<CheckoutOrderCommandHandler> logger)
        {
            _orderRepository = orderRepository;
            _mapper = mapper;
            _emailService = emailService;
            _logger = logger;
        }

        public async Task<int> Handle(CheckoutOrderCommand request, CancellationToken cancellationToken)
        {
            Order order = _mapper.Map<Order>(request);

            Order createdOrder = await _orderRepository.AddAsync(order);

            _logger.LogInformation("Order {0} is successfully created.", createdOrder.Id);

            await SendMail(order);

            return createdOrder.Id;
        }

        private async Task SendMail(Order order)
        {
            Email email = new ()
            {
                To = "hernanrago@msn.com",
                Body = "Order was created.",
                Subject = "Order was created."
            };

            try
            {
                await _emailService.SendEmail(email);   
            }
            catch (Exception ex)
            {
                 _logger.LogError("Order {0} failed due to an error: {1}", order.Id, ex.Message);
            }
        }
    }
}
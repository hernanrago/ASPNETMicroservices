using AutoMapper;
using Discount.gRPC.Entities;
using Discount.gRPC.Protos;
using Discount.gRPC.Repositories;
using Grpc.Core;
using static Discount.gRPC.Protos.DiscountProtoService;

namespace  Discount.gRPC.Services
{
    public class DiscountService : DiscountProtoServiceBase
    {
        private readonly IDiscountRepository _repository;
        private readonly ILogger<DiscountService> _logger;
        private readonly IMapper _mapper;

        public DiscountService(IDiscountRepository repository, ILogger<DiscountService> logger, IMapper mapper)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
        }
        
        public override async Task<CouponModel> Get(GetDiscountRequest request, ServerCallContext context)
        {
            var coupon = await _repository.Get(request.ProductName);
            
            if (coupon == null)
            {
                throw new RpcException(new Status(StatusCode.NotFound, $"Discount with product name = {request.ProductName} not found."));
            }

            return _mapper.Map<CouponModel>(coupon);
        }

        public override async Task<CouponModel> Create(CreateDiscountRequest request, ServerCallContext context)
        {
            var coupon = _mapper.Map<Coupon>(request.Coupon);

            var created = await _repository.Create(coupon);

            if (!created)
            {
                throw new RpcException(new Status(StatusCode.Internal, $"Discount with product name = {request.Coupon.ProductName} could not be created."));
            }

            return _mapper.Map<CouponModel>(coupon);
        }

        public override async Task<DeleteDiscountResponse> Delete(DeleteDiscountRequest request, ServerCallContext context)
        {
            bool deleted = await _repository.Delete(request.ProductName);
                      
            return new DeleteDiscountResponse() { Success = deleted  };
        }

        // public override bool Equals(object? obj)
        // {
        //     return base.Equals(obj);
        // }



        // public override int GetHashCode()
        // {
        //     return base.GetHashCode();
        // }

        // public override string? ToString()
        // {
        //     return base.ToString();
        // }

        // public override Task<CouponModel> Update(UpdateDiscountRequest request, ServerCallContext context)
        // {
        //     return base.Update(request, context);
        // }
    }
}
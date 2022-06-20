using Discount.gRPC.Protos;
using static Discount.gRPC.Protos.DiscountProtoService;

namespace Basket.API.GrpcServices
{
    public class DiscountGrpcService
    {
        private readonly DiscountProtoServiceClient _discountProtoService;

        public DiscountGrpcService(DiscountProtoServiceClient discountProtoService)
        {
            _discountProtoService = discountProtoService;
        }

        public async Task<CouponModel> Get(string productName)
        {
            var request = new GetDiscountRequest { ProductName = productName };

            return await _discountProtoService.GetAsync(request);
        }
    }
}
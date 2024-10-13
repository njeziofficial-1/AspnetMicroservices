using AutoMapper;
using Discount.Api.Repositories;
using Discount.Grpc.Entities;
using Discount.Grpc.Protos;
using Grpc.Core;

namespace Discount.Grpc.Services;

public class DiscountService : DiscountProtoService.DiscountProtoServiceBase
{
    readonly IDiscountRepository _repository;
    readonly ILogger<DiscountService> _logger;
    readonly IMapper _mapper;

    public DiscountService(IDiscountRepository repository, ILogger<DiscountService> logger, IMapper mapper)
    {
        _repository = repository;
        _logger = logger;
        _mapper = mapper;
    }

    public override async Task<CouponModel> GetDiscount(GetDiscountRequst request, ServerCallContext context)
    {
        var coupon = await _repository.GetDiscount(request.ProductName) ?? throw new RpcException(new Status(StatusCode.NotFound, $"Discount with ProductName = {request.ProductName} does not exist"));
        return _mapper.Map<CouponModel>(coupon);
    }

    public override async Task<CouponModel> CreateDiscount(CreateDiscountRequst request, ServerCallContext context)
    {
        var coupon = _mapper.Map<Coupon>(request.Coupon);
        await _repository.CreateDiscount(coupon);
        _logger.LogInformation($"Discount is successfully created. ProductName : {coupon.ProductName}");
        return _mapper.Map<CouponModel>(coupon);
    }

    public override async Task<CouponModel> UpdateDiscount(UpdateDiscountRequst request, ServerCallContext context)
    {
        var coupon = _mapper.Map<Coupon>(request.Coupon);
        await _repository.UpdateDiscount(coupon);
        _logger.LogInformation($"Discount is successfully updated. ProductName : {coupon.ProductName}");
        return _mapper.Map<CouponModel>(coupon);
    }

    public override async Task<DeleteDiscountResponse> DeleteDiscount(DeleteDiscountRequst request, ServerCallContext context)
    {
        var deleted = await _repository.DeleteDiscount(request.ProductName);
        return new DeleteDiscountResponse
        {
            Success = deleted
        };
    }
}

﻿using Discount.Grpc.Protos;

namespace Basket.Api.GrpcServices;

public class DiscountGrpcService
{
    readonly DiscountProtoService.DiscountProtoServiceClient _discountProtoService;

    public DiscountGrpcService(DiscountProtoService.DiscountProtoServiceClient discountProtoService)
    {
        _discountProtoService = discountProtoService;
    }

    public async Task<CouponModel> GetDiscount(string? productName)
    {
        var discountRequest = new GetDiscountRequst { ProductName = productName };
        return await _discountProtoService.GetDiscountAsync(discountRequest);
    }
}

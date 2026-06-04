using System;
using Discount.Grpc.Data;
using Grpc.Core;
using Microsoft.EntityFrameworkCore;
using Discount.Grpc.Models;
using Mapster;

namespace Discount.Grpc.Services;

public class DiscountService(DiscountDbContext dbContext, ILogger<DiscountService> logger) : DiscountProtoService.DiscountProtoServiceBase
{
    public override async Task<CouponModel> CreateDiscount(CreateDiscountRequest request, ServerCallContext context)
    {
        var coupon = request.Coupon.Adapt<Coupon>();
        if (coupon is null)
            throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid request"));

        await dbContext.Coupons.AddAsync(coupon);
        await dbContext.SaveChangesAsync();
        logger.LogInformation("Discount is successfully created. ProductName: {ProductName}, Amount: {Amount}, Description: {Description}", coupon.ProductName, coupon.Amount, coupon.Description);
        return coupon.Adapt<CouponModel>();
    }

    public override async Task<CouponModel> GetDiscount(GetDiscountRequest request, ServerCallContext context)
    {
        var coupon = await dbContext.Coupons.FirstOrDefaultAsync(c => c.ProductName == request.ProductName);
        coupon ??= new Coupon { ProductName = "No coupon", Description = "No coupon description", Amount = 0 };
        logger.LogInformation("Discount is retrieved for ProductName: {ProductName}, Amount: {Amount}, Description: {Description}", coupon.ProductName, coupon.Amount, coupon.Description);
        
        return coupon.Adapt<CouponModel>();
    }

    public override async Task<CouponModel> UpdateDiscount(UpdateDiscountRequest request, ServerCallContext context)
    {
        var coupon = request.Coupon.Adapt<Coupon>();
        if (coupon is null)
            throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid request"));

        dbContext.Coupons.Update(coupon);
        await dbContext.SaveChangesAsync();
        logger.LogInformation("Discount is successfully updated. ProductName: {ProductName}, Amount: {Amount}, Description: {Description}", coupon.ProductName, coupon.Amount, coupon.Description);
        
        return coupon.Adapt<CouponModel>();
    }

    public override async Task<DeleteDiscountResponse> DeleteDiscount(DeleteDiscountRequest request, ServerCallContext context)
    {
        var coupon = await dbContext.Coupons.FirstOrDefaultAsync(c => c.ProductName == request.ProductName);

        if (coupon is null)
            return new DeleteDiscountResponse { Success = false }; 

        dbContext.Coupons.Remove(coupon);
        await dbContext.SaveChangesAsync();
        logger.LogInformation("Discount is successfully deleted. ProductName: {ProductName}, Amount: {Amount}, Description: {Description}", coupon.ProductName, coupon.Amount, coupon.Description);

        return new DeleteDiscountResponse { Success = true };
    }
}

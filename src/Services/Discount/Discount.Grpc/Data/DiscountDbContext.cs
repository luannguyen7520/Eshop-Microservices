using System;
using Discount.Grpc.Models;
using Microsoft.EntityFrameworkCore;

namespace Discount.Grpc.Data;

public class DiscountDbContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<Coupon> Coupons { get; set; }
}

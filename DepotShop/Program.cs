using DepotShopDataAccess.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAuthentication("Bearer")
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new()
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = builder.Configuration.GetValue<string>("Authentication:Issuer"),
                        ValidAudience = builder.Configuration.GetValue<string>("Authentication:Audiance"),
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(
                            builder.Configuration.GetValue<string>("Authentication:SecretKey")!
                            ))
                    };
                });
builder.Services.AddAuthorization(builder =>
{
    builder.AddPolicy("Admin", Policy => Policy.RequireClaim("Role"));
    builder.AddPolicy("HasID", Policy => Policy.RequireClaim(JwtRegisteredClaimNames.Sub));
    builder.FallbackPolicy = new AuthorizationPolicyBuilder()
                            .RequireAuthenticatedUser()
                            .Build();
});
builder.Services.AddSingleton<IMongoClient>(new MongoClient(
    builder.Configuration.GetConnectionString("Default")
    ));
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();

builder.Services.AddAutoMapper(typeof(Program));
var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.useAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

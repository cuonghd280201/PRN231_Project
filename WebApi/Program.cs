using Bussiness;
using Bussiness.Config;
using Bussiness.Utils;
using DataAccess;
using DataAccess.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

// For Entity Framework  
builder.Services.AddDbContext<FUFlowerBouquetManagementContext>();

// For Identity
builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
{
    options.Password.RequiredLength = 6;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
}).AddEntityFrameworkStores<FUFlowerBouquetManagementContext>()
       .AddDefaultTokenProviders();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
            .AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidAudience = "user",
                    ValidIssuer = "nam@gmail.com",
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("JWTAuthenticationHIGHsecuredPasswordVVVp1OH7Xzyr"))
                };
            });

builder.Services.AddSingleton<TokenUtils>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddAutoMapper(typeof(MappingProfile));

builder.Services.AddScoped<CategoryService>();
builder.Services.AddTransient<IGenericRep<Category>, GenericRep<FUFlowerBouquetManagementContext, Category>>();

builder.Services.AddSingleton<FlowerService>();
builder.Services.AddTransient<IGenericRep<FlowerBouquet>, GenericRep<FUFlowerBouquetManagementContext, FlowerBouquet>>();

builder.Services.AddSingleton<OrderService>();
builder.Services.AddTransient<IGenericRep<Order>, GenericRep<FUFlowerBouquetManagementContext, Order>>();

builder.Services.AddSingleton<OrderDetailService>();
builder.Services.AddTransient<IGenericRep<OrderDetail>, GenericRep<FUFlowerBouquetManagementContext, OrderDetail>>();

builder.Services.AddScoped<SupplierService>();
builder.Services.AddTransient<IGenericRep<Supplier>, GenericRep<FUFlowerBouquetManagementContext, Supplier>>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c => {
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

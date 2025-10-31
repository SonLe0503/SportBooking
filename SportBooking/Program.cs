using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using SportBooking.Mappers;
using SportBooking.Models;

var builder = WebApplication.CreateBuilder(args);

// -------------------- Services --------------------
builder.Services.AddDbContext<SportBookingDbContext>(opt =>
    opt.UseSqlServer(builder.Configuration.GetConnectionString("MyCnn")));

builder.Services.AddAutoMapper(typeof(RegisterProfile).Assembly);
builder.Services.AddControllers();
builder.Services.AddSwaggerGen();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(
                System.Text.Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };
    });

// Lưu DataProtection key để bảo toàn cookie/token
builder.Services.AddDataProtection()
    .PersistKeysToFileSystem(new DirectoryInfo(@"d:\DZHosts\LocalUser\hungds16\www.sportspacevn.somee.com\keys"))
    .SetApplicationName("SportBookingApp");

// -------------------- CORS --------------------
var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
        policy =>
        {
            policy.WithOrigins(
                "https://sport-booking-font-end.vercel.app", // Frontend trên Vercel
                "http://localhost:3000"                      // Dev local
            )
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
        });
});

var app = builder.Build();

// -------------------- Middleware --------------------

// ✅ Không redirect HTTPS thủ công khi dùng Cloudflare Flexible SSL
// Nhưng vẫn xác định đúng scheme khi Cloudflare gửi request qua HTTP
app.Use((context, next) =>
{
    var forwardedProto = context.Request.Headers["X-Forwarded-Proto"].ToString();
    if (forwardedProto.Equals("https", StringComparison.OrdinalIgnoreCase))
    {
        context.Request.Scheme = "https";
    }
    return next();
});

app.UseCors(MyAllowSpecificOrigins);
app.UseStaticFiles();

app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

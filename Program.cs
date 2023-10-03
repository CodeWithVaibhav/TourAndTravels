using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.IdentityModel.Tokens;
using TourAndTravels.Infrastucture;
using TourAndTravels.Infrastucture.Services;
using Serilog;
using TourAndTravels.Data;
using Microsoft.EntityFrameworkCore;
using TourAndTravels.Data.Repositories;

var builder = WebApplication.CreateBuilder(args);

JwtConfig.Configuration = builder.Configuration;
builder.Host.UseSerilog((context, configuration) => configuration.ReadFrom.Configuration(context.Configuration));

builder.Services.AddDbContext<TourAndTravelsDbContext>(x => x.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnectionString")));

builder.Services.AddTransient<IUserRepository, UserRepository>();

builder.Services.AddTransient(x => { return JwtConfig.GetJwtIssuerOptions(); });
builder.Services.AddTransient<IJwtFactory, JwtFactory>();
builder.Services.AddTransient<ICurrentUserService, CurrentUserService>();
builder.Services.AddTransient<IAuthService, AuthService>();
builder.Services.AddSingleton<IMemoryCache, MemoryCache>();

// Add services to the container.
builder.Services.AddHttpContextAccessor();
builder.Services.AddAntiforgery(o =>
{
    o.Cookie.Name = CookieNames.Antiforgery;
    o.HeaderName = "X-XSRF-Token";
});

builder.Services.AddControllers().AddNewtonsoftJson(x => x.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

builder.Services.AddSpaStaticFiles(configuration =>
{
    configuration.RootPath = "ClientApp/dist/admgmt";
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
        {
            options.RequireHttpsMetadata = false;
            options.SaveToken = false;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidAudience = builder.Configuration["JwtToken:Audience"],
                ValidIssuer = builder.Configuration["JwtToken:Issuer"],
                IssuerSigningKey =
                    JwtConfig.GetSigningKey(),
                ValidateLifetime = true,
                ClockSkew = TimeSpan.FromMinutes(5),
            };
            options.Events = new JwtBearerEvents
            {
                OnMessageReceived = context =>
                {
                    context.Token = context?.Request?.Cookies[CookieNames.Auth];
                    return Task.CompletedTask;
                },
                OnAuthenticationFailed = context =>
                {
                    if (context.Exception is SecurityTokenExpiredException)
                    {
                        var cookieOptions = new CookieOptions
                        {
                            HttpOnly = false,
                            SameSite = SameSiteMode.Lax,
                            IsEssential = true,
                            Expires = DateTime.Now.AddDays(-1)
                        };

                        context.HttpContext.Response.Cookies.Delete(CookieNames.Auth, cookieOptions);
                        Console.WriteLine("---------> Token expired");
                    }
                    return Task.CompletedTask;
                }
            };
            options.AutomaticRefreshInterval = TimeSpan.FromMinutes(5);
        });


builder.Services.Configure<BrotliCompressionProviderOptions>(options =>
        options.Level = System.IO.Compression.CompressionLevel.Fastest);
builder.Services.AddResponseCompression(options =>
{
    options.Providers.Add<BrotliCompressionProvider>();
    options.EnableForHttps = true;
    options.MimeTypes = new[]
                        {
                                        // General
                                        "text/plain",
                                        // Static files
                                        "text/css",
                                        "application/javascript",
                                        // MVC
                                        "text/html",
                                        "application/xml",
                                        "text/xml",
                                        "application/json",
                                        "text/json",
                                    };
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseSerilogRequestLogging();

app.UseStaticFiles();
if (!app.Environment.IsDevelopment())
{
    app.UseSpaStaticFiles();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.All
});

app.UseSpa(spa =>
{
    // To learn more about options for serving an Angular SPA from ASP.NET Core,
    // see https://go.microsoft.com/fwlink/?linkid=864501

    spa.Options.SourcePath = "ClientApp";

    if (app.Environment.IsDevelopment())
    {
        spa.UseProxyToSpaDevelopmentServer("http://localhost:4300");
    }
});

app.Run();

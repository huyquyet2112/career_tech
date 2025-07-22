global using CareerTech.Model.AppSettings;
global using Microsoft.EntityFrameworkCore;
using CareerTech;
using CareerTech.Attributes;
using CareerTech.Extensions;
using CareerTech.Middlewares;
using CareerTech.Model.Context;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.Logging.Console;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Get all configuration content.
var configuration = builder.Configuration;

AppSettingProvider.Initialize(configuration);

builder.Services.AddHttpLogging(option =>
{ });

// Remove default logging in project.
builder.Logging.ClearProviders();

// configuration logging simple.
builder.Logging.AddSimpleConsole(options =>
{
    options.IncludeScopes = true;
    options.SingleLine = true;
    options.TimestampFormat = "HH:mm:ss ";
    options.ColorBehavior = LoggerColorBehavior.Enabled;
});

// get data configuration from appsetings.json.DefaultConnection.
var connectionString = configuration.GetConnectionString("DefaultConnection");

// register DatabaseContext in DI.
// use DatabaseContext for EFCore.
builder.Services.AddDbContext<DatabaseContext>(options => 
options.UseSqlServer(connectionString));

builder.Services.AddHttpContextAccessor();
builder.Services.AddHandler();
builder.Services.AddRepositories();
builder.Services.AddService();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(AppSettingProvider.AppSettings.Jwt.Secret)),
        ValidateIssuer = false,
        ValidateAudience = false,
    };

    options.Events = new JwtBearerEvents
    {
        OnAuthenticationFailed = context =>
        {
            var path = context.Request.Path.Value?.ToLower();

            if (path != null && path.StartsWith("/admin"))
                context.Response.Redirect("/login/admin");
            else if (path != null && path.StartsWith("/recruiters"))
                context.Response.Redirect("/login/recruiter");
            else
                context.Response.Redirect("/login");

            context.Response.StatusCode = 302;
            return Task.CompletedTask;
        },

        OnChallenge = context =>
        {
            var path = context.Request.Path.Value?.ToLower();

            if (path != null && path.StartsWith("/admin"))
                context.Response.Redirect("/login/admin");
            else if (path != null && path.StartsWith("/recruiters"))
                context.Response.Redirect("/login/recruiter");
            else
                context.Response.Redirect("/login");

            context.Response.StatusCode = 302;
            context.HandleResponse(); 
            return Task.CompletedTask;
        },

        OnForbidden = context =>
        {
            context.Response.Redirect("/403");
            return Task.CompletedTask;
        }
    };
});

builder.Services.AddAuthorization(options =>
{
    options.FallbackPolicy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .AddRequirements(new PermissionRequirement())
        .Build();
});

// Add services to the container.
builder.Services.AddControllersWithViews();

// Support Localization in project
// Files will find in folder Resources.
builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");

builder.Services.AddMvc()
        // Allow selecting Views based on language suffixes.
        .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
        .AddDataAnnotationsLocalization(options =>
        {
            options.DataAnnotationLocalizerProvider = (type, factory) =>
                factory.Create(typeof(Language));
        });

builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    var supportedCultures = AppSettingProvider.AppSettings.GetLanguageSupportCodes();
});

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = null;
        options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

var app = builder.Build();
app.UseHttpLogging();

// Configure the HTTP request pipeline.

//app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseRequestLocalization();
app.UseMiddleware<LanguageMiddleware>();
app.UseMiddleware<JwtCookiesMiddleware>();
app.UseMiddleware<FileValidationMiddleware>();
app.UseMiddleware<ErrorHandlingMiddleware>();

app.UseAuthentication();
app.UseAuthorization();


app.MapControllers();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}");

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.Run();


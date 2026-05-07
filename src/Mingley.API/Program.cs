using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using Serilog.Events;
using Mingley.Application;
using Mingley.Infrastructure;
using Mingley.API.Middleware;
using Mingley.API.Hubs;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.File("logs/mingley-.log", rollingInterval: RollingInterval.Day)
    .CreateLogger();

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog();

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSignalR();

builder.Services.AddCors(o => o.AddPolicy("AllowAll", p =>
    p.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()));

var jwtKey = builder.Configuration["Jwt:Secret"]!;
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
            ValidateIssuer = true, ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidateAudience = true, ValidAudience = builder.Configuration["Jwt:Audience"],
            ValidateLifetime = true, ClockSkew = TimeSpan.Zero
        };
        // Allow JWT in SignalR query string
        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                var token = context.Request.Query["access_token"];
                var path  = context.HttpContext.Request.Path;
                if (!string.IsNullOrEmpty(token) &&
                    (path.StartsWithSegments("/hubs/chat") || path.StartsWithSegments("/hubs/notifications")))
                    context.Token = token;
                return Task.CompletedTask;
            }
        };
    });

builder.Services.AddAuthorization();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Mingley Dating API", Version = "v1", Description = "Complete dating app backend" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization", Type = SecuritySchemeType.Http,
        Scheme = "Bearer", BearerFormat = "JWT", In = ParameterLocation.Header,
        Description = "Enter: Bearer {token}"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {{
        new OpenApiSecurityScheme { Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" } },
        Array.Empty<string>()
    }});
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Mingley API v1");
    c.RoutePrefix = "swagger";
});

app.UseCors("AllowAll");
app.UseSerilogRequestLogging();
app.UseMiddleware<ExceptionMiddleware>();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

// SignalR hubs
app.MapHub<ChatHub>("/hubs/chat");
app.MapHub<NotificationHub>("/hubs/notifications");

using (var scope = app.Services.CreateScope())
{
    try
    {
        var db = scope.ServiceProvider
            .GetRequiredService<Mingley.Infrastructure.Persistence.MingleyDbContext>();
        db.Database.EnsureCreated();
        Log.Information("Database ready.");
    }
    catch (Exception ex) { Log.Error(ex, "Database initialization failed."); }
}

Log.Information("Mingley API starting on http://localhost:7001");
app.Run();

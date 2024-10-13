using System.Net;
using System.Net.Mail;
using System.Text.Json.Serialization;
using DotNetEnv;
using FastEndpoints;
using FastEndpoints.Swagger;
using FastEndpoints.Security;
using Hangfire;
using Hangfire.PostgreSql;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Rendalicce.Configurations;
using Rendalicce.Features.App.Chats;
using Rendalicce.Infrastructure.Authentication;
using Rendalicce.Infrastructure.BackgroundJobs;
using Rendalicce.Infrastructure.Emails;
using Rendalicce.Infrastructure.Errors;
using Rendalicce.Infrastructure.Extensions;
using Rendalicce.Infrastructure.OpenApi;
using Rendalicce.Persistency;
using Serilog;
using Serilog.Events;

var builder = WebApplication.CreateBuilder();

Env.Load();
var connectionString = builder.Configuration.GetConnectionString("Database");
builder.Services.AddDbContext<DatabaseContext>(o => o.UseNpgsql(connectionString));

var jwtConfigurationSection = builder.Configuration.GetSection(nameof(JwtConfiguration));
var jwtConfiguration = jwtConfigurationSection.Get<JwtConfiguration>()!;
builder.Services.AddAuthenticationJwtBearer(s => s.SigningKey = jwtConfiguration.SigningKey);

var mailServerConfiguration =
    builder.Configuration.GetSection(nameof(MailServerConfiguration)).Get<MailServerConfiguration>()!;
builder.Services
    .AddFluentEmail(mailServerConfiguration.FromEmailAddress, mailServerConfiguration.FromDisplayName)
    .AddSmtpSender(
        new SmtpClient
        {
            Host = mailServerConfiguration.Host,
            Port = mailServerConfiguration.Port,
            Credentials = new NetworkCredential(mailServerConfiguration.Username, mailServerConfiguration.Password),
            EnableSsl = mailServerConfiguration.EnableSsl
        }
    )
    .AddRazorRenderer();

builder.Services.AddAuthorization();
builder.Services.AddFastEndpoints();

builder.Services.SwaggerDocument(
    o =>
    {
        o.EnableJWTBearerAuth = true;
        o.DocumentSettings = s =>
        {
            s.Title = "App";
            s.Version = "Latest";
            s.DocumentName = "App";
            s.Description = "Endpoints meant for implementing customer app UI.";
        };
        o.RemoveEmptyRequestSchema = true;
        o.ShortSchemaNames = true;
        o.AutoTagPathSegmentIndex = 1;
        o.EndpointFilter = ep => ep.Routes?.Any(epr => !epr.StartsWith("administration")) ?? true;
    }
);
builder.Services.SwaggerDocument(
    o =>
    {
        o.EnableJWTBearerAuth = true;
        o.DocumentSettings = s =>
        {
            s.Title = "Administration";
            s.Version = "Latest";
            s.DocumentName = "Administration";
            s.Description = "Endpoints meant for implementing administration UI.";
        };
        o.RemoveEmptyRequestSchema = true;
        o.ShortSchemaNames = true;
        o.AutoTagPathSegmentIndex = 2;
        o.EndpointFilter = ep => ep.Routes?.Any(epr => epr.StartsWith("administration")) ?? true;
    }
);

builder.Services.AddSingleton(jwtConfiguration);
builder.Services.AddSingleton<JwtProvider, JwtProvider>();
builder.AddSingletonConfiguration<ApplicationErrorNotificationConfiguration>();
builder.Services.AddTransient<EmailSendingService, EmailSendingService>();

builder.Services.AddSingleton<HangfireJobFailureNotificationFilter, HangfireJobFailureNotificationFilter>();
builder.Services.AddHangfire(
    configuration => configuration
        .UseSerializerSettings(new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All })
        .UsePostgreSqlStorage(o => o.UseNpgsqlConnection(connectionString))
        .WithJobExpirationTimeout(TimeSpan.FromDays(30))
);
builder.Services.AddHangfireServer();

var corsConfiguration = builder.Configuration.GetSection(nameof(CorsConfiguration)).Get<CorsConfiguration>()!;
builder.Services.AddCors(
    options =>
    {
        options.AddDefaultPolicy(
            pb =>
            {
                pb.WithOrigins(corsConfiguration.Origins);
                pb.AllowAnyHeader();
                pb.AllowAnyMethod();
                pb.AllowCredentials();
            }
        );
    }
);

builder.Services.AddSignalR();

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

// var columnOptions = new ColumnOptions
// {
//     AdditionalColumns = new List<SqlColumn>
//     {
//         new() { ColumnName = "LogEventType", DataLength = 127 },
//         new() { ColumnName = "RelatedEntityId", DataType = SqlDbType.NVarChar }
//     }
// };
// columnOptions.Store.Remove(StandardColumn.Properties);
// columnOptions.Store.Add(StandardColumn.LogEvent);
// columnOptions.Store.Add(StandardColumn.TraceId);
// columnOptions.LogEvent.ExcludeStandardColumns = true;
// columnOptions.TimeStamp.NonClusteredIndex = true;
//
// IDictionary<string, ColumnWriterBase> columnWriters = new Dictionary<string, ColumnWriterBase>
// {
//     {"message", new RenderedMessageColumnWriter(NpgsqlDbType.Text) },
//     {"message_template", new MessageTemplateColumnWriter(NpgsqlDbType.Text) },
//     {"level", new LevelColumnWriter(true, NpgsqlDbType.Varchar) },
//     {"raise_date", new TimestampColumnWriter(NpgsqlDbType.Timestamp) },
//     {"exception", new ExceptionColumnWriter(NpgsqlDbType.Text) },
//     {"properties", new LogEventSerializedColumnWriter(NpgsqlDbType.Jsonb) },
//     {"props_test", new PropertiesColumnWriter(NpgsqlDbType.Jsonb) },
//     {"machine_name", new SinglePropertyColumnWriter("MachineName", PropertyWriteMethod.ToString, NpgsqlDbType.Text, "l") }
// };

builder.Host.UseSerilog((context, configuration) =>
{
    configuration.ReadFrom.Configuration(builder.Configuration) // Reads the Serilog section from appsettings.json
        .Filter.ByIncludingOnly(logEvent =>
            logEvent.Properties.ContainsKey("SourceContext") &&
            logEvent.Properties["SourceContext"].ToString().Contains("Rendalicce"))
        .WriteTo.PostgreSQL(connectionString,
            "Logs",
            restrictedToMinimumLevel: LogEventLevel.Information,
            needAutoCreateTable: true
            // columnOptions:  new Dictionary<string, ColumnWriterBase>() TODO-FK
        )
        .Enrich.FromLogContext();
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    using var servicesScope = app.Services.CreateScope();
    using var dbContext = servicesScope.ServiceProvider.GetRequiredService<DatabaseContext>();
    // dbContext.Database.Migrate();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseCors();

app.UseExceptionHandler(o => { });
app.UseAuthentication();
app.UseAuthorization();
app.UseFastEndpoints(c =>
{
    c.Serializer.Options.Converters.Add(new JsonStringEnumConverter());
    c.Endpoints.RoutePrefix = "api";
});

app.UseSwaggerGen(
    c => { c.Path = "/{documentName}/specification.json"; },
    uic =>
    {
        uic.Path = "/swagger";
        uic.DocumentPath = "/{documentName}/specification.json";
        uic.DocumentTitle = "Rendalicce Docs";
    }
);

app.UseAuthenticatedUserSetter();

GlobalJobFilters.Filters.Add(app.Services.GetRequiredService<HangfireJobFailureNotificationFilter>());
app.UseHangfireDashboard(
    "/api/hangfire",
    new DashboardOptions
    {
        IgnoreAntiforgeryToken = true,
        DashboardTitle = "Rendalicce Tasks",
        AppPath = null,
        // Authorization = [new HangfireAuthorizationFilter()], TODO-FK: Uncomment after testing.
        Authorization = [],
        DarkModeEnabled = true,
        DefaultRecordsPerPage = 500
    }
);

app.MapHub<ChatHub>("/chats/hub");

app.Run();
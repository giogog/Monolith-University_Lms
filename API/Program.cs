using API.Extensions;
using Application.SignalR;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.ConfigureCors();
builder.Services.AddSignalR();
builder.Services.ConfigureSqlServer(builder.Configuration);
builder.Services.ConfigureMongoDb(builder.Configuration);
builder.Services.ConfigureAutomapper();
builder.Services.AddIdentityService(builder.Configuration);
builder.Services.ConfigureRepositoryManager();
builder.Services.AddScoped<HttpClient>();
builder.Services.ConfigureServiceManager();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.ConfigureMediatR();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseRouting();
app.UseCors("AllowSpecificOrigin");
app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapHub<ApplicantHub>("/applicantHub").RequireAuthorization();
    endpoints.MapHub<EnrollmentHub>("/enrollmentHub").RequireAuthorization();

});

app.MapControllers();

app.Run();

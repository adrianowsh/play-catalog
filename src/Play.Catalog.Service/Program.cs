using Play.Catalog.Service;
using Play.Common.MassTransitRabbitMq;
using Play.Common.MongoDb;

var builder = WebApplication.CreateBuilder(args);


builder.Services.ConfigureJwtBearerAuthentication(builder.Configuration);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMongodbSettings(builder.Configuration);
builder.Services.AddRepositories();
builder.Services.AddMassTransitWithRabbitMq(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.ConfigureCors(builder.Configuration);

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

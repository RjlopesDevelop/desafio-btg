using ConsumerBTGService.Application.Interfaces;
using ConsumerBTGService.Application.Services;
using ConsumerBTGService.Infrastructure;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Adicione serviços ao contêiner.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<BtgDbContext>(options =>
    options.UseMySql(connectionString,
    new MySqlServerVersion(new Version(8, 0, 21))));
    
    

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();

// builder.Services.AddSingleton<IRabbitMqService, RabbitMqService>();
// builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IOrderService, OrderService>();
//builder.Services.AddScoped<IRabbitMqService, RabbitMqService>();

// Registrar IServiceScopeFactory para criar escopos no RabbitMqService
builder.Services.AddSingleton<IServiceScopeFactory>(sp => sp.GetRequiredService<IServiceScopeFactory>());

// Registrar RabbitMqService como Singleton
builder.Services.AddSingleton<RabbitMqService>();

// Garantir que IRabbitMqService e IHostedService apontam para RabbitMqService
builder.Services.AddSingleton<IRabbitMqService>(sp => sp.GetRequiredService<RabbitMqService>());
builder.Services.AddSingleton<IHostedService>(sp => sp.GetRequiredService<RabbitMqService>());

// Adiciona o Hosted Service
builder.Services.AddHostedService<RabbitMqHostedService>();





var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();


// using (var scope = app.Services.CreateScope())
// {
//     var rabbitMqService = scope.ServiceProvider.GetRequiredService<IRabbitMqService>();
//     rabbitMqService.ConsumeMessage();
// }
// // Inicie o consumo das mensagens do RabbitMQ ao iniciar a aplicação
// var rabbitMqService = app.Services.GetRequiredService<IRabbitMqService>();
// rabbitMqService.ConsumeMessage();
// Inicie o consumo das mensagens do RabbitMQ ao iniciar a aplicação
// using (var scope = app.Services.CreateScope())
// {
//     var rabbitMqService = scope.ServiceProvider.GetRequiredService<IRabbitMqService>();
//     rabbitMqService.ConsumeMessage();
// }
// var rabbitMqService = app.Services.GetRequiredService<IRabbitMqService>();
// rabbitMqService.ConsumeMessage();

app.MapControllers();
app.Run();



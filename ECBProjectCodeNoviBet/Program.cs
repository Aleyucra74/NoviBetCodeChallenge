using ECBGatewayLibrary;
using ECBProjectCodeNoviBet.Data;
using ECBProjectCodeNoviBet.Repository;
using ECBProjectCodeNoviBet.Repository.Interface;
using ECBProjectCodeNoviBet.Strategy;
using ECBProjectCodeNoviBet.Strategy.Interface;
using Microsoft.EntityFrameworkCore;
using Quartz;
using Quartz.Impl;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHttpClient();
builder.Services.AddScoped<IWalletRepository,WalletRepository>();
builder.Services.AddSingleton<IAdjustBalanceStrategy, AddFundsStrategy>();
builder.Services.AddSingleton<IAdjustBalanceStrategy, SubtractFundsStrategy>();
builder.Services.AddSingleton<IAdjustBalanceStrategy, ForceSubtractFundsStrategy>();
//builder.Services.AddScoped<IAdjustBalanceStrategy, BaseStrategy>();

builder.Services.AddSingleton<BaseStrategy>();
builder.Services.AddSingleton<ECBClientHttp>();
builder.Services.AddSingleton<ECBParserXML>();


builder.Services.AddDbContext<DatabaseContext>(options => 
    options.UseInMemoryDatabase(builder.Configuration.GetConnectionString("NoviBetCodeChallenge")));

//quartz job
builder.Services.AddQuartz(options =>
{
    options.UseMicrosoftDependencyInjectionJobFactory();

});
builder.Services.AddQuartzHostedService(options => options.WaitForJobsToComplete = true);

builder.Services.AddQuartz(options => 
{
    options.ScheduleJob<ECBUpdateRatesJob>(trigger =>
        trigger.WithIdentity("ECBUpdateRatesJob")
        .StartNow()
        .WithSimpleSchedule(x => x.WithIntervalInMinutes(1).RepeatForever())

    );
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

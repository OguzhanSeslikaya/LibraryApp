using Loan.Outbox.Publisher.Service.Jobs;
using MassTransit;
using Quartz;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddMassTransit(configurator =>
{
    configurator.UsingRabbitMq((context, _configure) =>
    {
        _configure.Host("amqp://guest:guest@localhost:5672");
    });
});

builder.Services.AddQuartz(configurator =>
{
    JobKey jobKey = new("LoanOutboxPublishJob");
    configurator.AddJob<LoanOutboxPublishJob>(options => options.WithIdentity(jobKey));

    TriggerKey triggerKey = new("LoanOutboxPublishTrigger");
    configurator.AddTrigger(options => options.ForJob(jobKey)
    .WithIdentity(triggerKey)
    .StartAt(DateTime.UtcNow)
    .WithSimpleSchedule(builder => builder
        .WithIntervalInSeconds(5)
        .RepeatForever()));
});

builder.Services.AddQuartzHostedService(options => options.WaitForJobsToComplete = true);

var host = builder.Build();
host.Run();
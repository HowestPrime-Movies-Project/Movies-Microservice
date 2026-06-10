using Howestprime.Movies.Infrastructure.Messaging.IntegrationEvents;
using Howestprime.Movies.Infrastructure.Messaging.IntegrationEvents.Messages;
using Howestprime.Movies.Infrastructure.Messaging.IntegrationEvents.Shared;
using Howestprime.Movies.Infrastructure.Messaging.IntegrationEvents.Shared.Contracts;
using Howestprime.Movies.Infrastructure.Messaging.IntegrationEvents.Shared.Extensions;

namespace Howestprime.Movies.Main.Modules.Messaging.IntegrationEvents;

public static class MessagingModule
{
    public static IServiceCollection AddMessagingModule(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        return
            services
                .AddAmqpServices(configuration)
                .AddKeyedScoped<IController<ConsumerContext>, WhenPaymentSuccessCloseBookingController>(OperationIds.WhenPaymentSuccessCloseBooking)
                .AddKeyedScoped<IController<ConsumerContext>, WhenPaymentFailedCloseBookingController>(OperationIds.WhenPaymentFailedCloseBooking)
                .AddHostedService<MessagingBackgroundWorker>();
    }
}

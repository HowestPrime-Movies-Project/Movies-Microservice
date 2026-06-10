using System.Text.Json;
using System.Text.Json.Serialization;
using Howestprime.Movies.Application.Contracts.Ports;
using Howestprime.Movies.Application.Movies;
using Howestprime.Movies.Domain.Movies.ValueObjects;
using Howestprime.Movies.Infrastructure.Messaging.IntegrationEvents.Shared.Contracts;

namespace Howestprime.Movies.Infrastructure.Messaging.IntegrationEvents.Messages;

public class WhenPaymentSuccessCloseBookingController(
    IUseCase<CloseBookingInput> closeBooking
) : IController<ConsumerContext>
{
    public async Task Handle(ConsumerContext context)
    {
        var payload = JsonSerializer.Deserialize<PaymentEventPayload>(context.Message)
            ?? throw new ArgumentException("Invalid payment event payload.");

        await closeBooking.Execute(new CloseBookingInput(payload.BookingId, CloseBookingReason.PaymentSuccess));
    }
}

internal sealed record PaymentEventPayload(
    [property: JsonPropertyName("bookingId")] string BookingId
);

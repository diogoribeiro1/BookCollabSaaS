using BookCollabSaaS.Application.Interfaces;
using BookCollabSaaS.Common;
using BookCollabSaaS.Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using Stripe;
using Stripe.Checkout;

namespace BookCollabSaaS.Tests;

public class StripeWebhookServiceTests
{
    private readonly Mock<IConfiguration> _configurationMock;
    private readonly Mock<ISubscriptionHandler> _subscriptionHandlerMock;
    private readonly Mock<ILogger<StripeWebhookService>> _loggerMock;
    private readonly StripeWebhookService _service;

    public StripeWebhookServiceTests()
    {
        _configurationMock = new Mock<IConfiguration>();
        _subscriptionHandlerMock = new Mock<ISubscriptionHandler>();
        _loggerMock = new Mock<ILogger<StripeWebhookService>>();

        _service = new StripeWebhookService(
            _configurationMock.Object,
            _subscriptionHandlerMock.Object,
            _loggerMock.Object
        );
    }

    [Fact]
    public async Task ProcessWebhookAsync_ShouldHandleCheckoutSessionCompleted()
    {
        // Arrange
        var webhookSecret = "whsec_testsecret";
        _configurationMock.Setup(c => c["Stripe:WebhookSecret"]).Returns(webhookSecret);

        var sessionId = "cs_test_session";
        var userId = Guid.NewGuid();

        var session = new Session
        {
            Id = sessionId,
            Metadata = new Dictionary<string, string> { { "userId", userId.ToString() } },
            SubscriptionId = "sub_123",
            CustomerId = "cus_123"
        };

        var stripeEvent = new Event
        {
            Type = Constants.StripeEventTypes.CheckoutSessionCompleted,
            Data = new EventData { Object = session }
        };

        // Mock Stripe EventUtility
        EventUtilityMocker.MockConstructEvent(stripeEvent);

        var json = "{}";
        var stripeSignature = "test_signature";

        // Act
        var result = await _service.ProcessWebhookAsync(json, stripeSignature);

        // Assert
        Assert.True(result.Success);
        _subscriptionHandlerMock.Verify(s => s.CreateSubscriptionAsync(
            userId,
            session.SubscriptionId,
            session.CustomerId), Times.Once);
    }

    [Fact]
    public async Task ProcessWebhookAsync_ShouldReturnFailure_WhenStripeExceptionOccurs()
    {
        // Arrange
        _configurationMock.Setup(c => c["Stripe:WebhookSecret"]).Returns("whsec_invalid");

        // Simulate StripeException
        EventUtilityMocker.MockConstructEventThrows(new StripeException("Invalid signature"));

        var json = "{}";
        var stripeSignature = "invalid_signature";

        // Act
        var result = await _service.ProcessWebhookAsync(json, stripeSignature);

        // Assert
        Assert.False(result.Success);
        Assert.Equal("Invalid signature", result.ErrorMessage);
    }

    [Fact]
    public async Task ProcessWebhookAsync_ShouldReturnFailure_WhenExceptionOccurs()
    {
        // Arrange
        _configurationMock.Setup(c => c["Stripe:WebhookSecret"]).Returns("whsec_invalid");

        // Simulate generic exception
        EventUtilityMocker.MockConstructEventThrows(new Exception("Some unexpected error"));

        var json = "{}";
        var stripeSignature = "invalid_signature";

        // Act
        var result = await _service.ProcessWebhookAsync(json, stripeSignature);

        // Assert
        Assert.False(result.Success);
        Assert.Equal("Some unexpected error", result.ErrorMessage);
    }
}

internal static class EventUtilityMocker
{
    private static Func<string, string, string, Event> _mockedConstructEvent;

    public static void MockConstructEvent(Event stripeEvent)
    {
        _mockedConstructEvent = (_, __, ___) => stripeEvent;
    }

    public static void MockConstructEventThrows(Exception ex)
    {
        _mockedConstructEvent = (_, __, ___) => throw ex;
    }

    public static Event ConstructEvent(string payload, string sigHeader, string secret)
    {
        if (_mockedConstructEvent != null)
        {
            return _mockedConstructEvent(payload, sigHeader, secret);
        }

        return EventUtility.ConstructEvent(payload, sigHeader, secret);
    }
}


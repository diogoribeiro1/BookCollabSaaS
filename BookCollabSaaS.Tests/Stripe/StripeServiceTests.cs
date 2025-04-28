using BookCollabSaaS.Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Moq;
using Stripe.Checkout;
namespace BookCollabSaaS.Tests;

public class StripeServiceTests
{
    [Fact]
    public async Task CreateCheckoutSession_ShouldReturnSession()
    {
        // Arrange
        var mockConfiguration = new Mock<IConfiguration>();
        mockConfiguration.Setup(c => c["Stripe:SecretKey"]).Returns("sk_test_123");

        var mockSessionService = new Mock<SessionService>();
        var expectedSession = new Session { Id = "sess_123" };

        mockSessionService
            .Setup(s => s.CreateAsync(It.IsAny<SessionCreateOptions>(), null, default))
            .ReturnsAsync(expectedSession);

        var stripeService = new StripeService(mockConfiguration.Object, mockSessionService.Object);

        var priceId = "price_123";
        var successUrl = "https://success.com";
        var cancelUrl = "https://cancel.com";
        var userId = Guid.NewGuid();

        // Act
        var result = await stripeService.CreateCheckoutSession(priceId, successUrl, cancelUrl, userId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("sess_123", result.Id);
        mockSessionService.Verify(s => s.CreateAsync(It.IsAny<SessionCreateOptions>(), null, default), Times.Once);
    }
}

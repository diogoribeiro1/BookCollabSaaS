using System;

namespace BookCollabSaaS.Common;

public class WebhookProcessingResult
{
    public bool Success { get; private set; }
    public string ErrorMessage { get; private set; }

    private WebhookProcessingResult(bool success, string errorMessage = null)
    {
        Success = success;
        ErrorMessage = errorMessage;
    }

    public static WebhookProcessingResult SuccessResult() => new WebhookProcessingResult(true);

    public static WebhookProcessingResult FailureResult(string errorMessage) => new WebhookProcessingResult(false, errorMessage);
}
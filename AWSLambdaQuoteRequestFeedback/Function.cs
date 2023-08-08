using Amazon;
using Amazon.Lambda.Core;
using Amazon.Lambda.SQSEvents;
using MimeKit;
using System.Text.Json;
using MailKit.Net.Smtp;
using Amazon.SecretsManager.Model;
using Amazon.SecretsManager;
using Microsoft.Data.SqlClient;



// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace AWSLambdaQuoteRequestFeedback;

public class Function
{

    /// <summary>
    /// Default constructor. This constructor is used by Lambda to construct the instance. When invoked in a Lambda environment
    /// the AWS credentials will come from the IAM role associated with the function and the AWS region will be set to the
    /// region the Lambda function is executed in.
    /// </summary>
    public Function()
    {
    }


    /// <summary>
    /// This method is called for every Lambda invocation. This method takes in an SQS event object and can be used 
    /// to respond to SQS messages.
    /// </summary>
    /// <param name="evnt"></param>
    /// <param name="context"></param>
    /// <returns></returns>
    public async Task FunctionHandler(SQSEvent evnt, ILambdaContext context)
    {
        foreach (var message in evnt.Records)
        {
            await ProcessMessageAsync(message, context);
        }
    }

    private async Task ProcessMessageAsync(SQSEvent.SQSMessage message, ILambdaContext context)
    {
        context.Logger.LogInformation($"Processed message {message.Body}");
        // Deserialize the entire SNS message
        var snsMessage = JsonSerializer.Deserialize<SNSMessage>(message.Body);

        // Deserialize the "Message" field to get the actual message content
        var request = JsonSerializer.Deserialize<QuoteRequestMessage>(snsMessage.Message);

        if (request == null || request.QuoteRequestId == 0)
        {
            return;
        }

        var quoteRequestId = request.QuoteRequestId;

        var emailMessage = new MimeMessage();
        emailMessage.From.Add(new MailboxAddress("Traditional Handicraft Gallery", "no-reply@traditionalhandicraft.gallery"));
        emailMessage.To.Add(new MailboxAddress(request.UserName, request.UserEmail));
        emailMessage.Subject = $"Quote Request Received for: {request.HandicraftName}";

        // Create HTML body
        var htmlBody = new TextPart("html")
        {
            Text = $@"<h1>Quote Request Received</h1>
                  <p><strong>Handicraft Name:</strong> {request.HandicraftName}</p>
                  <p><strong>Requested At:</strong> {request.RequestedAt.ToString("g")}</p>
                  <p>We have received your quote request and will forward it to the author. You will hear back shortly.</p>
                  <p>Thank you for your interest in our Traditional Handicraft Gallery!</p>"
        };

        emailMessage.Body = htmlBody;

        using (var client = new SmtpClient())
        {
            string SMTPSecret = await GetSecret(context, "DDAC_SMTP");
            var smtpConfig = JsonSerializer.Deserialize<SMTPConfiguration>(SMTPSecret);

            if (smtpConfig != null)
            {
                string smtpUsername = smtpConfig.SMTPUsername;
                string smtpPassword = smtpConfig.SMTPPassword;
                await client.ConnectAsync("smtp.mailtrap.io", 587, false);
                await client.AuthenticateAsync(smtpUsername, smtpPassword);
                await client.SendAsync(emailMessage);
                await client.DisconnectAsync(true);
            }
        }

        // TODO: Do interesting work based on the new message
        await Task.CompletedTask;
    }

    static async Task<string> GetSecret(ILambdaContext context, string secretName)
    {
        string region = "us-east-1";
        string secret = "";

        IAmazonSecretsManager client = new AmazonSecretsManagerClient(RegionEndpoint.GetBySystemName(region));

        GetSecretValueRequest request = new GetSecretValueRequest
        {
            SecretId = secretName,
            //VersionStage = "AWSCURRENT", // VersionStage defaults to AWSCURRENT if unspecified.
        };

        GetSecretValueResponse response;

        try
        {
            response = await client.GetSecretValueAsync(request);
            if (response.SecretString != null)
            {
                secret = response.SecretString;
            }
        }
        catch (Exception e)
        {
            context.Logger.LogInformation($"Error in SQS lambda function for quote request: {e.Message}");
        }

        return secret;
    }

    public class QuoteRequestMessage
    {
        public int QuoteRequestId { get; set; } = 0;
        public string UserName { get; set; } = "";
        public string UserEmail { get; set; } = "";
        public string HandicraftName { get; set; } = "";
        public int HandicraftId { get; set; }
        public string HandicraftAuthorName { get; set; } = "";
        public string HandicraftAuthorEmail { get; set; } = "";

        public DateTime RequestedAt { get; set; }
    }

    public class SMTPConfiguration
    {
        public string SMTPUsername { get; set; }
        public string SMTPPassword { get; set; }
    }

    public class SNSMessage
    {
        public string Type { get; set; }
        public string MessageId { get; set; }
        public string TopicArn { get; set; }
        public string Message { get; set; }
        // other fields as needed
    }

}
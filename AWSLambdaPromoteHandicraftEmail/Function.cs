using Amazon.Lambda.Core;
using Amazon.Lambda.SQSEvents;
using MimeKit;
using System.Text.Json;
using MailKit.Net.Smtp;
using Amazon.SecretsManager.Model;
using Amazon.SecretsManager;
using Amazon;
using Microsoft.Data.SqlClient;



// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace AWSLambdaPromoteHandicraftEmail;

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
        var promotion = JsonSerializer.Deserialize<PromotionMessage>(snsMessage.Message);

        if (promotion == null || promotion.HandicraftId == 0)
        {
            return;
        }

        var userEmails = await GetEmailsOfUsersWithRoleAsync(context, "User");

        string SMTPSecret = await GetSecret(context, "DDAC_SMTP");
        var smtpConfig = JsonSerializer.Deserialize<SMTPConfiguration>(SMTPSecret);

        using (var client = new SmtpClient())
        {
            if (smtpConfig != null)
            {
                string smtpUsername = smtpConfig.SMTPUsername;
                string smtpPassword = smtpConfig.SMTPPassword;
                await client.ConnectAsync("smtp.mailtrap.io", 587, false);
                await client.AuthenticateAsync(smtpUsername, smtpPassword);

                foreach (string email in userEmails)
                {
                    var emailMessage = new MimeMessage();
                    emailMessage.From.Add(new MailboxAddress("Traditional Handicraft Gallery", "no-reply@traditionalhandicraft.gallery"));
                    emailMessage.To.Add(new MailboxAddress("", email));
                    emailMessage.Subject = $"Introducing {promotion.HandicraftName} - A New Handicraft in our Gallery";

                    // Create HTML body
                    var htmlBody = new TextPart("html")
                    {
                        Text = $@"<h1>{promotion.HandicraftName} - Now Available!</h1>
                      <p><strong>Author:</strong> {promotion.HandicraftAuthorName} ({promotion.HandicraftAuthorEmail})</p>
                      <p><strong>Type:</strong> {promotion.HandicraftTypeName}</p>
                      <p><strong>Description:</strong> {promotion.HandicraftDescription}</p>
                      <p>Explore this beautiful new addition to our Traditional Handicraft Gallery. We think you'll love it!</p>"
                    };

                    emailMessage.Body = htmlBody;
                    await client.SendAsync(emailMessage);
                }

                await client.DisconnectAsync(true);
            }
        }

        // TODO: Do interesting work based on the new message
        await Task.CompletedTask;
    }


    private async Task<List<string>> GetEmailsOfUsersWithRoleAsync(ILambdaContext context, string roleName)
    {
        var emails = new List<string>();

        string dbConnectionString = await GetSecret(context, "DDAC_RDSConnection");
        using (SqlConnection connection = new SqlConnection(dbConnectionString))
        {
            await connection.OpenAsync();

            string query = @"SELECT u.Email
                         FROM AspNetUsers u
                         JOIN AspNetUserRoles ur ON u.Id = ur.UserId
                         JOIN AspNetRoles r ON ur.RoleId = r.Id
                         WHERE r.Name = @RoleName";

            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@RoleName", roleName);

                using (SqlDataReader reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        emails.Add(reader.GetString(0));
                    }
                }
            }
        }

        return emails;
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

    public class PromotionMessage
    {
        public int HandicraftId { get; set; } = 0;
        public string HandicraftName { get; set; } = "";
        public string HandicraftDescription { get; set; } = "";
        public string HandicraftTypeName { get; set; } = "";
        public string HandicraftAuthorName { get; set; } = "";
        public string HandicraftAuthorEmail { get; set; } = "";
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
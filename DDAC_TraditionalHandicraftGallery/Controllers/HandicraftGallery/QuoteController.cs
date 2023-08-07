using Amazon;
using Amazon.Runtime;
using Amazon.SimpleNotificationService;
using Amazon.SimpleNotificationService.Model;
using DDAC_TraditionalHandicraftGallery.DataAccess;
using DDAC_TraditionalHandicraftGallery.Models;
using DDAC_TraditionalHandicraftGallery.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Data;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace DDAC_TraditionalHandicraftGallery.Controllers.HandicraftGallery
{
    public class QuoteController : Controller
    {
        private readonly ApplicationDbContext _context;

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IAmazonSimpleNotificationService _snsClient;

        private readonly string _snsTopicArn;

        public QuoteController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IOptions<AWSConfig> awsConfig, IConfiguration configuration)
        {
            _snsTopicArn = configuration["QuoteRequestSnsTopic"];
            _context = context;
            _userManager = userManager;

            var credentials = new SessionAWSCredentials(awsConfig.Value.AccessKey, awsConfig.Value.SecretKey, awsConfig.Value.SessionToken);
            var config = new AmazonSimpleNotificationServiceConfig { RegionEndpoint = RegionEndpoint.GetBySystemName(awsConfig.Value.Region) };

            _snsClient = new AmazonSimpleNotificationServiceClient(credentials, config);
        }


        [HttpPost]
        public async Task<IActionResult> RequestQuote(int id)
        {
            Console.WriteLine("Hi request quote");
            var handicraft = await _context.Handicrafts.FindAsync(id);
            if (handicraft == null || handicraft.IsHidden)
            {
                BadRequest("Unable to process the quote request.");
            }

            var user = await _userManager.GetUserAsync(User);

            if (user != null && handicraft != null)
            {
                var quoteRequest = new QuoteRequest
                {
                    UserID = user.Id,
                    HandicraftID = handicraft.Id,
                };

                _context.Add(quoteRequest);
                await _context.SaveChangesAsync();
                var quoteRequestId = quoteRequest.Id;

                var SNSQuoteRequest = new
                {
                    QuoteRequestId = quoteRequestId, // include the ID here
                    UserName = user.UserName,
                    UserEmail = user.Email,
                    HandicraftName = handicraft.Name,
                    HandicraftId = handicraft.Id,
                    HandicraftAuthorName = handicraft.AuthorName,
                    HandicraftAuthorEmail = handicraft.AuthorEmail,
                    RequestedAt = quoteRequest.RequestDate,
                    // Add other necessary fields
                };

                var request = new PublishRequest
                {
                    TopicArn = _snsTopicArn,
                    Message = JsonSerializer.Serialize(SNSQuoteRequest)
                };

                var response = await _snsClient.PublishAsync(request);

                if (response.HttpStatusCode == System.Net.HttpStatusCode.OK)
                {
                    Console.WriteLine("SENT TO SNS QUOTE REQUEST");
                    return RedirectToAction("Item", "Gallery", new { id = handicraft.Id, requestSent = true });
                }
            }

            // Handle error or invalid input
            return BadRequest("Unable to process the quote request.");
        }
    }
}

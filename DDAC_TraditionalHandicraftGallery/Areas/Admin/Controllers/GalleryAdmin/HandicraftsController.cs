﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DDAC_TraditionalHandicraftGallery.DataAccess;
using DDAC_TraditionalHandicraftGallery.Models;
using Microsoft.AspNetCore.Authorization;
using System.Data;
using DDAC_TraditionalHandicraftGallery.ViewModels;
using System.IO;
using Amazon.Runtime;
using Amazon.SimpleNotificationService;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Amazon.SimpleNotificationService.Model;
using System.Text.Json;
using Amazon;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Amazon.S3;
using Amazon.S3.Model;

namespace DDAC_TraditionalHandicraftGallery.Areas.Admin.Controllers.GalleryAdmin
{
    [Area("Admin")]
    public class HandicraftsController : Controller
    {
        private readonly ApplicationDbContext _context;

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IAmazonSimpleNotificationService _snsClient;
        private readonly IAmazonS3 _s3Client;

        private readonly string _snsTopicArn;
        private readonly string _s3BucketName;

        public HandicraftsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IOptions<AWSConfig> awsConfig, IConfiguration configuration, IWebHostEnvironment env)
        {
            _snsTopicArn = configuration["PromoteHandicraftSnsTopic"];
            _s3BucketName = configuration["S3BucketName"];
            _context = context;
            _userManager = userManager;
            var config = new AmazonSimpleNotificationServiceConfig { RegionEndpoint = RegionEndpoint.GetBySystemName(awsConfig.Value.Region) };
            var s3config = new AmazonS3Config { RegionEndpoint = RegionEndpoint.GetBySystemName(awsConfig.Value.Region) };

            if (env.IsDevelopment())
            {
                Console.WriteLine("DEVELOPMENT");
                // In development, use the provided AWS credentials
                var credentials = new SessionAWSCredentials(awsConfig.Value.AccessKey, awsConfig.Value.SecretKey, awsConfig.Value.SessionToken);

                _snsClient = new AmazonSimpleNotificationServiceClient(credentials, config);
                _s3Client = new AmazonS3Client(credentials, s3config);
            }
            else
            {
                Console.WriteLine("PRODUCTION");
                // In production, use the default credentials provider (i.e., the IAM role)
                _snsClient = new AmazonSimpleNotificationServiceClient(config);
                _s3Client = new AmazonS3Client(s3config);
            }

        }

        // GET: Admin/Handicrafts
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Handicrafts.Include(h => h.Type);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Admin/Handicrafts/Details/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Details(int? id, bool promoteSent = false)
        {
            if (id == null || _context.Handicrafts == null)
            {
                return NotFound();
            }

            var handicraft = await _context.Handicrafts
                .Include(h => h.Type)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (handicraft == null)
            {
                return NotFound();
            }

            ViewBag.PromoteSent = promoteSent;
            return View(handicraft);
        }

        // GET: Admin/Handicrafts/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            ViewData["TypeId"] = new SelectList(_context.HandicraftTypes, "Id", "Name");
            return View();
        }

        // POST: Admin/Handicrafts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([FromForm] HandicraftViewModel handicraftViewModel)
        {
            if (ModelState.IsValid)
            {
                var handicraft = new Handicraft
                {
                    Name = handicraftViewModel.Name,
                    Description = handicraftViewModel.Description,
                    AuthorName = handicraftViewModel.AuthorName,
                    AuthorEmail = handicraftViewModel.AuthorEmail,
                    TypeId = handicraftViewModel.TypeId,
                    IsHidden = handicraftViewModel.IsHidden,
                };
                if (handicraftViewModel.ImageURLFile != null && handicraftViewModel.ImageURLFile.Length > 0)
                {
                    // Check if the uploaded file is an image
                    string[] allowedExtensions = { ".jpg", ".jpeg", ".png", ".gif" }; // Add more extensions if needed
                    string fileExtension = Path.GetExtension(handicraftViewModel.ImageURLFile.FileName).ToLowerInvariant();
                    if (!allowedExtensions.Contains(fileExtension))
                    {
                        ModelState.AddModelError("ImageURLFile", "The uploaded file is not a valid image.");
                        ViewData["TypeId"] = new SelectList(_context.HandicraftTypes, "Id", "Name", handicraftViewModel.TypeId);
                        return View("Create", handicraftViewModel);
                    }
                    else if (handicraftViewModel.ImageURLFile.Length > 3 * 1024 * 1024)
                    {
                        ModelState.AddModelError("ImageURLFile", $"The uploaded image size exceeds the maximum allowed size of 3mb.");
                        ViewData["TypeId"] = new SelectList(_context.HandicraftTypes, "Id", "Name", handicraftViewModel.TypeId);
                        return View("Create", handicraftViewModel);
                    }
                    else
                    {
                        using (var memoryStream = new MemoryStream())
                        {
                            await handicraftViewModel.ImageURLFile.CopyToAsync(memoryStream);

                            var uploadRequest = new PutObjectRequest
                            {
                                BucketName = _s3BucketName,
                                Key = "images/" + DateTime.UtcNow.ToString("yyyyMMddHHmmss") + "-" + handicraftViewModel.ImageURLFile.FileName,
                                InputStream = memoryStream,
                                CannedACL = S3CannedACL.PublicRead
                            };

                            var response = await _s3Client.PutObjectAsync(uploadRequest);

                            if (response.HttpStatusCode == System.Net.HttpStatusCode.OK)
                            {
                                handicraft.ImageURL = $"https://{uploadRequest.BucketName}.s3.amazonaws.com/{uploadRequest.Key}";
                            }
                        }
                    }
                }
                // pending handle the file upload
                //await handicraft.ImageURLFile.CopyToAsync(memoryStream);

                //// Upload the file to AWS S3 and get the URL, below is a custom function that need to be defined before use
                //var imageUrl = await _awsS3Service.UploadFile(memoryStream, handicraft.ImageURLFile.FileName);

                _context.Add(handicraft);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            if (!ModelState.IsValid)
            {
                foreach (var modelStateKey in ModelState.Keys)
                {
                    var modelStateVal = ModelState[modelStateKey];
                    foreach (var error in modelStateVal.Errors)
                    {
                        // Log or print the error message
                        Console.WriteLine($"Key: {modelStateKey}, Error: {error.ErrorMessage}");
                    }
                }
            }

            ViewData["TypeId"] = new SelectList(_context.HandicraftTypes, "Id", "Name", handicraftViewModel.TypeId);
            return View(handicraftViewModel);
        }

        // GET: Admin/Handicrafts/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Handicrafts == null)
            {
                return NotFound();
            }

            var handicraft = await _context.Handicrafts.FindAsync(id);
            if (handicraft == null)
            {
                return NotFound();
            }
            ViewData["TypeId"] = new SelectList(_context.HandicraftTypes, "Id", "Name", handicraft.TypeId);

            HandicraftViewModel handicraftViewModel = new HandicraftViewModel
            {
                Id = handicraft.Id,
                Name = handicraft.Name,
                Description = handicraft.Description,
                AuthorName = handicraft.AuthorName,
                AuthorEmail = handicraft.AuthorEmail,
                TypeId = handicraft.TypeId,
                IsHidden = handicraft.IsHidden,
                ImageURL = handicraft.ImageURL,
            };

            return View(handicraftViewModel);
        }

        // POST: Admin/Handicrafts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, HandicraftViewModel handicraft)
        {
            if (id != handicraft.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {

                    if (handicraft.ImageURLFile != null && handicraft.ImageURLFile.Length > 0)
                    {
                        // Check if the uploaded file is an image
                        string[] allowedExtensions = { ".jpg", ".jpeg", ".png", ".gif" }; // Add more extensions if needed
                        string fileExtension = Path.GetExtension(handicraft.ImageURLFile.FileName).ToLowerInvariant();
                        if (!allowedExtensions.Contains(fileExtension))
                        {
                            ModelState.AddModelError("ImageURLFile", "The uploaded file is not a valid image.");
                            ViewData["TypeId"] = new SelectList(_context.HandicraftTypes, "Id", "Name", handicraft.TypeId);
                            return View("Edit", handicraft);
                        }
                        else if (handicraft.ImageURLFile.Length > 3 * 1024 * 1024)
                        {
                            ModelState.AddModelError("ImageURLFile", $"The uploaded image size exceeds the maximum allowed size of 3mb.");
                            ViewData["TypeId"] = new SelectList(_context.HandicraftTypes, "Id", "Name", handicraft.TypeId);
                            return View("Edit", handicraft);
                        }

                        if (handicraft.ImageURL != null)
                        {
                            //delete the old image first
                            Uri uri = new Uri(handicraft.ImageURL);
                            string objectKey = uri.AbsolutePath.TrimStart('/');
                            Console.WriteLine(objectKey);

                            var deleteRequest = new DeleteObjectRequest
                            {
                                BucketName = _s3BucketName,
                                Key = objectKey
                            };

                            DeleteObjectResponse response = await _s3Client.DeleteObjectAsync(deleteRequest);
                            _context.Handicrafts.Remove(handicraft);
                        }

                        using (var memoryStream = new MemoryStream())
                        {
                            await handicraft.ImageURLFile.CopyToAsync(memoryStream);

                            var uploadRequest = new PutObjectRequest
                            {
                                BucketName = _s3BucketName,
                                Key = "images/" + DateTime.UtcNow.ToString("yyyyMMddHHmmss") + "-" + handicraft.ImageURLFile.FileName,
                                InputStream = memoryStream,
                                CannedACL = S3CannedACL.PublicRead
                            };

                            await _s3Client.PutObjectAsync(uploadRequest);
                            handicraft.ImageURL = $"https://{uploadRequest.BucketName}.s3.amazonaws.com/{uploadRequest.Key}";
                        }

                    }

                    _context.Update(handicraft);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HandicraftExists(handicraft.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["TypeId"] = new SelectList(_context.HandicraftTypes, "Id", "Name", handicraft.TypeId);
            return View(handicraft);
        }

        // GET: Admin/Handicrafts/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Handicrafts == null)
            {
                return NotFound();
            }

            var handicraft = await _context.Handicrafts
                .Include(h => h.Type)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (handicraft == null)
            {
                return NotFound();
            }

            return View(handicraft);
        }

        // POST: Admin/Handicrafts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Handicrafts == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Handicrafts'  is null.");
            }
            var handicraft = await _context.Handicrafts.FindAsync(id);
            if (handicraft != null)
            {
                try
                {
                    Uri uri = new Uri(handicraft.ImageURL);
                    string objectKey = uri.AbsolutePath.TrimStart('/');
                    Console.WriteLine(objectKey);

                    var deleteRequest = new DeleteObjectRequest
                    {
                        BucketName = _s3BucketName,
                        Key = objectKey
                    };

                    DeleteObjectResponse response = await _s3Client.DeleteObjectAsync(deleteRequest);
                    _context.Handicrafts.Remove(handicraft);
                }
                catch (Exception ex)
                {
                    BadRequest(ex.Message);
                }

            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool HandicraftExists(int id)
        {
            return _context.Handicrafts.Any(e => e.Id == id);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> SendPromotion(int id)
        {
            Console.WriteLine("Hi promote");
            var handicraft = await _context.Handicrafts.Include(h => h.Type) // Include the related Type object
                               .FirstOrDefaultAsync(h => h.Id == id);
            if (handicraft == null || handicraft.IsHidden)
            {
                BadRequest("Unable to process the quote request.");
            }

            if (handicraft != null)
            {
                var promotionInformation = new
                {
                    HandicraftName = handicraft.Name,
                    HandicraftId = handicraft.Id,
                    HandicraftTypeName = handicraft.Type.Name,
                    HandicraftDescription = handicraft.Description,
                    HandicraftAuthorName = handicraft.AuthorName,
                    HandicraftAuthorEmail = handicraft.AuthorEmail
                };

                var request = new PublishRequest
                {
                    TopicArn = _snsTopicArn,
                    Message = JsonSerializer.Serialize(promotionInformation)
                };

                var response = await _snsClient.PublishAsync(request);

                if (response.HttpStatusCode == System.Net.HttpStatusCode.OK)
                {
                    Console.WriteLine("SENT TO SNS");
                    return RedirectToAction("Details", new { id = handicraft.Id, promoteSent = true });
                }
            }

            // Handle error or invalid input
            return BadRequest("Unable to process the quote request.");
        }
    }
}

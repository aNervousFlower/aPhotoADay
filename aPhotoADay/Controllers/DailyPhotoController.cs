using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using aPhotoADay.Services;
using aPhotoADay.Models;
using Microsoft.AspNetCore.Http;
using System.Globalization;

namespace aPhotoADay.Controllers
{
    public class DailyPhotoController : Controller
    {
        private readonly IDailyPhotoService _dailyPhotoService;
        public DailyPhotoController(IDailyPhotoService dailyPhotoService)
        {
            _dailyPhotoService = dailyPhotoService;
        }

        public async Task<IActionResult> Index(string id)
        {
            DateTimeOffset photoDate = DateTimeOffset.ParseExact(id, "yyyyMMdd",CultureInfo.InvariantCulture);
            if (photoDate.Date > DateTimeOffset.Now.Date)
            {
                return RedirectToAction("Index", new { id = DateTimeOffset.Now.ToString("yyyyMMdd") });
            }
            var photoArray = await _dailyPhotoService.GetDailyPhotoAsync(photoDate);

            var model = new DailyPhotoViewModel(photoArray, photoDate);

            return View(model);
        }

        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UploadPhoto(DailyPhoto newPhoto, IFormFile photoFile)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Index", new { id = newPhoto.PhotoDate.ToString("yyyyMMdd") });
            }

            var successful = await _dailyPhotoService.AddDailyPhotoAsync(newPhoto, photoFile);
            if (!successful)
            {
                return BadRequest("Could not add photo.");
            }
            
            return RedirectToAction("Index", new { id = newPhoto.PhotoDate.ToString("yyyyMMdd") });
        }
    }
}
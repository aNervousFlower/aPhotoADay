using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using aPhotoADay.Data;
using aPhotoADay.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace aPhotoADay.Services
{
    public class DailyPhotoService : IDailyPhotoService
    {
        private readonly ApplicationDbContext _context;
        private readonly IFileProvider _fileProvider;
        private readonly IWebHostEnvironment _hostingEnv;
        public DailyPhotoService(ApplicationDbContext context, IFileProvider fileProvider, IWebHostEnvironment env)
        {
            _context = context;
            _fileProvider = fileProvider;
            _hostingEnv = env;
        }

        public async Task<DailyPhoto[]> GetDailyPhotoAsync(DateTimeOffset photoDate)
        {
            return await _context.Photos.Where(x => x.PhotoDate == photoDate).ToArrayAsync();
        }

        public async Task<bool> AddDailyPhotoAsync(DailyPhoto newPhoto, IFormFile photoFile)
        {
            if (photoFile != null && photoFile.Length != 0)
            {
                Guid photoId = Guid.NewGuid();
                FileInfo file = new FileInfo(photoFile.FileName);
                var newFileName = photoId + file.Extension;
                var webPath = _hostingEnv.WebRootPath;
                var pathToSave = @"\imageFiles\" + newFileName;
                var path = Path.Combine("", webPath + pathToSave);
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await photoFile.CopyToAsync(stream);
                }

                newPhoto.Id = photoId;
                newPhoto.PhotoPath = pathToSave;

                _context.Photos.Add(newPhoto);

                var saveResult = await _context.SaveChangesAsync();
                return saveResult == 1;               
            }

            return false;
        }
    }
}

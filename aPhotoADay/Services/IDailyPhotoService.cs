using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using aPhotoADay.Models;
using Microsoft.AspNetCore.Http;

namespace aPhotoADay.Services
{
    public interface IDailyPhotoService
    {
        Task<DailyPhoto[]> GetDailyPhotoAsync(DateTimeOffset photoDate);
        Task<bool> AddDailyPhotoAsync(DailyPhoto newPhoto, IFormFile photoFile);
    }
}
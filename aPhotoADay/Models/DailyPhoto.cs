using System;
using System.ComponentModel.DataAnnotations;

namespace aPhotoADay.Models
{
    public class DailyPhoto
    {
        public Guid Id { get; set; }
        public DateTimeOffset PhotoDate { get; set; }
        public string PhotoPath { get; set; }
        public string Comment { get; set; }
    }
}
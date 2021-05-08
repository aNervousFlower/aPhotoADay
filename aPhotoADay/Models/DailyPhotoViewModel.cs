using System;

namespace aPhotoADay.Models
{
    public class DailyPhotoViewModel
    {
        public DailyPhoto Photo { get; set; }
        public bool HasDailyPhoto { get; set; }
        public DateTimeOffset PhotoDate { get; set; }

        public DailyPhotoViewModel(DailyPhoto[] photoArray, DateTimeOffset photoDate)
        {
            PhotoDate = photoDate;
            if (photoArray.Length > 0)
            {
                Photo = photoArray[0];
                HasDailyPhoto = true;
            }
            else
            {
                HasDailyPhoto = false;
            }
        }

        public bool OnCurrentDay()
        {
            return PhotoDate.Date == DateTimeOffset.Now.Date;
        }

        public string AdjustDateString(double numDays)
        {
            return PhotoDate.AddDays(numDays).ToString("yyyyMMdd");
        }
    }
}
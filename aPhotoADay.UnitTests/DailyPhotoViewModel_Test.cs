using System;
using aPhotoADay.Models;
using Xunit;

namespace aPhotoADay.UnitTests
{
    public class DailyPhotoViewModel_Test
    {
        [Fact]
        public void Constructor_noPhoto_Test()
        {
            // Arrange
            DailyPhoto[] photoArray = new DailyPhoto[] {};
            DateTimeOffset date = new DateTimeOffset(2021, 1, 1, 0, 0, 0, TimeSpan.Zero).Date;

            // Act
            var viewModel = new DailyPhotoViewModel(photoArray, date);

            // Assert
            Assert.Equal(viewModel.PhotoDate, date);
            Assert.Equal(viewModel.HasDailyPhoto, false);
        }
        
        [Fact]
        public void Constructor_hasPhoto_Test()
        {
            // Arrange
            DailyPhoto photo = new DailyPhoto { Id = Guid.NewGuid() };
            DailyPhoto[] photoArray = new DailyPhoto[] { photo };
            DateTimeOffset date = new DateTimeOffset(2021, 1, 1, 0, 0, 0, TimeSpan.Zero).Date;

            // Act
            var viewModel = new DailyPhotoViewModel(photoArray, date);

            // Assert
            Assert.Equal(viewModel.PhotoDate, date);
            Assert.Equal(viewModel.HasDailyPhoto, true);
            Assert.Equal(photo.Id, viewModel.Photo.Id);
        }
        
        [Fact]
        public void OnCurrentDay_Test()
        {
            // Arrange
            DailyPhoto[] photoArray = new DailyPhoto[] {};
            DateTimeOffset date_false = new DateTimeOffset(2021, 1, 1, 0, 0, 0, TimeSpan.Zero).Date;
            DateTimeOffset date_true = DateTimeOffset.Now.Date;
            var viewModel_false = new DailyPhotoViewModel(photoArray, date_false);
            var viewModel_true = new DailyPhotoViewModel(photoArray, date_true);

            // Act
            var result_false = viewModel_false.OnCurrentDay();
            var result_true = viewModel_true.OnCurrentDay();

            // Assert
            Assert.False(result_false);
            Assert.True(result_true);
        }
        
        [Fact]
        public void AdjustDateString_Test()
        {
            // Arrange
            DailyPhoto[] photoArray = new DailyPhoto[] {};
            DateTimeOffset date = new DateTimeOffset(2021, 1, 1, 0, 0, 0, TimeSpan.Zero).Date;
            var viewModel = new DailyPhotoViewModel(photoArray, date);

            // Act
            var result_plus2 = viewModel.AdjustDateString(2);
            var result_minus1 = viewModel.AdjustDateString(-1);

            // Assert
            Assert.Equal("20210103", result_plus2);
            Assert.Equal("20201231", result_minus1);
        }
    }
}
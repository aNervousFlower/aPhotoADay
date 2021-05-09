using System;
using System.IO;
using System.Threading.Tasks;
using aPhotoADay.Data;
using aPhotoADay.Models;
using aPhotoADay.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace aPhotoADay.UnitTests
{
    public class DailyPhotoService_Test
    {
        [Fact]
        public async Task AddDailyPhotoAsync_Test()
        {
            //Arrange
            var mockEnvironment = new Mock<IWebHostEnvironment>();
            mockEnvironment
                .Setup(m => m.EnvironmentName)
                .Returns("Hosting:UnitTestEnvironment");

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "Test_AddDailyPhoto").Options;
            
            using (var context = new ApplicationDbContext(options))
            {
                var mockService = new Mock<DailyPhotoService>(MockBehavior.Strict, context, mockEnvironment.Object);
                mockService.Setup(m => m.SavePhoto(It.IsAny<IFormFile>(), It.IsAny<string>())).ReturnsAsync(true);
                
                var file = getMockFile();
                //Act
                await mockService.Object.AddDailyPhotoAsync(new DailyPhoto
                {
                    PhotoDate = new DateTimeOffset(2021, 1, 1, 0, 0, 0, TimeSpan.Zero).Date,
                    Comment = "Test Comment"
                }, file);
            }

            // Assert
            using (var context = new ApplicationDbContext(options))
            {
                var itemsInDatabase = await context.Photos.CountAsync();
                Assert.Equal(1, itemsInDatabase);

                var photo = await context.Photos.FirstAsync();

                Assert.Equal("Test Comment", photo.Comment);

                var difference = new DateTimeOffset(2021, 1, 1, 0, 0, 0, TimeSpan.Zero).Date - photo.PhotoDate;
                Assert.True(difference < TimeSpan.FromSeconds(1));

                string photoPath = photo.PhotoPath.Substring(0, photo.PhotoPath.LastIndexOf('\\') + 1);
                Assert.Equal(@"\imageFiles\", photoPath);
            }
        }

        public IFormFile getMockFile()
        {
            var fileMock = new Mock<IFormFile>();

            var content = "Hello World from a Fake File";
            var fileName = "test.jpg";
            var ms = new MemoryStream();
            var writer = new StreamWriter(ms);
            writer.Write(content);
            writer.Flush();
            ms.Position = 0;
            fileMock.Setup(_ => _.OpenReadStream()).Returns(ms);
            fileMock.Setup(_ => _.FileName).Returns(fileName);
            fileMock.Setup(_ => _.Length).Returns(ms.Length);

            return fileMock.Object;
        }
    }
}
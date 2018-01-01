using BookHeaven.Data;
using BookHeaven.Data.Models;
using BookHeaven.Services.Implementations;
using BookHeaven.Services.UtilityServices.Contracts;
using BookHeaven.Tests.Mocks;
using BookHeaven.Tests.Services.Models;
using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace BookHeaven.Tests.Services
{
    public class BookServiceTests
    {
        private readonly BookHeavenDbContext databaseMock;
        private readonly Mock<IFileService> fileServicMock;
        private readonly BookService sut;

        public BookServiceTests()
        {
            AutoMapperInitializer.Initialize();

            this.databaseMock = BookHeavenDbContextInMemory.New();
            this.fileServicMock = new Mock<IFileService>();
            this.sut = new BookService(this.databaseMock, this.fileServicMock.Object);
        }

        [Fact]
        public async Task CountBySearchTermAsyncShouldReturnCountOfAllWhenSearchTermIsEmpty()
        {
            //Arrange
            this.FillDatabase();

            //Act
            var count = await this.sut.CountBySearchTermAsync("");

            //Assert
            count.Should().Be(this.databaseMock.Books.Count());
        }

        [Fact]
        public async Task CountBySearchTermAsyncShouldReturnCorrectCountWhenSearchTermIsNotEmpty()
        {
            //Arrange
            const string searchTerm = "Drama";
            const int resultsCount = 1;
            this.FillDatabase();

            //Act
            var count = await this.sut.CountBySearchTermAsync(searchTerm);

            //Assert
            count.Should().Be(resultsCount);
        }

        [Fact]
        public async Task ByIdAsyncShouldReturnItemWhenExists()
        {
            //Arrange
            const int bookSearchId = 1;
            this.FillDatabase();

            //Act
            var book = await this.sut.ByIdAsync<BookTestModel>(bookSearchId);

            //Assert
            book.Should().NotBeNull();
            book.Id.Should().Be(bookSearchId);
        }

        [Fact]
        public async Task ByIdAsyncShouldReturnNullWhenDoesntExist()
        {
            //Arrange
            this.FillDatabase();

            //Act
            var book = await this.sut.ByIdAsync<BookTestModel>(int.MaxValue);

            //Assert
            book.Should().BeNull();
        }

        [Fact]
        public async Task CreateAsyncShouldCreateBookSuccessfullyIfAllDataIsCorrect()
        {
            //Arrange
            string bookTitle = Guid.NewGuid().ToString();
            const decimal bookPrice = 100;
            const string bookDescription = "Random Description";
            List<int> categoryIds = new List<int> { 1, 2, 3 };
            const string pictureLink = "RandomLink";
            const string listingPictureLink = "RandomListingLink";
            const string userId = "RandomId";

            //Act
            var result = await this.sut.CreateAsync(bookTitle, bookPrice, bookDescription, categoryIds,
                pictureLink, listingPictureLink, userId);

            //Assert
            var book = this.databaseMock.Books.Find(result);

            book.Should().NotBe(null);
            book.Price.Should().Be(bookPrice);
            book.Description.Should().Be(bookDescription);
            book.Categories.Select(c => c.CategoryId).ShouldBeEquivalentTo(categoryIds, options => options.WithStrictOrdering());
            book.BookPicture.Should().Be(pictureLink);
            book.BookListingPicture.Should().Be(listingPictureLink);
            book.PublisherId.Should().Be(userId);
        }

        private void FillDatabase()
        {
            this.databaseMock.Books.AddRange(new List<Book>
            {
                new Book
                {
                    Id = 1,
                    Title = "Action Book",
                    Description = "Random Description1",
                    Price = 3,
                    PublishedDate = DateTime.MinValue
                },
                new Book
                {
                    Id = 2,
                    Title = "Drama Book",
                    Description = "Random Description2",
                    Price = 3,
                    PublishedDate = DateTime.MinValue
                },
                new Book
                {
                    Id = 3,
                    Title = "Thriller Book",
                    Description = "Random Description3",
                    Price = 3,
                    PublishedDate = DateTime.MinValue
                }
            });

            this.databaseMock.SaveChanges();
        }
    }
}
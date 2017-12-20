﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using BookHeaven.Data;
using BookHeaven.Data.Models;
using BookHeaven.Services.Contracts;
using BookHeaven.Services.Implementations;
using BookHeaven.Services.UtilityServices.Contracts;
using BookHeaven.Tests.Mocks;
using BookHeaven.Tests.Services.Models;
using BookHeaven.Web.Infrastructure.Mapping;
using FluentAssertions;
using Moq;
using Xunit;

namespace BookHeaven.Tests.Services
{
    public class BookServiceTests
    {
        public BookServiceTests()
        {
            AutoMapperInitializer.Initialize();
        }

        [Fact]
        public async Task CountBySearchTermAsyncShouldReturnCountOfAllWhenSearchTermIsEmpty()
        {
            //Arrange
            var db = BookHeavenDbContextInMemory.New();
            this.FillDatabase(db);
            var service = new BookService(db, null, null);

            //Act
            var count = await service.CountBySearchTermAsync("");

            //Assert
            count.Should().Be(db.Books.Count());
        }

        [Fact]
        public async Task CountBySearchTermAsyncShouldReturnCorrectCountWhenSearchTermIsNotEmpty()
        {
            const string searchTerm = "Drama";
            const int resultsCount = 1;

            //Arrange
            var service = this.GetSimpleBookService();

            //Act
            var count = await service.CountBySearchTermAsync(searchTerm);

            //Assert
            count.Should().Be(resultsCount);
        }

        [Fact]
        public async Task ByIdAsyncShouldReturnItemWhenExists()
        {
            //Arrange
            const int bookSearchId = 1;

            var service = this.GetSimpleBookService();

            //Act
            var book = await service.ByIdAsync<BookTestModel>(bookSearchId);

            //Assert
            book.Should().BeOfType<BookTestModel>();
            book.Should().NotBeNull();
            book.Id.Should().Be(bookSearchId);
        }

        [Fact]
        public async Task ByIdAsyncShouldReturnNullWhenDoesntExist()
        {
            //Arrange
            var service = this.GetSimpleBookService();

            //Act
            var book = await service.ByIdAsync<BookTestModel>(int.MaxValue);

            //Assert
            book.Should().BeNull();
        }

        [Fact]
        public async Task CreateAsyncShouldReturnFalseIfCategoryDoesNotExist()
        {
            //Arrange
            var db = BookHeavenDbContextInMemory.New();

            var fileServiceMock = new Mock<IFileService>();
            var categoryServiceMock = new Mock<ICategoryService>();
            categoryServiceMock.Setup(c => c.ExistsAsync(It.IsAny<int>())).ReturnsAsync(false);
            var service = new BookService(db, categoryServiceMock.Object, fileServiceMock.Object);

            //Act
            var result = await service.CreateAsync(null, 0, null, new List<int> { 1, 2, 3 }, null, null, null);

            //Assert
            result.Should().BeFalse();
        }

        [Fact]
        public async Task CreateAsyncShouldCreateBookSuccessfullyIfAllDataIsCorrect()
        {
            //Arrange
            string bookTitle = Guid.NewGuid().ToString();
            const decimal bookPrice = 100;
            const string bookDescription = "Random Description";
            List<int> categoryIds = new List<int> {1, 2, 3};
            const string pictureLink = "RandomLink";
            const string listingPictureLink = "RandomListingLink";
            const string userId = "RandomId";

            var db = BookHeavenDbContextInMemory.New();

            var fileServiceMock = new Mock<IFileService>();
            var categoryServiceMock = new Mock<ICategoryService>();
            categoryServiceMock.Setup(c => c.ExistsAsync(It.IsAny<int>())).ReturnsAsync(true);
            var service = new BookService(db, categoryServiceMock.Object, fileServiceMock.Object);

            //Act
            var result = await service.CreateAsync(bookTitle, bookPrice, bookDescription, categoryIds,
                pictureLink, listingPictureLink, userId);

            //Assert
            result.Should().BeTrue();
            var book = db.Books.FirstOrDefault(b => b.Title == bookTitle);

            book.Should().NotBe(null);
            book.Price.Should().Be(bookPrice);
            book.Description.Should().Be(bookDescription);
            book.Categories.Select(c => c.CategoryId).ShouldBeEquivalentTo(categoryIds);
            book.BookPicture.ShouldBeEquivalentTo(pictureLink);
            book.BookListingPicture.Should().Be(listingPictureLink);
            book.PublisherId.Should().Be(userId);

        }

        private IBookService GetSimpleBookService()
        {
            var db = BookHeavenDbContextInMemory.New();
            this.FillDatabase(db);
            return new BookService(db, null, null);
        }

        private void FillDatabase(BookHeavenDbContext db)
        {
            db.Books.AddRange(new List<Book>
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

            db.SaveChanges();
        }


    }
}

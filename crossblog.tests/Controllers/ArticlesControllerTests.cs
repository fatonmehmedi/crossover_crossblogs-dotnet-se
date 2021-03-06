﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using crossblog.Controllers;
using crossblog.Domain;
using crossblog.Model;
using crossblog.Repositories;
using FizzWare.NBuilder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace crossblog.tests.Controllers
{
    public class ArticlesControllerTests
    {
        private ArticlesController _articlesController;

        private Mock<IArticleRepository> _articleRepositoryMock = new Mock<IArticleRepository>();

        public ArticlesControllerTests()
        {
            _articlesController = new ArticlesController(_articleRepositoryMock.Object);
        }

        [Fact]
        public async Task Search_ReturnsEmptyList()
        {
            // Arrange
            var articleDbSetMock = Builder<Article>.CreateListOfSize(3).Build().ToAsyncDbSetMock();
            _articleRepositoryMock.Setup(m => m.Query()).Returns(articleDbSetMock.Object);

            // Act
            var result = await _articlesController.Search("Invalid");

            // Assert
            Assert.NotNull(result);

            var objectResult = result as OkObjectResult;
            Assert.NotNull(objectResult);

            var content = objectResult.Value as ArticleListModel;
            Assert.NotNull(content);

            Assert.Equal(0, content.Articles.Count());
        }

        [Fact]
        public async Task Search_ReturnsList()
        {
            // Arrange
            var articleDbSetMock = Builder<Article>.CreateListOfSize(3).Build().ToAsyncDbSetMock();
            _articleRepositoryMock.Setup(m => m.Query()).Returns(articleDbSetMock.Object);

            // Act
            var result = await _articlesController.Search("Title");

            // Assert
            Assert.NotNull(result);

            var objectResult = result as OkObjectResult;
            Assert.NotNull(objectResult);

            var content = objectResult.Value as ArticleListModel;
            Assert.NotNull(content);

            Assert.Equal(3, content.Articles.Count());
        }

        [Fact]
        public async Task Get_NotFound()
        {
            // Arrange
            _articleRepositoryMock.Setup(m => m.GetAsync(1)).Returns(Task.FromResult<Article>(null));

            // Act
            var result = await _articlesController.Get(1);

            // Assert
            Assert.NotNull(result);

            var objectResult = result as NotFoundResult;
            Assert.NotNull(objectResult);
        }

        [Fact]
        public async Task Get_ReturnsItem()
        {
            // Arrange
            _articleRepositoryMock.Setup(m => m.GetAsync(1)).Returns(Task.FromResult<Article>(Builder<Article>.CreateNew().Build()));

            // Act
            var result = await _articlesController.Get(1);

            // Assert
            Assert.NotNull(result);

            var objectResult = result as OkObjectResult;
            Assert.NotNull(objectResult);

            var content = objectResult.Value as ArticleModel;
            Assert.NotNull(content);

            Assert.Equal("Title1", content.Title);
        }

        [Fact]
        public async Task Post_BadRequest()
        {
            // Arrange
            _articlesController.ModelState.AddModelError("Content", "Required");

            // Act
            var result = await _articlesController.Post(Builder<ArticleModel>.CreateNew().Build());

            // Assert
            Assert.NotNull(result);

            var objectResult = result as BadRequestObjectResult;
            Assert.NotNull(objectResult);
        }


        [Fact]
        public async Task Post_Ok()
        {
            // Arrange
            _articleRepositoryMock.Setup(m => m.GetAsync(0)).Returns(Task.FromResult<Article>(Builder<Article>.CreateNew().Build()));
            //_articleRepositoryMock.Setup(m => m.UpdateAsync(0)).Returns(Task.FromResult<Article>(Builder<Article>.CreateNew().Build()));

            // Act
            var result =  await _articlesController.Post(Builder<ArticleModel>.CreateNew().Build());

            // Assert
            Assert.NotNull(result);

            var objectResult = result as CreatedResult;
            Assert.NotNull(objectResult);
        }

        [Fact]
        public async Task Put_BadRequest()
        {
            // Arrange
            var controller = new ArticlesController(_articleRepositoryMock.Object);
            controller.ModelState.AddModelError("Content", "Required");

            // Act
            var result = await controller.Put(Builder<int>.CreateNew().Build(), Builder<ArticleModel>.CreateNew().Build());

            // Assert
            Assert.NotNull(result);

            var objectResult = result as BadRequestObjectResult;
            Assert.NotNull(objectResult);
        }

        [Fact]
        public async Task Put_Ok()
        {
            // Arrange
            _articleRepositoryMock.Setup(m => m.GetAsync(0)).Returns(Task.FromResult<Article>(Builder<Article>.CreateNew().Build()));
            //_articleRepositoryMock.Setup(m => m.UpdateAsync(0)).Returns(Task.FromResult<Article>(Builder<Article>.CreateNew().Build()));

            // Act
            var result = await _articlesController.Put(Builder<int>.CreateNew().Build(), Builder<ArticleModel>.CreateNew().Build());

            // Assert
            Assert.NotNull(result);

            var objectResult = result as OkObjectResult;
            Assert.NotNull(objectResult);
        }

        [Fact]
        public async Task Delete_NotFound()
        {
            // Arrange
            _articleRepositoryMock.Setup(m => m.GetAsync(0)).Returns(Task.FromResult<Article>(null));

            // Act
            var result = await _articlesController.Delete(Builder<int>.CreateNew().Build());

            // Assert
            Assert.NotNull(result);

            var objectResult = result as NotFoundResult;
            Assert.NotNull(objectResult);
        }

        [Fact]
        public async Task Delete_OK()
        {
            // Arrange
            _articleRepositoryMock.Setup(m => m.GetAsync(0)).Returns(Task.FromResult<Article>(Builder<Article>.CreateNew().Build()));
            _articleRepositoryMock.Setup(m => m.DeleteAsync(0)).Returns(Task.FromResult<Article>(Builder<Article>.CreateNew().Build()));

            // Act
            var result = await _articlesController.Delete(Builder<int>.CreateNew().Build());

            // Assert
            Assert.NotNull(result);

            var objectResult = result as OkResult;
            Assert.NotNull(objectResult);
        }
    }
}

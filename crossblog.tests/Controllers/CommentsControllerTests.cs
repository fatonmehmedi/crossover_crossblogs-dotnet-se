using System;
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
    public class CommentsControllerTests
    {
        private CommentsController _commentsController;

        private Mock<IArticleRepository> _articleRepositoryMock = new Mock<IArticleRepository>();
        private Mock<ICommentRepository> _commentRepositoryMock = new Mock<ICommentRepository>();

        public CommentsControllerTests()
        {
            _commentsController = new CommentsController(_articleRepositoryMock.Object, _commentRepositoryMock.Object);
        }

        [Fact]
        public async Task Get_NotFound()
        {
            // Arrange
            _articleRepositoryMock.Setup(m => m.GetAsync(1)).Returns(Task.FromResult<Article>(null));

            // Act
            var result = await _commentsController.Get(1);

            // Assert
            Assert.NotNull(result);

            var objectResult = result as NotFoundResult;
            Assert.NotNull(objectResult);
            
        }

        [Fact]
        public async Task Get_Ok()
        {
            // Arrange
            _articleRepositoryMock.Setup(m => m.GetAsync(1)).Returns(Task.FromResult<Article>(Builder<Article>.CreateNew().Build()));
            
            var commentDbSetMock = Builder<Comment>.CreateListOfSize(3).Build().ToAsyncDbSetMock();
            _commentRepositoryMock.Setup(m => m.Query()).Returns(commentDbSetMock.Object);

            // Act
            var result = await _commentsController.Get(1);

            // Assert
            Assert.NotNull(result);

            var objectResult = result as OkObjectResult;
            Assert.NotNull(objectResult);
            
        }

        [Fact]
        public async Task Get_Comment_NotFound()
        {
            // Arrange
            _articleRepositoryMock.Setup(m => m.GetAsync(1)).Returns(Task.FromResult<Article>(null));

            // Act
            var result = await _commentsController.Get(1,1);

            // Assert
            Assert.NotNull(result);

            var objectResult = result as NotFoundResult;
            Assert.NotNull(objectResult);
            
        }

        [Fact]
        public async Task Get_Comment_Ok()
        {
            // Arrange
            _articleRepositoryMock.Setup(m => m.GetAsync(1)).Returns(Task.FromResult<Article>(Builder<Article>.CreateNew().Build()));
            
            var commentDbSetMock = Builder<Comment>.CreateListOfSize(1).Build().ToAsyncDbSetMock();
            _commentRepositoryMock.Setup(m => m.Query())
                .Returns(commentDbSetMock.Object);

            // Act
            var result = await _commentsController.Get(1, 1);

            // Assert
            Assert.NotNull(result);

            var objectResult = result as OkObjectResult;
            Assert.NotNull(objectResult);
        }
        
        
        [Fact]
        public async Task Post_ModelError_BadRequest()
        {
            // Arrange
            _commentsController.ModelState.AddModelError("Content", "Required");

            // Act
            var result = await _commentsController.Post(0, Builder<CommentModel>.CreateNew().Build());

            // Assert
            Assert.NotNull(result);

            var objectResult = result as BadRequestObjectResult;
            Assert.NotNull(objectResult);
        }
        
        [Fact]
        public async Task Post_Article_BadRequest()
        {
            // Arrange
            _articleRepositoryMock.Setup(m => m.GetAsync(0)).Returns(Task.FromResult<Article>(null));
            
            // Act
            var result = await _commentsController.Post(0, Builder<CommentModel>.CreateNew().Build());

            // Assert
            Assert.NotNull(result);

            var objectResult = result as NotFoundResult;
            Assert.NotNull(objectResult);
        }

        [Fact]
        public async Task Post_Ok()
        {
            // Arrange
            _articleRepositoryMock.Setup(m => m.GetAsync(0)).Returns(Task.FromResult<Article>(Builder<Article>.CreateNew().Build()));
            //_commentRepositoryMock.Setup(m => m.InsertAsync(It.IsAny<Comment>()).Returns( Task.CompletedTask);

            // Act
            var result =  await _commentsController.Post(0, Builder<CommentModel>.CreateNew().Build());

            // Assert
            Assert.NotNull(result);

            var objectResult = result as CreatedResult;
            Assert.NotNull(objectResult);
        }
    }
}

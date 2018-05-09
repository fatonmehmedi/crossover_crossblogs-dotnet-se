# Crossover - Project assessment C# (.NET) Software Engineer

This is the solution from Crossover - Project assessment for C# (.NET) Software Engineer position.
The assessment is of date 05/05/2018.

# Coveage Result Task 1

| File                                                                | Lines | Covered Lines | Percentage |
|---------------------------------------------------------------------|-------|---------------|------------|
| crossblog/Controllers/ArticlesController.cs                         |   55  |       54      |   98.182%  |
| crossblog/Controllers/CommentsController.cs                         |   61  |       60      |   98.361%  |
| crossblog/Domain/Article.cs                                         |    4  |        4      |  100.000%  |
| crossblog/Domain/BaseEntity.cs                                      |    3  |        3      |  100.000%  |
| crossblog/Domain/Comment.cs                                         |    7  |        7      |  100.000%  |
| crossblog/Domain/CrossBlogDbContext.cs                              |    4  |        0      |    0.000%  |
| crossblog/Exceptions/HttpStatusCodeException.cs                     |    9  |        0      |    0.000%  |
| crossblog/Exceptions/HttpStatusCodeExceptionMiddleware.cs           |   13  |        0      |    0.000%  |
| crossblog/Exceptions/HttpStatusCodeExceptionMiddlewareExtensions.cs |    1  |        0      |    0.000%  |
| crossblog/Model/ArticleListModel.cs                                 |    1  |        1      |  100.000%  |
| crossblog/Model/ArticleModel.cs                                     |    5  |        5      |  100.000%  |
| crossblog/Model/CommentListModel.cs                                 |    1  |        1      |  100.000%  |
| crossblog/Model/CommentModel.cs                                     |    6  |        6      |  100.000%  |
| crossblog/Repositories/ArticleRepository.cs                         |    2  |        0      |    0.000%  |
| crossblog/Repositories/CommentRepository.cs                         |    2  |        0      |    0.000%  |
| crossblog/Repositories/GenericRepository.cs                         |   13  |        0      |    0.000%  |
|  |  |  |  |
| All files                                                           |  187  |      141      |   75.401%  |

# Fix Bugs Task 2

Fix Article not deleting bug. On GenericRepository.cs line 43

        T entity =  await GetAsync(id);

# Optimize DB Task 3

Changed Content column to varchar 32000

        [StringLength(32000)]
        [Column(TypeName="VARCHAR(32000)")]
        public string Content { get; set; }

Added fulltext index to Articles.Content column

        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("ALTER TABLE `crossblog`.`Articles` ADD FULLTEXT INDEX `ContentTextIndex` (`Content` ASC);");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("ALTER TABLE `crossblog`.`Articles` DROP INDEX `textIndex`;");
        }

# Extra 

Postman Collection for testing API (8 requests)

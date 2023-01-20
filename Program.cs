﻿using BALTA.IO_FundamentosDataAcess.Models;
using Dapper;
using Microsoft.Data.SqlClient;

internal class Program
{
    private static void Main(string[] args)
    {
        const string connectionString = "Server=DEVELOPMENT;Database=balta;Integrated Security=True;TrustServerCertificate=True";

        using (var connection = new SqlConnection(connectionString))
        {
            // UpdateCategory(connection);
            // CreateCategory(connection);
            // DeleteCategory(connection);
            // GetCategory(connection);
            // CreateManyCategories(connection);
            // ExecuteProcedure(connection);
            // ExecuteReadProcedure(connection);
            // ExecuteScalar(connection);
            // ReadView(connection);
            // ListCategories(connection);
            // OneToOne(connection);
            // OneToMany(connection);
            // QueryMultiple(connection);
            // SelectIn(connection);
            // Like(connection);
            Transaction(connection);

        }
    }

    static void ListCategories(SqlConnection connection)
    {

        var categories = connection.Query<Category>("SELECT [Id], [Title] FROM [Category]");
        foreach (var item in categories)
        {
            Console.WriteLine($"{item.Id} - {item.Title}");
        }
    }

    static void CreateCategory(SqlConnection connection)
    {

        var category = new Category();
        category.Id = Guid.NewGuid();
        category.Title = "Amazon AWS";
        category.Url = "amazon";
        category.Description = "Categoria destinada a serviços do AWS";
        category.Order = 8;
        category.Summary = "AWS Cloud";
        category.Featured = false;

        var insertSql = @"INSERT INTO
                [Category]
            VALUES(
                @Id,
                @Title,
                @Url,
                @Summary,
                @Order,
                @Description,
                @Featured)";

        var rows = connection.Execute(insertSql, new
        {
            category.Id,
            category.Title,
            category.Url,
            category.Summary,
            category.Order,
            category.Description,
            category.Featured
        });
        Console.WriteLine($"{rows} linha inserida!");
    }

    static void UpdateCategory(SqlConnection connection)
    {
        var updateQuery = "UPDATE [Category] SET [Title]=@title WHERE [Id]=@id";
        var rows = connection.Execute(updateQuery, new
        {
            id = "AF3407AA-11AE-4621-A2EF-2028B85507C4",
            title = "Frontend"
        });

        Console.WriteLine($" {rows} registro atualizado!");
    }

    static void DeleteCategory(SqlConnection connection)
    {
        var deleteQuery = "DELETE FROM [Category] WHERE [Id]=@id";
        var rows = connection.Execute(deleteQuery, new
        {
            id = "36d72e29-0a41-4724-9817-824ad4ea844b",
        });

        Console.WriteLine($" {rows} registro deletado!");
    }

    static void GetCategory(SqlConnection connection)
    {
        var category = connection
            .QueryFirstOrDefault<Category>(
                "SELECT TOP 1 [Id], [Title] FROM [Category] WHERE [Id]=@id",
                new
                {
                    id = "af3407aa-11ae-4621-a2ef-2028b85507c4"
                });
        Console.WriteLine($"{category.Id} - {category.Title}");
    }

    static void CreateManyCategories(SqlConnection connection)
    {

        var category = new Category();
        category.Id = Guid.NewGuid();
        category.Title = "Amazon AWS";
        category.Url = "amazon";
        category.Description = "Categoria destinada a serviços do AWS";
        category.Order = 8;
        category.Summary = "AWS Cloud";
        category.Featured = false;

        var category2 = new Category();
        category2.Id = Guid.NewGuid();
        category2.Title = "Amazon AWS";
        category2.Url = "amazon";
        category2.Description = "Categoria destinada a serviços do AWS";
        category2.Order = 8;
        category2.Summary = "AWS Cloud";
        category2.Featured = false;

        var insertSql = @"INSERT INTO
                [Category]
            VALUES(
                @Id,
                @Title,
                @Url,
                @Summary,
                @Order,
                @Description,
                @Featured)";

        var rows = connection.Execute(insertSql, new[]{
            new
            {
            category.Id,
            category.Title,
            category.Url,
            category.Summary,
            category.Order,
            category.Description,
            category.Featured
            },
            new
            {
            category2.Id,
            category2.Title,
            category2.Url,
            category2.Summary,
            category2.Order,
            category2.Description,
            category2.Featured
            }
        }
        );
        Console.WriteLine($"{rows} linhas inseridas!");
    }

    static void ExecuteProcedure(SqlConnection connection)
    {
        var procedure = "[spDeleteStudent]";
        var pars = new { StudentId = "CF76945B-76D3-4CFB-AF30-FED15F0171A1" };

        var rows = connection.Execute(procedure, pars, commandType: System.Data.CommandType.StoredProcedure);

        Console.WriteLine($"{rows} registro deletado!");
    }

    static void ExecuteReadProcedure(SqlConnection connection)
    {
        var procedure = "[spGetCoursesByCategory]";
        var pars = new { CategoryId = "09CE0B7B-CFCA-497B-92C0-3290AD9D5142" };

        var courses = connection.Query(procedure,
         pars,
          commandType: System.Data.CommandType.StoredProcedure
          );

        foreach (var item in courses)
        {
            Console.WriteLine(item.Id);
        }

    }

    static void ExecuteScalar(SqlConnection connection)
    {
        var category = new Category();
        category.Title = "Amazon AWS";
        category.Url = "amazon";
        category.Description = "Categoria destinada a serviços do AWS";
        category.Order = 8;
        category.Summary = "AWS Cloud";
        category.Featured = false;

        var insertSql = @"INSERT INTO
                [Category]
            OUTPUT inserted.[Id]
            VALUES(
                NEWID(),
                @Title,
                @Url,
                @Summary,
                @Order,
                @Description,
                @Featured)";

        var id = connection.ExecuteScalar<Guid>(insertSql, new
        {
            category.Title,
            category.Url,
            category.Summary,
            category.Order,
            category.Description,
            category.Featured
        });
        Console.WriteLine($"O ID da nova categoria é {id}.");

    }

    static void ReadView(SqlConnection connection)
    {
        var sql = "SELECT * FROM [vwCourses]";
        var courses = connection.Query(sql);
        foreach (var item in courses)
        {
            Console.WriteLine($"{item.Id} - {item.Title}");
        }
    }

    static void OneToOne(SqlConnection connection)
    {
        var sql = @"SELECT * FROM [CareerItem] INNER JOIN [Course]
                    ON [CareerItem].[CourseId] = [Course].[Id]";

        var items = connection.Query<CareerItem, Course, CareerItem>(
            sql,
            (careerItem, course) =>
            {
                careerItem.Course = course;
                return careerItem;
            }, splitOn: "Id");

        foreach (var item in items)
        {
            Console.WriteLine($"{item.Title} - Curso:{item.Course.Title}");
        }
    }

    static void OneToMany(SqlConnection connection)
    {
        var sql = @"SELECT 
                    [C].[Id],
                    [C].[Title],
                    [CI].[CareerId],
                    [CI].[Title]

                    FROM [Career] C
                    INNER JOIN [CareerItem] CI
                    ON CI.[CareerId] = C.[Id]
                    ORDER BY 
                    [C].[Title]";

        var careers = new List<Career>();

        var items = connection.Query<Career, CareerItem, Career>(
            sql,
            (career, item) =>
            {
                var car = careers.Where(x => x.Id == career.Id).FirstOrDefault();
                if (car == null)
                {
                    car = career;
                    car.Items.Add(item);
                    careers.Add(car);
                }
                else
                {
                    car.Items.Add(item);
                }
                return career;
            }, splitOn: "CareerId");

        foreach (var career in careers)
        {

            Console.WriteLine($"***********************************{career.Title}***********************************");
            foreach (var item in career.Items)
            {
                Console.WriteLine($"{item.Title}");

            };

        }
    }

    static void QueryMultiple(SqlConnection connection)
    {
        var query = "SELECT * FROM [Category]; SELECT * FROM [Course]";

        using (var multi = connection.QueryMultiple(query))
        {
            var categories = multi.Read<Category>();
            var courses = multi.Read<Course>();

            foreach (var item in categories)
            {
                Console.WriteLine(item.Title);
            }

            foreach (var item in courses)
            {
                Console.WriteLine(item.Title);
            }
        }
    }

    static void SelectIn(SqlConnection connection)
    {
        var query = @"SELECT * FROM [Career] WHERE [Id] IN @Id";

        var items = connection.Query<Career>(query, new
        {
            Id = new[]{
                "01AE8A85-B4E8-4194-A0F1-1C6190AF54CB",
                "E6730D1C-6870-4DF3-AE68-438624E04C72"
                }
        });

        foreach (var item in items)
        {
            Console.WriteLine(item.Title);
        }
    }

    static void Like(SqlConnection connection)
    {
        var search = "api";
        var query = @"SELECT * FROM [Course] WHERE [Title] LIKE @exp";

        var items = connection.Query<Career>(query, new
        {
            exp = $"%{search}%"
        });

        foreach (var item in items)
        {
            Console.WriteLine(item.Title);
        }
    }

    static void Transaction(SqlConnection connection)
    {

        var category = new Category();
        category.Id = Guid.NewGuid();
        category.Title = "Não salvar";
        category.Url = "amazon";
        category.Description = "Categoria destinada a serviços do AWS";
        category.Order = 8;
        category.Summary = "AWS Cloud";
        category.Featured = false;

        var insertSql = @"INSERT INTO
                [Category]
            VALUES(
                @Id,
                @Title,
                @Url,
                @Summary,
                @Order,
                @Description,
                @Featured)";

        connection.Open();
        using (var transaction = connection.BeginTransaction())
        {
            var rows = connection.Execute(insertSql, new
            {
                category.Id,
                category.Title,
                category.Url,
                category.Summary,
                category.Order,
                category.Description,
                category.Featured
            }, transaction);

            // transaction.Commit();
            transaction.Rollback();
            Console.WriteLine($"{rows} linha inserida!");
        }

    }

}
using GenericBlazorWebApi.Tests.Shared.TestModels;
using Microsoft.Data.Sqlite;

namespace GenericBlazorWebApi.Tests.Shared.TestData;

public class TestDataContext : DbContext
{
    public virtual DbSet<TestModel> TestModels => Set<TestModel>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder
            .UseSqlite(SqliteInMemoryDatabase.GetNew().Connection);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TestModel>().HasData(
            new TestModel { Id = 1, Name = "TestModel1", UniqueName = "TestModel1" },
            new TestModel { Id = 2, Name = "TestModel2", UniqueName = "TestModel2" },
            new TestModel { Id = 3, Name = "TestModel3" }
        );
    }
}

public class SqliteInMemoryDatabase
{
    private static readonly object Key = new();

    private SqliteInMemoryDatabase()
    {
        lock (Key)
        {
            Connection = new SqliteConnection("Filename=:memory:");
            Connection.Open();
        }
    }

    public SqliteConnection Connection { get; }

    public static SqliteInMemoryDatabase GetNew()
    {
        return new SqliteInMemoryDatabase();
    }
}
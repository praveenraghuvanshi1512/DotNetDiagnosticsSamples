﻿using Dapper;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using TargetApp.Models;

namespace TargetApp
{
    public class CategoryRepository
    {
        private IDbConnection DbConnection => new SqlConnection(_dbOptions.ConnectionString);

        private readonly ILogger<CategoryRepository> _logger;
        private readonly ProductDatabaseSettings _dbOptions;
        private readonly IEnumerable<ProductCategory> _allCatergories;

        public CategoryRepository(IOptions<ProductDatabaseSettings> dbOptions, ILogger<CategoryRepository> logger)
        {
            _dbOptions = dbOptions?.Value ?? throw new ArgumentNullException(nameof(dbOptions)); ;
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

            _allCatergories = new List<ProductCategory>
            {
                new ProductCategory(1, "Sony", 1),
                new ProductCategory(2, "Apple", 1),
                new ProductCategory(3, "Samsung", 2)
            };
        }

        public IEnumerable<ProductCategory> GetAllCategories()
        {
            // using var db = DbConnection;

            // Because the DB query is quick, add a Task.Delay to simulate other slower tasks
            Task.Delay(100).Wait();

            return _allCatergories; // db.Query<ProductCategory>(@"SELECT ProductCategoryID Id, Name, ParentProductCategoryID ParentId FROM SalesLT.ProductCategory");
        }

        public async Task<IEnumerable<ProductCategory>> GetAllCategoriesAsync()
        {
            // using var db = DbConnection;

            // Because the DB query is quick, add a Task.Delay to simulate other slower tasks
            await Task.Delay(100);

            return await Task.FromResult<IEnumerable<ProductCategory>>(_allCatergories); // db.QueryAsync<ProductCategory>("SELECT ProductCategoryID Id, Name, ParentProductCategoryID ParentId FROM SalesLT.ProductCategory");
        }

        public ProductCategory GetCategory(int id)
        {
            using var db = DbConnection;
            return db.QueryFirstOrDefault<ProductCategory>("SELECT ProductCategoryID Id, Name, ParentProductCategoryID ParentId FROM SalesLT.ProductCategory WHERE ProductCategoryID=@id", new { id });
        }

        public async Task<ProductCategory> GetCategoryAsync(int id)
        {
            using var db = DbConnection;
            return await db.QueryFirstOrDefaultAsync<ProductCategory>("SELECT ProductCategoryID Id, Name, ParentProductCategoryID ParentId FROM SalesLT.ProductCategory WHERE ProductCategoryID=@id", new { id });
        }
    }
}

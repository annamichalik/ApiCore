using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace ApiCoffeeMug.Controllers
{
    [ApiController]
    [Route("[controller]")]
    // based on https://docs.microsoft.com/en-us/aspnet/core/tutorials/first-web-api?view=aspnetcore-3.0&tabs=visual-studio
    public class ProductController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<ProductController> _logger;
        private readonly IConfiguration _config;

        public ProductController(ILogger<ProductController> logger, IConfiguration config)
        {
            _logger = logger;
            _config = config;
        }

        [HttpGet]
        public IEnumerable<Product> Get()
        {
            var rng = new Random();
            return Enumerable.Range(1, 3).Select(index => new Product
            {
                Id = Guid.NewGuid(),
                Price = rng.Next(2, 55),
                Name = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }

        [HttpGet]
        [Route("List")]
        public IEnumerable<Product> List()
        {
      
            return ReadProductList(); 

        }
        [HttpGet]
        [Route("GetProductById")]
        //calling by /product/GetProductById?id=9150f2c7-d8e6-43fc-bd4b-1e0255f3bb31
        public Product GetProductById(Guid id)
        {
            var list = ReadProductList();
            return list.FirstOrDefault<Product>(x => x.Id == id);

        }

        [HttpPost]
        [Route("AddProduct")]
        public Guid AddProduct(Product todoItem)
        {
            var list = ReadProductList();
            var id = Guid.NewGuid();
            list.Add(todoItem);
            SaveProdcutList(list);
            return id;
        }

        [HttpPut("{id}")]
        [Route("UpdateProduct")]
        public string UpdateProduct(Guid id, Product todoItem)
        {
            var list = ReadProductList();
            var toUpdate = list.FirstOrDefault<Product>(x => x.Id == id);
            if (toUpdate != null)
            {
                toUpdate.Id = todoItem.Id;
                toUpdate.Name = todoItem.Name;
                toUpdate.Price = todoItem.Price;
                SaveProdcutList(list);
                return "done";
            }

            return "no such a product";
        }
        [HttpDelete]
        public string RemoveProduct(Guid id)
        {
            var list = ReadProductList();
            var toRemove = list.FirstOrDefault<Product>(x => x.Id == id);
            if (toRemove != null)
            {
                list.Remove(toRemove);
                SaveProdcutList(list);
                return "done";
            }
        
            return "no such a product";
        }
        private void SaveProdcutList(IList<Product> products)
        {
            var baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            var fileName = _config.GetValue<string>("FileName", "productsList.json");
            var jsonPath = Path.Combine(baseDirectory, @"App_Data\", fileName);


            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            };

            var json = JsonSerializer.Serialize(products, options);

            System.IO.File.WriteAllText(jsonPath, json);

        }
        private IList<Product> ReadProductList()
        {
            var baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            var fileName = _config.GetValue<string>("FileName", "productsList.json");
            var jsonPath = Path.Combine(baseDirectory, @"App_Data\", fileName);


            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            };
            var jsonString = System.IO.File.ReadAllText(jsonPath);
            var jsonModel = JsonSerializer.Deserialize<List<Product>>(jsonString, options);
            return jsonModel;
        }
   
    }
}

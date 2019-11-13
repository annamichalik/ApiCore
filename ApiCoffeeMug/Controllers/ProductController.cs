using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
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

        public ProductController(ILogger<ProductController> logger)
        {
            _logger = logger;
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
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new Product
            {
                Id = Guid.NewGuid(),
                Price = rng.Next(2, 55),
                Name = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }

        [HttpGet]
        [Route("GetProductById")]
        //calling by /product/GetProductById?id=062d57c9-6baf-4e9f-a09c-cc865cb7c1c2
        public Product GetProductById(Guid id)
        {
            return new Product
            {
                Id = id,
                Price = 500,
                Name = "onlySelected"
            };
        }
    }
}

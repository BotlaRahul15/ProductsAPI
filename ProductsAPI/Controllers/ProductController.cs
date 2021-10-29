using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using ProductsAPI.Models;
using ProductsAPI.Repository;
using ProductsAPI.Services;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ProductsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class ProductController : ControllerBase
    {
        private readonly IJwtAuth jwtAuth;
        private ApplicationDbContext _applicationDbContext;
        ProductService productService = new ProductService();
        public ProductController(IJwtAuth jwtAuth, ApplicationDbContext applicationDbContext)
        {
            this.jwtAuth = jwtAuth;
            _applicationDbContext = applicationDbContext;
        }

        [HttpGet]
        [Route("Get")]
        public IActionResult Get()
        {
            List<Product> productsList = _applicationDbContext.Product.ToList();
            if (productsList != null )
            {
                return Ok(productsList);
            }
            else
            {
                return NotFound("No products exists");
            }
        }

        [HttpGet]
        [Route("Get1")]
        public IActionResult Get1()
        {
            List<Product> productsList = productService.Get();
            //productsList = null;
            if (productsList.Count > 0)
            {
                return Ok(productsList);
            }
            else
            {
                return NotFound("No products exists");
            }
        }

        [HttpGet]
        [Route("GetBy")]
        public IActionResult GetBy(int id)
        {
            //Product product = productService.GetBy(id);
            Product product = _applicationDbContext.Product.Find(id);
            if (product != null)
            {
                return Ok(product);
            }
            else
            {
                return NotFound("No product exist with id " + id);
            }
        }

        [HttpPost]
        [Route("Create")]
        public IActionResult Create([FromBody] Product product)
        {
            if (!string.IsNullOrEmpty(product.ProductName))
            {
                _applicationDbContext.Add(product);
                _applicationDbContext.SaveChanges();
                //List<Product> productsList = productService.Create(product);
                List<Product> productsList = _applicationDbContext.Product.ToList();
                return Ok(productsList);
            }
            else
            {
                return BadRequest();
            }
           
        }

        // PUT api/<ProductController>/5
        [HttpPut]
        [Route("Update")]
        public IActionResult Update([FromBody] Product product)
        {
            //List<Product> productsList = productService.Update(product);
            Product product1 = _applicationDbContext.Product.Find(product.ProductID);
            if (product1 != null)
            {
                product1.ProductName = product.ProductName;
                product1.ProductFeatures = product.ProductFeatures;
                product1.ProductPrice = product.ProductPrice;
                _applicationDbContext.Update(product1);
                _applicationDbContext.SaveChanges();
                List<Product> productsList = _applicationDbContext.Product.ToList();
                return Ok(productsList);
            }
            else
            {
                return NotFound("Product not found with id " + product.ProductID);
            }
        }

        [HttpPatch]
        [Route("UpdatePatch")]
        public IActionResult UpdatePatch(int id, [FromBody] JsonPatchDocument<Product> product)
        {
            List<Product> productsList = productService.Get();
            Product result = productService.UpdateByPatch(id, product);
            if (result == null)
            {
                return NotFound("Product not found with id " + id);
            }
            else
            {
                return Ok(result);
            }
        }

        // DELETE api/<ProductController>/5
        [HttpDelete]
        [Route("Delete")]
        public IActionResult Delete(int id)
        {
            //List<Product> productsList = productService.Delete(id);
            Product product = _applicationDbContext.Product.Find(id);
            if (product == null)
            {
                return NotFound("Product not found with id " + id);
            }
            else
            {
                //return Ok(productsList);
                _applicationDbContext.Remove(product);
                _applicationDbContext.SaveChanges();
                List<Product> productsList = _applicationDbContext.Product.ToList();
                return Ok(new { Message = "Deleted" , ResultsAfterDeletion = productsList});
            }
        }

        [HttpGet]
        [Route("GetActiveOrDeactiveProducts")]
        public IActionResult GetActiveOrDeactiveProducts(bool status)
        {
            List<Product> productsList = productService.GetActiveOrDeactiveProducts(status);
            if (productsList == null)
            {
                return NotFound("Product not found with status " + status);
            }
            else
            {
                return Ok(productsList);
            }
        }

        //[AllowAnonymous]
        //// POST api/<MembersController>
        //[HttpPost("authentication")]
        //public IActionResult Authentication(string userName, string passWord)
        //{
        //    var token = jwtAuth.Authentication(userName, passWord);
        //    if (token == null)
        //        return Unauthorized();
        //    return Ok(token);
        //}

        [AllowAnonymous]
        // POST api/<MembersController>
        [HttpPost("authentication")]
        public IActionResult Authentication(LoginModel loginModel)
        {
            bool isValid = productService.IsValidUserInformation(loginModel);
            if (isValid)
            {
                var token = jwtAuth.Authentication(loginModel.UserName, loginModel.Password);
                if (token == null)
                    return Unauthorized();
                return Ok(token);
            }
            else
            {
                return BadRequest();
            }
        }

    }
}

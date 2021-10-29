using Microsoft.AspNetCore.JsonPatch;
using ProductsAPI.Models;
using System.Collections.Generic;

namespace ProductsAPI.IServices
{
    public interface IProductService
    {
        List<Product> Get();
        Product GetBy(int id);
        List<Product> Create(Product product);
        List<Product> Update(Product product);
        List<Product> Delete(int id);
        Product UpdateByPatch(int id, JsonPatchDocument<Product> product);
        bool IsValidUserInformation(LoginModel model);
    }
}

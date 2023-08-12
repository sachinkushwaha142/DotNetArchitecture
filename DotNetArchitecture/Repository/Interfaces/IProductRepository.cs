using DotNetArchitecture.Models.Request;
using Req = DotNetArchitecture.Models.Request;
using Res = DotNetArchitecture.Models.Response;


namespace DotNetArchitecture.Repository.Interfaces
{
    public interface IProductRepository
    {
        public Task<bool> Create(CreateProduct createProduct);
        public Task<bool> Edit(EditProduct editProduct);
        public Task<Models.Response.Product?> GetById(ProductById product);
        public Task<bool> Delete(ProductById product);
    }
}

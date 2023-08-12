using DM = DotNetArchitecture.Models.DBModel;
using Req = DotNetArchitecture.Models.Request;
using Res = DotNetArchitecture.Models.Response;
using Microsoft.EntityFrameworkCore;
using DotNetArchitecture.Repository.Interfaces;
using DotNetArchitecture.Models.DBModel;
using DotNetArchitecture.Models.Request;
using DotNetArchitecture.Repository.Base;
using DotNetArchitecture.Models.Response;


namespace DotNetArchitecture.Repository.Services
{

    public class ProductRepository : IProductRepository
    {
        private readonly IUnitOfWork _unitOfWork;
        public ProductRepository(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        /// <summary>
        /// create product data
        /// </summary>
        /// <param name="createProduct"></param>
        /// <returns></returns>
        public async Task<bool> Create(CreateProduct createProduct)
        {
            DM.Product product = new()
            {
                Name = createProduct.Name,
                Description = createProduct.Description,
                ImageUrl = createProduct.ImageUrl,
                Price = createProduct.Price,
                Discount = createProduct.Discount,
                Sku = createProduct.Sku,
                Status = createProduct.Status,
                IsDeleted = false,
                CreationDate = DateTimeOffset.Now,
                CreatorUserId = createProduct.UserId,
            };
            int result = await _unitOfWork.Repository<DM.Product>().Insert(product);
            return result > 0;
        }


        /// <summary>
        /// edit product data
        /// </summary>
        /// <param name="editProduct"></param>
        /// <returns></returns>
        public async Task<bool> Edit(EditProduct editProduct)
        {

            var product = await _unitOfWork.Repository<DM.Product>().GetSingle(x => x.Id == editProduct.Id && !x.IsDeleted);
            if (product != null)
            {
                product.Name = editProduct.Name;
                product.Description = editProduct.Description;
                product.ImageUrl = editProduct.ImageUrl;
                product.Price = editProduct.Price;
                product.Discount = editProduct.Discount;
                product.Sku = editProduct.Sku;
                product.Status = editProduct.Status;
                product.LastModifierUserId = editProduct.UserId;
                product.LastModifyDate = DateTimeOffset.Now;
                int result = await _unitOfWork.Repository<DM.Product>().Update(product);
                return result > 0;
            }
            return false;
        }


        // <summary>
        // Get requested product data by id
        // </summary>
        // <param name="id"></param>
        // <returns></returns>
        public async Task<Models.Response.Product?> GetById(ProductById product)
        {
            var productData = await _unitOfWork.Repository<DM.Product>().GetSingle(x => x.Id == product.Id && !x.IsDeleted);
            if (productData != null)
            {
                Models.Response.Product result = new()
                {
                    Id = product.Id,
                    Name = productData.Name,
                    Description = productData.Description,
                    ImageUrl = productData.ImageUrl,
                    Price = productData.Price,
                    Discount = productData.Discount,
                    Sku = productData.Sku,
                    Status = productData.Status
                };
                return result;
            }
            return null;
        }


        /// <summary>
        /// Deleted requested product by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> Delete(ProductById product)
        {
            var productdata = await _unitOfWork.Repository<DM.Product>().GetSingle(x => x.Id == product.Id && !x.IsDeleted);
            if (productdata != null)
            {
                productdata.IsDeleted = true;
                productdata.DeleterUserId = product.UserId;
                productdata.DeletionDate = DateTimeOffset.Now;
                int result = await _unitOfWork.Repository<DM.Product>().Update(productdata);
                return result > 0;
            }
            return false;
        }

    }



}

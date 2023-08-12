using DotNetArchitecture.Filters;
using DotNetArchitecture.Models.Request;
using DotNetArchitecture.Models.Response;
using DotNetArchitecture.Repository.Interfaces;
using DotNetArchitecture.Validator;
using DotNetArchitecture.Validator.Interfaces;
using DotNetArchitecture.Models.DBModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Req = DotNetArchitecture.Models.Request;
using Res = DotNetArchitecture.Models.Response;

namespace DotNetArchitecture.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class ProductController : ControllerBase
    {
        private readonly IProductRepository _productRepository;
        private readonly ILogger<ProductController> _logger;
        private readonly Guid _userId;
        private readonly IProductValidation _validation;
        public ProductController(IProductRepository productRepository, ILogger<ProductController> logger, IProductValidation validation)
        {
            _productRepository = productRepository;
            _logger = logger;
            _validation = validation;
            _userId = Guid.Parse("3fa85f64-5717-4562-b3fc-2c963f66afa6");
        }


        /// <summary>
        /// create product
        /// </summary>
        /// <param name="createProduct"></param>
        /// <returns></returns>
        [Route("create")]
        [HttpPost]
        public async Task<IActionResult> Create(CreateProduct createProduct)
        {
            ErrorResponse? errorResponse;
            try
            {
                #region Validate Request Model
                var validation = await _validation.CreateProductValidator.ValidateAsync(createProduct);
                errorResponse = CustomResponseValidator.CheckModelValidation(validation);
                if (errorResponse != null)
                {
                    return BadRequest(errorResponse);
                }
                #endregion

                createProduct.UserId = _userId;
                var result = await _productRepository.Create(createProduct);

                if (result)
                {
                    _logger.LogInformation("Product created successfully!");
                    return Ok(ResponseResult<DBNull>.Success("Product created successfully!"));
                }
                else
                {
                    _logger.LogCritical("Product not created!");
                    return Ok(ResponseResult<DBNull>.Success("Product not created!"));
                }
            }
            catch (DbUpdateException exp)
            {
                var ex = exp.InnerException as SqlException;
                errorResponse = ex != null ? new() { ErrorCode = Convert.ToInt32(ex.ErrorCode), Message = ex.Message } : new();
                _logger.LogError("LoggingAt:{date} RequestIdentifier:{api} Exception:{ex}", DateTime.Now, "create", ex);
                return BadRequest(errorResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError("LoggingAt:{date} RequestIdentifier:{api} Exception:{ex}", DateTime.Now, "create", ex);
                return Ok(ResponseResult<DBNull>.Failure("Product not created!"));
            }
        }

        /// <summary>
        /// edit product
        /// </summary>
        /// <param name="editProduct"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("edit")]
        public async Task<IActionResult> Edit(EditProduct editProduct)
        {

            ErrorResponse? errorResponse;
            try
            {
                #region Validate Request Model
                var validation = await _validation.EditProductValidator.ValidateAsync(editProduct);
                errorResponse = CustomResponseValidator.CheckModelValidation(validation);
                if (errorResponse != null)
                {
                    return BadRequest(errorResponse);
                }
                #endregion
                editProduct.UserId = _userId;
                var result = await _productRepository.Edit(editProduct);
                if (result)
                {
                    _logger.LogInformation("Product edited successfully!");
                    return Ok(ResponseResult<DBNull>.Success("Product edited successfully!"));
                }
                else
                {
                    _logger.LogCritical("Product not edited!");
                    return Ok(ResponseResult<DBNull>.Success("Product not edited!"));
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("LoggingAt:{date} RequestIdentifier:{api} Exception:{ex}", DateTime.Now, "edit", ex);
                return Ok(ResponseResult<DBNull>.Failure("Product not edited!"));
            }

        }

        /// <summary>
        /// get product details by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("getbyid")]
        [HttpPost]
        public async Task<IActionResult> GetById(ProductById product)
        {

            ErrorResponse? errorResponse;
            try
            {
                #region Validate Request Model
                var validation = await _validation.ProductValidator.ValidateAsync(product);
                errorResponse = CustomResponseValidator.CheckModelValidation(validation);
                if (errorResponse != null)
                {
                    return BadRequest(errorResponse);
                }
                #endregion
                var result = await _productRepository.GetById(product);

                if (result != null)
                {
                    _logger.LogInformation("Record get successfully");
                    return Ok(ResponseResult<Res.Product>.Success("Product get successfully", result));
                }
                else
                {
                    _logger.LogInformation("Record not found!");
                    return Ok(ResponseResult<DBNull>.Success("Product not found!"));
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("LoggingAt:{date} RequestIdentifier:{api} Exception:{ex}", DateTime.Now, "getbyid", ex);
                return Ok(ResponseResult<DBNull>.Failure("Product not found!"));
            }
        }

        /// <summary>
        /// delete product
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("delete")]
        public async Task<IActionResult> Delete(ProductById product)
        {
            ErrorResponse? errorResponse;
            try
            {
                #region Validate Request Model
                var validation = await _validation.DeleteValidator.ValidateAsync(product);
                errorResponse = CustomResponseValidator.CheckModelValidation(validation);
                if (errorResponse != null)
                {
                    return BadRequest(errorResponse);
                }
                #endregion
                product.UserId = _userId;
                var result = await _productRepository.Delete(product);
                if (result)
                {
                    _logger.LogInformation("Product deleted successfully!");
                    return Ok(ResponseResult<DBNull>.Success("Product deleted successfully!"));
                }
                else
                {
                    _logger.LogCritical("Product not deleted!");
                    return Ok(ResponseResult<DBNull>.Success("Product not deleted!"));
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("LoggingAt:{date} RequestIdentifier:{api} Exception:{ex}", DateTime.Now, "delete", ex);
                return Ok(ResponseResult<DBNull>.Failure("Product not Found!"));
            }

        }




    }
}

using Company_Project.Model;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Routing.Constraints;
using Company_Project.DTO;
using Company_Project.DTO.ReceversDTOs;
using Company_Project.ActionFilters;

namespace Company_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly AppDbContext context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductsController(AppDbContext _context, IWebHostEnvironment webHostEnvironment)
        {
            context = _context;
            _webHostEnvironment= webHostEnvironment;
        }

        ///////////////////////////////////////////////////////////
        [HttpGet]
        public async Task<IActionResult> GetAllProducts()
        {
            if (await context.Products.CountAsync() > 0)
            {
                var products = await context.Products
                    .Select(p => new ProductsDTO
                    {
                        id = p.id,
                        Name = p.Name,
                        Details = p.Details,
                        Department_no = p.Department_no,
                        Imagesrc = p.Imagesrc
                    })
                    .ToListAsync();

                return Ok(products);
            }
            else
            {
                return NotFound("There is no products yet");
            }
        }

        /// ////////////////////////////////////////////////////////
        [HttpGet]
        [Route("WithoutSrc")]
        public async Task<IActionResult> AllProductsWithoutImageSrc()
        {
            if (await context.Products.AnyAsync())
            {
                var products = await context.Products
                    .Select(p => new ProductsWithoutSrcDTO
                    {
                        id = p.id,
                        Name = p.Name,
                        Details = p.Details,
                        Department_no = p.Department_no
                    })
                    .ToListAsync();

                return Ok(products);
            }
            else
            {
                return NotFound("There is no products yet");
            }
        }
        ///////////////////////////////////////////////////////////
        [HttpGet("{id:int}")]
        public IActionResult getProductById(int id = -1)
        {
            try
            {
                if (id < 0)
                {
                    return BadRequest("There is no id like that");
                }
                if (!context.Products.Any(x => x.id == id))
                {
                    return NotFound();
                }
                else
                {
                    Products Pr = context.Products.FirstOrDefault(x => x.id == id);
                    ProductsDTO products = new ProductsDTO();
                    products.id = id;
                    products.Name = Pr.Name;
                    products.Details = Pr.Details;
                    products.Imagesrc = Pr.Imagesrc;
                    products.Department_no = Pr.Department_no;

                    return Ok(products);
                }
            }
            catch (Exception)
            {
                return NotFound("No Product with that id");
            }

        }
        ///////////////////////////////////////////////////////////
        [CheckHeaderSecurityValueFilter]
        [HttpPost]
        public async Task<IActionResult> addNewProduct([FromForm]ProductsWithoutIdDTO product1)
        {

            Products product = new Products();
            product.Name = product1.Name;
            product.Details = product1.Details;
            product.Department_no = product1.Department_no;

            if (product == null)
            {
                return BadRequest("No product submitted");
            }

            if (context.Products.Any(p => p.Name == product.Name))
            {
                return BadRequest("This Name already in use");
            }
            if(product1.client_file != null)
            {
                MemoryStream memoryStream = new MemoryStream();
                product1.client_file.CopyTo(memoryStream);
                product.Image = memoryStream.ToArray();
            }
            
            try
            {
                context.Products.Add(product);
                await context.SaveChangesAsync();
                product1.Imagesrc = product.Imagesrc;
                return Ok(product1);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        //public IActionResult addNewProduct(Products Pr)
        //{

        //    if (Pr == null)
        //    {
        //        return BadRequest("No product submitted");
        //    }
        //    if (context.Products.Any(x => x.Name == Pr.Name))
        //    {
        //        return BadRequest("This Name already in use");
        //    }
        //    try
        //    {
        //        context.Products.Add(Pr);
        //        context.SaveChanges();
        //        return Ok(Pr);
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //}
        ///////////////////////////////////////////////////////////
        [CheckHeaderSecurityValueFilter]
        [HttpPut("{id:int}")]
        public IActionResult UpdateProductDetails(int id,[FromForm]ProductPostDTO PU)
        {
            ProductsWithoutIdDTO products = new ProductsWithoutIdDTO
            {
                Name = PU.Name,
                Details = PU.Details,
                client_file = PU.client_file,
                Department_no= PU.Department_no
            };
            if (!context.Products.Any(x => x.id == id))
            {
                return NotFound("There is no Product id like that in DB");
            }
            if (products == null)
            {
                return BadRequest("No Product Submitted");
            }

            if (context.Products.Any(x =>x.id == id))
            {
                try
                {
                    Products Pr = new Products()
                    {
                        Name = products.Name,
                        Details = products.Details,
                        Department_no = products.Department_no,
                    };
                    if (products.client_file != null)
                    {
                        MemoryStream memoryStream = new MemoryStream();
                        products.client_file.CopyTo(memoryStream);
                        Pr.Image = memoryStream.ToArray();
                    }
                    Pr.id = id;
                    context.Products.Update(Pr);
                    context.SaveChanges();
                    return Ok("Product Updated Successfully !!");
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
            else
            {
                return NotFound();
            }
        }
        ///////////////////////////////////////////////////////////
        [CheckHeaderSecurityValueFilter]
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteProductById(int id)
        {
            if (context.Products.Any(x => x.id == id))
            {
                try
                {
                    var Pr = await context.Products.FindAsync(id);
                    context.Products.Remove(Pr);
                    await context.SaveChangesAsync();
                    return Ok("Product Deleted Successfully !!");
                }
                catch (Exception ex)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred while deleting the Product: {ex.Message}");
                }
            }
            return NotFound("there is no id like that in Products database table");
        }
        ///////////////////////////////////////////////////////////
        
    }
}

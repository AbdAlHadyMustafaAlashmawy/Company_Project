using Company_Project.ActionFilters;
using Company_Project.DTO;
using Company_Project.DTO.ReceversDTOs;
using Company_Project.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Company_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        private readonly AppDbContext context;

        public DepartmentController(AppDbContext _context) 
        {
            context = _context;
        }
        ///////////////////////////////////////////////////////////
        [HttpGet("WithoutImages")]
        public async Task<IActionResult> GetAllDepartmentsWithoutImages()
        {
            if (context.Departments.Any())
            {
                var DepartmentLS = await context.Departments.ToListAsync();
                List<DepartmentsWithoutSrcDTO> departments = new List<DepartmentsWithoutSrcDTO>();
                foreach (var item in DepartmentLS)
                {
                    departments.Add(new DepartmentsWithoutSrcDTO()
                    {
                        id = item.id,
                        Name = item.Name,
                        Describtion = item.Describtion,
                        Company_no = item.Company_no,
                    });
                }
                return Ok(departments);
            }
            else
            {
                return NotFound("There is no Departments yet");
            }
        }
        ///////////////////////////////////////////////////////////
        [HttpGet]
        public async Task<IActionResult> GetAllDepartments()
        {
            if (context.Departments.Any())
            {
                var DepartmentLS = await context.Departments.ToListAsync();
                List<DepartmentsDTO> departments = new List<DepartmentsDTO>();
                foreach (var item in DepartmentLS)
                {
                    departments.Add(new DepartmentsDTO()
                    {
                        id = item.id,
                        Name = item.Name,
                        Describtion = item.Describtion,
                        Company_no = item.Company_no,
                        Imagesrc = item.Imagesrc,
                    });
                }
                return Ok(departments);

            }
            else
            {
                return NotFound("There is no Departments yet");
            }
        }
        ///////////////////////////////////////////////////////////
        [HttpGet("{id:int}")]
        public IActionResult getDepartmentById(int id = -1)
        {
            try
            {
                if (id < 0)
                {
                    return BadRequest("There is no id like that");
                }
                if (!context.Departments.Any(x => x.id == id))
                {
                    return NotFound();
                }
                else
                {
                    var D = context.Departments.FirstOrDefault(x => x.id == id);
                    DepartmentsDTO D_DTO = new DepartmentsDTO();
                    D_DTO.id = id;
                    D_DTO.Name = D.Name;
                    D_DTO.Image = D.Image;
                    D_DTO.Imagesrc=D.Imagesrc;
                    D_DTO.Company_no= D.Company_no;
                    return Ok(D_DTO);
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
        public IActionResult addNewDepartment([FromForm]DepartmentPostDTO DP)
        {
            DepartmentWithoutIdDTO D = new DepartmentWithoutIdDTO
            {
                Name= DP.Name,
                Describtion= DP.Describtion,
                Company_no=DP.Company_no,
                client_file=DP.client_file,
            };
            if (D == null)
            {
                return BadRequest("No product submitted");
            }
            if (context.Departments.Any(x => x.Name == D.Name))
            {
                return BadRequest("This Name already in use");
            }
            try
            {
                var department = new Departments
                {
                    Name = D.Name,
                    Image = D.Image,
                    Company_no = D.Company_no,
                    Describtion = D.Describtion,
                    
                };
                if (!context.Companies.Any(c => c.id == D.Company_no))
                {
                    return BadRequest("Invalid company specified");
                }
                if (D.client_file != null)
                {
                    MemoryStream memoryStream = new MemoryStream();
                    D.client_file.CopyTo(memoryStream);
                    department.Image = memoryStream.ToArray();
                }
                context.Departments.Add(department);
                D.Imagesrc = department.Imagesrc;
                context.SaveChanges();
                
                return Ok(D);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        ///////////////////////////////////////////////////////////
        [CheckHeaderSecurityValueFilter]
        [HttpPut("{id_S:int}")]
        public IActionResult UpdateDepartmentDetails(int id_S, [FromForm]DepartmentPostDTO DP)
        {
            DepartmentsDTO D = new DepartmentsDTO
            {
                id=id_S,
                Describtion=DP.Describtion,
                client_file=DP.client_file,
                Company_no= DP.Company_no,
                Name=DP.Name,
            };
            if (!context.Departments.Any(x => x.id == id_S))
            {
                return NotFound("There is no Department id like that in DB");
            }
            if (D == null)
            {
                return BadRequest("No Department Submitted");
            }
            if (context.Departments.Any(x => x.id == id_S))
            {
                try
                {
                    var department = new Departments
                    {
                        id = id_S,
                        Name = D.Name,
                        Image = D.Image,
                        Describtion = D.Describtion,
                        Company_no = D.Company_no

                    };
                    if (D.client_file != null)
                    {
                        MemoryStream memoryStream = new MemoryStream();
                        D.client_file.CopyTo(memoryStream);
                        department.Image = memoryStream.ToArray();
                    }
                    context.Departments.Update(department);
                    context.SaveChanges();
                    return Ok("Department Updated Successfully !!");
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
        public async Task<IActionResult> DeleteDepartmentById(int id)
        {
            if (context.Departments.Any(x => x.id == id))
            {
                try
                {
                    var D = await context.Departments.FindAsync(id);
                    context.Departments.Remove(D);
                    await context.SaveChangesAsync();
                    return Ok("Department Deleted Successfully !!");
                }
                catch (Exception ex)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred while deleting the Department: {ex.Message}");
                }
            }
            return NotFound("there is no id like that in Department database table");
        }
        ///////////////////////////////////////////////////////////
    }
}

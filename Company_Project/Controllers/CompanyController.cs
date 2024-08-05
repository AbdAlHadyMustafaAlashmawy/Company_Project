using Azure;
using Company_Project.ActionFilters;
using Company_Project.DTO;
using Company_Project.DTO.ReceversDTOs;
using Company_Project.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using XAct.Messages;
using XAct.Security;

namespace Company_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles ="Admin")]
    public class CompanyController : ControllerBase
    {
        private readonly AppDbContext context;
        public CompanyController(AppDbContext _context) 
        {
            context=_context;
        }
        [HttpGet("NotAgentsSearchByNamePagenation")]
        public async Task<IActionResult> NotAgentsSearchByNamePagenation(string name, int page = 1, int pageSize = 12)
        {
            if (name == "" || name is null)
            {
                return BadRequest("There is no name added !!");
            }
            var totalRecords = await context.Companies.Where(x => x.IsAgent == false&&x.Name.Contains(name)).CountAsync(c => c.IsAgent == false);
            if (totalRecords == 0)
            {
                return NotFound("There are no non-agent companies yet"); 
            }

            if (page < 1 || pageSize <= 0)
            {
                return BadRequest("Invalid page or page size");
            }

            int totalPages = (int)Math.Ceiling((double)totalRecords / pageSize);

            if (page > totalPages)
            {
                return NotFound(); 
            }

            var companiesQuery = context.Companies
              .Where(c => c.IsAgent == false&&c.Name.Contains(name))
              .Select(c => new CompaniesDTO
              {
                  id = c.id,
                  Name = c.Name,
                  Image = c.Image,
                  Describtion = c.Describtion,
                  IsAgent = c.IsAgent,
                  Imagesrc = c.Imagesrc
              });

            var companies = await companiesQuery.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

            var response = new PagedResponse<CompaniesDTO>(companies, totalRecords, page, pageSize);

            return Ok(response);
        }

        [HttpGet("AgentsSearchByNamePagenation")]
        public async Task<IActionResult> AgentsSearchByNamePagenation(string name, int page = 1, int pageSize = 12)
        {
            if (name == "" || name is null)
            {
                return BadRequest("There is no name added !!");
            }
            var totalRecords = await context.Companies.Where(x => x.Name.Contains(name)&&x.IsAgent==true).CountAsync();
            if (totalRecords == 0)
            {
                return NotFound();
            }
            if (page < 1 || pageSize <= 0)
            {
                return BadRequest("Invalid page or page size");
            }
            int totalPages = (int)Math.Ceiling((double)totalRecords / pageSize);

            if (page > totalPages)
            {
                return NotFound();
            }
            var companiesQuery = context.Companies.Where(x=>x.IsAgent==true&&x.Name.Contains(name))
              .Select(c => new CompaniesDTO
              {
                  id = c.id,
                  Name = c.Name,
                  Image = c.Image,
                  Describtion = c.Describtion,
                  IsAgent = c.IsAgent,
                  Imagesrc = c.Imagesrc
              });

            var companies = await companiesQuery.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

            var response = new PagedResponse<CompaniesDTO>(companies, totalRecords, page, pageSize);

            return Ok(response);
        }
        [HttpGet("AgentsPagenation")]
        public async Task<IActionResult> GetAllAgentsPagenation(int page = 1, int pageSize = 12)
        {
            var totalRecords = await context.Companies.CountAsync(c => c.IsAgent == false);
            if (totalRecords == 0)
            {
                return NotFound("There are no non-agent companies yet");
            }

            if (page < 1 || pageSize <= 0)
            {
                return BadRequest("Invalid page or page size");
            }

            int totalPages = (int)Math.Ceiling((double)totalRecords / pageSize);

            if (page > totalPages)
            {
                return NotFound();
            }

            var companiesQuery = context.Companies
              .Where(c => c.IsAgent == false)
              .Select(c => new CompaniesDTO
              {
                  id = c.id,
                  Name = c.Name,
                  Image = c.Image,
                  Describtion = c.Describtion,
                  IsAgent = c.IsAgent,
                  Imagesrc = c.Imagesrc
              });

            var companies = await companiesQuery.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

            var response = new PagedResponse<CompaniesDTO>(companies, totalRecords, page, pageSize);

            return Ok(response);
        }
        [HttpGet("GetAllCompaniesPagenation")]
        public async Task<IActionResult> GetAllCompaniesPagenation(int page = 1, int pageSize = 12)
        {
            var totalRecords = await context.Companies.CountAsync();
            if (totalRecords == 0)
            {
                return NotFound(); 
            }

            if (page < 1 || pageSize <= 0)
            {
                return BadRequest("Invalid page or page size");
            }

            int totalPages = (int)Math.Ceiling((double)totalRecords / pageSize);

            if (page > totalPages)
            {
                return NotFound(); 
            }

            var companiesQuery = context.Companies
              .Select(c => new CompaniesDTO
              {
                  id = c.id,
                  Name = c.Name,
                  Image = c.Image, 
                  Describtion = c.Describtion,
                  IsAgent = c.IsAgent,
                  Imagesrc = c.Imagesrc
              });

            var companies = await companiesQuery.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

            var response = new PagedResponse<CompaniesDTO>(companies, totalRecords, page, pageSize);

            return Ok(response);
        }
        /// /////////////////////////////////////////////////////////////// <summary>
        [HttpGet("CompaniesSearchByNamePagenation")]
        public async Task<IActionResult> CompaniesSearchByNamePagenation(string name,int page = 1, int pageSize = 12)
        {
            if(name == ""||name is null)
            {
                return BadRequest("There is no name added !!");
            }
            var totalRecords = await context.Companies.Where(x=>x.Name.Contains(name)).CountAsync();
            if (totalRecords == 0)
            {
                return NotFound();
            }
            if (page < 1 || pageSize <= 0)
            {
                return BadRequest("Invalid page or page size");
            }
            int totalPages = (int)Math.Ceiling((double)totalRecords / pageSize);

            if (page > totalPages)
            {
                return NotFound();
            }
            var companiesQuery = context.Companies.Where(x=>x.Name.Contains(name))
              .Select(c => new CompaniesDTO
              {
                  id = c.id,
                  Name = c.Name,
                  Image = c.Image,
                  Describtion = c.Describtion,
                  IsAgent = c.IsAgent,
                  Imagesrc = c.Imagesrc
              });

            var companies = await companiesQuery.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

            var response = new PagedResponse<CompaniesDTO>(companies, totalRecords, page, pageSize);

            return Ok(response);
        }
        /// <summary>
        /// //////////////////////////////////////////////////////////////////////////////////////////
        /// </summary>
        /// <returns></returns>
        [HttpGet("CompaniesCount")]
        public IActionResult GetCompaniesCount()
        {
            int CompaniesCount=context.Companies.Count();
            return Ok(CompaniesCount);
        }
        [HttpGet("GetAgentsCount")]
        public IActionResult GetAgentsCount()
        {
            int CompaniesCount = context.Companies.Where(x=>x.IsAgent==true).Count();
            return Ok(CompaniesCount);
        }
        [HttpGet("GetNotAgentsCount")]
        public IActionResult GetNotAgentsCount()
        {
            int CompaniesCount = context.Companies.Where(x => x.IsAgent == false).Count();
            return Ok(CompaniesCount);
        }
        //////////////////////////////////////////////////////////
        [HttpGet("{id:int}")]
        public async Task<IActionResult> getCompanyById(int id = -1)
        {
            try
            {
                if (id < 0)
                {
                    return BadRequest("There is no id like that");
                }
                if (!await context.Companies.AnyAsync(x => x.id == id))
                {
                    return NotFound();
                }
                else
                {
                    var C = await context.Companies.FirstOrDefaultAsync(x => x.id == id);
                    CompaniesDTO c_DTO = new CompaniesDTO();
                    c_DTO.id = id;
                    c_DTO.Name = C.Name;
                    c_DTO.Image = C.Image;
                    c_DTO.Describtion = C.Describtion;
                    c_DTO.Imagesrc = C.Imagesrc;
                    return Ok(c_DTO);
                }
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }

        }

        ///////////////////////////////////////////////////////////
        [HttpGet("DepartmentsPagination/{Company_id:int}")]
        public async Task<IActionResult> GetAllDepartmentsOfSpecificCompanyPagination(int Company_id = -1, int page = 1, int pageSize = 12)
        {
            if (Company_id < 0)
            {
                return BadRequest("There is no id like that");
            }

            if (!await context.Companies.AnyAsync(x => x.id == Company_id))
            {
                return BadRequest("No Company with that ID found!");
            }

            if (page < 1 || pageSize <= 0)
            {
                return BadRequest("Invalid page or page size");
            }

            var totalRecords = await context.Departments.CountAsync(x => x.Company_no == Company_id);

            int totalPages = (int)Math.Ceiling((double)totalRecords / pageSize);

            if (page > totalPages)
            {
                return NotFound();
            }

            var departmentsQuery = context.Departments.Where(x => x.Company_no == Company_id)
              .Skip((page - 1) * pageSize)
              .Take(pageSize);

            var departments = await departmentsQuery.ToListAsync();

            var response = new PagedResponse<Departments>(departments, totalRecords, page, pageSize);

            return Ok(response);
        }

        [HttpGet("Departments{Company_id:int}")]
        public async Task<IActionResult> GetAllDepartmentsOfSpecificCompany(int Company_id = -1)
        {
            if(Company_id < 0)
            {
                return BadRequest("There is no id like that");
            }
            if (!await context.Companies.AnyAsync(x => x.id == Company_id))
            {
                return BadRequest("no Company id like that !!");
            }
            try
            {
                var departments = await context.Departments.Where(x => x.Company_no == Company_id).ToListAsync();
                return Ok(departments);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //////////////////////////////////////////////////////////
        [CheckHeaderSecurityValueFilter]
        [HttpPost]
        public IActionResult addNewCompany([FromForm]CompanyPostDTO CP)
        {
            CompaniesDTO C = new CompaniesDTO();
            C.Name=CP.Name;
            C.client_file = CP.client_file;
            C.Describtion= CP.Describtion;
            C.IsAgent= CP.IsAgent;
            if (C == null)
            {
                return BadRequest("No Compaines submitted");
            }
            if (context.Companies.Any(x => x.Name == C.Name))
            {
                return BadRequest("This Name already in use");
            }
            try
            {
                var companies = new Companies
                {
                    Name = C.Name,
                    Image = C.Image,
                    Describtion = C.Describtion,
                    IsAgent=C.IsAgent,
                };
                if (C.client_file != null)
                {
                    MemoryStream memoryStream = new MemoryStream();
                    C.client_file.CopyTo(memoryStream);
                    companies.Image = memoryStream.ToArray();
                }
                context.Companies.Add(companies);
                context.SaveChanges();
                C.id = companies.id;
                C.Imagesrc = companies.Imagesrc;
                return Ok(C);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        ///////////////////////////////////////////////////////////
        [CheckHeaderSecurityValueFilter]
        [HttpPut("{id_C:int}")]
        public IActionResult UpdateCompanyDetails(int id_C, [FromForm] CompanyPostDTO CP )
        {
            CompaniesDTO D = new CompaniesDTO();
            D.Name = CP.Name;
            D.client_file = CP.client_file;
            D.Describtion = CP.Describtion;
            D.IsAgent = CP.IsAgent;

            if (D == null)
            {
                return BadRequest("No Company Submitted");
            }
            if (!context.Companies.Any(x => x.id == id_C))
            {
                return NotFound("There is no Companies id like that in DB");
            }
            if (context.Companies.Any(x => x.id == id_C))
            {
                try
                {
                    var Company = new Companies
                    {
                        id = id_C,
                        Name = D.Name,
                        Image = D.Image,
                       Describtion=D.Describtion,
                       IsAgent=D.IsAgent

                    };
                    if (CP.client_file != null)
                    {
                        MemoryStream memoryStream = new MemoryStream();
                        CP.client_file.CopyTo(memoryStream);
                        Company.Image = memoryStream.ToArray();
                      }
                    context.Companies.Update(Company);
                    context.SaveChanges();
                    return Ok("Company Updated Successfully !!");
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
        public async Task<IActionResult> DeleteCompanyById(int id)
        {
            if (context.Companies.Any(x => x.id == id))
            {
                try
                {
                    var C = await context.Companies.FindAsync(id);
                    context.Companies.Remove(C);
                    await context.SaveChangesAsync();
                    return Ok("Company Deleted Successfully !!");
                }
                catch (Exception ex)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred while deleting the Company: {ex.Message}");
                }
            }
            return NotFound("there is no id like that in Company database table");
        }
    }
}

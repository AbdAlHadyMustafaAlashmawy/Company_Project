using Company_Project.ActionFilters;
using Company_Project.DTO.ReceversDTOs;
using Company_Project.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Company_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SocialMediaController : ControllerBase
    {
        private readonly AppDbContext context;

        public SocialMediaController(AppDbContext _context) 
        {
            context = _context;
        }
        ///////////////////////////////////////////////////////////
        [HttpGet("{id:int}")]
        public IActionResult getSocialMediaById(int id = -1)
        {
            try
            {
                if (id < 0)
                {
                    return BadRequest("There is no id like that");
                }
                if (!context.SocialMedia.Any(x => x.id == id))
                {
                    return NotFound();
                }
                else
                {
                    var sm = context.SocialMedia.FirstOrDefault(x => x.id == id);
                    return Ok(sm);
                }
            }
            catch (Exception)
            {
                return NotFound();
            }

        }
        ///////////////////////////////////////////////////////////
        [HttpGet]
        public async Task<IActionResult> GetAllSocialLinks()
        {
            if (context.SocialMedia.Any())
            {
                var links =await context.SocialMedia.ToListAsync();
                return Ok(links);
            }
            else
            {
                return NotFound("There is no Links yet");
            }
        }
        ///////////////////////////////////////////////////////////
        [CheckHeaderSecurityValueFilter]
        [HttpPost]
        public IActionResult addNewLink(LinkPostDTO S)
        {
            SocialMedia sm = new SocialMedia()
            {
                Link = S.Link,
                Name= S.Name,
            };
            if (sm == null)
            {
                return BadRequest("No Social Media Link submitted");
            }
            if(context.SocialMedia.Any(x=>x.Link==sm.Link))
            {
                return BadRequest("This Link already in use");
            }
            try
            {
                context.SocialMedia.Add(sm);
                context.SaveChanges();
                return Ok(sm);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [CheckHeaderSecurityValueFilter]
        [HttpPut("{id:int}")]
        public IActionResult UpdateSocialLink(int id, LinkPostDTO S)
        {
            SocialMedia sm = new SocialMedia()
            {
                Name=S.Name,
                Link= S.Link,
            };
            if (!context.SocialMedia.Any(x => x.id == id))
            {
                return NotFound("There is no Social Link id like that in DB");
            }
            if (sm == null)
            {
                return BadRequest("No Social Media Link");
            }
            if (context.SocialMedia.Any(x => x.id ==id))
            {
                try
                {
                    sm.id = id;
                    context.SocialMedia.Update(sm);
                    context.SaveChanges();
                    return Ok("Link Updated Successfully !!");
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
        [CheckHeaderSecurityValueFilter]
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteSocialLinkById(int id)
        {
            if (context.SocialMedia.Any(x => x.id == id))
            {
                try
                {
                    var sm = await context.SocialMedia.FindAsync(id);
                    context.SocialMedia.Remove(sm);
                    await context.SaveChangesAsync();
                    return Ok("Link Deleted Successfully !!");
                }
                catch (Exception ex)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred while deleting the social media link: {ex.Message}");
                }
            }
            return NotFound("there is no id like that in Links database");
        }
    }
}

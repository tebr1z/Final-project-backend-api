using LmsApiApp.Application.Interfaces;
using LmsApiApp.Application.Dtos.GroupDtos;
using LmsApiApp.Core.Entities;
using Microsoft.AspNetCore.Mvc;

namespace LmsApiApp.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GroupController : ControllerBase
    {
        private readonly IGroupService _groupService;

        public GroupController(IGroupService groupService)
        {
            _groupService = groupService;
        }

        // GET: api/Group
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GroupDto>>> GetGroups()
        {
            var groups = await _groupService.GetAllGroupsAsync();
            return Ok(groups);
        }

        // GET: api/Group/5
        [HttpGet("{id}")]
        public async Task<ActionResult<GroupDto>> GetGroup(int id)
        {
            var group = await _groupService.GetGroupByIdAsync(id);

            if (group == null)
            {
                return NotFound();
            }

            return Ok(group);
        }

        // POST: api/Group
        [HttpPost]
        public async Task<ActionResult> CreateGroup([FromBody] GroupDto groupDto)
        {
            await _groupService.AddGroupAsync(groupDto);
            return CreatedAtAction(nameof(GetGroup), new { id = groupDto.Id }, groupDto);
        }

        // PUT: api/Group/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateGroup(int id, [FromBody] GroupDto groupDto)
        {
            if (id != groupDto.Id)
            {
                return BadRequest("Group ID mismatch");
            }

            await _groupService.UpdateGroupAsync(id, groupDto);
            return NoContent();
        }

        // DELETE: api/Group/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGroup(int id)
        {
            await _groupService.DeleteGroupAsync(id);
            return NoContent();
        }

        // SOFT DELETE: api/Group/softdelete/5
        [HttpPut("softdelete/{id}")]
        public async Task<IActionResult> SoftDeleteGroup(int id)
        {
            var group = await _groupService.GetGroupByIdAsync(id);
            if (group == null) return NotFound();

            // Soft delete için grubu "silinmiş" işaretleyin
            group.IsDelete = true;  // Group'a IsDeleted alanı eklenmiş olmalı
            await _groupService.UpdateGroupAsync(id, group);

            return NoContent();
        }
    }
}

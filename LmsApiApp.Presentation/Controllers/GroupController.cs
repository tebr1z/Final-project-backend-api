using AutoMapper;
using LmsApiApp.Application.Interfaces;
using LmsApiApp.Application.Dtos.GroupDtos;
using LmsApiApp.Core.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Query.Internal;

namespace LmsApiApp.Presentation.Controllers
{
   
    [Route("api/[controller]")]
    [ApiController]
    public class GroupController : ControllerBase
    {
        private readonly IGroupService _groupService;
        private readonly IMapper _mapper;

        public GroupController(IGroupService groupService, IMapper mapper)
        {
            _groupService = groupService;
            _mapper = mapper;
        }
        [HttpGet("with-users")]
        public async Task<ActionResult<IEnumerable<GroupWithUsersDto>>> GetGroupsWithUsers()
        {
            var groups = await _groupService.GetAllGroupsWithUsersAsync();
            return Ok(groups);
        }

        // POST: api/Group
        [HttpPost]
        [Authorize(Roles = "Admin")] // Sadece Admin rolü ekleyebilir
        public async Task<ActionResult> CreateGroup([FromBody] GroupDto groupDto)
        {
            // DTO'dan Entity'e mapping
            var groupEntity = _mapper.Map<Group>(groupDto);

            await _groupService.AddGroupAsync(groupEntity);

            return CreatedAtAction(nameof(GetGroup), new { id = groupEntity.Id }, groupEntity);
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

            // Entity'den DTO'ya mapping
            var groups = await _groupService.GetAllGroupsWithUsersAsync();
            return Ok(groups);
           
        }

        [HttpPost("users")]
        // Sadece Admin rolü kullanıcı ekleyebilir
        public async Task<ActionResult> AddUserToGroup([FromBody] GroupEnrollmentDto groupEnrollmentDto)
        {
            var groupEnrollment = new GroupEnrollment
            {
                UserId = groupEnrollmentDto.UserId,
                GroupId = groupEnrollmentDto.GroupId,
                EnrolledDate = DateTime.Now
            };

            await _groupService.AddUserToGroupAsync(groupEnrollment);

            return Ok("Kullanıcı gruba başarıyla eklendi.");
        }

        [HttpPost("{groupId}/users/bulk")]
        public async Task<ActionResult> AddMultipleUsersToGroup(int groupId, [FromBody] GroupEnrollmentBulkDto groupEnrollmentBulkDto)
        {
            foreach (var userId in groupEnrollmentBulkDto.UserIds)
            {
                var groupEnrollment = new GroupEnrollment
                {
                    UserId = userId,
                    GroupId = groupId,
                    EnrolledDate = DateTime.Now
                };

                await _groupService.AddUserToGroupAsync(groupEnrollment);
            }

            return Ok("Kullanıcılar başarıyla gruba eklendi.");
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> EditGroup(int id, [FromBody] GroupEditDto groupEditDto)
        {
            var group = await _groupService.GetGroupByIdAsync(id);
            if (group == null)
            {
                return NotFound("Grup bulunamadı.");
            }

            group.Name = groupEditDto.Name;
             group.UpdatedDate = DateTime.Now;

            await _groupService.UpdateGroupAsync(group);

            return Ok("Grup başarıyla güncellendi.");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGroup(int id)
        {
            var group = await _groupService.GetGroupByIdAsync(id);
            if (group == null)
            {
                return NotFound("Grup bulunamadı.");
            }

            await _groupService.DeleteGroupAsync(id);

            return Ok("Grup başarıyla silindi.");
        }

    }
}

using chatService.Data;
using chatService.Data.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace chatService.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class GroupMembersController : ControllerBase
    {
        private readonly DataContext _dataContext;
        public GroupMembersController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetAsync()
        {
            var groupMembers = await _dataContext.GroupUsers.ToListAsync();
            return Ok(groupMembers);
        }

        [HttpGet]
        [Route("getGroupMemberById")]
        public async Task<IActionResult> GetGroupMemberByIdAsync(int id)
        {
            var groupMember = await _dataContext.GroupUsers.FindAsync(id);
            if (groupMember == null)
            {
                return Ok("GroupMemberID does not exist");
            }
            return Ok(groupMember);
        }

        [HttpPost]
        public async Task<IActionResult> PostAsync(GroupUsers groupMember)
        {
            var singleGroupMember = await _dataContext.GroupUsers.Where(gm => gm.UserId == groupMember.UserId && gm.GroupId == groupMember.GroupId).FirstOrDefaultAsync();
            if (singleGroupMember != null)
            {
                return Ok("This user already exist in this group");
            }

            _dataContext.GroupUsers.Add(groupMember);
            await _dataContext.SaveChangesAsync();
            return Created($"/getGroupMemberById?id={groupMember.GroupUsersId}", groupMember);
        }

        [HttpPut]
        public async Task<IActionResult> PutAsync(GroupUsers groupMemberToUpdate)
        {
            var singleGroupMember = await _dataContext.GroupUsers.FindAsync(groupMemberToUpdate.GroupUsersId);
            if (singleGroupMember == null)
            {
                return Ok("This user does not exist");
            }

            _dataContext.Entry(singleGroupMember).CurrentValues.SetValues(groupMemberToUpdate);
            await _dataContext.SaveChangesAsync();
            return Ok(groupMemberToUpdate);
        }

        [Route("{id}")]
        [HttpDelete]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var groupMemberToDelete = await _dataContext.GroupUsers.FindAsync(id);
            if (groupMemberToDelete == null)
            {
                return NoContent();
            }
            _dataContext.GroupUsers.Remove(groupMemberToDelete);
            await _dataContext.SaveChangesAsync();
            return NoContent();
        }
    }
}

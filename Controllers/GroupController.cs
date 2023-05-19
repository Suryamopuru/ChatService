using chatService.Data;
using chatService.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace chatService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GroupController : ControllerBase
    {
        private readonly DataContext _dataContext;
        public GroupController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        [HttpGet]
        [Route("All")]
        public async Task<IActionResult> GetAsync()
        {
            var groups = await _dataContext.Group.ToListAsync();
            return Ok(groups);
        }

        [HttpGet]
        [Route("{ID}")]
        public async Task<IActionResult> GetGroupByIdAsync(int ID)
        {
            var group = await _dataContext.Group.FindAsync(ID);
            if (group == null)
            {
                return Ok("GroupID no exist");
            }
            return Ok(group);
        }

        [HttpPost]
        [Route("Create")]
        public async Task<IActionResult> PostAsync(Group group)
        {
            var singleGroup = await _dataContext.Group.Where(g => g.GroupName == group.GroupName).FirstOrDefaultAsync();
            if (singleGroup != null)
            {
                return Ok("Group already exist");
            }
            group.CreatedOn = DateTime.Now;
            _dataContext.Group.Add(group);
            await _dataContext.SaveChangesAsync();
            return Created($"/getGroupById?id={group.GroupId}", group);
        }

        [HttpPut]
        [Route("Update")]
        public async Task<IActionResult> PutAsync(Group groupToUpdate)
        {
            var singleGroup = await _dataContext.Group.FindAsync(groupToUpdate.GroupId);
            if (singleGroup == null)
            {
                return Ok("This group does not exist");
            }
            groupToUpdate.ModifiedOn = DateTime.Now;
            _dataContext.Entry(singleGroup).CurrentValues.SetValues(groupToUpdate);
            await _dataContext.SaveChangesAsync();
            return Ok(groupToUpdate);
        }

        [HttpPost]
        [Route("Join")]
        public async Task<IActionResult> PostJoinAsync(GroupUsers groupUserToJoin)
        {
            var singleGroupUser = await _dataContext.GroupUsers.Where(gm => gm.UserId == groupUserToJoin.UserId && gm.GroupId == groupUserToJoin.GroupId).FirstOrDefaultAsync();
            if (singleGroupUser != null)
            {
                return Ok("This user already exist in this group");
            }
            groupUserToJoin.CreatedOn = DateTime.Now;
            _dataContext.GroupUsers.Add(groupUserToJoin);
            await _dataContext.SaveChangesAsync();
            return Ok(groupUserToJoin);
        }

        [HttpDelete]
        [Route("{ID}")]
        public async Task<IActionResult> DeleteAsync(int ID)
        {
            var groupToDelete = await _dataContext.Group.FindAsync(ID);
            if (groupToDelete == null)
            {
                return NoContent();
            }
            _dataContext.Group.Remove(groupToDelete);
            await _dataContext.SaveChangesAsync();
            return NoContent();
        }
    }
}

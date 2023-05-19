using chatService.Data;
using chatService.Data.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace chatService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        private readonly DataContext _dataContext;
        public MessageController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        [HttpGet]
        [Route("All")]
        public async Task<IActionResult> GetAsync()
        {
            var messages = await _dataContext.Messages.ToListAsync();
            return Ok(messages);
        }

        [HttpGet]
        [Route("Group/{ID}")]
        public async Task<IActionResult> GetMessageByGroupIDAsync(int ID)
        {
            var Messages = await _dataContext.Messages.Where(m => m.GroupId == ID).ToListAsync();
            return Ok(Messages);
        }

        [HttpGet]
        [Route("User/{ID}")]
        public async Task<IActionResult> GetMessageByUserIDAsync(int ID)
        {
            var Messages = await _dataContext.Messages.Where(m => m.UserId == ID).ToListAsync();
            return Ok(Messages);
        }

        /*[HttpGet]
        [Route("getGroupMessageById")]
        public async Task<IActionResult> GetGroupMessageByIdAsync(int id)
        {
            var groupMessage = await _dataContext.Messages.FindAsync(id);
            if (groupMessage == null)
            {
                return Ok("GroupMessageId does not exist");
            }
            return Ok(groupMessage);
        }*/

        [HttpPost]
        [Route("Send")]
        public async Task<IActionResult> PostAsync(Messages Message)
        {
            Message.createdOn = DateTime.Now;
            _dataContext.Messages.Add(Message);
            await _dataContext.SaveChangesAsync();
            return Created($"/getGroupMessageById?id={Message.MessageId}", Message);
        }

        [HttpPost]
        [Route("Reaction")]
        public async Task<IActionResult> PostReactionAsync(int id, Boolean reaction)
        {
            using (var dbContext = _dataContext)
            {
                var message = await dbContext.Messages.FindAsync(id);
                if (message == null)
                {
                    return Ok("Message does not exist");
                }

                message.Reactions += reaction ? 1 : -1;
                await dbContext.SaveChangesAsync();
                return Ok("Reaction updated successfully");
            }
        }

        [HttpPut]
        [Route("Update")]
        public async Task<IActionResult> PutUpdateAsync(Messages messageToUpdate)
        {
            var singleGroupMessage = await _dataContext.Messages.FindAsync(messageToUpdate.MessageId);
            if (singleGroupMessage == null)
            {
                return Ok("This message does not exist");
            }
            _dataContext.Entry(singleGroupMessage).CurrentValues.SetValues(messageToUpdate);
            await _dataContext.SaveChangesAsync();
            return Ok(messageToUpdate);
        }

        [Route("{ID}")]
        [HttpDelete]
        public async Task<IActionResult> DeleteAsync(int ID)
        {
            var groupMessageToDelete = await _dataContext.Messages.FindAsync(ID);
            if (groupMessageToDelete == null)
            {
                return NoContent();
            }
            _dataContext.Messages.Remove(groupMessageToDelete);
            await _dataContext.SaveChangesAsync();
            return NoContent();
        }
    }
}

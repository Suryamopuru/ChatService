using chatService.Data;
using chatService.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace chatService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class UserController : ControllerBase
    {
        private readonly DataContext _dataContext;
        public UserController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        [Route("{ID}")]
        [HttpGet]
        /*[Route("getUserById")]*/
        public async Task<IActionResult> GetUserByIdAsync(int ID)
        {
            var user = await _dataContext.User.FindAsync(ID);
            if(user == null)
            {
                return Ok("UserID no exist");
            }
            return Ok(user);
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> GetUserByEmailAsync(string email, string password)
        {
            var user = await _dataContext.User.Where(user => user.Email == email).FirstOrDefaultAsync();
            if (user == null)
            {
                return Ok("User no exist");
            }
            if (password != user.Password)
            {
                return Ok("Password incorrect");
            }
            using (var dbContext = _dataContext)
            {
                user.LastLogin= DateTime.Now;
                user.IsActive = true;
                await dbContext.SaveChangesAsync();
                return Ok(user);
            }

        }

        [HttpPost]
        [Route("Create")]
        public async Task<IActionResult> PostCreateAsync(User user)
        {
            var registeredUser = await _dataContext.User.Where(u => u.Email == user.Email).FirstOrDefaultAsync();
            if (registeredUser != null)
            {
                return Ok("User already exist");
            }
            user.CreatedOn = DateTime.Now;
            _dataContext.User.Add(user);
            await _dataContext.SaveChangesAsync();
            return Created($"/getUserById?id={user.UserId}", user);
        }

        [HttpPut]
        [Route("Update")]
        public async Task<IActionResult> PutUpdateAsync(User userToUpdate)
        {
            var singleUser = await _dataContext.User.FindAsync(userToUpdate.UserId);
            if (singleUser == null)
            {
                return Ok("This user does not exist");
            }

            userToUpdate.ModifiedOn = DateTime.Now;
            _dataContext.Entry(singleUser).CurrentValues.SetValues(userToUpdate);
            await _dataContext.SaveChangesAsync();
            return Ok(userToUpdate);
        }

        [HttpPost]
        [Route("Logout")]
        public async Task<IActionResult> PostLogoutAsync(int ID)
        {
            var singleUser = await _dataContext.User.FindAsync(ID);
            if(singleUser == null)
            {
                return Ok("User does not exist");
            }

            singleUser.IsActive = false;
            _dataContext.Entry(singleUser).CurrentValues.SetValues(singleUser);
            await _dataContext.SaveChangesAsync();
            return Ok(singleUser);
        }

        [Route("{ID}")]
        [HttpDelete]
        public async Task<IActionResult> DeleteAsync(int ID)
        {
            var userToDelete = await _dataContext.User.FindAsync(ID);
            if(userToDelete == null)
            {
                return NoContent();
            }
            _dataContext.User.Remove(userToDelete);
            await _dataContext.SaveChangesAsync();
            return NoContent();
        }


    }


}

using Microsoft.AspNetCore.Mvc;
using ISWBlacklist.API.ISWBlacklist.Repositories;
using ISWBlacklist.Domain.Entities;
using AutoMapper;
using ISWBlacklist.Infrastructure.Context;

namespace ISWBlacklist.API.ISWBlacklist.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly BlackListDbContext dbContext;
        private readonly IUserRepository userRepository;
        private readonly IMapper mapper;
        public UsersController(BlackListDbContext dbContext,IUserRepository userRepository, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.userRepository = userRepository;
            this.mapper = mapper;
        }

        // GET ALL USERS
        [HttpGet]
        //[Authorize(Roles = "ROLE_USER_ADMIN")]
        public async Task<IActionResult> Books()
        {
           var users = await userRepository.GetAllUsers();

           return Ok();
        }

        [HttpGet]
        [Route("{id:Guid}")]
        //[Authorize(Roles = "ROLE_BLACKLIST_ADMIN")]
        public async Task<IActionResult> GetBookById([FromRoute] Guid id)
        {
            var user = await userRepository.GetUserById(id);

            if (user == null)
            {
                return NotFound();
            }

            // Map Book Model to DTO
           return Ok();
        }

        // Update Book By Id
        // PUT: /api/Books/{id}
        //[Authorize(Roles = "ROLE_BLACKLIST_ADMIN")]
        // [HttpPut]
        // [Route("{id:Guid}")]
        // [ValidateModel]
        // public async Task<IActionResult> Update([FromRoute] Guid id, UpdateBookRequestDto updateBookRequestDto)
        // {

        //     // Map DTO to Book Model
        //     var book = mapper.Map<Book>(updateBookRequestDto);

        //     book = await bookRepository.UpdateBookById(id, book);

        //     if (book == null)
        //     {
        //         return NotFound();
        //     }

        //     // Map Domain Model to DTO
        //     return Ok();
        // }

        // Delete Book
        //[Authorize(Roles = "ROLE_USER_ADMIN")]
        [HttpDelete]
        [Route("{id:Guid}")]
        
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var user = await userRepository.DeleteUserById(id);

            if (user == null)
            {
                return NotFound();
            }

            return Ok();
        }
    }
}
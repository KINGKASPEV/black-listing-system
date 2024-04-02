using Microsoft.AspNetCore.Mvc;
using ISWBlacklist.API.ISWBlacklist.Repositories;
using ISWBlacklist.Domain.Entities;
using AutoMapper;
using ISWBlacklist.Infrastructure.Context;

namespace ISWBlacklist.API.ISWBlacklist.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly BlackListDbContext dbContext;
        private readonly IBookRepository bookRepository;
        private readonly IMapper mapper;
        public BooksController(BlackListDbContext dbContext,IBookRepository bookRepository, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.bookRepository = bookRepository;
            this.mapper = mapper;
        }

        // GET ALL BOOKS
        [HttpGet]
        //[Authorize(Roles = "ALL USERS")]
        // Implement filter logic to filter either all blacklisted books or not
        public async Task<IActionResult> Books()
        {
           var books = await bookRepository.GetAllBooks();

           return Ok();
        }

        [HttpGet]
        [Route("{id:Guid}")]
        //[Authorize(Roles = "ROLE_BLACKLIST_ADMIN")]
        public async Task<IActionResult> GetBookById([FromRoute] Guid id)
        {
            var book = await bookRepository.GetBookById(id);

            if (book == null)
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
        //[Authorize(Roles = "ROLE_BLACKLIST_ADMIN")]
        [HttpDelete]
        [Route("{id:Guid}")]
        
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var book = await bookRepository.DeleteBookById(id);

            if (book == null)
            {
                return NotFound();
            }

            return Ok();
        }
    }
}
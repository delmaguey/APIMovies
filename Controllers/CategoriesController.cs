using APIMovies.Models;
using APIMovies.Models.Dtos;
using APIMovies.Repository;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIMovies.Controllers
{
    [Route("api/categories")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryRepository _ctRepo;
        private readonly IMapper _mapper;

        public CategoriesController(ICategoryRepository ctRepo, IMapper mapper)
        {
            _ctRepo = ctRepo;
            _mapper = mapper;    
        }

        [HttpGet]
        public IActionResult GetCategories()
        {
            //var categoryList = _ctRepo.GetCategories();
            //return Ok(categoryList);

            var categoryList = _ctRepo.GetCategories();

            var categoryListDto = new List<CategoryDto>();

            foreach (var list in categoryList)
            {
                categoryListDto.Add(_mapper.Map<CategoryDto>(list));
            }

            return Ok(categoryListDto);
        }

        [HttpGet("{categoryId:int}", Name = "GetCategory")]
        //[HttpGet]
        public IActionResult GetCategory(int categoryId)
        {
            var itemtCategory = _ctRepo.GetCategory(categoryId);
            if (itemtCategory == null)
            {
                return NotFound();
            }

            var itemCategoryDto = _mapper.Map<CategoryDto>(itemtCategory);
            return Ok(itemCategoryDto);
        }

        [HttpPost]
        public IActionResult CreateCategory([FromBody] CategoryDto categoryDto)
        {
            if(categoryDto == null)
            {
                return BadRequest(ModelState);
            }

            if (_ctRepo.ExistCategory(categoryDto.Name))
            {
                ModelState.AddModelError("", "Category already exist");
                return StatusCode(404, ModelState);
            }

            var category = _mapper.Map<Category>(categoryDto);

            if(!_ctRepo.CreateCategory(category))
            {
                ModelState.AddModelError("", $"Something went wrong with registry {category.Name}");
                return StatusCode(500, ModelState);
            }

            //return Ok();
            return CreatedAtRoute("GetCategoria", new { categoryId = category.Id }, category);
        }

        [HttpPatch("{categoryId:int}", Name = "UpdateCategory")]
        public IActionResult UpdateCategory(int categoryId, [FromBody] CategoryDto categoryDto)
        {
            if(categoryDto == null || categoryId != categoryDto.Id)
            {
                return BadRequest(ModelState);
            }

            var category = _mapper.Map<Category>(categoryDto);

            if(!_ctRepo.UpdateCategory(category))
            {
                ModelState.AddModelError("", $"Something went wrong updating the registry {category.Name}");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        [HttpDelete("{categoryId:int}", Name = "DeleteCategory")]
        public IActionResult DeleteCategory(int categoryId)
        {

            if (_ctRepo.ExistCategory(categoryId))
            {
                return NotFound();
            }

            var category = _ctRepo.GetCategory(categoryId);

            if (!_ctRepo.DeleteCategory(category))
            {
                ModelState.AddModelError("", $"Something went wrong deleting the registry: {category.Name}");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

    }
}

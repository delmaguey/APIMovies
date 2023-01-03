using APIMovies.Models;
using APIMovies.Models.Dtos;
using APIMovies.Repository;
using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace APIMovies.Controllers
{
    [Route("api/movies")]
    [ApiController]
    public class MoviesController : Controller
    {
        private readonly IMovieRepository _movRepo;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly IMapper _mapper;

        public MoviesController(IMovieRepository movRepo, IMapper mapper, IWebHostEnvironment hostEnvironment)
        {
            _movRepo = movRepo;
            _mapper = mapper;
            _hostEnvironment = hostEnvironment;
        }

        [HttpGet]
        public IActionResult GetMovies()
        {
            var movieList = _movRepo.GetMovies();

            var movieListDto = new List<MovieDto>();

            foreach (var list in movieList)
            {
                movieListDto.Add(_mapper.Map<MovieDto>(list));
            }

            return Ok(movieListDto);
        }

        [HttpGet("{movieId:int}", Name = "GetMovie")]
        public IActionResult GetMovie(int movieId)
        {
            var movieItem = _movRepo.GetMovie(movieId);

            if (movieItem == null)
            {
                return NotFound();
            }

            var movieItemDto = _mapper.Map<MovieDto>(movieItem);

            return Ok(movieItemDto);
        }

        [HttpGet("GetMoviesInCategory/{categoryId:int}")]
        public IActionResult GetMoviesInCategory(int categoryId)
        {
            var movieList = _movRepo.GetMoviesInCategory(categoryId);

            if (movieList == null)
            {
                return NotFound();
            }

            var movieItem = new List<MovieDto>();
            foreach (var item in movieList)
            {
                movieItem.Add(_mapper.Map<MovieDto>(item));
            }

            return Ok(movieItem);
        }


        [HttpPost]
        public IActionResult CreateMovie([FromForm] MovieCreateDto movieCreateDto)
        {
            if (movieCreateDto == null)
            {
                return BadRequest(ModelState);
            }

            if (_movRepo.ExistMovie(movieCreateDto.Name))
            {
                ModelState.AddModelError("", "Movie already exist");
                return StatusCode(404, ModelState);
            }


            //****************** Upload image *******************
            var file = movieCreateDto.Photo;
            string mainPath = _hostEnvironment.WebRootPath;
            var files = HttpContext.Request.Form.Files;

            if (file.Length > 0)
            {
                var photoName = Guid.NewGuid().ToString();
                var uploaded = Path.Combine(mainPath, @"photos");
                var extension = Path.GetExtension(files[0].FileName);

                using (var fileStreams = new FileStream(Path.Combine(uploaded, photoName + extension), FileMode.Create))
                {
                    files[0].CopyTo(fileStreams);
                }

                movieCreateDto.ImagePath = @"\photos\" + photoName + extension;
            }

            //***************************************************


            var movie = _mapper.Map<Movie>(movieCreateDto);

            if (!_movRepo.CreateMovie(movie))
            {
                ModelState.AddModelError("", $"Something went wrong saving record: {movie.Name}");
                return StatusCode(500, ModelState);
            }

            return CreatedAtRoute("GetMovie", new { movieId = movie.Id }, movie);
        }

        [HttpGet("Search")]
        public IActionResult Search(string name)
        {
            try
            {
                var result = _movRepo.SearchMovie(name);
                if (result.Any())
                {
                    return Ok(result);
                }

                return NotFound();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving data");
            }
        }


        [HttpPatch("{movieId:int}", Name = "updateMovie")]
        public IActionResult UpdateMovie(int movieId, [FromBody] MovieDto movieDto)
        {
            if (movieDto == null || movieId != movieDto.Id)
            {
                return BadRequest(ModelState);
            }

            var movie = _mapper.Map<Movie>(movieDto);

            if (!_movRepo.UpdateMovie(movie))
            {
                ModelState.AddModelError("", $"Somethinf went wrong updating record: {movie.Name}");
                return BadRequest(ModelState);
            }

            return NoContent();
        }

        [HttpDelete]
        public IActionResult DeleteMovie(MovieDto movieDto)
        {
            if (!_movRepo.ExistMovie(movieDto.Id))
            {
                return NotFound();
            }

            var movie = _mapper.Map<Movie>(movieDto);

            if (!_movRepo.DeleteMovie(movie))
            {
                ModelState.AddModelError("", $"Somthing went wrong deleting the movie: {movie.Name}");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }
    }
}

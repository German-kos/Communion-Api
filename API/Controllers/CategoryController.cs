using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Controllers;
using api.Data;
using API.Interfaces;

namespace API.Controllers
{
    public class CategoryController : BaseApiController
    {
        private readonly IImageService _imageService;
        private readonly DataContext _context;
        public CategoryController(DataContext context, IImageService imageService)
        {
            _context = context;
            _imageService = imageService;

        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using API.Interfaces;
using API.Models;
using Microsoft.AspNetCore.Mvc;

namespace API.BLL.CategoryBLL
{
    public class CategoryRequestProcessors : ICategoryRequestProcessors
    {
        // Dependency Injections
        private readonly ICategoryMappers _map;
        public CategoryRequestProcessors(ICategoryMappers map)
        {
            _map = map;
        }


        // Methods:


        public ActionResult<CategoryDto> ProcessResult(ActionResult<Category>? result)
        {
            // Check for contents
            if (result != null)
            {
                // Check for an HTTP Response
                if (result.Result != null)
                    return result.Result;

                // Check for content in the returned value
                if (result.Value != null)
                    return _map.CategoryMapper(result.Value);
            }

            // Return an internal error if checks fail
            return new StatusCodeResult(500);
        }
    }
}
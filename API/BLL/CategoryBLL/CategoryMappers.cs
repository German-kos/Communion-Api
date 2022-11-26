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
    public class CategoryMappers : ICategoryMappers
    {
        public CategoryDto CategoryMapper(Category category)
        {
            // Deconstruction
            var (id, name, info, banner, subCategories) = category;

            // Return remap
            return new CategoryDto
            {
                Id = id,
                Name = name,
                Info = info,
                Banner = banner,
                SubCategories = SubCategoryMapper(subCategories),
            };
        }


        public ActionResult<List<CategoryDto>> CategoryMapper(List<Category>? categories)
        {
            // Check for contents
            if (categories == null || categories.Count == 0)
                return new NoContentResult();

            // Initializing the list
            List<CategoryDto> listOfCategories = new List<CategoryDto>();

            // Populating the list with remapped categories
            foreach (var category in categories)
                listOfCategories.Add(CategoryMapper(category)); // The singlular version of this mapper

            // Returning the list
            return listOfCategories;
        }


        /// <summary>
        /// This method takes the data recieved from the data access layer and converts it to a DTO suitable for the client.<br/>-----
        /// <br/>- converts -<br/>
        /// <paramref name="ForumCategory"/><br/>
        /// - to -<br/>
        /// <paramref name="ForumCategoryDto"/> <br/>-----
        /// </summary>
        /// <param name="subCategory">The <paramref name="ForumSubCategory"/> to remap.</param>
        /// <returns><paramref name="ForumSubCategoryDto"/> Remapped sub-category.</returns>
        private ForumSubCategoryDto SubCategoryMapper(SubCategory subCategory)
        {
            // Deconstruction
            var (id, categoryId, name) = subCategory;

            // Return remap
            return new ForumSubCategoryDto
            {
                Id = id,
                CategoryId = categoryId,
                Name = name
            };
        }


        /// <summary>
        /// A <paramref name="List"/> type overload for the <paramref name="SubCategoryMapper"/> method. Take in data recieved from the data access layer,
        /// and convert it to a Dto suitable for the client. <br/>-----<br/>
        /// - converts - <br/>
        /// <paramref name="List"/> of <paramref name="ForumSubCategory"/><br/>
        /// - to -<br/>
        /// <paramref name="List"/> of <paramref name="ForumSubCategoryDto"/> <br/>-----
        /// </summary>
        /// <param name="subCategories">The <paramref name="List"/> of <paramref name="ForumSubCategory"/> to remap.</param>
        /// <returns>A <paramref name="List"/> of remapped <paramref name="ForumSubCategoryDto"/>.</returns>
        private List<ForumSubCategoryDto> SubCategoryMapper(List<SubCategory> subCategories)
        {
            // Check for contents 
            if (subCategories == null || subCategories.Count == 0)
                return new List<ForumSubCategoryDto>(){
                new ForumSubCategoryDto{
                    Name = "No sub categories"
                }
            };

            // Initializing the list
            List<ForumSubCategoryDto> subCategoriesRemap = new List<ForumSubCategoryDto>();

            // Populating the list with remapped sub-categories
            foreach (var sub in subCategories)
                subCategoriesRemap.Add(SubCategoryMapper(sub)); // The singlular version of this mapper

            // Returning the list
            return subCategoriesRemap.OrderBy(i => i.Id).ToList();
        }
    }
}
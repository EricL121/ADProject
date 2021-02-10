﻿using ADProject.JsonObjects;
using ADProject.Models;
using ADProject.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace ADProject.ApiControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class mRecipeController : ControllerBase
    {
        private readonly ADProjContext _context;
        private readonly IRecipeService _recipesService;

        public mRecipeController(ADProjContext context, IRecipeService recipeService)
        {
            _context = context;
            _recipesService = recipeService;
        }

        /*[HttpGet]
        [Route("{id}")]
        public ActionResult<Recipe> GetRecipe(int id)
        {
            Recipe recipe = _recipesService.FindRecipeById(id);

            if (recipe == null)
            {
                return null;
            }
            return recipe;
        }*/

        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<Recipe>> GetRecipeById(int id)
        {
            var recipe = await _recipesService.GetRecipeById(id);

            if (recipe == null)
            {
                return NotFound();
            }

            return recipe;
        }

        [HttpGet]
        [Route("getAllRecipes")]
        public async Task<ActionResult<List<Recipe>>> GetAllRecipes()
        {
            List<Recipe> recipeList = await _recipesService.GetAllRecipesBasic();
            if (recipeList == null)
            {
                return NotFound();
            }

            return recipeList;
        }

        [HttpGet]
        [Route("search/{search}")]
        public async Task<ActionResult<List<Recipe>>> SearchRecipes(string search)
        {
            List<Recipe> recipeList = await _recipesService.GetAllRecipesSearch(search);
            if (recipeList == null)
            {
                return NotFound();
            }

            return recipeList;
        }



        [HttpPost]
        //[Route("post")]
        public async Task<ActionResult<Recipe>> CreateRecipe([FromBody] Recipe recipe)
        {
            try
            {
                recipe.User = _context.Users.FirstOrDefault();
                DateTime now = DateTime.Now;
                recipe.DateCreated = now;
                await _recipesService.AddRecipe(recipe);
                //_recipesService.AddRecipeNonAsync(recipe);
                
                return recipe;

            } catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return null;
            }
            
        }

        [HttpDelete]
        [Route("delete/{id}")]
        public async Task<booleanJson> DeleteRecipe(int id)
        {
            booleanJson isDeleted = new booleanJson();

            isDeleted.flag = await _recipesService.DeleteRecipe(id);
            return isDeleted;
        }

        [HttpPost]
        [Route("update/{id}")]
        public async Task<booleanJson> UpdateRecipe(int id, [FromBody] Recipe recipe)
        {
            booleanJson isUpdated = new booleanJson();

            isUpdated.flag = await _recipesService.EditRecipe(id, recipe);
            return isUpdated;
        }

        /*[HttpDelete]
        [Route("deleterecipe/{​​id}​​")]
        public async Task<ActionResult<booleanJson> DeleteRecipe(int id)
        {​​
            booleanJson sample = new booleanJson();
            try
            {​​
                await _recipesService.DeleteRecipe(id);
                sample.flag = true;
                return sample;
            }​​ catch
            {​​
                sample.flag = false;
                return sample;
            }​​
        }*/


    }
}

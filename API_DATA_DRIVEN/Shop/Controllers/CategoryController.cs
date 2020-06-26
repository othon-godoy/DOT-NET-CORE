using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shop.Data;
using Shop.Models;

[Route("categories")]
public class CategoryController : ControllerBase
{
    [HttpGet]
    [Route("")]
    public async Task<ActionResult<List<Category>>> Get(
        [FromServices] DataContext context
    )
    {
        var categories = await context.Categories.AsNoTracking().ToListAsync();
        return Ok(categories);
    }

    [HttpGet]
    [Route("{id:int}")]
    public async Task<ActionResult<Category>> GetById(
        long id,
        [FromServices] DataContext context
    )
    {
        var category = await context.Categories.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
        return Ok(category);
    }
    
    [HttpPost]
    [Route("")]
    public async Task<ActionResult<Category>> Post(
        [FromBody] Category model,
        [FromServices] DataContext context
    )
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            context.Categories.Add(model);
            await context.SaveChangesAsync();
            
            return Ok(model);
        }
        catch (Exception)
        {            
            return BadRequest(new { message = "Não foi possível cadastrar a categoria" });
        }

    }

    [HttpPut]
    [Route("{id:int}")]
    public async Task<ActionResult<Category>> Put(
        int id, 
        [FromBody]Category model,
        [FromServices] DataContext context
    )
    {
        // Verifica se o Id informado é o mesmo do model
        if (id != model.Id)
            return NotFound(new { message = "Categoria não encontrada" });

        // Verifica se os dados são válidos
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            context.Entry<Category>(model).State = EntityState.Modified;
            await context.SaveChangesAsync();
            return Ok(model);        
        }
        catch (DbUpdateConcurrencyException)
        {            
            return BadRequest( new { message = "Esta categoria já foi editada" } );
        }
        catch (Exception)
        {            
            return BadRequest( new { message = "Não foi possível editar a categoria" } );
        }
    }

    [HttpDelete]
    [Route("{id:int}")]
    public async Task<ActionResult<Category>> Delete(
        int id,
        [FromServices] DataContext context
    )
    {
        var category = await context.Categories.FirstOrDefaultAsync(x => x.Id == id);

        if (category == null)
            return NotFound(new { message = "Categoria não encontrada" });

        try
        {
            context.Categories.Remove(category);
            await context.SaveChangesAsync();
            return Ok(new { message = "Categoria removida com sucesso" });
        }
        catch (Exception)
        {   
            return BadRequest(new { message = "Não foi possível remover a categoria" });
        }
    }
}
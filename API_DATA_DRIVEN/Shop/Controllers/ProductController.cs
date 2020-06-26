using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shop.Data;
using Shop.Models;

[Route("products")]
public class ProductController : ControllerBase
{
    [HttpGet]
    [Route("")]
    public async Task<ActionResult<List<Product>>> Get(
        [FromServices] DataContext context
    )
    {
        var products = await context
            .Products
            .Include(x => x.Category)
            .AsNoTracking()
            .ToListAsync();

        return products;
    }

    [HttpGet]
    [Route("{id:int}")]
    public async Task<ActionResult<Product>> GetById(
        long id,
        [FromServices] DataContext context
    )
    {
        var product = await context
            .Products
            .Include(x => x.Category)
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id);

        return product;
    }

    [HttpGet]
    [Route("categories/{id:int}")]
    public async Task<ActionResult<List<Product>>> GetByCategory(
        long id,
        [FromServices] DataContext context
    )
    {
        var products = await context
            .Products
            .Include(x => x.Category)
            .AsNoTracking()
            .Where(x => x.CategoryId == id)
            .ToListAsync();

        return products;
    }
    
    [HttpPost]
    [Route("")]
    public async Task<ActionResult<Product>> Post(
        [FromBody] Product model,
        [FromServices] DataContext context
    )
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            context.Products.Add(model);
            await context.SaveChangesAsync();
            
            return Ok(model);
        }
        catch (Exception)
        {            
            return BadRequest(new { message = "Não foi possível cadastrar o produto" });
        }

    }

    [HttpPut]
    [Route("{id:int}")]
    public async Task<ActionResult<Product>> Put(
        int id, 
        [FromBody]Product model,
        [FromServices] DataContext context
    )
    {
        // Verifica se o Id informado é o mesmo do model
        if (id != model.Id)
            return NotFound(new { message = "Produto não encontrado" });

        // Verifica se os dados são válidos
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            context.Entry<Product>(model).State = EntityState.Modified;
            await context.SaveChangesAsync();
            return Ok(model);        
        }
        catch (DbUpdateConcurrencyException)
        {            
            return BadRequest( new { message = "Este produto já foi editado" } );
        }
        catch (Exception)
        {            
            return BadRequest( new { message = "Não foi possível editar o produto" } );
        }
    }

    [HttpDelete]
    [Route("{id:int}")]
    public async Task<ActionResult<Product>> Delete(
        int id,
        [FromServices] DataContext context
    )
    {
        var Product = await context.Products.FirstOrDefaultAsync(x => x.Id == id);

        if (Product == null)
            return NotFound(new { message = "Categoria não encontrada" });

        try
        {
            context.Products.Remove(Product);
            await context.SaveChangesAsync();
            return Ok(new { message = "Categoria removida com sucesso" });
        }
        catch (Exception)
        {   
            return BadRequest(new { message = "Não foi possível remover a categoria" });
        }
    }
}

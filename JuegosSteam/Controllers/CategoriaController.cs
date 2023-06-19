using JuegosSteam.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JuegosSteam.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriaController : ControllerBase
    {

        private readonly BaseSteamContext db = new();

        [HttpGet]
        public async Task<IActionResult> GetCategoria()
        {
            Response response = new();
            try
            {
                if (db.Categoria == null)
                {
                    response.Message = "La tabla no esta activa";
                    return NotFound(response);
                }
                var categorias = await db.Categoria.Select(
                    x => new
                    {
                        x.Id,
                        x.Nombre,
                        x.Descripcion
                    }).ToListAsync();
                if (categorias != null)
                {
                    if (categorias.Count == 0)
                    {
                        response.Message = "No hay registros";
                    }
                    response.Success = true;
                    response.Data = categorias;
                }
                return Ok(response);
            }
            catch (Exception ex)
            {

                response.Message = ex.ToString();
                return BadRequest(response);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategoria(int id)
        {
            Response response = new();
            try
            {
                //find busca solo por el identificador
                var buscarCategoria = await db.Categoria.FindAsync(id);
                if (buscarCategoria == null)
                {
                    response.Message = "No existe registron con ese id";
                    return NotFound(response);
                }
                else
                {
                    response.Success = true;
                    response.Data = buscarCategoria;
                }
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.Message = "Error: " + ex.ToString();
                return BadRequest();
            }
        }

        [HttpPost]
        public async Task<ActionResult<Categorium>> PostCategoria(Categorium categorium)
        {
            var existingCategoria= await db.Categoria.FirstOrDefaultAsync(d => d.Nombre == categorium.Nombre);
            if (existingCategoria != null)
            {
                Response response = new();
                response.Success = false;
                response.Message = "El nombre ya está en uso";
                return BadRequest(response);
            }
            db.Categoria.Add(categorium);
            await db.SaveChangesAsync();

            Response successResponse = new();
            successResponse.Success = true;
            successResponse.Message = "Guardado con éxito";

            //return Ok(response); //retorna el mensaje que entregamos
            //retorna al getid de sucursal
            return CreatedAtAction("GetCategoria", new { id = categorium.Id }, categorium);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutCategoria(int id, Categorium categorium)
        {
            Response response = new();
            try
            {
                var buscaCategoria = await db.Categoria.FindAsync(id);
                if (buscaCategoria == null)
                {
                    response.Message = "No existe registro con ese id";
                    return NotFound(response);
                }
            
                var existeCategoria = await db.Categoria.AnyAsync(d => d.Nombre == categorium.Nombre && d.Id != id);
                if (existeCategoria)
                {
                    response.Message = "Ya existe una categoria con el mismo nombre";
                    return BadRequest(response);
                }

                // Actualizar los datos del usuario con los valores proporcionados
                buscaCategoria.Id = categorium.Id;
                buscaCategoria.Nombre = categorium.Nombre;
                buscaCategoria.Descripcion = categorium.Descripcion;

                await db.SaveChangesAsync();

                response.Success = true;
                response.Message = "Registro actualizado con éxito";
                response.Data = buscaCategoria;

                return Ok(response);
            }
            catch (Exception ex)
            {
                response.Message = "Error: " + ex.ToString();
                return BadRequest(response);
            }
        }

        // DELETE: api/Sucursal/1
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategoria(int id)
        {
            Response response = new();
            var buscarCategoria = await db.Categoria.FindAsync(id);
            if (buscarCategoria != null)
            {
                var buscarjuegos = await db.Juegos.FirstOrDefaultAsync(x => x.Categoria == id);
                if (buscarjuegos != null)
                {
                    response.Message = "No se puede eliminar por tener datos";
                    return NotFound(response);
                }

                db.Remove(buscarCategoria);
                await db.SaveChangesAsync();
                response.Message = "El registro se ha eliminado con éxito";
                response.Success = true;
                return Ok(response);
            }
            response.Message = "No se encuntra el id";
            return NotFound(response);
        }


    }
}


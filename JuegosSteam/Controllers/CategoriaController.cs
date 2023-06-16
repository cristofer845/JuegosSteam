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
        public async Task<ActionResult<Categorium>> PostEditor(Editor editor)
        {

            db.Editors.Add(editor);
            await db.SaveChangesAsync();
            Response response = new();
            response.Success = true;
            response.Message = "Guardado con éxito";

            //return Ok(response); //retorna el mensaje que entregamos
            //retorna al getid de sucursal
            return CreatedAtAction("GetEditor", new { id = editor.Id }, editor);
        }
        // DELETE: api/Sucursal/1
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEditor(int id)
        {
            Response response = new();
            var buscarEditor = await db.Editors.FindAsync(id);
            if (buscarEditor != null)
            {

                db.Remove(buscarEditor);
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


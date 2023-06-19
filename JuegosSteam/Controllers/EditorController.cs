using JuegosSteam.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JuegosSteam.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EditorController : ControllerBase
    {
        private readonly BaseSteamContext db = new();

        [HttpGet]
        public async Task<IActionResult> GetEditor()
        {
            Response response = new();
            try
            {
                if (db.Editors == null)
                {
                    response.Message = "La tabla no esta activa";
                    return NotFound(response);
                }
                var editores = await db.Editors.Select(
                    x => new
                    {
                        x.Id,
                        x.Nombre,
                        x.Pais
                    }).ToListAsync();
                if (editores != null)
                {
                    if (editores.Count == 0)
                    {
                        response.Message = "No hay registros";
                    }
                    response.Success = true;
                    response.Data = editores;
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
        public async Task<IActionResult> GetEditor(int id)
        {
            Response response = new();
            try
            {
                //find busca solo por el identificador
                var buscarEditor = await db.Editors.FindAsync(id);
                if (buscarEditor == null)
                {
                    response.Message = "No existe registron con ese id";
                    return NotFound(response);
                }
                else
                {
                    response.Success = true;
                    response.Data = buscarEditor;
                }
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.Message = "Error: " + ex.ToString();
                return BadRequest();
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutEditor(int id, Editor editor)
        {
            Response response = new();
            try
            {
                var buscarEditor = await db.Editors.FindAsync(id);
                if (buscarEditor == null)
                {
                    response.Message = "No existe registro con ese id";
                    return NotFound(response);
                }

                var existeEditor = await db.Editors.AnyAsync(d => d.Nombre == editor.Nombre && d.Id != id);
                if (existeEditor)
                {
                    response.Message = "Ya existe un editor con el mismo nombre";
                    return BadRequest(response);
                }

                // Actualizar los datos del usuario con los valores proporcionados
                buscarEditor.Nombre = editor.Nombre;
                buscarEditor.Pais = editor.Pais;

                await db.SaveChangesAsync();

                response.Success = true;
                response.Message = "Registro actualizado con éxito";
                response.Data = buscarEditor;

                return Ok(response);
            }
            catch (Exception ex)
            {
                response.Message = "Error: " + ex.ToString();
                return BadRequest(response);
            }
        }

        [HttpPost]
        public async Task<ActionResult<Editor>> PostEditor(Editor editor)
        {

            var existeEditor = await db.Editors.FirstOrDefaultAsync(d => d.Nombre == editor.Nombre);
            if (existeEditor != null)
            {
                Response response = new();
                response.Success = false;
                response.Message = "El nombre ya está en uso";
                return BadRequest(response);
            }

            db.Editors.Add(editor);
            await db.SaveChangesAsync();

            Response successResponse = new();
            successResponse.Success = true;
            successResponse.Message = "Guardado con éxito";
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
                var existeJuego = await db.Juegos.AnyAsync(j => j.Editor == id);
                if (existeJuego)
                {
                    response.Message = "No se puede eliminar el editor porque hay juegos asociados a él";
                    return BadRequest(response);
                }

                db.Remove(buscarEditor);
                await db.SaveChangesAsync();
                response.Message = "El registro se ha eliminado con éxito";
                response.Success = true;
                return Ok(response);
            }
            response.Message = "No se encuentra el ID";
            return NotFound(response);

        }
    }
}
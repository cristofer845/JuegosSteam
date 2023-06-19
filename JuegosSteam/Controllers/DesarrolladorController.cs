using JuegosSteam.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JuegosSteam.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DesarrolladorController : ControllerBase
    {

        private readonly BaseSteamContext db = new();

        [HttpGet]
        public async Task<IActionResult> GetDesarrollador()
        {
            Response response = new();
            try
            {
                if (db.Desarrolladors == null)
                {
                    response.Message = "La tabla no esta activa";
                    return NotFound(response);
                }
                var desarrolladores = await db.Desarrolladors.Select(
                    x => new
                    {
                        x.Id,
                        x.Nombre,
                        x.Pais
                    }).ToListAsync();
                if (desarrolladores != null)
                {
                    if (desarrolladores.Count == 0)
                    {
                        response.Message = "No hay registros";
                    }
                    response.Success = true;
                    response.Data = desarrolladores;
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
        public async Task<IActionResult> GetDesarrollador(int id)
        {
            Response response = new();
            try
            {
                //find busca solo por el identificador
                var buscarDesarrollador = await db.Desarrolladors.FindAsync(id);
                if (buscarDesarrollador == null)
                {
                    response.Message = "No existe registron con ese id";
                    return NotFound(response);
                }
                else
                {
                    response.Success = true;
                    response.Data = buscarDesarrollador;
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
        public async Task<IActionResult> PutDesarrollador(int id, Desarrollador desarrollador)
        {
            Response response = new();
            try
            {
                var buscarDesarrollador = await db.Desarrolladors.FindAsync(id);
                if (buscarDesarrollador == null)
                {
                    response.Message = "No existe registro con ese id";
                    return NotFound(response);
                }

                var existeDesarrollador = await db.Desarrolladors.AnyAsync(d => d.Nombre == desarrollador.Nombre && d.Id != id);
                if (existeDesarrollador)
                {
                    response.Message = "Ya existe un desarrollador con el mismo nombre";
                    return BadRequest(response);
                }

                // Actualizar los datos del usuario con los valores proporcionados
                buscarDesarrollador.Nombre = desarrollador.Nombre;
                buscarDesarrollador.Pais = desarrollador.Pais;

                await db.SaveChangesAsync();

                response.Success = true;
                response.Message = "Registro actualizado con éxito";
                response.Data = buscarDesarrollador;

                return Ok(response);
            }
            catch (Exception ex)
            {
                response.Message = "Error: " + ex.ToString();
                return BadRequest(response);
            }
        }

        [HttpPost]
        public async Task<ActionResult<Desarrollador>> PostDesarrollador(Desarrollador desarrollador)
        {

            var existingDesarrollador = await db.Desarrolladors.FirstOrDefaultAsync(d => d.Nombre == desarrollador.Nombre);
            if (existingDesarrollador != null)
            {
                Response response = new();
                response.Success = false;
                response.Message = "El nombre ya está en uso";
                return BadRequest(response);
            }

            db.Desarrolladors.Add(desarrollador);
            await db.SaveChangesAsync();

            Response successResponse = new();
            successResponse.Success = true;
            successResponse.Message = "Guardado con éxito";
            return CreatedAtAction("GetDesarrollador", new { id = desarrollador.Id }, desarrollador);
        }
        // DELETE: api/Sucursal/1
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDesarrollador(int id)
        {
            Response response = new();
            var buscarDesarrollador = await db.Desarrolladors.FindAsync(id);
            if (buscarDesarrollador != null)
            {
                var existeJuego = await db.Juegos.AnyAsync(j => j.Desarrollador == id);
                if (existeJuego)
                {
                    response.Message = "No se puede eliminar el desarrollador porque hay juegos asociados a él";
                    return BadRequest(response);
                }

                db.Remove(buscarDesarrollador);
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


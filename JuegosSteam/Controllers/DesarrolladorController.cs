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

        [HttpPost]
        public async Task<ActionResult<Desarrollador>> PostDesarrollador(Desarrollador desarrollador)
        {

            db.Desarrolladors.Add(desarrollador);
            await db.SaveChangesAsync();
            Response response = new();
            response.Success = true;
            response.Message = "Guardado con éxito";

            //return Ok(response); //retorna el mensaje que entregamos
            //retorna al getid de sucursal
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

                db.Remove(buscarDesarrollador);
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


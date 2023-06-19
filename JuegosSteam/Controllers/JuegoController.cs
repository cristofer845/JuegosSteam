using JuegosSteam.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JuegosSteam.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JuegoController : ControllerBase
    {

        private readonly BaseSteamContext db = new();

        [HttpGet]
        public async Task<IActionResult> GetJuego()
        {
            Response response = new();
            try
            {
                if (db.Juegos == null)
                {
                    response.Message = "La tabla no esta activa";
                    return NotFound(response);
                }
                var juegos = await db.Juegos.Select(
                    x => new
                    {
                        x.Id,
                        x.Nombre,
                        x.Precio,
                        x.Categoria,
                        x.Plataforma,
                        x.Desarrollador,
                        x.UsuarioRegistrado,
                        x.Editor

                    }).ToListAsync();
                if (juegos != null)
                {
                    if (juegos.Count == 0)
                    {
                        response.Message = "No hay registros";
                    }
                    response.Success = true;
                    response.Data = juegos;
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
        public async Task<IActionResult> GetJuego(int id)
        {
            Response response = new();
            try
            {
                //find busca solo por el identificador
                var buscarJuego = await db.Juegos.FindAsync(id);
                if (buscarJuego == null)
                {
                    response.Message = "No existe registron con ese id";
                    return NotFound(response);
                }
                else
                {
                    response.Success = true;
                    response.Data = buscarJuego;
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
        public async Task<ActionResult<Juego>> PostJuego(Juego juego)
        {
            Response response = new();
            var existeJuego = await db.Juegos.FirstOrDefaultAsync(d => d.Nombre == juego.Nombre);
            if (existeJuego != null)
            {
                response.Success = false;
                response.Message = "El nombre ya está en uso";
                return BadRequest(response);
            }

            db.Juegos.Add(juego);
            await db.SaveChangesAsync();
            response.Success = true;
            response.Message = "Guardado con éxito";

            //return Ok(response); //retorna el mensaje que entregamos
            //retorna al getid de sucursal
            return CreatedAtAction("GetJuego", new { id = juego.Id }, juego);
        }
        // DELETE: api/Sucursal/1
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteJuego(int id)
        {
            Response response = new();
            var buscarJuego = await db.Juegos.FindAsync(id);
            if (buscarJuego != null)
            {

                db.Remove(buscarJuego);
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




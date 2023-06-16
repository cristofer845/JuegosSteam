using JuegosSteam.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JuegosSteam.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {

        private readonly BaseSteamContext db = new();

        [HttpGet]
        public async Task<IActionResult> GetRole()
        {
            Response response = new();
            try
            {
                if (db.Roles == null)
                {
                    response.Message = "La tabla no esta activa";
                    return NotFound(response);
                }
                var roles = await db.Roles.Select(
                    x => new
                    {
                        x.Id,
                        x.Nombre,
                        x.Permisos
                    }).ToListAsync();
                if (roles != null)
                {
                    if (roles.Count == 0)
                    {
                        response.Message = "No hay registros";
                    }
                    response.Success = true;
                    response.Data = roles;
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
        public async Task<IActionResult> GetRole(int id)
        {
            Response response = new();
            try
            {
                //find busca solo por el identificador
                var buscarRole = await db.Roles.FindAsync(id);
                if (buscarRole == null)
                {
                    response.Message = "No existe registron con ese id";
                    return NotFound(response);
                }
                else
                {
                    response.Success = true;
                    response.Data = buscarRole;
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
        public async Task<ActionResult<Role>> PostRole(Role role)
        {

            db.Roles.Add(role);
            await db.SaveChangesAsync();
            Response response = new();
            response.Success = true;
            response.Message = "Guardado con éxito";

            //return Ok(response); //retorna el mensaje que entregamos
            //retorna al getid de sucursal
            return CreatedAtAction("GetRole", new { id = role.Id }, role);
        }
        // DELETE: api/Sucursal/1
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRole(int id)
        {
            Response response = new();
            var buscarRole = await db.Roles.FindAsync(id);
            if (buscarRole != null)
            {

                db.Remove(buscarRole);
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


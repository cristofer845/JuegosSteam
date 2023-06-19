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

        [HttpPut("{id}")]
        public async Task<IActionResult> PutRole(int id, Role role)
        {
            Response response = new();
            try
            {
                var buscarRole = await db.Roles.FindAsync(id);
                if (buscarRole == null)
                {
                    response.Message = "No existe registro con ese id";
                    return NotFound(response);
                }

                var existeRole = await db.Roles.AnyAsync(d => d.Nombre == role.Nombre && d.Id != id);
                if (existeRole)
                {
                    response.Message = "Ya existe un rol con el mismo nombre";
                    return BadRequest(response);
                }

                // Actualizar los datos del usuario con los valores proporcionados
                buscarRole.Nombre = role.Nombre;
                buscarRole.Permisos = role.Permisos;

                await db.SaveChangesAsync();

                response.Success = true;
                response.Message = "Registro actualizado con éxito";
                response.Data = buscarRole;

                return Ok(response);
            }
            catch (Exception ex)
            {
                response.Message = "Error: " + ex.ToString();
                return BadRequest(response);
            }
        }

        [HttpPost]
        public async Task<ActionResult<Role>> PostRole(Role role)
        {

            var existingRole = await db.Roles.FirstOrDefaultAsync(d => d.Nombre == role.Nombre);
            if (existingRole != null)
            {
                Response response = new();
                response.Success = false;
                response.Message = "El nombre ya está en uso";
                return BadRequest(response);
            }

            db.Roles.Add(role);
            await db.SaveChangesAsync();

            Response successResponse = new();
            successResponse.Success = true;
            successResponse.Message = "Guardado con éxito";
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
                var existeUsuario = await db.Usuarios.AnyAsync(u => u.Roles == id);
                if (existeUsuario)
                {
                    response.Message = "No se puede eliminar el rol porque hay usuarios asociados a él";
                    return BadRequest(response);
                }

                db.Remove(buscarRole);
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
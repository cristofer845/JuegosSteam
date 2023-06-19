using JuegosSteam.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JuegosSteam.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {

        private readonly BaseSteamContext db = new();

        [HttpGet]
        public async Task<IActionResult> GetUsuario()
        {
            Response response = new();
            try
            {
                if (db.Usuarios == null)
                {
                    response.Message = "La tabla no esta activa";
                    return NotFound(response);
                }
                var usuarios = await db.Usuarios.Select(
                    x => new
                    {
                        x.Id,
                        x.Nombre,
                        x.Rut,
                        x.Telefono,
                        x.Correo,
                        x.Roles,

                    }).ToListAsync();
                if (usuarios != null)
                {
                    if (usuarios.Count == 0)
                    {
                        response.Message = "No hay registros";
                    }
                    response.Success = true;
                    response.Data = usuarios;
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
        public async Task<IActionResult> GetUsuario(int id)
        {
            Response response = new();
            try
            {
                //find busca solo por el identificador
                var buscarUsuario = await db.Usuarios.FindAsync(id);
                if (buscarUsuario == null)
                {
                    response.Message = "No existe registron con ese id";
                    return NotFound(response);
                }
                else
                {
                    response.Success = true;
                    response.Data = buscarUsuario;
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
        public async Task<ActionResult<Usuario>> PostUsuario(Usuario usuario)
        {

            db.Usuarios.Add(usuario);
            await db.SaveChangesAsync();
            Response response = new();
            response.Success = true;
            response.Message = "Guardado con éxito";

            //return Ok(response); //retorna el mensaje que entregamos
            //retorna al getid de sucursal
            return CreatedAtAction("GetUsuario", new { id = usuario.Id }, usuario);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutUsuario(int id, Usuario usuario)
        {
            Response response = new();
            try
            {
                var buscarUsuario = await db.Usuarios.FindAsync(id);
                if (buscarUsuario == null)
                {
                    response.Message = "No existe registro con ese id";
                    return NotFound(response);
                }

                // Actualizar los datos del usuario con los valores proporcionados
                buscarUsuario.Nombre = usuario.Nombre;
                buscarUsuario.Rut = usuario.Rut;
                buscarUsuario.Telefono = usuario.Telefono;
                buscarUsuario.Correo = usuario.Correo;
                buscarUsuario.Roles = usuario.Roles;

                await db.SaveChangesAsync();

                response.Success = true;
                response.Message = "Registro actualizado con éxito";
                response.Data = buscarUsuario;

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
        public async Task<IActionResult> DeleteUsuario(int id)
        {
            Response response = new();
            var buscarUsuario = await db.Usuarios.FindAsync(id);
            if (buscarUsuario != null)
            {

                db.Remove(buscarUsuario);
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



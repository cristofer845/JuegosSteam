using System;
using System.Collections.Generic;

namespace JuegosSteam.Models;

public partial class Usuario
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public string Rut { get; set; } = null!;

    public string Correo { get; set; } = null!;

    public int Telefono { get; set; }

    public int? Roles { get; set; }

    public virtual ICollection<Juego> Juegos { get; set; } = new List<Juego>();

    public virtual Role? RolesNavigation { get; set; }
}

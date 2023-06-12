using System;
using System.Collections.Generic;

namespace JuegosSteam.Models;

public partial class Desarrollador
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public string? Pais { get; set; }

    public virtual ICollection<Juego> Juegos { get; set; } = new List<Juego>();
}

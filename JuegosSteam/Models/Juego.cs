using System;
using System.Collections.Generic;

namespace JuegosSteam.Models;

public partial class Juego
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public int Categoria { get; set; }

    public int Desarrollador { get; set; }

    public int Editor { get; set; }

    public string? Plataforma { get; set; }

    public int Precio { get; set; }

    public int UsuarioRegistrado { get; set; }

    public virtual Categorium CategoriaNavigation { get; set; } = null!;

    public virtual Desarrollador DesarrolladorNavigation { get; set; } = null!;

    public virtual Editor EditorNavigation { get; set; } = null!;

    public virtual Usuario UsuarioRegistradoNavigation { get; set; } = null!;
}

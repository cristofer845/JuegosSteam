using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace JuegosSteam.Models;

public partial class BaseSteamContext : DbContext
{
    public BaseSteamContext()
    {
    }

    public BaseSteamContext(DbContextOptions<BaseSteamContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Categorium> Categoria { get; set; }

    public virtual DbSet<Desarrollador> Desarrolladors { get; set; }

    public virtual DbSet<Editor> Editors { get; set; }

    public virtual DbSet<Juego> Juegos { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("server=.\\sqlexpress; initial catalog=BaseSteam; Trusted_connection=True;Encrypt=False;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Categorium>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Descripcion)
                .IsUnicode(false)
                .HasColumnName("descripcion");
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("nombre");
        });

        modelBuilder.Entity<Desarrollador>(entity =>
        {
            entity.ToTable("Desarrollador");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("nombre");
            entity.Property(e => e.Pais)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("pais");
        });

        modelBuilder.Entity<Editor>(entity =>
        {
            entity.ToTable("Editor");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("nombre");
            entity.Property(e => e.Pais)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("pais");
        });

        modelBuilder.Entity<Juego>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Categoria).HasColumnName("categoria");
            entity.Property(e => e.Desarrollador).HasColumnName("desarrollador");
            entity.Property(e => e.Editor).HasColumnName("editor");
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("nombre");
            entity.Property(e => e.Plataforma)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("plataforma");
            entity.Property(e => e.Precio).HasColumnName("precio");
            entity.Property(e => e.UsuarioRegistrado).HasColumnName("usuario_registrado");

            entity.HasOne(d => d.CategoriaNavigation).WithMany(p => p.Juegos)
                .HasForeignKey(d => d.Categoria)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Juegos_Categoria");

            entity.HasOne(d => d.DesarrolladorNavigation).WithMany(p => p.Juegos)
                .HasForeignKey(d => d.Desarrollador)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Juegos_Desarrollador");

            entity.HasOne(d => d.EditorNavigation).WithMany(p => p.Juegos)
                .HasForeignKey(d => d.Editor)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Juegos_Editor");

            entity.HasOne(d => d.UsuarioRegistradoNavigation).WithMany(p => p.Juegos)
                .HasForeignKey(d => d.UsuarioRegistrado)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Juegos_Usuario");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("nombre");
            entity.Property(e => e.Permisos)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("permisos");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.ToTable("Usuario");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Correo)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("correo");
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("nombre");
            entity.Property(e => e.Roles).HasColumnName("roles");
            entity.Property(e => e.Rut)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("rut");
            entity.Property(e => e.Telefono).HasColumnName("telefono");

            entity.HasOne(d => d.RolesNavigation).WithMany(p => p.Usuarios)
                .HasForeignKey(d => d.Roles)
                .HasConstraintName("FK_Usuario_Roles");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

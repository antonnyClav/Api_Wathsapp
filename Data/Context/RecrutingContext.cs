using Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Data.Context
{
    public partial class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext()
        {
        }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) //ConnectionString
        {
        }

        public virtual DbSet<MensajesWhatsapp> MensajesWhatsapps { get; set; }
        //---------------------------LOGS---------------------------
        public virtual DbSet<WebapiLogs> webapi_logs { get; set; }

        //---------------------------VISTAS---------------------------

        //---------------------------PARAMETRIA---------------------------
        public virtual DbSet<Parametros> Parametros { get; set; }
        //---------------------------PARAMETRIA---------------------------

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<MensajesWhatsapp>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.ToTable("mensajes_whatsapp");

                entity.Property(e => e.Id).HasColumnName("mew_id");

                entity.Property(e => e.Accion)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("mew_accion");

                entity.Property(e => e.Adjunto)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("mew_adjunto");

                entity.Property(e => e.AltaFecha)
                    .HasColumnType("datetime")
                    .HasColumnName("mew_alta_fecha");

                entity.Property(e => e.BajaFecha)
                    .HasColumnType("datetime")
                    .HasColumnName("mew_baja_fecha");

                entity.Property(e => e.Destinatario)
                    .IsRequired()
                    .HasMaxLength(1000)
                    .IsUnicode(false)
                    .HasColumnName("mew_destinatario");

                entity.Property(e => e.IdWa)
                    .IsRequired()
                    .HasMaxLength(1000)
                    .IsUnicode(false)
                    .HasColumnName("mew_id_wa");

                entity.Property(e => e.Enviado).HasColumnName("mew_enviado");

                entity.Property(e => e.Filler)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("mew_filler");

                entity.Property(e => e.IdRef).HasColumnName("mew_id_ref");

                entity.Property(e => e.Mensaje)
                    .IsUnicode(false)
                    .HasColumnName("mew_mensaje");

                entity.Property(e => e.ModiFecha)
                    .HasColumnType("datetime")
                    .HasColumnName("mew_modi_fecha");

                entity.Property(e => e.Obs)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("mew_obs");

                entity.Property(e => e.Origen)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("mew_origen");

                entity.Property(e => e.Recibido).HasColumnName("mew_recibido");

                entity.Property(e => e.Leido).HasColumnName("mew_leido");

                entity.Property(e => e.RefEntidad)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("mew_ref_entidad");

                entity.Property(e => e.Remitente)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("mew_remitente");

                entity.Property(e => e.UsuId).HasColumnName("mew_usu_id");
            });

            modelBuilder.Entity<WebapiLogs>(entity =>
            {
                entity.ToTable("webapi_logs");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Clave)
                    .HasColumnName("clave")
                    .HasMaxLength(50);

                entity.Property(e => e.FechaLog).HasColumnName("fecha_log");

                entity.Property(e => e.Proceso)
                    .HasColumnName("proceso")
                    .HasMaxLength(50);

                entity.Property(e => e.Texto).HasColumnName("texto");

                entity.Property(e => e.Usu).HasColumnName("usu");
            });

            modelBuilder.Entity<Parametros>(entity =>
            {
                entity.HasKey(e => e.Id)
                    .HasName("PK__parametr__71142285871BFA1A");

                entity.ToTable("parametros");

                entity.Property(e => e.Id).HasColumnName("par_id");

                entity.Property(e => e.AltaFecha)
                    .HasColumnName("par_alta_fecha")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(CURRENT_TIMESTAMP)");

                entity.Property(e => e.Ambito)
                    .HasColumnName("par_ambito")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.BajaFecha)
                    .HasColumnName("par_baja_fecha")
                    .HasColumnType("datetime");

                entity.Property(e => e.Codigo)
                    .HasColumnName("par_codigo")
                    .HasMaxLength(25)
                    .IsUnicode(false);

                entity.Property(e => e.Filler)
                    .HasColumnName("par_filler")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.ModiFecha)
                    .HasColumnName("par_modi_fecha")
                    .HasColumnType("datetime");

                entity.Property(e => e.Nombre)
                    .HasColumnName("par_nombre")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ReqAuth).HasColumnName("par_req_auth");

                entity.Property(e => e.UsuId)
                    .HasColumnName("par_usu_id")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.Valor)
                    .HasColumnName("par_valor")
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}

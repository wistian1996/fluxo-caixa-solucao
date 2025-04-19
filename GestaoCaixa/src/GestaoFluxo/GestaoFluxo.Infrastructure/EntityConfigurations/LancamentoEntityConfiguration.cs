using GestaoFluxo.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace GestaoFluxo.Infrastructure.EntityConfigurations
{
    internal class LancamentoEntityConfiguration : IEntityTypeConfiguration<Lancamento>
    {
        public void Configure(EntityTypeBuilder<Lancamento> builder)
        {
            builder.ToTable("lancamentos");

            builder.Property(s => s.Id)
                .HasColumnName("id")
                .ValueGeneratedOnAdd();

            builder.HasKey(s => s.Id);

            builder.Property(s => s.ComercianteId)
                .HasColumnName("comerciante_id")
                .IsRequired();

            builder.Property(s => s.IsCredito)
                .HasColumnName("is_credito")
                .IsRequired();

            builder.Property(s => s.Valor)
                .HasColumnName("valor")
                .IsRequired()
                .HasPrecision(18, 2);

            builder.Property(s => s.DataCriacao)
                .HasColumnName("data_criacao")
                .HasDefaultValueSql("(UTC_TIMESTAMP(3))")
                .IsRequired();

            builder.HasIndex(s => new
            {
                s.ComercianteId,
                s.IsCredito,
                s.DataCriacao
            });

            builder.HasIndex(s => new
            {
                s.IsCredito,
                s.DataCriacao
            });

            builder.HasIndex(s => new
            {
                s.DataCriacao
            });
        }
    }
}

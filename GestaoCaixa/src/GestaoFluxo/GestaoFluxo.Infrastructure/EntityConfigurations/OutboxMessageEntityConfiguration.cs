using GestaoFluxo.Application.Interfaces.OutboxService;
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
    internal class OutboxMessageEntityConfiguration : IEntityTypeConfiguration<OutboxMessage>
    {
        public void Configure(EntityTypeBuilder<OutboxMessage> builder)
        {
            builder.ToTable("outbox_messages");

            builder.Property(s => s.Id)
                .HasColumnName("id")
                .ValueGeneratedOnAdd();

            builder.HasKey(s => s.Id);

            builder.Property(s => s.TipoEvento)
                .HasColumnName("tipo_evento")
                .HasMaxLength(40)
                .IsRequired();

            builder.Property(s => s.Payload)
                .HasColumnName("payload")
                .HasColumnType("JSON")
                .IsRequired();

            builder.Property(s => s.Destino)
                .HasColumnName("destino")
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(s => s.DataCriacao)
                .HasColumnName("data_criacao")
                .HasDefaultValueSql("(UTC_TIMESTAMP(3))")
                .IsRequired();

            builder.Property(s => s.DataProcessamento)
                .HasColumnName("data_processamento");

            builder.HasIndex(s => new
            {
                s.DataProcessamento,
                s.DataCriacao
            });
        }
    }
}

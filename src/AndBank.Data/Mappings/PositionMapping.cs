using AndBank.Processs.Aplication;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AndBank.Process.Data.Mappings
{
    public class PositionMapping : IEntityTypeConfiguration<PositionModel>
    {
        public void Configure(EntityTypeBuilder<PositionModel> builder)
        {
            builder.HasKey(c => c.Id);

            builder.Property<Guid>("Id")
                .HasColumnType("uuid");

            builder.Property<string>("PositionId")
                .HasColumnType("text");

            builder.Property<DateTime>("Date")
                .HasColumnType("timestamp without time zone");

            builder.Property<string>("ClientId")
                .HasColumnType("text");

            builder.Property<string>("ProductId")
                .HasColumnType("text");

            builder.Property<decimal>("Quantity")
                .HasColumnType("numeric");

            builder.Property<decimal>("Value")
                .HasColumnType("numeric");

            builder.ToTable("Positions");
        }
    }
}

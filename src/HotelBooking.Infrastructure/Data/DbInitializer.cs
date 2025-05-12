using Microsoft.EntityFrameworkCore;

namespace HotelBooking.Infrastructure.Data
{
    public class DbInitializer
    {
        private readonly ModelBuilder modelBuilder;
        public DbInitializer(ModelBuilder modelBuilder)
        {
            this.modelBuilder = modelBuilder;
        }

        public void Seed()
        {
        }
    }
}

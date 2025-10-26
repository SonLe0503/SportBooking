using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using SportBooking.Models;

namespace SportBooking
{
    public class SportBookingDbContextFactory : IDesignTimeDbContextFactory<SportBookingDbContext>
    {
        public SportBookingDbContext CreateDbContext(string[] args)
        {
            // Lấy đường dẫn thư mục hiện tại
            string basePath = Directory.GetCurrentDirectory();

            // Đọc file appsettings.json
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile("appsettings.json") // Đảm bảo file này có trong thư mục project
                .Build();

            // Bắt đầu cấu hình DbContext
            var builder = new DbContextOptionsBuilder<SportBookingDbContext>();

            // Lấy chuỗi kết nối tên "MyCnn" (giống trong file appsettings.json)
            var connectionString = configuration.GetConnectionString("MyCnn");

            // Báo cho DbContext dùng SQL Server với chuỗi kết nối này
            builder.UseSqlServer(connectionString);

            // Trả về một DbContext mới
            return new SportBookingDbContext(builder.Options);
        }
    }
}

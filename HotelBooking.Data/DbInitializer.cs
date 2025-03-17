using HotelBooking.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace HotelBooking.Data
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
        //    modelBuilder.Entity<UserModel>().HasData(
        //       new UserModel()
        //       {
        //           Id = 1,

        //           FirstName = "Admin",

        //           LastName = "Dat",

        //           Email = "admin@com.com",

        //           PhoneNo = "0335487991",

        //           PasswordHash = BCrypt.Net.BCrypt.HashPassword("123ABCabc!"),

        //           Address = "Hue",
        //           IsVerified = true,
        //           Avatar = "https://i.pinimg.com/originals/27/0c/be/270cbeb639f6d0ed64f3106daaeccc1d.jpg"

        //       },
        //       new UserModel()
        //       {
        //           Id = 2,

        //           FirstName = "Fawnia",

        //           LastName = "Alexandros",

        //           Email = "apeacocke1@google.ca",

        //           PhoneNo = "0354579415",

        //           PasswordHash = BCrypt.Net.BCrypt.HashPassword("123ABCabc!"),

        //           Address = "Hue",
        //           IsVerified = true,
        //           Avatar = "https://th.bing.com/th/Id/OIP.MbAE6jT-VnWsVr9ANrkYNwHaKw?rs=1&pId=ImgDetMain"


        //       },
        //       new UserModel()

        //       {

        //           Id = 3,

        //           FirstName = "Cazzie",

        //           LastName = "Pancoast",

        //           Email = "cpancoast2@wsj.com",

        //           PhoneNo = "0354596415",

        //           PasswordHash = BCrypt.Net.BCrypt.HashPassword("123ABCabc!"),

        //           Address = "Hue",
        //           IsVerified = true,
        //           Avatar = "https://th.bing.com/th/Id/R.47e446cab641c16b2a6f8c9db520ee19?rik=0vd8lTgoDpgUzQ&pId=ImgRaw&r=0"

        //       },
        //       new UserModel()

        //       {

        //           Id = 4,

        //           FirstName = "Marri",

        //           LastName = "Dat",

        //           Email = "datmarri@google.ca",

        //           PhoneNo = "0354579415",

        //           PasswordHash = BCrypt.Net.BCrypt.HashPassword("123ABCabc!"),

        //           Address = "Hue",
        //           IsVerified = true,
        //           Avatar = "https://th.bing.com/th/Id/OIP.-Xu8PRNaVrwqZ1J-f4E16wHaHa?w=1200&h=1200&rs=1&pId=ImgDetMain"

        //       },
        //       new UserModel()

        //       {

        //           Id = 5,

        //           FirstName = "Dat",

        //           LastName = "khong chin",

        //           Email = "datkhongchin@google.ca",

        //           PhoneNo = "0354579415",

        //           PasswordHash = BCrypt.Net.BCrypt.HashPassword("123ABCabc!"),

        //           Address = "Hue",
        //           IsVerified = true,
        //           Avatar = "https://th.bing.com/th/Id/OIP.-q9JRr6eAAyyBL3s-3g1PgHaKt?rs=1&pId=ImgDetMain"

        //       }
        //);
        //    modelBuilder.Entity<RoleModel>().HasData(
        //           new RoleModel()

        //           {

        //               RoleId = 1,

        //               RoleName = "Admin",

        //               RoleDesc = "manage it all"

        //           },
        //           new RoleModel()

        //           {

        //               RoleId = 2,

        //               RoleName = "Job Seeker",

        //               RoleDesc = "They are people looking for work"

        //           },
        //           new RoleModel()

        //           {

        //               RoleId = 3,

        //               RoleName = "Job Giver",

        //               RoleDesc = "They are someone who goes to work"

        //           }
        //    );
        //    modelBuilder.Entity<UserRoleModel>().HasData(
        //           new UserRoleModel()

        //           {

        //               id = 1,

        //               role_id = 1,

        //               user_id = 1,

        //           },
        //           new UserRoleModel()

        //           {

        //               id = 2,

        //               role_id = 2,

        //               user_id = 2,

        //           },
        //           new UserRoleModel()
        //           {

        //               id = 3,

        //               role_id = 3,

        //               user_id = 3,

        //           },
        //           new UserRoleModel()
        //           {

        //               id = 4,

        //               role_id = 2,

        //               user_id = 4,
        //           },
        //           new UserRoleModel()
        //           {
        //               id = 5,

        //               role_id = 3,

        //               user_id = 5,
        //           }
        //    );
        //    modelBuilder.Entity<NotificationModel>().HasData(
        //          new NotificationModel()
        //          {
        //              id = 1,
        //              notifi_content = "node ",
        //              is_accept = false,
        //              status = false,
        //              from_user_notifi_id = 2,
        //          },
        //          new NotificationModel()
        //          {
        //              id = 2,
        //              notifi_content = "node",
        //              is_accept = false,
        //              status = false,
        //              from_user_notifi_id = 3,
        //          },
        //          new NotificationModel()
        //          {
        //              id = 3,
        //              notifi_content = "node",
        //              is_accept = false,
        //              status = false,
        //              from_user_notifi_id = 4,

        //          });
        //    modelBuilder.Entity<FileModel>().HasData(
        //          new FileModel()
        //          {
        //              id = 1,

        //              name = "anh datvila",

        //              type = "jpg",

        //              url = "https://media.2dep.vn/upload/thucquyen/2022/05/19/dat-villa-la-ai-hot-tiktoker-9x-trieu-view-co-chuyen-tinh-xuyen-bien-gioi-voi-ban-gai-indonesia-social-1652941149.jpg",

        //          },
        //          new FileModel()

        //          {

        //              id = 2,

        //              name = "anh thong soai ca",

        //              type = "jpg",

        //              url = "https://newsmd2fr.keeng.vn/tiin/archive/imageslead/2023/06/14/90_c373d5ac0433257417f21a0a5e07fa11.jpg",

        //          },
        //          new FileModel()
        //          {
        //              id = 3,

        //              name = "justin",

        //              type = "mp4",

        //              url = "https://vIdeos.pexels.com/vIdeo-files/6698049/6698049-uhd_3840_2160_25fps.mp4",
        //          },
        //          new FileModel()
        //          {
        //              id = 4,

        //              name = "guiboss",

        //              type = "jpg",

        //              url = "https://cdn.eva.vn/upload/2-2020/images/2020-05-05/don-xin-viec-ba-dao-2-1588672247-395-wIdth1214height806.jpg",
        //          },
        //          new FileModel()
        //          {
        //              id = 5,

        //              name = "Xinnn",

        //              type = "jpg",

        //              url = "https://cdn.eva.vn/upload/2-2020/images/2020-05-05/don-xin-viec-ba-dao-1-1588672247-922-wIdth610height813.jpg",
        //          },
        //          new FileModel()
        //          {
        //              id = 6,

        //              name = "Xinviecti",

        //              type = "mp4",

        //              url = "https://vIdeos.pexels.com/vIdeo-files/2084066/2084066-hd_1920_1080_24fps.mp4",
        //          },
        //          new FileModel()
        //          {
        //              id = 7,

        //              name = "Diemyeu",

        //              type = "mp4",

        //              url = "https://vIdeos.pexels.com/vIdeo-files/2795169/2795169-uhd_3840_2160_25fps.mp4",
        //          },
        //          new FileModel()
        //          {
        //              id = 8,

        //              name = "DungDai",

        //              type = "mp4",

        //              url = "https://vIdeos.pexels.com/vIdeo-files/3959712/3959712-uhd_4096_2160_25fps.mp4",
        //          }
        //    );
        //    modelBuilder.Entity<PostModel>().HasData(
        //        new PostModel()
        //        {
        //            id = 1,
        //            caption = "Bạn là một nhiếp ảnh gia tài năng và đam mê việc ghi lại những khoảnh khắc độc đáo dưới nước? Chúng tôi đang tìm kiếm những nhà ảnh độc lập để ghi lại cuộc sống đa dạng dưới biển.",
        //            user_id = 3,
        //            job_id = 3,
        //            file_id = 3,
        //            title = "Nhà ảnh độc lập từ xa",
        //            created_at = new DateTime(2024, 4, 29)
        //        }
        //    );
        }
    }
}

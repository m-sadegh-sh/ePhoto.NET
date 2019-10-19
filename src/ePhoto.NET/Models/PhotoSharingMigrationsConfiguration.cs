using System.Data.Entity.Migrations;

namespace ePhoto.NET.Models {
    public class PhotoSharingMigrationsConfiguration : DbMigrationsConfiguration<PhotoSharingContext> {
        public PhotoSharingMigrationsConfiguration() {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = false;
        }
    }
}
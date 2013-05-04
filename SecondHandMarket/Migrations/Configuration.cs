namespace SecondHandMarket.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Diagnostics;
    using System.Linq;
    using System.Web.Security;

    internal sealed class Configuration : DbMigrationsConfiguration<SecondHandMarket.Models.MarketContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(SecondHandMarket.Models.MarketContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //Ìí¼Ó½ÇÉ«
            CreateDefaultRoles();

            CreateDefaultUsers();
            //
        }

        private void CreateDefaultUsers()
        {
            if (Membership.GetUser("admin") == null)
            {
                MembershipCreateStatus status;
                var user = Membership.CreateUser("admin", "admin123",
                    "secondhandmarket@163.com", null, null, true, out status);

                Roles.AddUserToRole("admin", "Administrator");
            }
        }

        private void CreateDefaultRoles()
        {
            if (!Roles.RoleExists("Administrator"))
            {
                Roles.CreateRole("Administrator");
            }
        }
    }
}

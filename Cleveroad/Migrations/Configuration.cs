namespace Cleveroad.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Data.Entity.SqlServer;
    using System.Globalization;
    using System.Linq;

    internal class SqlServerMigrationSqlGeneratorFixed : SqlServerMigrationSqlGenerator
    {
        protected override string Generate(DateTime defaultValue)
        {
            var value = defaultValue.Ticks != 0 ?
                defaultValue.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture) :
                "1753-01-01 00:00:00";

            return string.Format("'{0}'", value);
        }
    }

    internal sealed class Configuration : DbMigrationsConfiguration<Cleveroad.Models.OrdersContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(Cleveroad.Models.OrdersContext context)
        {
        }
    }
}

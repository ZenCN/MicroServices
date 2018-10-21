using DbAccess.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbAccess
{
    public class MicroSvrDb : DbContext
    {
        /* Enable-Migrations -ContextTypeName DbAccess.MicroSvrDb -MigrationsDirectory Migrations.MicroSvrDb -Force
         * Add-Migration 名字 -ConfigurationTypeName DbAccess.Migrations.MicroSvrDb.Configuration
         * Update-Database -ConfigurationTypeName DbAccess.Migrations.MicroSvrDb.Configuration
         */
        public MicroSvrDb()
           : base("name=MicroSvrDb")
        {

        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            /*用Entity Framework的 Fluent API方式配置Code First 指定数据库对应的表名、主键，也可以用Data Annotation注解方式设置映射约定
             * 使用Data Annotation注解方式，当数据库发生变化时，可不用做Code First数据库迁移
             */
            modelBuilder.Entity<Vehicle>().ToTable("VehicleInStock", "dbo");
        }

        public DbSet<Vehicle> Vehicles { get; set; }
    }
}

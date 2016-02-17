using NHibernateRepo.Migrations;
using System;

namespace ExampleRepo.Migrations.ExampleRepoMigrations
{
    public class Migration_20151125213730_Initial_Setup_Models : BaseMigration<ExampleRepo>
    {
        public override void Execute()
        {
            
            ExecuteSql(@"
    create table BankAccounts (
        Id INT IDENTITY NOT NULL,
       Name NVARCHAR(255) null,
       Balance DECIMAL(19,5) null,
       CustomerEntity_id INT null,
       primary key (Id)
    )");
            
            ExecuteSql(@"
    create table [BankEntity] (
        Id INT IDENTITY NOT NULL,
       Name NVARCHAR(255) null,
       Town NVARCHAR(255) null,
       primary key (Id)
    )");
            
            ExecuteSql(@"
    create table [CustomerEntity] (
        Id INT IDENTITY NOT NULL,
       Name NVARCHAR(255) null,
       BankEntity_id INT null,
       primary key (Id)
    )");
            
            ExecuteSql(@"
    alter table BankAccounts 
        add constraint FK31C68F0339C89602 
        foreign key (CustomerEntity_id) 
        references [CustomerEntity]");
            
            ExecuteSql(@"
    alter table [CustomerEntity] 
        add constraint FK3447876EB570371E 
        foreign key (BankEntity_id) 
        references [BankEntity]");
            
        }
    }
}

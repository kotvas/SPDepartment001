using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using SPDepartment001.Models.DB;

namespace SPDepartment001.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20170521163431_Initial")]
    partial class Initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.0.0-rtm-21431")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("SPDepartment001.Models.DepartmentEvent", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<decimal>("AmountOfEmployee");

                    b.Property<bool>("AreExpensesGenerated");

                    b.Property<DateTime>("DateCreated");

                    b.Property<DateTime>("DateOfEvent");

                    b.Property<int>("EmployeeId");

                    b.HasKey("Id");

                    b.HasIndex("EmployeeId");

                    b.ToTable("DepartmentEvents");
                });

            modelBuilder.Entity("SPDepartment001.Models.Deposit", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<decimal>("Amount");

                    b.Property<DateTime>("Date");

                    b.Property<int>("EmployeeId");

                    b.HasKey("Id");

                    b.HasIndex("EmployeeId");

                    b.ToTable("Deposits");
                });

            modelBuilder.Entity("SPDepartment001.Models.Employee", b =>
                {
                    b.Property<int>("EmployeeID")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid>("AssociatedUserId");

                    b.Property<DateTime>("DateOfBirth");

                    b.Property<string>("FirstName")
                        .IsRequired();

                    b.Property<bool>("IsActive");

                    b.Property<string>("LastName")
                        .IsRequired();

                    b.HasKey("EmployeeID");

                    b.ToTable("Employees");
                });

            modelBuilder.Entity("SPDepartment001.Models.EmployeeAccount", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<decimal>("Amount");

                    b.Property<DateTime>("DateOfLastUpdate");

                    b.Property<int>("EmployeeId");

                    b.HasKey("Id");

                    b.HasIndex("EmployeeId");

                    b.ToTable("EmployeesAccounts");
                });

            modelBuilder.Entity("SPDepartment001.Models.Expense", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<decimal>("Amount");

                    b.Property<DateTime>("DateCreated");

                    b.Property<DateTime?>("DateOfPayment");

                    b.Property<Guid>("DepartmentEventId");

                    b.Property<int?>("EmployeeId");

                    b.Property<bool>("IsPaid");

                    b.HasKey("Id");

                    b.HasIndex("DepartmentEventId");

                    b.HasIndex("EmployeeId");

                    b.ToTable("Expenses");
                });

            modelBuilder.Entity("SPDepartment001.Models.DepartmentEvent", b =>
                {
                    b.HasOne("SPDepartment001.Models.Employee", "Employee")
                        .WithMany()
                        .HasForeignKey("EmployeeId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("SPDepartment001.Models.Deposit", b =>
                {
                    b.HasOne("SPDepartment001.Models.Employee", "Employee")
                        .WithMany()
                        .HasForeignKey("EmployeeId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("SPDepartment001.Models.EmployeeAccount", b =>
                {
                    b.HasOne("SPDepartment001.Models.Employee", "Employee")
                        .WithMany()
                        .HasForeignKey("EmployeeId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("SPDepartment001.Models.Expense", b =>
                {
                    b.HasOne("SPDepartment001.Models.DepartmentEvent", "DepartmentEvent")
                        .WithMany()
                        .HasForeignKey("DepartmentEventId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("SPDepartment001.Models.Employee", "Employee")
                        .WithMany()
                        .HasForeignKey("EmployeeId");
                });
        }
    }
}

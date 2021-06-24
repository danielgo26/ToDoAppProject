using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ToDoAppAgain.Data.Model;

namespace ToDoAppAgain.Data
{
    public class ToDoContext : DbContext
    {
        public ToDoContext()
        {
            Database.EnsureCreated();
        }

        public DbSet<User> Users  { get; set; }
        public DbSet<ToDoList> ToDoLists { get; set; }
        public DbSet<Task> Tasks { get; set; }
        public DbSet<UserToDoList> UsersToDoLists { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string connectionString = "Server=(localdb)\\MSSQLLocalDB;Initial Catalog=ToDoAppDb;Integrated Security=True";
            optionsBuilder.UseSqlServer(connectionString);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // mapping between User and ToDoList
            modelBuilder.Entity<UserToDoList>().HasKey(utdl => new { utdl.UserId, utdl.ToDoListId });

            modelBuilder.Entity<UserToDoList>()
            .HasOne(utdl => utdl.User)
            .WithMany(u => u.UsersToDoLists)
            .HasForeignKey(utdl => utdl.UserId)
            .OnDelete(DeleteBehavior.ClientCascade);

            modelBuilder.Entity<UserToDoList>()
            .HasOne(utdl => utdl.ToDoList)
            .WithMany(tdl => tdl.UsersToDoLists)
            .HasForeignKey(utdl => utdl.ToDoListId)
            .OnDelete(DeleteBehavior.ClientCascade);

            //ToDoList -> User
            modelBuilder.Entity<ToDoList>()
                .HasOne(tdl => tdl.UserMadeLastChangeOfTheList)
                .WithMany(u => u.ToDoListsMadeLastChange)
                .HasForeignKey(tdl => tdl.UserMadeLastChangeOfTheListId)
                .OnDelete(DeleteBehavior.ClientCascade);

            modelBuilder.Entity<ToDoList>()
                .HasOne(tdl => tdl.CreatorOfTheList)
                .WithMany(u => u.ToDoListsCreated)
                .HasForeignKey(tdl => tdl.CreatorOfTheListId)
                .OnDelete(DeleteBehavior.ClientCascade);

            //Task -> ToDoList
            modelBuilder.Entity<Task>()
                .HasOne(t => t.ListOfTask)
                .WithMany(tdl => tdl.TasksOfTheList)
                .HasForeignKey(t => t.ListOfTaskId)
                .OnDelete(DeleteBehavior.ClientCascade);
            
            // Task -> User
            modelBuilder.Entity<Task>()
                .HasOne(t => t.CreatorOfTheTask)
                .WithMany(u => u.TasksCreated)
                .HasForeignKey(t => t.CreatorOfTheTaskId)
                .OnDelete(DeleteBehavior.ClientCascade);

            modelBuilder.Entity<Task>()
                .HasOne(t => t.UserMadeLastChangeOfTheTask)
                .WithMany(u => u.TasksLastChanged)
                .HasForeignKey(t => t.UserMadeLastChangeOfTheTaskId)
                .OnDelete(DeleteBehavior.ClientCascade);

            modelBuilder.Entity<Task>()
                .HasOne(t => t.UserAssignedToTheTask)
                .WithMany(u => u.TaskAssigned)
                .HasForeignKey(t => t.UserAssignedToTheTaskId)
                .OnDelete(DeleteBehavior.ClientCascade);


            // mapping the self-relating in the user class
            modelBuilder.Entity<User>()
             .HasOne(e => e.UserCreator)
             .WithMany()
             .HasForeignKey(m => m.UserCreatorId);

            modelBuilder.Entity<User>()
             .HasOne(e => e.UserMadeLastChange)
             .WithMany()
             .HasForeignKey(m => m.UserMadeLastChangeId);
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoAppAgain.Data.Model
{
    [Table("Users")]
    public class User
    {
        public User(string username, string password, string firstName, string lastName, bool isAdmin)
        {
            Username = username;
            Password = password;
            FirstName = firstName;
            LastName = lastName;
            IsAdmin = isAdmin;
            DateCreating = DateTime.Now;
            DateOfTheLastChange = DateTime.Now;
        }

        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool IsAdmin { get; set; }
        public DateTime DateCreating { get; set; }
        public int? UserCreatorId { get; set; }
        public virtual User UserCreator { get; set; }
        public DateTime DateOfTheLastChange { get; set; }
        public int? UserMadeLastChangeId { get; set; }
        public virtual User UserMadeLastChange { get; set; }
        public List<ToDoList> ToDoListsCreated { get; set; }
        public List<ToDoList> ToDoListsMadeLastChange { get; set; }
        public List<Task> TasksCreated { get; set; }
        public List<Task> TasksLastChanged { get; set; }
        public List<Task> TaskAssigned { get; set; }
        public IList<UserToDoList> UsersToDoLists { get; set; }

        public override string ToString()
        {
            return $"Id:{this.Id} - Name: {this.FirstName} {this.LastName} \n" +
                $"Username:{this.Username}, Password:{this.Password} \n" +
                $"AdminRights: {this.IsAdmin} \n" +
                $"Date of creating the user: {this.DateCreating} \n" +
                $"Date of last change: {this.DateOfTheLastChange}";
        }

    }
}

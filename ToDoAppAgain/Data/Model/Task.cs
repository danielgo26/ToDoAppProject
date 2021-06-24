using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoAppAgain.Data.Model
{
    [Table("Tasks")]
    public class Task
    {
        public Task(string title, string description, int listOfTaskId, int creatorOfTheTaskId, int userMadeLastChangeOfTheTaskId)
        {
            Title = title;
            Description = description;
            ListOfTaskId = listOfTaskId;
            CreatorOfTheTaskId = creatorOfTheTaskId;
            UserMadeLastChangeOfTheTaskId = userMadeLastChangeOfTheTaskId;
            IsCompleted = false;
            IsAssigned = false;
            DateCreating = DateTime.Now;
            DateLastChange = DateTime.Now;
        }

        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsCompleted { get; set; }
        public bool IsAssigned { get; set; }
        public int ListOfTaskId { get; set; }
        public ToDoList ListOfTask { get; set; }
        public DateTime DateCreating { get; set; }
        public int CreatorOfTheTaskId { get; set; }
        public User CreatorOfTheTask { get; set; }
        public DateTime DateLastChange { get; set; }
        public int UserMadeLastChangeOfTheTaskId { get; set; }
        public User UserMadeLastChangeOfTheTask { get; set; }
        public int? UserAssignedToTheTaskId { get; set; }
        public User UserAssignedToTheTask { get; set; }

        public override string ToString()
        {
            return $"Title: {this.Title} \n" +
                $"Description {this.Description}\n" +
                $"Is Completed -> {this.IsCompleted}\n" +
                $"Date of the creting: {this.DateCreating}\n" +
                $"Date of the last change: {this.DateLastChange}";
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoAppAgain.Data.Model
{
    [Table("ToDoLists")]
    public class ToDoList
    {
        public ToDoList(string title, int creatorOfTheListId, int userMadeLastChangeOfTheListId)
        {
            Title = title;
            CreatorOfTheListId = creatorOfTheListId;
            UserMadeLastChangeOfTheListId = userMadeLastChangeOfTheListId;
            DateCreating = DateTime.Now;
            DateLastChange = DateTime.Now;
        }

        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime DateCreating { get; set; }
        public DateTime DateLastChange { get; set; }
        public int CreatorOfTheListId { get; set; }
        public User CreatorOfTheList { get; set; }
        public int UserMadeLastChangeOfTheListId { get; set; }
        public User UserMadeLastChangeOfTheList { get; set; }
        public List<Task> TasksOfTheList { get; set; }
        public IList<UserToDoList> UsersToDoLists { get; set; }

        public override string ToString()
        {
            return $"Title: {this.Title} \n" +
                $"Date of the creting: {this.DateCreating} \n" +
                $"Date of the last change: {this.DateLastChange}"; 
        }
    }
}

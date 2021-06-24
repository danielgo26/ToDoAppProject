using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoAppAgain.Data.Model
{
    [Table("UsersToDoLists")]
    public class UserToDoList
    {
        public int UserId { get; set; }
        public User User { get; set; }
        public int ToDoListId { get; set; }
        public ToDoList ToDoList { get; set; }
        public UserToDoList(int userId, int toDoListId)
        {
            UserId = userId;
            ToDoListId = toDoListId;
        }
    }
}

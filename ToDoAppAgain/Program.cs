using System;
using ToDoAppAgain.Controller;
using ToDoAppAgain.Data;
using ToDoAppAgain.Data.Model;
using ToDoAppAgain.View;

namespace ToDoAppAgain
{
    class Program
    {
        static void Main(string[] args)
        {
            ToDoAppController toDoAppController = new ToDoAppController();
            if (!toDoAppController.CheckIsFulled()) // fill the root admin
            {
                User firstAdmin = new User("admin", "adminpass", "Dimitar", "Georgiev", true);
                toDoAppController.Add(firstAdmin);
            }

            Presentation pr = new Presentation();
            pr.Engine();

        }
    }
}

//Console.WriteLine("Nice");
            //ToDoContext tdc;
            //using (tdc = new ToDoContext())
            //{
            //    User user1 = new User("Terminatora", "az123", "Ivan", "Ivanov", true);
            //    tdc.Users.Add(user1);
            //    tdc.SaveChanges();

            //    ToDoList toDoList1 = new ToDoList("Chores", 1, 1);
            //    ToDoList toDoList2 = new ToDoList("Chores2", 1, 1);
            //    tdc.ToDoLists.Add(toDoList1);
            //    tdc.ToDoLists.Add(toDoList2);
            //    tdc.SaveChanges();

            //    Task task1 = new Task("Laundry","Hard", 1, 1, 1);
            //    Task task2 = new Task("Hoovering", "Medium", 2, 1, 1);
            //    tdc.Tasks.Add(task1);
            //    tdc.Tasks.Add(task2);
            //    tdc.SaveChanges();
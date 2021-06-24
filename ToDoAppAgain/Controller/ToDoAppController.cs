using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using System.Threading.Tasks;
using ToDoAppAgain.Data;
using ToDoAppAgain.Data.Model;

namespace ToDoAppAgain.Controller
{
    public class ToDoAppController
    {
        private ToDoContext toDoContext;
        public bool CheckIsFulled()
        {
            using (toDoContext = new ToDoContext())
            {
                return toDoContext.Users.Any();
            }
        }

        public void Add(object obj)
        {
            using (toDoContext = new ToDoContext())
            {
                if (obj.GetType() == typeof(User))
                {
                    toDoContext.Users.Add((User)obj);
                    toDoContext.SaveChanges();
                }
                else if (obj.GetType() == typeof(ToDoList))
                {
                    toDoContext.ToDoLists.Add((ToDoList)obj);
                    toDoContext.SaveChanges();
                }
                else if (obj.GetType() == typeof(Task))
                {
                    toDoContext.Tasks.Add((Task)obj);
                    toDoContext.SaveChanges();
                }
                else if (obj.GetType() == typeof(UserToDoList))
                {
                    toDoContext.UsersToDoLists.Add((UserToDoList)obj);
                    toDoContext.SaveChanges();
                }
            }
        }
        public User GetUser(string username, string pass)
        {
            using (toDoContext = new ToDoContext())
            {
                return toDoContext.Users
                .Where(u => (u.Username == username && u.Password == pass)).FirstOrDefault();
            }
        }
        public User GetUser(string username) // overload
        {
            using (toDoContext = new ToDoContext())
            {
                return toDoContext.Users
                .Where(u => u.Username == username).FirstOrDefault();
            }
        }
        public User GetUser(int id) // overload
        {
            using (toDoContext = new ToDoContext())
            {
                return toDoContext.Users
                .Where(u => u.Id == id).FirstOrDefault();
            }
        }
        public Task GetTask(string title) // overload
        {
            using (toDoContext = new ToDoContext())
            {
                return toDoContext.Tasks
                .Where(t => t.Title == title).FirstOrDefault();
            }
        }
        public List<User> GetAllUsers()
        {
            using (toDoContext = new ToDoContext())
            {
                return toDoContext.Users.ToList();
            }
        }
        public ToDoList GetToDoList(int toDoListId)
        {
            using (toDoContext = new ToDoContext())
            {
                return toDoContext.ToDoLists.Find(toDoListId);
            }
        }
        public ToDoList GetToDoList(string title)
        {
            using (toDoContext = new ToDoContext())
            {
                return toDoContext.ToDoLists.Where(tdl => tdl.Title == title).FirstOrDefault();
            }
        }
        public List<ToDoList> GetAllToDoListsOfTheUser(int userId)
        {
            using (toDoContext = new ToDoContext())
            {
                //return toDoContext.ToDoLists.
                //    Where(tdl => tdl.Id == toDoContext.UsersToDoLists
                //    .Where(utdl => utdl.UserId == userId).FirstOrDefault().ToDoListId).ToList();
                List<ToDoList> toDoLists = new List<ToDoList>();
                foreach (var utdl in toDoContext.UsersToDoLists)
                {
                    if (utdl.UserId == userId)
                    {
                        toDoLists.Add(GetToDoList(utdl.ToDoListId));
                    }
                }
                return toDoLists;
            }
        }

        public List<User> GetAllUsersOfTheToDoList(int toDoListId)
        {
            using (toDoContext = new ToDoContext())
            {
                List<User> users = new List<User>();
                foreach (var utdl in toDoContext.UsersToDoLists)
                {
                    if (utdl.ToDoListId == toDoListId)
                    {
                        users.Add(GetUser(utdl.UserId));
                    }
                }
                return users;
            }
        }

        public List<Task> GetAllTasksOfTheToDoList(int toDoListId)
        {
            using (toDoContext = new ToDoContext())
            {
                List<Task> tasks = new List<Task>();
                foreach (var t in toDoContext.Tasks)
                {
                    if (t.ListOfTaskId == toDoListId)
                    {
                        tasks.Add(t);
                    }
                }
                return tasks;
            }
        }

        public List<Task> GetAllUnAssignedTasksOfTheToDoList(int toDoListId)
        {
            using (toDoContext = new ToDoContext())
            {
                List<Task> tasks = new List<Task>();
                foreach (var t in toDoContext.Tasks)
                {
                    if (t.ListOfTaskId == toDoListId)
                    {
                        if (!t.IsAssigned)
                        {
                            tasks.Add(t);
                        }
                    }
                }
                return tasks;
            }
        }

        public List<Task> GetAllAssignedTasksOfTheUser(int userId)
        {
            using (toDoContext = new ToDoContext())
            {
                List<Task> tasks = new List<Task>();
                foreach (var t in toDoContext.Tasks)
                {
                    if (t.UserAssignedToTheTaskId == userId)
                    {
                        if (t.IsAssigned)
                        {
                            tasks.Add(t);
                        }
                    }
                }
                return tasks;
            }
        }

        public void UpdateUser(User user, string username, string password, string firstName, string lastName, int idOfTheChanger)
        {
            using (toDoContext = new ToDoContext())
            {
                User item = toDoContext.Users.Find(user.Id);
                if (item != null)
                {
                    item.Username = username;
                    item.Password = password;
                    item.FirstName = firstName;
                    item.LastName = lastName;
                    item.UserMadeLastChangeId = idOfTheChanger;
                    item.DateOfTheLastChange = DateTime.Now;
                    toDoContext.SaveChanges();
                }
            }
        }

        public void UpdateToDoList(ToDoList toDoList, string title, int idOfTheChanger)
        {
            using (toDoContext = new ToDoContext())
            {
                ToDoList item = toDoContext.ToDoLists.Find(toDoList.Id);
                if (item != null)
                {
                    item.Title = title;
                    item.UserMadeLastChangeOfTheListId = idOfTheChanger;
                    item.DateLastChange = DateTime.Now;
                    toDoContext.SaveChanges();
                }
            }
        }

        public void UpdateTask(Task task, string title, string description, int idOfTheChanger)
        {
            using (toDoContext = new ToDoContext())
            {
                Task item = toDoContext.Tasks.Find(task.Id);
                if (item != null)
                {
                    item.Title = title;
                    item.Description = description;
                    item.UserMadeLastChangeOfTheTaskId = idOfTheChanger;
                    item.DateLastChange = DateTime.Now;
                    toDoContext.SaveChanges();
                }
            }
        }

        public void DeleteTask(string title)
        {
            using (toDoContext = new ToDoContext())
            {
                foreach (var t in toDoContext.Tasks)
                {
                    if (t.Title == title)
                    {
                        toDoContext.Tasks.Remove(t);
                    }
                }
                toDoContext.SaveChanges();
            }
        }

        public void CompleteTask(Task task, int idOfTheChanger)
        {
            using (toDoContext = new ToDoContext())
            {
                Task item = toDoContext.Tasks.Find(task.Id);
                if (item != null)
                {
                    item.IsCompleted = true;
                    item.UserMadeLastChangeOfTheTaskId = idOfTheChanger;
                    item.DateLastChange = DateTime.Now;
                    toDoContext.SaveChanges();
                }
            }
        }

        public void DeleteUser(string username) // TODO: Delete the relations with ToDoLists of the user and Tasks of the user
        {
            //User item = toDoContext.Users.Find(user.Id);
            User item = this.GetUser(username);
            using (toDoContext = new ToDoContext())
            {
                if (item != null)
                {
                    // clear the user from the ToDoLists table
                    foreach (var toDoList1 in toDoContext.ToDoLists)
                    {
                        if (item.Id == toDoList1.CreatorOfTheListId)
                        {
                            toDoList1.CreatorOfTheListId = 1; // root admin
                            bool toBeAdded = true;
                            using (ToDoContext toDoContext2 = new ToDoContext())
                            {
                                foreach (var utdl in toDoContext2.UsersToDoLists)
                                {
                                    if (utdl.UserId == toDoList1.CreatorOfTheListId && utdl.ToDoListId == toDoList1.Id)
                                    {
                                        toBeAdded = false;
                                        break;
                                    }
                                }
                                if (toBeAdded)
                                {
                                    UserToDoList userToDoList = new UserToDoList(toDoList1.CreatorOfTheListId, toDoList1.Id);
                                    toDoContext2.UsersToDoLists.Add(userToDoList);
                                }
                            }
                        }
                        if (item.Id == toDoList1.UserMadeLastChangeOfTheListId)
                        {
                            toDoList1.UserMadeLastChangeOfTheListId = toDoList1.CreatorOfTheListId; // root admin
                        }

                    }
                    // clear the user from the Tasks table
                    foreach (var task1 in toDoContext.Tasks)
                    {
                        if (item.Id == task1.CreatorOfTheTaskId)
                        {
                            task1.CreatorOfTheTaskId = 1; // root admin
                        }
                        if (item.Id == task1.UserMadeLastChangeOfTheTaskId)
                        {
                            task1.UserMadeLastChangeOfTheTaskId = task1.CreatorOfTheTaskId; // root admin
                        }
                    }
                    // clear the user from the UsersToDoLists table
                    foreach (var userToDoList in toDoContext.UsersToDoLists)
                    {
                        if (userToDoList.UserId == item.Id)
                        {
                            toDoContext.UsersToDoLists.Remove(userToDoList);
                        }
                    }
                    toDoContext.Users.Remove(item);
                    toDoContext.SaveChanges();
                }
            }
        }

        public void DeleteToDoList(string toDoListTitle, User currentUser)
        {
            ToDoList toDoList = this.GetToDoList(toDoListTitle);
            using (toDoContext = new ToDoContext())
            {
                if (toDoList != null)
                {
                    if (currentUser.Id == toDoList.CreatorOfTheListId) // currentUser == creator of the list
                    {
                        foreach (var userToDoList in toDoContext.UsersToDoLists)
                        {
                            if (userToDoList.ToDoListId == toDoList.Id)
                            {
                                toDoContext.UsersToDoLists.Remove(userToDoList);
                            }
                        }
                        toDoContext.ToDoLists.Remove(toDoList);
                    }

                    else
                    {
                        foreach (var userToDoList in toDoContext.UsersToDoLists) // current user != creator of th list
                        {
                            if (userToDoList.ToDoListId == toDoList.Id && userToDoList.UserId == currentUser.Id)
                            {
                                toDoContext.UsersToDoLists.Remove(userToDoList);
                                break;
                            }
                        }
                    }
                    toDoContext.SaveChanges();
                }
            }
        }

        public void ShareToDoList(List<User> users, ToDoList toDoListToBeShared)
        {
            using (toDoContext = new ToDoContext())
            {
                foreach (var u in users)
                {
                    bool canBeShared = true;
                    foreach (var utdl in toDoContext.UsersToDoLists)
                    {
                        if (u.Id == utdl.UserId && toDoListToBeShared.Id == utdl.ToDoListId)
                        {
                            Console.WriteLine($"{u.Username} is already shared to this To Do List!");
                            canBeShared = false;
                            break;
                        }
                    }
                    if (canBeShared)
                    {
                        UserToDoList userToDoList = new UserToDoList(u.Id, toDoListToBeShared.Id);
                        this.Add(userToDoList);
                    }
                }
            }
        }

        public void AssignTaskToUser(int userId, int taskId)
        {
            using (toDoContext = new ToDoContext())
            {
                Task taskItem = toDoContext.Tasks.Find(taskId);
                taskItem.UserAssignedToTheTaskId = userId;
                taskItem.IsAssigned = true;
                toDoContext.SaveChanges();
            }
        }
    }
}

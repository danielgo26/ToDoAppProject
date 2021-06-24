using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
// System.Threading.Tasks;
using ToDoAppAgain.Controller;
using ToDoAppAgain.Data.Model;

namespace ToDoAppAgain.View
{
    public class Presentation
    {
        User currentUser;
        ToDoList chosenToDoList;
        ToDoAppController toDoAppController = new ToDoAppController();
        List<Task> unassignedTasks = new List<Task>();
        List<Task> assignedTasksofTheUser = new List<Task>();
        List<User> usersThatCanBeAssiged = new List<User>();
        //string management = "";
        /// <summary>
        /// maintains the flow of the program
        /// </summary>
        public void Engine()
        {
            LogIn();
            Connector();
            while (true)
            {
                Console.WriteLine("Do you want to exit the program? (yes\\no)");
                string answer = Console.ReadLine().ToLower();
                Console.Clear();
                if (answer == "yes")
                {
                    return;
                }
                else if (answer != "no")
                {
                    Console.WriteLine("Incorrect input!");
                }
                else
                {
                    break;
                }
            }
            Engine();
        }

        public void LogIn()
        {
            while (true)
            {
                Console.Write("Insert username: ");
                string inputUsername = Console.ReadLine();
                Console.Write("Insert password: ");
                string inputPass = Console.ReadLine();
                Console.Clear();
                currentUser = toDoAppController.GetUser(inputUsername, inputPass);
                if (currentUser != null)
                {
                    Console.WriteLine($"Welcome {currentUser.Username}!");
                    return;
                }
                Console.WriteLine("Wrong username or password. Try again!");
            }
        }

        public void Connector()
        {
            Console.WriteLine("1 - User management");
            Console.WriteLine("2 - ToDoList management");
            Console.WriteLine("3 - Task management");
            Console.WriteLine("4 - Complete your assigned tasks!");
            Console.WriteLine("0 - Exit User!");
            Console.Write("Choose section: ");
            int section = int.Parse(Console.ReadLine());
            Console.Clear();
            switch (section)
            {
                case 1:
                    if (currentUser.IsAdmin)
                    {
                        UserManagementLogic();
                        break;
                    }
                    Console.WriteLine("You do not have permission to user management!");
                    Connector();
                    break;
                case 2:
                    ToDoListManagement();
                    break;
                case 3:
                    if (CheckIfCurrentUserHaveLists())
                    {
                        ChooseToDoList();

                    }
                    else
                    {
                        Console.WriteLine("You do not have To Do Lists!");
                        Console.WriteLine("First create a To Do List!");
                    }
                    break;
                case 4:
                    if (CheckIfTheUserHaveAssignedTasks())
                    {
                        CompleteTask();
                    }
                    else
                    {
                        Console.WriteLine("You do not have assigned tasks!");
                    }
                    break;
                case 0:
                    return;
                default:
                    Console.WriteLine("Incorrect input!");
                    break;
            }
            Connector();
        }

        

        private void UserManagementLogic()
        {
            Console.WriteLine("Press 1 to list all the users!");
            Console.WriteLine("Press 2 to create a new user!");
            Console.WriteLine("Press 3 to edit user by username!");
            Console.WriteLine("Press 4 to delete user by username!");
            Console.WriteLine("Press 0 to exit!");
            Console.Write("Insert command:");
            int inputCommand = int.Parse(Console.ReadLine());
            Console.Clear();
            switch (inputCommand)
            {
                case 1:
                    ListAllUsers();
                    break;
                case 2:
                    CreateNewUser();
                    break;
                case 3:
                    EditUser();
                    break;
                case 4:
                    DeleteUser();
                    break;
                case 0:
                    return;
                default:
                    Console.WriteLine("Incorrect input!");
                    break;
            }
            UserManagementLogic();
        }

        private void DeleteUser()
        {
            Console.Write("Search user by username: ");
            string searchedUsername = Console.ReadLine();
            //User searchedUser = toDoAppController.GetUser(searchedUsername);
            //if (searchedUser == null)
            //{
                //Console.WriteLine("There isn't a user with this username!");
                //DeleteUser();
            //}
            //else
            //{
                toDoAppController.DeleteUser(searchedUsername);
            //}
        }

        private void EditUser()
        {
            Console.Write("Search user by username: ");
            string searchedUsername = Console.ReadLine();
            User searchedUser = toDoAppController.GetUser(searchedUsername);
            if (searchedUser == null)
            {
                Console.WriteLine("There isn't a user with this username!");
                EditUser();
            }
            else
            {
                Console.Write("New username: ");
                string newUsername = Console.ReadLine();
                Console.Write("New password: ");
                string newPassword = Console.ReadLine();
                Console.Write("New full name: ");
                string newName = Console.ReadLine();
                toDoAppController
                    .UpdateUser(searchedUser, newUsername, newPassword,
                    newName.Split(' ')[0], newName.Split(' ')[1], currentUser.Id);
            }
        }

        private void CreateNewUser()
        {
            Console.Write("Insert Username:");
            string userName = Console.ReadLine();
            Console.Write("Insert Password:");
            string password = Console.ReadLine();
            Console.Write("Insert full name: ");
            string names = Console.ReadLine();
            Console.Write("Insert administrator rights (yes\\no)!");
            string isAdminInput = Console.ReadLine();
            bool isAdmin = false;
            if (isAdminInput == "yes")
            {
                isAdmin = true;
            }
            User newUser = new User(userName, password, names.Split(' ')[0], names.Split(' ')[1], isAdmin);
            newUser.UserCreatorId = currentUser.Id;
            newUser.UserMadeLastChangeId = currentUser.Id;
            toDoAppController.Add(newUser);
        }

        private void ListAllUsers()
        {
            List<User> users = toDoAppController.GetAllUsers();
            foreach (var user in users)
            {
                Console.WriteLine(user.ToString());
            }
        }

        public void ToDoListManagement()
        {
            Console.WriteLine("Press 1 to list your To Do lists!");
            Console.WriteLine("Press 2 to create a new To Do List!");
            Console.WriteLine("Press 3 to edit To Do List by title!");
            Console.WriteLine("Press 4 to delete To Do List by title!");
            Console.WriteLine("Press 5 to share a To Do list with other users!");
            Console.WriteLine("Press 0 to exit!");
            Console.Write("Insert command:");
            int inputCommand = int.Parse(Console.ReadLine());
            Console.Clear();
            switch (inputCommand)
            {
                case 1:
                    ListAllToDoListsOfTheCurrentUser();
                    break;
                case 2:
                    CreateNewToDoList();
                    break;
                case 3:
                    EditToDoList();
                    break;
                case 4:
                    DeleteToDoList();
                    break;
                case 5:
                    ShareToDoList();
                    break;
                case 0:
                    return;
                default:
                    Console.WriteLine("Incorrect input!");
                    break;
            }
            ToDoListManagement();
        }

        private void ShareToDoList()
        {
            Console.WriteLine("To Do Lists:");
            ListAllToDoListsOfTheCurrentUser();
            Console.Write("Choose To Do List: ");
            string titleOfToDoListToBeShared = Console.ReadLine();
            ToDoList toDoListToBeShared = toDoAppController.GetToDoList(titleOfToDoListToBeShared);
            if (toDoListToBeShared == null)
            {
                Console.WriteLine("Incorrect title!");
                ShareToDoList();
                return;
            }
            Console.Write("Choose users by username to share To Do List: ");
            string[] searchedUsernames = Console.ReadLine().Split(", ");
            List<User> searchedUsers = new List<User>();
            foreach (var username in searchedUsernames)
            {
                User tempUser = toDoAppController.GetUser(username);
                if (tempUser == null)
                {
                    Console.WriteLine($"There isn't a user with this[{username}] username!");
                }
                else
                {
                    searchedUsers.Add(tempUser);
                }
            }
            toDoAppController.ShareToDoList(searchedUsers, toDoListToBeShared);
        }

        private void DeleteToDoList()
        {
            Console.Write("Search To Do List by title: ");
            string searchedTitle2 = Console.ReadLine();
            //ToDoList searchedToDoList2 = toDoAppController.GetToDoList(searchedTitle2);
            //if (searchedToDoList2 == null)
            //{
            //    Console.WriteLine("There isn't a ToDoList with this username!");
            //    DeleteToDoList();
            //}
            //else
            //{
            //    
            //}
            toDoAppController.DeleteToDoList(searchedTitle2, currentUser);
            Console.WriteLine("Successfully deleted!");
        }

        private void EditToDoList()
        {
            Console.Write("Search To Do List by title: ");
            string searchedTitle = Console.ReadLine();
            ToDoList searchedToDoList = toDoAppController.GetToDoList(searchedTitle);
            if (searchedToDoList == null)
            {
                Console.WriteLine("There isn't a ToDoList with this username!");
                EditToDoList();
            }
            else
            {
                Console.Write("New title: ");
                string newTitle = Console.ReadLine();
                toDoAppController.UpdateToDoList(searchedToDoList, newTitle, currentUser.Id);
            }
        }

        private void CreateNewToDoList()
        {
            Console.Write("Insert title: ");
            string title = Console.ReadLine();
            ToDoList newtoDoList = new ToDoList(title, currentUser.Id, currentUser.Id);
            toDoAppController.Add(newtoDoList);
            // !!! - not sure if this works
            UserToDoList userToDoList = new UserToDoList(currentUser.Id, newtoDoList.Id);
            toDoAppController.Add(userToDoList);
        }


        private void ListAllToDoListsOfTheCurrentUser()
        {
            List<ToDoList> yourToDoList = toDoAppController.GetAllToDoListsOfTheUser(currentUser.Id);
            //if (yourToDoList.Count == 0)
            //{
            //    Console.WriteLine("You do not have To Do Lists!");
            //    Console.WriteLine("First create a To Do List!");
            //    return;
            //}
            for (int i = 0; i < yourToDoList.Count; i++)
            {
                Console.WriteLine($"{i + 1} -> {yourToDoList[i]}");
            }
        }
        private bool CheckIfCurrentUserHaveLists()
        {
            List<ToDoList> yourToDoList = toDoAppController.GetAllToDoListsOfTheUser(currentUser.Id);
            return !(yourToDoList.Count == 0);
        }

        private void ChooseToDoList()
        {
            while (true)
            {
                ListAllToDoListsOfTheCurrentUser();
                Console.Write("Choose To Do List: ");
                string titleOftheChosenList = Console.ReadLine();
                chosenToDoList = toDoAppController.GetToDoList(titleOftheChosenList);
                Console.Clear();
                if (chosenToDoList == null)
                {
                    Console.WriteLine("Incorrect To Do List!");

                }
                else
                {
                    Console.WriteLine("Successfully entered!");
                    break;
                }
            }
            TaskManagement();
        }
        private void TaskManagement()
        {
            Console.WriteLine("Press 1 to list the tasks in this List!");
            Console.WriteLine("Press 2 to create a new Task in the List!");
            Console.WriteLine("Press 3 to edit a task in the List!");
            Console.WriteLine("Press 4 to delete a task in the List by title!");
            Console.WriteLine("Press 5 to assign a task to other users!"); // TODO
            Console.WriteLine("Press 0 to exit!");
            Console.Write("Insert command:");
            int inputCommand = int.Parse(Console.ReadLine());
            Console.Clear();
            switch (inputCommand)
            {
                case 1:
                    if (CheckIfChosenToDoListHaveTasks())
                    {
                        ListAllTasksOfTheToDoList(chosenToDoList.Id);
                    }
                    else
                    {
                        Console.WriteLine("You do not have Tasks in this list!");
                        Console.WriteLine("First create a Task!");
                    }
                    break;
                case 2:
                    CreateNewTask();
                    break;
                case 3:
                    EditTask();
                    break;
                case 4:
                    DeleteTask();
                    break;
                case 5:
                    if (CheckIfThereAreUnassignedTasksInTheList())
                    {
                        AssignTask();
                    }
                    else
                    {
                        Console.WriteLine("You do not have unassigned Tasks in this list!");
                        Console.WriteLine("First create new Task!");
                    }
                    break;
                case 0:
                    return;
                default:
                    break;
            }
            TaskManagement();
        }

        private void CompleteTask()
        {
            ListAllAssignedTasksOfTheUser();
            Console.Write("Search task by title: ");
            string searchedTitle = Console.ReadLine();
            Task searchedTask = GetAssignedtaskByTitle(searchedTitle);
            Console.Clear();
            if (searchedTask == null)
            {
                Console.WriteLine("There isn't a task with this title!");
                CompleteTask();
            }
            else
            {
                if (!searchedTask.IsCompleted)
                {

                    toDoAppController.CompleteTask(searchedTask, currentUser.Id);
                    Console.WriteLine("Successfully completed!");
                }
                else
                {
                    Console.WriteLine("You have already completed this task!");
                }
            }
        }

        private void ListAllTasksOfTheToDoList(int toDoListId)
        {
            List<Task> yourTasks = toDoAppController.GetAllTasksOfTheToDoList(toDoListId);
            //if (yourTasks.Count == 0)
            //{
            //    Console.WriteLine("You do not have Tasks in this list!");
            //    Console.WriteLine("First create a Task!");
            //    return;
            //}
            for (int i = 0; i < yourTasks.Count; i++)
            {
                Console.WriteLine($"{i + 1} -> {yourTasks[i]}");
            }
        }

        private void ListAllUnAssignedTasksOfTheToDoList(int toDoListId)
        {
            unassignedTasks = toDoAppController.GetAllUnAssignedTasksOfTheToDoList(toDoListId);
            for (int i = 0; i < unassignedTasks.Count; i++)
            {
                
                Console.WriteLine($"{i + 1} -> {unassignedTasks[i]}");
            }
        }

        private void ListAllAssignedTasksOfTheUser()
        {
            assignedTasksofTheUser = toDoAppController.GetAllAssignedTasksOfTheUser(currentUser.Id);
            for (int i = 0; i < assignedTasksofTheUser.Count; i++)
            {

                Console.WriteLine($"{i + 1} -> {assignedTasksofTheUser[i]}");
            }
        }



        private bool CheckIfChosenToDoListHaveTasks()
        {
            List<Task> yourTasks = toDoAppController.GetAllTasksOfTheToDoList(chosenToDoList.Id);
            return !(yourTasks.Count == 0);
        }

        private bool CheckIfThereAreUnassignedTasksInTheList()
        {
            unassignedTasks = toDoAppController.GetAllUnAssignedTasksOfTheToDoList(chosenToDoList.Id);
            return !(unassignedTasks.Count == 0);
        }
        private bool CheckIfTheUserHaveAssignedTasks()
        {
            assignedTasksofTheUser = toDoAppController.GetAllAssignedTasksOfTheUser(currentUser.Id);
            return !(assignedTasksofTheUser.Count == 0);
        }

        private void CreateNewTask()
        {
            Console.Write("Insert title: ");
            string title = Console.ReadLine();
            Console.Write("Insert description: ");
            string description = Console.ReadLine();
            Task newTask = new Task(title, description, chosenToDoList.Id, currentUser.Id, currentUser.Id);
            toDoAppController.Add(newTask);
            // !!! - not sure if this works
            //UserToDoList userToDoList = new UserToDoList(currentUser.Id, newtoDoList.Id);
            //toDoAppController.Add(userToDoList);
        }

        private void EditTask()
        {
            Console.Write("Search task by title: ");
            string searchedTitle = Console.ReadLine();
            Task searchedTask = toDoAppController.GetTask(searchedTitle);
            if (searchedTask == null)
            {
                Console.WriteLine("There isn't a task with this title!");
                EditTask();
            }
            else
            {
                Console.Write("New title: ");
                string newTitle = Console.ReadLine();
                Console.Write("New description: ");
                string description = Console.ReadLine();
                //Console.Write("Whether the task is completed: (yes\\no): ");
                //string isCompletedInfo = Console.ReadLine();
                //bool isCompleted = false;
                //if (isCompletedInfo == "yes")
                //{
                    //isCompleted = true;
                //}
                toDoAppController.UpdateTask(searchedTask, newTitle, description, currentUser.Id);
            }
        }

        private void DeleteTask()
        {
            Console.Write("Search task by title: ");
            string searchedTitle2 = Console.ReadLine();
            //ToDoList searchedToDoList2 = toDoAppController.GetToDoList(searchedTitle2);
            //if (searchedToDoList2 == null)
            //{
            //    Console.WriteLine("There isn't a ToDoList with this username!");
            //    DeleteToDoList();
            //}
            //else
            //{
            //    
            //}
            toDoAppController.DeleteTask(searchedTitle2);
            Console.WriteLine("Successfully deleted!");
        }

        private void AssignTask()
        {
            Task toBeAssigned;
            while (true)
            {
                ListAllUnAssignedTasksOfTheToDoList(chosenToDoList.Id);
                Console.Write("Choose a task by title: ");
                string taskTitle = Console.ReadLine();
                toBeAssigned = GetUnassignedtaskByTitle(taskTitle);
                Console.Clear();
                if (toBeAssigned == null)
                {
                    Console.WriteLine("Incorrect task!");
                }
                else
                {
                    break;
                }
            }
            usersThatCanBeAssiged = toDoAppController.GetAllUsersOfTheToDoList(chosenToDoList.Id);
            
            User userToBeAssigned;
            while (true)
            {
                foreach (var u in usersThatCanBeAssiged)
                {
                    Console.WriteLine(u.ToString());
                }
                Console.Write("Choose a user by username: ");
                string username = Console.ReadLine();
                userToBeAssigned = GetUserFromUsersThatCanBeAssigned(username);
                Console.Clear();
                if (userToBeAssigned == null)
                {
                    Console.WriteLine("Incorrect user!");
                }
                else
                {
                    toDoAppController.AssignTaskToUser(userToBeAssigned.Id, toBeAssigned.Id);
                    Console.WriteLine($"Successfully assigned {toBeAssigned.Title} to {username}!");
                    break;
                }
            }
            
        }

        private Task GetUnassignedtaskByTitle(string taskTitle)
        {
            return unassignedTasks
                .Where(t => t.Title == taskTitle).FirstOrDefault();
        }
        private Task GetAssignedtaskByTitle(string taskTitle)
        {
            return assignedTasksofTheUser
                .Where(t => t.Title == taskTitle).FirstOrDefault();
        }

        private User GetUserFromUsersThatCanBeAssigned(string username)
        {
            return usersThatCanBeAssiged.Where(u => u.Username == username).FirstOrDefault();
        }

    }
}

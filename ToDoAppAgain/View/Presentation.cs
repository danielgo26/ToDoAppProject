using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        /// <summary>
        /// maintains the flow of the program
        /// </summary>
        public void Engine()
        {
            LogIn();
            Connector();
            while (true)
            {
                Console.WriteLine("Do you want to exit the program? (yes\\no):");
                string answer = Console.ReadLine().ToLower();
                Console.Clear();
                if (answer == "yes")
                {
                    return;
                }
                else if (answer != "no")
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Incorrect input!");
                    Console.ForegroundColor = ConsoleColor.White;
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
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"Welcome {currentUser.Username}!");
                    Console.ForegroundColor = ConsoleColor.White;
                    return;
                }
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Wrong username or password. Try again!");
                Console.ForegroundColor = ConsoleColor.White;
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
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("You do not have permission to user management!");
                    Console.ForegroundColor = ConsoleColor.White;
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
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("You do not have To Do Lists!");
                        Console.WriteLine("First create a To Do List!");
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    break;
                case 4:
                    if (CheckIfTheUserHaveAssignedTasks())
                    {
                        CompleteTask();
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("You do not have assigned tasks!");
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    break;
                case 0:
                    return;
                default:
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Incorrect input!");
                    Console.ForegroundColor = ConsoleColor.White;
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
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Incorrect input!");
                    Console.ForegroundColor = ConsoleColor.White;
                    break;
            }
            UserManagementLogic();
        }

        private void DeleteUser()
        {
            Console.Write("Search user by username: ");
            string searchedUsername = Console.ReadLine();
            Console.Clear();
            if (searchedUsername == "admin")
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("You cannot delete admin!");
                Console.ForegroundColor = ConsoleColor.White;
                return;
            }
            else
            {

                if (toDoAppController.GetUser(searchedUsername) == null)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"There isn't a user with username {searchedUsername}!");
                    Console.ForegroundColor = ConsoleColor.White;
                }
                else
                {
                    if (searchedUsername == currentUser.Username)
                    {
                        Console.WriteLine($"Bye {searchedUsername}!");
                        System.Threading.Thread.Sleep(1000);
                        toDoAppController.DeleteUser(searchedUsername);
                        Console.Clear();
                        Engine();
                    }
                    else
                    {
                        toDoAppController.DeleteUser(searchedUsername);
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("Successfully deleted!");
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                }
            }
        }
        private void EditUser()
        {
            Console.Write("Search user by username: ");
            string searchedUsername = Console.ReadLine();
            Console.Clear();
            if (searchedUsername == "admin")
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("You cannot edit admin!");
                Console.ForegroundColor = ConsoleColor.White;
                return;
            }
            else
            {
                User searchedUser = toDoAppController.GetUser(searchedUsername);
                if (searchedUser == null)
                {
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("There isn't a user with this username!");
                    Console.ForegroundColor = ConsoleColor.White;
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
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Successfully editted!");
                    Console.ForegroundColor = ConsoleColor.White;
                }
            }
        }

        private void CreateNewUser()
        {
            string userName;
            while (true)
            {
                Console.Write("Insert Username:");
                userName = Console.ReadLine();
                if (toDoAppController.GetUser(userName) != null)
                {
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"There is already user with username {userName}!");
                    Console.ForegroundColor = ConsoleColor.White;
                }
                else
                {
                    break;
                }
            }
            Console.Write("Insert Password:");
            string password = Console.ReadLine();
            Console.Write("Insert full name: ");
            string names = Console.ReadLine();
            Console.Write("Insert administrator rights (yes\\no):");
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
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Clear();
            Console.WriteLine("Sucessfully created!");
            Console.ForegroundColor = ConsoleColor.White;
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
                    if (CheckIfCurrentUserHaveLists())
                    {
                        ListAllToDoListsOfTheCurrentUser();
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("You do not have To Do Lists!");
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    break;
                case 2:
                    CreateNewToDoList();
                    break;
                case 3:
                    if (toDoAppController.GetAllToDoListsOfTheUser(currentUser.Id).Count != 0)
                    {
                        EditToDoList();
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("You do not have any To Do Lists!");
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    break;
                case 4:
                    if (toDoAppController.GetAllToDoListsOfTheUser(currentUser.Id).Count != 0)
                    {
                        DeleteToDoList();
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("You do not have any To Do Lists!");
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    break;
                case 5:
                    if (toDoAppController.GetAllToDoListsOfTheUser(currentUser.Id).Count != 0)
                    {
                        ShareToDoList();
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("You do not have any To Do Lists!");
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    break;
                case 0:
                    return;
                default:
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Incorrect input!");
                    Console.ForegroundColor = ConsoleColor.White;
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
            Console.Clear();
            if (toDoListToBeShared == null)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Incorrect title!");
                Console.ForegroundColor = ConsoleColor.White;
                ShareToDoList();
                return;
            }
            Console.Write("Choose users by username to share To Do List: ");
            string[] searchedUsernames = Console.ReadLine().Split(", ");
            List<User> searchedUsers = new List<User>();
            foreach (var username in searchedUsernames)
            {
                User tempUser = toDoAppController.GetUser(username);
                Console.Clear();
                if (tempUser == null)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"There isn't a user with username {username}!");
                    Console.ForegroundColor = ConsoleColor.White;
                }
                else
                {
                    searchedUsers.Add(tempUser);
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Successfully shared!");
                    Console.ForegroundColor = ConsoleColor.White;
                }
            }
            //List<ToDoList> sharedListOfTheUser = toDoAppController.GetAllToDoListsOfTheUser(currentUser.Id);
            //foreach (var tdl in sharedListOfTheUser)
            //{
            //    if (tdl.Title==toDoListToBeShared.Title)
            //    {
            //        Console.ForegroundColor = ConsoleColor.Red;
            //        Console.WriteLine("This To Do List is already shared to this user!");
            //        Console.ForegroundColor = ConsoleColor.White;
            //    }
            //}
            toDoAppController.ShareToDoList(searchedUsers, toDoListToBeShared);
        }

        private void DeleteToDoList()
        {
            Console.Write("Search To Do List by title: ");
            string searchedTitle2 = Console.ReadLine();
            Console.Clear();
            if (toDoAppController.GetToDoList(searchedTitle2) == null)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"There isn't a To Do List with title {searchedTitle2}!");
                Console.ForegroundColor = ConsoleColor.White;
            }
            else
            {
                toDoAppController.DeleteToDoList(searchedTitle2, currentUser);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Successfully deleted!");
                Console.ForegroundColor = ConsoleColor.White;
            }
        }

        private void EditToDoList()
        {
            Console.Write("Search To Do List by title: ");
            string searchedTitle = Console.ReadLine();
            ToDoList searchedToDoList = toDoAppController.GetToDoList(searchedTitle);
            Console.Clear();
            if (searchedToDoList == null)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("There isn't a ToDoList with this title!");
                Console.ForegroundColor = ConsoleColor.White;
                EditToDoList();
            }
            else
            {
                Console.Write("New title: ");
                string newTitle = Console.ReadLine();
                Console.Clear();
                if (toDoAppController.GetToDoList(newTitle) != null)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("There is already a To Do List with this title!");
                    Console.ForegroundColor = ConsoleColor.White;
                }
                else
                {
                    toDoAppController.UpdateToDoList(searchedToDoList, newTitle, currentUser.Id);
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Successfully edited!");
                    Console.ForegroundColor = ConsoleColor.White;
                }
            }
        }

        private void CreateNewToDoList()
        {
            Console.Write("Insert title: ");
            string title = Console.ReadLine();
            Console.Clear();
            if (toDoAppController.GetToDoList(title) != null)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("There is already a To Do List with that name!");
                Console.ForegroundColor = ConsoleColor.White;
            }
            else
            {
                ToDoList newtoDoList = new ToDoList(title, currentUser.Id, currentUser.Id);
                toDoAppController.Add(newtoDoList);
                UserToDoList userToDoList = new UserToDoList(currentUser.Id, newtoDoList.Id);
                toDoAppController.Add(userToDoList);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Successfully created!");
                Console.ForegroundColor = ConsoleColor.White;
            }
        }

        private void ListAllToDoListsOfTheCurrentUser()
        {
            List<ToDoList> yourToDoList = toDoAppController.GetAllToDoListsOfTheUser(currentUser.Id);
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
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Incorrect To Do List!");
                    Console.ForegroundColor = ConsoleColor.White;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Successfully entered!");
                    Console.ForegroundColor = ConsoleColor.White;
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
            Console.WriteLine("Press 5 to assign a task to other users!");
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
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("You do not have Tasks in this list!");
                        Console.WriteLine("First create a Task!");
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    break;
                case 2:
                    CreateNewTask();
                    break;
                case 3:
                    if (toDoAppController.GetAllTasksOfTheToDoList(chosenToDoList.Id).Count!=0)
                    {
                        EditTask();
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("You do not have Tasks in this list!");
                        Console.WriteLine("First create a Task!");
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    break;
                case 4:
                    if (toDoAppController.GetAllTasksOfTheToDoList(chosenToDoList.Id).Count != 0)
                    {
                        DeleteTask();
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("You do not have Tasks in this list!");
                        Console.WriteLine("First create a Task!");
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    break;
                case 5:
                    if (CheckIfThereAreUnassignedTasksInTheList())
                    {
                        AssignTask();
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("You do not have unassigned Tasks in this list!");
                        Console.WriteLine("First create new Task!");
                        Console.ForegroundColor = ConsoleColor.White;
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
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("There isn't a task with this title!");
                Console.ForegroundColor = ConsoleColor.White;
                CompleteTask();
            }
            else
            {
                if (!searchedTask.IsCompleted)
                {

                    toDoAppController.CompleteTask(searchedTask, currentUser.Id);
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Successfully completed!");
                    Console.ForegroundColor = ConsoleColor.White;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("You have already completed this task!");
                    Console.ForegroundColor = ConsoleColor.White;
                }
            }
        }

        private void ListAllTasksOfTheToDoList(int toDoListId)
        {
            List<Task> yourTasks = toDoAppController.GetAllTasksOfTheToDoList(toDoListId);
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

            if (GetTaskFromCurrentList(title) != null)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"There is already a task with title {title}!");
                Console.ForegroundColor = ConsoleColor.White;
                return;
            }
            else
            {
                Console.Write("Insert description: ");
                string description = Console.ReadLine();
                Task newTask = new Task(title, description, chosenToDoList.Id, currentUser.Id, currentUser.Id);
                toDoAppController.Add(newTask);
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Successfully created!");
                Console.ForegroundColor = ConsoleColor.White;
            }
        }

        private void EditTask()
        {
            Console.Write("Search task by title: ");
            string searchedTitle = Console.ReadLine();
            Task searchedTask = toDoAppController.GetTask(searchedTitle);
            Console.Clear();
            if (searchedTask == null)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"There isn't a task with title {searchedTitle}!");
                Console.ForegroundColor = ConsoleColor.White;
                EditTask();
            }
            else
            {
                Console.Write("New title: ");
                string newTitle = Console.ReadLine();
                if (GetTaskFromCurrentList(newTitle) != null)
                {
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"There is already a task with title {newTitle}!");
                    Console.ForegroundColor = ConsoleColor.White;
                }
                else
                {
                    Console.Write("New description: ");
                    string description = Console.ReadLine();
                    toDoAppController.UpdateTask(searchedTask, newTitle, description, currentUser.Id);
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Successfully edited!");
                    Console.ForegroundColor = ConsoleColor.White;
                }
            }
        }

        private void DeleteTask()
        {
            Console.Write("Search task by title: ");
            string searchedTitle2 = Console.ReadLine();
            if (GetTaskFromCurrentList(searchedTitle2) == null)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"There isn't a task with title {searchedTitle2}!");
                Console.ForegroundColor = ConsoleColor.White;
                return;
            }
            else
            {
                toDoAppController.DeleteTask(searchedTitle2);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Successfully deleted!");
                Console.ForegroundColor = ConsoleColor.White;
            }
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
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Incorrect task!");
                    Console.ForegroundColor = ConsoleColor.White;
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
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Incorrect user!");
                    Console.ForegroundColor = ConsoleColor.White;
                }
                else
                {
                    toDoAppController.AssignTaskToUser(userToBeAssigned.Id, toBeAssigned.Id);
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"Successfully assigned {toBeAssigned.Title} to {username}!");
                    Console.ForegroundColor = ConsoleColor.White;
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

        private Task GetTaskFromCurrentList(string title)
        {
            // return chosenToDoList.TasksOfTheList.Where(t => t.Title == title).FirstOrDefault();
            foreach (var task in toDoAppController.GetAllTasksOfTheToDoList(chosenToDoList.Id))
            {
                if (task.Title == title)
                {
                    return task;
                }
            }
            return null;
        }

    }
}

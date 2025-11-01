namespace Homework;

class Program
{
    private static string versionText = "Версия программы: 02.13.01, дата создания: 31.10.2025.";
    private static List<ToDoItem> todoList = new List<ToDoItem>();
    private static ToDoUser User = new ToDoUser(null);
    private static string command;

    static void Main()
    {
        Console.WriteLine("Добро пожаловать в консольного бота :) \n");
        Console.WriteLine("Список доступных команд:");
        Console.WriteLine(" /start \n /help \n /info \n " +
                          "/addtask \n /showtasks \n /showalltasks \n /completetask \n " +
                          // "/removetask \n /echo \n " +
                          "/exit");

        do
        {
            Console.WriteLine("\n---------------------------");
            Console.Write("Введите название команды: ");
            command = Console.ReadLine();

            Console.WriteLine("\nВы ввели: " + command + "\n");

            switch (command)
            {
                case "/start":
                    StartApp();
                    break;
                case "/help":
                    ShowHelp();
                    break;
                case "/info":
                    ShowInfo();
                    break;
                case "/addtask":
                    AddTask();
                    break; 
                case "/completetask":
                    CompleteTask();
                    break;
                case "/showtasks":
                    ShowTasks("Active");
                    break;
                case "/showalltasks":
                    ShowTasks();
                    break;
                // case "/removetask":
                //     RemoveTask();
                //     break;
                // case string cmd when cmd.StartsWith("/echo"):
                //     // Echo(cmd, name);
                //     break;
            }
        } while (command != "/exit");

        Console.WriteLine("Программа завершена, до свидания.");
    }

    public static void StartApp()
    {
        do
        {
            Console.Write("Введите свое имя: ");
            User.TelegramUserName = Console.ReadLine();
        } while (string.IsNullOrWhiteSpace(User.TelegramUserName));

        Console.WriteLine($"Здравствуйте {User.TelegramUserName}.");
    }

    public static void ShowInfo()
    {
        Console.WriteLine(versionText);
    }

    public static void ShowHelp()
    {
        Console.WriteLine("Список доступных команд:");
        Console.WriteLine(" /start          - начало работы с программой.");
        Console.WriteLine(" /help           - вывод описания по доступным командам.");
        Console.WriteLine(" /info           - техническая информация о программе.");
        Console.WriteLine(" /showtasks      - отобразить все задачи из списка дел.");
        Console.WriteLine(" /showalltasks   - отобразить задачи только в статусе 'Active'.");
        Console.WriteLine(" /addtask        - добавить задачу в список дел.");
        Console.WriteLine(" /completetask   - завершить выбранную задачу.");
        // Console.WriteLine(" /removetask     - удалить задачу из списка дел.");
        // Console.WriteLine(" /echo           - выводит в консоль текст, введенный после команд /echo. " +
                          // "Например: /echo Hello World!");
        Console.WriteLine(" /exit           - выход из программы.");
    }
    
    public static bool ShowTasks(string mode = "all")
    {
        int taskCount = 0;

        if (todoList.Count == 0)
        {
            Console.WriteLine("Список задач пуст.");
        }
        else if (mode == "Active")
        {
            Console.WriteLine("Cписок текущих задач: ");

            for (int i = 0; i < todoList.Count && todoList[i].State == ToDoItem.ToDoItemState.Active; i++)
            {
                Console.WriteLine($"#{i + 1}. {todoList[i].CreatedAt} [{todoList[i].Id}] " +
                                  $"\nCтатус: {todoList[i].State}" +
                                  $"\nТекст: {todoList[i].Name}\n");
                taskCount++;
            }
        }
        else
        {
            Console.WriteLine("Cписок текущих задач: ");

            for (int i = 0; i < todoList.Count; i++)
            {
                Console.WriteLine($"#{i + 1}. {todoList[i].CreatedAt} [{todoList[i].Id}] " +
                                  $"\nCтатус: {todoList[i].State}" +
                                  $"\nТекст: {todoList[i].Name}\n");
                taskCount++;
            }
        }

        return taskCount > 0;
    }

    public static void AddTask()
    {
        string taskText = "";

        do
        {
            Console.WriteLine("Введите описание задачи: ");
            taskText = Console.ReadLine();
        } while (string.IsNullOrWhiteSpace(taskText));

        todoList.Add(new ToDoItem(User, taskText));
        Console.WriteLine("\nЗадача добавлена.");
    }

    public static void CompleteTask()
    {
        string taskId = "";
        
        if (ShowTasks("Active"))
        {
            Console.Write("\nВведите Id задачи для завершения: ");
            taskId = Console.ReadLine();

            for (int i = 0; i < todoList.Count && todoList[i].State == ToDoItem.ToDoItemState.Active; i++)
            {
                if (todoList[i].Id.ToString() == taskId)
                {
                    todoList[i].State = ToDoItem.ToDoItemState.Completed;
                    Console.WriteLine($"Задача с идентификатором [{todoList[i].Id.ToString()}] завершена." );
                }
            }
        }
    }
    
    /*
    public static void RemoveTask()
    {
        if (ShowTasks())
        {
            int taskNumber;
            bool exitRemoveFlag = false;

            do
            {
                Console.Write("\nВыберите номер задачи для удаления: ");
                int.TryParse(Console.ReadLine(), out taskNumber);

                if (taskNumber > 0 && taskNumber <= todoList.Count)
                {
                    todoList.RemoveAt(taskNumber - 1);
                    Console.WriteLine($"Задача #{taskNumber} удалена.");
                    exitRemoveFlag = true;
                }
                else
                {
                    Console.WriteLine($"\nЗадача c выбранным номером отсутствует в списке." +
                                      "\nВведите корректный номер задачи.\n");
                }
            } while ((taskNumber <= 0 || taskNumber > todoList.Count) && todoList.Count > 0 && !exitRemoveFlag);
        }
    }
    */
    
    /*
     public static void Echo(String cmd, String name)
    {
        if (!string.IsNullOrEmpty(name) || !string.IsNullOrWhiteSpace(name))
        {
            Console.WriteLine(cmd.Substring("/echo".Length).Trim());
        }
        else
        {
            Console.WriteLine("Команда /echo не доступна, т.к. не задано имя пользователя. "
                              + "Задайте имя используя команду /start.");
        }
    }
    */
}
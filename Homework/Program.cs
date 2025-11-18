namespace Homework;

class Program
{
    // Домашнее задание №16: Интерфейсы
    private static string versionText = "Версия программы: 02.16.01, дата создания: 18.11.2025.";
    private static List<ToDoItem> todoList = new List<ToDoItem>();
    private static ToDoUser User = new ToDoUser(null);
    private static string command;

    static void Main()
    {
        Console.WriteLine("Добро пожаловать в консольного бота :) \n");
        Console.WriteLine("Список доступных команд:");
        Console.WriteLine(" /start \n /help \n /info \n " +
                          "/addtask \n /showtasks \n /showalltasks \n /completetask \n " +
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
                    ShowTasks("All");
                    break;
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

            for (int i = 0; i < todoList.Count; i++)
            {
                if (todoList[i].State == ToDoItem.ToDoItemState.Active)
                {
                    Console.WriteLine($"#{i + 1}. {todoList[i].CreatedAt} [{todoList[i].Id}] " +
                                      $"\nТекст: {todoList[i].Name}\n");
                    
                    taskCount++;
                }
            }
        }
        else if (mode == "All")
        {
            Console.WriteLine("Cписок текущих задач: ");

            for (int i = 0; i < todoList.Count; i++)
            {
                Console.WriteLine($"#{i + 1}. {todoList[i].CreatedAt} [{todoList[i].Id}] " +
                                  $"\nCтатус: [{todoList[i].State}], изменен: {todoList[i].StateChangedAt}" +
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

            for (int i = 0; i < todoList.Count; i++)
            {
                if (todoList[i].Id.ToString() == taskId && todoList[i].State == ToDoItem.ToDoItemState.Active)
                {
                    todoList[i].State = ToDoItem.ToDoItemState.Completed;
                    todoList[i].StateChangedAt = DateTime.Now;
                    Console.WriteLine($"Задача с идентификатором [{todoList[i].Id.ToString()}] завершена." );
                }
            }
        }
    }
}
namespace Homework;

class Program
{
    private static string versionText = "Версия программы: 02.11.04, дата изменения: 26.11.2025.";
    private static List<ToDoItem> todoList = new List<ToDoItem>();
    private static ToDoUser User = new ToDoUser(null);
    private static string command;
    private static int taskCountLimit;
    private static int taskLengthLimit;
    
    static void Main()
    {
        try
        {
            Console.WriteLine("Добро пожаловать в консольного бота :) \n");
            Console.WriteLine("Список доступных команд:");
            Console.WriteLine(" /start \n /help \n /info \n " +
                              "/addtask \n /showtasks \n /showalltasks \n /completetask \n " +
                              "/exit");

            do
            {
                try
                {
                    RunApp();
                }
                catch (TaskCountLimitException e) {Console.WriteLine(e.Message);}
                catch (TaskLengthLimitException e) {Console.WriteLine(e.Message);}
                catch (DuplicateTaskException e) {Console.WriteLine(e.Message);}
                catch (ArgumentException e) { Console.WriteLine(e.Message); }
            } 
            while (command != "/exit");

            Console.WriteLine("Программа завершена, до свидания.");
        }
        catch (Exception e)
        {
            Console.WriteLine($"Произошла непредвиденная ошибка: " +
                              $"{e.GetType()}: {e.Message} \n {e.StackTrace} \n {e.InnerException}");
        }
    }

    public static void RunApp()
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
    }
    public static void StartApp()
    {
        Console.Write("Введите максимально допустимое количество задач (от 1 до 100): ");
        taskCountLimit = ParseAndValidateInt(Console.ReadLine(), 1, 100);
        
        if (taskCountLimit <= 0 || taskCountLimit > 100) 
            throw new ArgumentException("Максимально допустимое количество задач должно быть в диапазоне от 1 до 100");
        
        Console.Write("Введите максимально допустимую длину задачи: ");
        taskLengthLimit = ParseAndValidateInt(Console.ReadLine(), 1, 100);
        
        do
        {
            Console.Write("Введите свое имя: ");
            User.TelegramUserName = Console.ReadLine();
            ValidateString(User.TelegramUserName);
            
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
        if (todoList.Count >= taskCountLimit && taskCountLimit > 0) 
            throw new TaskCountLimitException(taskCountLimit);
        
        string taskText = "";

        do
        {
            Console.WriteLine("Введите описание задачи: ");
            taskText = Console.ReadLine();
        } while (string.IsNullOrWhiteSpace(taskText));
        
        ValidateString(taskText);

        if (taskText.Length > taskLengthLimit && taskLengthLimit > 0) 
            throw new TaskLengthLimitException(taskText.Length,  taskLengthLimit);
        
        if (todoList.Any(item => item.Name == taskText))
            throw new DuplicateTaskException(taskText);
        
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
            
            ValidateString(taskId);

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

    public static int ParseAndValidateInt(string? str, int min, int max)
    {
        int number;
        int.TryParse(str, out number);
        
        if (number < min || number > max)
            throw new ArgumentException($"Строка не является числом, либо выходит за пределы указанного диапазона от {min} до {max}.");
        
        return number;
    }
    
    public static void ValidateString(string? str)
    {
        if (string.IsNullOrEmpty(str) || string.IsNullOrWhiteSpace(str)) 
            throw new ArgumentException("Введенное значение не должно быть равно null, пустой строке или пробелу.");
    }
    
    public class TaskLengthLimitException : Exception
    {
        public TaskLengthLimitException(int taskLength, int taskLengthLimit) 
            : base($"Длина задачи ({taskLength}) превышает максимально допустимое значение {taskLengthLimit}.") { }
    }
    
    public class DuplicateTaskException : Exception
    {
        public DuplicateTaskException(string task) : base($"Задача '{task}' уже существует.") { }
    }
    
    public class TaskCountLimitException : Exception
    {
        public TaskCountLimitException(int taskCountLimit) 
            : base($"Превышено максимальное количество задач равное {taskCountLimit}.") { }
    }
}
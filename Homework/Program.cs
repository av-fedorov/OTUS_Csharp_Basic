using System.Runtime.InteropServices.JavaScript;

namespace Homework;

class Program
{
    private static string versionText = "Версия программы: 0.2.1, дата создания: 09.09.2025.";
    private static List<string> todoList = new List<string>();
    private static string command;
    private static string name = "";
    
    static void Main()
    {
        Console.WriteLine("Добро пожаловать в консольного бота :) \n");
        Console.WriteLine("Список доступных команд:");
        Console.WriteLine(" /start \n /help \n /info \n /echo \n" +
                          " /addtask \n /showtasks \n /removetask \n /exit");
        do
        {
            Console.WriteLine("\n---------------------------");
            Console.Write("Введите название команды: ");
            command = Console.ReadLine();

            Console.WriteLine("\nВы ввели: " + command + "\n");

            if (!string.IsNullOrEmpty(name) || !string.IsNullOrWhiteSpace(name))
            {
                Console.WriteLine("\nЗдравствуйте " + name + ".");
            }

            switch (command)
            {
                case "/start":
                    name = StartApp();
                    break;
                case "/help":
                    ShowHelp();
                    break;
                case "/info":
                    ShowInfo();
                    break;
                case string cmd when cmd.StartsWith("/echo"):
                    Echo(cmd, name);
                    break;
                case "/addtask":
                    AddTask();
                    break;
                case "/showtasks":
                    ShowTasks();
                    break;
                case "/removetask":
                    RemoveTask();
                    break;
            }
        } while (command != "/exit");

        Console.WriteLine("Программа завершена, до свидания.");
    }

    public static String StartApp()
    {
        while (string.IsNullOrWhiteSpace(name))
        {
            Console.Write("Введите свое имя: ");
            name = Console.ReadLine();
        }

        return name;
    }
    
    public static void ShowInfo()
    {
        Console.WriteLine(versionText);
    }
    
    public static void ShowHelp()
    {
        Console.WriteLine("Список доступных команд:");
        Console.WriteLine(" /start      - начало работы с программой.");
        Console.WriteLine(" /help       - вывод описания по доступным командам.");
        Console.WriteLine(" /info       - техническая информация о программе.");
        Console.WriteLine(" /echo       - выводит в консоль текст, введенный после команд /echo. " +
                          "Например: /echo Hello World!");
        Console.WriteLine(" /addtask    - добавить задачу в список дел.");
        Console.WriteLine(" /showtasks  - отобразить задачи из списка дел.");
        Console.WriteLine(" /removetask - удалить задачу из списка дел.");
        Console.WriteLine(" /exit       - выход из программы.");
    }
    
    public static bool ShowTasks()
    {
        if (todoList.Count == 0)
            Console.WriteLine("Список задач пуст.");
        else
        {
            Console.WriteLine("Cписок текущих задач: ");
            
            for (int i = 0; i < todoList.Count; i++)
                Console.WriteLine($"{i + 1}. {todoList[i]}");
        }
        
        return todoList.Count > 0;
    }
    
    public static void AddTask()
    {
        string taskText = "";

        while (string.IsNullOrWhiteSpace(taskText))
        {
            Console.WriteLine("Введите описание задачи: ");
            taskText = Console.ReadLine();
        }
        
        todoList.Add(taskText);
        Console.WriteLine("\nЗадача добавлена.");
    }

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
}
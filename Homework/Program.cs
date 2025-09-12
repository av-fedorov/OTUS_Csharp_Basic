namespace Homework;

class Program
{
    static void Main()
    {
        string command;
        string name = "";
        List<string> todoList = new List<string>();

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
                    Console.Write("Введите свое имя: ");
                    name = Console.ReadLine();
                    break;
                case "/help":
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
                    break;
                case "/info":
                    Console.WriteLine("Версия программы: 0.1, дата создания: 19.08.2025.");
                    break;
                case string cmd when cmd.IndexOf("/echo") >= 0:
                    if (!string.IsNullOrEmpty(name) || !string.IsNullOrWhiteSpace(name))
                    {
                        Console.WriteLine(cmd.Substring("/echo".Length).Trim());
                    }
                    else
                    {
                        Console.WriteLine("Команда /echo не доступна, т.к. не задано имя пользователя. "
                                          + "Задайте имя используя команду /start.");
                    }
                    break;
                case "/addtask":
                    Console.WriteLine("Введите описание задачи: ");
                    todoList.Add(Console.ReadLine());
                    Console.WriteLine("Задача добавлена.");
                    break;
                case "/showtasks":
                    ListTasks(todoList);
                    break;
                case "/removetask":
                    ListTasks(todoList);

                    if (todoList.Count > 0)
                    {
                        Console.Write("Выберите номер задачи для удаления: ");
                    
                        int taskNumber = Convert.ToInt32(Console.ReadLine());   
                        // планирую добавить exception после соответствующего урока :)
                    
                        if (taskNumber > 0 && taskNumber <= todoList.Count)
                        {
                            todoList.RemoveAt(taskNumber - 1);
                            Console.WriteLine($"Задача #{taskNumber} удалена.");    
                        }
                        else
                        {
                            Console.WriteLine($"Задача #{taskNumber} отсутствует в списке.");
                        }
                    }
                    break;
            }
        } while (command != "/exit");

        Console.WriteLine("Программа завершена, до свидания.");
    }
    
    public static void ListTasks(List<string> todoList)
    {
        if (todoList.Count == 0)
            Console.WriteLine("Список задач пуст.");
        else
        {
            for (int i = 0; i < todoList.Count; i++)
                Console.WriteLine($"{i + 1}. {todoList[i]}");    
        }
    }
}
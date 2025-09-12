namespace Homework;

class Program
{
    static void Main()
    {
        string command;
        string name = "";

        Console.WriteLine("Добро пожаловать в консольного бота :) \n");
        Console.WriteLine("Список доступных команд:");
        Console.WriteLine(" /start \n /help \n /info \n /echo \n /exit");

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
                    Console.WriteLine(" /start  - начало работы с программой.");
                    Console.WriteLine(" /help   - вывод описания по доступным командам.");
                    Console.WriteLine(" /info   - техническая информация о программе.");
                    Console.WriteLine(" /echo   - выводит в консоль текст, введенный после команд /echo. " +
                                      "Например: /echo Hello World!");
                    Console.WriteLine(" /exit   - выход из программы.");
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
            }
        } while (command != "/exit");

        Console.WriteLine("Программа завершена, до свидания.");
    }
}
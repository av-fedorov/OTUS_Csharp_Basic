using System.Globalization;
using Otus.ToDoList.ConsoleBot;
using Otus.ToDoList.ConsoleBot.Types;

namespace Homework;

public class UpdateHandler : IUpdateHandler
{
    private static string versionText = "Версия программы: 02.16.01 " +
                                        "\nДата создания: 18.11.2025 " +
                                        "\nДата обновления: 09.12.2025";
    private static string command;
    private static IReadOnlyList<ToDoItem> todoList = new List<ToDoItem>();
    private static ToDoService TaskService = new();
    private static UserService UserService = new();
    private static ToDoUser CurrentUser = null;

    public void HandleUpdateAsync(ITelegramBotClient botClient, Update update)
    {
        try
        {
            RunApp();
        }
        catch (ArgumentException e) { botClient.SendMessage(update.Message.Chat, e.Message); }
        catch (Exception e) { botClient.SendMessage(update.Message.Chat, e.Message); }
        void RunApp()
        {
            botClient.SendMessage(update.Message.Chat, "Список доступных команд:");
            botClient.SendMessage(update.Message.Chat, " /start \n /help \n /info \n " +
                                                       "/addtask \n /showtasks \n /showalltasks \n " +
                                                       "/completetask /removetask /exit\n");

            botClient.SendMessage(update.Message.Chat, $"Вы ввели: '{update.Message.Text}'\n");

            switch (update.Message.Text)
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
                case string cmd when cmd.StartsWith("/addtask") && CanRunCmd():
                    AddTask();
                    break;
                case string cmd when cmd.StartsWith("/completetask") && CanRunCmd():
                    CompleteTask();
                    break;
                case string cmd when cmd.StartsWith("/removetask") && CanRunCmd():
                    RemoveTask();
                    break;
                case string cmd when cmd == "/showtasks" && CanRunCmd():
                    ShowTasks("active");
                    break;
                case string cmd when cmd == "/showalltasks" && CanRunCmd():
                    ShowTasks();
                    break;
            }
        }

        ///////////////////////
        // Команда запуска/инициализации приложения
        void StartApp()
        {
            CurrentUser = UserService.GetUser(update.Message.From.Id);
            
            if (CurrentUser is null)
                CurrentUser = UserService.RegisterUser(update.Message.From.Id, update.Message.From.Username);
                
            // CurrentUser = new UserService().RegisterUser(update.Message.From.Id, update.Message.From.Username);
            botClient.SendMessage(update.Message.Chat, $"Здравствуйте {CurrentUser.TelegramUserName}.");

            botClient.SendMessage(update.Message.Chat,
                "Введите максимально допустимое количество задач (от 1 до 100): ");
            TaskService.TaskCountLimit = ParseAndValidateInt(Console.ReadLine(), 1, 100);
            
            botClient.SendMessage(update.Message.Chat,
                "Введите максимально допустимую длину задачи: ");
            TaskService.TaskLengthLimit = ParseAndValidateInt(Console.ReadLine(), 1, 255);
            
            botClient.SendMessage(update.Message.Chat,
                "Параметры работы приложения заданы. Введите команду для продолжения работы.");
        }

        ///////////////////////
        // Отображение версии/информации о приложении
        void ShowInfo()
        {
            botClient.SendMessage(update.Message.Chat, versionText);
        }

        ///////////////////////
        // Отображение доступных команд 
        void ShowHelp()
        {
            botClient.SendMessage(update.Message.Chat, "Список доступных команд:");
            botClient.SendMessage(update.Message.Chat, " /start          - начало работы с программой.");
            botClient.SendMessage(update.Message.Chat, " /help           - вывод описания по доступным командам.");
            botClient.SendMessage(update.Message.Chat, " /info           - техническая информация о программе.");
            botClient.SendMessage(update.Message.Chat, " /showtasks      - отобразить все задачи из списка дел.");
            botClient.SendMessage(update.Message.Chat,
                " /showalltasks   - отобразить задачи только в статусе 'Active'.");
            botClient.SendMessage(update.Message.Chat, " /addtask текст  - добавить задачу в список дел.");
            botClient.SendMessage(update.Message.Chat, " /completetask № - завершить выбранную задачу.");
            botClient.SendMessage(update.Message.Chat, " /removetask № - удалить выбранную задачу.");
            botClient.SendMessage(update.Message.Chat, " /exit           - выход из программы.");
        }

        ///////////////////////
        // Отображение списка задач. Параметр mode определяет условия фильтрации задач в списке
        // 'all' = все задачи, 'active' = только активные. 
        bool ShowTasks(string mode = "all")
        {
            if (mode == "active")
                todoList = TaskService.GetActiveByUserId(CurrentUser.UserId);
            else
                todoList = TaskService.GetAllByUserId(CurrentUser.UserId);

            if (todoList.Count == 0)
                botClient.SendMessage(update.Message.Chat, "Список задач пуст.");
            else
            {
                for (int i = 0; i < todoList.Count; i++)
                {
                    botClient.SendMessage(update.Message.Chat,
                        $"#{i + 1}. {todoList[i].CreatedAt} [{todoList[i].Id}] " +
                        $"\nCтатус: [{todoList[i].State}], изменен: {todoList[i].StateChangedAt}" +
                        $"\nТекст: {todoList[i].Name}\n");
                }
            }

            return todoList.Count > 0;
        }

        ///////////////////////
        // Добавление задач в список. Текст задачи указывается после команды.
        void AddTask()
        {
            var taskText = update.Message.Text.Substring("/addtask".Length).Trim();
            
            ValidateString(taskText);
            
            if (!string.IsNullOrWhiteSpace(taskText))
            {
                TaskService.Add(CurrentUser, taskText);
                botClient.SendMessage(update.Message.Chat, "Задача добавлена.");
            }
            else
                botClient.SendMessage(update.Message.Chat, "Текст задачи не задан.");
        }

        ///////////////////////
        // Завершение задачи. Номер завершаемой задачи указывается после команды.
        void CompleteTask()
        {
            if (ShowTasks("active"))
            {
                todoList = TaskService.GetActiveByUserId(CurrentUser.UserId);
                int.TryParse(update.Message.Text.Substring("/completetask".Length).Trim(), out var taskNumber);

                if (taskNumber > 0 && taskNumber <= todoList.Count)
                {
                    TaskService.MarkCompleted(todoList[taskNumber - 1].Id);
                    botClient.SendMessage(update.Message.Chat, $"Задача №{taskNumber} завершена.");
                }
                else
                    botClient.SendMessage(update.Message.Chat, $"Задача №{taskNumber} отсутствует в списке.");
            }
        }

        ///////////////////////
        // Удаление задачи. Номер удаляемой задачи указывается после команды.
        void RemoveTask()
        {
            if (ShowTasks("active"))
            {
                todoList = TaskService.GetActiveByUserId(CurrentUser.UserId);
                int.TryParse(update.Message.Text.Substring("/removetask".Length).Trim(), out var taskNumber);

                if (taskNumber > 0 && taskNumber <= todoList.Count)
                {
                    TaskService.Delete(todoList[taskNumber - 1].Id);
                    botClient.SendMessage(update.Message.Chat, $"Задача №{taskNumber} удалена.");
                }
                else
                    botClient.SendMessage(update.Message.Chat, $"Задача №{taskNumber} отсутствует в списке.");
            }
        }

        ///////////////////////
        // Проверка возможности запуска необходимой команды.
        bool CanRunCmd()
        {
            if (CurrentUser != null)
                return true;
            else
            {
                botClient.SendMessage(update.Message.Chat,
                    $"Использование команды недоступно, т.к. пользователь не зарегистрирован." +
                    $"\n Необходимо выполнить команду '/start'.\n");

                return false;
            }
        }
        
        int ParseAndValidateInt(string? str, int min, int max)
        {
            int number;
            int.TryParse(str, out number);
        
            if (number < min || number > max)
                throw new ArgumentException($"Строка не является числом, либо выходит за пределы указанного диапазона от {min} до {max}.");
        
            return number;
        }
    
        void ValidateString(string? str)
        {
            if (string.IsNullOrEmpty(str) || string.IsNullOrWhiteSpace(str)) 
                throw new ArgumentException("Введенное значение не должно быть равно null, пустой строке или пробелу.");
        }
    }
}
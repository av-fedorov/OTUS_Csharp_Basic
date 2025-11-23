using Otus.ToDoList.ConsoleBot;
using Otus.ToDoList.ConsoleBot.Types;


namespace Homework;

public class UpdateHandler : IUpdateHandler
{
    private static string versionText = "Версия программы: 02.16.01, дата создания: 18.11.2025.";
    private static List<ToDoItem> todoList = new List<ToDoItem>();
    private static string command;
    private static ToDoUser User = null;

    public void HandleUpdateAsync(ITelegramBotClient botClient, Update update)
    {
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
            case "/addtask":
                if (CanRunCmd()) AddTask();
                break;
            case "/completetask":
                if (CanRunCmd()) CompleteTask();
                break;
            case "/showtasks":
                if (CanRunCmd()) ShowTasks("Active");
                break;
            case "/showalltasks":
                if (CanRunCmd()) ShowTasks("All");
                break;
        }

        void StartApp()
        {
            // 1) Не нужно запрашивать имя
            // Добавить использование IUserService в UpdateHandler. Получать IUserService нужно через конструктор
            // 2) Для обработки команды нужно использовать IUserService.GetUser.
            // Если пользователь не найден, то вызывать IUserService.RegisterUser
            // 3) Если пользователь не зарегистрирован, то ему доступны только команды /help /info
            
            if (User == null)
                User = new UserService().RegisterUser(update.Message.From.Id, update.Message.From.Username);
            
            botClient.SendMessage(update.Message.Chat, $"Здравствуйте {User.TelegramUserName}.");
        }

        void ShowInfo()
        {
            botClient.SendMessage(update.Message.Chat, versionText);
        }

        void ShowHelp()
        {
            botClient.SendMessage(update.Message.Chat, "Список доступных команд:");
            botClient.SendMessage(update.Message.Chat, " /start          - начало работы с программой.");
            botClient.SendMessage(update.Message.Chat, " /help           - вывод описания по доступным командам.");
            botClient.SendMessage(update.Message.Chat, " /info           - техническая информация о программе.");
            botClient.SendMessage(update.Message.Chat, " /showtasks      - отобразить все задачи из списка дел.");
            botClient.SendMessage(update.Message.Chat, " /showalltasks   - отобразить задачи только в статусе 'Active'.");
            botClient.SendMessage(update.Message.Chat, " /addtask        - добавить задачу в список дел.");
            botClient.SendMessage(update.Message.Chat, " /completetask   - завершить выбранную задачу.");
            botClient.SendMessage(update.Message.Chat, " /exit           - выход из программы.");
        }

        bool ShowTasks(string mode = "all")
        {
            int taskCount = 0;

            if (todoList.Count == 0)
            {
                botClient.SendMessage(update.Message.Chat, "Список задач пуст.");
            }
            else if (mode == "Active")
            {
                botClient.SendMessage(update.Message.Chat, "Cписок текущих задач: ");

                for (int i = 0; i < todoList.Count; i++)
                {
                    if (todoList[i].State == ToDoItem.ToDoItemState.Active)
                    {
                        botClient.SendMessage(update.Message.Chat, 
                            $"#{i + 1}. {todoList[i].CreatedAt} [{todoList[i].Id}] " + $"\nТекст: {todoList[i].Name}\n");

                        taskCount++;
                    }
                }
            }
            else if (mode == "All")
            {
                botClient.SendMessage(update.Message.Chat, "Cписок текущих задач: ");

                for (int i = 0; i < todoList.Count; i++)
                {
                    botClient.SendMessage(update.Message.Chat, $"#{i + 1}. {todoList[i].CreatedAt} [{todoList[i].Id}] " +
                                      $"\nCтатус: [{todoList[i].State}], изменен: {todoList[i].StateChangedAt}" +
                                      $"\nТекст: {todoList[i].Name}\n");
                    taskCount++;
                }
            }

            return taskCount > 0;
        }

        void AddTask()
        {
            // string taskText = "";
            //
            // do
            // {
            //     botClient.SendMessage(update.Message.Chat, "Введите описание задачи: ");
            //     taskText = Console.ReadLine();
            // } while (string.IsNullOrWhiteSpace(taskText));
            //
            // todoList.Add(new ToDoItem(User, taskText));
            // botClient.SendMessage(update.Message.Chat, "\nЗадача добавлена.");
        }

        void CompleteTask()
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
                        botClient.SendMessage(update.Message.Chat, $"Задача с идентификатором [{todoList[i].Id.ToString()}] завершена.");
                    }
                }
            }
        }

        bool CanRunCmd()
        {
            if (User != null)
            {
                return true;
            }
            else
            {
                botClient.SendMessage(update.Message.Chat,
                    $"Использование команды недоступно, т.к. пользователь не зарегистрирован." +
                    $"\n Необходимо выполнить команду '/start'.\n");
                
                return false;
            }
        }
    }
}
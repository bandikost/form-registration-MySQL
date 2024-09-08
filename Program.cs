using System;
using MySql.Data.MySqlClient;
using System.Threading;

class Program
{
    static bool stopAnimation = false;
    static void Main()
    {

        string connectionString = "Server=localhost;Database=SECRET;User ID=SECRET;Password=SECRET;";

        try
        {
            using MySqlConnection connection = new(connectionString);

            connection.Open();
            Console.WriteLine("| Главное меню |");
            Console.WriteLine("");
            while (true)
            {
                Console.WriteLine("1. Регистрация");
                Console.WriteLine("2. Авторизация");
                Console.WriteLine("3. Выход");
                Console.WriteLine("");
                Console.Write("Выберите действие: ");
                
                string? choice = Console.ReadLine();

                if (choice == "1")
                {
                    Register(connection);
                }
                else if (choice == "2")
                {
                    Login(connection);
                }
                else if (choice == "3")
                {
                    Console.WriteLine("Выход из программы.");
                    break;
                }
                else
                {
                    Console.WriteLine("Неверный выбор. Попробуйте снова.");
                }
                break;
            }

        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка: {ex.Message}");
        }


    }

   

    static void Register(MySqlConnection connection)
    {
        while (true)
        {
            Console.Write("Введите имя пользователя: ");
            string? username = Console.ReadLine();
            if (string.IsNullOrEmpty(username))
            {
                Console.WriteLine("Имя не может быть пустым.");
                continue;
            }

            Console.Write("Введите email: ");
            string? email = Console.ReadLine();

            if (string.IsNullOrEmpty(email))
            {
                Console.WriteLine("Почта не может быть пустой.");

            }

            Console.Write("Введите пароль: ");
            string? user_password = Console.ReadLine();

            if (string.IsNullOrEmpty(user_password))
            {
                Console.WriteLine("Пароль не может быть пустым.");

            }

            string insertQuery = "INSERT INTO users (username, email, user_password) VALUES (@username, @email, @user_password)";

            using MySqlCommand cmd = new(insertQuery, connection);

            cmd.Parameters.AddWithValue("@username", username);
            cmd.Parameters.AddWithValue("@email", email);
            cmd.Parameters.AddWithValue("@user_password", user_password);
            cmd.ExecuteNonQuery();
            Console.WriteLine("Данные успешно вставлены.");
            Console.WriteLine("Вы перенаправлены на авторизацию.");
            Login(connection);

            break;
        }
    }



    static void Login(MySqlConnection connection)
    {
        int attemptschecker = 0;

        while (true)
        {
            Console.WriteLine("");
            Console.WriteLine("Введите 1, чтобы вернуться в главное меню: ");

            Console.Write("Введите email: ");
            string? email = Console.ReadLine();

            if (string.IsNullOrEmpty(email))
            {
                Console.WriteLine("Почта и пароль не могут быть пустыми.");

            }
            else if (email == "1")
            {
                Main();      
                break;
            }

            Console.Write("Введите пароль: ");
            string? user_password = Console.ReadLine();

            if (string.IsNullOrEmpty(user_password))
            {
                Console.WriteLine("Почта и пароль не могут быть пустыми.");

            }

            


            if (attemptschecker == 2)
            {
                Console.Write("Извините, вы превысили попытки ");
                break;
            }

            string selectQuery = "SELECT * FROM users WHERE email = @Email AND user_password = @Password";
            using MySqlCommand cmd = new(selectQuery, connection);
            cmd.Parameters.AddWithValue("@Email", email);
            cmd.Parameters.AddWithValue("@Password", user_password);

            using MySqlDataReader reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                Console.WriteLine("");
                Console.WriteLine($"Авторизация успешна. Добро пожаловать, {reader["username"]}!");
                Console.WriteLine("");
                reader.Close();
                ManagerAccount(connection, email, user_password);
            }
            else
            {
                Console.WriteLine("Неверный email или пароль.");
                attemptschecker++;
                continue;
            }
            break;
        }


    }


    static void ManagerAccount(MySqlConnection connection, string email, string user_password)
    {
        Console.WriteLine("Вы находитесь в управлении аккаунта");
        while (true)
        {
            
            Console.WriteLine("1. Удалить Аккаунт");
            Console.WriteLine("2. Изменить Ваши данные");
            Console.WriteLine("3. Назад ");
            Console.WriteLine("");
            Console.Write("Выберите действие: ");

            string? answer = Console.ReadLine();

            if (string.IsNullOrEmpty(answer))
            {
                Console.WriteLine("поле не может быть пустым");
            }
            

            if (answer == "1")
            {
                DeleteAccount(connection, email, user_password);
                break;
            }
            else if (answer == "2")

            {
                UpdateAccount(connection, email, user_password);
                break;
            }
            else if (answer == "3")
            {
                Main();
            }
            else continue;
            break;
        }


        static void DeleteAccount(MySqlConnection connection, string email, string user_password)
        {
            while (true)
            {
                string deleteQuery = "DELETE FROM users WHERE email = @Email AND user_password = @Password";
                using MySqlCommand cmd = new(deleteQuery, connection);
                cmd.Parameters.AddWithValue("@Email", email);
                cmd.Parameters.AddWithValue("@Password", user_password);

                int rowsAffected = cmd.ExecuteNonQuery();
                if (rowsAffected > 0)  // Если удаление успешно
                {
                    Console.WriteLine("Удаление успешно.");
                    break;
                }
                else  // Если пользователь не найден или пароль неверен
                {
                    Console.WriteLine("Неверный email или пароль.");
                    continue;
                }
            }
        }



        static void UpdateAccount(MySqlConnection connection, string oldEmail, string oldPassword)
        {
            
            while (true)
            {
                Console.WriteLine("");
                Console.WriteLine($"Текущий email: {oldEmail}");

                if (string.IsNullOrEmpty(oldEmail) || string.IsNullOrEmpty(oldPassword))
                {
                    Console.WriteLine("Текущий email и пароль не могут быть пустыми.");
                    continue;
                }

                Console.Write("Введите новое имя: ");
                string? username = Console.ReadLine();

                Console.Write("Введите новый email: ");
                string? email = Console.ReadLine();

                Console.Write("Введите новый пароль: ");
                string? user_password = Console.ReadLine();

                if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(user_password))
                {
                    Console.WriteLine("Поля не могут быть пустыми.");
                    continue;
                }

                Console.Write("Загрузка ");
                Thread loadingThread = new(LoadingAnimation);
                loadingThread.Start();

                // Имитируем работу программы (например, подключение к БД или выполнение задачи)
                Thread.Sleep(5000); // Ожидание 5 секунд

                // Остановка анимации
                stopAnimation = true;
                loadingThread.Join(); // Дождаться завершения анимации

                Console.WriteLine("\nЗагрузка завершена.");

                string updateQuery = "UPDATE users SET username = @Username, email = @Email, user_password = @Password  WHERE  email = @OldEmail AND user_password = @OldEmail ";

                using MySqlCommand cmd = new(updateQuery, connection);

                cmd.Parameters.AddWithValue("@Username", username);
                cmd.Parameters.AddWithValue("@Email", email);
                cmd.Parameters.AddWithValue("@Password", user_password);
                
                cmd.Parameters.AddWithValue("@OldEmail", oldEmail);
                cmd.Parameters.AddWithValue("@OldPassword", oldPassword);

                int rowsAffected = cmd.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    Console.WriteLine("Данные успешно обновлены.");
                }
                else
                {
                    Console.WriteLine("Данные не обновлены, возможно, запись не найдена.");
                }

                break;
               
            }
        }


        static void LoadingAnimation()
        {
            string[] animationChars = ["|", "/", "-", "\\"];  // Массив символов для анимации
            int index = 0;

            while (!stopAnimation)
            {
                Console.Write(animationChars[index % animationChars.Length]);  // Вывод символа
                Console.SetCursorPosition(Console.CursorLeft - 1, Console.CursorTop);  // Возврат курсора для обновления символа
                index++;
                Thread.Sleep(100);  // Задержка перед следующим шагом анимации
            }
        }

    }
}


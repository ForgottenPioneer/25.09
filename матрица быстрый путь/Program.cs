using System;
using System.Collections.Generic;

class Program
{
    static void Main()
    {
        int n, m;
        do
        {
            Console.Write("Введите количество строк (n > 1): ");
        } while (!int.TryParse(Console.ReadLine(), out n) || n <= 1);

        do
        {
            Console.Write("Введите количество столбцов (m > 1): ");
        } while (!int.TryParse(Console.ReadLine(), out m) || m <= 1);

        bool[,] obstacles = new bool[n, m];
        Console.WriteLine("Введите координаты препятствий в формате 'x y' (или 'exit' для завершения):");

        while (true)
        {
            string input = Console.ReadLine();
            if (input.ToLower() == "exit") break;

            var coords = input.Split(' ');
            if (coords.Length == 2 &&
                int.TryParse(coords[0], out int x) &&
                int.TryParse(coords[1], out int y) &&
                x >= 0 && x < n && y >= 0 && y < m)
            {
                obstacles[x, y] = true;
            }
            else
            {
                Console.WriteLine("Некорректные координаты. Попробуйте снова.");
            }
        }

        int pathCount = CountPaths(n, m, obstacles);
        Console.WriteLine($"Количество возможных путей: {pathCount}");

        List<string> routes = new List<string>();
        FindAllPaths(0, 0, n, m, obstacles, "", routes);

        Console.WriteLine("Все возможные маршруты:");
        foreach (var route in routes)
        {
            Console.WriteLine(route);
        }
    }

    static int CountPaths(int n, int m, bool[,] obstacles)
    {
        int[,] dp = new int[n, m];

        // Инициализация первой строки и первого столбца
        for (int i = 0; i < n; i++)
        {
            if (obstacles[i, 0]) break;
            dp[i, 0] = 1;
        }

        for (int j = 0; j < m; j++)
        {
            if (obstacles[0, j]) break;
            dp[0, j] = 1;
        }

        // Заполнение таблицы динамического программирования
        for (int i = 1; i < n; i++)
        {
            for (int j = 1; j < m; j++)
            {
                if (!obstacles[i, j])
                {
                    dp[i, j] = dp[i - 1, j] + dp[i, j - 1];
                }
            }
        }

        return dp[n - 1, m - 1];
    }

    static void FindAllPaths(int x, int y, int n, int m, bool[,] obstacles, string path, List<string> routes)
    {
        // Если вышли за пределы или на препятствие
        if (x >= n || y >= m || obstacles[x, y]) return;

        // Если достигли правого нижнего угла
        if (x == n - 1 && y == m - 1)
        {
            routes.Add(path);
            return;
        }

        // Движение вправо
        FindAllPaths(x, y + 1, n, m, obstacles, path + " вправо", routes);

        // Движение вниз
        FindAllPaths(x + 1, y, n, m, obstacles, path + " вниз", routes);
    }
}

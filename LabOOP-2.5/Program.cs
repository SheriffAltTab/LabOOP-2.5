using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

class Program
{
    static void Main(string[] args)
    {
        while (true)
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.InputEncoding = Encoding.UTF8;
            Console.WriteLine("Введіть номер завдання (1-3) або 0 для виходу:");
            int taskNumber = Convert.ToInt32(Console.ReadLine());

            if (taskNumber == 0)
            {
                break;
            }

            switch (taskNumber)
            {
                case 1:
                    Console.WriteLine("Введіть розмір дошки:");
                    int n = Convert.ToInt32(Console.ReadLine());
                    var solutions = SolveNQueens(n);
                    foreach (var solution in solutions)
                    {
                        foreach (var row in solution)
                        {
                            Console.WriteLine(row);
                        }
                        Console.WriteLine();
                    }
                    break;
                case 2:
                    Console.WriteLine("Введіть елементи першого масиву, розділені пробілами:");
                    var nums1 = Console.ReadLine().Split().Select(int.Parse).ToArray();
                    Console.WriteLine("Введіть елементи другого масиву, розділені пробілами:");
                    var nums2 = Console.ReadLine().Split().Select(int.Parse).ToArray();
                    double median = FindMedianSortedArrays(nums1, nums2);
                    Console.WriteLine($"Медіана двох відсортованих масивів: {median}");
                    break;
                case 3:
                    Console.WriteLine("Введіть початкове слово:");
                    string beginWord = Console.ReadLine();
                    Console.WriteLine("Введіть кінцеве слово:");
                    string endWord = Console.ReadLine();
                    Console.WriteLine("Введіть словник, розділені пробілами:");
                    var wordList = new List<string>(Console.ReadLine().Split());
                    int transformationLength = LadderLength(beginWord, endWord, wordList);
                    Console.WriteLine($"Кількість слів у найкоротшій послідовності перетворення: {transformationLength}");
                    break;
                default:
                    Console.WriteLine("Невірний номер завдання.");
                    break;
            }
        }
    }

    // Завдання 1: Головоломка з n ферзями
    public static IList<IList<string>> SolveNQueens(int n)
    {
        var results = new List<IList<string>>();
        SolveNQueens(new int[n], 0, results);
        return results;
    }

    private static void SolveNQueens(int[] queens, int row, IList<IList<string>> results)
    {
        int n = queens.Length;
        if (row == n)
        {
            var board = new string[n];
            for (int i = 0; i < n; i++)
            {
                char[] rowChars = new char[n];
                Array.Fill(rowChars, '.');
                rowChars[queens[i]] = 'Q';
                board[i] = new string(rowChars);
            }
            results.Add(board);
        }
        else
        {
            for (int col = 0; col < n; col++)
            {
                if (IsValid(queens, row, col))
                {
                    queens[row] = col;
                    SolveNQueens(queens, row + 1, results);
                }
            }
        }
    }

    private static bool IsValid(int[] queens, int row, int col)
    {
        for (int i = 0; i < row; i++)
        {
            if (queens[i] == col || queens[i] - i == col - row || queens[i] + i == col + row)
            {
                return false;
            }
        }
        return true;
    }

    // Завдання 2: Медіана двох відсортованих масивів
    public static double FindMedianSortedArrays(int[] nums1, int[] nums2)
    {
        if (nums1.Length > nums2.Length)
        {
            var temp = nums1;
            nums1 = nums2;
            nums2 = temp;
        }

        int x = nums1.Length;
        int y = nums2.Length;
        int low = 0;
        int high = x;

        while (low <= high)
        {
            int partitionX = (low + high) / 2;
            int partitionY = (x + y + 1) / 2 - partitionX;

            int maxLeftX = (partitionX == 0) ? int.MinValue : nums1[partitionX - 1];
            int minRightX = (partitionX == x) ? int.MaxValue : nums1[partitionX];

            int maxLeftY = (partitionY == 0) ? int.MinValue : nums2[partitionY - 1];
            int minRightY = (partitionY == y) ? int.MaxValue : nums2[partitionY];

            if (maxLeftX <= minRightY && maxLeftY <= minRightX)
            {
                if ((x + y) % 2 == 0)
                {
                    return ((double)Math.Max(maxLeftX, maxLeftY) + Math.Min(minRightX, minRightY)) / 2;
                }
                else
                {
                    return (double)Math.Max(maxLeftX, maxLeftY);
                }
            }
            else if (maxLeftX > minRightY)
            {
                high = partitionX - 1;
            }
            else
            {
                low = partitionX + 1;
            }
        }

        throw new ArgumentException();
    }

    // Завдання 3: Кількість слів у найкоротшій послідовності перетворення
    public static int LadderLength(string beginWord, string endWord, IList<string> wordList)
    {
        var wordSet = new HashSet<string>(wordList);
        if (!wordSet.Contains(endWord))
        {
            return 0;
        }

        var beginSet = new HashSet<string> { beginWord };
        var endSet = new HashSet<string> { endWord };
        int length = 1;

        while (beginSet.Any())
        {
            var nextSet = new HashSet<string>();
            foreach (var word in beginSet)
            {
                for (int i = 0; i < word.Length; i++)
                {
                    for (char c = 'a'; c <= 'z'; c++)
                    {
                        var newWord = word.Substring(0, i) + c + word.Substring(i + 1);
                        if (endSet.Contains(newWord))
                        {
                            return length + 1;
                        }
                        if (wordSet.Contains(newWord))
                        {
                            nextSet.Add(newWord);
                            wordSet.Remove(newWord);
                        }
                    }
                }
            }

            beginSet = (nextSet.Count < endSet.Count) ? nextSet : endSet;
            endSet = (beginSet == nextSet) ? endSet : nextSet;
            length++;
        }

        return 0;
    }
}

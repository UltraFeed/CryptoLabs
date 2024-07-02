#pragma warning disable CA1303
#pragma warning disable CS8604

namespace CryptoLabs.MenuItems;

internal sealed class MenuItemTerm_1_Lab_6 : MenuItemCore
{
    private static readonly byte [] keys = [1, 2, 3, 4, 5, 0];

    internal override string Title => $"Feistel cipher";

    internal override void Execute ()
    {
        Console.Clear();
        int a = Utilities.GetInt($"Enter {nameof(a)} (default: 4):", 4);
        int b = Utilities.GetInt($"Enter {nameof(b)}: (default: 3)", 3);
        Console.Clear();

        while (true)
        {
            Console.WriteLine($"Feistel cipher");
            Console.WriteLine($"0. Exit");
            Console.WriteLine($"1. Encrypt");
            Console.WriteLine($"2. Decrypt");
            Console.WriteLine($"{nameof(a)} = {a}");
            Console.WriteLine($"{nameof(b)} = {b}");

            if (a == b)
            {
                Console.WriteLine($"{nameof(a)} = {a} must be different from the {nameof(b)} = {b}");
                Utilities.WaitForKey();
                break;
            }

            int input = Utilities.GetInt($"{nameof(input)} In Range (0-2): ");

            if (input == 0)
            {
                Utilities.WaitForKey();
                break;
            }

            else if (input == 1)
            {
                using OpenFileDialog openFileDialog = new()
                {
                    Filter = "All files (*.*)|*.*",
                    Title = "Choose a file to encrypt"
                };

                // Если пользователь выбирает файл и нажимает "ОК"
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string inputFile = openFileDialog.FileName;
                    string outputFile = Path.Combine(Path.GetDirectoryName(inputFile), Path.GetFileNameWithoutExtension(inputFile) + "_encrypted" + Path.GetExtension(inputFile));
                    byte [] data = File.ReadAllBytes(inputFile); // Читаем байты из файла
                    byte [] encryptedData = ProcessData(data, a, b, false); // Шифруем данные с использованием ключа
                    File.WriteAllBytes(outputFile, encryptedData); // Записываем зашифрованные данные в файл
                    Console.WriteLine("File encrypted");
                }
            }

            else if (input == 2)
            {
                // Открываем диалоговое окно для выбора файла для расшифровки
                using OpenFileDialog openFileDialog = new()
                {
                    Filter = "All files (*.*)|*.*",
                    Title = "Choose a file to decrypt"
                };

                // Если пользователь выбирает файл и нажимает "ОК"
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string inputFile = openFileDialog.FileName;
                    string outputFile = Path.Combine(Path.GetDirectoryName(inputFile), Path.GetFileNameWithoutExtension(inputFile) + "_decrypted" + Path.GetExtension(inputFile));
                    byte [] data = File.ReadAllBytes(inputFile); // Читаем байты из файла
                    byte [] decryptedData = ProcessData(data, a, b, true); // Расшифровываем данные с использованием ключа
                    File.WriteAllBytes(outputFile, decryptedData); // Записываем расшифрованные данные в файл
                    Console.WriteLine("File decrypted");
                }
            }

            else
            {
                Console.WriteLine("Unknown Input");
            }

            Utilities.WaitForKey();

        }
    }

    private static int FunctionF (int x, int key, int a, int b)
    {
        return (byte) ((x << a) ^ (x << b) ^ key);
    }

    private static byte [] ProcessData (byte [] input, int a, int b, bool reverse)
    {
        int rounds = keys.Length;

        //цикл по каждому байту
        for (int j = 0; j < input.Length; j++)
        {
            int left = input [j] >> 4;   // первые 4 бита
            int right = input [j] & 0x0F; // последние 4 бита

            int round = reverse ? rounds - 1 : 0;
            int inc = reverse ? -1 : 1;

            for (int i = 0; i < rounds; i++)
            {
                if (i < rounds - 1) // если не последний раунд
                {
                    int temp = left;
                    left = right ^ FunctionF(left, keys [round], a, b);
                    right = temp;
                }
                else // последний раунд
                {
                    right ^= FunctionF(left, keys [round], a, b);
                }

                round += inc;
            }
            // объединяем left и right обратно в один байт
            input [j] = (byte) ((left << 4) + (right & 0x0F));
        }

        return input;
    }
}
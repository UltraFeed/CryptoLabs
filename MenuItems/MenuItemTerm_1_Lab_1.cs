#pragma warning disable CA1303
#pragma warning disable CS8604

using System.Numerics;

namespace CryptoLabs.MenuItems;

internal sealed class MenuItemTerm_1_Lab_1 : MenuItemCore
{
    internal override string Title => $"Affine cipher bruteforce";

    internal override void Execute ()
    {
        Console.Clear();
        int m = Utilities.GetInt($"Enter {nameof(m)} (default: 256): ", 256);
        int a = Utilities.GetInt($"Enter {nameof(a)} (default: 7):", 7);
        int b = Utilities.GetInt($"Enter {nameof(b)}: (default: 18)", 18);
        Console.Clear();

        while (true)
        {
            Console.WriteLine($"Affine cipher");
            Console.WriteLine($"0. Exit");
            Console.WriteLine($"1. Encrypt");
            Console.WriteLine($"2. Decrypt");
            Console.WriteLine($"{nameof(m)} = {m}");
            Console.WriteLine($"{nameof(a)} = {a}");
            Console.WriteLine($"{nameof(b)} = {b}");

            if (BigInteger.GreatestCommonDivisor(a, m) != 1)
            {
                Console.WriteLine($"{nameof(a)} = {a} must be coprime with {nameof(m)} = {m}");
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
                    byte [] encryptedData = EncryptData(data, a, b, m); // Шифруем данные с использованием ключа
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
                    byte [] decryptedData = DecryptData(data, a, b, m); // Расшифровываем данные с использованием ключа
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

    // Функция для шифрования данных
    private static byte [] EncryptData (byte [] data, int a, int b, int m)
    {
        for (int i = 0; i < data.Length; i++)
        {
            // Вычисляем новое значение байта данных, применяя к нему аффинное преобразование
            data [i] = (byte) (((a * data [i]) + b) % m);
        }

        return data;
    }

    // Функция для дешифрования данных
    private static byte [] DecryptData (byte [] data, int a, int b, int m)
    {
        // Вычисляем обратное значение "a" по модулю "m"
        int inverseA = Utilities.ModInverse(a, m);

        for (int i = 0; i < data.Length; i++)
        {
            // Вычисляем новое значение байта данных, применяя обратное аффинное преобразование
            data [i] = (byte) (inverseA * (data [i] - b + m) % m);
        }

        return data;
    }
}

#pragma warning disable CA1303
#pragma warning disable CA1305
#pragma warning disable CS8604

namespace CryptoLabs.MenuItems;

internal sealed class MenuItemTerm_1Lab_2 : MenuItemCore
{
	internal override string Title => $"Transposition cipher";

	internal override void Execute ()
	{
		Console.Clear();
		int key = Utilities.GetInt($"Enter {nameof(key)} (default: 651234): ", 651234);
		Console.Clear();

		while (true)
		{
			Console.WriteLine($"Transposition cipher");
			Console.WriteLine($"0. Exit");
			Console.WriteLine($"1. Encrypt");
			Console.WriteLine($"2. Decrypt");
			Console.WriteLine($"{nameof(key)} = {key}");

			if (!IsUniqueSixDigits(key.ToString()))
			{
				Console.WriteLine($"{nameof(key)} must contain six unique numbers from {1} to {6}");
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

					byte [] keyBytes = BitConverter.GetBytes(key); // Создаём из введённой строки массив с ключом
					byte [] data = File.ReadAllBytes(inputFile); // Читаем байты из файла
					byte [] encryptedData = EncryptData(data, keyBytes); // Шифруем данные с использованием ключа
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

					byte [] keyBytes = BitConverter.GetBytes(key); // Создаём из введённой строки массив с ключом
					byte [] data = File.ReadAllBytes(inputFile); // Читаем байты из файла
					byte [] decryptedData = DecryptData(data, keyBytes); // Расшифровываем данные с использованием ключа
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
	private static byte [] EncryptData (byte [] data, byte [] key)
	{
		// Создаем буфер для хранения зашифрованных данных такой же длины, как и исходные данные
		byte [] encryptedData = new byte [data.Length];

		// Проходим по каждому байту исходных данных
		for (int i = 0; i < data.Length; i++)
		{
			// Зашифровываем байт, складывая его с соответствующим байтом ключа (циклический выбор ключевого байта)
			encryptedData [i] = (byte) (data [i] + key [i % key.Length]);
		}

		// Возвращаем зашифрованные данные
		return encryptedData;
	}

	// Функция для дешифрования данных
	private static byte [] DecryptData (byte [] data, byte [] key)
	{
		// Создаем буфер для хранения расшифрованных данных такой же длины, как и исходные данные
		byte [] decryptedData = new byte [data.Length];

		// Проходим по каждому байту исходных данных
		for (int i = 0; i < data.Length; i++)
		{
			// Расшифровываем байт, вычитая из него соответствующий байт ключа (циклический выбор ключевого байта)
			decryptedData [i] = (byte) (data [i] - key [i % key.Length]);
		}

		// Возвращаем расшифрованные данные
		return decryptedData;
	}

	// Функция для проверки, что строка состоит из шести уникальных цифр
	private static bool IsUniqueSixDigits (string input)
	{
		if (input.Length != 6)
		{
			return false;
		}

		// Создаем HashSet для отслеживания уникальных цифр
		HashSet<char> uniqueDigits = [];

		foreach (char c in input)
		{
			if (c is < '1' or > '6')
			{
				return false;
			}

			// Пытаемся добавить символ в HashSet
			if (!uniqueDigits.Add(c))
			{
				// Если символ уже присутствует в HashSet, это означает, что он не уникален, поэтому возвращаем false
				return false;
			}
		}

		return true;
	}
}
#pragma warning disable CS8604

using System.Collections;

namespace CryptoLabs.MenuItems;

internal class MenuItemTerm_1Lab_5 : MenuItemCore
{
	internal override string Title => $"S-box";

	// Задаём наш S-блок
	private static readonly byte [,] sBlockArray =
		{
				{ 4, 11, 2, 14, 15, 0, 8, 13, 3, 12, 9, 7, 5, 10, 6, 1 },
				{ 13, 0, 11, 7, 4, 9, 1, 10, 14, 3, 5, 12, 2, 15, 8, 6 },
				{ 1, 4, 11, 13, 12, 3, 7, 14, 10, 15, 6, 8, 0, 5, 9, 2 },
				{ 6, 11, 13, 8, 1, 4, 10, 7, 9, 5, 0, 15, 14, 2, 3, 12 }
		};

	internal override void Execute ()
	{
		Console.Clear();
		int a = Utilities.GetInt($"Enter {nameof(a)} (default: 5):", 5);
		int b = Utilities.GetInt($"Enter {nameof(b)}: (default: 3)", 3);
		Console.Clear();

		while (true)
		{
			Console.WriteLine($"S-box");
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
			else if (a <= 0 || b <= 0)
			{
				Console.WriteLine($"{nameof(a)} = {a} and {nameof(b)} = {b} must be greater than {0}");
				Utilities.WaitForKey();
				break;
			}
			else if (a > 5 || b > 5)
			{
				Console.WriteLine($"{nameof(a)} = {a} and {nameof(b)} = {b} must be smaller than {5}");
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
				OpenFileDialog openFileDialog = new()
				{
					Filter = "All files (*.*)|*.*",
					Title = "Choose a file to encrypt"
				};

				// Если пользователь выбирает файл и нажимает "ОК"
				if (openFileDialog.ShowDialog() == DialogResult.OK)
				{
					string inputFile = openFileDialog.FileName;
					string outputFile = Path.Combine(Path.GetDirectoryName(inputFile), Path.GetFileNameWithoutExtension(inputFile) + "_encrypted" + Path.GetExtension(inputFile));
					byte [] inputBytes = File.ReadAllBytes(inputFile);
					BitArray outputBits = EncryptData(sBlockArray, new BitArray(inputBytes), a, b);
					byte [] outputBytes = new byte [outputBits.Length / 8];
					outputBits.CopyTo(outputBytes, 0);
					File.WriteAllBytes(outputFile, outputBytes);
					Console.WriteLine("File encrypted");
				}
			}

			else if (input == 2)
			{
				// Открываем диалоговое окно для выбора файла для расшифровки
				OpenFileDialog openFileDialog = new()
				{
					Filter = "All files (*.*)|*.*",
					Title = "Choose a file to decrypt"
				};

				// Если пользователь выбирает файл и нажимает "ОК"
				if (openFileDialog.ShowDialog() == DialogResult.OK)
				{
					string inputFile = openFileDialog.FileName;
					string outputFile = Path.Combine(Path.GetDirectoryName(inputFile), Path.GetFileNameWithoutExtension(inputFile) + "_decrypted" + Path.GetExtension(inputFile));
					byte [] inputBytes = File.ReadAllBytes(inputFile);
					BitArray outputBits = DecryptData(sBlockArray, new BitArray(inputBytes), a, b);
					byte [] outputBytes = new byte [outputBits.Length / 8];
					outputBits.CopyTo(outputBytes, 0);
					File.WriteAllBytes(outputFile, outputBytes);
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
	private static BitArray EncryptData (byte [,] sBlockArray, BitArray InputBits, int a, int b)
	{
		// Создаем новый BitArray для хранения зашифрованных данных с тем же размером, что и входные данные
		BitArray OutputBits = new(InputBits.Count);

		// цикл по 6 битам
		for (int i = 0; i < InputBits.Count / 6 * 6; i += 6)
		{
			// определяем номер строки, оператор << - побитовый сдвиг влево на 1 позицию. Арифметически тоже самое, что умножить на два
			int row = (Convert.ToInt32(InputBits [i + a]) << 1) + Convert.ToInt32(InputBits [i + b]);

			// Вычисляем номер столбца, исключая биты a и b
			int column = 0;
			for (int j = 0; j < 6; j++)
			{
				if (j != a && j != b)
				{
					column += Convert.ToInt32(InputBits [i + j]);
					column <<= 1; // Сдвигаем биты влево на 1 позицию
				}
			}

			column >>= 1; // компенсируем лишний сдвиг

			// Получаем значение из S-блока, используя рассчитанные row и column
			BitArray BlockValue = new(new byte [] { sBlockArray [row, column] });

			// Записываем биты a и b в выходной BitArray
			OutputBits [i + 0] = InputBits [i + a];
			OutputBits [i + 1] = InputBits [i + b];

			// Записываем 4 бита из S-блока в выходной BitArray
			for (int j = 0; j < 4; j++)
			{
				OutputBits [i + j + 2] = BlockValue [j];
			}
		}

		// в конце может быть несколько битов, если входной массив не делится ровно на блоки по 6 битов - их тупо скопируем без изменения
		for (int k = 0; k < InputBits.Count % 6; k++)
		{
			OutputBits [InputBits.Count - k - 1] = InputBits [InputBits.Count - k - 1];
		}

		return OutputBits;
	}

	// Функция для дешифрования данных
	private static BitArray DecryptData (byte [,] sBlockArray, BitArray InputBits, int a, int b)
	{
		// Создаем новый BitArray для хранения расшифрованных данных с тем же размером, что и входные данные
		BitArray OutputBits = new(InputBits.Count);

		// цикл по 6 битам
		for (int i = 0; i < InputBits.Count / 6 * 6; i += 6)
		{
			// Определяем номер строки в таблице S-блоков, используя первые два бита в блоке
			int row = (Convert.ToInt32(InputBits [i]) << 1) + Convert.ToInt32(InputBits [i + 1]);

			//следующие 4 бита - значение, которое мы подставили из блока, надо получить десятичное число
			int sBlockValue = Convert.ToInt32(InputBits [i + 2]) + (Convert.ToInt32(InputBits [i + 3]) << 1) + (Convert.ToInt32(InputBits [i + 4]) << 2) + (Convert.ToInt32(InputBits [i + 5]) << 3);

			// Используем функцию FindValue для поиска исходного значения в S-блоке и создаем BitArray для бинарного представления этого значения
			BitArray BlockValue = new(new byte [] { FindValue(sBlockArray, row, sBlockValue) });

			// Записываем 2-битный номер строки в выходной BitArray, возвращая исходное значение
			OutputBits [i + a] = InputBits [i + 0];
			OutputBits [i + b] = InputBits [i + 1];

			// Преобразуем значение из таблицы в 4 бита и записываем их в выходной BitArray, возвращая исходное значение
			for (int j = 0, k = 3; j < 6; j++)
			{
				if (j != a && j != b)
				{
					OutputBits [i + j] = BlockValue [k--];
				}
			}
		}

		// в конце может быть несколько битов, если входной массив не делится ровно на блоки по 6 битов - их тупо скопируем без изменения
		for (int k = 0; k < InputBits.Count % 6; k++)
		{
			OutputBits [InputBits.Count - 1 - k] = InputBits [InputBits.Count - 1 - k];
		}

		return OutputBits;
	}

	// Функция которая возвращает значение в таблице в виде байта
	private static byte FindValue (byte [,] sBlockArray, int row, int value)
	{
		int sBlockCols = sBlockArray.Length / (sBlockArray.GetUpperBound(0) + 1); // 16
		for (byte i = 0; i < sBlockCols; i++)
		{
			if (sBlockArray [row, i] == value)
			{
				return i;
			}
		}

		return 255;
	}
}
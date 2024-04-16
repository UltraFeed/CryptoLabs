#pragma warning disable CA1303
#pragma warning disable CA1304
#pragma warning disable CA1305
#pragma warning disable CA1311

using System.Numerics;

namespace CryptoLabs.MenuItems;
internal sealed class MenuItemTerm_2_Lab_4 : MenuItemCore
{
	internal override string Title => $"RSA cipher";

	internal override void Execute ()
	{
		Console.Clear();

		BigInteger p = 227; // Первое простое число
		BigInteger q = 373; // Второе простое число
		BigInteger n = p * q; // Используется в качестве модуля при шифровании и дешифровании

		// Исходный текст
		string plaintext = "Привет Мир".ToUpper(); // Приводим все буквы к заглавным

		// Генерация нескольких пар ключей
		List<KeyPair> keyPairs = GenerateKeyPairs(p, q, 3);

		// Вывод пользователю всех пар (открытый ключ, закрытый ключ)
		Console.WriteLine("Выберите ключ:");
		for (int i = 0; i < keyPairs.Count; i++)
		{
			Console.WriteLine($"Пара {i + 1}: Открытый ключ: {keyPairs [i].publicKey}, Закрытый ключ: {keyPairs [i].privateKey}");
		}

		// Пользователь выбирает пару ключей
		int choice;
		do
		{
			choice = Utilities.GetInt("Введите номер пары ключей: ");
		} while (choice < 1 || choice > keyPairs.Count);
		KeyPair selectedKeyPair = keyPairs [choice - 1];

		// Пример шифрования и дешифрования
		Console.WriteLine();

		Console.WriteLine($"Исходный текст: {plaintext}");

		Console.WriteLine("Исходный текст в числовом представлении:");
		foreach (char c in plaintext)
		{
			Console.Write($"{CharToNumber(c)} ");
		}

		Console.WriteLine();
		Console.WriteLine($"{nameof(n)} = {n}");
		string ciphertext = Encrypt(plaintext, selectedKeyPair.publicKey, n);
		Console.WriteLine($"Зашифрованный текст: {ciphertext}");
		string decryptedText = Decrypt(ciphertext, selectedKeyPair.privateKey, n);
		Console.WriteLine($"Расшифрованный текст: {decryptedText}");

		Utilities.WaitForKey();
	}

	// Класс для хранения пары ключей
	private sealed class KeyPair
	{
		public BigInteger publicKey;
		public BigInteger privateKey;
	}

	// Алфавит
	private static readonly Dictionary<char, int> alphabet = new()
	{
		{'А', 10}, {'Б', 11}, {'В', 12}, {'Г', 13}, {'Д', 14}, {'Е', 15}, {'Ж', 16}, {'З', 17}, {'И', 18}, {'Й', 19},
		{'К', 20}, {'Л', 21}, {'М', 22}, {'Н', 23}, {'О', 24}, {'П', 25}, {'Р', 26}, {'С', 27}, {'Т', 28}, {'У', 29},
		{'Ф', 30}, {'Х', 31}, {'Ц', 32}, {'Ч', 33}, {'Ш', 34}, {'Щ', 35}, {'Ъ', 36}, {'Ы', 37}, {'Ь', 38}, {'Э', 39},
		{'Ю', 40}, {'Я', 41}, {' ', 99}
	};

	// Генерация пар ключей
	private static List<KeyPair> GenerateKeyPairs (BigInteger p, BigInteger q, int count)
	{
		List<KeyPair> keyPairs = [];

		BigInteger euler = (p - 1) * (q - 1);

		// Выбираем открытый ключ e (взаимно простое с euler)
		for (BigInteger e = 2; e < euler; e++)
		{
			if (BigInteger.GreatestCommonDivisor(e, euler) == 1)
			{
				// Вычисляем закрытый ключ d
				BigInteger d = Utilities.ModInverse(e, euler);

				// Проверяем наличие пары ключей
				if (d != BigInteger.Zero)
				{
					keyPairs.Add(new KeyPair { publicKey = e, privateKey = d });

					if (keyPairs.Count >= count)
					{
						break;
					}
				}
			}
		}

		return keyPairs;
	}

	private static int CharToNumber (char c)
	{
		return alphabet.TryGetValue(c, out int number) ? number : -1;
	}

	private static char NumberToChar (int number)
	{
		foreach (KeyValuePair<char, int> kvp in alphabet)
		{
			if (kvp.Value == number)
			{
				return kvp.Key;
			}
		}

		return '?';
	}

	// Функция для шифрования текста
	private static string Encrypt (string plaintext, BigInteger publicKey, BigInteger n)
	{
		string encryptedText = "";

		foreach (char c in plaintext)
		{
			int number = CharToNumber(c); // Преобразуем символ в число
			BigInteger encryptedNumber = BigInteger.ModPow(new BigInteger(number), publicKey, n); // Шифруем число
			encryptedText += encryptedNumber.ToString() + " "; // Добавляем зашифрованное число в зашифрованный текст
		}

		return encryptedText.Trim(); // Удаляем лишние пробелы в конце
	}

	// Функция для дешифрования текста
	private static string Decrypt (string ciphertext, BigInteger privateKey, BigInteger n)
	{
		string [] numbers = ciphertext.Split(' ', StringSplitOptions.RemoveEmptyEntries); // Разбиваем зашифрованный текст на числа
		string decryptedText = "";

		foreach (string num in numbers)
		{
			BigInteger decryptedNumber = BigInteger.ModPow(BigInteger.Parse(num), privateKey, n); // Дешифруем число
			char decryptedChar = NumberToChar((int) decryptedNumber); // Преобразуем число в символ
			decryptedText += decryptedChar; // Добавляем расшифрованный символ в расшифрованный текст
		}

		return decryptedText;
	}
}

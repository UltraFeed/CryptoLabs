#pragma warning disable CA1303

using System.Numerics;
using System.Text;

namespace CryptoLabs.MenuItems;

internal sealed class MenuItemTerm_1_Lab_4 : MenuItemCore
{
	internal override string Title => $"Transposition cipher bruteforce";

	internal override void Execute ()
	{
		char [] encodingAlphabet = ['а', 'б', 'в', 'г', 'д', 'е', 'ж', 'з', 'и', 'й', 'к', 'л', 'м', 'н', 'о', 'п', 'р', 'с', 'т', 'у', 'ф', 'х', 'ц', 'ч', 'ш', 'щ', 'ъ', 'ы', 'ь', 'э', 'ю', 'я'];
		string text = "бсиьбжгаоялплцкщдцаэглшнокжцкшгезогльнглщишияжржгишкдгибжшксицкщгляябцерлкгэхицкъглскокшгкзихлояигдшазяокэкяжкхс";
		List<KeyValuePair<char, int>> textFrequentSymbols = GetSymbolFrequency(text);

		int textFrequentSymbolCode1 = Array.IndexOf(encodingAlphabet, textFrequentSymbols [0].Key);
		int textFrequentSymbolCode2 = Array.IndexOf(encodingAlphabet, textFrequentSymbols [1].Key);

		char [] encodingFrequentSymbols = ['о', 'н'];
		int encodingFrequentSymbolCode1 = Array.IndexOf(encodingAlphabet, encodingFrequentSymbols [0]);
		int encodingFrequentSymbolCode2 = Array.IndexOf(encodingAlphabet, encodingFrequentSymbols [1]);

		int [] result = MakeCryptoanalysisAffineCipher(encodingAlphabet.Length, textFrequentSymbolCode1, textFrequentSymbolCode2, encodingFrequentSymbolCode1, encodingFrequentSymbolCode2);

		Console.WriteLine($"Encrypted text: {text}");
		Console.WriteLine($"Most common chars: {textFrequentSymbols [0].Key}, {textFrequentSymbols [1].Key}");
		Console.WriteLine($"Decrypted text: {DecryptText(text, result [0], result [1], encodingAlphabet)}");
		Console.WriteLine($"Key: [a, b] = [{result [0]}, {result [1]}]");
		Utilities.WaitForKey();
	}

	private static int [] MakeCryptoanalysisAffineCipher (int mod, int textFrequentSymbolCode1, int textFrequentSymbolCode2, int encodingFrequentSymbolCode1, int encodingFrequentSymbolCode2)
	{
		int [] assumptionCoefficients = GetAssumptionAlphaAndBeta(mod, textFrequentSymbolCode1, textFrequentSymbolCode2, encodingFrequentSymbolCode1, encodingFrequentSymbolCode2);

		if (assumptionCoefficients [0] == -1)
		{
			Console.WriteLine("Unable to get alpha and beta");
			Utilities.WaitForKey();
		}

		return assumptionCoefficients;
	}

	private static int [] GetAssumptionAlphaAndBeta (int mod, int textFrequentSymbolCode1, int textFrequentSymbolCode2, int encodingFrequentSymbolCode1, int encodingFrequentSymbolCode2)
	{
		for (int alpha = 1; alpha < mod; alpha++)
		{
			if (BigInteger.GreatestCommonDivisor(alpha, mod) != 1)
			{
				continue;
			}

			int beta = (encodingFrequentSymbolCode1 - (textFrequentSymbolCode1 * alpha)) % mod;
			beta = (beta < 0) ? beta + mod : beta;

			if (((textFrequentSymbolCode2 * alpha) + beta) % mod == encodingFrequentSymbolCode2)
			{
				return [alpha, beta];
			}
		}

		return [-1];
	}

	private static List<KeyValuePair<char, int>> GetSymbolFrequency (string text)
	{
		Dictionary<char, int> charFrequency = text.GroupBy(c => c).ToDictionary(g => g.Key, g => g.Count());
		IEnumerable<KeyValuePair<char, int>> sortedCharFrequency = charFrequency.OrderByDescending(kvp => kvp.Value).Take(2);
		return sortedCharFrequency.ToList();
	}

	private static string DecryptText (string text, int alpha, int beta, char [] EncodingAlphabet)
	{
		StringBuilder decryptedText = new();
		foreach (char c in text)
		{
			int j = Array.IndexOf(EncodingAlphabet, c);
			int decryptedSymbolCode = ((alpha * j) + beta) % EncodingAlphabet.Length;
			char decryptedSymbol = EncodingAlphabet [decryptedSymbolCode];
			_ = decryptedText.Append(decryptedSymbol);
		}

		return decryptedText.ToString();
	}
}

#pragma warning disable CA1812
internal sealed class MenuItemTerm_1_Lab_4_old : MenuItemCore
{
	internal override string Title => $"Transposition cipher bruteforce";

	internal override void Execute ()
	{
		// Определяем алфавит для дешифровки текста
		string alphabet = "абвгдежзийклмнопрстуфхцчшщъыьэюя";

		// Зашифрованный текст, который нужно дешифровать
		string ciphertext = "мэиэщэоэиэчцэюиацшмыдыоэиюыцчлыуиюйбэфоэлэгэвлэчущятлючудэбэфрэиавэъуиытбэйэътщлыцш";

		// Путь для сохранения результатов дешифровки
		string outputPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads", "results.txt");

		// Запускаем функцию для брутфорса дешифровки и сохранения результатов в файл
		BruteForceAffineDecrypt(ciphertext, outputPath, alphabet);

		// Выводим сообщение о завершении и о месте сохранения результатов
		Console.WriteLine($"Output saved in: {outputPath}");

		Utilities.WaitForKey();
	}

	// Функция, которая проверяет, начинается ли строка с "ь", "ъ" или "ы"
	private static bool IsInvalidStart (string text)
	{
		return text.StartsWith('ь') || text.StartsWith('ъ') || text.StartsWith('ы');
	}

	// Функция для брутфорса дешифровки
	private static void BruteForceAffineDecrypt (string ciphertext, string outputPath, string alphabet)
	{
		using StreamWriter writer = new(outputPath);
		// Перебираем возможные значения 'a' (ключ a) в аффинном шифре
		for (int a = 1; a < alphabet.Length; a++)
		{
			// Вычисляем обратное значение 'a' по модулю длины алфавита
			int modInverseA = ModInverse(a, alphabet.Length);

			// Если обратное значение 'a' равно -1, пропускаем это значение 'a'
			if (modInverseA == -1)
			{
				continue;
			}

			// Перебираем возможные значения 'b' (ключ b) в аффинном шифре
			for (int b = 0; b < alphabet.Length; b++)
			{
				StringBuilder decryptedText = new();

				// Дешифруем каждый символ в зашифрованном тексте
				foreach (char ch in ciphertext)
				{
					// Находим индекс символа в алфавите
					int x = alphabet.IndexOf(ch, StringComparison.CurrentCulture);

					// Применяем формулу дешифрования аффинного шифра
					int decryptedX = modInverseA * (x - b + alphabet.Length) % alphabet.Length;

					// Получаем дешифрованный символ
					char decryptedChar = alphabet [decryptedX];

					// Добавляем дешифрованный символ к результату
					_ = decryptedText.Append(decryptedChar);
				}

				// Создаем строку с результатом дешифрования и параметрами (a и b)
				string result = $"Parameters (a={a}, b={b}): {decryptedText}\n";

				// Если дешифрованный текст начинается с "ь", "ъ" или "ы", выводим сообщение об отсеивании
				if (IsInvalidStart(decryptedText.ToString()))
				{
					Console.WriteLine("String was filtered: " + result);
				}
				else
				{
					// Иначе записываем результат в указанный файл
					writer.WriteLine(result);
				}
			}
		}
	}

	// Функция для нахождения обратного элемента по модулю m
	private static int ModInverse (int a, int m)
	{
		for (int i = 0; i < m; i++)
		{
			if (a * i % m == 1)
			{
				return i;
			}
		}

		return -1;
	}
}

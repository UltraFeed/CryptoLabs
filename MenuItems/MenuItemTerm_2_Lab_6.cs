#pragma warning disable CA1303
#pragma warning disable IDE0044

using System.Numerics;
using System.Security.Cryptography;

namespace CryptoLabs.MenuItems;
internal sealed class MenuItemTerm_2_Lab_6 : MenuItemCore
{
	internal override string Title => $"El Gamal Signature";
	// Заданные параметры
	private static int p = 23; // простое число p
	private static int g = 5; // примитивное по модулю p число g
	private static int x = 7; // закрытый ключ
	private static int y = ModPow(g, x, p); // открытый ключ проверки подписи
	private static int generator = 123; // генератор для CRC

	internal override void Execute ()
	{
		Console.Clear();

		string message = "Hello World!";
		Tuple<int, int, int> signature = GenerateSignature(message);

		Console.WriteLine($"Message: {message}");
		Console.WriteLine($"Signature (r, s, h): {signature.Item1}, {signature.Item2}, {signature.Item3}");

		bool isValid = VerifySignature(signature.Item1, signature.Item2, signature.Item3);
		Console.WriteLine("Signature check: " + (isValid ? "valid" : "invalid"));

		Utilities.WaitForKey();
	}

	// Генерация случайного числа, взаимно простого с m
	private static int GenerateRandomCoprime (int number)
	{
		using RandomNumberGenerator rng = RandomNumberGenerator.Create();
		byte [] randomNumber = new byte [4]; // Длина в байтах для int
		int result;

		do
		{
			rng.GetBytes(randomNumber);
			result = Math.Abs(BitConverter.ToInt32(randomNumber, 0) % (number - 2)) + 2; // Диапазон [2, m)
		} while (BigInteger.GreatestCommonDivisor(result, number) != 1);

		return result;
	}

	// Функция формирования подписи
	private static Tuple<int, int, int> GenerateSignature (string message)
	{
		int h = ComputeHash(message); // вычисление хэш-значения сообщения
		int k = GenerateRandomCoprime(p - 1); // выбор случайного числа k

		int r = ModPow(g, k, p);
		int u = (h - (x * r)) % (p - 1);
		if (u < 0)
		{
			u += p - 1;
		}

		int kInv = Utilities.ModInverse(k, p - 1);
		int s = kInv * u % (p - 1);

		return Tuple.Create(r, s, h);
	}

	// Функция проверки подписи
	private static bool VerifySignature (int r, int s, int h)
	{
		if (r <= 0 || r >= p || s <= 0 || s >= p - 1)
		{
			return false;
		}

		int lhs = ModPow(y, r, p) * ModPow(r, s, p) % p;
		int rhs = ModPow(g, h, p);

		return lhs == rhs;
	}

	private static int ComputeHash (string message)
	{
		int msgLength = message.Length * 8; // Длина сообщения в битах
		int genLength = (int) Math.Ceiling(Math.Log2(generator + 1)); // Длина генератора в битах

		int messageValue = 0;
		foreach (char c in message)
		{
			messageValue = (messageValue << 8) + c; // Преобразование символов в числовое значение (ASCII)
		}

		int remainder = messageValue << (genLength - 1); // Добавление нулей для деления

		for (int i = 0; i < msgLength; i++)
		{
			if ((remainder & (1 << (msgLength + genLength - 2 - i))) != 0)
			{
				remainder ^= generator << (msgLength - genLength + i);
			}
		}

		return remainder & ((1 << (genLength - 1)) - 1); // Получение CRC
	}

	// Алгоритм быстрого возведения в степень по модулю
	private static int ModPow (int value, int exponent, int modulus)
	{
		if (modulus == 1)
		{
			return 0;
		}

		int result = 1;
		value %= modulus;
		while (exponent > 0)
		{
			if (exponent % 2 == 1)
			{
				result = result * value % modulus;
			}

			exponent >>= 1;
			value = value * value % modulus;
		}

		return result;
	}
}

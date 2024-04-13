#pragma warning disable CA1303

using System.Numerics;

namespace CryptoLabs;

internal static class Utilities
{
	internal static void WaitForKey ()
	{
		Console.Write("Press Enter To Continue");
		_ = Console.ReadLine();
		Console.Clear();
	}

	internal static int GetInt (string message, int defaultValue = 0)
	{
		if (!string.IsNullOrEmpty(message))
		{
			Console.WriteLine(message);
		}

		while (true)
		{
			if (int.TryParse(Console.ReadLine(), out int iValue))
			{
				return iValue;
			}

			if (defaultValue != 0)
			{
				return defaultValue;
			}

			Console.WriteLine("Parsing Error. Enter Other Value. Format Int32: ");
		}
	}

	// Функция нахождения обратного элемента по модулю (расширенный алгоритм Евклида)
	internal static BigInteger ModInverse (BigInteger a, BigInteger m)
	{
		BigInteger m0 = m;
		BigInteger y = 0, x = 1;

		if (m == 1)
		{
			return 0;
		}

		while (a > 1)
		{
			BigInteger q = a / m;
			BigInteger t = m;
			m = a % m;
			a = t;
			t = y;
			y = x - (q * y);
			x = t;
		}

		if (x < 0)
		{
			x += m0;
		}

		return x;
	}

	// Функция нахождения обратного элемента по модулю (расширенный алгоритм Евклида)
	internal static int ModInverse (int a, int m)
	{
		int m0 = m;
		int y = 0, x = 1;

		if (m == 1)
		{
			return 0;
		}

		while (a > 1)
		{
			int q = a / m;
			int t = m;
			m = a % m;
			a = t;
			t = y;
			y = x - (q * y);
			x = t;
		}

		if (x < 0)
		{
			x += m0;
		}

		return x;
	}
}
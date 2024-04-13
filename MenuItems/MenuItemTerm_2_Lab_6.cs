using System.Numerics;
using System.Security.Cryptography;

namespace CryptoLabs.MenuItems;
internal sealed class MenuItemTerm_2_Lab_6 : MenuItemCore
{
	internal override string Title => $"El Gamal Signature";

	internal override void Execute ()
	{
		Console.Clear();

		// Параметры для алгоритма Эль-Гамаля
		int p = 227;
		int g = 24;
		int x = 3;
		int y = (int) BigInteger.ModPow(g, x, p);
		bool [] crcKey = [true, true, false, true];

		byte message = Convert.ToByte(Utilities.GetInt("Enter message (byte):"));

		(int r, int s) = CreateSignature(p, g, x, crcKey, message);
		Console.WriteLine($"Generated signature: r = {r}, s = {s}");

		Console.WriteLine($"Signature verification check with r = {r}, s = {s}: {(VerifySignature(p, g, y, crcKey, message, r, s) ? "valid" : "not valid")}");

		Console.WriteLine($"Signature verification check with r = {p - g}, s = {g + x}: {(VerifySignature(p, g, y, crcKey, message, p - g, g + x) ? "valid" : "not valid")}");

		Utilities.WaitForKey();
	}

	private static (int r, int s) CreateSignature (int p, int g, int x, bool [] crcKey, byte message)
	{
		byte hash = ConvertBoolArrayToByte(MenuItemTerm_2_Lab_5.CalculateCRC(ConvertByteToBoolArray(message), crcKey)); // Получение значения ХЭШ-функции
		int randomK = GenerateRandomCoprime(p);
		int r = (int) BigInteger.ModPow(g, randomK, p);
		int u = (hash - (x * r)) % (p - 1);
		while (u < 0)
		{
			u += p - 1;
		}

		int s = Utilities.ModInverse(randomK, p - 1) * u % (p - 1);

		return (r, s);
	}

	private static bool VerifySignature (int p, int g, int y, bool [] crcKey, byte message, int r, int s)
	{
		byte hash = ConvertBoolArrayToByte(MenuItemTerm_2_Lab_5.CalculateCRC(ConvertByteToBoolArray(message), crcKey)); // Получение значения ХЭШ-функции

		return (BigInteger.ModPow(y, r, p) * BigInteger.ModPow(r, s, p) % p).Equals(BigInteger.ModPow(g, hash, p));
	}

	private static bool [] ConvertByteToBoolArray (byte b)
	{
		bool [] result = new bool [8];
		for (int i = 0; i < 8; i++)
		{
			result [i] = (b & (1 << i)) != 0;
		}

		Array.Reverse(result);
		return result;
	}

	private static byte ConvertBoolArrayToByte (bool [] source)
	{
		byte result = 0;
		int index = 8 - source.Length;
		foreach (bool b in source)
		{
			if (b)
			{
				result |= (byte) (1 << (7 - index));
			}

			index++;
		}

		return result;
	}

	private static int GenerateRandomCoprime (int prime)
	{
		using RandomNumberGenerator rng = RandomNumberGenerator.Create();
		byte [] bytes = new byte [4];
		int k;
		do
		{
			rng.GetBytes(bytes);
			k = BitConverter.ToInt32(bytes, 0) & int.MaxValue;
		} while (k >= prime - 1 || BigInteger.GreatestCommonDivisor(k, prime - 1) != 1);

		return k;
	}
}

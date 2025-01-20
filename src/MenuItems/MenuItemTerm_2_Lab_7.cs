using System.Numerics;

namespace CryptoLabs.MenuItems;

internal sealed class MenuItemTerm_2_Lab_7 : MenuItemCore
{
	internal override string Title => $"RSA Bruteforce";
	internal override void Execute ()
	{
		Console.Clear();

		// Дано
		BigInteger e = 131;
		BigInteger N = 21733;
		BigInteger C = 258;

		// Факторизация N
		(BigInteger p, BigInteger q) = Factorize(N);

		// Определение закрытого ключа
		BigInteger d = CalculatePrivateKey(e, p, q);

		// Расшифровка сообщения
		BigInteger M = DecryptMessage(C, d, N);

		Console.WriteLine($"p = {p}");
		Console.WriteLine($"q = {q}");
		Console.WriteLine($"d = {d}");
		Console.WriteLine($"Decrypted Message (M) = {M}");

		Utilities.WaitForKey();
	}

	// Факторизация методом Ферма
	private static (BigInteger p, BigInteger q) Factorize (BigInteger N)
	{
		BigInteger p = Sqrt(N) + 1;
		BigInteger b2 = (p * p) - N;
		BigInteger sqrt_b2 = Sqrt(b2);

		while (sqrt_b2 * sqrt_b2 != b2)
		{
			p++;
			b2 = (p * p) - N;
			sqrt_b2 = Sqrt(b2);
		}

		BigInteger q = sqrt_b2;
		return (p - q, p + q);
	}

	// Метод для вычисления квадратного корня для BigInteger
	private static BigInteger Sqrt (BigInteger n)
	{
		if (n == 0)
		{
			return 0;
		}

		if (n == 1)
		{
			return 1;
		}

		BigInteger root = n / 2;
		BigInteger lastRoot;

		do
		{
			lastRoot = root;
			root = (root + (n / root)) / 2;
		} while (root < lastRoot);

		return lastRoot;
	}

	// Функция для расчета закрытого ключа (d) по открытому ключу (e), простым множителям (p и q)
	private static BigInteger CalculatePrivateKey (BigInteger e, BigInteger p, BigInteger q)
	{
		BigInteger phi = (p - 1) * (q - 1); // Вычисление функции Эйлера
		return Utilities.ModInverse(e, phi); // Вычисление обратного элемента по модулю phi
	}

	// Функция для расшифровки сообщения C с использованием закрытого ключа d и модуля N
	private static BigInteger DecryptMessage (BigInteger C, BigInteger d, BigInteger N)
	{
		return BigInteger.ModPow(C, d, N);
	}
}

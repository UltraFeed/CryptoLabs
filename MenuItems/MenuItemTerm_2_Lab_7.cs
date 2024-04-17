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
		BigInteger a = (int) Math.Sqrt((int) N) + 1;
		BigInteger b2 = (a * a) - N;
		while (!((int) Math.Sqrt((int) b2) * (int) Math.Sqrt((int) b2) == b2))
		{
			a++;
			b2 = (a * a) - N;
		}

		BigInteger b = (int) Math.Sqrt((int) b2);
		return (a - b, a + b);
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

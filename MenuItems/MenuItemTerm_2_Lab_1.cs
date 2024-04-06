namespace CryptoLabs.MenuItems;

internal sealed class MenuItemTerm_2_Lab_1 : MenuItemCore
{

	internal override string Title => $"Linear congruential generator";
	internal override void Execute ()
	{
		Console.Clear();
		// Параметры генератора
		const int a = 9;
		const int c = 12;
		const int m = 137;
		const int x0 = 1; // Начальное значение

		int previous = x0; // Переменная для хранения предыдущего значения

		// Переменные для анализа периода
		bool periodFound = false;
		int periodLength = 1;

		// Переменные для подсчета статистики
		int zeroCount = 0;
		int oneCount = 0;
		int evenByteCount = 0;
		int oddByteCount = 0;

		// Выработка псевдослучайной последовательности и анализ
		while (!periodFound)
		{
			// Генерация следующего числа
			int current = ((a * previous) + c) % m;
			Console.WriteLine(current);

			// Проверка наличия периода
			if (current == x0)
			{
				periodFound = true;
			}
			else
			{
				periodLength++;
			}

			// Обновление статистики
			for (int i = 0; i < sizeof(int) * 8; i++)
			{
				int bit = (current >> i) & 1; // Получение i-го бита числа current
				if (bit == 0)
				{
					zeroCount++;
				}
				else
				{
					oneCount++;
				}
			}

			// Перебираем каждый байт и проверяем четность/нечетность
			byte [] bytes = BitConverter.GetBytes(current);
			foreach (byte b in bytes)
			{
				if (b % 2 == 0)
				{
					evenByteCount++;
				}
				else
				{
					oddByteCount++;
				}
			}

			// Обновление предыдущего значения
			previous = current;
		}

		// Вывод результатов анализа
		Console.WriteLine($"{x0} - initial state");
		Console.WriteLine($"Generator period length in bits: {periodLength * sizeof(int) * 8}");
		Console.WriteLine($"Number of zeros in one period in bits: {zeroCount}");
		Console.WriteLine($"Number of ones in one period in bits: {oneCount}");
		Console.WriteLine($"Number of even bytes in one period: {evenByteCount}");
		Console.WriteLine($"Number of odd bytes in one period: {oddByteCount}");

		Utilities.WaitForKey();
	}
}

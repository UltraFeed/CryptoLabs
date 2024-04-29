#pragma warning disable CA1861
#pragma warning disable CS8618

using System.Collections;
using System.Numerics;

namespace CryptoLabs.MenuItems;

internal sealed class MenuItemTerm_2_Lab_2 : MenuItemCore
{
	internal override string Title => $"Linear feedback shift register";

	internal override void Execute ()
	{
		Console.Clear();

		// Создаем объект класса LFSR
		LFSR lfsr = new();

		// Полином определяет, какие биты из регистра будут использоваться для вычисления следующего бита в последовательности выходных значений регистра
		BitArray polynomial = new(new bool [] { true, true, false, false, false, false, false, true });

		// Начальное состояние регистра
		BitArray seed = new(new bool [] { true, true, true, false, true, false, false, true });

		// Инициализируем LFSR
		lfsr.Init(seed, polynomial);

		// Определяем интересующие нас данные
		(int stepsCount, int zeroCount, int oneCount, int evenCount, int oddCount) = GetPeriodInfo(lfsr, seed);

		Console.WriteLine($"{Utilities.BitArrayToString(seed)} - initial state");
		Console.WriteLine($"Generator period length: {stepsCount}");
		Console.WriteLine($"Number of zeros in one period in bits: {zeroCount}");
		Console.WriteLine($"Number of ones in one period in bits: {oneCount}");
		Console.WriteLine($"Number of even numbers in one period: {evenCount}");
		Console.WriteLine($"Number of odd numbers in one period: {oddCount}");

		Utilities.WaitForKey();
	}

	// Метод для определения длины периода
	private static (int stepsCount, int zeroCount, int oneCount, int evenCount, int oddCount) GetPeriodInfo (LFSR lfsr, BitArray seed)
	{
		BitArray currentState = seed;
		int stepsCount = 0;
		int evenCount = 0;
		int zeroCount = 0;
		Console.WriteLine(Utilities.BitArrayToString(seed));
		do
		{
			currentState = lfsr.GenerateNextState(currentState);
			Console.WriteLine(Utilities.BitArrayToString(currentState));
			stepsCount++;

			// Подсчет количества четных и нечетных чисел при однобайтовом представлении
			BigInteger currentStateBigInt = BitArrayToBigInt(currentState);
			if (currentStateBigInt % 2 == 0)
			{
				evenCount++;
			}

			// Подсчет количества нулей и единиц при битовом представлении
			foreach (bool bit in currentState)
			{
				if (bit == false)
				{
					zeroCount++;
				}
			}
		} while (!seed.Cast<bool>().SequenceEqual(currentState.Cast<bool>()));

		return (stepsCount += 1, zeroCount, (seed.Length * stepsCount) - zeroCount, evenCount, stepsCount - evenCount);
	}

	private static BigInteger BitArrayToBigInt (BitArray bitArray)
	{
		BigInteger result = 0;
		for (int i = 0; i < bitArray.Length; i++)
		{
			if (bitArray [i])
			{
				result |= BigInteger.One << i; // Установка i-го бита в результате
			}
		}

		return result;
	}
}
internal sealed class LFSR
{
	// Полином, определяющий характеристики генератора
	internal BitArray Polynomial { get; set; }

	// Начальное состояние регистра
	internal BitArray Seed { get; set; }

	// Метод для инициализации LFSR с указанными начальным состоянием и полиномом
	internal void Init (BitArray _seed, BitArray _polynomial)
	{
		Polynomial = _polynomial;
		Seed = _seed;
	}

	// Метод для генерации следующего состояния на основе текущего состояния
	internal BitArray GenerateNextState (BitArray currentState)
	{
		BitArray nextState = new(currentState.Length);

		// Вычисляем новое значение для первого бита с использованием XOR
		BitArray xoredBits = XORBits(currentState, Polynomial);
		nextState [0] = xoredBits [0];

		// Копируем все остальные биты из предыдущего состояния
		for (int i = 1; i < currentState.Length; i++)
		{
			nextState [i] = currentState [i - 1];
		}

		return nextState;
	}

	// Метод для вычисления XOR битов
	private static BitArray XORBits (BitArray baseBits, BitArray polynomial)
	{
		BitArray result = new(polynomial.Length);
		bool? bit = null;

		for (int j = 0; j < polynomial.Length; j++)
		{
			if (polynomial [j])
			{
				if (bit == null)
				{
					bit = baseBits [j];
				}
				else
				{
					bool currentBitValue = baseBits [j];
					bit ^= currentBitValue;
				}
			}
		}

		result [0] = (bit & true) == true;
		return result;
	}
}
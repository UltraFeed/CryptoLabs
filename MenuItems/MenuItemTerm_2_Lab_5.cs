#pragma warning disable IDE0305

namespace CryptoLabs.MenuItems;
internal sealed class MenuItemTerm_2_Lab_5 : MenuItemCore
{
	internal override string Title => $"CRC hashes";
	internal override void Execute ()
	{
		bool [] mask = [true, false, true, false];
		byte srcByte = 123;
		bool [] [] mass = new bool [256] [];
		int counter = 0;

		for (int i = 0; i < 256; i++)
		{
			mass [i] = CalculateCRC(ByteToBoolArray((byte) i), mask);
		}

		for (int i = 0; i < 256; i++)
		{
			for (int j = 0; j < 256; j++)
			{
				if (mass [i].SequenceEqual(mass [j]))
				{
					Console.Write($"The collision of {i} and {j} coincided:");
					Console.Write(string.Join("", mass [i].Select(b => b ? 1 : 0)));
					Console.WriteLine();
					counter++;
				}
			}
		}

		Console.WriteLine($"\nCRC collisions counter = {counter}");
		Console.WriteLine($"\nCRC for byte {srcByte} and mask {string.Join("", mask.Select(b => b ? 1 : 0))} is {string.Join("", CalculateCRC(ByteToBoolArray(srcByte), mask).Select(b => b ? 1 : 0))}");
		Utilities.WaitForKey();
	}

	// Функция преобразования байта в массив булевых значений
	private static bool [] ByteToBoolArray (byte data)
	{
		bool [] array = new bool [8];
		for (int i = 0; i < 8; i++)
		{
			array [i] = ((data >> (7 - i)) & 1) == 1;
		}

		return array;
	}

	private static bool [] CalculateCRC (bool [] b, bool [] key)
	{
		b = b.Concat([false, false, false]).ToArray(); // Добавление трех нулей в конце

		for (int i = 0; i < 8; i++)
		{
			if (b [i])
			{
				b [i] = b [i] ^ key [0];
				b [i + 1] = b [i + 1] ^ key [1];
				b [i + 2] = b [i + 2] ^ key [2];
				b [i + 3] = b [i + 3] ^ key [3];
			}
		}

		return [b [8], b [9], b [10]];
	}
}

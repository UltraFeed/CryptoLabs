#pragma warning disable IDE0305

namespace CryptoLabs.MenuItems;
internal sealed class MenuItemTerm_2_Lab_5 : MenuItemCore
{
	internal override string Title => $"CRC hashes";
	internal override void Execute ()
	{
		Console.Clear();

		bool [] mask = [true, false, true, false];
		byte srcByte = 123;
		bool [] [] mass = new bool [256] [];

		for (int i = 0; i < 256; i++)
		{
			mass [i] = CalculateCRC(Utilities.ByteToBoolArray((byte) i), mask);
		}

		// Хранит результаты хеша и соответствующие числа
		Dictionary<string, List<int>> hashTable = [];

		// Заполняем хеш-таблицу
		for (int i = 0; i < 256; i++)
		{
			string hash = string.Join("", mass [i].Select(b => b ? 1 : 0));

			if (!hashTable.TryGetValue(hash, out List<int>? value))
			{
				value = [];
				hashTable [hash] = value;
			}

			value.Add(i);
		}

		// Выводим числа, дающие одинаковый хеш
		foreach (KeyValuePair<string, List<int>> entry in hashTable)
		{
			if (entry.Value.Count > 1)
			{
				Console.WriteLine($"Hash: {entry.Key}, Numbers: ");
				Console.WriteLine(string.Join(", ", entry.Value));
				Console.WriteLine();
			}
		}

		Console.WriteLine($"CRC for byte {srcByte} and mask {string.Join("", mask.Select(b => b ? 1 : 0))} is {string.Join("", CalculateCRC(Utilities.ByteToBoolArray(srcByte), mask).Select(b => b ? 1 : 0))}");
		Utilities.WaitForKey();
	}

	internal static bool [] CalculateCRC (bool [] b, bool [] key)
	{
		b = b.Concat(new bool [3]).ToArray(); // Добавление трех нулей в конце

		for (int i = 0; i < 8; i++)
		{
			if (b [i])
			{
				for (int j = 0; j < 4; j++)
				{
					b [i + j] ^= key [j];
				}
			}
		}

		return b.Skip(8).Take(3).ToArray();
	}
}

#pragma warning disable CA1303
#pragma warning disable CA1861
#pragma warning disable CS8604

using System.Collections;
using System.Text;

namespace CryptoLabs.MenuItems;

internal sealed class MenuItemTerm_2_Lab_3 : MenuItemCore
{
	internal override string Title => $"Gamma cipher";

	internal override void Execute ()
	{
		Console.Clear();

		// Инициализация LFSR с seed и polynomial
		BitArray seed = new(new bool [] { true, false, true, false });
		BitArray polynomial = new(new bool [] { true, false, false, true });
		LFSR lfsr = new();
		lfsr.Init(seed, polynomial);

		while (true)
		{
			Console.WriteLine($"Gamma cipher");
			Console.WriteLine($"0. Exit");
			Console.WriteLine($"1. Encrypt/Decrypt");
			Console.WriteLine($"{nameof(seed)} = {BitArrayToString(seed)}");
			Console.WriteLine($"{nameof(polynomial)} = {BitArrayToString(polynomial)}");

			int input = Utilities.GetInt($"{nameof(input)} In Range (0-1): ");

			if (input == 0)
			{
				Utilities.WaitForKey();
				break;
			}

			else if (input == 1)
			{
				using OpenFileDialog openFileDialog = new()
				{
					Filter = "Text files (*.txt)|*.txt",
					Title = "Choose a file to decrypt"
				};

				// Если пользователь выбирает файл и нажимает "ОК"
				if (openFileDialog.ShowDialog() == DialogResult.OK)
				{
					string inputFile = openFileDialog.FileName;
					string outputFile = Path.Combine(Path.GetDirectoryName(inputFile), Path.GetFileNameWithoutExtension(inputFile) + "_processed" + Path.GetExtension(inputFile));
					byte [] data = File.ReadAllBytes(inputFile);
					byte [] processedData = ProcessData(data, lfsr);
					File.WriteAllBytes(outputFile, processedData);
					Console.WriteLine("File encrypted/decrypted");
				}
			}

			else
			{
				Console.WriteLine("Unknown Input");
			}

			Utilities.WaitForKey();

		}
	}

	private static byte [] ProcessData (byte [] data, LFSR lfsr)
	{
		// Генерация гаммы с использованием LFSR
		byte [] keyStream = GenerateKeyStream(lfsr, data.Length);

		// Применение гаммы к исходным данным
		byte [] processedData = new byte [data.Length];
		for (int i = 0; i < data.Length; i++)
		{
			processedData [i] = (byte) (data [i] ^ keyStream [i]);
		}

		return processedData;
	}

	private static byte [] GenerateKeyStream (LFSR lfsr, int length)
	{
		byte [] keyStream = new byte [length];
		BitArray currentState = new(lfsr.Seed);

		for (int i = 0; i < length; i++)
		{
			// Генерация следующего состояния регистра
			currentState = lfsr.GenerateNextState(currentState);
			// Формирование байта из бита регистра сдвига
			byte keyByte = BitArrayToByte(currentState);
			keyStream [i] = keyByte;
		}

		return keyStream;
	}

	private static byte BitArrayToByte (BitArray bits)
	{
		if (bits.Length > 8)
		{
			throw new ArgumentException("BitArray length must be at most 8 bits.");
		}

		byte [] bytes = new byte [1];
		bits.CopyTo(bytes, 0);
		return bytes [0];
	}

	internal static string BitArrayToString (BitArray array)
	{
		StringBuilder sb = new();
		for (int i = 0; i < array.Length; i++)
		{
			_ = sb.Append(array [i] ? "1" : "0");
		}

		return sb.ToString();
	}
}
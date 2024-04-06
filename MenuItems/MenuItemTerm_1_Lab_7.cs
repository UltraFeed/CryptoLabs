#pragma warning disable CA1303
#pragma warning disable IDE0058
#pragma warning disable SYSLIB1045

using System.Text.RegularExpressions;
using OxyPlot;
using OxyPlot.Series;
using OxyPlot.WindowsForms;

namespace CryptoLabs.MenuItems;

internal sealed class MenuItemTerm_1_Lab_7 : MenuItemCore
{
	internal override string Title => $"Entropy calculator";

	internal override void Execute ()
	{
		Console.Clear();
		Application.EnableVisualStyles();
		Application.SetCompatibleTextRenderingDefault(false);

		string sourceString = "Надоело говорить и спорить, И любить усталые глаза... В флибустьерском дальнем море Бригантина подымает паруса... Капитан, обветренный, как скалы, Вышел в море, не дождавшись нас... На прощанье подымай бокалы Золотого терпкого вина.";
		sourceString = sourceString.ToLower(System.Globalization.CultureInfo.CurrentCulture);
		Console.WriteLine($"Source string:\n{sourceString}\n");
		string modifiedString = Regex.Replace(sourceString, "[!\"#$%&'()*+,-./:;<=>?@[\\]^_`{|}~ ]", string.Empty);
		Console.WriteLine("Modified (without spaces and punctuation marks):");
		Console.WriteLine(modifiedString);
		Console.WriteLine();
		Console.WriteLine("Calculations:");

		List<DataPoint> dataPoints = [];

		for (int i = 1; i < modifiedString.Length; i++)
		{
			double entropy = CalcEntropy(modifiedString, i) / i;
			Console.WriteLine($"k = {i}    H(k)/k = {entropy}");
			dataPoints.Add(new DataPoint(i, entropy));
		}

		// Построение графика
		PlotModel plotModel = new();
		LineSeries lineSeries = new()
		{
			ItemsSource = dataPoints
		};

		plotModel.Series.Add(lineSeries);

		// Отображение графика
		PlotView plotView = new()
		{
			Dock = DockStyle.Fill,
			Model = plotModel
		};

		Form form = new()
		{
			Text = "Entropy", // Устанавливаем заголовок формы
			Width = 800,
			Height = 600
		};

		form.Controls.Add(plotView);
		form.ShowDialog();
		Utilities.WaitForKey();
		form.Close();
	}

	private static double CalcEntropy (string s, int k)
	{
		Dictionary<string, int> dic = [];

		for (int i = 0; i < s.Length - k + 1; i++)
		{
			string substring = s.Substring(i, k);
			if (!dic.TryGetValue(substring, out int value))
			{
				dic.Add(substring, 1);
			}
			else
			{
				dic [substring] = ++value;
			}
		}

		double val = 0.0;

		foreach (KeyValuePair<string, int> kvp in dic)
		{
			double p = Convert.ToDouble(kvp.Value) / Convert.ToDouble(s.Length - k + 1);
			val += p * Math.Log2(p);
		}

		return val;
	}
}

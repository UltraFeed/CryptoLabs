#pragma warning disable CA1303

using CryptoLabs.MenuItems;

namespace CryptoLabs;

internal static class Program
{
	[STAThread]
	private static void Main ()
	{

		Menu.ClearItems();
		Menu.AddItem(new MenuItemExit());

		Menu.AddItem(new MenuItemTerm_1_Lab_1());
		Menu.AddItem(new MenuItemTerm_1_Lab_2());
		Menu.AddItem(new MenuItemTerm_1_Lab_4());
		Menu.AddItem(new MenuItemTerm_1_Lab_5());
		Menu.AddItem(new MenuItemTerm_1_Lab_6());
		Menu.AddItem(new MenuItemTerm_1_Lab_7());

		Menu.AddItem(new MenuItemTerm_2_Lab_1());
		Menu.AddItem(new MenuItemTerm_2_Lab_2());
		Menu.AddItem(new MenuItemTerm_2_Lab_3());
		Menu.AddItem(new MenuItemTerm_2_Lab_4());
		Menu.AddItem(new MenuItemTerm_2_Lab_5());
		Menu.AddItem(new MenuItemTerm_2_Lab_6());
		Menu.AddItem(new MenuItemTerm_2_Lab_7());

		while (true)
		{
			Menu.ShowMenu();
			Menu.Execute();
		}
	}
}

internal static class Menu
{
	private static readonly List<MenuItemCore> MenuItems = [];
	internal static void ClearItems ()
	{
		MenuItems.Clear();
	}

	internal static void AddItem (MenuItemCore menuItem)
	{
		MenuItems.Add(menuItem);
	}

	internal static void Execute ()
	{
		int iMenu = Utilities.GetInt("");
		if (iMenu >= 0 && iMenu < MenuItems.Count)
		{
			MenuItems.ToArray() [iMenu].Execute();
		}
		else
		{
			Console.WriteLine("Unknown Index");
			Utilities.WaitForKey();
		}
	}
	internal static void ShowMenu ()
	{
		int iMenuItem = 0;
		foreach (MenuItemCore menuItem in MenuItems)
		{
			Console.WriteLine($"{iMenuItem++}: {menuItem.Title}");
		}
	}
}

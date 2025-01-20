namespace CryptoLabs.MenuItems;

internal sealed class MenuItemExit : MenuItemCore
{
	internal override string Title => "Exit";

	internal override void Execute ()
	{
		Console.Clear();
		Utilities.WaitForKey();
		Environment.Exit(0);
	}
}

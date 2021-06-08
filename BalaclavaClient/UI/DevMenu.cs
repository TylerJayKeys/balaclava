extern alias draw;
using draw::System.Drawing;
using CitizenFX.Core;
using NativeUI;
using BalaclavaClient.Interiors;

namespace BalaclavaClient.UI
{
    class DevMenu : BaseScript
    {
        private MenuPool _menuPool;

        public DevMenu()
        {
            _menuPool = new MenuPool();
            var mainMenu = new UIMenu("Balaclava", "~b~DEV MENU", new PointF(1280, 0), true);
            mainMenu.MouseControlsEnabled = false;

            _menuPool.Add(mainMenu);
            AddGotoMenu(mainMenu);
            _menuPool.RefreshIndex();

            Tick += async () =>
            {
                _menuPool.ProcessMenus();
                if (Game.IsControlJustPressed(0, Control.ReplayStartStopRecording)) // menu on/off switch, should be F1
                {
                    mainMenu.Visible = !mainMenu.Visible;
                }
            };
        }

        // For now all this does is teleport you to casino
        // TODO: make submenu with more preset locations
        public void AddGotoMenu(UIMenu menu)
        {
            var newItem = new UIMenuItem("Goto Casino", "Teleport to the casino.");
            newItem.SetLeftBadge(UIMenuItem.BadgeStyle.Car);
            menu.AddItem(newItem);
            menu.OnItemSelect += (sender, item, index) =>
            {
                if (item == newItem)
                {
                    Game.PlayerPed.Position = Casino.mainPos; // very hacky for now :|
                }
            };
        }
    }
}

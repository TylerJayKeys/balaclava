using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;
using static CitizenFX.Core.Native.API;

namespace BalaclavaClient.Managers
{
    class CommandManager : BaseScript
    {
        public CommandManager()
        {
            EventHandlers["onClientResourceStart"] += new Action<string>(RegisterCommands);
        }

        private static void RegisterCommands(string resourceName)
        {
            if (GetCurrentResourceName() != resourceName) return;
            
            RegisterTeleportCommand();
            RegisterPosCommand();
        }

        private static void RegisterPosCommand()
        {
            RegisterCommand("pos", new Action<int, List<object>, string>((source, args, rawCommand) =>
            {
                TriggerEvent("chat:addMessage", new
                {
                    color = new[] { 255, 0, 0 },
                    args = new[] { "[Balaclava]", $"Your position is {Game.PlayerPed.Position}" }
                });

                Debug.WriteLine($"Current position: {Game.PlayerPed.Position}");

            }), false);
        }

        private static void RegisterTeleportCommand()
        {
            RegisterCommand("tp", new Action<int, List<object>, string>((source, args, rawCommand) =>
            {
                if (args.Count == 3)
                {
                    // lol @ casting
                    float x = float.Parse((string)args[0]);
                    float y = float.Parse((string)args[1]);
                    float z = float.Parse((string)args[2]);

                    Game.PlayerPed.Position = new Vector3(x, y, z);
                    Debug.WriteLine($"Teleported {Game.Player.Name} to {Game.PlayerPed.Position}");
                }
            }), false);
        }
    }
}

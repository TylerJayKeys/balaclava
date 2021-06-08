using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;
using static CitizenFX.Core.Native.API;
using BalaclavaClient.Managers;
using BalaclavaClient.Interiors;

namespace BalaclavaClient
{
    public class Main : BaseScript
    {
        //
        // MANAGERS
        //
        private SpawnManager SpawnManager = new SpawnManager();
        private CommandManager CommandManager = new CommandManager();

        //
        // INTERIORS
        //
        //private OldCasino Casino = new OldCasino();
        private Casino Casino = new Casino();

        public Main()
        {
            EventHandlers["onClientResourceStart"] += new Action<string>(OnClientResourceStart);
            EventHandlers["onClientMapStart"] += new Action(OnClientMapStart);
        }

        private void OnClientResourceStart(string resourceName)
        {
            if (GetCurrentResourceName() != resourceName) return;

            // some gamerules
            SetRadarBigmapEnabled(false, false);
            NetworkSetFriendlyFireOption(true);
            SetCanAttackFriendly(PlayerPedId(), true, true);

            LoadAllInteriors();

            Debug.WriteLine("Balaclava Framework started on client!");
        }

        private void OnClientMapStart()
        {
            Exports["spawnmanager"].setAutoSpawn(true);
            Exports["spawnmanager"].forceRespawn();
        }

        private void LoadAllInteriors()
        {
            Casino.LoadAll();
            Debug.WriteLine("All interiors loaded!");
        }
    }
}

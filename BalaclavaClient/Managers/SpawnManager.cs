using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;
using static CitizenFX.Core.Native.API;

namespace BalaclavaClient.Managers
{
    class SpawnManager : BaseScript
    {
        private bool isFirstSpawn = true;
        private Vector3 lastDeathLocation = new Vector3();
        private bool isRespawning = true;


        // TODO: make spawn location depend on team etc
        private Vector3 firstSpawnLocation = new Vector3(-1037.91F, -2737.98F, 20.17F);
        private float firstSpawnHeading = 323.5F;

        public SpawnManager()
        {
            EventHandlers["playerSpawned"] += new Action(PlayerSetup);

            Exports["spawnmanager"].setAutoSpawnCallback(new Action(Respawn));
        }

        private static void PlayerSetup()
        {
            StatSetInt((uint)GetHashKey("MP0_STAMINA"), 100, true);
            StatSetInt((uint)GetHashKey("MP0_SHOOTING_ABILITY"), 100, true);
            StatSetInt((uint)GetHashKey("MP0_STRENGTH"), 100, true);
            StatSetInt((uint)GetHashKey("MP0_STEALTH_ABILITY"), 100, true);
            StatSetInt((uint)GetHashKey("MP0_FLYING_ABILITY"), 100, true);
            StatSetInt((uint)GetHashKey("MP0_WHEELIE_ABILITY"), 100, true);
            StatSetInt((uint)GetHashKey("MP0_LUNG_CAPACITY"), 100, true);

            GiveWeaponToPed(PlayerPedId(), (uint)WeaponHash.Pistol, 12 * 7, false, false);

            SetPlayerSkin();
        }

        // The functions here will be very useful for character creation ;)
        private static void SetPlayerSkin()
        {
            Random rand = new Random(PlayerPedId().GetHashCode());
            int player = PlayerPedId();

            // sets ped default clothes
            SetPedDefaultComponentVariation(player);

            // random face + skin tone
            int father = rand.Next(0, 20);
            int mother = rand.Next(21, 45);
            SetPedHeadBlendData(player, father, mother, 0, father, mother, 0, (float)rand.NextDouble(), (float)rand.NextDouble(), 0, false);

            // random hair style + color
            int hairStyle = rand.Next(0, 74);
            int hairColor = rand.Next(0, 63);
            int beardStyle = rand.Next(0, 28);
            int beardOpacity = rand.Next(0, 255);
            int eyebrowStyle = rand.Next(0, 33);
            int eyebrowOpacity = rand.Next(0, 255);
            int chesthairStyle = rand.Next(0, 16);
            int chesthairOpacity = rand.Next(0, 255);
            if (hairStyle == 23) { hairStyle++; }

            // Hair
            SetPedComponentVariation(player, 2, hairStyle, 0, 0);
            SetPedHairColor(player, hairColor, hairColor);
            // Beard
            SetPedHeadOverlay(player, 1, beardStyle, beardOpacity);
            SetPedHeadOverlayColor(player, 1, 1, hairColor, hairColor);
            // Eyebrows
            SetPedHeadOverlay(player, 2, eyebrowStyle, eyebrowOpacity);
            SetPedHeadOverlayColor(player, 2, 1, hairColor, hairColor);
            // Chest hair
            SetPedHeadOverlay(player, 10, chesthairStyle, chesthairOpacity);
            SetPedHeadOverlayColor(player, 10, 1, hairColor, hairColor);

            // Ageing
            int ageingStyle = rand.Next(0, 14);
            int ageingOpacity = rand.Next(0, 255);
            SetPedHeadOverlay(player, 3, ageingStyle, ageingOpacity);
        }

        private void Respawn()
        {
            if (isFirstSpawn)
            {
                Exports["spawnmanager"].spawnPlayer(new
                {
                    x = firstSpawnLocation.X,
                    y = firstSpawnLocation.Y,
                    z = firstSpawnLocation.Z,
                    heading = firstSpawnHeading,
                    model = "mp_m_freemode_01",
                    skipFade = false
                });
                isFirstSpawn = false;
                // TriggerEvent("firstSpawn");
            }
            else if (!isFirstSpawn && Game.Player.IsDead)
            {
                Vector3 deathLocation = Game.PlayerPed.Position;
                Vector3 randomCoord = new Vector3();
                Vector3 respawnLocation = new Vector3();
                GetNthClosestVehicleNode(deathLocation.X, deathLocation.Y, deathLocation.Z, 20, ref randomCoord, 0, 0, 0);
                GetPointOnRoadSide(randomCoord.X, randomCoord.Y, randomCoord.Z, 0, ref respawnLocation);
                // could also use GetSafeCoordForPed

                Exports["spawnmanager"].spawnPlayer(new
                {
                    x = respawnLocation.X,
                    y = respawnLocation.Y,
                    z = respawnLocation.Z,
                    heading = 260.0,
                    skipFade = false
                });
            }
            else if (!Game.Player.IsDead)
            {
                Game.PlayerPed.ClearBloodDamage();
                Game.PlayerPed.ResetVisibleDamage();
            }
        }
    }
}

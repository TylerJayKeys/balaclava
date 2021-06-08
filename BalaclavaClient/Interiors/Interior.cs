using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;
using static CitizenFX.Core.Native.API;

namespace BalaclavaClient.Interiors
{
    class Interior : BaseScript
    {
        // doorway has Vector3 entryPoint/entryPos, Vector3 exitPoint/exitPos
        // pos = teleporter marker location, point = ped teleport location

        public Dictionary<string, InteriorTeleporter> interiors;
        public string interiorName;

        public int markerColor_R, markerColor_G, markerColor_B, markerColor_A;

        public Interior(string interiorName, int r, int g, int b, int a)
        {
            this.interiorName = interiorName;

            AddTextEntry(interiorName + "EntryTextHelper", "Press ~INPUT_TALK~ to enter " + interiorName);
            AddTextEntry(interiorName + "ExitTextHelper", "Press ~INPUT_TALK~ to exit " + interiorName);

            markerColor_R = r;
            markerColor_G = g;
            markerColor_B = b;
            markerColor_A = a;
        }

        public void LoadAll()
        {
            foreach (string ipl in interiors.Keys)
            {
                if (!IsIplActive(ipl))
                {
                    RequestIpl(ipl);
                }
            }
        }

        public void UnloadAll()
        {
            foreach (string ipl in interiors.Keys)
            {
                if (IsIplActive(ipl))
                {
                    RemoveIpl(ipl);
                }
            }
        }

        public async Task DrawEntryPoints()
        {
            Vector3 pedPos = Game.PlayerPed.Position;

            foreach(InteriorTeleporter teleporter in interiors.Values)
            {
                Vector3 teleEntry = teleporter.entryFromPos;
                Vector3 teleExit = teleporter.exitFromPos;

                // Entry teleporter marker
                DrawMarker(1, teleEntry.X, teleEntry.Y, teleEntry.Z, 0, 0, 0, 0, 0, 0, 2, 2, 2, markerColor_R, markerColor_G, markerColor_B, markerColor_A, false, true, 2, true, null, null, false);

                // Exit teleporter marker
                DrawMarker(1, teleExit.X, teleExit.Y, teleExit.Z, 0, 0, 0, 0, 0, 0, 2, 2, 2, markerColor_R, markerColor_G, markerColor_B, markerColor_A, false, true, 2, true, null, null, false);

                // Entry teleporter logic
                // TODO: make this and the exit tele logic less copy-pasty if possible
                if (GetDistanceBetweenCoords(pedPos.X, pedPos.Y, pedPos.Z, teleEntry.X, teleEntry.Y, teleEntry.Z, true) <= 1.5)
                {
                    DisplayHelpTextThisFrame(interiorName + "EntryTextHelper", false);

                    if (IsControlJustPressed(0, 46)) // INPUT_TALK
                    {
                        await DoEntryPointTransition(teleporter.entryToPos);
                    }
                }

                // Exit teleporter logic
                if (GetDistanceBetweenCoords(pedPos.X, pedPos.Y, pedPos.Z, teleExit.X, teleExit.Y, teleExit.Z, true) <= 1.5)
                {
                    DisplayHelpTextThisFrame(interiorName + "ExitTextHelper", false);

                    if (IsControlJustPressed(0, 46)) // INPUT_TALK
                    {
                        await DoEntryPointTransition(teleporter.exitToPos);
                    }
                }
            }
        }

        private async Task DoEntryPointTransition(Vector3 pos)
        {
            DoScreenFadeOut(500);
            await Delay(500);

            Game.PlayerPed.Position = pos;

            await Delay(1000);
            DoScreenFadeIn(1000);
        }
    }
}

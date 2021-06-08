using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;
using static CitizenFX.Core.Native.API;

namespace BalaclavaClient.Interiors
{
    class Casino : Interior
    {
        public static Vector3 mainPos = new Vector3(1091.085f, 210.4563f, -50);

        public Casino() : base("Casino", 255, 100, 0, 100)
        {
            // TODO: if possible, make this constructor more user-friendly
            interiors = new Dictionary<string, InteriorTeleporter>
            {
                {
                    "vw_casino_main",
                    new InteriorTeleporter(
                        new Vector3(935.5551f, 46.46945f, 80.1f), mainPos,
                        new Vector3(1089.615f, 206.7595f, -50), new Vector3(934.1057f, 40.99028f, 81.09578f))
                }
            };

            Tick += DrawEntryPoints;
        }
    }
}

using CitizenFX.Core;

namespace BalaclavaClient.Interiors
{
    class InteriorTeleporter : BaseScript
    {
        public Vector3 entryFromPos, entryToPos, exitFromPos, exitToPos;

        /// <summary>
        /// Defines a series of positions for teleporter markers and their destinations in relation to an interior's entry/exits
        /// </summary>
        /// <param name="entryFrom">Marker location for the entry teleporter</param>
        /// <param name="entryTo">Position you will land at once activating the entry teleporter</param>
        /// <param name="exitFrom">Marker location for the exit teleporter</param>
        /// <param name="exitTo">Position you will land at once activating the exit teleporter</param>
        public InteriorTeleporter(Vector3 entryFrom, Vector3 entryTo, Vector3 exitFrom, Vector3 exitTo)
        {
            entryFromPos = entryFrom;
            entryToPos = entryTo;
            exitFromPos = exitFrom;
            exitToPos = exitTo;
        }

        public Vector3 GetOppositeTeleporterPos(Vector3 pos)
        {
            if (pos == entryFromPos)
            {
                return entryToPos;
            }
            else if (pos == entryToPos)
            {
                return entryFromPos;
            }
            else if (pos == exitFromPos)
            {
                return exitToPos;
            }
            else if (pos == exitToPos)
            {
                return exitFromPos;
            }
            else
            {
                return Vector3.Zero;
            }
        }
    }
}

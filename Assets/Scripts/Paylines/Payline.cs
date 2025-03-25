using System;

namespace SlotMachineEngine
{
    [Serializable]
    public class Payline
    {
        public readonly int ID;
        public readonly int PAYOUTSYMBOLID;
        public readonly int[] SYMBOLPOSITIONS;
        public readonly int SYMBOLSCOUNT;
        public readonly double WON;

        private Payline() { }

        public Payline(int id, int payoutSymbolId, int[] symbolPositions, int symbolShowCount, double won)
        {
            ID = id;
            PAYOUTSYMBOLID = payoutSymbolId;
            SYMBOLPOSITIONS = new int[symbolPositions.Length];
            SYMBOLPOSITIONS = symbolPositions;
            SYMBOLSCOUNT = symbolShowCount;
            WON = won;
        }
    }

    [Serializable]
    public class SpecialPayline
    {
        public readonly int[,] POSITIONS;
        private SpecialPayline() { }
        public SpecialPayline(string[] positions)
        {
            POSITIONS = new int[positions.Length, 2];

            for (int i = 0; i < positions.Length; i++)
            {
                string[] temp = positions[i].Split(',');
                POSITIONS[i, 0] = int.Parse(temp[0]);
                POSITIONS[i, 1] = int.Parse(temp[1]);
            }
        }
    }
}
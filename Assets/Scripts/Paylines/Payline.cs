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
        
    }
}
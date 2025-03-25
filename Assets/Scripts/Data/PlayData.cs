using SlotMachineEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Data
{
    public class Payline
    {
        public int paylineId;
        public int symbol;
        public int[] positions;
        public int symbolCount;
        public double won;
    }
    public class Scatter
    {
        public int count;
        public string[] positions;
    }
    public class Bonus
    {
        public int count;
        public string[] positions;
    }
    public class FreeGame
    {
        public int currentFreeSpin;
        public int totalFreeSpin;
    }
    public class PlayData
    {
        public double balance;
        public List<List<int>> matrix = new List<List<int>>();
        public Payline[] payline;
        public Scatter scatter;
        public FreeGame freeGame;
        public Bonus bonus;
        public double FreeSpinTotalCreditsWon;
    }
}

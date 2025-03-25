using Generics;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Data;
using FSM.GameState;

public class RNG : MonoSingleton<RNG>
{
    private double currentBalance;
    public double CurrentBalance { get => currentBalance; set => currentBalance = value; }

    public Action CompleteResponseFetchedEvent;

    public Payline payline = new Payline();
    public PlayData playData = new PlayData();
    public Scatter scatter = new Scatter();
    public Bonus bonus = new Bonus();
    public FreeGame freeGame = new FreeGame();
    List<List<int>> matrix = new();
    List<System.Action> dataSets = new List<System.Action>();

    public int GetPaylineNumber()
    {
        return 20; // payline number
    }
    public int GetPaylineCount()
    {
        return 1; // payline Count
    }

    public double GetCurrentSpinTotalWon()
    {
        return 200; // Total Win amount for payline
    }

    public double GetTotalCreditWon()
    {
        return 200;
    }

    public void SendSpinRequest()
    {
        SendNormalSpinRequest();
        CreateNewDataSet();
    }

    private void CreateNewDataSet()
    {
        // TODO: Update new data and create it
        EventManager.InvokeSpinResponse();
    }

    private void SendNormalSpinRequest()
    {
        EventManager.InvokeSpinResponse();
    }


    // -----------------------------------------------------
    /* 
    What we need as a data set, a payline data set, scatter data set, bonus data set and the recreation of the data set after spin has been complete,
    just when we click on the spin again, data set should be recreated. */

    public void SetCurrentBalance()
    {
        playData.balance = 10000;
        currentBalance = playData.balance;
    }
    public void SpinTheReel()
    {
        GenerateNewData();
    }

    public void SpinTheFreeReel(int remainingfreereel)
    {
        ClearData();
        RunFreeStates(remainingfreereel);
    }

    private void GenerateNewData()
    {
        ClearData();
        UnityEngine.Random.InitState((int)System.DateTime.Now.Ticks);
        dataSets = new List<System.Action>()
        {
            DataSet1,
            DataSet2,
            DataSet3,
            DataSet4,
            DataSet5,
            DataSet6
            // Add more data sets here...
        };

        // Choose a random data set.
        int randomIndex = UnityEngine.Random.Range(0, dataSets.Count);
        dataSets[randomIndex].Invoke();

    }

    private void ClearData()
    {
        payline.paylineId = 0;
        payline.symbol = 0;
        payline.positions = null;
        payline.symbolCount = 0;
        payline.won = 0;

        playData.matrix.Clear();
        
        scatter.positions = null;
        scatter.count = 0;
        bonus.count = 0;
        freeGame.totalFreeSpin = 0;
        freeGame.currentFreeSpin = 0;
    }

    private void DataSet1()
    {
        payline.paylineId = 1;
        payline.symbol = 1;
        payline.positions = new[] { 1, 1, 1, 1, 1 };
        payline.symbolCount = 5;
        payline.won = 200;


        playData.matrix.Add(new List<int> { 2, 1, 5 });
        playData.matrix.Add(new List<int> { 3, 1, 8 });
        playData.matrix.Add(new List<int> { 0, 1, 5 });
        playData.matrix.Add(new List<int> { 2, 1, 7 });
        playData.matrix.Add(new List<int> { 4, 1, 7 });

        scatter.count = 0;
        bonus.count = 0;
    }

    private void DataSet2()
    {
        payline.paylineId = 14;
        payline.symbol = 3;
        payline.positions = new[] { 0, 1, 0, 1, 0 };
        payline.symbolCount = 4;
        payline.won = 4;

        playData.matrix.Add(new List<int> { 3, 1, 5 });
        playData.matrix.Add(new List<int> { 2, 3, 8 });
        playData.matrix.Add(new List<int> { 3, 1, 9 });
        playData.matrix.Add(new List<int> { 2, 2, 7 });
        playData.matrix.Add(new List<int> { 1, 1, 6 });

        scatter.count = 0;
        bonus.count = 0;
    }

    private void DataSet3()
    {
        payline.paylineId = 19;
        payline.symbol = 8;
        payline.positions = new[] { 2, 2, 0, 2, 2 };
        payline.symbolCount = 5;
        payline.won = 1000;

        playData.matrix.Add(new List<int> { 1, 0, 8 });
        playData.matrix.Add(new List<int> { 9, 3, 8 });
        playData.matrix.Add(new List<int> { 8, 1, 9 });
        playData.matrix.Add(new List<int> { 2, 2, 8 });
        playData.matrix.Add(new List<int> { 1, 1, 8 });

        scatter.count = 0;
        bonus.count = 0;
    }
    private void DataSet4()
    {
        playData.matrix.Add(new List<int> { 1, 0, 5 });
        playData.matrix.Add(new List<int> { 9, 3, 11 });
        playData.matrix.Add(new List<int> { 7, 0, 9 });
        playData.matrix.Add(new List<int> { 0, 2, 10 });
        playData.matrix.Add(new List<int> { 1, 1, 11 });
    }

    private void DataSet5()
    {
        playData.matrix.Add(new List<int> { 10, 2, 8 });
        playData.matrix.Add(new List<int> { 9, 3, 1 });
        playData.matrix.Add(new List<int> { 7, 8, 9 });
        playData.matrix.Add(new List<int> { 0, 2, 10 });
        playData.matrix.Add(new List<int> { 9, 1, 0 });
    }

    private void DataSet6()
    {
        playData.matrix.Add(new List<int> { 11, 2, 8 });
        playData.matrix.Add(new List<int> { 9, 3, 1 });
        playData.matrix.Add(new List<int> { 11, 8, 9 });
        playData.matrix.Add(new List<int> { 0, 2, 11 });
        playData.matrix.Add(new List<int> { 9, 1, 0 });

        scatter.count = 3;
        scatter.positions = new[] { "0,0", "2,0", "3,2" };

        freeGame.totalFreeSpin = 3;
        freeGame.currentFreeSpin = 3;
    }

    private void FreeDataSet1()
    {
        playData.matrix.Add(new List<int> { 10, 2, 8 });
        playData.matrix.Add(new List<int> { 9, 3, 1 });
        playData.matrix.Add(new List<int> { 7, 8, 9 });
        playData.matrix.Add(new List<int> { 0, 2, 10 });
        playData.matrix.Add(new List<int> { 9, 1, 0 });

        freeGame.totalFreeSpin = 3;
        freeGame.currentFreeSpin = 1;
    }

    private void FreeDataSet2()
    {
        playData.matrix.Add(new List<int> { 1, 2, 8 });
        playData.matrix.Add(new List<int> { 9, 3, 10 });
        playData.matrix.Add(new List<int> { 7, 0, 9 });
        playData.matrix.Add(new List<int> { 0, 2, 2 });
        playData.matrix.Add(new List<int> { 9, 1, 2 });

        freeGame.totalFreeSpin = 3;
        freeGame.currentFreeSpin = 2;
    }

    private void FreeDataSet3()
    {
        payline.paylineId = 19;
        payline.symbol = 6;
        payline.positions = new[] { 2, 2, 0, 2, 2 };
        payline.symbolCount = 4;
        payline.won = 900;

        playData.matrix.Add(new List<int> { 1, 0, 6 });
        playData.matrix.Add(new List<int> { 9, 3, 6 });
        playData.matrix.Add(new List<int> { 6, 1, 9 });
        playData.matrix.Add(new List<int> { 2, 2, 6 });
        playData.matrix.Add(new List<int> { 1, 1, 2 });

        freeGame.totalFreeSpin = 3;
        freeGame.currentFreeSpin = 3;
    }

    public int RemainingFreeSpins()
    {
        return 3; // Test Data
    }

    private void RunFreeStates(int remainingfreereel)
    {
        if (remainingfreereel == 3) FreeDataSet1();
        else if(remainingfreereel == 2) FreeDataSet2();
        else if(remainingfreereel == 1) FreeDataSet3();
    }

    public void SpinCompleted()
    {
        EventManager.InvokeUpdateBalance();
        CompleteResponseFetchedEvent?.Invoke();
    }
}

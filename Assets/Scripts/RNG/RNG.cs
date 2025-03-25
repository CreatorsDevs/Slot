using Generics;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Data;

public class RNG : MonoSingleton<RNG>
{
    private double currentBalance;
    public double CurrentBalance { get => currentBalance; }

    public Action CompleteResponseFetchedEvent;

    public Payline payline = new Payline();
    public PlayData playData = new PlayData();
    public Scatter scatter = new Scatter();
    public Bonus bonus = new Bonus();
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
            DataSet5
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

        scatter.count = 0;
        bonus.count = 0;
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
        playData.matrix.Add(new List<int> { 3, 0, 5 });
        playData.matrix.Add(new List<int> { 9, 3, 8 });
        playData.matrix.Add(new List<int> { 7, 1, 9 });
        playData.matrix.Add(new List<int> { 2, 2, 10 });
        playData.matrix.Add(new List<int> { 1, 1, 6 });
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

    public void SpinCompleted()
    {
        EventManager.InvokeUpdateBalance();
        CompleteResponseFetchedEvent?.Invoke();
    }
}

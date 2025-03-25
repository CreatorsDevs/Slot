using Generics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RNG : MonoSingleton<RNG>
{
    private double currentBalance = 100000d;
    public double CurrentBalance { get => currentBalance; }

    public List<List<int>> GetMatrix()
    {
        List<List<int>> matrix = new List<List<int>>();

        matrix.Add(new List<int> { 1, 2, 5 });
        matrix.Add(new List<int> { 3, 2, 8 });
        matrix.Add(new List<int> { 0, 8, 5 });
        matrix.Add(new List<int> { 2, 11, 7 });
        matrix.Add(new List<int> { 4, 11, 7 });

        return matrix;
    }

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
}

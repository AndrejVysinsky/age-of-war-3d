using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GoldController: MonoBehaviour
{
    [SerializeField] TMPro.TMP_Text _goldText;
    [SerializeField] int _balance = 0;
    private const int ZERO_PREFIX_COUNT = 5;

    public FactionEnum Faction { get; private set; }

    public void Start()
    {
        this.UpdateBalance();
    }

    public void Update()
    {
        
    }

    public void Initialize(FactionEnum faction)
    {
        Faction = faction;
    }

    public void AddBalance(int balance)
    {
        if (balance < 0)
        {
            throw new InvalidOperationException("For subtracting use different method");
        }
        this._balance += balance;
        this.UpdateBalance();
    }

    public bool RemoveBalance(int balance)
    {
        if ((this._balance - balance) < 0)
        {
            Debug.LogWarning("Try to subtract more than balance");
            return false;
        }

        this._balance -= balance;
        this.UpdateBalance();
        return true;
    }

    public int GetBalance()
    {
        return this._balance;
    }

    private void UpdateBalance()
    {
        int digitCount = (int)Math.Floor(Math.Log10(_balance) + 1);
        string balanceString = "";
        
        for (int i = 0; i < (ZERO_PREFIX_COUNT - digitCount); i++)
        {
            balanceString += "0";
        }
        balanceString += _balance.ToString();
        if (_goldText != null)
        {
            this._goldText.text = balanceString;
        }
    }
}

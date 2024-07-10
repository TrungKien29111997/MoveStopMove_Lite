using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class CounterTime
{
    //action se duoc goi moi vong lap
    UnityAction doneAction;
    //action duoc goi khi den vong lap cuoi cung (counter == 0)
    UnityAction completeAction;
    //number > 0 -> so lan loop tiep theo
    //number < 0 -> loop vo han 
    //number == 0 -> stop
    private int number;
    private float timeAlive;
    private float time;
    //kiem tra dang chay hay k
    public bool IsRunning => time > 0;
    ///tong thoi gian
    public float TimeAlive { get => timeAlive; set => timeAlive = value; }
    /// thoi gian con lai
    public float Time => time;
    /// thoi gian hoan thanh 0 -> 1
    public float RateTime => 1 - Time / TimeAlive;
    /// so luot chay con lai
    public int Number => number;

    public CounterTime Start(UnityAction doneAction, float time)
    {
        this.doneAction = doneAction;
        this.completeAction = null;
        this.TimeAlive = time;
        this.time = time;
        this.number = 0;

        if (time <= 0) Done();
        return this;
    }

    public void SetTime(float time)
    {
        this.time = time;
    }

    public void Execute(float deltaTime)
    {
        if (time > 0)
        {
            time -= deltaTime;
            if (time <= 0)
            {
                time = 0;
                Done();
            }
        }
    }

    private void Done()
    {
        doneAction?.Invoke();

        if (number < 0)
        {
            //loop vo han
            time = TimeAlive;
        }
        else if (number > 0)
        {
            //tru 1 lan counter
            number--;
            time = TimeAlive;
        }
        else
        {
            //ket thuc
            completeAction?.Invoke();
        }
    }

    public void Stop()
    {
        time = -1;
        number = 0;
        doneAction = null;
        completeAction = null;
    }

    public CounterTime SetLoop(bool loop)
    {
        //loop vo han
        this.number = loop ? -1 : 0;
        return this;
    }

    public CounterTime SetLoop(int number)
    {
        //loop so lan nhat dinh
        this.number = number - 1;
        return this;
    }

    public CounterTime SetComplete(UnityAction completeAction)
    {
        //ket thuc loop thi thuc hien
        this.completeAction = completeAction;
        return this;
    }
}

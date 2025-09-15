using System;

namespace Balance;

public class Trigger
{

    private Func<bool> _condition;
    private System.Action _consequence;

    public Trigger(Func<bool> condition, System.Action consequence)
    {
        _condition = condition;
        _consequence = consequence;

    }


    public void Run()
    {
        if (_condition())
        {
            _consequence();
        }
    }

}

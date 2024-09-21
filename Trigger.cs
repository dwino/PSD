using System;

namespace Balance;

public class Trigger
{

    private Func<bool> _condition;
    private System.Action _consequence;
    private bool _removeOnTriggered;

    public Trigger(Func<bool> condition, System.Action consequence, bool removeOnTriggered = false)
    {
        _condition = condition;
        _consequence = consequence;
        _removeOnTriggered = removeOnTriggered;
        Remove = false;
    }

    public bool Remove { get; set; }

    public void Run()
    {
        if (_condition())
        {
            _consequence();
            Remove = _removeOnTriggered;
        }
    }

}

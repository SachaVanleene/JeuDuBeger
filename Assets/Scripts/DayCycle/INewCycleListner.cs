using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public interface INewCycleListner
{
    void DayStart(); // is called when the day begins
    void NightStart(); // is called when the night begins

    void WaitingAt(int goal, int angle); // is called when the cycle reached or go beyond its goal and is waiting
}

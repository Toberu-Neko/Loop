using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITimeStopable
{
    void DoTimeStopWithTime(float stopTime);
    void DoTimeStop();
    void EndTimeStop();
}

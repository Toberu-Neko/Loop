using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITimeSlowable
{
    void DoTimeSlow();
    void EndTimeSlow();
    GameObject GetGameObject();

}

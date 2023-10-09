using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISlowable
{
    void SetActionSpeedMultiplier(float multiplier, float delayTime);
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISlowable
{
    void SetDebuffMultiplier(float multiplier, float delayTime = 0f);
    void SetDebuffMultiplierOne();
}

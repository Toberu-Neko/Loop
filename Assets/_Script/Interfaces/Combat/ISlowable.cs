using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISlowable
{
    void MultiplyMovementMultiplier(float multiplier);

    void DevideMovementMultiplier(float multiplier, float delayTime = 0f);
}

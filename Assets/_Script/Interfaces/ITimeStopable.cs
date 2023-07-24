using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITimeStopable
{
    public void HandleTimeStop() { }

    public void HandleTimeStart() { }
}

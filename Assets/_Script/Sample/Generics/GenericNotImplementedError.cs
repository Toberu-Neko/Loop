using UnityEngine;

public static class GenericNotImplementedError<T>
{
    public static T TryGet(T val, string name)
    {
        if(val != null)
        {
            return val;
        }

        Debug.LogError(typeof(T) + "not implemented on " + name);
        return default;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DDOL_Canvas : MonoBehaviour
{
    public static DDOL_Canvas Instance { get; private set; }
    [SerializeField] private CanvasGroup group;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        DontDestroyOnLoad(gameObject);
    }

    public void SetGroupAlpha(float alpha)
    {
        group.alpha = alpha;
    }

    public void LoadingObjActive(bool active)
    {
        group.gameObject.SetActive(active);
    }
}

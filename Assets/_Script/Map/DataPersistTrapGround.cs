using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataPersistTrapGround : DataPersistMapObjBase
{
    [SerializeField] private GameObject trapGroundObj;
    [SerializeField] private Sound activeSFX;

    protected override void Start()
    {
        base.Start();

        if (isActivated)
        {
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isActivated = true;
            
            gameObject.SetActive(false);
            AudioManager.instance.PlaySoundFX(activeSFX, transform);
            CamManager.Instance.CameraShake(2f);
            trapGroundObj.SetActive(false);
            Invoke(nameof(SaveData), 2f);
        }
    }

    private void SaveData()
    {
        CancelInvoke(nameof(SaveData));
        DataPersistenceManager.Instance.SaveGame(false);
    }
}

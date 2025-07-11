using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiNotifier : MonoBehaviour
{
    [SerializeField] private Vector3 _offset;
    [SerializeField] private int _batchCreationSize = 5;
    [SerializeField] private GameObject _damageNotifPrefab;

    private void Awake()
    {
        UiNotificationHelper.SetNotifier(this);
    }

    private void CreateDmgNotes()
    {
        int created = 0;
        while (created < _batchCreationSize && _batchCreationSize >= 0)
        {

            GameObject newNotification = Instantiate(_damageNotifPrefab,this.transform);
            newNotification.GetComponent<BattleNotification>().SetNotifier(this);
            created++;
        }
    }

    public void PlayDamageNotification(int value, Transform transformPosition)
    {
        if (transformPosition != null)
        {
            if (transform.childCount == 0)
                CreateDmgNotes();

            if (transform.childCount == 0)
            {
                Debug.LogError("Failed to create Dmg Notifications for some reason.");
                return;
            }

            GameObject selectedNotif = transform.GetChild(0).gameObject;


            //show the notification
            selectedNotif.GetComponent<BattleNotification>().ShowNotification($"{value}", transformPosition, _offset);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UiNotificationHelper
{
    private static UiNotifier _notifier;






    public static void SetNotifier(UiNotifier notifier) {  _notifier = notifier; }


    public static void ShowDamageNotification(int value, Transform transformPosition)
    {
        if (_notifier != null)
            _notifier.PlayDamageNotification(value, transformPosition);
        else Debug.LogWarning("Ui notifier reference Missing within the Ui Notification Helper.");
    }
}

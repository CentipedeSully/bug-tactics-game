using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class VisualizeDevCommand : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _cmdText;
    [SerializeField] private List<GameObject> _indicators = new();


    //monobehaviours
    private void Start()
    {
        HideAllindicators();
    }
    private void OnEnable()
    {
        DevCommandTracker.OnCommandStateEntered += UpdateCmdText;
    }

    private void OnDisable()
    {
        DevCommandTracker.OnCommandStateEntered -= UpdateCmdText;
    }




    //internals
    private void UpdateCmdText(DevCommandState currentState)
    {
        switch (currentState)
        {
            case DevCommandState.unset:
                HideAllindicators();
                _cmdText.text = "";
                break;

            case DevCommandState.SpawnObject:
                _cmdText.text = $"Spawn {DevCommandTracker.GetSpecifiedUnitPrefab()} ({DevCommandTracker.GetGamePieceType()})";
                ShowAllIndicators();
                break;

            case DevCommandState.DespawnObject:
                _cmdText.text = $"Despawn {DevCommandTracker.GetGamePieceType()}";
                ShowAllIndicators();
                break;

            case DevCommandState.DamageUnit:
                _cmdText.text = $"Damage Unit by {DevCommandTracker.GetSpecifiedValue()}";
                break;
        }
    }

    private void ShowAllIndicators()
    {
        foreach (GameObject indicator in _indicators)
            indicator.SetActive(true);
    }

    private void HideAllindicators()
    {
        foreach(GameObject indicator in _indicators)
            indicator.SetActive(false);
    }
}

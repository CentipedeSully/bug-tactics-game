using SullysToolkit.TableTop;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitGroup : MonoBehaviour
{
    [SerializeField] private string _groupName = "unnamed group";
    [SerializeField] private List<UnitAttributes> _unitsInGroup = new();
    private GamePiece _gamePiece;



    public delegate void UnitGroupEvent(UnitGroup unit);
    public event UnitGroupEvent OnUnitGroupChanged;



    private void Awake()
    {
        _gamePiece = GetComponent<GamePiece>();
    }





    public void AddToGroup(UnitAttributes unit)
    {
        if (unit == null)
        {
            Debug.LogWarning($"Attempted to add null unit to group '{_groupName}'. Ignoring AddUnit request");
            return;
        }

        if (_unitsInGroup.Contains(unit))
        {
            Debug.LogWarning($"Unit {unit.UnitName()} already exists in group '{_groupName}'. Ignoring AddUnit request");
            return;
        }


        _unitsInGroup.Add(unit);
        OnUnitGroupChanged?.Invoke(this);
        
    }
    public void RemoveFromGroup(UnitAttributes unit)
    {
        if (unit == null)
        {
            Debug.LogWarning($"Attempted to remove null unit to group '{_groupName}'. Ignoring RemoveUnit request");
            return;
        }

        if (!_unitsInGroup.Contains(unit))
        {
            Debug.LogWarning($"Unit {unit.UnitName()} doesn't exists in group '{_groupName}'. Ignoring RemoveUnit request");
            return;
        }


        _unitsInGroup.Remove(unit);
        OnUnitGroupChanged?.Invoke(this);
    }
    public bool IsUnitInGroup(UnitAttributes unit) {  return _unitsInGroup.Contains(unit);}




}

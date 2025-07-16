using SullysToolkit.TableTop;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class UnitAttributes : MonoBehaviour
{
    //Delcarations
    [SerializeField] private string _name;
    [SerializeField] private UnitPrefabName _prefabName;
    [SerializeField] private int _maxHp;
    [SerializeField] private int _currentHp;

    [SerializeField] private int _evasion;
    [SerializeField] private int _armor;
    [SerializeField] private int _naturalToughness;
    [SerializeField] private int _lethalThreshold;

    [SerializeField] private int _speed;
    [SerializeField] private int _armorPenetration;
    [SerializeField] private int _strength;
    [SerializeField] private int _lethality;

    //[Tooltip("The excess value an attacker must score over this unit's Defence for the atk to be considered lethal")]
    //[SerializeField] private int _lethalDefence;
    public delegate void GeneralAttributeEvent();
    public delegate void ValuedAttributeEvent(int value);
    public event GeneralAttributeEvent OnHealthZero;
    public event ValuedAttributeEvent OnDamageTaken;


    //Monobehaviours

    private void OnEnable()
    {
        ResetAttributes();
    }

    //Internals
    private void ResetAttributes(GamePiece specifiedPiece) { ResetAttributes(); }
    private void ResetAttributes()
    {
        _currentHp = _maxHp;
    }





    //Externals
    public string UnitName() { return _name; }
    public UnitPrefabName UnitPrefabName() { return _prefabName; }
    public int MaxHp() {  return _maxHp; }
    public int Hp() { return _currentHp; }
    public void TakeDamage(int value)
    {
        
        _currentHp -= value;
        //Debug.Log($"{gameObject.name} took {value} damage");
        OnDamageTaken?.Invoke( value );
        UiNotificationHelper.ShowDamageNotification(value, transform);

        if (_currentHp <= 0)
        {
            //Debug.Log($"{gameObject.name}'s health reached zero");
            OnHealthZero?.Invoke();
        }
    }
    public void TakeLethalDamage()
    {
        TakeDamage(_maxHp);
    }

    public int GetEvasionDefence(int attackerSpeed, bool ignoreEvasion)
    {
        if (ignoreEvasion)
            return -attackerSpeed;
        else return _evasion - attackerSpeed;
    }
    public int GetArmorDefence(int attackerArmorPenetration, bool ignoreArmor)
    {
        if (ignoreArmor)
            return -attackerArmorPenetration;
        else return _armor - attackerArmorPenetration;
    }
    public int GetNaturalDefence(int attackerStrength, bool ignoreNatToughness)
    {
        if (ignoreNatToughness)
            return -attackerStrength;
        else return _naturalToughness - attackerStrength;
    }

    //Yo! If an attack ignores lethal defence, then it'll AUTOKILL the defender (if the atkScore surpasses the defender's other defenses)
    //Use with caution ;)
    public int GetLethalDefense(int attackerLethality, bool ignoreLethalDef) 
    {
        if (ignoreLethalDef)
            return -attackerLethality;
        else return _lethalThreshold - attackerLethality;
    }


}

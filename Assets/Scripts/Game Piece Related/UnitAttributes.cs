using SullysToolkit.TableTop;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitAttributes : MonoBehaviour
{
    //Delcarations
    [SerializeField] private int _maxHp;
    [SerializeField] private int _currentHp;
    private GamePiece _gamepiece;

    public delegate void GeneralAttributeEvent();
    public delegate void ValuedAttributeEvent(int value);
    public event GeneralAttributeEvent OnHealthZero;
    public event ValuedAttributeEvent OnDamageTaken;


    //Monobehaviours
    private void Awake()
    {
        _gamepiece = GetComponent<GamePiece>();
    }

    private void OnEnable()
    {
        _gamepiece.OnEnteredPlay += ResetAttributes;
    }

    private void OnDisable()
    {
        _gamepiece.OnEnteredPlay -= ResetAttributes;
    }


    //Internals
    private void ResetAttributes(GamePiece specifiedPiece) { ResetAttributes(); }
    private void ResetAttributes()
    {
        _currentHp = _maxHp;
    }





    //Externals
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





}

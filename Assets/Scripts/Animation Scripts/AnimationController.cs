using SullysToolkit.TableTop;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    [SerializeField] private float _onDamagedAnimationDuration;
    private float _onDamagedTime;
    private bool _isOnDamagedTriggered;

    private Animator _animator;
    private UnitAttributes _unitAttributes;
    GamePiece _gamePiece;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _unitAttributes = GetComponent<UnitAttributes>();
        _gamePiece = GetComponent<GamePiece>();
        
    }

    private void OnEnable()
    {
        _unitAttributes.OnDamageTaken += PlayDamagedAnim;
        _gamePiece.OnEnteredPlay += ResetAnimations;
    }

    private void OnDisable()
    {
        _unitAttributes.OnDamageTaken -= PlayDamagedAnim;
        _gamePiece.OnEnteredPlay -= ResetAnimations;
    }

    private void Update()
    {
        if (_isOnDamagedTriggered)
            ManageOnDamagedAnim();
    }



    private void ResetAnimations(GamePiece piece) {ResetAnimations();}

    private void ResetAnimations()
    {
        _animator.SetBool("IsSelected", false);
        _animator.ResetTrigger("OnDamaged");
    }

    private void PlayDamagedAnim(int value) {PlayDamagedAnim();}


    public void PlayDamagedAnim()
    {
        _animator.SetTrigger("OnDamaged");
        _isOnDamagedTriggered = true;
        _onDamagedTime = 0;

    }

    private void ManageOnDamagedAnim()
    {
        _onDamagedTime += Time.deltaTime;
        
        if (_onDamagedTime >= _onDamagedAnimationDuration)
        {
            _isOnDamagedTriggered = false;
            _onDamagedTime = 0;
            _animator.ResetTrigger("OnDamaged");
        }
    }

    public void EnterSelectedAnim() {_animator.SetBool("IsSelected", true);}
    public void ExitSelectedAnim() {_animator.SetBool("IsSelected", false);}
}

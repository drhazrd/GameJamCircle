using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimation : MonoBehaviour
{
    PlayMove2DLicht playerInput;
    public AudioClip footstepClip;
    Animator _anim;
    int _currentState;
    float _attackAnimTime = .25f;
    private bool _grounded;
    private bool _attacked;
    private bool _landed;
    private bool _jumpTriggered;
    private float _lockedTill;
    private static readonly int Idle = Animator.StringToHash("Idle");
    private static readonly int Walk = Animator.StringToHash("Walk");
    private static readonly int WallRide = Animator.StringToHash("WallRide");
    private static readonly int Jump = Animator.StringToHash("Jump");
    private static readonly int CrouchWalk = Animator.StringToHash("CrouchWalk");
    private static readonly int Death = Animator.StringToHash("Death");
    private static readonly int Attack = Animator.StringToHash("Attack");

    void Start()
    {
        _anim = GetComponent<Animator>();
        playerInput = GetComponent<PlayMove2DLicht>();
    }
    void Update(){
        if(playerInput != null) PlayerData();
        Animate();
    }
    void PlayerData(){
        _grounded = playerInput.IsGrounded();

    }
    void Animate()
    {
        var state = GetState();

        _jumpTriggered = false;
        _landed = false;
        _attacked = false;

        if (state == _currentState) return;
        _anim.CrossFade(state, 0, 0);
        _currentState = state;
    }
    private int GetState()
    {
        if (Time.time < _lockedTill) return _currentState;

        // Priorities
        if (_attacked) return LockState(Attack, _attackAnimTime);
        //if (_player.Crouching) return Crouch;
        //if (_landed) return LockState(Land, _landAnimDuration);
        if (_jumpTriggered) return Jump;

        if (_grounded) return playerInput.horizontal == 0 ? Idle : Walk;
        //return _player.Speed.y > 0 ? Jump : Fall;

        return _currentState;
        int LockState(int s, float t)
        {
           _lockedTill = Time.time + t;
            return s;
        }
    }
    public void Wave(){
        Debug.Log($"Say Hi {this.gameObject.name}");
        _anim.SetTrigger("Wave");
    }
    
    public void FootStep(){
        AudioManager.instance.PlayClip(footstepClip);
    }
}

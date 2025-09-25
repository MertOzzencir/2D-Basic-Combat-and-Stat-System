using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : Entity
{
    [SerializeField] private float SlideRayDistance;
    [SerializeField] private Transform SlideRayPosition;
    [SerializeField] private float SlideRayDelay;




    [Range(0, 1)]


    #region //States

    
    #endregion
    public InputManager InputManager { get; private set; }

   

    private bool _dashState;
    private float _jumpTimer; 
    private int attackIndex = 0;
    private bool canAttack;
    private float lastTimeAttacked;
    private bool canContinueAttack;


    void Awake()
    {
        InitilizeGetComponents();
        InputManager = GetComponent<InputManager>();
    }
    void Start()
    {
        stats = GetComponent<EntityStats>();
        stats.CurrentHealth = stats.Health;
        EntityState.OnDashChange += DashControl;
        StateMachine = new StateMachine();
        SlideState = new Player_SlideState(StateMachine, this);
        IdleState = new Player_IdleState(StateMachine, this);
        MoveState = new Player_MoveState(StateMachine, this);
        JumpState = new Player_JumpState(StateMachine, this);
        AirState = new Player_AirState(StateMachine, this);
        WallJumpState = new Player_WallJumpState(StateMachine, this);
        DashState = new Player_DashState(StateMachine, this);
        AttackState = new Player_AttackState(StateMachine, this);
        DeathState = new Player_DeathState(StateMachine, this);

        StateMachine.Initialize(IdleState);
        InputManager.OnRestart += Restart;
        SR = GetComponentInChildren<SpriteRenderer>();
        MainMaterial =SR.material;
    }

 

    void Update()
    {
        InputVector = InputManager.MovementVector();
        StateMachineUpdate();
        GroundCheck();
        SlideCheck();
    }
    void FixedUpdate() => StateMachineFixedUpdate();

    private void SlideCheck()
    {
        IsSlided = Physics2D.Raycast(transform.position, SlideRayPosition.right, SlideRayDistance, GroundMask);
        SlideWallPosition = Physics2D.Raycast(transform.position, SlideRayPosition.right, SlideRayDistance, GroundMask);
        flipDirection = (SlideWallPosition.point - (Vector2)transform.position).normalized;
    }

    public override void EnterIdle()
    {
        base.EnterIdle();
        InputManager.OnJump += ToJumpState;
        InputManager.OnDash += ToDashState;
        InputManager.OnLeftMouseButton += ToAttackState;
    }
    public override void ExitIdle()
    {
        base.ExitIdle();
        InputManager.OnJump -= ToJumpState;
        InputManager.OnDash -= ToDashState;
        InputManager.OnLeftMouseButton -= ToAttackState;
    }
    public override void UpdateIdle()
    {
        if (InputVector.x != 0)
            StateMachine.ChangeState(MoveState);
        if (RB.linearVelocityY < 0)
            StateMachine.ChangeState(AirState);
    }
    public override void EnterMove()
    {
        base.EnterMove();
        InputManager.OnJump += ToJumpState;
        InputManager.OnDash += ToDashState;
        InputManager.OnLeftMouseButton += ToAttackState;
    }
    public override void ExitMove()
    {
        base.ExitMove();
        InputManager.OnJump -= ToJumpState;
        InputManager.OnDash -= ToDashState;
        InputManager.OnLeftMouseButton -= ToAttackState;
    }
    public override void UpdateMove()
    {
        base.UpdateMove();
         FlipCheck(InputVector);
        if (InputVector.x == 0)
            StateMachine.ChangeState(IdleState);
        if (RB.linearVelocityY < 0)
        {
            if (Time.time > _jumpTimer + JumpCornerThreshold)
                StateMachine.ChangeState(AirState);
        }
        else
            _jumpTimer = Time.time;
        
    }
    public override void MoveEntity()
    {
        base.MoveEntity();
        RB.linearVelocity = new Vector2(InputVector.x * Speed, RB.linearVelocity.y);

    }

    public override void EnterAttack()
    {
        base.EnterAttack();
        canContinueAttack = false;
        InputManager.OnLeftMouseButton += ComboAttack;
        if (attackIndex > 2 || Time.time > lastTimeAttacked + 3f)
            ResetAnimationIndex();
        PlayerAnimator.SetInteger("attackIndex", attackIndex);
        RB.linearVelocity = Vector2.zero;
        canAttack = true;
    }
    public override void ExitAttack()
    {
        PlayerAnimator.SetBool("canAttack", canContinueAttack);
        attackIndex++;
        lastTimeAttacked = Time.time;
    }
    public override void UpdateAttack()
    {
        FlipCheck(InputVector);
        if (InputVector.x != 0)
            canContinueAttack = false;
        var info = PlayerAnimator.GetCurrentAnimatorStateInfo(0);

        if (canContinueAttack && info.normalizedTime >= 1f)
        {
            StateMachine.ChangeState(AttackState);
        }
        else
        {
            if (info.IsName("PLAYER_ATTACK") && info.normalizedTime >= .5f && canAttack)
            {
                Attack();
                canAttack = false;
            }

            if (info.IsName("PLAYER_ATTACK1") && info.normalizedTime >= .3f && canAttack)
            {
                Attack();
                canAttack = false;
            }

            if (info.IsName("PLAYER_ATTACK2") && info.normalizedTime >= .3f && canAttack)
            {
                Attack();
                canAttack = false;
            }

            if (info.IsTag("Attack") && info.normalizedTime >= 1f)
            {
                StateMachine.ChangeState(IdleState);
            }
        }
    }
    private void ComboAttack()
    {
        canContinueAttack = true;
    }

    public override void EnterDeathState()
    {
        base.EnterDeathState();
        if (_isDead)
            return;
        PlayerAnimator.SetTrigger("canDeath");
        InputManager.enabled = false;
        _isDead = true;
        RB.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;

    }
    public override void ExitDeathState()
    {
        base.ExitDeathState();
    }
    public override void UpdateDeathState()
    {
        base.UpdateDeathState();
    }

    private void ResetAnimationIndex()
    {
        attackIndex = 0;
    }



    void DashControl(bool state) => _dashState = state; 

    private void Restart()
    {
        SceneManager.LoadScene(0);
    }
    void OnDrawGizmos()
    {
        Gizmos.DrawRay(transform.position, Vector2.down * GroundedRayDistance);
        Gizmos.DrawRay(SlideRayPosition.position, SlideRayPosition.right * SlideRayDistance);
        Gizmos.DrawWireSphere(_attackPoint.position, _attackRadius);
    }
    public void ToJumpState()
    {
        if (IsGrounded || StateMachine.CurrentState == SlideState || JumpCornerTimer < JumpCornerThreshold)
            StateMachine.ChangeState(JumpState);
    }
     public void ToDashState()
    {
        if (_dashState && Timer.DashTimer > 1)
        {
            StateMachine.ChangeState(DashState);
            Timer.ResetDashTimer();
        }
    }
}

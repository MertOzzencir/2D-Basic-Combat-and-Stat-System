using System;
using System.Collections;
using UnityEngine;

public class Entity : MonoBehaviour
{
    public event Action OnFlip;
    public LayerMask GroundMask;
    public float GroundedRayDistance;
    public Transform _attackPoint;
    public LayerMask _enemyLayerMask;
    public float _attackRadius;
    public bool _isDead;

    public float Speed;
    public float AirSpeed;
    public float DashPower;
    public float JumpPower;
    public float SlidePower;
    public Material DamagedMaterial;
    public Material MainMaterial { get; set; }

    public bool IsGrounded { get; set; }
    public Animator PlayerAnimator { get; set; }
    public Rigidbody2D RB { get; set; }
    #region //States
    public StateMachine StateMachine { get; set; }
    public Player_IdleState IdleState { get; set; }
    public Player_AttackState AttackState { get; set; }
    public Player_DashState DashState { get; set; }
    public Player_SlideState SlideState { get; set; }
    public Player_AirState AirState { get; set; }
    public Player_MoveState MoveState { get; set; }
    public Player_JumpState JumpState { get; set; }
    public Player_WallJumpState WallJumpState { get; set; }
    public Enemy_PatrolState PatrolState { get; set; }
    public Enemy_ReactionState ReactionState { get; set; }
    public Player_DeathState DeathState { get; set; }

    #endregion

    public RaycastHit2D SlideWallPosition { get; set; }


    public Vector2 flipDirection { get; set; }



    public Vector2 InputVector { get; set; }
    public bool IsSlided { get; set; }


    public float JumpCornerTimer { get; set; }
    public float JumpCornerThreshold;
    public EntityStats stats;

    public Collider2D CD => GetComponent<Collider2D>();
    public SpriteRenderer SR;
    public virtual void EnterIdle()
    {
        PlayerAnimator.SetBool("canIdle", true);
        RB.linearVelocity = new Vector2(0, RB.linearVelocity.y);
    }
    public virtual void ExitIdle()
    {
        PlayerAnimator.SetBool("canIdle", false);
    }
    public virtual void UpdateIdle()

    {

    }

    public virtual void EnterMove()
    {
        PlayerAnimator.SetBool("canIdle", true);

    }
    public virtual void ExitMove()
    {
        PlayerAnimator.SetBool("canIdle", false);
    }
    public virtual void UpdateMove()
    {

    }
    public virtual void MoveEntity()
    {

    }

    public virtual void EnterAttack()
    {
        PlayerAnimator.SetBool("canAttack", true);

    }
    public virtual void ExitAttack()
    {
        PlayerAnimator.SetBool("canAttack", false);

    }
    public virtual void UpdateAttack()
    {

    }

    public virtual void EnterPatrol()
    {

    }
    public virtual void ExitPatrol()
    {

    }
    public virtual void UpdatePatrol()
    {

    }

    public virtual void ReactionEnter()
    {

    }
    public virtual void ReactionExit()
    {

    }
    public virtual void ReactionUpdate()
    {

    }

    public virtual void EnterDeathState()
    {

    }
    public virtual void ExitDeathState()
    {

    }
    public virtual void UpdateDeathState()
    {

    }
    public void StateMachineUpdate()
    {
        StateMachine.UpdateState();
    }
    public void StateMachineFixedUpdate()
    {
        StateMachine.FixedUpdateState();
    }
    public void InitilizeGetComponents()
    {
        RB = GetComponent<Rigidbody2D>();
        PlayerAnimator = GetComponentInChildren<Animator>();
    }
    public void GroundCheck()
    {
        IsGrounded = Physics2D.Raycast(transform.position, Vector2.down, GroundedRayDistance, GroundMask);
        JumpCornerTimer = IsGrounded ? 0 : JumpCornerTimer += Time.deltaTime;
    }

    public void Attack()
    {
        Collider2D[] enemies = Physics2D.OverlapCircleAll(_attackPoint.position, _attackRadius);

        foreach (Collider2D collider in enemies)
        {
            IDamageable entity = collider.GetComponent<IDamageable>();
            if (entity == null)
                continue;
            if (!_isDead)
            {
                float generalDamage = stats.GetPhysicalDamage(out bool isCrit); 
                entity.TakeDamage(generalDamage, transform,isCrit);
            }
        }
    }

    public void FlipCheck(Vector2 input)
    {
        if (input.x < 0 && EulerFlipValue() == 0)
            FlipCharacter(new Vector3(0, 180, 0));
        else if (input.x > 0 && EulerFlipValue() > 0)
            FlipCharacter(new Vector3(0, 0, 0));
        OnFlip?.Invoke();
    }
    public void FlipCharacter(Vector3 rotationValue)
    {
        transform.eulerAngles = rotationValue;

    }
    public float EulerFlipValue()
    {
        return transform.eulerAngles.y;
    }

    public void ToAttackState()
    {
        StateMachine.ChangeState(AttackState);
    }

    public virtual void VerticalAndHorizontalAnimationSet()
    {
        PlayerAnimator.SetFloat("xVelocity", RB.linearVelocity.x);
        PlayerAnimator.SetFloat("yVelocity", RB.linearVelocity.y);
    }
}

using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class Enemy : Entity
{

    [SerializeField] private float _distanceToFindPlayer;
    [SerializeField] private float _attackDistance;
    [SerializeField] private Transform _patrolGroundCheck;
    [SerializeField] private float _patrolDistance;
    [SerializeField] private float _amountTimeToPatrol;
    private Player player;
    private bool _rightRay;
    private bool _leftRay;
    private bool canAttack;
    private Vector2 _directionVector;
    private float _patrolTimer;
    private bool _canPatrol;
    private bool _canPatrolMoveStop;
    private float _lastTimeReacted;
    void Awake()
    {
        InitilizeGetComponents();
    }
    void Start()
    {
        stats = GetComponent<EntityStats>();
        stats.CurrentHealth = stats.Health;
        player = FindAnyObjectByType<Player>();
        StateMachine = new StateMachine();
        IdleState = new Player_IdleState(StateMachine, this);
        MoveState = new Player_MoveState(StateMachine, this);
        AttackState = new Player_AttackState(StateMachine, this);
        PatrolState = new Enemy_PatrolState(StateMachine, this);
        ReactionState = new Enemy_ReactionState(StateMachine, this);
        DeathState = new Player_DeathState(StateMachine, this);
        StateMachine.Initialize(IdleState);
        SR = GetComponentInChildren<SpriteRenderer>();
        MainMaterial = SR.material;

    }

    void Update()
    {
        if (player != null)
            _directionVector = player.transform.position - transform.position;
        InputVector = _directionVector.normalized;
        AttackDistanceCheck();
        PatrolGroundCheck();
        StateMachine.UpdateState();
    }
    void FixedUpdate()
    {
        StateMachine.FixedUpdateState();
    }



    public override void EnterIdle()
    {
        base.EnterIdle();
        _patrolTimer = 0;
    }
    public override void ExitIdle()
    {
        base.ExitIdle();
        _patrolTimer = 0;
    }
    public override void UpdateIdle()
    {
        base.UpdateIdle();
        _patrolTimer += Time.deltaTime;
        StateMoveAndGetReadyAttack();
        if (_patrolTimer >= _amountTimeToPatrol)
            StateMachine.ChangeState(PatrolState);
    }



    public override void UpdatePatrol()
    {
        base.UpdatePatrol();
        if (_canPatrol && !_canPatrolMoveStop)
            RB.linearVelocity = transform.right * Speed;
        else
        {
            FlipCharacter(new Vector3(transform.eulerAngles.x, transform.eulerAngles.y - 180, transform.eulerAngles.z));
            StateMachine.ChangeState(IdleState);
        }

        StateMoveAndGetReadyAttack();

    }
 
    public override void UpdateMove()
    {
        base.UpdateMove();
        FlipCheck(InputVector);
        if (!_rightRay && !_leftRay)
        {
            StateMachine.ChangeState(IdleState);
        }
        CheckAttackDistance();
    }
    public override void MoveEntity()
    {
        base.MoveEntity();
        FlipCheck(InputVector);
        RB.linearVelocity = new Vector2(InputVector.x * Speed *1.5f, RB.linearVelocity.y);
    }
   
    public override void EnterAttack()
    {
        base.EnterAttack();

        FlipCheck(InputVector);
        canAttack = true;
        if (Vector3.Distance(transform.position, player.transform.position) < 1f)
            RB.linearVelocity = -(player.transform.position - transform.position).normalized * 5f;
    }
    public override void ExitAttack()
    {
        base.ExitAttack();
    }
    public override void UpdateAttack()
    {


        var info = PlayerAnimator.GetCurrentAnimatorStateInfo(0);
        if (info.IsName("ENEMY_ATTACK") && info.normalizedTime >= .39f && canAttack)
        {
            Attack();
            canAttack = false;
        }
        if (info.IsName("ENEMY_ATTACK") && info.normalizedTime >= 1f)
        {
            if (_leftRay || _rightRay)
                StateMachine.ChangeState(MoveState);
            else
                StateMachine.ChangeState(IdleState);
        }

    }

    public override void ReactionEnter()
    {
        FlipCheck(InputVector);
        PlayerAnimator.SetBool("canReaction", true);
        RB.linearVelocity = Vector2.zero;

    }
    public override void ReactionExit()
    {
        base.ReactionExit();
        _lastTimeReacted = Time.time;
        PlayerAnimator.SetBool("canReaction", false);

    }
    public override void ReactionUpdate()
    {
        base.ReactionUpdate();
        if (Time.time > _lastTimeReacted + 10f)
        {
            var info = PlayerAnimator.GetCurrentAnimatorStateInfo(0);
            if (info.IsName("ENEMY_REACTIONTOPLAYER") && info.normalizedTime >= 1f)
                StateMachine.ChangeState(MoveState);
        }
        else
            StateMachine.ChangeState(MoveState);


    }

    public override void EnterDeathState()
    {
        base.EnterDeathState();
        PlayerAnimator.SetTrigger("canDeath");
        _isDead = true;
        Destroy(gameObject,2f);

    }
    public override void ExitDeathState()
    {
        base.ExitDeathState();
    }
    
    private void StateMoveAndGetReadyAttack()
    {
        if (_rightRay || _leftRay)
        {
            StateMachine.ChangeState(ReactionState);
        }
    }
    private void CheckAttackDistance()
    {
        if (Mathf.Abs(_directionVector.x) < _attackDistance && !player._isDead)
        {
            StateMachine.ChangeState(AttackState);
        }
    }
    private void AttackDistanceCheck()
    {
        _rightRay = Physics2D.Raycast(transform.position, Vector2.right, _distanceToFindPlayer, _enemyLayerMask);
        _leftRay = Physics2D.Raycast(transform.position, Vector2.left, _distanceToFindPlayer, _enemyLayerMask);
    }


    private void PatrolGroundCheck()
    {
        _canPatrol = Physics2D.Raycast(_patrolGroundCheck.position, _patrolGroundCheck.up, _patrolDistance, GroundMask);
        RaycastHit2D hit = Physics2D.Raycast(_patrolGroundCheck.position, -_patrolGroundCheck.right, _patrolDistance, GroundMask);
       
        if (hit.collider != null)
        {
            _canPatrolMoveStop = Vector2.Distance(hit.point, transform.position) < 2f ? true : false;
        }
        else
            _canPatrolMoveStop = false;
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawRay(_patrolGroundCheck.position, _patrolGroundCheck.up * _patrolDistance);
        Gizmos.DrawRay(_patrolGroundCheck.position, -_patrolGroundCheck.right * _patrolDistance);

    }
}

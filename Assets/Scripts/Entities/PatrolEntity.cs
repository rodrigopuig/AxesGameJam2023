using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using StateId = PatrolEntity.State.StateId;

public class PatrolEntity : MonoBehaviour
{
    public float speed;

    protected Transform[] patrolPoints;

    protected int pointCounter;
    protected int patrolPointsLength;

    protected Transform trTarget;
    protected Dictionary<StateId, State> idStatePairs;
    protected State currentState;

    protected bool isDead;

    public Vector3 deathDirection;

    protected void Awake()
    {
        idStatePairs = new Dictionary<StateId, State>()
        {
            [StateId.Idle] = new IdleState(this),
            [StateId.Movement] = new MovementState(this),
            [StateId.TurnAround] = new TurnAroundState(this),
            [StateId.Die] = new DieState(this)
        };

        ChangeState(StateId.Idle);
    }

    // Update is called once per frame
    protected void Update()
    {
        float _deltaTime = Time.deltaTime;
        currentState.Update(_deltaTime);
    }

    public void SetTargets(Transform[] _targets)
    {
        patrolPoints = _targets;
        patrolPointsLength = patrolPoints.Length;
        transform.position = patrolPoints[pointCounter].position;

        pointCounter = (pointCounter + 1) % patrolPointsLength;

        transform.LookAt(transform.position + (patrolPoints[pointCounter].position - transform.position).normalized);
        trTarget = patrolPoints[pointCounter];
    }

    public void ChangeTarget()
    {
        pointCounter = (pointCounter + 1) % patrolPointsLength;
        trTarget = patrolPoints[pointCounter];
    }

    public void ChangeState(StateId _stateId)
    {
        if(currentState == null || currentState.GetStateId() != _stateId)
        {
            currentState?.Exit();
            currentState = idStatePairs[_stateId];
            currentState.Enter();
        }
    }


    public class State
    {
        public enum StateId { Idle, Movement, TurnAround, Die}

        protected PatrolEntity patrolEntity;
        

        public State(PatrolEntity _patrolEntity) { patrolEntity = _patrolEntity; }

        public virtual void Enter()
        {

        }

        public virtual void Exit()
            {

        }

        public virtual void Update(float _deltaTime)
        {
           
        }

        public virtual void ChangeState(StateId _stateId) { patrolEntity.ChangeState(_stateId); }
        public virtual StateId GetStateId() { return StateId.Idle; }
    }

    public class IdleState : State
    {
        public IdleState(PatrolEntity _patrolEntity) : base(_patrolEntity) { }

        public override void Enter()
        {
        }

        public override void Exit()
        {
        }

        public override void Update(float _deltaTime)
        {
            if (patrolEntity.trTarget != null)
                ChangeState(StateId.Movement);
        }

        public override StateId GetStateId() { return StateId.Idle; }
    }

    public class MovementState : State
    {
        public MovementState(PatrolEntity _patrolEntity) : base(_patrolEntity) { }

        Vector3 direction;

        public override void Enter()
        {
        }

        public override void Exit()
        {
        }

        public override void Update(float _deltaTime)
        {
            direction = Vector3.ProjectOnPlane(patrolEntity.trTarget.position - patrolEntity.transform.position, Vector3.up).normalized;
            patrolEntity.transform.position = patrolEntity.transform.position + direction * _deltaTime * patrolEntity.speed;

            if (Vector3.ProjectOnPlane(patrolEntity.trTarget.position - patrolEntity.transform.position, Vector3.up).magnitude < 1f)
            {
                ChangeState(StateId.TurnAround);
            }
        }

        public override StateId GetStateId() { return StateId.Movement; }
    }

    public class TurnAroundState : State
    {
        public TurnAroundState(PatrolEntity _patrolEntity) : base(_patrolEntity) { }

        float fromAngle = 0;
        float targetAngle = 0;

        float time = 1;
        float counter;

        public override void Enter()
        {
            patrolEntity.ChangeTarget();

            Vector2 _forward = -new Vector2(patrolEntity.transform.forward.x, patrolEntity.transform.forward.z);
            fromAngle = patrolEntity.transform.localRotation.eulerAngles.y + 720;//-Vector2.SignedAngle(Vector2.up, _forward) + 720;
            Vector2 _newDirection = new Vector2(patrolEntity.trTarget.position.x - patrolEntity.transform.position.x, patrolEntity.trTarget.position.z - patrolEntity.transform.position.z);
            targetAngle = -Vector2.SignedAngle(Vector2.up, _newDirection) + 720;

            counter = 0;
        }

        public override void Exit()
        {
        }

        public override void Update(float _deltaTime)
        {
            counter += _deltaTime;
            patrolEntity.transform.localRotation = Quaternion.Euler(0, Mathf.Lerp(fromAngle, targetAngle, counter / time), 0);

            if (counter > time)
            {
                counter = 0;
                ChangeState(StateId.Movement);
            }
               
        }

        public override StateId GetStateId() { return StateId.TurnAround; }
    }

    public class DieState : State
    {
        public DieState(PatrolEntity _patrolEntity) : base(_patrolEntity) { }

        float currentAngle;

        float goToShitSpeed = 100;
        float turnSpeed;

        float timeToDesappear = 1;

        public override void Enter()
        {
            currentAngle = patrolEntity.transform.localRotation.eulerAngles.y;
            turnSpeed = 3600;

            GameObject.Destroy(patrolEntity.gameObject, timeToDesappear);
        }

        public override void Exit()
        {
        }

        public override void Update(float _deltaTime)
        {
            currentAngle += _deltaTime * turnSpeed;

            patrolEntity.transform.position = patrolEntity.transform.position + patrolEntity.deathDirection * goToShitSpeed * Time.deltaTime;
            patrolEntity.transform.localRotation = Quaternion.Euler(0, currentAngle, 0);
        }

        public override StateId GetStateId() { return StateId.Die; }
    }
}

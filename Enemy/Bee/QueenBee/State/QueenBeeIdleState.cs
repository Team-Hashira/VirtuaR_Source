using Hashira.Core;
using Hashira.Enemies.Components;
using Hashira.Entities;
using Hashira.FSM;
using Hashira.Players;
using UnityEngine;

namespace Hashira.Enemies.Bee.QueenBee
{
    public class QueenBeeIdleState : EntityState
    {
        private QueenBee _queenBee;

        private EnemyMover _enemyMover;
        private EnemyDetector _enemyDetector;

        private Player _target;

        private bool _isEvasioned;

        public QueenBeeIdleState(Entity entity, StateSO stateSO) : base(entity, stateSO)
        {
            _queenBee = entity as QueenBee;

            _enemyMover = entity.GetEntityComponent<EnemyMover>();
            _enemyDetector = entity.GetEntityComponent<EnemyDetector>();

            _target = PlayerManager.Instance.Player;
            _entityStateMachine.SetShareVariable("Target", _target);

            _isEvasioned = false;
        }

        public override void OnEnter()
        {
            base.OnEnter();
            _enemyMover.StopImmediately();
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            if (!_isEvasioned)
            {
                if (_enemyDetector.IsTargetOnAttackRange(_target.transform, _queenBee.EvasionRange))
                {
                    _entityStateMachine.ChangeState("Evasion");
                    _isEvasioned = true;
                }
            }
        }
    }
}

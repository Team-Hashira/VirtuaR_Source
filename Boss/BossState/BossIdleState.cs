using Hashira.Bosses.Patterns;
using Hashira.Entities;
using Hashira.FSM;
using UnityEngine;

namespace Hashira.Bosses.States
{
    public class BossIdleState : EntityState
    {
        private Boss _boss;
        private float _currentDelayTime;
        private BossPattern _nextPattern;

        public BossIdleState(Entity entity, StateSO stateSO) : base(entity, stateSO)
        {
            _boss = entity as Boss;
        }

        public override void OnEnter()
        {
            base.OnEnter();
            _currentDelayTime = Time.time;
            PickPattern();
        }

        private void PickPattern()
        {
            _nextPattern = _boss.GetRandomBossPattern();
        }

        private void SetBossPattern()
        {
            _boss.SetCurrentBossPattern(_nextPattern);
            _entityStateMachine.ChangeState("Pattern");
        }

        public override void OnUpdate()
        {
            base.OnUpdate();

            if (_currentDelayTime + _boss.PatternPickDelay < Time.time) // 같은 패턴 또 나오면 연속으로 실행
                SetBossPattern();
        }
    }
}

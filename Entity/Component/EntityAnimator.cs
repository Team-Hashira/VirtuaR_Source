using Hashira.Core.AnimationSystem;
using Hashira.Items;
using Hashira.Players;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D.IK;

namespace Hashira.Entities.Components
{
    public enum EAnimationTriggerType
    {
        Start,
        Trigger,
        End,
    }

    public delegate void OnAnimationTriggeredEvent(EAnimationTriggerType triggerType, int count);
    [RequireComponent(typeof(Animator))]
    public class EntityAnimator : MonoBehaviour, IEntityComponent, IAfterInitialzeComponent
    {
        [field: SerializeField] public Animator Animator { get; private set; }
        [SerializeField] private AnimatorParamSO _moveDirParamSO;
        [SerializeField] private AnimatorParamSO _yVelocityParamSO;

        private Entity _entity;
        private EntityMover _mover;
        private EntityRenderer _renderer;

        private Dictionary<EAnimationTriggerType, int> _triggerDictionary = new();
        public event OnAnimationTriggeredEvent OnAnimationTriggeredEvent;

        public void Initialize(Entity entity)
        {
            _entity = entity;
        }

        public void AfterInit()
        {
            _mover = _entity.GetEntityComponent<PlayerMover>();
            _renderer = _entity.GetEntityComponent<EntityRenderer>();
        }

        private void AnimationTrigger(EAnimationTriggerType eAnimationTriggerType)
        {
            if (_triggerDictionary.ContainsKey(eAnimationTriggerType))
                _triggerDictionary[eAnimationTriggerType]++;
            else
                _triggerDictionary[eAnimationTriggerType] = 1;
            OnAnimationTriggeredEvent?.Invoke(eAnimationTriggerType, _triggerDictionary[eAnimationTriggerType]);
        }

        public void ClearAnimationTriggerDictionary()
            => _triggerDictionary.Clear();

        private void Update()
        {
            if (_mover != null && _renderer != null)
            {
                float xVelocity = Mathf.Sign(_mover.Velocity.x) * _renderer.FacingDirection;
                SetParam(_moveDirParamSO, xVelocity);
                SetParam(_yVelocityParamSO, _mover.Velocity.y);
            }
        }

        #region Param Funcs
        public void SetParam(int hash, float value) => Animator?.SetFloat(hash, value);
        public void SetParam(int hash, int value) => Animator?.SetInteger(hash, value);
        public void SetParam(int hash, bool value) => Animator?.SetBool(hash, value);
        public void SetParam(int hash) => Animator?.SetTrigger(hash);

        public void SetParam(AnimatorParamSO param, float value) => Animator?.SetFloat(param.hash, value);
        public void SetParam(AnimatorParamSO param, int value) => Animator?.SetInteger(param.hash, value);
        public void SetParam(AnimatorParamSO param, bool value) => Animator?.SetBool(param.hash, value);
        public void SetParam(AnimatorParamSO param) => Animator?.SetTrigger(param.hash);
        #endregion
    }
}

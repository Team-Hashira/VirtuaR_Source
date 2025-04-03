using Crogen.CrogenPooling;
using Hashira.Combat;
using Hashira.Entities;
using Hashira.MainScreen;
using System;
using UnityEngine;

namespace Hashira
{
    public class Knife : MonoBehaviour, IPoolingObject
    {
        public string OriginPoolType { get; set; }
        GameObject IPoolingObject.gameObject { get; set; }

        [SerializeField] private float _speed;
        [SerializeField] private Vector2 _offset;
        [SerializeField] private Vector2 _size;
        [SerializeField] private LayerMask _whatIsOnlyTarget;
        [SerializeField] private LayerMask _whatIsTarget;
        [SerializeField] private TrailRenderer _trailRenderer;

        private int _damage;
        private Entity _owner;
        private bool _isPenetrable;
        private Vector3 _positionOffset;
        private bool _isShooted;
        private bool _isHited;

        public void Init(int damage, int index, int maxIndex, Entity owner, bool isPenetrable)
        {
            _damage = damage;
            _owner = owner;
            _isPenetrable = isPenetrable;
            _isShooted = false;
            _isHited = false;
            _trailRenderer.enabled = false;

            float indexForCenter = index - (maxIndex - 1) / 2;
            float angle = 20f;
            _positionOffset = Quaternion.Euler(0, 0, -indexForCenter * angle) * Vector3.up * 1.5f;
        }

        private void Update()
        {
            if (_isShooted == false)
            {
                Vector3 targetPos = _owner.transform.position + _positionOffset;
                transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * 10f);
            }
            else if (_isHited == false)
            {
                Vector3 movement = transform.up * _speed * Time.deltaTime;
                RaycastHit2D raycastHit2D = Physics2D.BoxCast(transform.position + transform.rotation * _offset, _size, transform.eulerAngles.z, movement.normalized, movement.magnitude, _isPenetrable ? _whatIsOnlyTarget : _whatIsTarget);
                if (raycastHit2D)
                {
                    _isHited = true;
                    transform.position += movement.normalized * raycastHit2D.distance;
                    if (raycastHit2D.transform.TryGetComponent(out IDamageable damageable))
                    {
                        AttackInfo attackInfo = new AttackInfo(_damage);
                        damageable.ApplyDamage(attackInfo, raycastHit2D, _owner.transform);
                    }
                    CameraManager.Instance.ShakeCamera(5, 5, 0.2f);
                    gameObject.Pop(EffectPoolType.KnifeHitEffect, transform.position, transform.rotation);
                    this.Push();
                }
                else
                    transform.position += movement;
            }
        }

        public void Shoot(Transform target)
        {
            SoundManager.Instance.PlaySFX("StackKnifeThrow", transform, 1f);
            if (target != null)
                transform.up = target.position - transform.position;
            else
            {
                Vector3 mousePos = Hashira.CanvasUI.UIManager.WorldMousePosition;
                mousePos = MainScreenEffect.OriginPositionConvert(mousePos);
                transform.up = mousePos - transform.position;
            }
                _isShooted = true;
            _trailRenderer.enabled = true;
        }

        public void OnPop()
        {

        }

        public void OnPush()
        {

        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, Vector3.one);
            Gizmos.DrawWireCube(_offset, _size);
        }
    }
}

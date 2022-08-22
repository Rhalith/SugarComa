using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBox : MonoBehaviour, IHitDetector
{
    [SerializeField] private BoxCollider _collider;
    [SerializeField] private LayerMask _layerMask;
    [SerializeField] private HurtBoxMask _hurtBoxMask;

    private float _thickness = 0.025f;
    private IHitResponder _hitResponder;
    public IHitResponder HitResponder { get => _hitResponder; set => _hitResponder = value; }

    public void CheckHit()
    {
        Vector3 _scaledSize = new Vector3(_collider.size.x * transform.lossyScale.x, _collider.size.y * transform.lossyScale.y, _collider.size.z * transform.lossyScale.z);
        float _distance = _scaledSize.y - _thickness;
        Vector3 _direction = transform.up;
        Vector3 _center = transform.TransformPoint(_collider.center);
        Vector3 _start = _center - _direction * (_distance / 2);
        Vector3 _halfExtents = new Vector3(_scaledSize.x, _thickness, _scaledSize.z) / 2;
        Quaternion _orientation = transform.rotation;

        HitData _hitData = null;
        IHurtBox _hurtBox = null;
        RaycastHit[] _hits = Physics.BoxCastAll(_start, _halfExtents, _direction, _orientation, _distance, _layerMask);

        foreach (RaycastHit _hit in _hits)
        {
            if(_hit.collider.TryGetComponent<IHurtBox>(out _hurtBox))
            {
                if (_hurtBox.Active)
                {
                    if (_hurtBoxMask.HasFlag((HurtBoxMask)_hurtBox.Type))
                    {
                        _hitData = new HitData
                        {
                            damage = _hitResponder == null ? 0 : _hitResponder.damage,
                            hitPoint = _hit.point == Vector3.zero ? _center : _hit.point,
                            hitNormal = _hit.normal,
                            hurtBox = _hurtBox,
                            hitDetector = this
                        };
                        if (_hitData.Validate())
                        {
                            _hitData.hitDetector.HitResponder?.Response(_hitData);
                            _hitData.hurtBox.HurtResponder?.Response(_hitData);
                        }
                    }
                }
            }
        }
    }
}

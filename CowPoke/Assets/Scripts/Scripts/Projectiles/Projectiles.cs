using UnityEngine;
using System.Collections.Generic;

public struct HitPayload { public float damage; public bool isCrit; public StatusEffectDef[] effects; }

public class Projectile : MonoBehaviour
{
    HitPayload _payload;
    float _speed, _lifetime;
    int _remainingPierce;
    float _age;
    Vector3 _lastPos;

    public void Init(HitPayload payload, float speed, float lifetime, int pierce)
    {
        _payload = payload; _speed = speed; _lifetime = lifetime;
        _remainingPierce = pierce; _age = 0f;
        _lastPos = transform.position;
        gameObject.SetActive(true);
    }

    void Update()
    {
        float dt = Time.deltaTime;
        _age += dt;
        if (_age >= _lifetime) { Despawn(); return; }

        Vector3 next = transform.position + transform.forward * (_speed * dt);

        // Continuous collision check
        if (Physics.SphereCast(_lastPos, 0.15f, (next - _lastPos).normalized, out var hit,
                               Vector3.Distance(_lastPos, next), LayerMask.GetMask("Enemy")))
        {
            if (hit.collider.TryGetComponent<IDamageable>(out var d))
            {
                d.ApplyHit(new HitContext {
                    damage = _payload.damage,
                    isCrit = _payload.isCrit,
                    effects = _payload.effects,
                    hitPoint = hit.point
                });

                if (_remainingPierce-- <= 0) { Despawn(); return; }
            }
        }

        transform.position = next;
        _lastPos = next;
    }

    void Despawn() => gameObject.SetActive(false); // pooled in practice
}

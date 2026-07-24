using UnityEngine;

public class Projectile_Rocket : MonoBehaviour
{
    
    private float speed;
    private int damage;
    public float turnRate = 5f; // how sharply rocket can turn
    private MoleCule target;
    private Vector3 velocity;

    public void SetProjectileSND(float _speed, int _damage)
    {
        speed = _speed;
        damage = _damage;
    }
    public void SetTarget(MoleCule _target)
    {
        target = _target;
        velocity = transform.up * speed; // initial forward velocity
    }

    void Update()
    {
        if(Machine.instance.isOver) return;

        if (target == null)
        {
            Destroy(gameObject);
            return;
        }

        // Desired direction toward target
        Vector3 dir = (target.transform.position - transform.position).normalized * speed;

        // Smoothly steer toward target
        velocity = Vector3.Lerp(velocity, dir, turnRate * Time.deltaTime);

        // Move rocket
        transform.position += velocity * Time.deltaTime;

        // Rotate rocket to face movement direction
        float angle = Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg - 90f;
        transform.rotation = Quaternion.Euler(0, 0, angle);

        // Hit check
        if (Vector3.Distance(transform.position, target.transform.position) < 0.3f)
        {
            target.TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}

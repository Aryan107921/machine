using UnityEngine;

public class Projectile : MonoBehaviour
{
    

    private float speed;
    private int damage;
    private MoleCule target;
    private bool hasHit = false;   // ensures damage is applied only once
   
    public void SetProjectileSND(float _speed, int _damage)
    {
        speed = _speed;
        
        damage = _damage;
    }

    public void SetTarget(MoleCule _target)
    {
        target = _target;
    }

    
    void Update()
    {
        if(Machine.instance.isOver) return;

        if (target == null)
        {
            Destroy(gameObject);
            return;
        }

        transform.position = Vector3.MoveTowards(
            transform.position,
            target.transform.position,
            speed * Time.deltaTime
        );

        if (!hasHit && Vector3.Distance(transform.position, target.transform.position) < 0.1f)
        {
            hasHit = true;
            target.TakeDamage(damage);
            Destroy(this.gameObject);
        }
    }

}

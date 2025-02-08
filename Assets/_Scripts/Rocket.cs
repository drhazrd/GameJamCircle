using UnityEngine;

public class Rocket : MonoBehaviour
{
    private Transform target;
    public float speed = 3f;
    public GameObject destroyVFX;

    public void Seek(Transform _target)
    {
        target = _target;
    }
    void Update()
    {
        if(target == null){
            Destroy(gameObject);
            if(destroyVFX != null) Instantiate(destroyVFX, transform.position, transform.rotation);
            return;
        }

        Vector3 dir = target.position - transform.position;
        transform.LookAt(target);
        float distanceThisFrame = speed * Time.deltaTime;

        if(dir.magnitude <= distanceThisFrame){
            HitTarget();
            return;
        }
        transform.Translate(dir.normalized * distanceThisFrame, Space.World);
    }

    private void HitTarget()
    {
        Destroy(gameObject);
        Destroy(target.gameObject);
        if(destroyVFX != null) Instantiate(destroyVFX, transform.position, transform.rotation);
    }
}

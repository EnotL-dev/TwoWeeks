using UnityEngine;

public class DestroyMyselfByTimer : MonoBehaviour
{
    [SerializeField] private float lifeTimer = 1f;
    private void Update()
    {
        lifeTimer -= Time.deltaTime;

        if (lifeTimer < 0)
            Destroy(gameObject);
    }
}

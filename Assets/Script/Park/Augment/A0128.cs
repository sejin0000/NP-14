using UnityEngine;

public class A0128 : MonoBehaviour
{
    int objSize;// 돌아가는 투사체 갯수
    public float circleR = 1f; //반지름
    private float deg; //각도
    public float objSpeed = 140f; //원운동 속도
    public GameObject[] target;
    public PlayerStatHandler playerStat;

    private void Start()
    {
        objSize = target.Length;
        transform.localPosition = Vector3.zero;
    }
    void Update()
    {
        deg += Time.deltaTime * objSpeed;
        if (deg < 360)
        {
            for (int i = 0; i < objSize; i++)
            {
                var rad = Mathf.Deg2Rad * (deg + (i * (360 / objSize)));
                var x = circleR * Mathf.Sin(rad);
                var y = circleR * Mathf.Cos(rad);
                target[i].transform.position = transform.position + new Vector3(x, y);
            }
        }
        else
        {
            deg = 0;
        }

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Bullet _Bullet = collision.GetComponent<Bullet>();
        if (_Bullet != null && _Bullet.targets.ContainsValue((int)BulletTarget.Player))
        {
            if (_Bullet.targets.ContainsValue((int)BulletTarget.Enemy))
            {
                Destroy(collision.gameObject);
            }

        }
    }
}

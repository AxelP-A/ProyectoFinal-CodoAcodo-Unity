using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] float maxDistance;
    private float currentDistance;

    void Awake()
    {
        speed = 8;
        maxDistance = 50;
    }


    // Update is called once per frame
    void Update()
    {
        Vector2 pos = transform.position;
        pos = new Vector2(pos.x, pos.y + speed * Time.deltaTime);
        transform.position = pos;

        currentDistance += speed * Time.deltaTime;

        if (currentDistance > maxDistance)
        {
            BulletFactory.Instance.ReturnBullet(this);
        }
    }

    private void Reset()
    {
        currentDistance = 0;
    }

    public static void TurnOn(Bullet bullet)
    {
        bullet.Reset();
        bullet.gameObject.SetActive(true);
    }

    public static void TurnOff(Bullet bullet)
    {
        bullet.gameObject.SetActive(false);

    }
}

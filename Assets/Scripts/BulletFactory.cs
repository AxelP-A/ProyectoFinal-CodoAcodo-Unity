using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletFactory : MonoBehaviour
{
    public static BulletFactory Instance
    {
        get
        {
            return _instance;
        }
    }

    private static BulletFactory _instance;
    [SerializeField] Bullet _bullet;
    [SerializeField] int _initialStock;

    private ObjectPooler<Bullet> bulletPool;

    private void Awake()
    {
        _instance = this;
        bulletPool = new ObjectPooler<Bullet>(BulletCreator, Bullet.TurnOn, Bullet.TurnOff, _initialStock);
    }

    private Bullet BulletCreator()
    {
        return Instantiate(_bullet);
    }

    public Bullet GetBullet()
    {
        return bulletPool.GetObject();
    }

    public void ReturnBullet(Bullet bullet)
    {
        bulletPool.ReturnObject(bullet);
    }

}
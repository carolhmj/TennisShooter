using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolingManager : MonoBehaviour
{
    private static ObjectPoolingManager instance;
    public static ObjectPoolingManager Instance { get { return instance; } }

    public GameObject bulletPrefab;
    public int bulletAmount = 20;

    private List<GameObject> bullets;

    private void Awake()
    {
        instance = this;

        bullets = new List<GameObject>(bulletAmount);
        for (int i = 0; i < bulletAmount; i++)
        {
            GameObject prefabInstance = Instantiate(bulletPrefab);
            prefabInstance.transform.SetParent(transform);
            prefabInstance.SetActive(false);

            bullets.Add(prefabInstance);
        }
    }

    public GameObject GetBullet(bool shotByPlayer)
    {
        foreach(GameObject bullet in bullets)
        {
            if (!bullet.activeInHierarchy)
            {
                bullet.GetComponent<Bullet>().ShotByPlayer = shotByPlayer;
                bullet.SetActive(true);
                return bullet;
            }
        }

        GameObject prefabInstance = Instantiate(bulletPrefab);
        prefabInstance.GetComponent<Bullet>().ShotByPlayer = shotByPlayer;
        prefabInstance.transform.SetParent(transform);
        bullets.Add(prefabInstance);

        return prefabInstance;
    }
}

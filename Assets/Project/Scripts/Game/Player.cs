using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Visuals")]
    public Camera playerCamera;

    [Header("Gameplay")]
    public int initialHealth = 100;
    public int initialAmmo = 12;
    public float knockbackForce = 10f;
    public float hurtDuration = 0.5f;

    private bool isHurt = false;

    private int health;
    public int Health { get { return health; } }

    private int ammo;
    public int Ammo { get { return ammo; } }

    private bool killed;
    public bool Killed { get { return killed; } }

    // Start is called before the first frame update
    void Start()
    {
        health = initialHealth;
        ammo = initialAmmo;        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (ammo > 0 && !killed)
            {
                ammo--;
                
                GameObject bulletObject = ObjectPoolingManager.Instance.GetBullet(true);
                bulletObject.transform.position = playerCamera.transform.position + playerCamera.transform.forward;
                bulletObject.transform.forward = playerCamera.transform.forward;
            }
        }
    }

    void OnTriggerEnter(Collider otherCollider)
    {
        if (otherCollider.GetComponent<AmmoCrate>() != null)
        {
            AmmoCrate ammoCrate = otherCollider.GetComponent<AmmoCrate>();
            ammo += ammoCrate.ammo;

            Destroy(ammoCrate.gameObject);
        } else if (otherCollider.GetComponent<HealthCrate>() != null)
        {
            HealthCrate healthCrate = otherCollider.GetComponent<HealthCrate>();
            health += healthCrate.health;

            Destroy(healthCrate.gameObject);
        }

        if (!isHurt)
        {
            GameObject hazard = null;
            if (otherCollider.GetComponent<Enemy>() != null)
            {
                Debug.Log("Collide with enemy");
                Enemy enemy = otherCollider.GetComponent<Enemy>();
                if (!enemy.Killed)
                {
                    hazard = enemy.gameObject;
                    health -= enemy.damage;
                }
            } 
            else if (otherCollider.GetComponent<Bullet>() != null)
            {
                Bullet bullet = otherCollider.GetComponent<Bullet>();
                if (!bullet.ShotByPlayer)
                {
                    Debug.Log("Collide with bullet");
                    hazard = bullet.gameObject;
                    health -= bullet.damage;

                    bullet.gameObject.SetActive(false);
                }
            }

            if (hazard != null)
            {
                isHurt = true;

                Vector3 hurtDirection = (transform.position - hazard.transform.position).normalized;
                Vector3 knockbackDirection = (hurtDirection + Vector3.up).normalized;
                GetComponent<ForceReceiver>().AddForce(knockbackDirection, knockbackForce);

                StartCoroutine(HurtRoutine());
            }
        }

        if (health <= 0 && !killed)
        {
            killed = true;
            OnKill();
        }
    }

    private void OnKill()
    {
        GetComponent<CharacterController>().enabled = false;
        GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().enabled = false;
    }

    IEnumerator HurtRoutine()
    {
        yield return new WaitForSeconds(hurtDuration);

        isHurt = false;
    }
}

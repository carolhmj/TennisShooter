﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int health = 5;
    public int damage = 5;

    private bool killed = false;
    public bool Killed { get { return killed; } }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Bullet>() != null)
        {
            Bullet bullet = other.GetComponent<Bullet>();

            if (bullet.ShotByPlayer)
            {
                health -= bullet.damage;

                bullet.gameObject.SetActive(false);

                if (health <= 0 && !killed)
                {
                    killed = true;
                    OnKill();
                }
            }
        }
    }

    protected virtual void OnKill()
    {

    }
}

﻿using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.AI;

public class ShootingEnemy : Enemy
{
    public AudioSource deathSound;
    public float shootingInterval = 4f;
    public float shootingDistance = 10f;
    public float chasingInterval = 2f;
    public float chasingDistance = 12f;

    private Player player;
    private float shootingTimer;
    private float chasingTimer;
    private NavMeshAgent agent;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
        shootingTimer = Random.Range(1, shootingTimer);
        agent = GetComponent<NavMeshAgent>();

        agent.SetDestination(player.transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        if (player.Killed)
        {
            agent.enabled = false;
            this.enabled = false;
            GetComponent<Rigidbody>().isKinematic = true;
        }

        shootingTimer -= Time.deltaTime;
        if (shootingTimer <= 0 && Vector3.Distance(transform.position, player.transform.position) <= shootingDistance)
        {
            shootingTimer = shootingInterval;

            GameObject bullet = ObjectPoolingManager.Instance.GetBullet(false);
            bullet.transform.position = transform.position;
            bullet.transform.forward = (player.transform.position - transform.position).normalized;

        }

        chasingTimer -= Time.deltaTime;
        if (chasingTimer <= 0 && Vector3.Distance(transform.position, player.transform.position) <= chasingDistance)
        {
            agent.SetDestination(player.transform.position);

            chasingTimer = chasingInterval;
        }
    }

    protected override void OnKill()
    {
        base.OnKill();

        deathSound.Play();

        agent.enabled = false;
        this.enabled = false;

        transform.localEulerAngles = new Vector3(10, transform.localEulerAngles.y, transform.localEulerAngles.z);
    }
}

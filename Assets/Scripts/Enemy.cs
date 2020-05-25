
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class Enemy : MonoBehaviour
{
    internal List<Vector3> _enemyPath;
    public NavMeshAgent _navAgent;
    public GameObject _player;
    public Game _game;
    private bool _attack = false;

    private void Awake()
    {
        _navAgent = GetComponent<NavMeshAgent>();
        _player = GameObject.FindGameObjectWithTag("Player");
        _game = FindObjectOfType<Game>();
    }

    void Update()
    {
        CheckPath();
    }

    private void FixedUpdate()
    {
        if (Physics.Raycast(transform.position, Vector3.forward, 4))
        {
            Alarm();
        }
    }

    private void CheckPath()
    {
        if (_navAgent.remainingDistance < 0.1f)
        {
            SetPath();
        }

        if (_navAgent.remainingDistance < 0.3f && _attack)
        {
            _game.GameOver();
        }
    }

    public void SetPath()
    {
        var random = Random.Range(0, _enemyPath.Count);
        _navAgent.SetDestination(_enemyPath[random]);
    }

    public void Alarm()
    {
        _navAgent.SetDestination(_player.transform.position);
        _attack = true;
    }
}

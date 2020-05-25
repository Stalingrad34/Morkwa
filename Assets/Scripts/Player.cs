using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class Player : MonoBehaviour
{

    [SerializeField] private float _speed;
    [SerializeField] private Image _noise;
    [SerializeField] private Vector3 _noiseOffset;

    private CharacterController _charController;
    private Animator _animator;
    public UnityEvent alarm;   

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _charController = GetComponent<CharacterController>();
    }

    private void Start()
    {
        _noise.fillAmount = 0;
    }

    private void Update()
    {
        MoveCharacter();
        CheckNoise();
    }   

    private void MoveCharacter()
    {
        float deltaX = Input.GetAxis("Horizontal") * _speed;
        float deltaZ = Input.GetAxis("Vertical") * _speed;

        Vector3 movement = new Vector3(deltaX, 0, deltaZ);
        _charController.Move(movement * Time.deltaTime);


        if (movement != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(movement);
            _animator.SetBool("Walk", true);
            _noise.fillAmount += (Time.deltaTime * 0.3f);
        }
        else
        {
            _animator.SetBool("Walk", false);
            _noise.fillAmount -= (Time.deltaTime * 0.2f);
        }
    }

    private void CheckNoise()
    {
        _noise.GetComponent<RectTransform>().position = Camera.main.WorldToScreenPoint(transform.position + _noiseOffset);
        if (_noise.fillAmount == 1)
        {
            alarm.Invoke();
        }
    }
}

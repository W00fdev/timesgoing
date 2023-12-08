using System;
using System.Collections;
using UnityEngine;

public class Character : MonoBehaviour
{
    public float Speed;
    public float ShootingCooldown;
    public float ReloadingTime;
    public bool CanShoot;
    public bool IsReloading;

    [Range(0f, 1f)]
    public float Spread;

    public GameObject BulletPrefab;
    public Transform GunPoint;
    public LayerMask RaycastFloor;

    private Animatable _animatable;
    private BulletCounterView _bulletCounterView;
    private CharacterController _controller;
    private Vector3 _input;
    private float _time;

    [SerializeField]
    private int _maxBullets;
    private int _curBullets;

    private void Awake()
    {
        _controller = GetComponent<CharacterController>();
        _bulletCounterView = GetComponent<BulletCounterView>();
        _animatable = GetComponent<Animatable>();

        CanShoot = true;
        _time = 0f;

        _curBullets = _maxBullets;

        StartCoroutine(CooldownRoutine());
    }

    private void Update()
    {
        ProcessMovement();
        RotateCharacter();
    }

    private void LateUpdate()
    {
        HandleShooting();
    }

    private void HandleShooting()
    {
        if (IsReloading == true)
            return;

        if (Input.GetMouseButtonDown(0) && CanShoot == true)
        {
            Vector3 bulletEuler = transform.forward;
            bulletEuler.z += UnityEngine.Random.Range(-Spread, Spread);
            bulletEuler.x += UnityEngine.Random.Range(-Spread, Spread);

            var bullet = Instantiate(BulletPrefab, GunPoint.position, Quaternion.Euler(bulletEuler)).GetComponent<Bullet>();
            bullet.Initialize(bulletEuler);
            CanShoot = false;
            _time = 0f;

            _curBullets = Mathf.Clamp(_curBullets - 1, 0, _maxBullets);
            if (_curBullets == 0)
            {
                StartCoroutine(ReloadingRoutine());
                return;
            }

            _bulletCounterView.ShootBullet();
        }
    }

    private void RotateCharacter()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, 50f, RaycastFloor.value))
        {
            Vector3 projectionAccurateDirection = Vector3.ProjectOnPlane(hit.point - transform.position, Vector3.up);

            float radiansVectorsAngle = Vector3.SignedAngle(Camera.main.transform.forward, hit.normal, hit.normal) * Mathf.Deg2Rad;
            projectionAccurateDirection.z += Mathf.Sin(radiansVectorsAngle) * _controller.height;
            projectionAccurateDirection.x += Mathf.Sin(radiansVectorsAngle) * _controller.height;

            //Debug.Log(projectionAccurateDirection);
            transform.forward = projectionAccurateDirection;
        }
    }

    private void ProcessMovement()
    {
        _input = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical"));
        _input = TranslateCameraDirection(_input);

        Vector3 relativeInput = Vector3.zero;
        var relativeAngleZ = Vector3.SignedAngle(_input, transform.forward, transform.forward) * Mathf.Deg2Rad;
        var relativeAngleX = Vector3.SignedAngle(_input, transform.right, transform.right) * Mathf.Deg2Rad;
        
        if (_input.z != 0)
            relativeInput.z = Mathf.Cos(relativeAngleZ);

        if (_input.x != 0)
            relativeInput.x = Mathf.Cos(relativeAngleX);

        _animatable.ProcessInput(relativeInput);

        var moving = _input * Speed ;
        _controller.Move(moving * Time.deltaTime);
    }

    private Vector3 TranslateCameraDirection(Vector3 inputVector)
    {
        inputVector.Normalize();

        if (inputVector != Vector3.zero)
            inputVector = Camera.main.transform.TransformDirection(inputVector);

        inputVector = Vector3.ProjectOnPlane(inputVector, Vector3.up);
        inputVector.Normalize();

        return inputVector;
    }

    private void OnDrawGizmos()
    {/*
        Gizmos.color = Color.yellow;
        
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, 50f, RaycastFloor.value))
        {
            Gizmos.DrawLine(hit.point, hit.point + Vector3.up * 1f);
        }*/
    }

    IEnumerator ReloadingRoutine()
    {
        IsReloading = true;
        yield return new WaitForSeconds(ReloadingTime);
        IsReloading = false;
        _bulletCounterView.RefillAmmo();
    }

    IEnumerator CooldownRoutine()
    {
        while (true)
        {
            if (CanShoot == true || IsReloading == true)
                yield return null;

            if (CanShoot == false)
            {
                _time += Time.deltaTime;
                
                if (_time >= ShootingCooldown)
                {
                    CanShoot = true;
                    _time = 0f;
                }    

                yield return null;
            }
        }
    }
}
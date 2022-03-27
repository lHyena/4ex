using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LernProject
{
    public class Enemy : MonoBehaviour , ITakeDamage
    {
        [SerializeField] private Player _player; // нахождение игрока
        [SerializeField] private GameObject _bulletPrefab;
        [SerializeField] private Transform _spawnPosition;
        private Vector3 _direction; // направление врага *
        public float speed = 1.5f; // скорость врага
        [SerializeField] private float _durability = 1f; //*
        [SerializeField] private float _cooldown;
        [SerializeField] private bool _isFire;

        [SerializeField] private float _speedRotate;
        private bool _look;
        private bool _isSpawnBool;

        void Start()
        {
            _player = FindObjectOfType<Player>();// это нежелательный метод, тк при большом кол-ве игроков, может заглючить игру

        }

        void FixedUpdate() // проверка значений
        {

            if (Vector3.Distance(transform.position, _player.transform.position) < 6)
            {
                _look = false;
                Look();
            }

        }

        private void Update()
        {
            /*transform.LookAt(_player.transform);*/ // поворот противника (персонажа)на игрока

            transform.Translate(Vector3.forward * Time.deltaTime * speed); // изменение на позиции. движение.

            Ray ray = new Ray(_spawnPosition.position, transform.forward); // определение местонахождение игрока лучем

            if (Physics.Raycast(ray, out RaycastHit hit, 6) )//Raycast - нахождение первого предмета 
            {
                Debug.DrawRay(_spawnPosition.position, transform.forward * hit.distance, Color.blue); // проверка луча

                if (hit.collider.CompareTag("Player")) // если луч(ray), выпущенный из направления(_spawnPosition.position)
                {                                      // на дистанцию 6, столкнулся(hit), с collider.CompareTag("Player"), только тогда Fire().
                    if (_isFire)
                        Fire();

                }

                //_look = true;

            }

            //if (Vector3.Distance(transform.position, _player.transform.position) < 6)
            //{
            //    _look = true;
            //}

        }

        private void Fire()
        {
            _isFire = false;
            var shieldObj = Instantiate(_bulletPrefab, _spawnPosition.position, _spawnPosition.rotation);
            var shield = shieldObj.GetComponent<Bullet>(); // получем ссылку на экземпляр класса ( щита)
            shield.Init(_player.transform, 10, 0.6f);

           Invoke(nameof(Reloading), _cooldown);

        }
        private void Reloading() // перезарядка стрельбы
        {
            _isFire = true;
        }

        public void Init(float durability) //*
        {
            _durability = durability;

            Destroy(gameObject, t: 1f);
        }

        public void Hit(float damage) //Уничтожение *
        {
            _durability -= damage;

            if (_durability <= 0)
            {
                Destroy(gameObject);
            }
        }
        

        private void Look()
        {
            transform.LookAt(_player.transform); // поворот противника (персонажа)на игрока

            var direction = _player.transform.position - transform.position;
            var stepRotate = Vector3.RotateTowards(transform.forward, direction, _speedRotate * Time.fixedDeltaTime, 0f);// (указываем текущее направление взгляда, указываем точку взгляда


            transform.rotation = Quaternion.LookRotation(stepRotate);// указание направления
        }


    }
}





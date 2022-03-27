using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LernProject
{
    public class Enemy : MonoBehaviour , ITakeDamage
    {
        [SerializeField] private Player _player; // ���������� ������
        [SerializeField] private GameObject _bulletPrefab;
        [SerializeField] private Transform _spawnPosition;
        private Vector3 _direction; // ����������� ����� *
        public float speed = 1.5f; // �������� �����
        [SerializeField] private float _durability = 1f; //*
        [SerializeField] private float _cooldown;
        [SerializeField] private bool _isFire;

        [SerializeField] private float _speedRotate;
        private bool _look;
        private bool _isSpawnBool;

        void Start()
        {
            _player = FindObjectOfType<Player>();// ��� ������������� �����, �� ��� ������� ���-�� �������, ����� ��������� ����

        }

        void FixedUpdate() // �������� ��������
        {

            if (Vector3.Distance(transform.position, _player.transform.position) < 6)
            {
                _look = false;
                Look();
            }

        }

        private void Update()
        {
            /*transform.LookAt(_player.transform);*/ // ������� ���������� (���������)�� ������

            transform.Translate(Vector3.forward * Time.deltaTime * speed); // ��������� �� �������. ��������.

            Ray ray = new Ray(_spawnPosition.position, transform.forward); // ����������� ��������������� ������ �����

            if (Physics.Raycast(ray, out RaycastHit hit, 6) )//Raycast - ���������� ������� �������� 
            {
                Debug.DrawRay(_spawnPosition.position, transform.forward * hit.distance, Color.blue); // �������� ����

                if (hit.collider.CompareTag("Player")) // ���� ���(ray), ���������� �� �����������(_spawnPosition.position)
                {                                      // �� ��������� 6, ����������(hit), � collider.CompareTag("Player"), ������ ����� Fire().
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
            var shield = shieldObj.GetComponent<Bullet>(); // ������� ������ �� ��������� ������ ( ����)
            shield.Init(_player.transform, 10, 0.6f);

           Invoke(nameof(Reloading), _cooldown);

        }
        private void Reloading() // ����������� ��������
        {
            _isFire = true;
        }

        public void Init(float durability) //*
        {
            _durability = durability;

            Destroy(gameObject, t: 1f);
        }

        public void Hit(float damage) //����������� *
        {
            _durability -= damage;

            if (_durability <= 0)
            {
                Destroy(gameObject);
            }
        }
        

        private void Look()
        {
            transform.LookAt(_player.transform); // ������� ���������� (���������)�� ������

            var direction = _player.transform.position - transform.position;
            var stepRotate = Vector3.RotateTowards(transform.forward, direction, _speedRotate * Time.fixedDeltaTime, 0f);// (��������� ������� ����������� �������, ��������� ����� �������


            transform.rotation = Quaternion.LookRotation(stepRotate);// �������� �����������
        }


    }
}





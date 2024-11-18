using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Renderer))]
[RequireComponent(typeof(Rigidbody))]
public class Cube : MonoBehaviour
{
    [SerializeField] private AudioSource[] _soundsDrop;

    private Renderer _renderer;
    private Rigidbody _rigidbody;
    private AudioSource _drop;
    private bool _isTouch;
    private float _timeLife;

    public event Action<Cube> Deathed;

    private void Awake()
    {
        _renderer = GetComponent<Renderer>();
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        _drop = GetRandomSoundDrop();

        if (_isTouch == false)
        {
            if (collision.collider.TryGetComponent(out Platform platform))
            {
                _drop.Play();
                _renderer.material.color = UnityEngine.Random.ColorHSV();
                _isTouch = true;

                StartCoroutine(ReturnToPool());
            }
        }
    }

    public void Initialize(Vector3 position)
    {
        _renderer.material.color = Color.blue;
        transform.position = position;
        _rigidbody.velocity = Vector3.zero;
        transform.rotation = Quaternion.identity;
        _isTouch = false;
    }

    private AudioSource GetRandomSoundDrop()
    {
        return _soundsDrop[UnityEngine.Random.Range(0, _soundsDrop.Length)];
    }

    private IEnumerator ReturnToPool()
    {
        _timeLife = GetRandomDelay();

        yield return new WaitForSeconds(_timeLife);

        Deathed?.Invoke(this);
    }

    private float GetRandomDelay()
    {
        float minRandom = 2f, maxRandom = 5f;

        return UnityEngine.Random.Range(minRandom, maxRandom);
    }
}
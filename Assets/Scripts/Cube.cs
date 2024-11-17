using System;
using UnityEngine;

[RequireComponent(typeof(Renderer))]
[RequireComponent(typeof(Rigidbody))]
public class Cube : MonoBehaviour
{
    [SerializeField] private AudioSource[] _soundsDrop;

    private Renderer _renderer;
    private Rigidbody _rigidbody;
    private bool _isTouch;

    public event Action<Cube> Touched;

    private void Awake()
    {
        _renderer = GetComponent<Renderer>();
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        AudioSource drop = GetRandomSoundDrop();

        if (collision.collider.TryGetComponent(out Platform platform))
        {
            Touched?.Invoke(this);

            if (_isTouch == false)
            {
                drop.Play();
                _renderer.material.color = UnityEngine.Random.ColorHSV();
                _isTouch = true;
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
}
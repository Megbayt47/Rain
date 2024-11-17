using System.Collections;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private Cube _cube;
    [SerializeField] private Platform _platform;
    [SerializeField] private int _spawnCount = 10;
    [SerializeField] private float _hightPoint = 20;
    [SerializeField] private float _repiteRate = 1f;

    private Pool<Cube> _pool;
    private float _delay;
    private Vector3 _position;
    private Color _color;
    private WaitForSeconds _wait;

    private void Awake()
    {
        _pool = new Pool<Cube>(_cube, _spawnCount);
        _wait = new WaitForSeconds(_repiteRate);
    }

    private void Start()
    {
        StartCoroutine(SpawnCubes());
    }

    private IEnumerator SpawnCubes()
    {
        while (enabled)
        {
            _position = GetRandomPosition();

            Cube cube = _pool.GetObject();
            cube.Initialize(_position);
            cube.Touched += OnTouch;

            yield return _wait;
        }
    }

    private IEnumerator ReturnToPool(Cube cube)
    {
        _delay = GetRandomDelay();

        yield return new WaitForSeconds(_delay);

        _pool.ReleaseObject(cube);
    }

    private void OnTouch(Cube cube)
    {
        StartCoroutine(ReturnToPool(cube));
        cube.Touched -= OnTouch;
    }

    private float GetRandomDelay()
    {
        float minRandom = 2f, maxRandom = 5f;

        return Random.Range(minRandom, maxRandom);
    }

    private Vector3 GetRandomPosition()
    {
        int negativeDevider = -2;
        int devider = 2;

        float minRandomX = _platform.transform.localScale.x / negativeDevider;
        float maxRandomX = _platform.transform.localScale.x / devider;
        float minRandomZ = _platform.transform.localScale.z / negativeDevider;
        float maxRandomZ = _platform.transform.localScale.z / devider;

        float X = Random.Range(minRandomX, maxRandomX + 1);
        float Y = _hightPoint;
        float Z = Random.Range(minRandomZ, maxRandomZ + 1);

        return new Vector3(X, Y, Z);
    }
}
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
    private Vector3 _position;
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
            cube.Deathed += OnDeath;

            yield return _wait;
        }
    }

    private void OnDeath(Cube cube)
    {
        _pool.ReleaseObject(cube);
        cube.Deathed -= OnDeath;
    }

    private Vector3 GetRandomPosition()
    {
        int negativeDevider = -2;
        int devider = 2;

        float minRandomX = _platform.transform.localScale.x / negativeDevider;
        float maxRandomX = _platform.transform.localScale.x / devider;
        float minRandomZ = _platform.transform.localScale.z / negativeDevider;
        float maxRandomZ = _platform.transform.localScale.z / devider;

        float positionX = Random.Range(minRandomX, maxRandomX);
        float positionY = _hightPoint;
        float positionZ = Random.Range(minRandomZ, maxRandomZ);

        return new Vector3(positionX, positionY, positionZ);
    }
}
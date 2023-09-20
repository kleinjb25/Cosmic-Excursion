using UnityEngine;

public class AsteroidGenerator : MonoBehaviour
{
    public GameObject asteroidSmall;
    public GameObject asteroidMedium;
    public GameObject asteroidLarge;
    public GameObject targetPrefab;
    public int numAsteroids = 10;
    public Vector3 areaSize = new Vector3(10f, 10f, 10f);

    private void Start()
    {
        GenerateAsteroids();
    }

    private void GenerateAsteroids()
    {
        float asteroidSize = Random.Range(1,3);
        for (int i = 0; i < numAsteroids; i++)
        {
            Vector3 position = GetRandomPositionWithinArea();
            Quaternion rotation = Quaternion.Euler(Random.Range(-50f, 50f), Random.Range(-50f, 50f), Random.Range(-50f, 50f));
            Vector3 scale = new Vector3(Random.Range(1f, 5f), Random.Range(1f, 5f), Random.Range(1f, 5f));

            if (IsPositionValid(position, scale))
            {
                GameObject asteroid;
                if (asteroidSize == 1)
                {
                    asteroid = Instantiate(asteroidSmall, position, rotation);
                }
                else if (asteroidSize == 2)
                {
                      asteroid = Instantiate(asteroidMedium, position, rotation);
                }
                else
                {
                    asteroid = Instantiate(asteroidLarge, position, rotation);
                }
                
                asteroid.transform.localScale = scale;

                // Place target on the asteroid surface
                GameObject target = Instantiate(targetPrefab, GetRandomPointOnAsteroidSurface(asteroid), Quaternion.identity);
                target.transform.SetParent(asteroid.transform);
            }
            else
            {
                i--;
            }
        }
    }

    private Vector3 GetRandomPositionWithinArea()
    {
        float x = Random.Range(-areaSize.x / 2f, areaSize.x / 2f);
        float y = Random.Range(-areaSize.y / 2f, areaSize.y / 2f);
        float z = Random.Range(-areaSize.z / 2f, areaSize.z / 2f);
        return new Vector3(x, y, z);
    }

    private bool IsPositionValid(Vector3 position, Vector3 scale)
    {
        Collider[] colliders = Physics.OverlapBox(position, scale / 2f);
        return colliders.Length == 0;
    }

    private Vector3 GetRandomPointOnAsteroidSurface(GameObject asteroid)
    {
        Vector3 randomPoint = Random.onUnitSphere * asteroid.transform.localScale.x / 2f;
        return asteroid.transform.position + randomPoint;
    }
}

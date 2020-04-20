using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class SpawnManager : MonoBehaviour
{
    public GameObject virusPrefab;
    public GameObject toiletPaperPrefab;
    public GameObject goldenToiletPaperPrefab;
    public GameObject maskPrefab;
    public GameObject handSanitizerPrefab;
    public GameObject lysolPrefab;
    public GameManager gameManager; 
    private GameObject playerObj;
    private List<GameObject> activeItems;
    private float spawnRateVirus;
    private float spawnRateToiletPaper;
    private float spawnRateSanitizer;
    private float spawnRateLysol;
    private float spawnRateMask;
    private int maxVirusesOnMap;
    private int maxMasksOnMap;
    private int maxToiletPaperOnMap;
    private int minDistanceBetweenToiletPapers;
    private int minDistanceBetweenMasks;
    private float spawnRateGoldenToiletPaper;
    private int maxNumberOfLocationChecks;

    void Start()
    {
        activeItems = new List<GameObject>();
        minDistanceBetweenToiletPapers = 5;
        maxNumberOfLocationChecks = 250;
        SetSpawnRates();
        SetMaxOnMapLimits();
    }

    public void EndSpawners()
    {
        StopAllCoroutines();
    }

    public void RemoveObjectFromList(GameObject destroyedObject)
    {
            activeItems.Remove(destroyedObject);
    }

    private void SetSpawnRates()
    {
        spawnRateVirus = .3f;
        spawnRateToiletPaper = 1f;
        spawnRateSanitizer = 15f;
        spawnRateMask = 10f;
        spawnRateLysol = 35f;
    }

    private void SetMaxOnMapLimits()
    {
        maxVirusesOnMap = 200;
        maxToiletPaperOnMap = 15;
        maxMasksOnMap = 2;
    }

    private void ActivateSpawners()
    {
        StartCoroutine(VirusSpawner());
        StartCoroutine(ToiletPaperSpawner());
        StartCoroutine(HandSanitizerSpawner());
        StartCoroutine(LysolSpawner());
        StartCoroutine(MaskSpawner());
        StartCoroutine(GoldenToiletPaperSpawner());

        //TODO: Consider sending the objects as parameters to an Ienumerator which uses a switch statement to run the correct spawning coroutine.
    }

 #region Coroutines

    IEnumerator VirusSpawner()
    {
        while (gameManager.gameActive == true)
        {
            if (GetQuantityOfItemsOnMap(virusPrefab) < maxVirusesOnMap)
            {
                yield return new WaitForSeconds(spawnRateVirus);
                spawnRateVirus *= .9965f;
                activeItems.Add(SpawnObject(virusPrefab));
            }
            yield return null;
        }
    }
 
    IEnumerator ToiletPaperSpawner()
    {
        while (gameManager.gameActive == true)
        {
            if (GetQuantityOfItemsOnMap(toiletPaperPrefab) < maxToiletPaperOnMap)
            {
                yield return new WaitForSeconds(spawnRateToiletPaper);
                activeItems.Add(SpawnObject(toiletPaperPrefab));
            }
            yield return null;
        }
    }

    IEnumerator HandSanitizerSpawner()
    {
        while (gameManager.gameActive == true)
        {
            yield return new WaitForSeconds(spawnRateSanitizer);
            activeItems.Add(SpawnObject(handSanitizerPrefab));
        }
    }

    IEnumerator LysolSpawner()
    {
        while (gameManager.gameActive == true)
        {
            yield return new WaitForSeconds(spawnRateLysol);
            activeItems.Add(SpawnObject(lysolPrefab));
        }
    }

    IEnumerator MaskSpawner()
    {
        IEnumerable<GameObject> spawnedMasks;
        while (gameManager.gameActive == true)
        {
            if (GetQuantityOfItemsOnMap(maskPrefab) < maxMasksOnMap)
                {
                    yield return new WaitForSeconds(spawnRateMask); 
                    activeItems.Add(SpawnObject(maskPrefab));
                }
            yield return null;
        }
    }

    IEnumerator GoldenToiletPaperSpawner()
    {
        while (gameManager.gameActive == true)
        {
            spawnRateGoldenToiletPaper = Random.Range(5, 20);
            yield return new WaitForSeconds(spawnRateGoldenToiletPaper);
            activeItems.Add(SpawnObject(goldenToiletPaperPrefab));
        }
    }

    #endregion

    private GameObject SpawnObject(GameObject objectToSpawn)
    {
        return Instantiate(objectToSpawn, GenerateSpawnLocation(objectToSpawn), new Quaternion(0f, 0f, 0f, 0f));
    }

#region SpawnLocations
    private Vector3 GenerateSpawnLocation(GameObject objectToSpawn)
    {
        
        Vector3 location;
        bool acceptableLocation = false;
        int i = 0;

        do
        {
            if (objectToSpawn.name == "Virus")
                { location = GetRandomVirusLocation(); }
            else
                { location = GetRandomLocation(); }
            // Debug.Log($"Testing location {i}");
            acceptableLocation = DoesSpawnMeetConditions(location, objectToSpawn);
            ++i;
        } while (acceptableLocation == false && i < maxNumberOfLocationChecks);

        Debug.Log($"Tested {i} locations before spawning {objectToSpawn.name}");
        return location;
    }
     

    private Vector3 GetRandomVirusLocation()
    {
        int wallToSpawnAgainst = Random.Range(1, 4);

        switch (wallToSpawnAgainst)
        {
            case 1:
                return new Vector3(-Map.MAP_SIZE_X+1, Random.Range(-Map.MAP_SIZE_Y, Map.MAP_SIZE_Y));
            case 2:
                return new Vector3(Map.MAP_SIZE_X-1, Random.Range(-Map.MAP_SIZE_Y, Map.MAP_SIZE_Y));
            case 3:
                return new Vector3(Random.Range(-Map.MAP_SIZE_X, Map.MAP_SIZE_X), -Map.MAP_SIZE_Y);
            case 4:
                return new Vector3(Random.Range(-Map.MAP_SIZE_X, Map.MAP_SIZE_X), Map.MAP_SIZE_Y);
            default:
                throw new System.Exception();
        }    
    }

    private Vector3 GetRandomLocation()
    {
        return new Vector3(Random.Range(-Map.MAP_SIZE_X, Map.MAP_SIZE_X), Random.Range(-Map.MAP_SIZE_Y, Map.MAP_SIZE_Y));
    }

    /// <summary>
    /// Checks whether the location chosen is too close to the player.
    /// </summary>
    /// <param name="locationToTest"></param>
    /// <param name="objectToSpawn"></param>
    /// <returns></returns>
    /// 
    private bool DoesSpawnMeetConditions(Vector3 locationToTest, GameObject objectToSpawn)
    {
        float distanceFromPlayer = Vector3.Distance(locationToTest, playerObj.transform.position);

        switch (objectToSpawn.name)
        {
            case ("Virus"):
                {
                    if (distanceFromPlayer > 5)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            // TODO: Add additional rules for different object types. Ensure that two TPs, for example, cannot spawn on top of one another.
            case ("Toilet Paper"):
            {
                return isValidToiletPaperSpawn(objectToSpawn);
            }
            
            default:
                {
                    if (distanceFromPlayer > 5)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
        }
    }
        #endregion


    private int GetQuantityOfItemsOnMap(GameObject prefab)
    {
        return activeItems.Where(items => items.name == (prefab.name + "(Clone)")).Count();
    }

    public void RemoveItemFromActiveItemsList(GameObject item)
    {
        bool wasItemRemoved = activeItems.Remove(item);
        Debug.Log($"{item.name} was removed from items list: {wasItemRemoved}");
    }



    private bool isValidToiletPaperSpawn(GameObject newToiletPaper)
    {
        List<GameObject> activeToiletPapers = new List<GameObject>();
        activeToiletPapers.AddRange(activeItems.Where(item => item.name == "Toilet Paper(Clone)"));
        activeToiletPapers.AddRange(activeItems.Where(item => item.name == "Golden Toilet Paper(Clone)"));

        foreach (GameObject toiletPaper in activeToiletPapers)
        {
            if (Vector3.Distance(toiletPaper.transform.position, newToiletPaper.transform.position) < minDistanceBetweenToiletPapers)
            {
                return false;
            }
        }
        return true;
    }
    public void OnPlayerCreated(GameObject source, System.EventArgs e)
    {
        playerObj = source;
        ActivateSpawners();

    }



}

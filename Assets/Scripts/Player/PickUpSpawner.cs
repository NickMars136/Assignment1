using UnityEngine;

public class PickupSpawner : MonoBehaviour
{
    #region Spawn Settings
    [Header("Collectibles")]
    [SerializeField] private SimplePickup[] collectibles;

    [Header("Spawn Locations")]
    [SerializeField] private Transform[] spawnLocations;
    #endregion

    // Start is called once before the first execution of Update
    void Start()
    {
        SpawnCollectibles();
    }

    // Spawns a random collectible at every spawn location
    void SpawnCollectibles()
    {
        // Check if collectibles exist
        if (collectibles == null || collectibles.Length == 0)
        {
            Debug.LogWarning("No collectibles assigned!");
            return;
        }

        // Check if spawn locations exist
        if (spawnLocations == null || spawnLocations.Length == 0)
        {
            Debug.LogWarning("No spawn locations assigned!");
            return;
        }

        // Loop through every spawn location
        for (int spawnIndex = 0; spawnIndex < spawnLocations.Length; spawnIndex++)
        {
            // Get the current spawn location
            Transform currentSpawnLocation = spawnLocations[spawnIndex];

            // Check if spawn location exists
            if (currentSpawnLocation == null)
            {
                Debug.LogWarning("Spawn location " + spawnIndex + " is missing!");
                continue;
            }
            // Randomly select a collectible
            int randomCollectibleIndex = Random.Range(0, collectibles.Length);

            SimplePickup selectedPickup = collectibles[randomCollectibleIndex];

            // Check if selected collectible exists
            if (selectedPickup == null)
            {
                Debug.LogWarning("Selected collectible is missing!");
                continue;
            }

            // Spawn collectible at the current location
            Instantiate(
                selectedPickup, currentSpawnLocation.position, currentSpawnLocation.rotation);

            Debug.Log("Spawned: " + selectedPickup.name);
        }
    }
}
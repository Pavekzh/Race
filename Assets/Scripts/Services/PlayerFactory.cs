using UnityEngine;

public class PlayerFactory:MonoBehaviour
{
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private Vector3 spawnPosition;

    public void CreatePlayer(Fusion.NetworkRunner network,WorldGenerator worldGenerator, int startLane)
    {
        float xPos = worldGenerator.LanesXs[startLane];
        spawnPosition = new Vector3(xPos, spawnPosition.y, spawnPosition.z);
        network.Spawn(playerPrefab, spawnPosition);
    }
}
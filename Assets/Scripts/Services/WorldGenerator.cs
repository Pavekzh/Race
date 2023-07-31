using Fusion;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WorldGenerator:NetworkBehaviour
{
    [System.Serializable]
    struct SpawnObject
    {
        public GameObject Prefab;
        [Range(0,1)]
        public float ProportionalProbability;
        public bool IsObstacle;
    }

    [Header("Sections")]
    [SerializeField] private GameObject finishSection;
    [SerializeField] private GameObject[] sections;
    [SerializeField] private int levelLength = 30;
    [SerializeField] private Vector3 startPoint = Vector3.zero;
    [Header("Spawn objects")]
    [SerializeField] SpawnObject[] spawnObjects;
    [Range(0,1)]
    [SerializeField] private float emptyPlaceProbability = 0.5f;
    [SerializeField] private float safeZoneLength = 20;
    [SerializeField] private int placesInRow = 4;
    [SerializeField] private float placeWidth = 3;
    [SerializeField] private float rowLength = 5;
    [SerializeField] private int maxObstaclesInRow = 2;

    [Networked] private int RandomSeed { get; set; }

    public float[] LanesXs { get; private set; }

    private float[] laneXs;

    private Vector3 firstPlace;
    private float levelRealLength;

    public event System.Action LevelGenerated;

    private Transform sectionsParent;
    private Transform obstacleParent;

    public void Init(Transform sectionsParent, Transform obstacleParent)
    {
        this.sectionsParent = sectionsParent;
        this.obstacleParent = obstacleParent;

        LanesXs = new float[placesInRow];
        laneXs = new float[placesInRow];

        float firstPlaceX = startPoint.x - (placeWidth * placesInRow) / 2;
        float firstPlaceZ = startPoint.z + safeZoneLength;
        firstPlace = new Vector3(firstPlaceX, startPoint.y, firstPlaceZ);

        for (int i = 0; i < placesInRow; i++)
        {            
            LanesXs[i] = (firstPlace.x + placeWidth * i) + (placeWidth / 2);
            laneXs[i] = firstPlace.x + placeWidth * i;
        }

    }

    public override void Spawned()
    {        
        if (HasStateAuthority)
            RandomSeed = Random.Range(int.MinValue, int.MaxValue);

        Random.InitState(RandomSeed);

        BuildSections();
        BuildObjects();
        LevelGenerated?.Invoke();
    }

    private void BuildSections()
    {
        int[] selectedSections = SelectSections();

        Vector3 position = startPoint;
        for (int i = 0; i < selectedSections.Length; i++)
        {
            GameObject section = sections[selectedSections[i]];
            BuildSection(section, ref position);
        }        
        levelRealLength = position.z - startPoint.z;
        
        BuildSection(finishSection, ref position);
    }

    private int[] SelectSections()
    {
        int[] result = new int[levelLength];
        for (int i = 0; i < levelLength; i++)
            result[i] = Random.Range(0, sections.Length);

        return result;
    }

    private void BuildSection(GameObject section, ref Vector3 position)
    {
        GameObject instance = Instantiate(section, position, Quaternion.identity, sectionsParent);
        Collider sectionCollider = instance.GetComponent<Collider>();

        if (sectionCollider == null)
            Debug.LogError("Section GameObject must have Collider component");

        float length = sectionCollider.bounds.size.z;
        position.z += length;
    }


    private void BuildObjects()
    {
        var LevelObjects = FormLevelObjects();

        foreach(var obj in LevelObjects)
        {
            float posX = firstPlace.x + (obj.column * placeWidth);
            float posZ = firstPlace.z + (obj.row * rowLength);
            Vector3 pos = new Vector3(posX, firstPlace.y, posZ);

            GameObject prefab = spawnObjects[obj.index].Prefab;
            Instantiate(prefab, pos, Quaternion.identity, obstacleParent);
        }
    }

    private List<(int row,int column, int index)> FormLevelObjects()
    {
        var withObstacles = spawnObjects.Select((value, index) => (value, index)).ToArray();
        var withoutObstacles = spawnObjects.Select((value, index) => (value, index)).Where(pair => !pair.value.IsObstacle).ToArray();

        int rowsCount = (int)(levelRealLength / rowLength);

        List<(int row, int column, int index)> levelObjects = new List<(int column, int row, int index)>();


        for (int i = 0; i < rowsCount; i++)
        {
            int obstaclesCount = 0;

            for (int j = 0; j < placesInRow; j++)
            {
                int selected;

                if (obstaclesCount < maxObstaclesInRow)
                {
                    selected = SelectSpawnObject(withObstacles);


                    if (selected != -1)
                    {
                        levelObjects.Add(new(i, j, selected));

                        if (spawnObjects[selected].IsObstacle)
                            obstaclesCount++;
                    }
                }
                else
                {
                    selected = SelectSpawnObject(withoutObstacles);

                    if (selected != -1)
                        levelObjects.Add(new(i, j, selected));
                }
            }

        }

        return levelObjects;
    }

    private int SelectSpawnObject((SpawnObject value,int index)[] array)
    {
        if (Random.Range(0,(float)1) > emptyPlaceProbability)
        {
            float maxRandom = 0;
            foreach (var pair in array)
                maxRandom += pair.value.ProportionalProbability;

            float random = Random.Range(0, maxRandom);

            float rangeValue = 0;
            foreach(var pair in array)
            {
                rangeValue += pair.value.ProportionalProbability;
                if (random <= rangeValue)
                    return pair.index;

            }
   
        }

        return -1;

    }

}
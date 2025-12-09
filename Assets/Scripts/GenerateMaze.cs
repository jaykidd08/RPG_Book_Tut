using UnityEngine;

public class GenerateMaze : MonoBehaviour
{
    const int N = 1, S = 2, E = 3, W = 4;
    int[,] grid;

    [SerializeField]
    [Range(5, 100)]
    int width, heigth;

    [SerializeField]
    [Range(5, 100)]
    int wallSize;

    public GameObject verticalWall, horizontalWall;
    GameObject[,] gridObjectsH, gridObjectsV;
    GameObject[] allObjectsInScene;
    float wallHeight;

    public GameObject player, objectToCollect, npc;

    void Start()
    {

        Init();
        GenerateMazeBinary();
        DisplayGrid();
        //AddPlayer();
        AddObjects();
        AddNPCs();

        float xOffset = -(width * wallSize) / 2; float zOffset = -(heigth * wallSize) / 2;
        GameObject.Find("Player").transform.position = new Vector3(xOffset + wallSize * 4, 0.52f, zOffset + wallSize * 4);

    }
    private void Init()
    {

        heigth = width;
        wallHeight = 4;
        verticalWall.transform.localScale = new Vector3(.1f, wallHeight, wallSize);
        horizontalWall.transform.localScale = new Vector3(wallSize, wallHeight, .1f);

        grid = new int[width, heigth];
        gridObjectsV = new GameObject[width + 1, heigth + 1];
        gridObjectsH = new GameObject[width + 1, heigth + 1];
        drawFullgrid();

        GameObject.Find("ground").transform.localScale = new Vector3((width + 1) * wallSize, 1, (heigth + 1) * wallSize);
        GameObject.Find("ceiling").transform.localScale = new Vector3((width + 1) * wallSize, 1, (heigth + 1) * wallSize);
        GameObject.Find("ceiling").transform.position = new Vector3(GameObject.Find("ceiling").transform.position.x, wallSize - 1, GameObject.Find("ceiling").transform.position.z);

        void drawFullgrid()
        {
            for (int i = 0; i <= heigth; i++)
            {
                for (int j = 0; j <= width; j++) 
                {
                    if (i < heigth)
                    {

                        float vWallSize = verticalWall.transform.localScale.z;
                        float xOffset, zOffset;
                        //so that maze centered around (0,0)
                        xOffset = -(width * vWallSize) / 2;
                        zOffset = -(heigth * vWallSize) / 2;
                        gridObjectsV[j, i] = (GameObject)(Instantiate(verticalWall, new Vector3(-vWallSize / 2 + j * vWallSize + xOffset, wallSize/2, i * vWallSize + zOffset), Quaternion.identity));
                        gridObjectsV[j, i].SetActive(true);
                        gridObjectsV[j, i].name = "v" + i + j;
                        gridObjectsV[j, i].tag = "wall";

                    }

                    if (j < width)
                    {
                        float hWallSize = horizontalWall.transform.localScale.x;
                        float xOffset, zOffset;
                        xOffset = -(width * hWallSize) / 2;
                        zOffset = -(heigth * hWallSize) / 2;
                        gridObjectsH[j, i] = (GameObject)(Instantiate(horizontalWall, new Vector3(j * hWallSize + xOffset, wallSize / 2, -(hWallSize / 2) + i * hWallSize + zOffset), Quaternion.identity));
                        //PF
                        gridObjectsH[j, i].SetActive(true);
                        gridObjectsH[j, i].name = "h" + i + j;
                        gridObjectsH[j, i].tag = "wall";
                    }
                }
            }

        }
    }

    void GenerateMazeBinary()
    {

        for (int row = 0; row < heigth; row++)
        {
            for (int cell = 0; cell < width; cell++)
            {
                float randomNumber = Random.Range(0, 100);
                int carvingDirection;

                if (randomNumber > 30)
                {
                    carvingDirection = N;
                }
                else
                {
                    carvingDirection = E;
                }

                if (cell == width - 1)
                {
                    if (row < heigth - 1)
                    {
                        carvingDirection = N;
                    }
                    else
                    {
                        carvingDirection = W;
                    }

                }
                else if (row == heigth - 1)
                {
                    if (cell < width - 1)
                    {
                        carvingDirection = E;
                    }
                    else
                    {
                        carvingDirection = -1;
                    }
                }

                grid[cell, row] = carvingDirection;
            }
        }

    }

    void DisplayGrid()
    {

        for (int row = 0; row < heigth; row++)
        {
            for (int cell = 0; cell < width; cell++)
            {
                //Changed from Deprecated active
                if (grid[cell, row] == N) gridObjectsH[cell, row + 1].SetActive(false);
                if (grid[cell, row] == E) gridObjectsV[cell + 1, row].SetActive(false);
            }
        }

    }

    void AddPlayer()
    {

        float xOffset = -(width * wallSize) / 2;
        float zOffset = -(heigth * wallSize) / 2;
        GameObject p = (GameObject)(Instantiate(player, new Vector3(xOffset, 1.0f, zOffset), Quaternion.identity));
        p.name = "player";

    }

    void AddObjects()
    {

        float xOffset = -(width * wallSize) / 2;
        float zOffset = -(heigth * wallSize) / 2;
        GameObject object1 = (GameObject)(Instantiate(objectToCollect, new Vector3(xOffset + wallSize * 1, 1.5f, zOffset + wallSize), Quaternion.identity));
        object1.GetComponent<ObjectToBeCollected>().type = Item.ItemType.GOLD; GameObject object2 = (GameObject)(Instantiate(objectToCollect, new Vector3(xOffset + wallSize * 2, 1.5f, zOffset + wallSize * 2), Quaternion.identity));
        object2.GetComponent<ObjectToBeCollected>().type = Item.ItemType.RED_DIAMOND;
        GameObject object3 = (GameObject)(Instantiate(objectToCollect, new Vector3(xOffset + wallSize * 3, 1.5f, zOffset + wallSize * 3), Quaternion.identity));
        object3.GetComponent<ObjectToBeCollected>().type = Item.ItemType.BLUE_DIAMOND;

    }

    void AddNPCs()
    {

        float xOffset = -(width * wallSize) / 2;
        float zOffset = -(heigth * wallSize) / 2;
        GameObject npc1 = (GameObject)(Instantiate(npc, new Vector3(xOffset + wallSize * 1, 0.52f, zOffset + wallSize * 2), Quaternion.identity));
        //npc1.GetComponent<ControlNPCGuard>().guardType = ControlNPCGuard.GUARD_TYPE.IDLE;
        npc1.GetComponent<ControlNPCGuard>().guardType = ControlNPCGuard.GUARD_TYPE.CHASER;
        npc1.GetComponent<ControlNPCGuard>().player = GameObject.Find("player");
        npc1.GetComponent<ControlNPCGuard>().player = GameObject.Find("Player");
    }
}

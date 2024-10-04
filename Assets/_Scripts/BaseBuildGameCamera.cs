using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BaseBuildGameCamera : MonoBehaviour
{
    public float panSpeed = 20f;
    public float panBorderThickness = 10f;
    public float zoomSpeed = 2f;
    public bool isOrthagraphic = true;
    public float minZoom = 5f;
    public float maxZoom = 10f;
    [SerializeField] float panShrink = 2f;
    Vector3 mousePosition;
    Quaternion mouseTranslation;

    [Header("Placement Info")]
    Camera cam;
    int currentBuildID;
    public GameObject prefab;
    public GameObject[] buildingPrefabs;
    [SerializeField] LayerMask placementLayer;
    Vector3 lastPosition;
    public GameObject cursorMarkerPrefab; 
    private Transform cursorInstance;
    private Transform cursorMarkerInstance;
    public Grid grid;
    

    [Header("UI Debug")]
    public GameObject gameUI;
    public TextMeshProUGUI resourceText;
    public TextMeshProUGUI buildText;
    public TextMeshProUGUI bulidPrefabText;
    public Button upgradeButton;
    public Color canbuildHere, cantbuildHere;

    [Header("Gameplay Debug")]
    bool isPaused;
    float timer = 5;
    float clock;

    int buildCount;
    int buildLimit = 10;
    public int resources;
    int resourceLimit = 999;
    bool canbuild;
    bool inBuildMode;
    int buildLevelCost = 150;
    int buildLevel = 1;
    int multiplier = 1;

    void Start(){
        cam = GetComponent<Camera>();
        if (cam == null)
        {
            cam = Camera.main; // Default to main camera if none assigned
        }
        cam.orthographic = isOrthagraphic;
        GameObject newMouse = Instantiate(prefab, mousePosition, Quaternion.identity) as GameObject;
        cursorInstance = newMouse.transform;
        //GameObject newIndicatior = Instantiate(cursorMarkerPrefab, mousePosition, Quaternion.identity) as GameObject;
        cursorMarkerInstance = cursorMarkerPrefab.transform; //newIndicatior.transform;

    }
    void AlignToGrid(){
        Vector3 newMousePosition = mousePosition;
        if (cursorInstance != null)
        {
            cursorInstance.position = newMousePosition;
        }
        Vector3Int gridPosition = grid.WorldToCell(newMousePosition);
        if (cursorMarkerInstance != null)
        {
            cursorMarkerInstance.position = grid.CellToWorld(gridPosition) + grid.cellSize / 2;
        }
        Debug.Log(gridPosition);
    }
    public Vector3 GetSelectedMapPosition(){
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = cam.nearClipPlane;
        Ray ray = cam.ScreenPointToRay(mousePos);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 300, placementLayer))
        {
            lastPosition = hit.point;
        }
        return lastPosition;
    
    }
    void Update()
    {
        mousePosition = GetSelectedMapPosition();
        AlignToGrid();
        Vector3 pos = transform.position;
        if(Input.GetButtonDown("Cancel")){
            isPaused = !isPaused;
            Time.timeScale = isPaused ? 0 : 1;
        }
        if(Input.GetKey("w") || Input.mousePosition.y  >= Screen.height - panBorderThickness){
            pos.z += panSpeed * Time.deltaTime;
        }
        if(Input.GetKey("s") || Input.mousePosition.y <= panBorderThickness){
            pos.z -= panSpeed * Time.deltaTime;
        }
        if(Input.GetKey("d") || Input.mousePosition.x  >= Screen.width - panBorderThickness){
            pos.x += panSpeed * Time.deltaTime;
        }
        if(Input.GetKey("a") || Input.mousePosition.x  <= panBorderThickness){
            pos.x -= panSpeed * Time.deltaTime;
        }
        if(Input.GetKeyDown(KeyCode.Tab)){
            inBuildMode = !inBuildMode;
        }
    
        if(Input.GetKeyDown("r")){
            pos = Vector3.Lerp(pos, new Vector3(0, transform.position.y, 0), 0.01f * Time.deltaTime);
        }

        if(Input.GetKeyDown("q")){
            currentBuildID--;
            if(currentBuildID < 0){
                currentBuildID = buildingPrefabs.Length - 1;
            }
            SwapBuildings(currentBuildID);
        }

        if(Input.GetKeyDown("e")){
            currentBuildID++;
            if(currentBuildID > buildingPrefabs.Length - 1){
                currentBuildID = 0;
            }
            SwapBuildings(currentBuildID);
        }
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0.0f)
        {
            cam.orthographicSize -= scroll * zoomSpeed;
            panSpeed -= scroll * panShrink;
            if(isOrthagraphic){
                cam.orthographicSize = Mathf.Clamp(cam.orthographicSize, minZoom, maxZoom);
                panSpeed = Mathf.Clamp(panSpeed, minZoom, maxZoom);
            } else {
                cam.fieldOfView = Mathf.Clamp(cam.fieldOfView, 30f, 80f);
                panSpeed = Mathf.Clamp(panSpeed, minZoom, maxZoom);
            }
        }
        transform.position = pos;
        
        UpdateUI();

        if (Input.GetMouseButtonDown(0)){
            //Build
        }
        if (Input.GetMouseButtonDown(1)){
            //UnBuild
        }
    }


    void Build(GameObject prefab, Vector3 pos, Quaternion rot){
        if(canbuild){
            GameObject newBuild = Instantiate(prefab, pos, rot) as GameObject;
            newBuild.GetComponentInChildren<BoxCollider>().enabled = true;
            Building newBuilding = newBuild.transform.GetComponentInChildren<Building>();
            newBuilding.canSpawn = true;
            newBuilding.autoSpawn = true;
            resources -= newBuilding.cost;
            multiplier++;
            buildCount++;
        }
    }
    void UnBuild(GameObject prefab){
        Building targetBuilding = prefab.transform.GetComponent<Building>();
        resources += targetBuilding.value;
        Destroy(targetBuilding);
        buildCount--;
    }
    void GenerateInWorldCursor(GameObject prefab, Vector3 pos, Quaternion rot){
        if(canbuild && cursorInstance == null){
            GameObject newPrefabGhost = Instantiate(prefab, pos, rot) as GameObject;
            cursorInstance = newPrefabGhost.transform;
        }
        if(canbuild && cursorMarkerInstance == null){
            GameObject newMarkerGhost = Instantiate(cursorMarkerPrefab, pos, rot) as GameObject;
            cursorMarkerInstance = newMarkerGhost.transform;
        }
    }
    void DestroyInWorldCursor(){
        if(cursorInstance != null) {
            Destroy(cursorInstance.gameObject); 
            cursorInstance = null;
        }
        if(cursorMarkerInstance != null){
            Destroy(cursorMarkerInstance.gameObject);
            cursorMarkerInstance = null;
        }
    }
    void UpdateInWorldCursor(Vector3 pos, Quaternion rot){
        if(cursorInstance != null){
            cursorInstance.position = pos;
            cursorInstance.rotation = rot;
        }
        if(cursorMarkerInstance != null){
            cursorMarkerInstance.position = pos;
            cursorMarkerInstance.rotation = rot;
        }
    }
    void UpdateInWorldCursorPrefab(GameObject gObj){
        GenerateInWorldCursor(gObj, cursorInstance.position, cursorInstance.rotation);
    }
    void SwapBuildings(int id){
        DestroyInWorldCursor();
        cursorInstance = null;
        prefab = buildingPrefabs[currentBuildID];
        Debug.Log("Swap");
    }
    void UpdateUI(){
        if(buildText != null) buildText.text = $"Build Level: {buildLevel}  ({buildCount} / {buildLimit})";
        if(resourceText != null) resourceText.text = $"Local Resources: $ {resources}";
    }
    public void IncresceBuildLvl(){
        if(resources > buildLevelCost){
            resources -= buildLevelCost;
            buildLevel++;
            buildLimit += 10;
            buildLevelCost += 50 * multiplier;
        }
    }
}

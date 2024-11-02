using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PanningGameCamera : MonoBehaviour
{
    public float panSpeed = 20f;
    public float panBorderThickness = 10f;
    public float zoomSpeed = 2f;
    public bool isOrthagraphic = true;
    public float minZoom = 5f;
    public float maxZoom = 10f;
    public GameObject prefab;
    public GameObject[] buildingPrefabs;
    int currentBuildID;
    Camera cam;
    [SerializeField] float panShrink = 2f;
    Vector3 mousePosition;
    Quaternion mouseTranslation;

    [Header("UI Debug")]
    public GameObject gameUI;
    public TextMeshProUGUI resourceText;
    public TextMeshProUGUI buildText;
    public TextMeshProUGUI bulidPrefabText;
    public Button upgradeButton;
    public Color canbuildHere, cantbuildHere;

    [Header("Gameplay Debug")]
    int buildCount;
    int buildLimit = 10;
    public int resources;
    int resourceLimit = 999;
    bool canbuild;
    bool inBuildMode;
    float timer = 5;
    float clock;
    int buildLevelCost = 150;
    int buildLevel = 1;
    int maxBuildLevel = 4;
    int multiplier = 1;
    public GameObject cursorMarkerPrefab; 
    private Transform cursorInstance;
    private Transform cursorMarkerInstance;
    public int defaultResource = 3;

    void Start(){
        cam = GetComponent<Camera>();
        if (cam == null)
        {
            cam = Camera.main; // Default to main camera if none assigned
        }
        cam.orthographic = isOrthagraphic;
        transform.position = new Vector3(0, transform.position.y, 0);
        if(gameUI != null) gameUI.SetActive(inBuildMode);

    }
    void Update()
    {
        
        if(timer < 0){
            if(resources < resourceLimit)resources += defaultResource * multiplier;
            timer = 5;
        } else {
            timer -=  Time.deltaTime;
        }
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            mousePosition = hit.point;
            mouseTranslation = Quaternion.LookRotation(hit.normal) * Quaternion.Euler(90, 0, 0);
        }
        Vector3 pos = transform.position;
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
            if(gameUI != null) gameUI.SetActive(inBuildMode);
        }
    
        if(Input.GetKeyDown("r")){
            pos = Vector3.Lerp(pos, new Vector3(0, transform.position.y, 0), 0.1f * Time.deltaTime);
        }
        if(Input.GetKeyDown("q")){
            currentBuildID--;
            if(currentBuildID < 0){
                currentBuildID = buildLevel - 1;
            }
            SwapBuildings(currentBuildID);
        }
        if(Input.GetKeyDown("e")){
            currentBuildID++;
            if(currentBuildID > buildLevel - 1){
                currentBuildID = 0;
            }
            SwapBuildings(currentBuildID);
        }
        transform.position = pos;
        
        UpdateUI();

        if (Input.GetMouseButtonDown(0)){

            if (Physics.Raycast(ray, out hit))
            {
                Build(prefab, mousePosition, mouseTranslation);
            }
        }
        if (Input.GetMouseButtonDown(1)){

            if (Physics.Raycast(ray, out hit))
            {
                GameObject hitObject = hit.collider.gameObject;
                if (hitObject.GetComponent<Building>() != null) UnBuild(hitObject);
            }
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
        canbuild = buildCount < buildLimit && inBuildMode && resources > 0;
        bool canUpgrade = resources > buildLevelCost ? true : false;
        if(canUpgrade)upgradeButton.GetComponentInChildren<TextMeshProUGUI>().text = $"Updgrade Level {buildLevelCost}"; else upgradeButton.GetComponentInChildren<TextMeshProUGUI>().text = $"Next Level {buildLevelCost}";
        if(cursorMarkerInstance != null){
            cursorMarkerInstance.GetComponentInChildren<MeshRenderer>().material.color = canbuild ? canbuildHere : cantbuildHere; 
        }
        if(canbuild){
            GenerateInWorldCursor(prefab, mousePosition, mouseTranslation);
            UpdateInWorldCursor(mousePosition, mouseTranslation);
        } else {
            DestroyInWorldCursor();
            return;
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
            if(buildLevel < maxBuildLevel) buildLevel++;
            buildLimit += 10;
            buildLevelCost += 5 * multiplier;
        }
    }
}

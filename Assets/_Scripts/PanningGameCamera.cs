using UnityEngine;
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

    [Header("Gameplay Debug")]
    int buildCount;
    int buildLimit = 10;
    public int resources;
    int resourceLimit = 999;
    bool canbuild;
    bool inBuildMode;
    float timer = 5;
    int clock;
    //public GameObject cursorPrefab; 
    private Transform cursorInstance;

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
            resources += 3;
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
        canbuild = buildCount < buildLimit && inBuildMode;
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
            resources -= newBuilding.cost;
            
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
    }
    void DestroyInWorldCursor(){
        if(cursorInstance != null) {
            Destroy(cursorInstance.gameObject); 
            cursorInstance = null;
        }
    }
    void UpdateInWorldCursor(Vector3 pos, Quaternion rot){
        if(cursorInstance != null){
            cursorInstance.position = pos;
            cursorInstance.rotation = rot;
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
        if(buildText != null) buildText.text = $"Buildings {buildCount} / {buildLimit}";
        if(resourceText != null) resourceText.text = $"Buildings {resources} / {resourceLimit}";
    }
}

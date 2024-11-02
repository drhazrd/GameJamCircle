using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
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

    [Header("Placement Info")]
    Camera cam;
    [SerializeField] public ObjectDatabaseSO database;
    int currentObjectID;
    int currentBuildID;
    public GameObject prefab;
    public GameObject[] buildingPrefabs;
    [SerializeField] LayerMask placementLayer;
    Vector3 lastPosition;
    public GameObject cursorMarkerPrefab; 
    private Transform cursorInstance;
    private Transform cursorMarkerInstance;
    public GameObject gridHolder;
    public GameObject gameUI;
    public Grid grid;
    

    [Header("UI Debug")]
    public TextMeshProUGUI resourceText;
    public TextMeshProUGUI buildText;

    public event Action OnClicked, OnExit;
    [Header("Gameplay Debug")]

    bool isPaused;


    int buildCount;
    int buildLimit = 10;
    public int resources;
    bool inBuildMode;
    int buildLevelCost = 150;
    int buildLevel = 1;
    int multiplier = 1;

    void Start(){
        cam = GetComponent<Camera>();
        if (cam == null)
        {
            cam = Camera.main;
        }
        cam.orthographic = isOrthagraphic;
        StopPlacement();
    }
    void StartPlacement(int ID){
        StopPlacement();
        currentObjectID = database.objectData.FindIndex(data => data.ID == ID);
        if(currentObjectID < 0){
            return;
        }
        GenerateVisuals();
        OnClicked += Build;
        OnExit += StopPlacement;
    }
    void StopPlacement(){
        currentObjectID = -1;
        DestroyVisuals();
        OnClicked -= Build;
        OnExit -= StopPlacement;
    }
    void EnterBuildMode(){
        inBuildMode = true;
        StartPlacement(currentObjectID);
    }
    void ExitBuildMode(){
        inBuildMode = false;
        StopPlacement();
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
    public bool IsPointerOverUI() => EventSystem.current.IsPointerOverGameObject();
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
            if(!inBuildMode){
                EnterBuildMode();
            }else{
                ExitBuildMode();
            }
            gameUI.SetActive(inBuildMode);
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
            panSpeed -= scroll * panShrink;
            if(isOrthagraphic){
                cam.orthographicSize -= scroll * zoomSpeed;
                cam.orthographicSize = Mathf.Clamp(cam.orthographicSize, minZoom, maxZoom);
                panSpeed = Mathf.Clamp(panSpeed, minZoom, maxZoom);
            } else {
                Vector3 camPosition = cam.transform.position;
                camPosition.y = Mathf.Clamp(camPosition.y, 30f, 80f); // Adjust the y-axis for zooming
                cam.transform.position = camPosition;
                panSpeed = Mathf.Clamp(panSpeed, minZoom, maxZoom);
            }
        }
        transform.position = pos;
        
        UpdateUI();

        if (Input.GetMouseButtonDown(0)){
            Build();
        }
        if (Input.GetMouseButtonDown(1)){
            //UnBuild
        }
    }


    void Build(){
        if(IsPointerOverUI()){
            return;
        }
        Vector3 newMousePosition = mousePosition;
        if (cursorInstance != null)
        {
            cursorInstance.position = newMousePosition;
        }
        Vector3Int gridPosition = grid.WorldToCell(newMousePosition);
        GameObject newBuild = Instantiate(database.objectData[currentObjectID].Prefab);
        newBuild.transform.position = grid.CellToWorld(gridPosition);
    }
    void UnBuild(GameObject prefab){
        Building targetBuilding = prefab.transform.GetComponent<Building>();
        if(resources < resourceLimit) resources += targetBuilding.value;
        Destroy(targetBuilding);
        buildCount--;
    }
    
    void GenerateVisuals()
    {
        GameObject newMouse = Instantiate(prefab, mousePosition, Quaternion.identity) as GameObject;
        cursorInstance = newMouse.transform;
        GameObject newIndicatior = Instantiate(cursorMarkerPrefab, mousePosition, Quaternion.identity) as GameObject;
        cursorMarkerInstance = newIndicatior.transform;
        gridHolder.SetActive(true);
    }
    void DestroyVisuals(){
        
        if(cursorInstance != null){
            Destroy(cursorInstance.gameObject);
            cursorInstance = null;
        }
        if(cursorMarkerInstance != null){
            Destroy(cursorMarkerInstance.gameObject);
            cursorMarkerInstance = null;
        }
        if(gridHolder != null)gridHolder.SetActive(false);
    }

    void SwapBuildings(int id){
    }
    void UpdateUI(){
        if(buildText != null) buildText.text = $"Build Level: {buildLevel}  ({buildCount} / {buildLimit})";
        if(resourceText != null) resourceText.text = $"Local Resources: $ {resources}";
        if(gridHolder != null && inBuildMode) gridHolder.SetActive(true); else gridHolder.SetActive(false);
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

using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class BubbleMiniGameManager : MonoBehaviour
{
    public static BubbleMiniGameManager bubbleManager;
    public Camera gameCamera;
    public TextMeshProUGUI scoreText;
    int score, count;
    public int maxcount = 4;
    float spawnTimer;
    public float spawnTime = 3f;
    public AudioClip popSFX;
    public GameObject bubbleObj;
    public Transform bubbleSpawnLocation;
    PlayerControls controls;

    void OnDisable(){
        BubbleMiniGameBubble.onBubblePop -= BubblePop;
        controls.Disable();
    }
    void OnEnable(){
        BubbleMiniGameBubble.onBubblePop += BubblePop;
        controls.Enable();
    }

    private void Awake()
    {
        if (bubbleManager == null)
        {
            bubbleManager = this;
        }
        else
        {
            Destroy(gameObject);
        }

        controls = new PlayerControls();
    }
    void Start()
    {
        score = 0;
        controls.Touch.TouchPress.started += ctx => StartTouch(ctx);
    }

    void Update()
    {
        bool canSpawn = count < maxcount;
        if(spawnTimer < spawnTime){
            spawnTimer += Time.deltaTime;
        } else if (spawnTimer >= spawnTime){
            spawnTimer = 0;
            if (canSpawn) MakeBubble();
        }
        if(scoreText != null){
            scoreText.text = $"{score}";
        }
    }

    private void MakeBubble()
    {
        Instantiate(bubbleObj, bubbleSpawnLocation.position, bubbleSpawnLocation.rotation);
        count++;
    }
    void StartTouch(InputAction.CallbackContext context){
        TargetBubble(controls.Touch.TouchPosition.ReadValue<Vector2>());
    }
    void TargetBubble(Vector2 rayPosition){
        
        RaycastHit2D hit =
        Physics2D.Raycast(gameCamera.ScreenToWorldPoint(rayPosition), Vector2.zero);

        if (hit.collider != null)
        {
            BubbleMiniGameBubble bubble = hit.collider.GetComponent<BubbleMiniGameBubble>();
            if (bubble != null)
            {
                bubble.Pop();
            }
        }
    }

    void BubblePop(){
        AudioManager.instance.PlaySFXClip(popSFX);
        score++;
        count--;
    }
}

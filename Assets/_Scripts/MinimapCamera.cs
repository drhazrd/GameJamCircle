using UnityEngine;


public class MinimapCamera: MonoBehaviour
{
    public Transform target;
	Camera mapCamera;

	[SerializeField]float defaultSize = 3.25f;
	private void Awake()
	{
		mapCamera = GetComponentInChildren<Camera>();
   		mapCamera.orthographicSize = defaultSize;
		mapCamera.transform.rotation = Quaternion.Euler(new Vector3(90, 0, 0));
    }

	public void TargetSetup(Transform t)
	{
		target = t;
	}
	void Update()
	{		
		if (!target) return; else transform.position = new Vector3(target.position.x, defaultSize, target.position.z);
	}
}


using UnityEngine;
 
[ExecuteInEditMode]
public class EcolocationCameraEffect : MonoBehaviour
{
 
        public Material m_material;
 
        void Start()
        {
            GetComponent<Camera>().depthTextureMode = DepthTextureMode.Depth;
        }
 
        void OnRenderImage(RenderTexture source, RenderTexture destination)
        {
            Graphics.Blit(source, destination, m_material);
        }
}

using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace UserInterface
{
    [ExecuteInEditMode]
    public class ItemIconGenerator : MonoBehaviour
    {
        public string pathToItem;
        public float orthographicSize = 0.5f;
        public Vector3 cameraPosition = new Vector3(0, 0.25f, -10);
        public int textureWidth = 256;
        public int textureHeight = 256;
        
        [Button]
        public void Generate()
        {
            // Create a new render texture
            var renderTexture = new RenderTexture(textureWidth, textureHeight, 24);
            renderTexture.Create();

            // Create a new camera to render the prefab
            var camera = new GameObject().AddComponent<Camera>();
            camera.targetTexture = renderTexture;
            camera.transform.position = cameraPosition;
            camera.orthographic = true;
            camera.orthographicSize = orthographicSize;
            camera.clearFlags = CameraClearFlags.SolidColor;
            camera.backgroundColor = Color.clear;
            
            // Set the camera size to fit the object bounds
            var item = AssetDatabase.LoadAssetAtPath<GameObject>(pathToItem);
            var bounds = item.GetComponent<MeshRenderer>().bounds;
            var cameraHeight = camera.orthographicSize;
            var cameraWidth = cameraHeight * camera.aspect;
            var scaleX = cameraWidth / bounds.size.x;
            var scaleY = cameraHeight / bounds.size.y;
            var scale = Mathf.Max(scaleX, scaleY);
            camera.transform.localScale = new Vector3(scale, scale, 1);

            // Render the prefab to the render texture
            var instance = Instantiate(item, Vector3.zero, Quaternion.identity);
            camera.Render();

            // Create a new texture from the render texture
            var texture = new Texture2D(textureWidth, textureHeight);
            RenderTexture.active = renderTexture;
            texture.ReadPixels(new Rect(0, 0, textureWidth, textureHeight), 0, 0);
            texture.Apply();
            RenderTexture.active = null;

            // Destroy the camera and render texture
            DestroyImmediate(camera.gameObject);
            DestroyImmediate(instance);
            RenderTexture.ReleaseTemporary(renderTexture);

            AssetDatabase.CreateAsset(texture, "Assets/" + "GeneratedTexture" + ".asset");
            AssetDatabase.SaveAssets();
        }
    }
}
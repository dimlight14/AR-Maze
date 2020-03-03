using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ARMaze
{
    public class TextureCapture : MonoBehaviour
    {
        public int TextureResolution = 512;

        private string screensPath;
        private int TextureResolutionX;
        private int TextureResolutionY;

        private Camera Render_Texture_Camera;
        private RenderTexture CameraOutputTexture;

        private void Start() {
            Render_Texture_Camera = GetComponent<Camera>();
            StartRenderingToTexture();
        }

        private void StartRenderingToTexture()      // Note: RenderTexture will be delayed by one frame
        {
            TextureResolutionX = TextureResolution;
            TextureResolutionY = TextureResolution;

            if (CameraOutputTexture) {
                Render_Texture_Camera.targetTexture = null;
                CameraOutputTexture.Release();
                CameraOutputTexture = null;
            }

            CameraOutputTexture = new RenderTexture(TextureResolutionX, TextureResolutionY, 0);
            CameraOutputTexture.Create();
            Render_Texture_Camera.targetTexture = CameraOutputTexture;

            if (transform.parent) gameObject.layer = transform.parent.gameObject.layer;
            Render_Texture_Camera.cullingMask = 1 << gameObject.layer;
        }

        // public void MakeScreen() {
        //     StartRenderingToTexture();  // Restart

        //     StartCoroutine(TakeScreen());
        // }

        // private IEnumerator TakeScreen() {
        //     yield return new WaitForEndOfFrame();

        //     Texture2D FrameTexture = new Texture2D(CameraOutputTexture.width, CameraOutputTexture.height, TextureFormat.RGBA32, false);
        //     RenderTexture.active = CameraOutputTexture;
        //     FrameTexture.ReadPixels(new Rect(0, 0, CameraOutputTexture.width, CameraOutputTexture.height), 0, 0);
        //     RenderTexture.active = null;

        //     FrameTexture.Apply();
        // }

        public Texture2D GetTexture() {
            Texture2D FrameTexture = new Texture2D(CameraOutputTexture.width, CameraOutputTexture.height, TextureFormat.RGBA32, false);
            RenderTexture.active = CameraOutputTexture;
            FrameTexture.ReadPixels(new Rect(0, 0, CameraOutputTexture.width, CameraOutputTexture.height), 0, 0);
            RenderTexture.active = null;

            FrameTexture.Apply();
            return FrameTexture;
        }

    }
}

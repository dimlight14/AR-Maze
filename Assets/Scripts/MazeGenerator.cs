using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using Vuforia;

namespace ARMaze
{
    public class MazeGenerator : MonoBehaviour
    {
        [SerializeField] private GameObject cubePrefab = null;
        [SerializeField] private TextureCapture textureCapturer = null;
        [SerializeField] private Text toleranceDisplay = null;
        [SerializeField] private GameObject premadeMaze = null;
        [SerializeField] private MouseController mouseController;

        private List<GameObject> mazeParts = new List<GameObject>();

        private const int GridWidth = 8;
        private const float CellWorldSpaceSize = 0.041796875f;
        private const float GridLeftSide = -0.1671875f;
        private const float GridBottom = -0.1671875f;
        private const float YPosition = 0.03f;
        private const float ToleranceStep = 0.05f;
        private const float MinTolerance = 1.00f;
        private const float MaxTolerance = 2.95f;

        private float colorTolerance = 2.65f;
        private int cellSize = 0;
        private Texture2D capturedTexture;

        private void Start() {
            premadeMaze.SetActive(false);
            UpdateToleranceDisplay();
            cellSize = Mathf.RoundToInt(textureCapturer.TextureResolution / GridWidth);
        }

        public void GenerateMaze() {
            if (mazeParts.Count != 0) {
                foreach (GameObject part in mazeParts) {
                    Destroy(part);
                }
                mazeParts.Clear();
            }

            mouseController.OnMazeGenerated();
            capturedTexture = textureCapturer.GetTexture();
            AnalyzeTexture();

            premadeMaze.SetActive(true);
        }

        private void AnalyzeTexture() {
            for (int i = 0; i < GridWidth; i++) {
                for (int j = 0; j < GridWidth; j++) {
                    AnalyzeCell(i, j);
                }
            }
        }
        private void AnalyzeCell(int x, int y) {
            Color[] pixels = capturedTexture.GetPixels(x * cellSize, y * cellSize, cellSize, cellSize);

            float r = 0;
            float g = 0;
            float b = 0;

            for (int i = 0; i < pixels.Length; i++) {
                Color pixel = pixels[i];
                r += pixel.r;
                g += pixel.g;
                b += pixel.b;
            }

            Color averageColor = new Color(r / pixels.Length, g / pixels.Length, b / pixels.Length, 1);
            float channelSum = averageColor.r + averageColor.b + averageColor.g;

            if (channelSum < colorTolerance) {
                PlaceMazeCube(x, y, averageColor);
            }
        }
        private void PlaceMazeCube(int x, int y, Color color) {
            GameObject newCube = Instantiate(cubePrefab, new Vector3(), Quaternion.identity, premadeMaze.transform);

            newCube.transform.localPosition = new Vector3(
                GridLeftSide + (x + 0.5f) * CellWorldSpaceSize,
                YPosition,
                GridBottom + (y + 0.5f) * CellWorldSpaceSize
            );

            MeshRenderer meshRenderer = newCube.GetComponent<MeshRenderer>();
            meshRenderer.material.color = color;

            mazeParts.Add(newCube);
        }

        public void DecreaseColorTolerance() {
            colorTolerance -= ToleranceStep;
            if (colorTolerance < MinTolerance) {
                colorTolerance = MinTolerance;
            }
            UpdateToleranceDisplay();
        }
        public void IncreaseColorTolerance() {
            colorTolerance += ToleranceStep;
            if (colorTolerance > MaxTolerance) {
                colorTolerance = MaxTolerance;
            }
            UpdateToleranceDisplay();
        }
        private void UpdateToleranceDisplay() {
            float number = Mathf.RoundToInt(colorTolerance * 100);
            number /= 100;
            toleranceDisplay.text = number.ToString();
        }

    }
}
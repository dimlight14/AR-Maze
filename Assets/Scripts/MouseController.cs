using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace ARMaze
{
    public class MouseController : MonoBehaviour
    {
        [SerializeField] private NavMeshAgent mouse = null;
        [SerializeField] private Camera cam = null;
        private bool mazeOn = false;

        public void OnMazeGenerated() {
            mouse.gameObject.SetActive(true);
            mazeOn = true;
        }

        private void Update() {
            if (mazeOn && Input.GetMouseButtonDown(0)) {
                Ray ray = cam.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit)) {
                    mouse.SetDestination(hit.point);
                }

            }
        }
    }
}

using System;
using System.Linq;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.UIElements;

namespace Mouse
{
    
    public class PlayerMouseInput : MonoBehaviour
    {
        public new Camera camera;
        public bool isMouseOverUI;
        public Subject<RaycastHit[]> hitsSubject = new();
        
        [SerializeField]
        public MonoBehaviour[] consumers;
        
        private void OnEnable()
        {
            DetectMouseOverUI();
            DetectRaycastHits();
        }

        private void DetectMouseOverUI()
        {
            var uiDocument = FindObjectOfType<UIDocument>();
            if (uiDocument == null) return;
            
            var root = uiDocument.rootVisualElement;
            var container = root.Q("UIContainer");
            
            foreach (var visualElement in container.Children())
            {
                visualElement.RegisterCallback<PointerEnterEvent>(_ => isMouseOverUI = true);
                visualElement.RegisterCallback<PointerLeaveEvent>(_ => isMouseOverUI = false);
            }
        }

        private void DetectRaycastHits()
        {
            if (camera == null) camera = Camera.main;

            var hits = new RaycastHit[100];
            
            var fixedUpdateHits =
                gameObject
                    .FixedUpdateAsObservable()
                    .Select(_ =>
                    {
                        var ray = camera.ScreenPointToRay(Input.mousePosition);
                        var numHits = Physics.RaycastNonAlloc(ray, hits);
                        return numHits == 0 ? Array.Empty<RaycastHit>() : hits.Take(numHits).ToArray();
                    });

            var updateHits =
                fixedUpdateHits
                    .CombineLatest(gameObject.UpdateAsObservable(), (raycastHits, unit) => raycastHits)
                    .Where(r => !isMouseOverUI)
                    .Where(r => r.Length > 0);

            updateHits
                .Subscribe(hitsSubject);
                
            updateHits
                .Where(_ => Input.GetMouseButtonDown(0))
                .Subscribe(hits =>
                {
                    foreach (var consumer in consumers)
                    {
                        if (consumer is not IMouseInputConsumer mouseInputConsumer) continue;
                        if (mouseInputConsumer.OnMouseInput(new MouseInput { hits = hits }))
                            break;
                    }
                });
        }
    }
}
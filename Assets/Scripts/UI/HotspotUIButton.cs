using UnityEngine;
using UnityEngine.UI;
using dev.xrlab.interactive3dviewer.common;

namespace dev.xrlab.interactive3dviewer.ui
{
    public class HotspotUIButton : MonoBehaviour
    {
        [SerializeField] private Button button;
        [SerializeField] private HotspotData hotspotData;

        private void Awake()
        {
            button.onClick.AddListener(OnClick);
        }

        private void OnDestroy()
        {
            button.onClick.RemoveListener(OnClick);
        }

        private void OnClick()
        {
            HotspotEvents.OnHotspotActivated?.Invoke(hotspotData);
        }
    }
}
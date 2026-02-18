using dev.xrlab.interactive3dviewer.common;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using TMPro;

namespace dev.xrlab.interactive3dviewer.ui
{
    public class InfoUIController : MonoBehaviour
    {
        [Header("Root")]
        [SerializeField] private GameObject infoRoot;

        [Header("Text Module")]
        [SerializeField] private GameObject textModule;
        [SerializeField] private TMP_Text titleText;
        [SerializeField] private TMP_Text descriptionText;

        [Header("Image Module")]
        [SerializeField] private GameObject imageModule;
        [SerializeField] private Image contentImage;

        [Header("Video Module")]
        [SerializeField] private GameObject videoModule;
        [SerializeField] private VideoPlayer videoPlayer;

        [Header("Controls")]
        [SerializeField] private Button backButton;

        private HotspotData currentData;

        private void Awake()
        {
            infoRoot.SetActive(false);
            backButton.onClick.AddListener(OnBackPressed);
        }

        private void OnEnable()
        {
            HotspotEvents.OnCameraFocusReached += OnCameraFocusReached;
        }

        private void OnDisable()
        {
            HotspotEvents.OnCameraFocusReached -= OnCameraFocusReached;
        }

        private void OnCameraFocusReached(HotspotData data)
        {
            if (data == null)
                return;

            currentData = data;

            BuildUI(data);
            infoRoot.SetActive(true);
        }

        private void BuildUI(HotspotData data)
        {
            ResetModules();

            switch (data.contentType)
            {
                case HotspotContentType.TextOnly:
                    SetupText(data);
                    break;

                case HotspotContentType.Image:
                    SetupImage(data);
                    break;

                case HotspotContentType.Video:
                    SetupVideo(data);
                    break;

                case HotspotContentType.TextAndImage:
                    SetupText(data);
                    SetupImage(data);
                    break;

                case HotspotContentType.TextAndVideo:
                    SetupText(data);
                    SetupVideo(data);
                    break;
            }
        }

        private void ResetModules()
        {
            textModule.SetActive(false);
            imageModule.SetActive(false);
            videoModule.SetActive(false);

            if (videoPlayer != null && videoPlayer.isPlaying)
                videoPlayer.Stop();
        }

        private void SetupText(HotspotData data)
        {
            textModule.SetActive(true);

            if (titleText != null)
                titleText.text = data.title;

            if (descriptionText != null)
                descriptionText.text = data.description;
        }

        private void SetupImage(HotspotData data)
        {
            if (data.image == null || contentImage == null)
                return;

            imageModule.SetActive(true);
            contentImage.sprite = data.image;
        }

        private void SetupVideo(HotspotData data)
        {
            if (data.videoClip == null || videoPlayer == null)
                return;

            videoModule.SetActive(true);
            videoPlayer.clip = data.videoClip;
            videoPlayer.Play();
        }

        private void OnBackPressed()
        {
            if (videoPlayer != null && videoPlayer.isPlaying)
                videoPlayer.Stop();

            infoRoot.SetActive(false);

            HotspotEvents.OnHotspotExited?.Invoke(currentData);

            currentData = null;
        }
    }
}
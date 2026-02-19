using TMPro;
using UnityEngine;
using UnityEngine.UI;
using User;
using VContainer;

namespace Session.Scheme.Windows
{
    public class CameraSettingsWindow : BaseWindow
    {
        [Inject]
        public void Construct(WindowFactory windowFactory, CameraController cameraController)
        {
            _windowFactory = windowFactory;
            _cameraController = cameraController;
        }

        private WindowFactory _windowFactory;
        private CameraController _cameraController;

        [Header("UI")]
        [SerializeField] private Button _closeButton;
        [SerializeField] private Slider _moveSensSlider;
        [SerializeField] private TextMeshProUGUI _moveSensText;
        [SerializeField] private Slider _zoomSensSlider;
        [SerializeField] private TextMeshProUGUI _zoomSensText;

        private void Start()
        {
            _moveSensSlider.maxValue = 2;
            _moveSensSlider.value = _cameraController.MoveSensitivityMultiplier;
            _moveSensText.text = _moveSensSlider.value.ToString();
            _zoomSensSlider.maxValue = 2;
            _zoomSensSlider.value = _cameraController.ZoomSensitivityMultiplier;
            _zoomSensText.text = _zoomSensSlider.value.ToString();

            _closeButton.onClick.AddListener(() =>
            {
                _windowFactory.CloseWindow(this);
            });

            _moveSensSlider.onValueChanged.AddListener(v =>
            {
                _cameraController.MoveSensitivityMultiplier = v;
                _moveSensText.text = v.ToString();
            });

            _zoomSensSlider.onValueChanged.AddListener(v =>
            {
                _cameraController.ZoomSensitivityMultiplier = v;
                _zoomSensText.text = v.ToString();
            });
        }

        private void OnDestroy()
        {
            _closeButton.onClick.RemoveAllListeners();
            _moveSensSlider.onValueChanged.RemoveAllListeners();
            _zoomSensSlider.onValueChanged.RemoveAllListeners();
        }
    }
}
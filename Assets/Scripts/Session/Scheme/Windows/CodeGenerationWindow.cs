using GlobalServices;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace Session.Scheme.Windows
{
    public class CodeGenerationWindow : BaseWindow
    {
        [Inject]
        public void Construct(CodeGenerationFactory codeGenerationFactory, WindowFactory windowFactory)
        {
            _codeGenerationFactory = codeGenerationFactory;
            _windowFactory = windowFactory;
        }

        private CodeGenerationFactory _codeGenerationFactory;
        private WindowFactory _windowFactory;

        [Header("UI")]
        [SerializeField] private RectTransform _content;
        [SerializeField] private Button _closeButton;
        [SerializeField] private Button _generateCodeButton;
        [SerializeField] private Button _copyCodeButton;
        [SerializeField] private TMP_Text _outputText;

        private void Start()
        {
            _closeButton.onClick.AddListener(() => { _windowFactory.CloseWindow(this); });
            _generateCodeButton.onClick.AddListener(async () => { await GenerateCode(); });
            _copyCodeButton.onClick.AddListener(() => GUIUtility.systemCopyBuffer = _outputText.text);
        }

        private async Task GenerateCode()
        {
            _closeButton.interactable = false;
            _outputText.text = "Code generation in process...";

            string code = await _codeGenerationFactory.GenerateCode();

            _outputText.text = code;
            _outputText.ForceMeshUpdate(true, true);
            Canvas.ForceUpdateCanvases();

            _content.sizeDelta = new Vector2(
                _content.sizeDelta.x,
                _outputText.preferredHeight);

            _closeButton.interactable = true;
        }

        private void OnDestroy()
        {
            _closeButton.onClick.RemoveAllListeners();
            _generateCodeButton.onClick.RemoveAllListeners();
            _copyCodeButton.onClick.RemoveAllListeners();
        }
    }
}
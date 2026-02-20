using GlobalServices;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VContainer;
using static System.Net.Mime.MediaTypeNames;

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
        [SerializeField] private TMP_Text _outputText;

        private void Start()
        {
            _closeButton.onClick.AddListener(() => { _windowFactory.CloseWindow(this); });
            _generateCodeButton.onClick.AddListener(async () => { await GenerateCode(); });
        }

        private async Task GenerateCode()
        {
            _closeButton.interactable = false;
            _outputText.text = "Генерация в процессе...";
            string code = await _codeGenerationFactory.GenerateCode();

            _outputText.text = code;
            _outputText.ForceMeshUpdate(true, true);
            Canvas.ForceUpdateCanvases();

            float newHeight = Mathf.Max(_outputText.preferredHeight, 1);

            _outputText.rectTransform.rect.Set(
                _outputText.rectTransform.rect.x,
                _outputText.rectTransform.rect.y,
                _outputText.rectTransform.rect.width,
                newHeight);

            _content.rect.Set(
                _content.rect.x,
                _content.rect.y,
                _content.rect.width,
                newHeight);

            _closeButton.interactable = true;
        }

        private void OnDestroy()
        {
            _closeButton.onClick.RemoveAllListeners();
            _generateCodeButton.onClick.RemoveAllListeners();
        }
    }
}

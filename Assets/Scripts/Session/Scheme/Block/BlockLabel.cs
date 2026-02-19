using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace Session.Scheme.Block
{
    public class BlockLabel : MonoBehaviour
    {
        [SerializeField] private TMP_Text _text;
        [SerializeField] private Vector2 _padding = new(0.5f, 0.5f);

        public UnityAction<Vector2> OnChanged;

        public void SetText(string text)
        {
            if (_text == null) return;

            _text.text = text;
            _text.ForceMeshUpdate(true, true);
            Canvas.ForceUpdateCanvases();

            float newWidth = Mathf.Max(_text.preferredWidth + _padding.x, 1);
            float newHeight = Mathf.Max(_text.preferredHeight + _padding.y, 1);

            _text.transform.localPosition = Vector3.zero;

            OnChanged?.Invoke(new Vector2(newWidth, newHeight));
        }

        public string GetText()
        {
            return _text.text;
        }

        private void OnDestroy()
        {
            OnChanged = null;
        }
    }
}
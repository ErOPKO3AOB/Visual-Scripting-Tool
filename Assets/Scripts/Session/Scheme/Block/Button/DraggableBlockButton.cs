using GlobalServices.ProjectLifetime;
using System.Collections;
using UnityEngine;
using User;
using VContainer;

namespace Session.Scheme.Block.Button
{
    public class DraggableBlockButton : BaseBlockButton
    {
        public void ConstructManually(BlockConfigs blockConfigs)
        {
            _blockConfigs = blockConfigs;
        }

        private BlockConfigs _blockConfigs;
        private Camera _camera;

        private SpriteRenderer _spriteRenderer;
        private Color _startColor;
        private Vector3 _startScale;

        public bool CanDrag { get; private set; } = true;
        private bool _dragging;
        private Vector3 _worldPointerPosition;

        protected override void Start()
        {
            base.Start();

            _spriteRenderer = GetComponent<SpriteRenderer>();
            _startColor = _spriteRenderer.color;
            _startScale = transform.localScale;
        }

        public void SetWorldMousePosition(Vector3 worldPosition)
        {
            _worldPointerPosition = worldPosition;
        }

        public override void Use()
        {
            if (!CanDrag) return;

            _dragging = true;

            if (_spriteRenderer != null)
            {
                _spriteRenderer.color = _blockConfigs.DraggingColorAffect;
            }

            transform.localScale = _startScale * _blockConfigs.DraggingSizeAffect;

            StartCoroutine(UseProccesRoutine(/*transform.position - new Vector3(_worldPointerPosition.x, _worldPointerPosition.y, transform.position.z)*/Vector3.zero));
        }

        private IEnumerator UseProccesRoutine(Vector3 dragOffset)
        {
            while (_dragging)
            {
                Vector3 targetPosition = Vector3.Lerp(transform.position,
                    new Vector3(_worldPointerPosition.x, _worldPointerPosition.y, transform.position.z) + dragOffset,
                    Time.deltaTime * _blockConfigs.DragSensitivity);

                transform.position = targetPosition;
                yield return null;
            }
        }

        public void StopUsage()
        {
            if (!CanDrag) return;

            _dragging = false;

            if (_spriteRenderer != null)
            {
                _spriteRenderer.color = _startColor;
            }

            transform.localScale = _startScale;
        }

        public void SetDragUsage(bool value)
        {
            CanDrag = value;
        }
    }
}
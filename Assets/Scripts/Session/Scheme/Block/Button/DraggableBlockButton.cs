using GlobalServices.ProjectLifetime;
using UnityEngine;
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

        private SpriteRenderer _spriteRenderer;
        private Color _startColor;
        private Vector3 _startScale;

        protected override void Start()
        {
            base.Start();

            _spriteRenderer = GetComponent<SpriteRenderer>();
            _startColor = _spriteRenderer.color;
            _startScale = transform.localScale;
        }

        public override void Use()
        {
            if (_spriteRenderer != null)
            {
                _spriteRenderer.color = _blockConfigs.DraggingColorAffect;
            }

            transform.localScale = _startScale * _blockConfigs.DraggingSizeAffect;
        }

        public void StopUsage()
        {
            if (_spriteRenderer != null)
            {
                _spriteRenderer.color = _startColor;
            }

            transform.localScale = _startScale;
        }
    }
}
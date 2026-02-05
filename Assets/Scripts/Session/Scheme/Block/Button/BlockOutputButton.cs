using GlobalServices.ProjectLifetime;
using Session.Scheme.Connector;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Session.Scheme.Block.Button
{
    public class BlockOutputButton : BaseBlockButton
    {
        public void ConstructManually(BlockConfigs blockConfigs, IActionProvider block)
        {
            _blockConfigs = blockConfigs;
            _block = block;
        }

        private BlockConfigs _blockConfigs;
        private IActionProvider _block;

        [SerializeField] private float _holdTime = 2f;
        private float _holdTimer = 0;
        private bool _holding = false;

        private SpriteRenderer _spriteRenderer;
        private Color _startColor;

        public UnityAction OnFullyHolded;

        private bool _hasConnector = false;
        public bool HasConnector => _hasConnector;

        private void OnValidate()
        {
            if (_holdTime < 0) _holdTime = 0;
        }

        protected override void Start()
        {
            base.Start();

            _spriteRenderer = GetComponent<SpriteRenderer>();
            _startColor = _spriteRenderer.color;
        }

        public override void Use()
        {
            if (!_hasConnector)
            {
                _holding = true;
                StartCoroutine(UseProccesRoutine());

                Debug.Log("Use");
            }
        }

        private IEnumerator UseProccesRoutine()
        {
            Color currentColor = _spriteRenderer.color;
            while (_holding && _holdTimer < _holdTime)
            {
                _holdTimer += Time.deltaTime;
                _spriteRenderer.color = Color.Lerp(currentColor, _blockConfigs.DraggingColorAffect, _holdTimer / _holdTime);
                yield return null;
            }

            // Fully holded
            if (_holdTimer >= _holdTime)
            {
                _spriteRenderer.color = _blockConfigs.DraggingColorAffect;
                ActionConnecorFacade actionConnecorFacade = Instantiate(_blockConfigs.ActionConnecorFacadePrefab, transform);
                actionConnecorFacade.ConstructManually(_block, _blockConfigs);
                _hasConnector = true;
            }

            StopUsage();

            currentColor = _spriteRenderer.color;
            float returningTimer = 0;
            while (true)
            {
                returningTimer += Time.deltaTime;
                _spriteRenderer.color = Color.Lerp(currentColor, _startColor, returningTimer / _holdTime);
                yield return null;
            }
        }

        public void StopUsage()
        {
            _holdTimer = 0;
            _holding = false;
            Debug.Log("Stop usage");
        }
    }
}
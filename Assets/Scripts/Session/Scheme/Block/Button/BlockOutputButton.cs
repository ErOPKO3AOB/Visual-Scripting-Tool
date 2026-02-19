using GlobalServices.ProjectLifetime;
using Session.Scheme.Connector;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace Session.Scheme.Block.Button
{
    public class BlockOutputButton : BaseBlockButton
    {
        public void ConstructManually(BlockConfigs blockConfigs, IBlock block, Vector3 startConnectorOffset)
        {
            _blockConfigs = blockConfigs;
            _block = block;
            _startConnectorOffset = startConnectorOffset;
        }

        private BlockConfigs _blockConfigs;
        private IBlock _block;
        public IBlock Block => _block;

        [SerializeField] private float _holdTime = 0.1f;
        private Vector3 _startConnectorOffset;
        private float _holdTimer = 0;
        private bool _holding = false;

        private SpriteRenderer _spriteRenderer;
        private Color _startColor;

        public UnityAction OnFullyHolded;

        [SerializeField] private bool _hasConnector = false;
        public bool HasConnector => _hasConnector;

        private ActionConnecorFacade _actionConnecorFacade;
        public ActionConnecorFacade ActionConnecorFacade => _actionConnecorFacade;

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
            _holding = true;
            StartCoroutine(UseProccesRoutine());
        }

        private IEnumerator UseProccesRoutine()
        {
            Color currentColor = _spriteRenderer.color;
            while (_holding && _holdTimer < _holdTime)
            {
                _holdTimer += Time.deltaTime;
                _spriteRenderer.color = Color.Lerp(currentColor, _blockConfigs.HoldingColorAffect, _holdTimer / _holdTime);
                yield return null;
            }

            // Fully holded
            if (_holdTimer >= _holdTime)
            {
                _spriteRenderer.color = _blockConfigs.HoldingColorAffect;

                if (_hasConnector && _actionConnecorFacade != null)
                {
                    int index = _block.Facade.BlockOutputButtons.ToList().FindIndex(oB => oB == this);
                    _actionConnecorFacade.OnDisconnected(index);
                    _hasConnector = false;
                }

                else
                {
                    _actionConnecorFacade = Instantiate(_blockConfigs.ActionConnecorFacadePrefab, transform);
                    _actionConnecorFacade.ConstructManually(_block, _blockConfigs, _startConnectorOffset, this);
                    _hasConnector = true;
                }
            }

            StopUsage();

            currentColor = _spriteRenderer.color;
            float returningTimer = 0;
            while (returningTimer < _holdTime)
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
        }
    }
}
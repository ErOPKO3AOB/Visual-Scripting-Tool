using GlobalServices.ProjectLifetime;
using Session.Scheme.Block.Types;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using VContainer;

namespace Session.Scheme.Windows
{
    public class InventoryBlockItem : BaseWindow
    {
        [Inject]
        public void Construct(BlockConfigs blockConfigs)
        {
            _blockConfigs = blockConfigs;
        }

        public void ConstructManualy(Type blockType, string blockName, string labelName)
        {
            //Color blockColor = new();

            if (blockType == typeof(ConditionBlock))
            {
                _image.sprite = _blockConfigs.RhombSprite;

                
            }

            else if (blockType == typeof(InputBlock) || blockType == typeof(OutputBlock))
            {
                _image.sprite = _blockConfigs.ParallelogramSprite;
            }

            else if (blockType == typeof(StartBlock) || blockType == typeof(EndBlock))
            {
                _image.sprite = _blockConfigs.CircleSprite;
            }

            else
            {
                _image.sprite = _blockConfigs.BoxSprite;
            }

            //_image.color = 
            _blockName = blockName;
            _label.text = labelName;

            _button.onClick.AddListener(() =>
            {
                OnPressed?.Invoke(blockName);
            });
        }

        [SerializeField] private Button _button;
        [SerializeField] private TMP_Text _label;
        [SerializeField] private Image _image;

        public UnityAction<string> OnPressed;

        private BlockConfigs _blockConfigs;
        private string _blockName;

        private void Start()
        {
            
        }

        private void OnDestroy()
        {
            _button.onClick.RemoveAllListeners();
        }
    }
}
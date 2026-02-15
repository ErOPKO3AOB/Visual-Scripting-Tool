using UnityEngine;

namespace Session.Scheme
{
    public interface IActionProvider
    {
        public IActionProvider Next { get; set; }

        void ProvideAction();
    }
}
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace TFPlay.Modules.PopUpText
{
    public class PopUpTextService : MonoBehaviour, IPopUpTextService
    {
        [SerializeField] private Transform messagesParent;
        [SerializeField] private PopUpText textPrefab;
        [Min(1)] [SerializeField] private int startingPoolSize = 5;
        [Min(1)] [SerializeField] private int maxPoolSize = 20;

        private List<PopUpText> _pool;

        private void Start()
        {
            CreatePool();
        }

        public void ShowPopUp(string text, Vector3 position)
        {
            var popUpText = GetPopUpText();
            if (!popUpText) return;

            popUpText.Show(text, position);
        }

        public void ShowPopUp(string text, Vector3 position, Vector2 size)
        {
            var popUpText = GetPopUpText();
            if (!popUpText) return;

            popUpText.Show(text, position, size);
        }

        private void CreatePool()
        {
            _pool = new List<PopUpText>();
            for (var i = 0; i < startingPoolSize; i++)
                CreatePopUpText();
        }

        private PopUpText CreatePopUpText()
        {
            var popUpText = Instantiate(textPrefab, messagesParent, true);
            popUpText.gameObject.SetActive(false);

            _pool.Add(popUpText);
            return popUpText;
        }

        private PopUpText GetPopUpText()
        {
            var popUpText = _pool.FirstOrDefault(w => !w.gameObject.activeSelf);
            if (_pool.Count < maxPoolSize && !popUpText) popUpText = CreatePopUpText();

            return popUpText;
        }
    }
}
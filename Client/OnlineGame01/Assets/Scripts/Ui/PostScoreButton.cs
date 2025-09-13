using UnityEngine;
using TMPro;
using System;
using Cysharp.Threading.Tasks;
using UnityEngine.UI;

namespace OnlineGame01.Ui
{
    public class PostScoreButton : MonoBehaviour
    {
        [SerializeField] private TMP_InputField _scoreInput;
        [SerializeField] private TMP_Text _consoleText;
        [SerializeField] private Button _button;

        private void Awake()
        {
            _button.onClick.AddListener(() => OnPostScoreButtonPressedAsync().Forget());
        }

        private async UniTask OnPostScoreButtonPressedAsync()
        {
            if (!int.TryParse(_scoreInput.text, out int score))
            {
                _consoleText.text = "<color=red>Invalid score format.</color>";
                return;
            }

            _consoleText.text = "Posting score...";
            try
            {
                string response = await ApiService.Instance.PostScoreAsync(score);
                _consoleText.text = $"Success: {response}";
            }
            catch (Exception e)
            {
                _consoleText.text = $"<color=red>Error:</color> {e.Message}";
            }
        }
    }
}
using UnityEngine;
using TMPro;
using System;
using Cysharp.Threading.Tasks;
using UnityEngine.UI;

namespace OnlineGame01.Ui
{
    public class LoginButton : MonoBehaviour
    {
        [SerializeField] private TMP_InputField _usernameInput;
        [SerializeField] private TMP_InputField _passwordInput;
        [SerializeField] private TMP_Text _consoleText;
        [SerializeField] private Button _button;

        private void Awake()
        {
            _button.onClick.AddListener(() => OnLoginButtonPressedAsync().Forget());
        }

        private async UniTask OnLoginButtonPressedAsync()
        {
            _consoleText.text = "Logging in...";
            try
            {
                string response = await new ApiService().LoginAsync(_usernameInput.text, _passwordInput.text);
                _consoleText.text = $"<color=green>{response}</color>";
            }
            catch (Exception e)
            {
                _consoleText.text = $"<color=red>Error:</color> {e.Message}";
            }
        }
    }
}
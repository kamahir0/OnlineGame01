using UnityEngine;
using TMPro;
using System;
using Cysharp.Threading.Tasks;
using UnityEngine.UI;

namespace OnlineGame01.Ui
{
    public class RegisterButton : MonoBehaviour
    {
        [SerializeField] private TMP_InputField _usernameInput;
        [SerializeField] private TMP_InputField _passwordInput;
        [SerializeField] private TMP_Text _consoleText;
        [SerializeField] private Button _button;

        private void Awake()
        {
            _button.onClick.AddListener(() => OnRegisterButtonPressedAsync().Forget());
        }

        private async UniTask OnRegisterButtonPressedAsync()
        {
            _consoleText.text = "Registering...";
            try
            {
                string response = await new ApiService().RegisterAsync(_usernameInput.text, _passwordInput.text);
                _consoleText.text = $"Success: {response}";
            }
            catch (Exception e)
            {
                _consoleText.text = $"<color=red>Error:</color> {e.Message}";
            }
        }
    }
}
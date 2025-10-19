using UnityEngine;
using TMPro;
using System;
using System.Text;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine.UI;

namespace OnlineGame01.Ui
{
    public class GetScoresButton : MonoBehaviour
    {
        [SerializeField] private TMP_Text _consoleText;
        [SerializeField] private TMP_Text _rankingText;
        [SerializeField] private Button _button;

        private void Awake()
        {
            _button.onClick.AddListener(() => OnGetScoresButtonPressedAsync().Forget());
        }

        private async UniTask OnGetScoresButtonPressedAsync()
        {
            _consoleText.text = "Getting ranking...";
            _rankingText.text = ""; // ランキング表示をクリア

            try
            {
                List<ScoreResponseData> scores = await new ApiService().GetScoresAsync();
                _consoleText.text = "<color=green>Get Ranking Success!</color>";

                var sb = new StringBuilder();
                int rank = 1;
                foreach (var score in scores)
                {
                    sb.AppendLine($"{rank++}. {score.Username}: {score.Score}");
                }

                _rankingText.text = sb.ToString();
            }
            catch (Exception e)
            {
                _consoleText.text = $"<color=red>Error:</color> {e.Message}";
            }
        }
    }
}
using TMPro;
using UnityEngine;

namespace Sources.UI.Services
{
    public class WalletView: UIViewBase
    {
        [SerializeField] private TextMeshProUGUI _moneyText;

        public void UpdateMoneyText(int money)
        {
            _moneyText.text = money.ToString();
        }
    }
}
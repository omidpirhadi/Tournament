using System;
using UnityEngine;
using UnityEngine.UI;

namespace Diaco.UI.Reports
{
    public class ReportInvitedPlayerCard : MonoBehaviour
    {
        public Image Profile;
        public RTLTMPro.RTLTextMeshPro Name;
        public Text Cup;

        private Button ButtonCard;

        public void InvitedPlayerCard(Sprite profile, string name, string cup, Action OnClickCard)
        {
            ButtonCard = GetComponent<Button>();
            Profile.sprite = profile;
            Name.text = name;
            Cup.text = cup;
            ButtonCard.onClick.AddListener(() => { OnClickCard(); });
        }

    }
}
using Hashira.Cards;
using System.Collections.Generic;
using UnityEngine;

namespace Hashira
{
    public static class CardColor
    {
        private static Dictionary<ECardType, Color> _CardColorDict;

        static CardColor()
        {
            _CardColorDict = new Dictionary<ECardType, Color>
            {
                { ECardType.Bullet, new Color(212/255f,25/255f,60/255f) },
                { ECardType.Magic, new Color(233/255f,126/255f,22/255f) },
                { ECardType.Stat, new Color(70/255f,25/255f,212/255f) }
            };
        }

        public static Color GetColorByType(ECardType type)
            => _CardColorDict[type];
    }
}

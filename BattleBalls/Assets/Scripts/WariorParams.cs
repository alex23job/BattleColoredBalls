using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts
{
    public class WariorParams
    {
        public string nameWarior;
        public string nameWariorEn;
        public int maxHP;
        public int immun;
        public int toxinPrc;
        public int firePrc;
        public int lvlSpell;
        public int lvlMagic;
        public int lvlHealing;
        public int lvlCian;
        public int lvlBrown;

        public WariorParams() { }

        public WariorParams(string nm_ru, string nm_en, int hp, int im, int tp, int fp, int ls, int lm, int lh, int lc, int lb)
        {
            nameWarior = nm_ru;
            nameWariorEn = nm_en;
            maxHP = hp;
            immun = im;
            toxinPrc = tp;
            firePrc = fp;
            lvlSpell = ls;
            lvlMagic = lm;
            lvlHealing = lh;
            lvlCian = lc;
            lvlBrown = lb;
        }

        public static WariorParams[] arrWariorParams = {
            new WariorParams("Враг1", "Warior1", 100, 0, 1, 1, 1, 1, 1, 1, 1),
            new WariorParams("Враг2", "Warior2", 200, 3, 50, 30, 2, 2, 2, 2, 2),
            new WariorParams("Враг3", "Warior3", 300, 1, 80, 60, 3, 3, 3, 3, 3)
        };
    }
}

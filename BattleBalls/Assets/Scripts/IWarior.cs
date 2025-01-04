﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts
{
    public interface IWarior
    {
        public int MaxHP { get; set; }
        public int CurrentHP { get; }
        public int ToxinPercent { get; }
        public int FirePercent { get; }
        public int LevelSpell { get; }  //  уровень заклинаний (yellow)
        public int LevelMagic { get; }  //  уровень заклинаний (blue)
        public int LevelHealing { get; }  //  уровень заклинаний (magenta)
        public int LevelCian { get; }  //  уровень заклинаний влияющих на магию (cian)
        public int LevelBrown { get; }  //  уровень заклинаний влияющих на огонь (brown)

        public void ChangeHP(int zn);

        public void BallsEffect(int zn, int col, int prc);
    }
}
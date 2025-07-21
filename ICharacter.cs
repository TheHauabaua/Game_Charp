using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
    public interface ICharacter
    {
        int Attack(Character target);
        void Defend(int damage);
        bool IsAlive();
        void PerformUniqueAbility(Character target);
        void ApplyStatusEffects();
    }
}

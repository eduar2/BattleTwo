using Lib.Repository.Entities;
using Lib.Repository.Repository;
using Microsoft.Extensions.Logging;

namespace Lib.Repository.Services;

public class BattleService
{
    private readonly IBattleOfMonstersRepository _repository;

    public BattleService(IBattleOfMonstersRepository repository)
    {
        _repository = repository;
    }
    public Monster GetWinner(Monster monsterA, Monster monsterB)
    {
        Monster winner = new Monster();
        var firstAttacker = DefineFirstAttacker(monsterA, monsterB);
        var secondAttacker = firstAttacker == monsterA ? monsterB : monsterA;
        var firstAttackerHP = firstAttacker.Hp;
        var secondAttackerHP = secondAttacker.Hp;

        int damage;
        while (firstAttackerHP > 0 && secondAttackerHP > 0)
        {

            damage = getDamage(firstAttacker.Attack, secondAttacker.Defense);
            secondAttackerHP -= damage;

            if (secondAttackerHP <= 0)
            {
                winner = firstAttacker;
            }

            damage = getDamage(secondAttacker.Attack, firstAttacker.Defense);

            firstAttackerHP -= damage;

            if (firstAttackerHP <= 0)
            {
                winner = secondAttacker;
            }
        }
        return winner;
    }

    private int getDamage(int attack, int defense)
    {
        return attack - defense <= 0 ? 1 : attack - defense;
    }
    private Monster DefineFirstAttacker(Monster monsterA, Monster monsterB) {
        var attacker = monsterA.Speed > monsterB.Speed || 
            (monsterA.Speed == monsterB.Speed && monsterA.Attack > monsterB.Attack)? monsterA : monsterB;
        return attacker;
    }
}
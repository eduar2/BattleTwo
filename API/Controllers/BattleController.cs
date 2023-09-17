using API.Utils;
using Lib.Repository.Entities;
using Lib.Repository.Repository;
using Lib.Repository.Services;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;


public class BattleController : BaseApiController
{
    private readonly IBattleOfMonstersRepository _repository;

    public BattleController(IBattleOfMonstersRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> GetAll()
    {
        IEnumerable<Battle> battles = await _repository.Battles.GetAllAsync();
        return Ok(battles);
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> Find(int id)
    {
        var battle = await _repository.Battles.FindAsync(id);
        return battle is not null ? Ok(battle) : NotFound(Constant.BATTLE_NOT_EXIST);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> Add([FromBody] Battle battle)
    {
        if (battle == null || battle.MonsterA == null || battle.MonsterB == null) {
            return BadRequest("Missing ID");
        }

        BattleService battleService = new BattleService(_repository);
        var monsterA = await _repository.Monsters.FindAsync(battle.MonsterA);
        var monsterB = await _repository.Monsters.FindAsync(battle.MonsterB);

        if (monsterA is null || monsterB is null)
        {
            return NotFound(Constant.MONSTER_NOT_EXIST); 
        }

        
        var winner = battleService.GetWinner(monsterA, monsterB);
        battle.Winner = winner.Id;
        var result = await _repository.Battles.AddAsync(battle);
        await _repository.Save();
        return Ok(battle);
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> Remove(int id)
    {
        var existingBattle = await _repository.Battles.FindAsync(id);

        if (existingBattle is null)
        {
            return NotFound(Constant.MONSTER_NOT_EXIST);
        }
        await _repository.Battles.RemoveAsync(id);
        await _repository.Save();
        return Ok();
    }
}



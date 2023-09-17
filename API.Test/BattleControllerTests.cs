using API.Controllers;
using API.Test.Fixtures;
using FluentAssertions;
using Lib.Repository.Entities;
using Lib.Repository.Repository;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace API.Test;

public class BattleControllerTests
{
    private readonly Mock<IBattleOfMonstersRepository> _repository;

    public BattleControllerTests()
    {
        this._repository = new Mock<IBattleOfMonstersRepository>();
    }

    [Fact]
    public async void Get_OnSuccess_ReturnsListOfBattles()
    {
        this._repository
            .Setup(x => x.Battles.GetAllAsync())
            .ReturnsAsync(BattlesFixture.GetBattlesMock());

        BattleController sut = new BattleController(_repository.Object);
        ActionResult result = await sut.GetAll();
        OkObjectResult objectResults = (OkObjectResult) result;
        objectResults?.Value.Should().BeOfType<Battle[]>();
    }
    
    [Fact]
    public async Task Post_BadRequest_When_StartBattle_With_nullMonster()
    {
        Monster[] monstersMock = MonsterFixture.GetMonstersMock().ToArray();
        
        Battle b = new Battle()
        {
            MonsterA = null,
            MonsterB = monstersMock[1].Id
        };

        this._repository.Setup(x => x.Battles.AddAsync(b));

        int? idMonsterA = null;
        this._repository
            .Setup(x => x.Monsters.FindAsync(idMonsterA))
            .ReturnsAsync(() => null);

        int? idMonsterB = monstersMock[1].Id;
        Monster monsterB = monstersMock[1];

        this._repository
            .Setup(x => x.Monsters.FindAsync(idMonsterB))
            .ReturnsAsync(monsterB);

        BattleController sut = new BattleController(_repository.Object);

        ActionResult result = await sut.Add(b);
        BadRequestObjectResult objectResults = (BadRequestObjectResult) result;
        result.Should().BeOfType<BadRequestObjectResult>();
        Assert.Equal("Missing ID", objectResults.Value);
    }
    
    [Fact]
    public async Task Post_OnNoMonsterFound_When_StartBattle_With_NonexistentMonster()
    {
        int? idMonsterA = 8;
        this._repository
            .Setup(x => x.Monsters.FindAsync(idMonsterA))
            .ReturnsAsync(() => null);


        Monster[] monstersMock = MonsterFixture.GetMonstersMock().ToArray();

        Battle b = new Battle()
        {
            MonsterA = idMonsterA,
            MonsterB = monstersMock[1].Id
        };

        BattleController sut = new BattleController(_repository.Object);
        ActionResult result = await sut.Add(b);
        result.Should().BeOfType<NotFoundObjectResult>();
    }

    [Fact]
    public async Task Post_OnSuccess_Returns_With_MonsterAWinning()
    {
        Monster[] monstersMock = MonsterFixture.GetMonstersMock().ToArray();

        Battle b = new Battle()
        {
            MonsterA = monstersMock[6].Id,
            MonsterB = monstersMock[5].Id
        };

        this._repository.Setup(x => x.Battles.AddAsync(It.IsAny<Battle>())).Verifiable();
        this._repository.Setup(x => x.Save()).Verifiable();
        this._repository
            .Setup(x => x.Monsters.FindAsync(b.MonsterA))
            .ReturnsAsync(() => monstersMock[6]);

        this._repository
            .Setup(x => x.Monsters.FindAsync(b.MonsterB))
            .ReturnsAsync(monstersMock[5]);

        BattleController sut = new BattleController(_repository.Object);
        ActionResult result = await sut.Add(b);

        result.Should().BeOfType<OkObjectResult>();
        OkObjectResult objectResults = (OkObjectResult)result;
        objectResults.Value.As<Battle>().Winner.Should().Be(b.MonsterA);
    }


    [Fact]
    public async Task Post_OnSuccess_Returns_With_MonsterBWinning()
    {
        Monster[] monstersMock = MonsterFixture.GetMonstersMock().ToArray();

        Battle b = new Battle()
        {
            MonsterA = monstersMock[5].Id,
            MonsterB = monstersMock[6].Id
        };

        this._repository.Setup(x => x.Battles.AddAsync(It.IsAny<Battle>())).Verifiable();
        this._repository.Setup(x => x.Save()).Verifiable();
        this._repository
            .Setup(x => x.Monsters.FindAsync(b.MonsterA))
            .ReturnsAsync(() => monstersMock[5]);

        this._repository
            .Setup(x => x.Monsters.FindAsync(b.MonsterB))
            .ReturnsAsync(monstersMock[6]);

        BattleController sut = new BattleController(_repository.Object);
        ActionResult result = await sut.Add(b);

        result.Should().BeOfType<OkObjectResult>();
        OkObjectResult objectResults = (OkObjectResult)result;
        objectResults.Value.As<Battle>().Winner.Should().Be(b.MonsterB);
    }

    [Fact]
    public async Task Post_OnSuccess_Returns_With_MonsterAWinning_When_TheirSpeedsSame_And_MonsterA_Has_Higher_Attack()
    {
        Monster[] monstersMock = MonsterFixture.GetMonstersMock().ToArray();

        Battle b = new Battle()
        {
            MonsterA = monstersMock[0].Id,
            MonsterB = monstersMock[5].Id
        };

        this._repository.Setup(x => x.Battles.AddAsync(It.IsAny<Battle>())).Verifiable();
        this._repository.Setup(x => x.Save()).Verifiable();
        this._repository
            .Setup(x => x.Monsters.FindAsync(b.MonsterA))
            .ReturnsAsync(() => monstersMock[5]);

        this._repository
            .Setup(x => x.Monsters.FindAsync(b.MonsterB))
            .ReturnsAsync(monstersMock[0]);

        BattleController sut = new BattleController(_repository.Object);
        ActionResult result = await sut.Add(b);

        result.Should().BeOfType<OkObjectResult>();

        OkObjectResult objectResults = (OkObjectResult)result;
        objectResults.Value.As<Battle>().Winner.Should().Be(b.MonsterA);
    }

    [Fact]
    public async Task Post_OnSuccess_Returns_With_MonsterBWinning_When_TheirSpeedsSame_And_MonsterB_Has_Higher_Attack()
    {
        Monster[] monstersMock = MonsterFixture.GetMonstersMock().ToArray();

        Battle b = new Battle()
        {
            MonsterA = monstersMock[5].Id,
            MonsterB = monstersMock[0].Id
        };

        this._repository.Setup(x => x.Battles.AddAsync(It.IsAny<Battle>())).Verifiable();
        this._repository.Setup(x => x.Save()).Verifiable();
        this._repository
            .Setup(x => x.Monsters.FindAsync(b.MonsterA))
            .ReturnsAsync(() => monstersMock[0]);

        this._repository
            .Setup(x => x.Monsters.FindAsync(b.MonsterB))
            .ReturnsAsync(monstersMock[5]);

        BattleController sut = new BattleController(_repository.Object);
        ActionResult result = await sut.Add(b);

        result.Should().BeOfType<OkObjectResult>();
        OkObjectResult objectResults = (OkObjectResult)result;
        objectResults.Value.As<Battle>().Winner.Should().Be(b.MonsterB);
    }

    [Fact]
    public async Task Post_OnSuccess_Returns_With_MonsterAWinning_When_TheirDefensesSame_And_MonsterA_Has_Higher_Speed()
    {
        Monster[] monstersMock = MonsterFixture.GetMonstersMock().ToArray();

        Battle b = new Battle()
        {
            MonsterA = monstersMock[1].Id,
            MonsterB = monstersMock[0].Id
        };

        this._repository.Setup(x => x.Battles.AddAsync(It.IsAny<Battle>())).Verifiable();
        this._repository.Setup(x => x.Save()).Verifiable();
        this._repository
            .Setup(x => x.Monsters.FindAsync(b.MonsterA))
            .ReturnsAsync(() => monstersMock[1]);

        this._repository
            .Setup(x => x.Monsters.FindAsync(b.MonsterB))
            .ReturnsAsync(monstersMock[0]);

        BattleController sut = new BattleController(_repository.Object);
        ActionResult result = await sut.Add(b);

        result.Should().BeOfType<OkObjectResult>();
        OkObjectResult objectResults = (OkObjectResult)result;
        objectResults.Value.As<Battle>().Winner.Should().Be(b.MonsterA);
    }

    [Fact]
    public async Task Delete_OnSuccess_RemoveBattle()
    {
        const int id = 1;
        Battle[] battles= BattlesFixture.GetBattlesMock().ToArray();
        
        
        this._repository
            .Setup(x => x.Battles.FindAsync(id))
            .ReturnsAsync(battles[0]);

        this._repository
           .Setup(x => x.Battles.RemoveAsync(id));

        BattleController sut = new BattleController(_repository.Object);

        ActionResult result = await sut.Remove(id);
        OkResult objectResults = (OkResult)result;
        objectResults.StatusCode.Should().Be(200);
        
    }

    [Fact]
    public async Task Delete_OnNoBattleFound_Returns404()
    {
        this._repository
            .Setup(x => x.Battles.RemoveAsync(It.IsAny<int>()))
            .ThrowsAsync(new Exception());

        BattleController sut = new BattleController(_repository.Object);
        ActionResult result = await sut.Remove(123);
        
        result.Should().BeOfType<NotFoundObjectResult>();
    }
}

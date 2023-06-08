using UnityEngine;

public interface IInteractable
{
    void Interact(PlayerCharacter interactor);
    void Highlight(PlayerCharacter interactor);
    void UnHighlight(PlayerCharacter interactor);

}

public interface IChoppable
{
    void Chop(PlayerCharacter interactor);
}

public interface ICollectable
{
    void Collect(PlayerCharacter interactor);
}

public interface IAttackable
{
    void Attack(PlayerCharacter interactor);
}

public interface IBuildable
{
    void Build(PlayerCharacter interactor);
}

public interface IRepairable
{
    void Repair(PlayerCharacter interactor);
}

public interface IUpgradeable
{
    void Upgrade(PlayerCharacter interactor);
}

public interface IHealable
{
    void Heal(PlayerCharacter interactor);
}

public interface IDamageable
{
    void Damage(PlayerCharacter interactor);
}

public interface IOpenable
{
    void Open(PlayerCharacter interactor);
}   

public interface ICloseable
{
    void Close(PlayerCharacter interactor);
}

public interface IUseable
{
    void Use(PlayerCharacter interactor);
}


public interface IPlaceable
{
    void Place(PlayerCharacter interactor);
}

public interface IRemoveable
{
    void Remove(PlayerCharacter interactor);
}

using UnityEngine;

public interface IColliderService 
{
    public Character Player { get; }
    public void SetPlayer(Character character);
    public void AddCharacter(Collider collider, Character character);
    public bool GetCharacter(Collider collider, out Character character);
}

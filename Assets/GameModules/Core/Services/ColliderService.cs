using System.Collections.Generic;
using UnityEngine;

public class ColliderService : IColliderService
{   
    private Dictionary<Collider, Character> characters = new();
    public Character Player { get; private set; }

    public void SetPlayer(Character character)
    {
        Player = character;
    }

    public void AddCharacter(Collider collider, Character character)
    {
        if(!characters.ContainsKey(collider))
            characters.Add(collider, character);
    }

    public bool GetCharacter(Collider collider, out Character character)
    {
        if (characters.ContainsKey(collider))
        {
            character = characters[collider];
            return true;
        }
        else
        {
            character = null;
            return false;
        }
    }
}

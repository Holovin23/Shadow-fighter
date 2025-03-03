public class ColliderService : IColliderService
{   
    public Character Player { get; private set; }

    public void SetPlayer(Character character)
    {
        Player = character;
    }
}

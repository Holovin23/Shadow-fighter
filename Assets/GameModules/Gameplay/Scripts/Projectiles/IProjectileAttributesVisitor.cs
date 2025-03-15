namespace GameModules.Gameplay.Scripts.Projectiles
{
    public interface IProjectileAttributesVisitor
    {
        public void Visit(ProjectileAttribute attribute);
        public void Visit(ProjectileDamageAttribute attribute);
        public void Visit(ProjectileDirectionForceAttribute attribute);
        public void Visit(ProjectileCollisionsAttribute attribute);
        public void Visit(ProjectileLineForceAttribute attribute);
        public void Visit(ProjectileBounceAttribute attribute);
        public void Visit(ProjectilePiercingAttribute attribute);
    }
}
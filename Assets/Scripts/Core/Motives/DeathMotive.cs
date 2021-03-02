public class DeathMotive : Motive
{
    public DeathMotive(ITurnbound target) => this.target = target;
    
    public ITurnbound target;
}
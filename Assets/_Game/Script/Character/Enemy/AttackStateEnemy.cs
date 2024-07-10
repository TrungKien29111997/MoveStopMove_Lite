public class AttackStateEnemy : IState<Enemy>
{
    public void OnEnter(Enemy enemy)
    {
        enemy.EnterAttackState();
    }

    public void OnExecute(Enemy enemy)
    {
        enemy.AttackState();
    }

    public void OnExit(Enemy enemy)
    {

    }
}

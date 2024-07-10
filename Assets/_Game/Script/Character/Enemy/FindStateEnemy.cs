public class FindStateEnemy : IState<Enemy>
{
    public void OnEnter(Enemy enemy)
    {
        enemy.EnterFindState();
    }

    public void OnExecute(Enemy enemy)
    {
        enemy.FindState();
    }

    public void OnExit(Enemy enemy)
    {

    }
}

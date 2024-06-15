namespace Mod.AStar.Heuristics
{
    public interface ICalculateHeuristic
    {
        int Calculate(Position source, Position destination);
    }
}
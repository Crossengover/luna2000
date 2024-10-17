namespace luna2000.Logs.Impl;

public class LogMessageGeneratorFactory : ILogMessageGeneratorFactory
{
    private readonly Dictionary<Type, ILogMessageGenerator> _generators;

    public LogMessageGeneratorFactory(IEnumerable<ILogMessageGenerator> generators)
    {
        _generators = generators.ToDictionary(g => g.GetTypeOfEntity());
    }

    public ILogMessageGenerator? GetGenerator(Type entityType)
    {
        return _generators.TryGetValue(entityType, out var generator) ? generator : null;
    }
}
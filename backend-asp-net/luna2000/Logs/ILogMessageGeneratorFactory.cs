namespace luna2000.Logs;

public interface ILogMessageGeneratorFactory
{
    ILogMessageGenerator? GetGenerator(Type entityType);
}
using QSPLib_CppWinrt;

namespace QSPEditor.Services
{
    public interface IEngineService
    {
        Engine Engine { get; }
    }

    public class EngineService : IEngineService
    {
        public Engine Engine { get; private set; }

        public EngineService()
        {
            Engine = new Engine();
        }
    }
}

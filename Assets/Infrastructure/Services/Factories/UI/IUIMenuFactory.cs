namespace Infrastructure.Services.Factories.UI
{
    /// <summary>
    /// Creates ui menus
    /// </summary>
    public interface IUIMenuFactory : IService
    {
        void CreateBottomMenu();
    }
}
namespace TryCostEngine.MAUI.Services.UserPreferences;

using System.Threading.Tasks;
using Blazored.LocalStorage;

public interface IUserPreferencesService
{
    public Task SaveUserPreferences(UserPreferences userPreferences);
    public Task<UserPreferences> LoadUserPreferences();
}
    
public class UserPreferencesService : IUserPreferencesService
{
    private readonly ILocalStorageService _localStorage;
    private const string Key = "userPreferences";
        
    public UserPreferencesService(ILocalStorageService localStorage)
    {
        _localStorage = localStorage;
    }
        
    public async Task SaveUserPreferences(UserPreferences userPreferences)
    {
        await _localStorage.SetItemAsync(Key, userPreferences);
    }

    public async Task<UserPreferences> LoadUserPreferences()
    {
        return await _localStorage.GetItemAsync<UserPreferences>(Key);
    }
}
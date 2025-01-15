public class UserServices
{

    private readonly UserAccess _userAccess;

    public UserServices(UserAccess userAccess)
    {
        _userAccess = userAccess;
    }

    public async Task<User> GetUser(string email, string password)
    {
        List<User> users = await _userAccess.GetAll();
        return users.First(user => user.Email == email && user.Password == password);
    }

    public async Task<User> GetUser(int id)
    {
        List<User> users = await _userAccess.GetAll();
        return users.First(user => user.Id == id);
    }

    public async Task<bool> DoesUserExist(string email)
    {
        List<User> users = await _userAccess.GetAll();
        if (users.Any(user => user.Email == email)) return true;
        return false;
    }

    public async Task<bool> SaveUser(User user)
    {
        try
        {
            if (await DoesUserExist(user.Email!)) return false;
            return await _userAccess.Add(user);
        }
        catch
        {
            return false;
        }
    }
}
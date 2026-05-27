

using GeneralElementsUI.Entities;
using User = DB.Entities.User;

namespace GenericLibraryUtil;

public class Fields
{
    public static List<GeneralElementsUI.Entities.UserField> GetLoginFields()
    {
        return
        [
            new GeneralElementsUI.Entities.UserLoginField(),
            new GeneralElementsUI.Entities.UserPasswordField()
        ];
    }

    public static List<GeneralElementsUI.Entities.UserField> GetRegisterFields()
    {
        var loginField = new UserLoginField();
        var passwordField = new UserPasswordField();
        var repeatPasswordField = new UserRepeatPasswordField(passwordField);
        var addressField = new UserAddressField();

        return [loginField, passwordField, repeatPasswordField, addressField];
    }
}

public class Converter
{
    public static DB.Entities.User Convert(GeneralElementsUI.Entities.User user)
    {
        var res = new DB.Entities.User
        {
            Role = User.UserRole.Client
        };
        foreach (var field in user.Fields)
        {
            if (field is UserLoginField login) res.Login = login.Value;
            if (field is UserPasswordField password) res.Password =  password.Value;
            if (field is UserAddressField address)  res.Address = address.Value;
        }

        return res;
    }
}

public class UserAddressField : UserField
{
    public override string Name { get; } = "Адрес";
    public override string Value { get; set; }
    public override FieldType Type { get; } = FieldType.Text;
    public override bool IsRequired { get; set; } = true;
}
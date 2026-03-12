using System;
using System.Collections.Generic;
using System.Linq;

class PasswordException : Exception
{
    public PasswordException(string message) : base(message) { }
}

class LengthPasswordException : PasswordException
{
    public LengthPasswordException(string msg) : base(msg) { }
}

class LetterPasswordException : PasswordException
{
    public LetterPasswordException(string msg) : base(msg) { }
}

class LowerPasswordException : PasswordException
{
    public LowerPasswordException(string msg) : base(msg) { }
}

class UpperPasswordException : PasswordException
{
    public UpperPasswordException(string msg) : base(msg) { }
}

class SpacePasswordException : PasswordException
{
    public SpacePasswordException(string msg) : base(msg) { }
}

class SpecialPasswordException : PasswordException
{
    public SpecialPasswordException(string msg) : base(msg) { }
}

class CheckPassword
{
    private string password;

    public string Password
    {
        get { return password; }
        set
        {
            var errors = new List<Exception>();

            bool isLetter = false;
            bool isUpper = false;
            bool isLower = false;
            bool hasSpace = false;
            bool hasSpecial = false;

            foreach (char c in value)
            {
                if (char.IsLetter(c)) isLetter = true;
                if (char.IsUpper(c)) isUpper = true;
                if (char.IsLower(c)) isLower = true;
                if (char.IsWhiteSpace(c)) hasSpace = true;
                if (!char.IsLetterOrDigit(c) && !char.IsWhiteSpace(c)) hasSpecial = true;
            }

            if (value.Length <= 6)
                errors.Add(new LengthPasswordException("Пароль должен быть больше 6 символов"));

            if (!isLetter)
                errors.Add(new LetterPasswordException("Внутри пароля должна быть хотя бы одна буква"));

            if (!isUpper)
                errors.Add(new UpperPasswordException("Внутри пароля должна быть хотя бы одна буква высокого регистра"));

            if (!isLower)
                errors.Add(new LowerPasswordException("Внутри пароля должна быть хотя бы одна буква низкого регистра"));

            if (!hasSpace)
                errors.Add(new SpacePasswordException("Внутри пароля должен быть пробел"));

            if (!hasSpecial)
                errors.Add(new SpecialPasswordException("Внутри пароля должен быть специальный символ"));

            if (errors.Any())
                throw new AggregateException("Пароль не прошёл проверку", errors);

            password = value;
        }
    }

    public CheckPassword(string password)
    {
        Password = password;
    }
}

class Program
{
    static void Main()
    {
        try
        {
            var check = new CheckPassword("123123");
            Console.WriteLine("Ваш пароль прошёл все проверки");
        }
        catch (AggregateException ex)
        {
            Console.WriteLine(ex.Message);

            foreach (var err in ex.InnerExceptions)
            {
                Console.WriteLine("- " + err.Message);
            }
        }
        finally
        {
            Console.WriteLine("Пароль был обработан");
        }
    }
}

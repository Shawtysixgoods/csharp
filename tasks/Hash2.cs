using System.Security.Cryptography;
using System.Text;

abstract class HashKit {
    protected HashAlgorithm Algo {get; set;}
    protected byte[] getBytes(string word) => Encoding.UTF8.GetBytes(word);
    protected string getString(byte[] bytes) => Convert.ToBase64String(bytes);
    protected byte[] getHash(byte[] bytes) => Algo.ComputeHash(bytes);
    public string GetHashWord(string word) => getString(getHash(getBytes(word)));
}

class MD5Kit : HashKit {
    public MD5Kit() => this.Algo = MD5.Create();
    public override string ToString() => "MD5 Algorithm";
}
class SHA1Kit : HashKit {
    public SHA1Kit() => this.Algo = SHA1.Create();
    public override string ToString() => "SHA1 Algorithm";
}
class SHA256Kit : HashKit {
    public SHA256Kit() => this.Algo = SHA256.Create();
    public override string ToString() => "SHA256 Algorithm";
}



class HashManager {
    public HashKit GetHashAlgorithm(int choice) {
        return choice switch
        {
            1 => new MD5Kit(),
            2 => new SHA1Kit(),
            3 => new SHA256Kit(),
            _ => throw new ArgumentException("Неверный выбор алгоритма")
        };
    }
}
class Program{
    static void Main()
    {
        Console.WriteLine("Выберите:\n\n1. MD5\n2. SHA1\n3. SHA256");
        int choice = Convert.ToInt32(Console.ReadLine());
        Console.Write("Введите строку: ");
        string word = Console.ReadLine();

        HashManager manager = new HashManager();
        HashKit newHashAlgorithm = manager.GetHashAlgorithm(choice);
        string hash = newHashAlgorithm.GetHashWord(word);

        Console.WriteLine(hash);
        Console.WriteLine(newHashAlgorithm);
    }
}

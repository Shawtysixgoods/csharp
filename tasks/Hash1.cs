using System.Security.Cryptography;
using System.Text;

class HashKit(HashAlgorithm algo) {
    protected HashAlgorithm Algo {get; set;} = algo;
    protected byte[] getBytes(string word) => Encoding.UTF8.GetBytes(word);
    protected string getString(byte[] bytes) => Convert.ToBase64String(bytes);
    protected byte[] getHash(byte[] bytes) => Algo.ComputeHash(bytes);
    protected string GetHashWord(string word) => getString(getHash(getBytes(word)));
}

class HashManager {
    public HashKit GetHashAlgorithm(int choice) {
        return choice switch
        {
            1 => new HashKit(MD5.Create),
            2 => new HashKit(SHA1.Create),
            3 => new HashKit(SHA256.Create),
            _ => throw new ArgumentException("Неверный выбор алгоритма")
        };
    }
}
class Program{
    static void Main()
    {
        Console.WriteLine("Выберите:\n\n1. MD5\n2. SHA1\n3. SHA256");
        int choice = Parse.Int(Console.ReadLine());
        Console.Write("Введите строку: ");
        string word = Console.ReadLine();

        HashManager manager = new HashManager();
        HashKit newHashAlgorithm = manager.GetHashAlgorithm(choice);
        string hash = newHashAlgorithm.GetHashCode(word);

        Console.WriteLine(hash);
    }
}

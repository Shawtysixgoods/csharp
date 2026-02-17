/// <summary>
/// Написать анализатор файловой системы.
/// Программа анализирует попадающие файлы и перемещает их по соответсвующим папкам.
/// </summary>


// Правила для расширений файлов 
class FileRules {
    public Dictionary<string, string> GetRules() { 
        return new Dictionary<string, string>() {
            [".png"] = "Images",    [".jpeg"] = "Images",        [".jpg"] = "Images",
            [".doc"] = "Documents", [".docx"] = "Documents",     [".odt"] = "Documents",
            [".txt"] = "Documents",
            [".zip"] = "Archives",  [".rar"]  = "Archives",      [".7z"]  = "Archives",
            [".mov"] = "Videos",    [".mp4"]  = "Videos",        [".gif"] = "Videos",
        };
    }
}
// Path - Класс предназначенный для работы с путями
// File - Класс предназначенный для работы с файлами
// Directory - Класс предназначенный для работы с папками

// Main -> (root) => FileOrganizer = (определяет все файлы) = FileClassify, FileMover

// Различие и определения файла (то куда он попадёт)
class FileClassify {

    private FileRules rules;

    public FileClassify(FileRules rules)
    {
        this.rules = rules;
    }

    public string? Classify(string source)
    {   // Получить расширение файла
        var ext = Path.GetExtension(source);

        // Понять, работает ли наша программа с этим расширением
        // 1. Понять, работает ли программа с этим расширением
        // 2. Если да, создать переменную, которая содержит имя папки ( в которую попадёт наш файл )
        // 3. Иначе, если наша программа с таким расширением не работает, вернуть null

        // Получаем описанные правила, а далее создаём переменную.
        // 
        rules.GetRules().TryGetValue(ext, out string? folder);

        return folder;
        // Определить в какую папку попадает текущий файл
    }
}

// За физическое перемещение файла
class FileMover {
    // Source - Абсолютный путь вместе с названием файла
    // Target - это название папки.
    private readonly string root;

    public FileMover(string root){this.root = root; }

    public void Move(string source, string target) {
        // Вычисляем абсолютный путь папки (таргета)
        var path = Path.Combine(root, target);
        Directory.CreateDirectory(path);
        // Вычисляем абсолютный путь файла, где он должен лежать
        var currentPathFile = Path.Combine(path, Path.GetFileName(source));
        Console.WriteLine(currentPathFile);
        // Физически перемещаем файл
        File.Move(source, currentPathFile);
    }

}

class FileOrganizer { 

    private readonly FileClassify fileClassify;

    private readonly FileMover fileMover;

    private readonly string root;

    public FileOrganizer(string root)
    {
        this.fileClassify = new FileClassify(new FileRules());
        this.fileMover = new FileMover(root);
        this.root = root;
    }

    public (int, int) Run()
    {
        var files = Directory.GetFiles(root);

        int skipped = 0;
        int moved = 0;

        foreach (var file in files)
        {
            
            var target = fileClassify.Classify(file); // классифицируем
            if(target != null)
            {
                fileMover.Move(file, target); // Перемещаем
                moved++;
            } else {  skipped++; }
        }

        return (skipped, moved);

    }

}

class Program
{
    static void Main()
    {
        var dir = "C:\\Users\\dyubanov_b\\Downloads\\";


        var organizer = new FileOrganizer(dir);
        (var skipped, var moved) = organizer.Run();

        Console.WriteLine($"Было пропущено {skipped} файлов");
        Console.WriteLine($"Было перемещено {moved} файлов");
    }
}

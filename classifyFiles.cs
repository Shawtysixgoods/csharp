// Цель: Анализатор файловой системы
// Программа работает в определенной папке.
// Когда в папку попадает какой-то файл, программа распознает файл 
// И отправляет в определенную папку. 

// Вход: подаются файлы
// Выход: Отсортированные по папкам файлы


// Поставить правила, с какими файлами мы работаем
// Различать и понимать какой файл к нам попал
// Физически переместить
// Должен быть тот, который будет управлять всем этим процессам

// FileRules - правила для файлов, с которыми мы работаем
// FileClassify - он отвечает за различие файла, и куда он попадёт
// FileMover - он отвечает за физическое перемещение файла
// FileOrganizer - он отвечает за то, чтобы произвести работу над каждым файлом
 
class FileRules 
{
    // Задаём правила на вход, с какими файлами работаем
    // И куда попадёт каждый файл
    public Dictionary<string, string> GetRules()
    {
        return new Dictionary<string, string>()
        {
            [".png"] = "Images", [".jpeg"] = "Images", [".jpg"] = "Images",
            [".doc"] = "Documents", [".odt"] = "Documents", [".txt"] = "Documents",
            [".zip"] = "Archive", [".7z"] = "Archive", [".rar"] = "Archive",
            [".mp4"] = "Videos", [".mov"] = "Videos", [".gif"] = "Videos"
        };
    }
}
// Path - Класс, для работы с путями
// File - Класс, для работы с файлами
// Directory - Класс, для работы с папками

// FileClassify - отвечает на распознавание, что за файл и куда попадёт
class FileClassify
{
    // За классификацию (понять, что за файл) и куда он попадёт
    private FileRules rules;
    public FileClassify(FileRules rules){ this.rules = rules; }
    public string? Classify(string source)
    {
        string ext = Path.GetExtension(source);
        // 1. Получить наши правила и сравнить, то расширение, которое есть
        // С тем расширением, которое мы описались в правилах.
        // 2. Если расширение в правилах указано - должны получить 
        // и возвратить название папки, иначе вернуть - отсутствие правила

        rules.GetRules().TryGetValue(ext, out string? Folder);
        return Folder;
    }
}
// Создаёт папку в которую должен попасть файл и физически его перемещает
class FileMover
{   // root - папка, в которой происходит работа программы
    private string root; 

    public FileMover(string root){ this.root = root; }
    // source - это абсолютный путь файла, который мы должны переместить
    // target - это название папки, куда он должен попасть source
    public void Move(string source, string target)
    {
        // Сначала находим абсолютный путь папки, которую мы хотим создать
        string path = Path.Combine(root, target);
        // Создаём папку (если она есть, инструкция пропускается)
        Directory.CreateDirectory(path);
        // Я вычисляю имя файла (с расширением)
        string filename = Path.GetFileName(source);
        // Формирую абсолютный путь папки с названием файла
        string pathFolder = Path.Combine(path, filename);
        // Физически перемещаю файл
        File.Move(source, pathFolder);
    }
}

class FileOrganizer
{
    private FileClassify classify;
    private FileMover mover;
    private string root;
    // Создаём объекты только при создании объекта FileOrganizer
    public FileOrganizer(string dir, FileRules rules)
    {
        root = dir;
        classify = new FileClassify(rules);
        mover = new FileMover(dir);
    }
    public (int, int) Run() // Запускаем процесс классификации и перемещения файлов
    {
        var files = Directory.GetFiles(root);

        int skipped = 0, moved = 0;

        foreach (var file in files) { 
            var output = classify.Classify(file);
            if (output != null)
            {
                moved++;
                mover.Move(file, output);
            }
            else { skipped++; };
        }

        return (skipped, moved);
    }
}

class Program
{
    static void Main()
    {
        var dir = Directory.GetCurrentDirectory();
        var organizer = new FileOrganizer(dir, new FileRules());

        (int skipped, int moved) = organizer.Run();

        Console.WriteLine($"Перемещено: {moved}, Пропущено: {skipped}");
    }
}

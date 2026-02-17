
// Main <-- FileOrganizer = FileRules, FileClassify, FileMover

using Microsoft.VisualBasic;

/// <summary>
/// 1. Вычислить правила, по которым мы определяем куда попадёт конкретный файл
/// 2. Вычислить пути, куда попадут файлы (физическое перемещение)
/// 3. Статистика, то куда и сколько файлов было перенесено
/// 4. Постоянная работа программы, так чтобы 
/// она реагировала на попадание новых файлов в директорию
/// </summary>
class FileRules
{
    public Dictionary<string, string> GetRules()
    {
        return new Dictionary<string, string>()
        {
            [".png"] = "Images", [".jpeg"] = "Images", [".jpg"] = "Images",
            [".mov"] = "Videos", [".mp4"] = "Videos", [".gif"] = "Videos",
            [".rar"] = "Archives", [".zip"] = "Archives", [".7z"] = "Archives",
            [".doc"] = "Documents", [".docx"] = "Documents", [".odt"] = "Documents",
            [".txt"] = "Documents",
        };
    }
}

// Инструмент для понимания, что за файл и куда он попадёт
class FileClassify
{
    // 1. Получить расширение файла
    // 2. Если файл не поддерживается - пропустить его и вернуть null
    // 3. Если файл поддерживается - возвращаем его название
    
    private readonly FileRules _rules;

    public FileClassify(FileRules rules) => _rules = rules;

    public string? Classify(string path)
    {
        string ext = Path.GetExtension(path).ToLower();
        bool found = _rules.GetRules().TryGetValue(ext, out string? folder);

        return found ? folder : null;
    }
}

class FileMover
{
    private readonly string root;
    public FileMover(string root) => this.root = root;

    // 1. Определить абсолютный путь 
    // 2. Создать соответствующую папку 
    // 3. Физически переместить файл в папку
    public void Move(string source, string target)
    {
        string path = Path.Combine(root, target); // Конкретная папка для файла
        Directory.CreateDirectory(path);

        // Если у нас в Donwloads есть файл точно таким же названием
        // Как и в папке Videos, то мы должны для нового файла добавлять что-то в название

        string currentFilePath = Path.Combine(path, Path.GetFileName(source));

        if (File.Exists(currentFilePath))
        {
            string filename = Path.GetFileNameWithoutExtension(currentFilePath);
            string extension = Path.GetExtension(currentFilePath);

            int count = 0;

            do
            {
                count++;
                currentFilePath = Path.Combine(path, filename + $" ({count})" + extension);
            } while (File.Exists(currentFilePath));

            File.Move(source, currentFilePath);
        }
    }

    class FileOrganizer
    {
        private readonly FileClassify _classify;

        private readonly FileMover _mover;


        public FileOrganizer(FileRules rules, string rootDir)
        {
            _classify = new FileClassify(rules);

            _mover = new FileMover(rootDir);
        }

        public (int moved, int skipped) Run(string dir)
        {
            // Получаем все файлы
            // Небольшую статистику
            // Классифицировать каждый файл
            // Перемещать файл физически
            // Вернуть данные обратно

            var files = Directory.GetFiles(dir);

            int moved = 0;
            int skipped = 0;

            foreach (var file in files)
            {
                var folder = _classify.Classify(file);

                if (folder != null)
                {
                    _mover.Move(file, folder);
                    moved++;
                }
                else
                {
                    skipped++;
                }
            }

            return (moved, skipped);
        }
    }


    class Program
    {


        static void Main()
        {
            string dir = Directory.GetCurrentDirectory();

            var organizer = new FileOrganizer(new FileRules(), dir);

            using var watcher = new FileSystemWatcher(dir);


            watcher.Created += (s, a) =>
            {
                (var moved, var skipped) = organizer.Run(dir);
                Console.WriteLine("Перемещено: " + moved);
                Console.WriteLine("Пропущено: " + skipped);
            };

            watcher.EnableRaisingEvents = true;
            Console.ReadLine();
        }
    }
}

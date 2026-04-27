using System;
using System.IO;

/// <summary>
/// В данном задании вам нужно опираться на комментарии в коде.
/// Реализуйте всё что подписано TODO:
/// </summary>


// Интерфейс, определяющий контракт для работы с файловой системой (бизнес-логика)
public interface IFileNavigator
{
    // Свойство: Текущая папка, в которой мы сейчас находимся
    DirectoryInfo CurrentPath { get; }

    // Метод: Возвращает список всех вложенных папок
    DirectoryInfo[] GetDirectories();

    // Метод: Возвращает список всех файлов в текущей папке
    FileInfo[] GetFiles();

    // Метод: Изменяет текущий путь на вложенную папку по ее номеру из списка
    void GoToDirectory(int index);

    // Метод: Изменяет текущий путь на любой указанный абсолютный путь
    void GoToPath(string path);

    // Метод: Изменяет текущий путь на родительскую папку (на уровень выше)
    void GoUp();

    // Метод: Читает файл по его номеру и возвращает его содержимое в виде текста
    string ReadFileContent(int index);
}

// Интерфейс, определяющий контракт для визуальной части приложения
public interface IUserInterface
{
    // Главный метод: Запускает работу интерфейса (обычно это бесконечный цикл)
    void Run();
}

/* * TODO: ШАГ 1. Создайте новый класс (например, LocalFileNavigator).
 * Унаследуйте его от абстрактного класса BaseFileNavigator: 
 * public class LocalFileNavigator : BaseFileNavigator { ... }
 * Внутри нового класса вам нужно будет нажать "Реализовать абстрактный класс" 
 * и написать код для каждого метода ниже.
 */
public abstract class BaseFileNavigator : IFileNavigator
{
    public DirectoryInfo CurrentPath { get; protected set; }

    // Конструктор базового класса (уже реализован, трогать не нужно)
    protected BaseFileNavigator(string startPath)
    {
        if (string.IsNullOrWhiteSpace(startPath) || !Directory.Exists(startPath))
        {
            throw new ArgumentException("Указан неверный стартовый путь.");
        }
        CurrentPath = new DirectoryInfo(startPath);
    }

    // TODO: Реализуйте метод в классе-наследнике. 
    // Он должен возвращать массив папок: return CurrentPath.GetDirectories();
    public abstract DirectoryInfo[] GetDirectories();

    // TODO: Реализуйте метод в классе-наследнике. 
    // Он должен возвращать массив файлов: return CurrentPath.GetFiles();
    public abstract FileInfo[] GetFiles();

    // TODO: Реализуйте метод в классе-наследнике. 
    // Получите папки (GetDirectories), проверьте, что переданный index не выходит за границы массива,
    // и присвойте свойству CurrentPath новую выбранную папку.
    public abstract void GoToDirectory(int index);

    // TODO: Реализуйте метод в классе-наследнике. 
    // Проверьте существование пути через Directory.Exists(path). 
    // Если он существует, обновите CurrentPath новым объектом DirectoryInfo.
    public abstract void GoToPath(string path);

    // TODO: Реализуйте метод в классе-наследнике. 
    // Проверьте, что CurrentPath.Parent != null. Если родитель есть, перейдите в него.
    public abstract void GoUp();

    // TODO: Реализуйте метод в классе-наследнике. 
    // Найдите файл по индексу в массиве GetFiles(). Прочитайте его текст через File.ReadAllText().
    // ОБЯЗАТЕЛЬНО оберните чтение в try-catch, чтобы программа не падала при ошибке доступа,
    // и возвращайте текст ошибки, если прочитать не удалось.
    public abstract string ReadFileContent(int index);
}


/* * TODO: ШАГ 2. Создайте новый класс (например, ConsoleTerminal).
 * Унаследуйте его от абстрактного класса BaseConsoleUI: 
 * public class ConsoleTerminal : BaseConsoleUI { ... }
 * Внутри нового класса реализуйте визуальную часть интерфейса.
 */
public abstract class BaseConsoleUI : IUserInterface
{
    protected readonly IFileNavigator Navigator;

    // Конструктор базового класса (уже реализован, трогать не нужно)
    protected BaseConsoleUI(IFileNavigator navigator)
    {
        Navigator = navigator;
    }

    // Главный цикл программы (уже реализован, трогать не нужно)
    public void Run()
    {
        while (true)
        {
            ClearScreen();
            DrawHeader();
            DrawMenu();

            if (!ProcessInput())
            {
                break;
            }
        }
    }

    // TODO: Реализуйте метод в классе-наследнике. 
    // Напишите внутри просто: Console.Clear();
    protected abstract void ClearScreen();

    // TODO: Реализуйте метод в классе-наследнике. 
    // Выведите на экран текущий путь: Navigator.CurrentPath.FullName
    // Затем запросите папки через Navigator.GetDirectories() и выведите их на экран циклом с нумерацией.
    protected abstract void DrawHeader();

    // TODO: Реализуйте метод в классе-наследнике. 
    // Просто выведите текст меню (например: 1. Открыть папку, 2. Прочитать файл, 0. Выход).
    protected abstract void DrawMenu();

    // TODO: Реализуйте метод в классе-наследнике. 
    // Считайте ввод пользователя (Console.ReadLine). Через switch/case вызовите нужные 
    // методы Navigator (GoToDirectory, GoUp и т.д.). 
    // Метод должен возвращать true, чтобы цикл продолжался. Верните false ТОЛЬКО если пользователь выбрал "Выход".
    protected abstract bool ProcessInput();

    // TODO: Реализуйте метод в классе-наследнике. 
    // 1. Запросите список файлов через Navigator.GetFiles() и выведите их с номерами.
    // 2. Попросите пользователя ввести номер файла.
    // 3. Считайте номер и вызовите Navigator.ReadFileContent(номер).
    // 4. Выведите полученный текст на экран и сделайте паузу (Console.ReadKey), чтобы пользователь успел прочитать.
    protected abstract void HandleFileReading();
}








class Program {
    static void Main() {
        // TODO: ШАГ 3. Создайте объекты ваших новых классов и запустите программу.
        // Пример (раскомментировать, когда классы будут созданы):

        // string startPath = Directory.GetCurrentDirectory();
        // IFileNavigator myNavigator = new LocalFileNavigator(startPath);
        // IUserInterface myUI = new ConsoleTerminal(myNavigator);
        // myUI.Run();
    }
}

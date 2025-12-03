using System;
using System.Collections.Generic;
using System.Text;

namespace HtmlBuilderPattern
{
    // 1. PRODUCT: Клас, що представляє складний об'єкт
    public class HtmlDocument
    {
        public string Title { get; set; }
        public List<string> MetaTags { get; set; } = new List<string>();
        public List<string> Styles { get; set; } = new List<string>();
        public List<string> Scripts { get; set; } = new List<string>();
        public StringBuilder BodyContent { get; set; } = new StringBuilder();

        // Метод для збірки всього документу в один рядок
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("<!DOCTYPE html>");
            sb.AppendLine("<html>");
            
            // Head
            sb.AppendLine("<head>");
            sb.AppendLine($"    <title>{Title}</title>");
            foreach (var meta in MetaTags) sb.AppendLine($"    {meta}");
            if (Styles.Count > 0)
            {
                sb.AppendLine("    <style>");
                foreach (var style in Styles) sb.AppendLine($"        {style}");
                sb.AppendLine("    </style>");
            }
            sb.AppendLine("</head>");

            // Body
            sb.AppendLine("<body>");
            sb.AppendLine(BodyContent.ToString());

            // Scripts (зазвичай в кінці body)
            if (Scripts.Count > 0)
            {
                sb.AppendLine("    <script>");
                foreach (var script in Scripts) sb.AppendLine($"        {script}");
                sb.AppendLine("    </script>");
            }
            sb.AppendLine("</body>");
            sb.AppendLine("</html>");

            return sb.ToString();
        }
    }

    // 2. BUILDER INTERFACE: Визначає методи для налаштування частин продукту
    public interface IHtmlBuilder
    {
        void SetTitle(string title);
        void AddMetaTag(string name, string content);
        void AddStyle(string cssRule);
        void AddScript(string jsCode);
        void AddToBody(string htmlTag);
        
        // Метод для отримання результату
        HtmlDocument GetDocument();
    }

    // 3. CONCRETE BUILDER: Реалізує кроки побудови
    public class MyHtmlBuilder : IHtmlBuilder
    {
        private HtmlDocument _document = new HtmlDocument();

        public void SetTitle(string title)
        {
            _document.Title = title;
        }

        public void AddMetaTag(string name, string content)
        {
            _document.MetaTags.Add($"<meta name=\"{name}\" content=\"{content}\">");
        }

        public void AddStyle(string cssRule)
        {
            _document.Styles.Add(cssRule);
        }

        public void AddScript(string jsCode)
        {
            _document.Scripts.Add(jsCode);
        }

        public void AddToBody(string htmlTag)
        {
            _document.BodyContent.AppendLine($"    {htmlTag}");
        }

        public HtmlDocument GetDocument()
        {
            HtmlDocument result = _document;
            // Скидаємо будівельник для можливості створення нового об'єкта (опціонально)
            _document = new HtmlDocument(); 
            return result;
        }
    }

    // 4. CLIENT: Використання
    class Program
    {
        static void Main(string[] args)
        {
            // Створюємо екземпляр будівельника
            IHtmlBuilder builder = new MyHtmlBuilder();

            Console.WriteLine("=== Створення HTML документу ===");

            // Наповнюємо документ частинами
            builder.SetTitle("Моя C# Сторінка");
            builder.AddMetaTag("description", "Приклад патерну Builder");
            builder.AddMetaTag("author", "Gemini User");
            
            builder.AddStyle("body { font-family: Arial, sans-serif; background-color: #f0f0f0; }");
            builder.AddStyle("h1 { color: navy; }");
            
            builder.AddToBody("<h1>Привіт, світ!</h1>");
            builder.AddToBody("<p>Цей HTML згенеровано за допомогою патерну Builder на C#.</p>");
            builder.AddToBody("<button onclick=\"showAlert()\">Натисни мене</button>");

            builder.AddScript("function showAlert() { alert('Працює!'); }");

            // Отримуємо готовий об'єкт
            HtmlDocument doc = builder.GetDocument();

            // Виводимо результат
            Console.WriteLine(doc.ToString());

            // Опціонально: Збереження у файл
            // System.IO.File.WriteAllText("index.html", doc.ToString());
            
            Console.ReadLine();
        }
    }
}
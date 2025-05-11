using System;

namespace NoteApp
{
    public class Note
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime Date { get; set; }

        public Note(string title, string content)
        {
            Title = title;
            Content = content;
            Date = DateTime.Now;
        }

        public Note(string title, string content, DateTime date) : this(title, content)
        {
            Date = date; // Явно присваиваем Date из параметра
        }
    }
}
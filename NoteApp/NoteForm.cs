using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NoteApp
{
    public partial class NoteForm : Form
    {
        private NoteManager noteManager;
        private TextBox titleTextBox;
        private TextBox contentTextBox;
        private Button addNoteButton;
        private ListBox notesListBox;
        private Button removeNoteButton;

        public NoteForm()
        {
            InitializeComponent(); // ВЫЗЫВАЕМ ЕГО В ПЕРВУЮ ОЧЕРЕДЬ!

            // Задаем свойства формы (можно и в дизайнере)
            this.Text = "Управление заметками";
            this.Width = 500;
            this.Height = 400;

            // Создаем и настраиваем titleTextBox
            titleTextBox = new TextBox
            {
                Location = new System.Drawing.Point(10, 10),
                Width = 200,
                Text = "Заголовок", // Изначальный placeholder
                ForeColor = Color.Gray // Изначальный цвет placeholder
            };
            titleTextBox.Enter += titleTextBox_Enter; // Подписываемся на события
            titleTextBox.Leave += titleTextBox_Leave;

            // Создаем и настраиваем contentTextBox
            contentTextBox = new TextBox
            {
                Location = new System.Drawing.Point(10, 40),
                Width = 200,
                Height = 100,
                Multiline = true,
                ScrollBars = ScrollBars.Both,
                Text = "Содержание", // Изначальный placeholder
                ForeColor = Color.Gray // Изначальный цвет placeholder
            };
            contentTextBox.Enter += contentTextBox_Enter;
            contentTextBox.Leave += contentTextBox_Leave;

            // Создаем и настраиваем addNoteButton
            addNoteButton = new Button
            {
                Location = new System.Drawing.Point(10, 150),
                Text = "Добавить",
                Width = 100
            };
            addNoteButton.Click += AddNoteButton_Click;

            // Создаем и настраиваем notesListBox
            notesListBox = new ListBox
            {
                Location = new System.Drawing.Point(220, 10),
                Width = 250,
                Height = 200
            };

            // Создаем и настраиваем removeNoteButton
            removeNoteButton = new Button
            {
                Location = new System.Drawing.Point(220, 220),
                Text = "Удалить",
                Width = 100
            };
            removeNoteButton.Click += RemoveNoteButton_Click;

            // Добавляем элементы управления на форму
            this.Controls.Add(titleTextBox);
            this.Controls.Add(contentTextBox);
            this.Controls.Add(addNoteButton);
            this.Controls.Add(notesListBox);
            this.Controls.Add(removeNoteButton);

            // Инициализируем noteManager и обновляем список заметок
            noteManager = new NoteManager();
            UpdateNotesList();
        }


        private void UpdateNotesList()
        {
            notesListBox.Items.Clear();
            foreach (var note in noteManager.Notes)
            {
                notesListBox.Items.Add($"{note.Title} ({note.Date.ToString("yyyy-MM-dd")})");
            }
        }

        private void AddNoteButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(titleTextBox.Text) || string.IsNullOrEmpty(contentTextBox.Text))
            {
                MessageBox.Show("Заполните все поля!");
                return;
            }
            Note newNote = new Note(titleTextBox.Text, contentTextBox.Text);
            try
            {
                noteManager.AddNote(newNote);
                titleTextBox.Text = "Заголовок";
                titleTextBox.ForeColor = Color.Gray;
                contentTextBox.Text = "Содержание";
                contentTextBox.ForeColor = Color.Gray;
                UpdateNotesList();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void titleTextBox_Enter(object sender, EventArgs e)
        {
            if (titleTextBox.Text == "Заголовок")
            {
                titleTextBox.Text = "";
                titleTextBox.ForeColor = Color.Black; // Сделайте текст нормальным цветом
            }
        }

        private void titleTextBox_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(titleTextBox.Text))
            {
                titleTextBox.Text = "Заголовок";
                titleTextBox.ForeColor = Color.Gray;  // Сделайте текст серым, чтобы было видно, что это placeholder
            }
        }

        private void contentTextBox_Enter(object sender, EventArgs e)
        {
            if (contentTextBox.Text == "Содержание")
            {
                contentTextBox.Text = "";
                contentTextBox.ForeColor = Color.Black; // Сделайте текст нормальным цветом
            }
        }

        private void contentTextBox_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(contentTextBox.Text))
            {
                contentTextBox.Text = "Содержание";
                contentTextBox.ForeColor = Color.Gray;  // Сделайте текст серым, чтобы было видно, что это placeholder
            }
        }

        private void RemoveNoteButton_Click(object sender, EventArgs e)
        {
            if (notesListBox.SelectedIndex == -1)
            {
                MessageBox.Show("Выберите заметку для удаления!");
                return;
            }
            string selectedItem = notesListBox.SelectedItem.ToString();
            string[] parts = selectedItem.Split(new[] { '(' }, StringSplitOptions.None);
            if (parts.Length >= 2)
            {
                string title = parts[0];
                DateTime date;
                if (DateTime.TryParse(parts[1].Split(')')[0], out date))
                {
                    var noteToRemove = noteManager.Notes.Find(n => n.Title == title && n.Date.Date == date.Date);
                    if (noteToRemove != null)
                    {
                        try
                        {
                            noteManager.RemoveNote(noteToRemove);
                            UpdateNotesList();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                    }
                }
            }
        }
    }
}

using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace University
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            // Создание 4 студентов и 2 групп изначально
            Student student1 = new Student("Bob", "Sinclar", 18, "images\\Fox.jpeg");
            Student student2 = new Student("Ann", "Smith", 17, "images\\Hummingbird.jfif");
            Student student3 = new Student("Tim", "Brown", 17, "images\\Cat.jfif");
            Student student4 = new Student("Jack", "Rassel", 18, "images\\penguin.jfif");
            Group group1 = new Group("KVO-37", new List<Student> { student1, student2 });
            Group group2 = new Group("KVO-40", new List<Student> { student3, student4 });
            Groups.Items.Add(group1);
            Groups.Items.Add(group2);
            
        }

        private void Groups_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // При выборе группы - очищается ListBox Students и добавляются студенты соответствующей группы
            Students.Items.Clear();
            if (Groups.SelectedItem != null)
            {
                Group group = Groups.SelectedItem as Group;
                for (int i = 0; i < group.Students.Count; i++)
                {
                    Students.Items.Add(group.Students[i]);
                }
            }
        }

        private void Students_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // При выборе студента отображается информация в Info + фото по умолчанию, если ссылка не введена
            if (Students.SelectedItem != null)
            {
                Student student = Students.SelectedItem as Student;

                NameField.Text = student.Name;
                LastnameField.Text = student.LastName;
                AgeField.Text = (student.Age).ToString();

                if (!string.IsNullOrEmpty(student.LinkPhoto))
                {
                    Photo.Source = BitmapFrame.Create(new Uri(student.LinkPhoto, UriKind.Relative));
                }
                else
                    Photo.Source = BitmapFrame.Create(new Uri("Photo.png", UriKind.Relative));

                // При выборе студента изменяется Progress Info

                if (!string.IsNullOrWhiteSpace(student.LinkProgress))
                    StudentINFO.Text = File.ReadAllText(student.LinkProgress);
                else
                    StudentINFO.Text = "";

            }
        }

        private void AddGroupButton_Click(object sender, RoutedEventArgs e)
        {
            // Добавление группы
            if (!string.IsNullOrEmpty(GroupNameField.Text))
            {
                Group newGroup = new Group(GroupNameField.Text);
                Groups.Items.Add(newGroup);
                GroupNameField.Text = "";
            }
        }

        private void Create_Click(object sender, RoutedEventArgs e)
        {
            // Добавление студента
            // Выход, если группа предварительно не выбрана
            if (Groups.SelectedItem == null)
            {
                MessageBox.Show("Choice the group", "Add Students", MessageBoxButton.OK);
                StudentNameField.Text = "";
                StudentLastnameField.Text = "";
                StudentAgeField.Text = "";
                PhotoLinkTB.Text = "";
                return;
            }
            // Если строки ФИО заполнениы - добавление студента
            if (!string.IsNullOrEmpty(StudentNameField.Text) && !string.IsNullOrEmpty(StudentLastnameField.Text))
            {
                // Проверка введенного возраста
                int age = 0;
                if (int.TryParse(StudentAgeField.Text, out int result))
                {
                    if (result < 12 || result > 65)
                    {
                        MessageBox.Show("Enter correct age", "Error", MessageBoxButton.OK);
                        StudentAgeField.Text = "";
                        return;
                    }
                    age = result;
                }
                else
                {
                    MessageBox.Show("Enter correct age", "Error", MessageBoxButton.OK);
                    StudentAgeField.Text = "";
                    return;
                }


                Student newStudent = new Student(StudentNameField.Text, StudentLastnameField.Text, age, PhotoLinkTB.Text);
                newStudent.LinkProgress = ProgressLinkTB.Text; 
                ((Group)Groups.SelectedItem).Students.Add(newStudent);
                Students.Items.Add(newStudent);
                StudentNameField.Text = "";
                StudentLastnameField.Text = "";
                StudentAgeField.Text = "";
                PhotoLinkTB.Text = "";
            }

            // Обновление ListBox с группами (для отображения актуального кол-ва студентов после добавления)
            Group newGroup = Groups.SelectedItem as Group;
            for (int i = 0; i < Groups.Items.Count; i++)
            {
                if (Groups.Items[i] == Groups.SelectedItem)
                {
                    Groups.Items.RemoveAt(i);
                }
            }
            Groups.Items.Add(new Group(newGroup.Name, newGroup.Students));

            // Пересоздание ListBox Students
            for (int i = 0; i < newGroup.Students.Count; i++)
            {
                Students.Items.Add(newGroup.Students[i]);
            }

        }

        private void AddPhoto_Click(object sender, RoutedEventArgs e)
        {
            // Выбор файла для фото в диалоговом окне
            OpenFileDialog myFileDialog = new OpenFileDialog();
            myFileDialog.Filter = "Images (*.png)|*.png; |Images (*.jpg)| *.jpg; |Images (*.jpeg)| *.jpeg; |Images (*.jfif)| *.jfif; |All files|*.*; |TXT (*.txt)| *.txt";

            if (myFileDialog.ShowDialog() == true)
            {
                PhotoLinkTB.Text = myFileDialog.FileName;
            }
            else
            {
                PhotoLinkTB.Text = "";
            }
        }

        private void AddFile_Click(object sender, RoutedEventArgs e)
        {
            // Выбор файла в диалоговом окне
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Filter = "File (*.txt)|*.txt";

            if (fileDialog.ShowDialog() == true)
            {
                // Отображение пути файла в текстблоке
                ProgressLinkTB.Text = fileDialog.FileName;
                       
                // Вывод текста файла в текстбокс StudentINFO
                StudentINFO.Text = File.ReadAllText(fileDialog.FileName);

                // Вывод среднего балла в текстблок Average
                string info = File.ReadAllText(fileDialog.FileName);
                string[] stringsOfInfo = info.Split(";", StringSplitOptions.RemoveEmptyEntries); // Убирает пустые подстроки
                List<int> marks = new List<int>();
                foreach (var item in stringsOfInfo)
                {
                    marks.Add(int.Parse(item.Split("-")[1]));
                }
                Average.Text = marks.Average().ToString();
            } 
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            //File.WriteAllText(путь, "Hello EveryOne!");

            // Создание новой строки для файла с оценками
            string newInfo = $"Math -{int.Parse(Math.Text)};\nC# -{int.Parse(CSh.Text)};\n" +
                $"Admin -{int.Parse(Admin.Text)};\nC++ -{int.Parse(Cpl.Text)}";

           
            foreach (Student item in Students.Items)
            {
                if(item == Students.SelectedItem)
                {
                    File.WriteAllText(item.LinkProgress, newInfo);
                    StudentINFO.Text = File.ReadAllText(item.LinkProgress);
                    Average.Text = ((int.Parse(Math.Text) + int.Parse(CSh.Text) + int.Parse(Admin.Text) + int.Parse(Cpl.Text)) / 4).ToString();
                }
            }

            // Очищение оценок в текстбоксах
            Math.Text = "";
            CSh.Text = "";
            Admin.Text = "";
            Cpl.Text = "";
        }
    }

    class Student
    {
        public Student(string name, string lastName, int age, string linkPhoto)
        {
            Name = name;
            LastName = lastName;
            Age = age;
            LinkPhoto = linkPhoto;
        }

        public string Name { get; set; }
        public string LastName { get; set; }
        public int? Age { get; set; }
        public string? LinkPhoto { get; set; }

        public string? LinkProgress { get; set; }   

        public override string ToString()
        {
            return $"{Name} {LastName}, age: {Age}";
        }

    }

    class Group
    {
        public Group(string name)
        {
            Name = name;
            Students = new List<Student>();
        }

        public Group(string name, List<Student> students)
        {
            Name = name;
            Students = students;
        }

        public string Name { get; set; }
        public List<Student> Students { get; set; }


        public override string ToString()
        {
            return $"{Name}\t\t{Students.Count()} students";
        }
    }
}

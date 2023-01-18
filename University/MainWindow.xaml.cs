using System;
using System.Collections.Generic;
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

            Student student1 = new Student("Bob", "Sinclar", 18, "D:\\Настя\\C#\\University\\University\\Fox.jpeg");
            Student student2 = new Student("Ann", "Smith", 17, "D:\\Настя\\C#\\University\\University\\Hummingbird.jfif");
            Student student3 = new Student("Tim", "Brown", 17, "D:\\Настя\\C#\\University\\University\\Cat.jfif");
            Student student4 = new Student("Jack", "Rassel", 18, "D:\\Настя\\C#\\University\\University\\penguin.jfif");
            Group group1 = new Group("KVO-37", new List<Student> { student1, student2 });
            Group group2 = new Group("KVO-40", new List<Student> { student3, student4 });
            Groups.Items.Add(group1);
            Groups.Items.Add(group2);   
        }

        private void Groups_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
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
            if(Students.SelectedItem != null)
            {
                Student student = Students.SelectedItem as Student;

                NameField.Text = student.Name;
                LastnameField.Text = student.LastName;
                AgeField.Text = (student.Age).ToString();
                if(student.LinkPhoto != null)
                {
                    Photo.Source = BitmapFrame.Create(new Uri(student.LinkPhoto));
                }
                else
                {
                    Photo.Source = BitmapFrame.Create(new Uri("D:\\Настя\\C#\\University\\University\\Photo.png"));
                }
            }
        }
    }

    class Student
    {
        public Student(string name, string lastName, int age)
        {
            Name = name;
            LastName = lastName;
            Age = age;
        }

        public Student(string name, string lastName, int age, string linkPhoto)
        {
            Name = name;
            LastName = lastName;
            Age = age;
            LinkPhoto = linkPhoto;
        }

        public string Name { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
        public string? LinkPhoto { get; set; }

        public override string ToString()
        {
            return $"{Name}, {LastName}, age: {Age}";    
        }

    }

    class Group
    {
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

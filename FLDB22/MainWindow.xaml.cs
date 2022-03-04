using FLDB22.Models;
using FLDB22.Repostories;
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

namespace FLDB22
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            var db = new DbRepository();
            var people = new List<Person>();

            var person = new Person()
            {
                 Firstname="Arne",
                 Lastname = "Anka"
            };
            people.Add(person);

            person = new Person()
            {
                Firstname = "Stina",
                Lastname = "Gås"
            };
            people.Add(person);
            var persons =  db.GetPersons();

            try
            {
                var newPeople = db.AddPersons(people);
                db.AddPerson();
                //var person = db.GetPersonById(22);
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }
    }
}

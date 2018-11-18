using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using MySql.Data.MySqlClient;
using ProjetSimulateurImpot.Config;

namespace ProjetSimulateurImpot
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string devise = " Fcfa";
        public MainWindow()
        {
            InitializeComponent();
            initialiserImpots();
            DispatcherTimer timer = new DispatcherTimer(new TimeSpan(0, 0, 1), DispatcherPriority.Normal, delegate
            {
                CDate.Content = "Date: " + DateTime.Now.ToString();
            }, this.Dispatcher);
            try
            {
                MySqlConnection connection = DBUtils.GetDBConnection();
                connection.Open();
                connection.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show("Erreur lors de la connecion à la base de données.", e.Message);
                this.Close();
            }
        }

        private void BCalculer_Click(object sender, RoutedEventArgs e)
        {
            if (sender == BCalculer)
            {
                string messageBoxTitle = "Erreur de saisie, valeur entière attendue.", nom = CNom.Text, prenom = CPrenom.Text;
                if (!int.TryParse(CSalaireBrut.Text, out int salaireBrut))
                {
                    MessageBox.Show("Le champs 'Salaire Brut' doit etre un entier.", messageBoxTitle);
                    return;
                }
                if (!int.TryParse(CConjoint.Text, out int conjoints))
                {
                    MessageBox.Show("Le champs 'Conjoint' doit etre un entier.", messageBoxTitle);
                    return;
                }
                if (!int.TryParse(CNombreDeJours.Text, out int nombreDejours))
                {
                    MessageBox.Show("Le champs 'Nombre De Jours' doit etre un entier.", messageBoxTitle);
                    return;
                }
                if (!int.TryParse(CEnfants.Text, out int enfants))
                {
                    MessageBox.Show("Le champs 'Enfants' doit etre un entier.", messageBoxTitle);
                    return;
                }
                try
                {
                    Employe employe = new Employe(nom, prenom, salaireBrut, nombreDejours, conjoints, enfants);
                    employe.CalculerImpots();
                    bool stockageEmploye = QueryData.InsererEmploye(employe);
                    if (stockageEmploye)
                    {
                        CBrutFiscalAnnuel.Content = string.Copy(employe.BrutFiscalAnnuel + devise);
                        CBrutFiscalApresAbattement.Content = string.Copy(employe.BrutFiscalApresAbattement + devise);
                        CIRPPAvantReduction.Content = string.Copy(employe.IRPPAvantReduction + devise);
                        CAbattement.Content = string.Copy(employe.Abattement + devise);
                        CNombreDeParts.Content = string.Copy(employe.NombreDeParts + " parts");
                        CReduction.Content = string.Copy(employe.Reduction + devise);
                        CImpots.Content = string.Copy(employe.Impot + devise);
                        MessageBox.Show("L'utilisateur a été enregistré à la base de données avec succès.");
                    }
                    else
                    {
                        throw new ArgumentException("Erreur lors de l'enregistrement de l'utilisateur dans la base de données.");
                    }
                }
                catch (ArgumentException argExc)
                {
                    messageBoxTitle = argExc.Message;
                    MessageBox.Show(messageBoxTitle, "Des erreurs sont survenues, veuillez ressaisir le formulaire svp!\n");
                    return;
                }
            }
        }

        private void BQuitter_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void BAnnuler_Click(object sender, RoutedEventArgs e)
        {
            CNom.Text = string.Empty;
            CSalaireBrut.Text = string.Empty;
            CConjoint.Text = string.Empty;
            CPrenom.Text = string.Empty;
            CNombreDeJours.Text = string.Empty;
            CEnfants.Text = string.Empty;
            initialiserImpots();
        }

        private void initialiserImpots()
        {
            CBrutFiscalAnnuel.Content = string.Copy(devise);
            CBrutFiscalApresAbattement.Content = string.Copy(devise);
            CIRPPAvantReduction.Content = string.Copy(devise);
            CAbattement.Content = string.Copy(devise);
            CNombreDeParts.Content = string.Copy(" parts");
            CReduction.Content = string.Copy(devise);
            CImpots.Content = string.Copy(devise);
        }
    }
}

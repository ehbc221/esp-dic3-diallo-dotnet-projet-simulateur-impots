using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ProjetSimulateurImpot.Config
{
    class QueryData
    {
        public static bool InsererEmploye(Employe employe)
        {
            bool insertion = true;
            try
            {
                MySqlConnection connection = DBUtils.GetDBConnection();
                string commandText = "INSERT INTO employe (Nom, Prenom, SalaireBrut, NombreDeJours, Conjoint, Enfants, BrutFiscalAnnuel, BrutFiscalApresAbattement, IRPPAvantReduction, Abattement, NombreDeParts, Reduction, Impot) " +
                               "VALUES (@Nom, @Prenom, @SalaireBrut, @NombreDeJours, @Conjoint, @Enfants, @BrutFiscalAnnuel, @BrutFiscalApresAbattement, @IRPPAvantReduction, @Abattement, @NombreDeParts, @Reduction, @Impot);";
                MySqlCommand commande = connection.CreateCommand();
                commande.CommandText = commandText;
            
                commande.Parameters.AddWithValue("@Nom", employe.Nom);
                commande.Parameters.AddWithValue("@Prenom", employe.Prenom);
                commande.Parameters.AddWithValue("@SalaireBrut", employe.SalaireBrut);
                commande.Parameters.AddWithValue("@NombreDeJours", employe.NombreDeJours);
                commande.Parameters.AddWithValue("@Conjoint", employe.Conjoint);
                commande.Parameters.AddWithValue("@Enfants", employe.Enfants);
                commande.Parameters.AddWithValue("@BrutFiscalAnnuel", employe.BrutFiscalAnnuel);
                commande.Parameters.AddWithValue("@BrutFiscalApresAbattement", employe.BrutFiscalApresAbattement);
                commande.Parameters.AddWithValue("@IRPPAvantReduction", employe.IRPPAvantReduction);
                commande.Parameters.AddWithValue("@Abattement", employe.Abattement);
                commande.Parameters.AddWithValue("@NombreDeParts", employe.NombreDeParts);
                commande.Parameters.AddWithValue("@Reduction", employe.Reduction);
                commande.Parameters.AddWithValue("@Impot", employe.Impot);
            
                connection.Open();
                Int32 compteur = commande.ExecuteNonQuery();
                connection.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur BD : ", ex.Message);
                insertion = false;
            }
            return insertion;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetSimulateurImpot
{
    class Employe
    {
        private string nom;
        private string prenom;
        private double salaireBrut;
        private int nombreDeJours;
        private int conjoint;
        private int enfants;

        private double brutFiscalAnnuel;
        private double brutFiscalApresAbattement;
        private double iRPPAvantReduction;
        private double abattement;
        private double nombreDeParts;
        private double reduction;
        private double impot;

        public string Nom { get => nom; set => nom = value; }
        public string Prenom { get => prenom; set => prenom = value; }
        public double SalaireBrut { get => salaireBrut; set => salaireBrut = value; }
        public int NombreDeJours { get => nombreDeJours; set => nombreDeJours = value; }
        public int Conjoint { get => conjoint; set => conjoint = value; }
        public int Enfants { get => enfants; set => enfants = value; }
        public double BrutFiscalAnnuel { get => brutFiscalAnnuel; set => brutFiscalAnnuel = value; }
        public double BrutFiscalApresAbattement { get => brutFiscalApresAbattement; set => brutFiscalApresAbattement = value; }
        public double IRPPAvantReduction { get => iRPPAvantReduction; set => iRPPAvantReduction = value; }
        public double Abattement { get => abattement; set => abattement = value; }
        public double NombreDeParts { get => nombreDeParts; set => nombreDeParts = value; }
        public double Reduction { get => reduction; set => reduction = value; }
        public double Impot { get => impot; set => impot = value; }

        public Employe(string nom, string prenom, double salaireBrut, int nombreDeJours, int conjoint, int enfants)
        {
            int[] conjoints = { 0, 1, 2 };
            this.Nom = nom.Equals("") ? throw new ArgumentException("Le 'Nom' ne doit pas est vide.") : nom;
            this.Prenom = prenom.Equals("") ? throw new ArgumentException("Le 'Prénom' ne doit pas est vide.") : prenom;
            this.SalaireBrut = (salaireBrut <= 0 || salaireBrut > 20000000) ? throw new ArgumentException("Le 'Salaire Brut' doit etre positif.") : salaireBrut;
            this.NombreDeJours = (nombreDeJours < 0 || nombreDeJours > 31) ? throw new ArgumentException("Le 'Nombre De Jours' doit etre positif ou nul.") : nombreDeJours;
            this.Conjoint = (!conjoints.Contains(conjoint)) ? throw new ArgumentException("Les valeurs acceptées pour le champs 'Conjoint' sont '0', '1' ou '2'.") : conjoint;
            this.Enfants = (enfants < 0 || enfants > 25) ? throw new ArgumentException("Le champs 'Enfants' doit etre positif ou nul.") : enfants;
        }

        public void CalculerImpots()
        {
            BrutFiscalAnnuel = CalculerBrutFiscalAnnuel(SalaireBrut, NombreDeJours);
            Abattement = CalculerAbattement(BrutFiscalAnnuel);
            BrutFiscalApresAbattement = CalculerBrutFiscalApresAbattement(BrutFiscalAnnuel, Abattement);
            IRPPAvantReduction = CalculerIRPPAvantReduction(BrutFiscalApresAbattement);
            NombreDeParts = CalculerNombreDeParts(Enfants, Conjoint);
            Reduction = CalculReduction(IRPPAvantReduction, NombreDeParts);
            Impot = CalculerImpot(IRPPAvantReduction, Reduction, NombreDeJours);
        }

        private double CalculerImpot(double iRPPAvantReduction, double reduction, int nombreDeJours)
        {
            double impot = (iRPPAvantReduction - reduction) / (double)(360 / nombreDeJours);
            return (impot < 0) ? 0 : impot;
        }

        private double CalculerAbattement(double brutFiscalAnnuel)
        {
            double abattement = brutFiscalAnnuel * 0.3;
            return abattement;
        }

        private double CalculerBrutFiscalAnnuel(double salaireBrut, int nombreDeJours)
        {
            double brutFiscalAnnuel = salaireBrut * (double)(360 / nombreDeJours);
            return brutFiscalAnnuel;
        }

        private double CalculerBrutFiscalApresAbattement(double brutFiscalAnnuel, double abattement)
        {
            double brutFiscalApresAbattement = brutFiscalAnnuel, plafond = 900000;
            if (abattement > plafond)
            {
                brutFiscalApresAbattement -= plafond;
            }
            else
            {
                brutFiscalApresAbattement -= abattement;
            }
            return brutFiscalApresAbattement;
        }

        private double CalculerNombreDeParts(double enfants, int conjoint)
        {
            double nombreDeParts = 0.5 * enfants;
            switch (conjoint)
            {
                case 0:
                {
                    nombreDeParts += 1.0;
                    break;
                }
                case 1:
                {
                    nombreDeParts += 1.5;
                    break;
                }
                case 2:
                {
                    nombreDeParts += 2.0;
                    break;
                }
                default:
                {
                    break;
                }
            }
            return nombreDeParts;
        }

        private double CalculReduction(double irrpar, double nombreParts)
        {
            double reduction = 0.0;
            switch (nombreParts)
            {
                case 1:
                    {
                        reduction = 0;
                        break;
                    }
                case 1.5:
                    {
                        reduction = CalculerIntermediaireReduction(irrpar, 0.1, 100000, 300000);
                        break;
                    }
                case 2:
                    {
                        reduction = CalculerIntermediaireReduction(irrpar, 0.15, 200000, 650000);
                        break;
                    }
                case 2.5:
                    {
                        reduction = CalculerIntermediaireReduction(irrpar, 0.2, 300000, 1100000);
                        break;
                    }
                case 3:
                    {
                        reduction = CalculerIntermediaireReduction(irrpar, 0.25, 400000, 1650000);
                        break;
                    }
                case 3.5:
                    {
                        reduction = CalculerIntermediaireReduction(irrpar, 0.3, 500000, 2030000);
                        break;
                    }
                case 4:
                    {
                        reduction = CalculerIntermediaireReduction(irrpar, 0.35, 600000, 2490000);
                        break;
                    }
                case 4.5:
                    {
                        reduction = CalculerIntermediaireReduction(irrpar, 0.4, 700000, 2755000);
                        break;
                    }
                case 5:
                    {
                        reduction = CalculerIntermediaireReduction(irrpar, 0.45, 800000, 3180000);
                        break;
                    }
                default:
                    {
                        break;
                    }
            }
            return reduction;
        }

        private double CalculerIntermediaireReduction(double irrpar, double taux, double min, double max)
        {
            double resultat = 0.0;
            double temp = irrpar * taux;
            if (temp < min) resultat = min;
            else if (temp > max) resultat = max;
            else resultat = temp;
            return resultat;
        }

        private double CalculerIRPPAvantReduction(double brutFiscalApresAbattement)
        {
            double iRPPAvantReduction;
            if (brutFiscalApresAbattement < 630000)
            {
                iRPPAvantReduction = 0;
            }
            else if (brutFiscalApresAbattement > 630000 && brutFiscalApresAbattement <= 1500000)
            {
                iRPPAvantReduction = (brutFiscalApresAbattement - 630000) * 0.2;
            }
            else if (brutFiscalApresAbattement > 1500000 && brutFiscalApresAbattement <= 4000000)
            {
                iRPPAvantReduction = 174000 + (brutFiscalApresAbattement - 1500000) * 0.3;
            }
            else if (brutFiscalApresAbattement > 4000000 && brutFiscalApresAbattement <= 8000000)
            {
                iRPPAvantReduction = 174000 + 750000 + (brutFiscalApresAbattement - 4000000) * 0.35;
            }
            else if (brutFiscalApresAbattement > 8000000 && brutFiscalApresAbattement <= 13500000)
            {
                iRPPAvantReduction = 174000 + 750000 + 1400000 + (brutFiscalApresAbattement - 8000000) * 0.37;
            }
            else if (brutFiscalApresAbattement > 13500000 && brutFiscalApresAbattement <= 1000000000)
            {
                iRPPAvantReduction = 174000 + 750000 + 1400000 + 2035000 + (brutFiscalApresAbattement - 13500000) * 0.4;
            }
            else
            {
                iRPPAvantReduction = 398959000;
            }
            return iRPPAvantReduction;
        }
    }
}

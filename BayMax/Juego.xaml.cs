using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace BayMax2
{
    /// <summary>
    /// Lógica de interacción para Juego.xaml
    /// </summary>
    public partial class Juego : Window
    {
        private MainWindow mainW;
        int controlSecuencia = 0, logro_puntos = 0;
        Random nAleatorio = new Random();
        int puntos = 0;

        List<int> secuencia = new List<int>();
        bool spk = false;

        Storyboard seleccionbtn0, seleccionbtn1, seleccionbtn2, seleccionbtn3, seleccionbtn4, seleccionbtn5,
            seleccionbtn6, seleccionbtn7, seleccionbtn8, seleccionbtn9, seleccionbtn10,
            seleccionbtn11, seleccionbtn12, seleccionbtn13, seleccionbtn14;

        public int Puntos { get => puntos; set => puntos = value; }


        private void btn10_Click(object sender, RoutedEventArgs e)
        {
            verificarBoton(10);
        }

        private void btn11_Click(object sender, RoutedEventArgs e)
        {
            verificarBoton(11);
        }

        private void btn12_Click(object sender, RoutedEventArgs e)
        {
            verificarBoton(12);
        }

        private void btn13_Click(object sender, RoutedEventArgs e)
        {
            verificarBoton(13);
        }

        private void btn14_Click(object sender, RoutedEventArgs e)
        {
            verificarBoton(14);
        }

        private void btn9_Click(object sender, RoutedEventArgs e)
        {
            verificarBoton(9);
        }

        private void btn8_Click(object sender, RoutedEventArgs e)
        {
            verificarBoton(8);
        }

        private void btn7_Click(object sender, RoutedEventArgs e)
        {
            verificarBoton(7);
        }

        private void btn6_Click(object sender, RoutedEventArgs e)
        {
            verificarBoton(6);
        }

        private void btn5_Click(object sender, RoutedEventArgs e)
        {
            verificarBoton(5);
        }

        private void btn_4_Click(object sender, RoutedEventArgs e)
        {
            verificarBoton(4);
        }

        private void btn_3_Click(object sender, RoutedEventArgs e)
        {
            verificarBoton(3);
        }

        private void btn_2_Click(object sender, RoutedEventArgs e)
        {
            verificarBoton(2);
        }

        private void btn_1_Click(object sender, RoutedEventArgs e)
        {
            verificarBoton(1);
        }

        private void btn_0_Click(object sender, RoutedEventArgs e)
        {
            verificarBoton(0);
        }

        public Juego(MainWindow m)
        {
            this.mainW = m;
            InitializeComponent();
            nAleatorio = new Random();

            seleccionbtn0 = (Storyboard)this.Resources["seleccionBtn0"];
            seleccionbtn1 = (Storyboard)this.Resources["seleccionBtn1"];
            seleccionbtn2 = (Storyboard)this.Resources["seleccionBtn2"];
            seleccionbtn3 = (Storyboard)this.Resources["seleccionBtn3"];
            seleccionbtn4 = (Storyboard)this.Resources["seleccionBtn4"];
            seleccionbtn5 = (Storyboard)this.Resources["seleccionBtn5"];
            seleccionbtn6 = (Storyboard)this.Resources["seleccionBtn6"];
            seleccionbtn7 = (Storyboard)this.Resources["seleccionBtn7"];
            seleccionbtn8 = (Storyboard)this.Resources["seleccionBtn8"];
            seleccionbtn9 = (Storyboard)this.Resources["seleccionBtn9"];
            seleccionbtn10 = (Storyboard)this.Resources["seleccionBtn10"];
            seleccionbtn11 = (Storyboard)this.Resources["seleccionBtn11"];
            seleccionbtn12 = (Storyboard)this.Resources["seleccionBtn12"];
            seleccionbtn13 = (Storyboard)this.Resources["seleccionBtn13"];
            seleccionbtn14 = (Storyboard)this.Resources["seleccionBtn14"];


        }

        public void iniciar()
        {
            spk = true;
            foreach (int i in secuencia)
            {
                switch (i)
                {

                    case 0:
                        seleccionbtn0.Begin();
                        break;
                    case 1:
                        seleccionbtn1.Begin();
                        break;
                    case 2:
                        seleccionbtn2.Begin();
                        break;
                    case 3:
                        seleccionbtn3.Begin();
                        break;
                    case 4:
                        seleccionbtn4.Begin();
                        break;
                    case 5:
                        seleccionbtn5.Begin();
                        break;
                    case 6:
                        seleccionbtn6.Begin();
                        break;
                    case 7:
                        seleccionbtn7.Begin();
                        break;
                    case 8:
                        seleccionbtn8.Begin();
                        break;
                    case 9:
                        seleccionbtn9.Begin();
                        break;
                    case 10:
                        seleccionbtn10.Begin();
                        break;
                    case 11:
                        seleccionbtn11.Begin();
                        break;
                    case 12:
                        seleccionbtn12.Begin();
                        break;
                    case 13:
                        seleccionbtn13.Begin();
                        break;
                    case 14:
                        seleccionbtn14.Begin();
                        break;
                }
            }
            spk = false;
        }

        private void btnEmpezar_Click(object sender, RoutedEventArgs e)
        {
            secuencia.Add(nAleatorio.Next(0, 15));
            iniciar();

        }

        public void verificarBoton(int valorBoton)
        {
            if (spk || secuencia.Count == 0) return;

            if (secuencia[controlSecuencia] == valorBoton) controlSecuencia++;
            else
            {


                rcFinal.Visibility = System.Windows.Visibility.Visible;
                lblFinal.Visibility = System.Windows.Visibility.Visible;

                Puntos = secuencia.Count();
                if (Puntos >= 7 && logro_puntos == 0)
                {
                    mainW.addPoints(50, true);
                    logro_puntos++;
                }
                else
                {
                    mainW.addPoints(5 * Puntos, false);
                }
                lblFinal.Content = "Tu puntuación es de: " + Puntos + "\nMira mejor la próxima vez!!!\n\nHas recuperado puntos de diversión\npara tu Baymax";
                controlSecuencia = 0;
                secuencia = new List<int>();
                btnEmpezar.IsEnabled = false;




                return;
            }

            if (controlSecuencia >= secuencia.Count)
            {
                controlSecuencia = 0;
                secuencia.Add(nAleatorio.Next(0, 15));
                iniciar();
            }
            lblPuntuación.Content = secuencia.Count.ToString();
        }
    }
}

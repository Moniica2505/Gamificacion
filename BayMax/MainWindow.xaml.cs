using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Diagnostics;
using System.Linq;
using System.Speech.Recognition;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using WMPLib;

namespace BayMax2
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        int minutos, segundos, controlVoz=0;
        int logro_comer = 0, logro_dormir = 0, logro_jugar = 0, puntos_experiencia = 0, logro_barra = 0, logro_evolucionar = 0;
        double intervalo = 7000.0;
        Avatar miAvatar = new Avatar(100, 100, 100);
        bool conseguido = false, sound = true;

        Boolean voice = false;

        Juego game;

        DispatcherTimer t1, t2;

        Storyboard sbCansado, sbparpadeo, sbrespirar, sbTriste, sbHambre, sbAburrido,
            sbDormir, sbGuardarCaja, sbVoice, sbCuidar, sbBurguer, sbComer, sbCupcake, sbPiña,
            sbEnfermo, sbMorir, sb_logrosinicio, sb_logroConseguido, sb_logroBarra, sbHero, sb120puntos, 
            sb120conseguido, sbVozStop;

        OleDbConnection myconect;
        OleDbCommand mycommand;

        WMPLib.WindowsMediaPlayer wplayer;



        public MainWindow()
        {
            InitializeComponent();

            
            // BBDD Access
            myconect = new OleDbConnection(@"Provider = Microsoft.ACE.OLEDB.12.0; Data Source = Avatar.accdb");
            myconect.Open();

            game = new Juego(this);

            t1 = new DispatcherTimer();
            t1.Interval = TimeSpan.FromMilliseconds(intervalo);
            t1.Tick += new EventHandler(tickConsumoHandler);
            t1.Start();

            t2 = new DispatcherTimer();
            t2.Interval = TimeSpan.FromSeconds(1);
            t2.Tick += t2_tick;
            t2.Start();

            sbCansado = (Storyboard)this.Resources["Cansado"];
            sbparpadeo = (Storyboard)this.Resources["parpadeo"];
            sbrespirar = (Storyboard)this.Resources["Respiracion"];
            sbTriste = (Storyboard)this.Resources["Triste"];
            sbHambre = (Storyboard)this.Resources["Apetito"];
            sbAburrido = (Storyboard)this.Resources["Aburrido"];
            sbDormir = (Storyboard)this.Resources["Dormir"];
            sbGuardarCaja = (Storyboard)this.Resources["GuardarEnCaja"];
            sbVoice = (Storyboard)this.Resources["Voice"];
            sbCuidar = (Storyboard)this.Resources["Cuidar"];
            sbBurguer = (Storyboard)this.Resources["c_burguer"];
            sbComer = (Storyboard)this.Resources["Comer"];
            sbCupcake = (Storyboard)this.Resources["cupcake"];
            sbPiña = (Storyboard)this.Resources["pineapple"];
            sbMorir = (Storyboard)this.Resources["Morir"];
            sbEnfermo = (Storyboard)this.Resources["Enfermo"];
            sb_logrosinicio = (Storyboard)this.Resources["logros_iniciales"];
            sb_logroConseguido = (Storyboard)this.Resources["logro_conseguido"];
            sb_logroBarra = (Storyboard)this.Resources["logro_barras"];
            sbHero = (Storyboard)this.Resources["Hero"];
            sb120puntos = (Storyboard)this.Resources["logro_120"];
            sb120conseguido = (Storyboard)this.Resources["logro_120Conseguido"];
            sbVozStop = (Storyboard)this.Resources["sbVozStop"];

            lblPoints.Content = puntos_experiencia;


        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (sound)
            {
                sound = false;
                wplayer = new WMPLib.WindowsMediaPlayer();
                wplayer.URL = @"snd_Principal.mp3";
                wplayer.controls.play();
            } else
            {
                sound = true;
                wplayer.controls.stop();
            }
            
        }


        private void window_Activated(object sender, EventArgs e)
        {
            t1.Start();
            mycommand = myconect.CreateCommand();
            mycommand.CommandText = "SELECT * FROM Avatar WHERE Id=1";
            mycommand.CommandType = CommandType.Text;

            OleDbDataReader DBreader = mycommand.ExecuteReader();
            if (DBreader.Read())
            {
                pbEnergia.Value = Convert.ToDouble(DBreader["Energia"].ToString());
                pbApetito.Value = Convert.ToDouble(DBreader["Apetito"].ToString());
                pbDiversion.Value = Convert.ToDouble(DBreader["Diversion"].ToString());
            }

            sbparpadeo.Begin();
            sbrespirar.Begin();
        }
        
        private void btnReiniciar_Click(object sender, RoutedEventArgs e)
        {
            Process.Start(Application.ResourceAssembly.Location);

            Application.Current.Shutdown();
        }

        SpeechRecognitionEngine escucha = new SpeechRecognitionEngine();

        

        private void t2_tick(object sender, EventArgs e)
        {
            segundos += 1;

            if (segundos == 10 && minutos == 0 && logro_barra == 0)
            {
                sb_logroBarra.Begin();
            }

            if (segundos == 20 && minutos == 0 && logro_evolucionar == 0)
            {
                sb120puntos.Begin();
            }

            if (segundos == 60)
            {
                minutos += 1;
                segundos = 0;
            }
        }

        public void recognizer(object sender, SpeechRecognizedEventArgs e)
        {
            if (voice == true)
            {
                foreach (RecognizedWordUnit palabra in e.Result.Words)
                {
                    if (palabra.Text == "dormir")
                    {
                        logro_dormir++;
                        if (logro_dormir == 1)
                        {
                            puntos_experiencia += 10;
                            lblPoints.Content = puntos_experiencia;
                            sb_logrosinicio.Stop();
                            sb_logroConseguido.Begin();
                        }
                        pbEnergia.Value += 100;
                        sbparpadeo.Stop();
                        sbrespirar.Stop();
                        sbCansado.Stop();
                        sbAburrido.Stop();
                        sbHambre.Stop();
                        sbDormir.Begin();
                    }

                    if (palabra.Text == "cuidar")
                    {
                        sbparpadeo.Stop();
                        sbCuidar.Begin();
                    }

                    if (palabra.Text == "evoluciona" && conseguido)
                    {

                        sbHero.Begin();
                    }

                    if (palabra.Text == "hamburguesa")
                    {
                        logro_comer++;
                        if (logro_comer == 1)
                        {
                            puntos_experiencia += 10;
                            lblPoints.Content = puntos_experiencia;
                            sb_logrosinicio.Stop();
                            sb_logroConseguido.Begin();
                        }
                        pbApetito.Value += 40;
                        sbComer.Begin();
                        sbBurguer.Begin();
                    }

                    if (palabra.Text == "fruta")
                    {
                        logro_comer++;
                        if (logro_comer == 1)
                        {
                            puntos_experiencia += 10;
                            lblPoints.Content = puntos_experiencia;
                            sb_logrosinicio.Stop();
                            sb_logroConseguido.Begin();
                        }
                        pbApetito.Value += 40;
                        sbPiña.Begin();
                        sbComer.Begin();
                    }

                    if (palabra.Text == "dulce")
                    {
                        logro_comer++;
                        if (logro_comer == 1)
                        {
                            puntos_experiencia += 10;
                            lblPoints.Content = puntos_experiencia;
                            sb_logrosinicio.Stop();
                            sb_logroConseguido.Begin();
                        }
                        pbApetito.Value += 40;
                        sbCupcake.Begin();
                        sbComer.Begin();
                    }

                    if (palabra.Text == "jugar")
                    {
                        logro_jugar++;
                        if (logro_jugar == 1)
                        {
                            puntos_experiencia += 10;
                            lblPoints.Content = puntos_experiencia;
                            sb_logrosinicio.Stop();
                            sb_logroConseguido.Begin();
                        }

                        pbDiversion.Value = 100;
                        game = new Juego(this);
                        game.Show();

                        String sql = "UPDATE Avatar SET Energia= " + Convert.ToInt32(pbEnergia.Value) + ", Apetito= " + Convert.ToInt32(pbApetito.Value) + ", Diversion= " +
                            Convert.ToInt32(pbDiversion.Value) + " WHERE Id=1";
                        OleDbCommand com = new OleDbCommand(sql, myconect);

                        try
                        {
                            com.ExecuteNonQuery();
                        } catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                        

                        t1.Stop();
                    }
                }
            }

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            if(controlVoz%2==0)
            {
                controlVoz++;
                sbVoice.Begin();
                voice = true;

                try
                {
                    escucha.SetInputToDefaultAudioDevice();
                    escucha.LoadGrammar(new DictationGrammar());
                    escucha.SpeechRecognized += new EventHandler<SpeechRecognizedEventArgs>(recognizer);
                    escucha.RecognizeAsync(RecognizeMode.Multiple);

                }
                catch (InvalidOperationException ex)
                {
                    MessageBox.Show(ex.Message);
                }
            } else
            {
                controlVoz++;
                sbVozStop.Begin();
                escucha.RecognizeAsyncStop();
            }
        }
            

        private void tickConsumoHandler(object sender, EventArgs e)
        {
            int energia = Aleatorizar(15);
            int apetito = Aleatorizar(20);
            int diversion = Aleatorizar(15);

            pbEnergia.Value -= energia;
            miAvatar.Energía -= energia;
            pbApetito.Value -= apetito;
            miAvatar.Apetito -= apetito;
            pbDiversion.Value -= diversion;
            miAvatar.Diversión -= diversion;

            controlarEstado();
        }

        private int Aleatorizar(int n)
        {
            Random aleatorio = new Random();
            return 1 + aleatorio.Next(n);
        }


        private void btnDormir_Click(object sender, RoutedEventArgs e)
        {
            logro_dormir++;
            if (logro_dormir == 1)
            {
                puntos_experiencia += 10;
                lblPoints.Content = puntos_experiencia;
                sb_logrosinicio.Stop();
                sb_logroConseguido.Begin();
            }

            pbEnergia.Value += 100;
            sbparpadeo.Stop();
            sbrespirar.Stop();
            sbCansado.Stop();
            sbAburrido.Stop();
            sbHambre.Stop();
            sbDormir.Begin();

            if (puntos_experiencia >= 120 && logro_evolucionar == 0)
            {
                conseguido = true;
                sb120conseguido.Begin();
                logro_evolucionar++;
            }

            if (logro_barra == 0 && pbApetito.Value == 100 && pbDiversion.Value == 100 && pbEnergia.Value == 100)
            {
                puntos_experiencia += 30;
                lblPoints.Content = puntos_experiencia;
                logro_barra++;
                sb_logroConseguido.Begin();
            }

        }

        private void btnComer_Click(object sender, RoutedEventArgs e)
        {
            logro_comer++;
            if (logro_comer == 1)
            {
                puntos_experiencia += 10;
                lblPoints.Content = puntos_experiencia;
                sb_logrosinicio.Stop();
                sb_logroConseguido.Begin();
            }


            Random nAleatorio = new Random();
            int n = nAleatorio.Next(0, 3);
            pbApetito.Value += 40;
            sbparpadeo.Stop();

            if (puntos_experiencia >= 120 && logro_evolucionar == 0)
            {
                conseguido = true;
                sb120conseguido.Begin();
                logro_evolucionar++;
            }

            if (logro_barra == 0 && pbApetito.Value == 100 && pbDiversion.Value == 100 && pbEnergia.Value == 100)
            {
                puntos_experiencia += 30;
                lblPoints.Content = puntos_experiencia;
                logro_barra++;
                sb_logroConseguido.Begin();
            }

            switch (n)
            {
                case 0:
                    sbPiña.Begin();
                    sbComer.Begin();
                    break;
                case 1:
                    sbComer.Begin();
                    sbCupcake.Begin();
                    break;
                case 2:
                    sbComer.Begin();
                    sbBurguer.Begin();
                    break;
            }
        }

        private void btnJugarClick(object sender, RoutedEventArgs e)
        {
            logro_jugar++;
            if (logro_jugar == 1)
            {
                puntos_experiencia += 10;
                lblPoints.Content = puntos_experiencia;
                sb_logrosinicio.Stop();
                sb_logroConseguido.Begin();
            }

            pbDiversion.Value = 100;
            game = new Juego(this);
            game.Show();

            String sql = "UPDATE Avatar SET Energia= " + Convert.ToInt32(pbEnergia.Value) + ", Apetito= " + Convert.ToInt32(pbApetito.Value) + ", Diversion= " +
                            Convert.ToInt32(pbDiversion.Value) + " WHERE Id=1";
            OleDbCommand com = new OleDbCommand(sql, myconect);

            try
            {
                com.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            t1.Stop();

            if (puntos_experiencia >= 120 && logro_evolucionar == 0)
            {
                conseguido = true;
                sb120conseguido.Begin();
                logro_evolucionar++;
            }

            if (logro_barra == 0 && pbApetito.Value == 100 && pbDiversion.Value == 100 && pbEnergia.Value == 100)
            {
                puntos_experiencia += 30;
                lblPoints.Content = puntos_experiencia;
                logro_barra++;
                sb_logroConseguido.Begin();
            }
        }


        private void controlarEstado()
        {
            if (pbEnergia.Value <= 50)
            {
                sbCansado.Begin();
            }
            if (pbDiversion.Value <= 50)
            {
                sbAburrido.Begin();
            }
            if (pbApetito.Value <= 50)
            {
                sbparpadeo.Stop();
                sbTriste.Begin();
                sbHambre.Begin();
            }

            if (pbEnergia.Value <= 25 || pbApetito.Value <= 25 || pbDiversion.Value <= 25)
            {
                sbCansado.Stop();
                sbAburrido.Stop();
                sbHambre.Stop();
                sbEnfermo.Begin();
            }


            if (pbEnergia.Value <= 0 || pbApetito.Value <= 0 || pbDiversion.Value <= 0)
            {
                sbMorir.Begin();
                sbGuardarCaja.Begin();
                sbEnfermo.Stop();

                btnComer.IsEnabled = false;
                btnDormir.IsEnabled = false;
                btnJugar.IsEnabled = false;
                btnVoz.IsEnabled = false;


                escucha.RecognizeAsyncStop();
                t1.Stop();

                rctMuerte.Visibility = System.Windows.Visibility.Visible;
                lblMuerte.Visibility = System.Windows.Visibility.Visible;
                btnReiniciar.Visibility = System.Windows.Visibility.Visible;

                lblMuerte.Content = "Oh no! Has perdido!\nNo dejes que las barras\nse consuman.\nHas aguantado:\n" + minutos + ":" + segundos;
            }

        }

        public void addPoints(int p, bool logro)
        {
            puntos_experiencia += p;
            lblPoints.Content = puntos_experiencia;

            if (logro)
            {
                sb_logroConseguido.Begin();
            }

            if (puntos_experiencia >= 120 && logro_evolucionar == 0)
            {
                conseguido = true;
                sb120conseguido.Begin();
                logro_evolucionar++;
            }
        }
    }
}

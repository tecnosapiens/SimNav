using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace SimNav
{
    public partial class DatosBuquePropio : UserControl
    {
        BuquePropio buquePropio;
        Form1 mainForm;
        
        DateTime inicioNavegacion;
        int hora;
        int minuto;
        int segundo;

       private Thread myThreadNav;
        private Mutex myMutex;
       public bool navegacionActiva;
       bool seleccionCoordenadas;
       bool hiloCreado;

        public DatosBuquePropio()
        {
            InitializeComponent();
            navegacionActiva = false;
            inicioNavegacion = new DateTime();
            hora = 0;
            minuto = 0;
            segundo = 0;
            seleccionCoordenadas = false;
            hiloCreado = false;
            
        }

        public DatosBuquePropio(Form1 mainForm)
        {
            this.mainForm = mainForm;
            buquePropio = new BuquePropio();
            InitializeComponent();
            navegacionActiva = false;
            seleccionCoordenadas = false;
            hiloCreado = false;

        }


        

        private void button_aceptarDatosBuquePropio_Click(object sender, EventArgs e)
        {
            //killProcesoNavegacion();
            seleccionCoordenadas = false;
            buquePropio.setDatosBuquePropio((int)numericUpDown_gradoLatitud.Value,
                                          (int)numericUpDown_minutoLatitud.Value,
                                          (int)numericUpDown_segundosLatitud.Value,
                                          comboBox_signoLatitud.Text,
                                          (int)numericUpDown_gradoLongitud.Value,
                                          (int)numericUpDown_minutoLongitud.Value,
                                          (int)numericUpDown_segundosLongitud.Value,
                                          comboBox_signoLongitud.Text,
                                          (int)numericUpDown_velocidad.Value,
                                          (int)numericUpDown_rumbo.Value,
                                          (int)numericUpDown_profundidad.Value,
                                          (int)numericUpDown_dirViento.Value,
                                          (int)numericUpDown_velViento.Value);
            this.Visible = false;

            mainForm.numericUpDown_velocidadBuque.Value = (int)numericUpDown_velocidad.Value;
            mainForm.label_latitud.Text = buquePropio.getLatitudStr();
            mainForm.label_longitud.Text = buquePropio.getLongitudStr();
            mainForm.label_rumbo.Text = buquePropio.getRumboStr();
            mainForm.label_velocidad.Text = buquePropio.getVelocidadStr();
            mainForm.label_profundidad.Text = buquePropio.getProfundidadStr();

            mainForm.label_velViento.Text = buquePropio.getVientoVelocidadStr();
            mainForm.label_dirViento.Text = buquePropio.getVientoDireccionStr();


            mainForm.canvasMapa.set_NocursorSeleccionCoordenadas();


            // inicia el hilo principal de la navegacion del buque programa
            hiloCreado = true;
            myMutex = new Mutex();
            myThreadNav = new Thread(new ThreadStart(hiloNavegacion));
            myThreadNav.IsBackground = true;

            myThreadNav.Start();

            
        }

        private void hiloNavegacion()
        {
            this.obtenerHoraInicialNavegacion();
          
            while (myThreadNav.IsAlive)
            {

                if (navegacionActiva)
                {
                    myMutex.WaitOne();

                    getNuevaPosGeo(1000);
                    getNuevaPosGeoVector();
                    actualizarNuevaPosGeoBuquePropioGUI();
                    actualizarTiempoNavegacion();
                    actualizarMillasNavegadas();

                    myMutex.ReleaseMutex();


                }
                else
                {
                    inicializarTiempoNavegacion();
                    inicializarMillasNavegadas();

                }



                if (mainForm.canvasMapa.flagMapaAbierto)
                {
                    mainForm.canvasMapa.refreshMapa();
                    mainForm.canvasMapa.graficarBuquePropio(get_Latitud(), get_Longitud());

                    if (mainForm.canvasMapa.verTrayectoria)
                    {
                        mainForm.canvasMapa.graficarTrayecto(get_trayectoria());
                    }
                    if (navegacionActiva)
                    {
                        mainForm.canvasMapa.graficarVectorVel(get_Latitud(), get_Longitud(), get_LatitudFinVector(), get_LongitudFinVector());
                    
                    }
                                    
                    
                   
                }

                 Thread.Sleep(1000);





            }
           
        }

        public void detenerNavegacion()
        {
            myThreadNav.Abort();
        }


        private void button_cancelarDatosBuqe_Click(object sender, EventArgs e)
        {
            this.Visible = false;
        }

        public string[] getDatos_GPS()
        {
            return buquePropio.getTramasNMEA_GPS();
        }

        public string[] getDatos_GPSPropio()
        {
            return buquePropio.getTramasNMEA_GPSPropio();
        }
        public string[] getDatos_DGPS()
        {
            return buquePropio.getTramasNMEA_DGPS();
        }

        public string[] getDatos_GYRO()
        {
            return buquePropio.getTramasNMEA_DGPS();
        }

        public string[] getDatos_CORREDERA()
        {
            return buquePropio.getTramasNMEA_CORREDERA();
        }

        public string[] getDatos_ECOSONDA()
        {
            return buquePropio.getTramasNMEA_ECOSONDA();
        }

        public string[] getDatos_AIS()
        {
            return buquePropio.getTramasNMEA_AIS();
        }

        public string[] getDatos_ANEMOMETRO()
        {
            return buquePropio.getTramasNMEA_ANEMOMETRO();
        }

        

        public void getNuevaPosGeo(int tiempo)
        {
            buquePropio.actualizarPosicionGeografica(tiempo);
        }

        public void getNuevaPosGeoVector()
        {
            buquePropio.actualizarPosicionVector();
        }

        public void actualizarNuevaPosGeoBuquePropioGUI()
        {
            
            mainForm.setLabelLatitud(buquePropio.getLatitudStr());
            mainForm.setLabelLongitud(buquePropio.getLongitudStr());
            mainForm.setLabelRumbo(buquePropio.getRumboStr());
            

        }

        public List<PuntoGeografico> get_trayectoria()
        {
            return buquePropio.getTrayectoria();
        }
        public double get_Latitud()
        {
            return buquePropio.getLatitud();
        }
        public double get_Longitud()
        {
            return buquePropio.getLongitud();
        }
        public double get_LatitudInicio()
        {
            return buquePropio.getLatitudInicial();
        }
        public double get_LongitudInicio()
        {
            return buquePropio.getLongitudInicial();
        }
        public double get_LatitudFinVector()
        {
            return buquePropio.getLatitudFinVector();
        }
        public double get_LongitudFinVector()
        {
            return buquePropio.getLongitudFinVector();
        }
        public int get_Velocidad()
        {
            return buquePropio.getVelocidad();
        }

        public void set_seleccionCoordenadas(bool status)
        {
            seleccionCoordenadas = status;
        }

        public bool get_seleccionCoordenadas()
        {
            return seleccionCoordenadas;
        }

        public void set_coordenadasCanvas(double Lat, double Lon)
        {

        }

        public void set_latitudCanvas(double lat)
        {
            buquePropio.setLatitud(lat);
        }

        public void set_longitudCanvas(double lon)
        {
            buquePropio.setLongitud(lon);
        }

        private void obtenerHoraInicialNavegacion()
        {
           inicioNavegacion = DateTime.Now;
        }

        private void actualizarTiempoNavegacion()
        {
          
            segundo++;
            if(segundo == 60)
            {
                segundo = 0;
                minuto++;

            }
            if(minuto == 60)
            {
                minuto = 0;
                hora++;
            }

            mainForm.setLabelTiempo(hora.ToString("00") + ":" + minuto.ToString("00") + ":" + segundo.ToString("00"));
            

        }
        private void inicializarTiempoNavegacion()
        {
            segundo = 0;
            minuto = 0;
            hora = 0;
            

            mainForm.setLabelTiempo(hora.ToString("00") + ":" + minuto.ToString("00") + ":" + segundo.ToString("00"));


        }

        private void actualizarMillasNavegadas()
        {
            double millas;
            millas = Math.Abs(buquePropio.getLatitudInicial() - buquePropio.getLatitud())/60;

            buquePropio.ObtenerDistancia(get_LatitudInicio(), get_LongitudInicio(), get_Latitud(), get_Longitud(), ref millas);

            mainForm.setLabelMilla(millas.ToString("00.00"));
        }

        private void inicializarMillasNavegadas()
        {
            double millas = 0.0;
            mainForm.setLabelMilla(millas.ToString("00.00"));
        }

        /**
		* Esta función xxxxxxxxx
		* @param double latitud --> 
		* @param double longitud --> 
		* @param ref string cadenaLatitud --> 
		* @param ref string cadenaLongitud --> 
		*/
        public void actualizarCoordenadas(double latitud, double longitud)
        {


            double minutos;
            double grados;
            double segundos;
            double entero, enteroMin, decMin, decSeg;



            if (longitud < 0)
            {
                comboBox_signoLongitud.Text = "W";
                  
                longitud = Math.Abs(longitud);
            }
            else
            {
                comboBox_signoLongitud.Text = "E"; 
            }

            if (latitud < 0)
            { 
                comboBox_signoLatitud.Text = "S"; 
                latitud = Math.Abs(latitud);
            }
            else
            { 
               comboBox_signoLatitud.Text = "N";
            }



            if (latitud < 90 || latitud > -90 || longitud < 180 || longitud > -180)
            {
                decMin = latitud - (entero = Math.Floor(latitud));
                minutos = decMin * 60;

                decSeg = minutos - (enteroMin = Math.Floor(minutos));
                segundos = decSeg * 60;

                grados = entero;
                minutos = enteroMin;

                if (grados < 0) grados = grados * (-1);
                if (minutos < 0) minutos = minutos * (-1);
                if (segundos < 0) segundos = segundos * (-1);

                

                numericUpDown_gradoLatitud.Value = (int)grados;
                numericUpDown_minutoLatitud.Value = (int)minutos;
                numericUpDown_segundosLatitud.Value = (int)segundos;


                //+++++++++++++++++++++    conversion longitud
                decMin = longitud - (entero = Math.Floor(longitud));
                minutos = decMin * 60;
                decSeg = minutos - (enteroMin = Math.Floor(minutos));
                segundos = decSeg * 60;
                grados = entero;
                minutos = enteroMin;
                if (grados < 0) grados = grados * (-1);
                if (minutos < 0) minutos = minutos * (-1);
                if (segundos < 0) segundos = segundos * (-1);

                numericUpDown_gradoLongitud.Value = (int)grados;
                numericUpDown_minutoLongitud.Value = (int)minutos;
                numericUpDown_segundosLongitud.Value = (int)segundos;

            }
        }

        public bool get_estadoHiloNavegacion()
        {
            return hiloCreado;
        }

        public void killProcesoNavegacion()
        {
            if (hiloCreado)
            {
                this.detenerNavegacion();
                hiloCreado = false;
               
            }
        
        
        




        }



        // variables de navegacion en tiempo real
        public void set_anguloTimon(int angulo)
        {
            buquePropio.set_anguloTimon(angulo);
        }

        public void set_velocidadBuque(int vel)
        {
            buquePropio.setVelocidad(vel);
        }



        

         
    
    }//fin clase;

}

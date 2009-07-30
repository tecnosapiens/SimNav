using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace SimNav
{
    public partial class Form1 : Form
    {

        /* declaracion de variable de control de usuario para configuracion puerto
         * variable ObjetoPuertoSerial es del tipo PuertoSerial que es un tipo de dato creado por
         * el programador y se utiliza para gurdar todos los datos de configuracion del puerto serial 
         * que se va a utilizar para la transmision de datos.
         * Los datos que se obtienen del usuario se obtienen a traves de un cuadro de dialogo de configuracion 
         * de puerto serial tambien creado por el programador*/
        
         public PuertoSerial ObjetoPuertoSerial;

         public DatosBuquePropio buquePropio;

         public MapControl canvasMapa;


         bool conLoop;
         bool hiloIniciado;
         bool reproduccion;

         private Thread myThread;
        

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            ObjetoPuertoSerial = new PuertoSerial();
            buquePropio = new DatosBuquePropio(this);
            conLoop = true;

            canvasMapa = new MapControl(this);
            this.Controls.Add(canvasMapa);
            buquePropio.Location = new Point(370,6);
            //buquePropio.Size = new Size(groupBox_datosConfigurados.Size.Width, groupBox_datosConfigurados.Size.Height);
            canvasMapa.BringToFront();
            canvasMapa.Visible = true;
            hiloIniciado = false;
            reproduccion = false;



        }

        private void button_configurarPuerto_Click(object sender, EventArgs e)
        {
           
            
            ConfiguraPuertoSerialDialog configuracionPuertoSerial = new ConfiguraPuertoSerialDialog(this);

            configuracionPuertoSerial.BringToFront();
            configuracionPuertoSerial.Show();
            button_detener.Enabled = true;
            //button_transmitir.Enabled = true;
           

        }

        private void button_transmitir_Click(object sender, EventArgs e)
        {
            button_transmitir.Enabled = false;
            groupBox_reproduccion.Enabled = false;

            if(ObjetoPuertoSerial.get_estadoPuertoSerial())
            {
                label_bitDatos.Text = ObjetoPuertoSerial.getBitDatos();
                label_bitParada.Text = ObjetoPuertoSerial.getBitParada();
                label_paridad.Text = ObjetoPuertoSerial.getParidad();
                label_puertoCom.Text = ObjetoPuertoSerial.getNombrePuerto();
                label_velocidadTransmicion.Text = ObjetoPuertoSerial.getVelocidadPuerto();
                listBox_visorDatosNmea.Items.Add("Inicia Transmision de Datos");
                // se enviaran los datos por puerta serial con loop
                
                    // inicia el hilo principal del programa
                    myThread = new Thread(new ThreadStart(hiloPrincipal));
                    myThread.IsBackground = true;
                    hiloIniciado = true;
                  
                    myThread.Start();

               

                button_configurarPuerto.Enabled = false;
            }
            else
            {
                MessageBox.Show("Debe Configurar el Puerto Serial", "Informacion", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

            


            
        }

        private void hiloPrincipal()
        {
            ObjetoPuertoSerial.AbrirPuertoSerial();
            string[] data = { "" };
            //ImprimirSalida("inicio de transmision");
            while (myThread.IsAlive)
            {
                if (reproduccion)
                {
                    if (conLoop)
                    {
                        ImprimirArchivoTramasNMEA();
                    }
                    else
                    {
                        ImprimirArchivoTramasNMEA();
                        //button_configurarPuerto.Enabled = true;
                        //button_transmitir.Enabled = true;
                        //button_detener.Enabled = false;
                        this.myThread.Abort();
                        

                    }
                


                    
                }
                else
                {
                    if (checkBox_gpsPropio.Checked)
                    {
                        data = buquePropio.getDatos_GPSPropio();

                        foreach (string datas in data)
                        {
                            ObjetoPuertoSerial.enviarData(datas);
                            SetText("GPS Bluetooth:\t" + datas);
                            Thread.Sleep(ObjetoPuertoSerial.getTiempoTransmisionINT());


                        }
                    }
                    if (checkBox_estadoGPS.Checked)
                    {
                        data = buquePropio.getDatos_GPS();

                        foreach (string datas in data)
                        {
                            ObjetoPuertoSerial.enviarData(datas);
                            SetText("GPS:\t" + datas);
                            Thread.Sleep(ObjetoPuertoSerial.getTiempoTransmisionINT());


                        }
                    }
                    if (checkBox_estadoDGPS.Checked)
                    {
                        data = buquePropio.getDatos_DGPS();

                        foreach (string datas in data)
                        {
                            ObjetoPuertoSerial.enviarData(datas);
                            SetText("DGPS:\t" + datas);
                            Thread.Sleep(ObjetoPuertoSerial.getTiempoTransmisionINT());


                        }

                    }
                    if (checkBox_estadoGyro.Checked)
                    {
                        data = buquePropio.getDatos_GYRO();

                        foreach (string datas in data)
                        {
                            ObjetoPuertoSerial.enviarData(datas);
                            SetText("GYRO:\t" + datas);
                            Thread.Sleep(ObjetoPuertoSerial.getTiempoTransmisionINT());


                        }

                    }
                    if (checkBox_estadoEcosonda.Checked)
                    {
                        data = buquePropio.getDatos_ECOSONDA();

                        foreach (string datas in data)
                        {
                            ObjetoPuertoSerial.enviarData(datas);
                            SetText("ECOSONDA:\t" + datas);
                            Thread.Sleep(ObjetoPuertoSerial.getTiempoTransmisionINT());


                        }
                    }
                    if (checkBox_estadoCorredera.Checked)
                    {
                        data = buquePropio.getDatos_CORREDERA();

                        foreach (string datas in data)
                        {
                            ObjetoPuertoSerial.enviarData(datas);
                            SetText("CORREDERA:\t" + datas);
                            Thread.Sleep(ObjetoPuertoSerial.getTiempoTransmisionINT());


                        }
                    }
                    if (checkBox_estadoAnemometro.Checked)
                    {
                        data = buquePropio.getDatos_ANEMOMETRO();

                        foreach (string datas in data)
                        {
                            ObjetoPuertoSerial.enviarData(datas);
                            SetText("ANEMOMETRO:\t" + datas);
                            Thread.Sleep(ObjetoPuertoSerial.getTiempoTransmisionINT());


                        }
                    }
                    if (checkBox_estadoAIS.Checked)
                    {
                        data = buquePropio.getDatos_AIS();

                        foreach (string datas in data)
                        {
                            ObjetoPuertoSerial.enviarData(datas);
                            SetText("AIS:\t" + datas);
                            Thread.Sleep(ObjetoPuertoSerial.getTiempoTransmisionINT());


                        }
                    }

                
                }
                
            }
            ObjetoPuertoSerial.CerrarPuertoSerial();
        }

        private void button_configurarDatosBuque_Click(object sender, EventArgs e)
        {
            if (buquePropio.get_estadoHiloNavegacion())
            {
                buquePropio.killProcesoNavegacion();
            }
                this.tabPage_configuracion.Controls.Add(buquePropio);
                buquePropio.Location = new Point(groupBox_datosConfigurados.Location.X, groupBox_datosConfigurados.Location.Y);
                //buquePropio.Size = new Size(groupBox_datosConfigurados.Size.Width, groupBox_datosConfigurados.Size.Height);
                buquePropio.BringToFront();
                buquePropio.Visible = true;
                buquePropio.set_seleccionCoordenadas(true);
                canvasMapa.set_cursorSeleccionCoordenadas();
            
           
            
                
           
            
            
            
                      
           
        }

        private void checkBox_Loop_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_Loop.Checked)
            {
                conLoop = true;
            }
            else
            {
                conLoop = false;
            }
        }

        private void button_detener_Click(object sender, EventArgs e)
        {
            if (hiloIniciado)
            {
                this.myThread.Abort();
            }
            
              ObjetoPuertoSerial.CerrarPuertoSerial();

            reproduccion = false;
            checkBox_navegacionBuque.Checked = false;
            button_seleccionArchivo.Enabled = true;
            button_configurarPuerto.Enabled = true;
            button_transmitir.Enabled = true;
            groupBox_reproduccion.Enabled = true;

           // this.checkBox_navegacionBuque.Click += new System.EventHandler(this.checkBox_navegacionBuque_CheckedChanged);
        }

        private void ImprimirSalida(string salida)
        {
            listBox_visorDatosNmea.Items.Add(salida);
        }

        // Usando delegados y funciones "callback"
//
// Este delegado define un método void
// que recibe un parámetro de tipo string
delegate void SetTextCallback(string text);

private void SetText(string text)
{
    // InvokeRequired required compares the thread ID of the
    // calling thread to the thread ID of the creating thread.
    // If these threads are different, it returns true.
    if (this.listBox_visorDatosNmea.InvokeRequired)
    {
        SetTextCallback d = new SetTextCallback(SetText);
        this.Invoke(d, new object[] { text });
    }
    else
    {
        this.listBox_visorDatosNmea.Items.Add(text);
        this.listBox_visorDatosNmea.TopIndex = (listBox_visorDatosNmea.Items.Count -4);
    }
}

//delegate void SetTextCallback(string text);

public void setLabelLatitud(string text)
{
    // InvokeRequired required compares the thread ID of the
    // calling thread to the thread ID of the creating thread.
    // If these threads are different, it returns true.
    if (this.label_latitud.InvokeRequired)
    {
        SetTextCallback d = new SetTextCallback(setLabelLatitud);
        this.Invoke(d, new object[] { text });
    }
    else
    {
        this.label_latitud.Text = text;
    }
}

public void setLabelLongitud(string text)
{
    // InvokeRequired required compares the thread ID of the
    // calling thread to the thread ID of the creating thread.
    // If these threads are different, it returns true.
    if (this.label_longitud.InvokeRequired)
    {
        SetTextCallback d = new SetTextCallback(setLabelLongitud);
        this.Invoke(d, new object[] { text });
    }
    else
    {
        this.label_longitud.Text = text;
    }
}

public void setLabelRumbo(string text)
{
    // InvokeRequired required compares the thread ID of the
    // calling thread to the thread ID of the creating thread.
    // If these threads are different, it returns true.
    if (this.label_rumbo.InvokeRequired)
    {
        SetTextCallback d = new SetTextCallback(setLabelRumbo);
        this.Invoke(d, new object[] { text });
    }
    else
    {
        this.label_rumbo.Text = text;
    }
}

public void setLabelTiempo(string text)
{
    // InvokeRequired required compares the thread ID of the
    // calling thread to the thread ID of the creating thread.
    // If these threads are different, it returns true.
    if (this.label_latitud.InvokeRequired)
    {
        SetTextCallback d = new SetTextCallback(setLabelTiempo);
        this.Invoke(d, new object[] { text });
    }
    else
    {
        this.label_tiempoNavegado.Text = text;
    }
}

public void setLabelMilla(string text)
{
    // InvokeRequired required compares the thread ID of the
    // calling thread to the thread ID of the creating thread.
    // If these threads are different, it returns true.
    if (this.label_latitud.InvokeRequired)
    {
        SetTextCallback d = new SetTextCallback(setLabelMilla);
        this.Invoke(d, new object[] { text });
    }
    else
    {
        this.label_millasNavegadas.Text = text;
    }
}

private void checkBox_estadoGPS_CheckedChanged(object sender, EventArgs e)
{
             if(checkBox_estadoGPS.Checked)
			 {
				 checkBox_estadoGPS.BackColor = System.Drawing.Color.Green;
				 checkBox_estadoGPS.Text = "Activado";
				 
			 }
			 else
			 {
				 checkBox_estadoGPS.BackColor = System.Drawing.Color.Red;
				 checkBox_estadoGPS.Text = "Desactivado";
				 
			 }
}

private void checkBox_estadoDGPS_CheckedChanged(object sender, EventArgs e)
{
            if(checkBox_estadoDGPS.Checked)
			 {
				 checkBox_estadoDGPS.BackColor = System.Drawing.Color.Green;
				 checkBox_estadoDGPS.Text = "Activado";
				 
			 }
			 else
			 {
				 checkBox_estadoDGPS.BackColor = System.Drawing.Color.Red;
				 checkBox_estadoDGPS.Text = "Desactivado";
				 
			 }
}

private void checkBox_estadoGyro_CheckedChanged(object sender, EventArgs e)
{
            if(checkBox_estadoGyro.Checked)
			 {
				 checkBox_estadoGyro.BackColor = System.Drawing.Color.Green;
				 checkBox_estadoGyro.Text = "Activado";
				 
			 }
			 else
			 {
				 checkBox_estadoGyro.BackColor = System.Drawing.Color.Red;
				 checkBox_estadoGyro.Text = "Desactivado";
				 
			 }
}

private void checkBox_estadoEcosonda_CheckedChanged(object sender, EventArgs e)
{
            if(checkBox_estadoEcosonda.Checked)
			 {
				 checkBox_estadoEcosonda.BackColor = System.Drawing.Color.Green;
				 checkBox_estadoEcosonda.Text = "Activado";
				 
			 }
			 else
			 {
				 checkBox_estadoEcosonda.BackColor = System.Drawing.Color.Red;
				 checkBox_estadoEcosonda.Text = "Desactivado";
				 
			 }
}

private void checkBox_estadoAnemometro_CheckedChanged(object sender, EventArgs e)
{
            if(checkBox_estadoAnemometro.Checked)
			 {
				 checkBox_estadoAnemometro.BackColor = System.Drawing.Color.Green;
				 checkBox_estadoAnemometro.Text = "Activado";
				 
			 }
			 else
			 {
				 checkBox_estadoAnemometro.BackColor = System.Drawing.Color.Red;
				 checkBox_estadoAnemometro.Text = "Desactivado";
				 
			 }
}

private void checkBox_estadoCorredera_CheckedChanged(object sender, EventArgs e)
{
             if(checkBox_estadoCorredera.Checked)
			 {
				 checkBox_estadoCorredera.BackColor = System.Drawing.Color.Green;
				 checkBox_estadoCorredera.Text = "Activado";
				 
			 }
			 else
			 {
				 checkBox_estadoCorredera.BackColor = System.Drawing.Color.Red;
				 checkBox_estadoCorredera.Text = "Desactivado";
				 
			 }
}

private void checkBox_estadoAIS_CheckedChanged(object sender, EventArgs e)
{
            if(checkBox_estadoAIS.Checked)
			 {
				 checkBox_estadoAIS.BackColor = System.Drawing.Color.Green;
				 checkBox_estadoAIS.Text = "Activado";
				 
			 }
			 else
			 {
				 checkBox_estadoAIS.BackColor = System.Drawing.Color.Red;
				 checkBox_estadoAIS.Text = "Desactivado";
				 
			 }
}

private void checkBox_navegacionBuque_CheckedChanged(object sender, EventArgs e)
{
    if (checkBox_navegacionBuque.Checked)
    {
        checkBox_navegacionBuque.BackColor = System.Drawing.Color.Green;
        checkBox_navegacionBuque.Text = "Activado";
        buquePropio.navegacionActiva = true;
        groupBox_controlBuque.Enabled = true;
        button_configurarDatosBuque.Enabled = false;
        

    }
    else
    {
        checkBox_navegacionBuque.BackColor = System.Drawing.Color.Red;
        checkBox_navegacionBuque.Text = "Desactivado";
        buquePropio.navegacionActiva = false;
        groupBox_controlBuque.Enabled = false;
        button_configurarDatosBuque.Enabled = true;

    }
}

public void setCoordenadasCanvas(double latitud, double longitud)
{
    if (buquePropio.get_seleccionCoordenadas())
    {
        buquePropio.actualizarCoordenadas(latitud, longitud);
    }
}

private void button_limpiar_Click(object sender, EventArgs e)
{
    listBox_visorDatosNmea.Items.Clear();
}

private void button_seleccionArchivo_Click(object sender, EventArgs e)
{
    MessageBox.Show("Al utilizar esta funcionalidad dejara inactiva la funcion de Simulacion de Navegación", "Informacion", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

    if (hiloIniciado)
    {
        this.myThread.Abort();
    }

    if (buquePropio.get_estadoHiloNavegacion())
    {
        buquePropio.killProcesoNavegacion();
    }

    checkBox_Loop.Visible = true;
    checkBox_Loop.Enabled = true;
    checkBox_navegacionBuque.Checked = false;
    if (!ObjetoPuertoSerial.get_estadoPuertoSerial())
    {
        button_configurarPuerto.Enabled = true;
    }
  
    //button_transmitir.Enabled = true;


    string nfile;
    

		
			
					
			if  (openFileDialog_seleccionArchivo.ShowDialog()== System.Windows.Forms.DialogResult.OK)
			{
				
                nfile = System.IO.Path.GetFileName(openFileDialog_seleccionArchivo.FileName);
                label_nombreArchivo.Text = nfile;
                button_seleccionArchivo.Enabled = false;
				reproduccion = true;
				
				
			}
			else
			{
				MessageBox.Show("Debes elejir un Archivo","Informacion ",MessageBoxButtons.OK,MessageBoxIcon.Question);
			}
		 






}

void EnviarDataReproduccionSinLoop()
{

    if (reproduccion)
    {
        ImprimirArchivoTramasNMEA();

    }
   
   

}

        void ImprimirArchivoTramasNMEA()
		  {

              string serialData = "";
				System.IO.StreamReader srFile = new System.IO.StreamReader(openFileDialog_seleccionArchivo.FileName);

				while (srFile.ReadLine() != null)
				{
                    
				  serialData = srFile.ReadLine();
                  ObjetoPuertoSerial.enviarData(serialData);
                  SetText(serialData);
                  Thread.Sleep(ObjetoPuertoSerial.getTiempoTransmisionINT());  
				 }
				 srFile.Close();

		  }


        public void activarBotonTransmitir()
        {
            button_transmitir.Enabled = true;
        }

        private void numericUpDown_velocidadBuque_ValueChanged(object sender, EventArgs e)
        {
            buquePropio.set_velocidadBuque((int)numericUpDown_velocidadBuque.Value);
        }

        private void numericUpDown_anguloTimon_ValueChanged(object sender, EventArgs e)
        {
            buquePropio.set_anguloTimon((int)numericUpDown_anguloTimon.Value);
        }

        private void checkBox_gpsPropio_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_gpsPropio.Checked)
            {
                checkBox_gpsPropio.BackColor = System.Drawing.Color.Green;
                checkBox_gpsPropio.Text = "Activado";

            }
            else
            {
                checkBox_gpsPropio.BackColor = System.Drawing.Color.Red;
                checkBox_gpsPropio.Text = "Desactivado";

            }

        }

        private void checkBox_ocultarPanelTramasNMEA_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_ocultarPanelTramasNMEA.Checked)
            {
                groupBox_pantallaTramasNMEA.Visible = false;

                canvasMapa.redimensionarCanvas((canvasMapa.Width), (canvasMapa.Height + groupBox_pantallaTramasNMEA.Height));
               
            }
            else
            {
                groupBox_pantallaTramasNMEA.Visible = true;

                canvasMapa.redimensionarCanvas((canvasMapa.Width), (canvasMapa.Height - groupBox_pantallaTramasNMEA.Height));
                
            }
        }

        private void Form1_MaximizedBoundsChanged(object sender, EventArgs e)
        {
           
        }

        private void Form1_SizeChanged(object sender, EventArgs e)
        {
            canvasMapa.redimensionarCanvas((this.Width-tabControl_controles.Width) - 50, canvasMapa.Height);
        }

    }// fin de clase 
}

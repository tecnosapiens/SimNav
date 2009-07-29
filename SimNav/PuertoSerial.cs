using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;

using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;


namespace SimNav
{
    public class PuertoSerial
    {
        
        public PuertoSerial()
        {
            puertoSerial = new SerialPort();
            tiempoTransmision = 3000;
           
            velocidadPuertoSerial = "velocidad";
            nombrePuertoSerial = "nombrePuerto";
            bitDatosPuertoSerial = "bitDatos";
            bitParadaPuertoSerial = "bitParada";
            paridadPuertoSerial = "paridad";
            estadoPuertoSerial = false;

        }
        public void setPuertoSerial(string nombrePuerto, string bitParada, string bitDatos, string paridad, string velocidad, string tiempo)
        {
            tiempoTransmision = int.Parse(tiempo);
            velocidadPuertoSerial = velocidad;
            nombrePuertoSerial = nombrePuerto;
            bitDatosPuertoSerial = bitDatos;
            bitParadaPuertoSerial = bitParada;
            paridadPuertoSerial = paridad;

            puertoSerial = new SerialPort();

            puertoSerial.BaudRate = int.Parse(velocidad);
            puertoSerial.DataBits = int.Parse(bitDatos);
            puertoSerial.PortName = nombrePuerto;

            obtenerBitParada(bitParada);
            obtenerParidad(paridad);
            tiempoTransmision = int.Parse(tiempo);
            estadoPuertoSerial = true;


        }

        System.IO.Ports.SerialPort puertoSerial;
        int tiempoTransmision;
        string velocidadPuertoSerial;
        string nombrePuertoSerial;
        string bitDatosPuertoSerial;
        string bitParadaPuertoSerial;
        string paridadPuertoSerial;
        bool estadoPuertoSerial;

        //private Thread myThread;
      

        private void obtenerBitParada(string bitParada)
        {

            switch (bitParada)
            {
                case "1":
                    puertoSerial.StopBits = StopBits.One;

                    break;
                case "1.5":
                    puertoSerial.StopBits = StopBits.OnePointFive;
                    break;
                case "2":
                    puertoSerial.StopBits = StopBits.Two;
                    break;
                case "ninguno":
                    puertoSerial.StopBits = StopBits.None;
                    break;
                default:
                    break;
            }
        }

        private void obtenerParidad(string paridad)
        {
            switch(paridad)
            {
                case "Par":
                    puertoSerial.Parity = Parity.Odd;
                    break;
                case "Impar":
                    puertoSerial.Parity = Parity.Even;
                    break;
                case "Ninguno":
                    puertoSerial.Parity = Parity.None;
                    break;
                case "Marca":
                    puertoSerial.Parity = Parity.Mark;
                    break;
                case "Espacio":
                    puertoSerial.Parity = Parity.Space;
                    break;
                default:
                    break;
                      
            }
					  
        }


        public string getTiempoTransmision() { return tiempoTransmision.ToString(); }
        public int getTiempoTransmisionINT() { return tiempoTransmision; }
        public string getVelocidadPuerto() { return velocidadPuertoSerial; }
        public string getNombrePuerto() { return nombrePuertoSerial; }
        public string getBitDatos() { return bitDatosPuertoSerial; }
        public string getBitParada() { return bitParadaPuertoSerial; }
        public string getParidad() { return paridadPuertoSerial; }
        public bool get_estadoPuertoSerial()
        {
            return estadoPuertoSerial;
        }

        public bool get_estadoPuertoSerialAbierto()
        {
            return puertoSerial.IsOpen;
        }


        public void AbrirPuertoSerial()
		 {					 
			   try
			   {
				   puertoSerial.Open();
                  
			   }
               catch (Exception e)
               {
                   MessageBox.Show(e.Message, "Informacion", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
               }                       
		 }
        public void CerrarPuertoSerial()
        {
            try
            {
                puertoSerial.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Informacion", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }
        public void enviarData(string dato)
        {
            if (puertoSerial.IsOpen)
            {
                puertoSerial.WriteLine(dato + "\r");
            }
            
        }

        


      }// fin de clase
}

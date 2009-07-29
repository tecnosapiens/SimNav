using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimNav
{
    
    public class PuntoGeografico
    {
        double latitud;
        double longitud;


        public PuntoGeografico(double lat, double lon)
        {
            latitud = lat;
            longitud = lon;
        }

        public void set_latitud(double lat) { latitud = lat; }
        public void set_longitud(double lon) { longitud = lon; }
        public double get_latitud() { return latitud; }
        public double get_longitud() { return longitud; }





    }
}

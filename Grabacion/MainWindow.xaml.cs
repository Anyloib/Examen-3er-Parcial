﻿using System;
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

using NAudio;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using Microsoft.Win32;
using System.Windows.Threading;
using NAudio.Dsp;



namespace Grabacion
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        WaveIn waveIn;
        WaveFormat formato;
        WaveFileWriter writer;
        WaveOutEvent output;
        AudioFileReader reader;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnIniciar_Click(object sender, RoutedEventArgs e)
        {
            waveIn = new WaveIn();
            waveIn.WaveFormat = new WaveFormat(44100, 16, 1);
            formato = waveIn.WaveFormat;



            waveIn.DataAvailable += OnDataAvailable;
            waveIn.RecordingStopped += OnRecordingStopped;

            writer =
                new WaveFileWriter("sonido2.wav", formato);

            waveIn.StartRecording();
        }

        void OnRecordingStopped(object sender, StoppedEventArgs e)
        {
            writer.Dispose();

        }

        void OnDataAvailable (object sender, WaveInEventArgs e)
        {

            byte[] buffer = e.Buffer;
            int bytesGrabados = e.BytesRecorded;


            double acumulador = 0;
            double numMuestras = bytesGrabados / 2;
            int exponente = 1;
            int numeroMuestrasComplejas = 0;
            int bitsMaximos = 0;

            do //1,200
            {
                bitsMaximos = (int) Math.Pow(2, exponente);
                exponente++;
            } while (bitsMaximos < numMuestras);

            //bitsMaximos = 2048
            //exponente = 12

            //numeroMuestrasComplejas = 1024
            //exponente = 10

            exponente -= 2;
            numeroMuestrasComplejas = bitsMaximos / 2;

            Complex[] muestrasComplejas =
                new Complex[numeroMuestrasComplejas];

            for (int i=0; i < bytesGrabados; i+=2)
            {
                // byte i =  0 1 1 0 0 1 1 1
                //byte i+1 = 0 0 0 0 0 0 0 0 0 1 1 0 0 1 1 1
                // or      = 0 1 1 0 0 1 1 1 0 1 1 0 0 1 1 1
                short muestra =
                        (short)Math.Abs((buffer[i + 1] << 8)|buffer[i]);
                //lblMuestra.Text = muestra.ToString();
                //sldVolumen.Value = (double)muestra;

                float muestra32bits = (float)muestra / 32768.0f;
                sldVolumen.Value = Math.Abs(muestra32bits);

                if (i / 2 < numeroMuestrasComplejas)
                {
                    muestrasComplejas[i / 2].X = muestra32bits;
                }
                //acumulador += muestra;
                //numMuestras++;
            }
            //double promedio = acumulador / numMuestras;
            //sldVolumen.Value = promedio;
            //writer.Write(buffer, 0, bytesGrabados);

            FastFourierTransform.FFT(true, exponente, muestrasComplejas);
            float[] valoresAbsolutos = 
                new float[muestrasComplejas.Length];
            for(int i=0; i <muestrasComplejas.Length; i++)
            {
                valoresAbsolutos[i] = (float)
                    Math.Sqrt((muestrasComplejas[i].X * muestrasComplejas[i].X) +
                    (muestrasComplejas[i].Y * muestrasComplejas[i].Y));

            }

            int indiceMaximo =
                valoresAbsolutos.ToList().IndexOf(
                    valoresAbsolutos.Max());

            float frecuenciaFundamental =
                (float)(indiceMaximo * waveIn.WaveFormat.SampleRate) / (float)valoresAbsolutos.Length;
            
            lblFrecuenciaActual.Text = frecuenciaFundamental.ToString();

            if(frecuenciaFundamental > 100 && frecuenciaFundamental < 120)
            {
                lblFrecuenciaActual.Text = "A";
            }
            else if(frecuenciaFundamental > 120 && frecuenciaFundamental < 140)
            {
                lblFrecuenciaActual.Text = "B";
            }
            else if(frecuenciaFundamental > 140 && frecuenciaFundamental < 160)
            {
                lblFrecuenciaActual.Text = "C";
            }
            else if(frecuenciaFundamental > 160 && frecuenciaFundamental < 180)
            {
                lblFrecuenciaActual.Text = "D";
            }
            else if(frecuenciaFundamental > 180 && frecuenciaFundamental < 200)
            {
                lblFrecuenciaActual.Text = "E";
            }
            else if(frecuenciaFundamental > 200 && frecuenciaFundamental < 220)
            {
                lblFrecuenciaActual.Text = "F";
            }
            else if (frecuenciaFundamental > 220 && frecuenciaFundamental < 240)
            {
                lblFrecuenciaActual.Text = "G";
            }
            else if (frecuenciaFundamental > 260 && frecuenciaFundamental < 280)
            {
                lblFrecuenciaActual.Text = "H";
            }
            else if (frecuenciaFundamental > 280 && frecuenciaFundamental < 300)
            {
                lblFrecuenciaActual.Text = "I";
            }
            else if (frecuenciaFundamental > 300 && frecuenciaFundamental < 320)
            {
                lblFrecuenciaActual.Text = "J";
            }
            else if(frecuenciaFundamental > 320 && frecuenciaFundamental < 340)
            {
                lblFrecuenciaActual.Text = "K";
            }
            else if (frecuenciaFundamental > 340 && frecuenciaFundamental < 360)
            {
                lblFrecuenciaActual.Text = "L";
            }
            else if (frecuenciaFundamental > 360 && frecuenciaFundamental < 380)
            {
                lblFrecuenciaActual.Text = "M";
            }
            else if (frecuenciaFundamental > 380 && frecuenciaFundamental < 400)
            {
                lblFrecuenciaActual.Text = "N";
            }
            else if(frecuenciaFundamental > 440 && frecuenciaFundamental < 460)
            {
                lblFrecuenciaActual.Text = "O";
            }
            else if(frecuenciaFundamental > 460 && frecuenciaFundamental < 480)
            {
                lblFrecuenciaActual.Text = "P";
            }
            else if(frecuenciaFundamental > 480 && frecuenciaFundamental < 500)
            {
                lblFrecuenciaActual.Text = "Q";
            }
            else if(frecuenciaFundamental > 500 && frecuenciaFundamental < 520)
            {
                lblFrecuenciaActual.Text = "R";
            }
            else if(frecuenciaFundamental > 520 && frecuenciaFundamental < 540)
            {
                lblFrecuenciaActual.Text = "S";
            }
            else if(frecuenciaFundamental > 540 && frecuenciaFundamental < 560)
            {
                lblFrecuenciaActual.Text = "T";
            }
            else if(frecuenciaFundamental > 560 && frecuenciaFundamental < 580)
            {
                lblFrecuenciaActual.Text = "U";
            }
            else if (frecuenciaFundamental > 580 && frecuenciaFundamental < 600)
            {
                lblFrecuenciaActual.Text = "V";
            }
            else if (frecuenciaFundamental > 600 && frecuenciaFundamental < 620)
            {
                lblFrecuenciaActual.Text = "W";
            }
            else if (frecuenciaFundamental > 620 && frecuenciaFundamental < 240)
            {
                lblFrecuenciaActual.Text = "X";
            }
            else if (frecuenciaFundamental > 240 && frecuenciaFundamental < 260)
            {
                lblFrecuenciaActual.Text = "Y";
            }
            else if (frecuenciaFundamental > 260 && frecuenciaFundamental < 280)
            {
                lblFrecuenciaActual.Text = "Z";
            }
        }

        private void btnDetener_Click(object sender, RoutedEventArgs e)
        {
            waveIn.StopRecording();
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            output = new WaveOutEvent();
            reader = new AudioFileReader("sonido2.wav");
            output.Init(reader);
            output.Play();

        }
    }
}

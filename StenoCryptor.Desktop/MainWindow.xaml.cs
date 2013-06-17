using Microsoft.Win32;
using StenoCryptor.Commons;
using StenoCryptor.Commons.Constants;
using StenoCryptor.Commons.Enums;
using StenoCryptor.Desktop.Helpers;
using StenoCryptor.Engyne.CryptAlgorithms;
using StenoCryptor.Engyne.Detectors;
using StenoCryptor.Engyne.Embeders;
using StenoCryptor.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mime;
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

namespace StenoCryptor.Desktop
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (!File.Exists(ContainerTextBox.Text))
                MessageBox.Show("Container is not selected.");

            try
            {
                //Inject dependencies
                CryptType cryptType = (CryptType)System.Enum.Parse(typeof(CryptType), CryptTypeComboBox.Text);
                EmbedType embedType = (EmbedType)System.Enum.Parse(typeof(EmbedType), EmbedType.Text);
                ICryptor cryptor = _algorithmFactory.GetInstance(cryptType);
                IEmbeder embeder = _embederFactory.GetInstance((EmbedType)System.Enum.Parse(typeof(EmbedType), EmbedType.Text));

                //Create container and key
                string contentType = DefineContentType(ContainerTextBox.Text);
                Container container = new Container(File.Open(ContainerTextBox.Text, FileMode.Open), contentType);
                StenoCryptor.Commons.Key key = KeyMakerHelper.GenerateKey(container, MessageTextBox.Text, cryptType, embedType, cryptor, KeyTextBox.Text);

                //Embed DWM
                DwmProcessorHelper.EmbedDwm(cryptor, embeder, MessageTextBox.Text, key, container);

                //Prepare streams to be saved to the files
                Stream keyStream = SerializeHelper.SerializeBinary(key);
                Dictionary<string, Stream> files = new Dictionary<string, Stream>();

                files.Add(Path.GetFileName(ContainerTextBox.Text), container.InputStream);
                files.Add(Constants.DEFAULT_KEY_NAME, keyStream);

                //Packing files to the zip file
                string zipFileName = ZipHelper.CompressFiles(files);

                SaveFile(zipFileName);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void SaveFile(string zipFileName)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();

            if (saveFileDialog1.ShowDialog() == true)
            {
                File.Copy(zipFileName, saveFileDialog1.FileName);
            }
        }


        private string DefineContentType(string fileName)
        {
            string extension = System.IO.Path.GetExtension(fileName);

            switch (extension)
            {
                case ".jpg":
                case ".jpeg":
                    return "image/jpg";
                case ".png":
                    return "image/png";
                default:
                    throw new NotSupportedException("File type is not supported.");
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog().Value) // Test result.
            {
                ContainerTextBox.Text = ofd.FileName;
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            if (!File.Exists(DetectContainerTextBox.Text))
                MessageBox.Show("Select file!");
            else
            {
                IDetector detector = _detectorFactory.GetInstance();
                if (DwmProcessorHelper.DetectDwm(detector, File.Open(DetectContainerTextBox.Text, FileMode.Open)))
                {
                    MessageBox.Show("DWM detected!");
                }
                else
                {
                    MessageBox.Show("DWM not detected!");
                }
            }
        }

        IAlgorithmFactory _algorithmFactory = new AlgorithmFactory();
        IEmbederFactory _embederFactory = new EmbederFactory();
        IDetectorFactory _detectorFactory = new DetectorFactory();

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog().Value) // Test result.
            {
                ExtractContainerTextBox.Text = ofd.FileName;
            }
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog().Value) // Test result.
            {
                DetectContainerTextBox.Text = ofd.FileName;
            }
        }

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog().Value) // Test result.
            {
                ExtractKeyTextBox.Text = ofd.FileName;
            }
        }

        private void Button_Click_6(object sender, RoutedEventArgs e)
        {
            if (!File.Exists(ExtractKeyTextBox.Text) || !File.Exists(ExtractContainerTextBox.Text))
                MessageBox.Show("Select file!");
            else
            {
                StenoCryptor.Commons.Key key = SerializeHelper.DeserializeBinary(File.Open(ExtractKeyTextBox.Text, FileMode.Open)) as StenoCryptor.Commons.Key;
                if (key == null)
                {
                    MessageBox.Show("Key is not correct!");
                    return;
                }

                IEmbeder embeder = _embederFactory.GetInstance(key.EmbedType);
                ICryptor cryptor = _algorithmFactory.GetInstance(key.CryptType);
                Container container = new Container(File.Open(ExtractContainerTextBox.Text, FileMode.Open), DefineContentType(ExtractContainerTextBox.Text));
                string message = DwmProcessorHelper.ExtractDwm(embeder, cryptor, key, container);

                ResultTextBlock.Text = message;
                ResultTextBlock.Visibility = System.Windows.Visibility.Visible;
            }
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace SEA_LNA
{
    public partial class Form1 : Form
    {

        public String Sh = null;
        public String CalcHash = null;
        public String HashFile = null;
        public int HashFileLen = 0;
        public String FirstFolder = null;
        public String PathString = null;
        public String HashString = null;
        public StringBuilder ListaDeArquivos = new StringBuilder("");
        public StringBuilder ListaDeArquivosIguais = new StringBuilder("");
        public Int32 ContArquivos = 1;
        public Dictionary<string, string> DicArqQtdBytDif = new Dictionary<string, string>();               //( K->(bytes), V->(Flag+Path) )
        public Dictionary<string, string> DicArqsHashs_ArqQtdBytIgu = new Dictionary<string, string>();     //( K->(bytes), V->(Path+Hash\r)(n) )
        public Dictionary<string, string> DicDifArqsHashs = new Dictionary<string, string>();               //( K->(Hash), V->(Flag+Path) )
        public Dictionary<string, string> DicArqsHashIgu = new Dictionary<string, string>();                //( K->(Hash), V->(Path\r)(n) )

        public Form1()
        {
            InitializeComponent();
            InitializeOpenFileDialog();
        }

        private void InitializeOpenFileDialog()
        {
            this.openFileDialog1.InitialDirectory = "c:\\";
            this.openFileDialog1.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
            this.openFileDialog1.FilterIndex = 2;
            this.openFileDialog1.RestoreDirectory = true;
            this.openFileDialog1.AutoUpgradeEnabled = true;
            this.openFileDialog1.Multiselect = true;
            this.openFileDialog1.Title = "(SEA) - Lista Nomes dos Arquivos";
            this.openFileDialog1.FileName = "";
        }

        private void OpenFileDialog_Click(object sender, EventArgs e)
        {

            CalcHash = null;
            ContArquivos = 1;
            DicArqQtdBytDif.Clear();
            DicArqsHashIgu.Clear();
            DicArqsHashs_ArqQtdBytIgu.Clear();
            DicDifArqsHashs.Clear();
            FirstFolder = null;
            HashFile = null;
            HashFileLen = 0;
            HashString = null;
            ListaDeArquivos.Clear();
            ListaDeArquivosIguais.Clear();
            PathString = null;

            ListaDeArquivos.Append(DateTime.Now.ToString("yyyy MM dd hh mm ss fff")
                               + Environment.NewLine
                               + Environment.NewLine);

            ListaDeArquivos.Append("Ítem\tDiretório\tDtUtcCriação\t"
                               + "Arquivo\tDtUtcCriação\tDtUtcÚltimoAcesso\tDtUtcÚltimaAlteração\t"
                               + "Atributos\tSóDeLeitura\tNúmeroBytes\tHashDoArquivo"
                               + Environment.NewLine);

            using (FolderBrowserDialog dialog = new FolderBrowserDialog())
            {
                dialog.Description = "Selecione a pasta a partir da qual quer listar os arquivos";
                dialog.ShowNewFolderButton = false;
                dialog.RootFolder = Environment.SpecialFolder.MyComputer;
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    FirstFolder = dialog.SelectedPath;
                    if (!backgroundWorker1.IsBusy)
                    {
                        backgroundWorker1.RunWorkerAsync();
                    }
                }
            }
        }

        private void BackgroundWorker1_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            var Arquivos = Directory.EnumerateFiles(FirstFolder, "*.*");
            foreach (string Arquivo in Arquivos)
            {
                String HashDoArq = null;
                System.IO.FileInfo fi = null;
                try
                {
                    fi = new System.IO.FileInfo(Arquivo);
                }
                catch (Exception)
                {
                    throw;
                }

                if (chkBCalcularHash.Checked)
                {
                    Calcula_Hash(Arquivo);
                    HashDoArq = HashFile;
                    HashFile = null;
                }

                try
                {
                    //Armazena a qtd de bytes do arquivo = k, seu flag de único com esta qtd de bytes + tab + seu path completo = V
                    DicArqQtdBytDif.Add(fi.Length.ToString(), "1" + "\t" + Arquivo);
                }
                catch (ArgumentException)
                {
                    //Se o flag de único com esta qtd de bytes ainda = 1, é a 
                    //primeira vez que outro arquivo com mesma qtd bytes é encontrado.
                    //Altera-se o flag de 1 para 0 no DicArqQtdBytDif indicando que esta quantidade não é única
                    //Calcula os hashs dos respectivos arquivos e armazena suas respectivas informações
                    if (DicArqQtdBytDif[fi.Length.ToString()].StartsWith("1"))
                    {
                        //Altera o flag de 1 para 0
                        DicArqQtdBytDif[fi.Length.ToString()] = "0\t" + (DicArqQtdBytDif[fi.Length.ToString()]).Remove(0, 2);
                        //Calcula hash do primeiro arquivo "(DicArqQtdBytDif[fi.Length.ToString()]).Remove(0, 2)"
                        Calcula_Hash((DicArqQtdBytDif[fi.Length.ToString()]).Remove(0, 2));
                        String HashPrimeiroArq = HashFile;
                        //Calcula hash do segundo arquivo  "Arquivo"      
                        Calcula_Hash(Arquivo);
                        String HashSegundoArq = HashFile;
                        //Inicializa o dicionário de Arquivos e Hashs dos arquivos com quantidades iguais de bytes
                        DicArqsHashs_ArqQtdBytIgu.Add(fi.Length.ToString(), Arquivo);
                        //Armazena os dados do primeiro e do segundo arquivo no dicionário DicArqsHashs_ArqQtdBytIgu com seus respectivos hashs calculados
                        DicArqsHashs_ArqQtdBytIgu[fi.Length.ToString()] = (DicArqQtdBytDif[fi.Length.ToString()]).Remove(0, 2)
                                                                          + "\t" + HashPrimeiroArq + "\r"
                                                                          + DicArqsHashs_ArqQtdBytIgu[fi.Length.ToString()]
                                                                          + "\t" + HashSegundoArq;
                    }
                    else
                    {
                        //Calcula hash do segundo arquivo  "Arquivo"
                        Calcula_Hash(Arquivo);
                        String HashSegundoArq = HashFile;
                        //Armazena os dados do segundo arquivo no dicionário DicArqsHashs_ArqQtdBytIgu e seus hash calculado
                        DicArqsHashs_ArqQtdBytIgu[fi.Length.ToString()] = DicArqsHashs_ArqQtdBytIgu[fi.Length.ToString()]
                                                                          + "\r" + Arquivo + "\t" + HashSegundoArq;
                    }
                    HashFile = null;
                }

                ListaDeArquivos.Append(ContArquivos.ToString() + "\t" + FirstFolder + "\t" + fi.Directory.CreationTimeUtc.ToString() + "\t"
                                                + Arquivo.Substring(FirstFolder.Length + 1) + "\t" + fi.CreationTimeUtc.ToString() + "\t"
                                                + fi.LastAccessTimeUtc.ToString() + "\t" + fi.LastWriteTimeUtc.ToString() + "\t"
                                                + fi.Attributes.ToString() + "\t" + fi.IsReadOnly.ToString() + "\t" + fi.Length.ToString() + "\t"
                                                + HashDoArq + Environment.NewLine);

                ContArquivos = ContArquivos + 1;
                backgroundWorker1.ReportProgress(10);
            }
            DirSearch(FirstFolder);
        }

        void DirSearch(string FirstFolder)
        {
            try
            {
                //foreach (string SubFolder in Directory.GetDirectories(FirstFolder))
                List<string> dirs = new List<string>(Directory.EnumerateDirectories(FirstFolder));
                foreach (string SubFolder in dirs)                
                {
                    //DirectoryInfo info = new DirectoryInfo(SubFolder);
                    //if ((info.Attributes & FileAttributes.Hidden) != FileAttributes.Hidden)
                    if (!SubFolder.EndsWith("\\$RECYCLE.BIN") && !SubFolder.EndsWith("\\System Volume Information") && !SubFolder.EndsWith("\\Recovery"))
                    {
                        //foreach (string Arquivo in Directory.GetFiles(SubFolder, "*.*"))
                        var Arquivos = Directory.EnumerateFiles(SubFolder, "*.*");
                        foreach (string Arquivo in Arquivos)
                        {
                            String HashDoArq = null;
                            System.IO.FileInfo fi = null;
                            try
                            {
                                fi = new System.IO.FileInfo(Arquivo);
                            }
                            catch (Exception)
                            {
                                throw;
                            }
                            if (chkBCalcularHash.Checked)
                            {
                                Calcula_Hash(Arquivo);
                                HashDoArq = HashFile;
                                HashFile = null;
                            }
                            try
                            {
                                //Armazena a qtd de bytes do arquivo = k, seu flag de único com esta qtd de bytes + tab + seu path completo = V
                                DicArqQtdBytDif.Add(fi.Length.ToString(), "1" + "\t" + Arquivo);
                            }
                            catch (ArgumentException)
                            {
                                //Se o flag de único com esta qtd de bytes ainda = 1, é a 
                                //primeira vez que outro arquivo com mesma qtd bytes é encontrado.
                                //Altera-se o flag de 1 para 0 no DicArqQtdBytDif indicando que esta quantidade não é única
                                //Calcula os hashs dos respectivos arquivos e armazena suas respectivas informações
                                if (DicArqQtdBytDif[fi.Length.ToString()].StartsWith("1"))
                                {
                                    //Altera o flag de 1 para 0
                                    DicArqQtdBytDif[fi.Length.ToString()] = "0\t" + (DicArqQtdBytDif[fi.Length.ToString()]).Remove(0, 2);
                                    //Calcula hash do primeiro arquivo "(DicArqQtdBytDif[fi.Length.ToString()]).Remove(0, 2)"
                                    Calcula_Hash((DicArqQtdBytDif[fi.Length.ToString()]).Remove(0, 2));
                                    String HashPrimeiroArq = HashFile;
                                    //Calcula hash do segundo arquivo  "Arquivo"      
                                    Calcula_Hash(Arquivo);
                                    String HashSegundoArq = HashFile;
                                    //Inicializa o dicionário de Arquivos e Hashs dos arquivos com quantidades iguais de bytes
                                    DicArqsHashs_ArqQtdBytIgu.Add(fi.Length.ToString(), Arquivo);
                                    //Armazena os dados do primeiro e do segundo arquivo no dicionário DicArqsHashs_ArqQtdBytIgu com seus respectivos hashs calculados
                                    DicArqsHashs_ArqQtdBytIgu[fi.Length.ToString()] = (DicArqQtdBytDif[fi.Length.ToString()]).Remove(0, 2)
                                                                                       + "\t" + HashPrimeiroArq + "\r"
                                                                                       + DicArqsHashs_ArqQtdBytIgu[fi.Length.ToString()]
                                                                                       + "\t" + HashSegundoArq;
                                }
                                else
                                {
                                    //Calcula hash do segundo arquivo  "Arquivo"
                                    Calcula_Hash(Arquivo);
                                    String HashSegundoArq = HashFile;
                                    //Armazena os dados do segundo arquivo no dicionário DicArqsHashs_ArqQtdBytIgu e seus hash calculado
                                    DicArqsHashs_ArqQtdBytIgu[fi.Length.ToString()] = DicArqsHashs_ArqQtdBytIgu[fi.Length.ToString()]
                                                                                      + "\r" + Arquivo
                                                                                      + "\t" + HashSegundoArq;
                                }
                                HashFile = null;
                            }

                            ListaDeArquivos.Append(ContArquivos.ToString() + "\t" + SubFolder + "\t" + fi.Directory.CreationTimeUtc.ToString() + "\t"
                                                            + Arquivo.Substring(SubFolder.Length + 1) + "\t" + fi.CreationTimeUtc.ToString() + "\t"
                                                            + fi.LastAccessTimeUtc.ToString() + "\t" + fi.LastWriteTimeUtc.ToString() + "\t"
                                                            + fi.Attributes.ToString() + "\t" + fi.IsReadOnly.ToString() + "\t" + fi.Length.ToString() + "\t"
                                                            + HashDoArq + Environment.NewLine);
                            ContArquivos = ContArquivos + 1;
                            backgroundWorker1.ReportProgress(10);
                        }
                        DirSearch(SubFolder);
                    }
                }
            }
            catch (System.Exception)
            {
                //throw;
            }
        }

        private void BackgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            String Cont = (ContArquivos - 1).ToString();
            richTextBox1.Text = "Processando arquivos: > " + Cont + " <";
        }

        private void CancelaListagem_Click(object sender, EventArgs e)
        {
            // Cancel the asynchronous operation.
            this.backgroundWorker1.CancelAsync();
            // Disable the Cancel button.
            CancelaListagem.Enabled = false;
            Application.Exit();
        }

        private void BackgroundWorker1_RunWorkerCompleted_1(object sender, RunWorkerCompletedEventArgs e)
        {
            DicArqQtdBytDif.Clear();
            //Marcado provisóriamente para debug retornar este código depois
            try
            {
                richTextBox1.Text = ListaDeArquivos.ToString()
                                    + Environment.NewLine
                                    + DateTime.Now.ToString("yyyy MM dd hh mm ss fff")
                                    + Environment.NewLine;
            }
            catch (Exception)
            {
                throw;
            }

            //Inicia o processo de análise nos arquivos com a mesma quantidade de bytes
            int Cont = 0;
            foreach (var regArqsHashs in DicArqsHashs_ArqQtdBytIgu)
            {
                Cont += 1;
                string[] ArqsHashs = (regArqsHashs.Value).Split(new Char[] { '\r' });
                foreach (string ArqHash in ArqsHashs)
                {
                    PathString = ArqHash.Substring(0, ((ArqHash.Length) - (HashFileLen + 1)));
                    HashString = ArqHash.Substring(((ArqHash.Length) - (HashFileLen)), HashFileLen);
                    //////////////////////////////////////////////////////////////////////////////////////////////////////////
                    try
                    {
                        //Armazena HashString do arquivo = k, seu flag de único com este hash + tab + PathString como seu path completo = V
                        DicDifArqsHashs.Add(HashString, "1" + "\t" + PathString);
                    }
                    catch (ArgumentException)
                    {
                        //Se o flag de único com este hash ainda = 1, é a 
                        //primeira vez que outro arquivo com mesmo hash é encontrado.
                        //Altera-se o flag de 1 para 0 no DicDifArqsHashs indicando que este hash não é único
                        if (DicDifArqsHashs[HashString].StartsWith("1"))
                        {
                            //Altera o flag de 1 para 0
                            DicDifArqsHashs[HashString] = "0\t" + (DicDifArqsHashs[HashString]).Remove(0, 2);
                            //Inicializa o dicionário de arquivos com hashs iguais
                            DicArqsHashIgu.Add(HashString, PathString);
                            //Armazena os dados do primeiro e do segundo arquivo no dicionário DicArqHashIgu com seus respectivos hashs calculados
                            DicArqsHashIgu[HashString] = (DicDifArqsHashs[HashString]).Remove(0, 2)
                                                         + "\r"
                                                         + DicArqsHashIgu[HashString];
                        }
                        else
                        {
                            //Armazena os dados do segundo arquivo no dicionário DicArqHashIgu e seu hash calculado
                            DicArqsHashIgu[HashString] = DicArqsHashIgu[HashString] + "\r" + PathString;
                        }
                    }
                    /////////////////////////////////////////////////////////////////////////////////////////////////////////
                }
            }
            ////////////////////////////////////////////////////////////////////////////////////////

            ListaDeArquivosIguais.Append(DateTime.Now.ToString("yyyy MM dd hh mm ss fff")
                                        + Environment.NewLine
                                        + Environment.NewLine);

            int Cont1 = 0;
            foreach (var regArqsHashIgu in DicArqsHashIgu)
            {
                Cont1 += 1;
                string[] RegArqs = (regArqsHashIgu.Value).Split(new Char[] { '\r' });
                foreach (string regArq in RegArqs)
                {
                    ListaDeArquivosIguais.Append("[" + Cont1.ToString() + "]\t" + regArq + Environment.NewLine);
                    PathString = regArq;
                }
            }
            ////////////////////////////////////////////////////////////////////////////////////////
            ListaDeArquivosIguais.Append(Environment.NewLine
                                        + Environment.NewLine
                                        + DateTime.Now.ToString("yyyy MM dd hh mm ss fff"));

            ListaDeArquivosIguais.Append("----Fim de Processamento----"
                                        + Environment.NewLine);

            richTextBox2.Text = ListaDeArquivosIguais.ToString();
        }

        void Calcula_Hash(string Arquivo)
        {
            Stream myStream = null;
            try
            {
                using (myStream)
                {
                    FileStream file1 = null;
                    try
                    {
                        String ValHex = null;
                        HashFile = null;
                        file1 = new FileStream(Arquivo, FileMode.Open, FileAccess.Read);
                        byte[] ResultHashValueSHA;
                        switch (Sh)
                        {
                            case "160":
                                HashFileLen = 40; //20 x 2 + 19 = 59
                                SHA1 sha160 = new SHA1Managed();
                                ResultHashValueSHA = sha160.ComputeHash(file1);
                                break;
                            case "256":
                                HashFileLen = 64; //32 x 2 + 31 = 95
                                SHA256 sha256 = new SHA256Managed();
                                ResultHashValueSHA = sha256.ComputeHash(file1);
                                break;
                            case "384":
                                HashFileLen = 96; //48 x 2 + 47 = 143
                                SHA384 sha384 = new SHA384Managed();
                                ResultHashValueSHA = sha384.ComputeHash(file1);
                                break;
                            case "512":
                                HashFileLen = 128; //64 x 2 + 63 = 191
                                SHA512 sha512 = new SHA512Managed();
                                ResultHashValueSHA = sha512.ComputeHash(file1);
                                break;
                            default:
                                HashFileLen = 40; //20 x 2 + 19 = 59
                                SHA1 shaD = new SHA1Managed();
                                ResultHashValueSHA = shaD.ComputeHash(file1);
                                break;
                        }
                        file1.Close();
                        for (int b = 0; b < ResultHashValueSHA.Length; b++)
                        {
                            if (string.Format(string.Format("{0:X}", ResultHashValueSHA[b])).Length < 2)
                            {
                                ValHex = "0" + string.Format("{0:X}", ResultHashValueSHA[b]);
                            }
                            else
                            {
                                ValHex = string.Format("{0:X}", ResultHashValueSHA[b]);
                            }
                            HashFile = HashFile + ValHex;
                            // Incere - na sequencia hexa do hash calculado
                            //if (b < ResultHashValueSHA.Length - 1)
                            //{
                            //    HashFile = HashFile + "-";
                            //}
                        }
                    }
                    catch (Exception ex)
                    {
                        try
                        {
                            file1.Close();
                        }
                        catch (Exception)
                        {
                            MessageBox.Show("Erro: ..: " + ex.Message);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro: Não foi possível ler o arquivo do disco. Erro original: " + ex.Message);
            }
        }

        private void RBSHA1_CheckedChanged_1(object sender, EventArgs e)
        {
            Sh = "160";
            HashFileLen = 40; //20 x 2 + 19 = 59
        }

        private void RBSHA256_CheckedChanged_1(object sender, EventArgs e)
        {
            Sh = "256";
            HashFileLen = 64; //32 x 2 + 31 = 95
        }

        private void RBSHA384_CheckedChanged_1(object sender, EventArgs e)
        {
            Sh = "384";
            HashFileLen = 96; //48 x 2 + 47 = 143
        }

        private void RB512_CheckedChanged(object sender, EventArgs e)
        {
            Sh = "512";
            HashFileLen = 128; //64 x 2 + 63 = 191
        }
    }
}


//////////////////////////////////
//////////////////////////////////

////////////using System;
////////////using System.Collections.Generic;

////////////public class Example
////////////{
////////////    public static void Main()
////////////    {
////////////        // Create a new dictionary of strings, with string keys.
////////////        //
////////////        Dictionary<string, string> openWith = 
////////////            new Dictionary<string, string>();

////////////        // Add some elements to the dictionary. There are no 
////////////        // duplicate keys, but some of the values are duplicates.
////////////        openWith.Add("txt", "notepad.exe");
////////////        openWith.Add("bmp", "paint.exe");
////////////        openWith.Add("dib", "paint.exe");
////////////        openWith.Add("rtf", "wordpad.exe");

////////////        // The Add method throws an exception if the new key is 
////////////        // already in the dictionary.
////////////        try
////////////        {
////////////            openWith.Add("txt", "winword.exe");
////////////        }
////////////        catch (ArgumentException)
////////////        {
////////////            Console.WriteLine("An element with Key = \"txt\" already exists.");
////////////        }

////////////        // The Item property is another name for the indexer, so you 
////////////        // can omit its name when accessing elements. 
////////////        Console.WriteLine("For key = \"rtf\", value = {0}.", 
////////////            openWith["rtf"]);

////////////        // The indexer can be used to change the value associated
////////////        // with a key.
////////////        openWith["rtf"] = "winword.exe";
////////////        Console.WriteLine("For key = \"rtf\", value = {0}.", 
////////////            openWith["rtf"]);

////////////        // If a key does not exist, setting the indexer for that key
////////////        // adds a new key/value pair.
////////////        openWith["doc"] = "winword.exe";

////////////        // The indexer throws an exception if the requested key is
////////////        // not in the dictionary.
////////////        try
////////////        {
////////////            Console.WriteLine("For key = \"tif\", value = {0}.", 
////////////                openWith["tif"]);
////////////        }
////////////        catch (KeyNotFoundException)
////////////        {
////////////            Console.WriteLine("Key = \"tif\" is not found.");
////////////        }

////////////        // When a program often has to try keys that turn out not to
////////////        // be in the dictionary, TryGetValue can be a more efficient 
////////////        // way to retrieve values.
////////////        string value = "";
////////////        if (openWith.TryGetValue("tif", out value))
////////////        {
////////////            Console.WriteLine("For key = \"tif\", value = {0}.", value);
////////////        }
////////////        else
////////////        {
////////////            Console.WriteLine("Key = \"tif\" is not found.");
////////////        }

////////////        // ContainsKey can be used to test keys before inserting 
////////////        // them.
////////////        if (!openWith.ContainsKey("ht"))
////////////        {
////////////            openWith.Add("ht", "hypertrm.exe");
////////////            Console.WriteLine("Value added for key = \"ht\": {0}", 
////////////                openWith["ht"]);
////////////        }

////////////        // When you use foreach to enumerate dictionary elements,
////////////        // the elements are retrieved as KeyValuePair objects.
////////////        Console.WriteLine();
////////////        foreach( KeyValuePair<string, string> kvp in openWith )
////////////        {
////////////            Console.WriteLine("Key = {0}, Value = {1}", 
////////////                kvp.Key, kvp.Value);
////////////        }

////////////        // To get the values alone, use the Values property.
////////////        Dictionary<string, string>.ValueCollection valueColl =
////////////            openWith.Values;

////////////        // The elements of the ValueCollection are strongly typed
////////////        // with the type that was specified for dictionary values.
////////////        Console.WriteLine();
////////////        foreach( string s in valueColl )
////////////        {
////////////            Console.WriteLine("Value = {0}", s);
////////////        }

////////////        // To get the keys alone, use the Keys property.
////////////        Dictionary<string, string>.KeyCollection keyColl =
////////////            openWith.Keys;

////////////        // The elements of the KeyCollection are strongly typed
////////////        // with the type that was specified for dictionary keys.
////////////        Console.WriteLine();
////////////        foreach( string s in keyColl )
////////////        {
////////////            Console.WriteLine("Key = {0}", s);
////////////        }

////////////        // Use the Remove method to remove a key/value pair.
////////////        Console.WriteLine("\nRemove(\"doc\")");
////////////        openWith.Remove("doc");

////////////        if (!openWith.ContainsKey("doc"))
////////////        {
////////////            Console.WriteLine("Key \"doc\" is not found.");
////////////        }
////////////    }
////////////}

/////////////* This code example produces the following output:

////////////An element with Key = "txt" already exists.
////////////For key = "rtf", value = wordpad.exe.
////////////For key = "rtf", value = winword.exe.
////////////Key = "tif" is not found.
////////////Key = "tif" is not found.
////////////Value added for key = "ht": hypertrm.exe

////////////Key = txt, Value = notepad.exe
////////////Key = bmp, Value = paint.exe
////////////Key = dib, Value = paint.exe
////////////Key = rtf, Value = winword.exe
////////////Key = doc, Value = winword.exe
////////////Key = ht, Value = hypertrm.exe

////////////Value = notepad.exe
////////////Value = paint.exe
////////////Value = paint.exe
////////////Value = winword.exe
////////////Value = winword.exe
////////////Value = hypertrm.exe

////////////Key = txt
////////////Key = bmp
////////////Key = dib
////////////Key = rtf
////////////Key = doc
////////////Key = ht

////////////Remove("doc")
////////////Key "doc" is not found.
//////////// */


//////////////////////////////////
//////////////////////////////////



////////////////using System;
////////////////using System.Collections.Generic;

////////////////public class Example
////////////////{
////////////////    public static void Main()
////////////////    {
////////////////         Create a new sorted dictionary of strings, with string
////////////////         keys.
////////////////        SortedDictionary<string, string> openWith = 
////////////////            new SortedDictionary<string, string>();

////////////////         Add some elements to the dictionary. There are no 
////////////////         duplicate keys, but some of the values are duplicates.
////////////////        openWith.Add("txt", "notepad.exe");
////////////////        openWith.Add("bmp", "paint.exe");
////////////////        openWith.Add("dib", "paint.exe");
////////////////        openWith.Add("rtf", "wordpad.exe");

////////////////         The Add method throws an exception if the new key is 
////////////////         already in the dictionary.
////////////////        try
////////////////        {
////////////////            openWith.Add("txt", "winword.exe");
////////////////        }
////////////////        catch (ArgumentException)
////////////////        {
////////////////            Console.WriteLine("An element with Key = \"txt\" already exists.");
////////////////        }

////////////////         The Item property is another name for the indexer, so you 
////////////////         can omit its name when accessing elements. 
////////////////        Console.WriteLine("For key = \"rtf\", value = {0}.", 
////////////////            openWith["rtf"]);

////////////////         The indexer can be used to change the value associated
////////////////         with a key.
////////////////        openWith["rtf"] = "winword.exe";
////////////////        Console.WriteLine("For key = \"rtf\", value = {0}.", 
////////////////            openWith["rtf"]);

////////////////         If a key does not exist, setting the indexer for that key
////////////////         adds a new key/value pair.
////////////////        openWith["doc"] = "winword.exe";

////////////////         The indexer throws an exception if the requested key is
////////////////         not in the dictionary.
////////////////        try
////////////////        {
////////////////            Console.WriteLine("For key = \"tif\", value = {0}.", 
////////////////                openWith["tif"]);
////////////////        }
////////////////        catch (KeyNotFoundException)
////////////////        {
////////////////            Console.WriteLine("Key = \"tif\" is not found.");
////////////////        }

////////////////         When a program often has to try keys that turn out not to
////////////////         be in the dictionary, TryGetValue can be a more efficient 
////////////////         way to retrieve values.
////////////////        string value = "";
////////////////        if (openWith.TryGetValue("tif", out value))
////////////////        {
////////////////            Console.WriteLine("For key = \"tif\", value = {0}.", value);
////////////////        }
////////////////        else
////////////////        {
////////////////            Console.WriteLine("Key = \"tif\" is not found.");
////////////////        }

////////////////         ContainsKey can be used to test keys before inserting 
////////////////         them.
////////////////        if (!openWith.ContainsKey("ht"))
////////////////        {
////////////////            openWith.Add("ht", "hypertrm.exe");
////////////////            Console.WriteLine("Value added for key = \"ht\": {0}", 
////////////////                openWith["ht"]);
////////////////        }

////////////////         When you use foreach to enumerate dictionary elements,
////////////////         the elements are retrieved as KeyValuePair objects.
////////////////        Console.WriteLine();
////////////////        foreach( KeyValuePair<string, string> kvp in openWith )
////////////////        {
////////////////            Console.WriteLine("Key = {0}, Value = {1}", 
////////////////                kvp.Key, kvp.Value);
////////////////        }

////////////////         To get the values alone, use the Values property.
////////////////        SortedDictionary<string, string>.ValueCollection valueColl = 
////////////////            openWith.Values;

////////////////         The elements of the ValueCollection are strongly typed
////////////////         with the type that was specified for dictionary values.
////////////////        Console.WriteLine();
////////////////        foreach( string s in valueColl )
////////////////        {
////////////////            Console.WriteLine("Value = {0}", s);
////////////////        }

////////////////         To get the keys alone, use the Keys property.
////////////////        SortedDictionary<string, string>.KeyCollection keyColl = 
////////////////            openWith.Keys;

////////////////         The elements of the KeyCollection are strongly typed
////////////////         with the type that was specified for dictionary keys.
////////////////        Console.WriteLine();
////////////////        foreach( string s in keyColl )
////////////////        {
////////////////            Console.WriteLine("Key = {0}", s);
////////////////        }

////////////////         Use the Remove method to remove a key/value pair.
////////////////        Console.WriteLine("\nRemove(\"doc\")");
////////////////        openWith.Remove("doc");

////////////////        if (!openWith.ContainsKey("doc"))
////////////////        {
////////////////            Console.WriteLine("Key \"doc\" is not found.");
////////////////        }
////////////////    }
////////////////}

/////////////////* This code example produces the following output:

////////////////An element with Key = "txt" already exists.
////////////////For key = "rtf", value = wordpad.exe.
////////////////For key = "rtf", value = winword.exe.
////////////////Key = "tif" is not found.
////////////////Key = "tif" is not found.
////////////////Value added for key = "ht": hypertrm.exe

////////////////Key = bmp, Value = paint.exe
////////////////Key = dib, Value = paint.exe
////////////////Key = doc, Value = winword.exe
////////////////Key = ht, Value = hypertrm.exe
////////////////Key = rtf, Value = winword.exe
////////////////Key = txt, Value = notepad.exe

////////////////Value = paint.exe
////////////////Value = paint.exe
////////////////Value = winword.exe
////////////////Value = hypertrm.exe
////////////////Value = winword.exe
////////////////Value = notepad.exe

////////////////Key = bmp
////////////////Key = dib
////////////////Key = doc
////////////////Key = ht
////////////////Key = rtf
////////////////Key = txt

////////////////Remove("doc")
////////////////Key "doc" is not found.
//////////////// */



///////////////////////////////////////////////////////////////
///////////////////////////////////////////////////////////////
////////////////class QueryDuplicateFileNames
////////////////{
////////////////    static void Main(string[] args)
////////////////    {
////////////////        // Uncomment QueryDuplicates2 to run that query.
////////////////        QueryDuplicates();
////////////////        // QueryDuplicates2();

////////////////        // Keep the console window open in debug mode.
////////////////        Console.WriteLine("Press any key to exit.");
////////////////        Console.ReadKey();
////////////////    }

////////////////    static void QueryDuplicates()
////////////////    {
////////////////        // Change the root drive or folder if necessary
////////////////        string startFolder = @"c:\program files\Microsoft Visual Studio 9.0\";

////////////////        // Take a snapshot of the file system.
////////////////        System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo(startFolder);

////////////////        // This method assumes that the application has discovery permissions
////////////////        // for all folders under the specified path.
////////////////        IEnumerable<System.IO.FileInfo> fileList = dir.GetFiles("*.*", System.IO.SearchOption.AllDirectories);

////////////////        // used in WriteLine to keep the lines shorter
////////////////        int charsToSkip = startFolder.Length;

////////////////        // var can be used for convenience with groups.
////////////////        var queryDupNames =
////////////////            from file in fileList
////////////////            group file.FullName.Substring(charsToSkip) by file.Name into fileGroup
////////////////            where fileGroup.Count() > 1
////////////////            select fileGroup;

////////////////        // Pass the query to a method that will
////////////////        // output one page at a time.
////////////////        PageOutput<string, string>(queryDupNames);
////////////////    }

////////////////    // A Group key that can be passed to a separate method.
////////////////    // Override Equals and GetHashCode to define equality for the key.
////////////////    // Override ToString to provide a friendly name for Key.ToString()
////////////////    class PortableKey
////////////////    {
////////////////        public string Name { get; set; }
////////////////        public DateTime CreationTime { get; set; }
////////////////        public long Length { get; set; }

////////////////        public override bool Equals(object obj)
////////////////        {
////////////////            PortableKey other = (PortableKey)obj;
////////////////            return other.CreationTime == this.CreationTime &&
////////////////                   other.Length == this.Length &&
////////////////                   other.Name == this.Name;
////////////////        }

////////////////        public override int GetHashCode()
////////////////        {
////////////////            string str = String.Format("{0}{1}{2}", this.CreationTime, this.Length, this.Name);
////////////////            return str.GetHashCode();
////////////////        }
////////////////        public override string ToString()
////////////////        {
////////////////            return String.Format("{0} {1} {2}", this.Name, this.Length, this.CreationTime);
////////////////        }
////////////////    }

////////////////    static void QueryDuplicates2()
////////////////    {
////////////////        // Change the root drive or folder if necessary.
////////////////        string startFolder = @"c:\program files\Microsoft Visual Studio 9.0\Common7";

////////////////        // Make the the lines shorter for the console display
////////////////        int charsToSkip = startFolder.Length;

////////////////        // Take a snapshot of the file system.
////////////////        System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo(startFolder);
////////////////        IEnumerable<System.IO.FileInfo> fileList = dir.GetFiles("*.*", System.IO.SearchOption.AllDirectories);

////////////////        // Note the use of a compound key. Files that match
////////////////        // all three properties belong to the same group.
////////////////        // A named type is used to enable the query to be
////////////////        // passed to another method. Anonymous types can also be used
////////////////        // for composite keys but cannot be passed across method boundaries
////////////////        // 
////////////////        var queryDupFiles =
////////////////            from file in fileList
////////////////            group file.FullName.Substring(charsToSkip) by
////////////////                new PortableKey { Name = file.Name, CreationTime = file.CreationTime, Length = file.Length } into fileGroup
////////////////            where fileGroup.Count() > 1
////////////////            select fileGroup;

////////////////        var list = queryDupFiles.ToList();

////////////////        int i = queryDupFiles.Count();

////////////////        PageOutput<PortableKey, string>(queryDupFiles);
////////////////    }


////////////////    // A generic method to page the output of the QueryDuplications methods
////////////////    // Here the type of the group must be specified explicitly. "var" cannot
////////////////    // be used in method signatures. This method does not display more than one
////////////////    // group per page.
////////////////    private static void PageOutput<K, V>(IEnumerable<System.Linq.IGrouping<K, V>> groupByExtList)
////////////////    {
////////////////        // Flag to break out of paging loop.
////////////////        bool goAgain = true;

////////////////        // "3" = 1 line for extension + 1 for "Press any key" + 1 for input cursor.
////////////////        int numLines = Console.WindowHeight - 3;

////////////////        // Iterate through the outer collection of groups.
////////////////        foreach (var filegroup in groupByExtList)
////////////////        {
////////////////            // Start a new extension at the top of a page.
////////////////            int currentLine = 0;

////////////////            // Output only as many lines of the current group as will fit in the window.
////////////////            do
////////////////            {
////////////////                Console.Clear();
////////////////                Console.WriteLine("Filename = {0}", filegroup.Key.ToString() == String.Empty ? "[none]" : filegroup.Key.ToString());

////////////////                // Get 'numLines' number of items starting at number 'currentLine'.
////////////////                var resultPage = filegroup.Skip(currentLine).Take(numLines);

////////////////                //Execute the resultPage query
////////////////                foreach (var fileName in resultPage)
////////////////                {
////////////////                    Console.WriteLine("\t{0}", fileName);
////////////////                }

////////////////                // Increment the line counter.
////////////////                currentLine += numLines;

////////////////                // Give the user a chance to escape.
////////////////                Console.WriteLine("Press any key to continue or the 'End' key to break...");
////////////////                ConsoleKey key = Console.ReadKey().Key;
////////////////                if (key == ConsoleKey.End)
////////////////                {
////////////////                    goAgain = false;
////////////////                    break;
////////////////                }
////////////////            } while (currentLine < filegroup.Count());

////////////////            if (goAgain == false)
////////////////                break;
////////////////        }
////////////////    }
////////////////}

////////////////-----------------------------------------------------------------------------------------------------------------------
////////////////-----------------------------------------------------------------------------------------------------------------------
////////////////System.IO
////////////////class QueryContents
////////////////{
////////////////    public static void Main()
////////////////    {
////////////////        // Modify this path as necessary.
////////////////        string startFolder = @"c:\program files\Microsoft Visual Studio 9.0\";

////////////////        // Take a snapshot of the file system.
////////////////        System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo(startFolder);

////////////////        // This method assumes that the application has discovery permissions
////////////////        // for all folders under the specified path.
////////////////        IEnumerable<System.IO.FileInfo> fileList = dir.GetFiles("*.*", System.IO.SearchOption.AllDirectories);

////////////////        string searchTerm = @"Visual Studio";

////////////////        // Search the contents of each file.
////////////////        // A regular expression created with the RegEx class
////////////////        // could be used instead of the Contains method.
////////////////        // queryMatchingFiles is an IEnumerable<string>.
////////////////        var queryMatchingFiles =
////////////////            from file in fileList
////////////////            where file.Extension == ".htm"
////////////////            let fileText = GetFileText(file.FullName)
////////////////            where fileText.Contains(searchTerm)
////////////////            select file.FullName;

////////////////        // Execute the query.
////////////////        Console.WriteLine("The term \"{0}\" was found in:", searchTerm);
////////////////        foreach (string filename in queryMatchingFiles)
////////////////        {
////////////////            Console.WriteLine(filename);
////////////////        }

////////////////        // Keep the console window open in debug mode.
////////////////        Console.WriteLine("Press any key to exit");
////////////////        Console.ReadKey();
////////////////    }

////////////////    // Read the contents of the file.
////////////////    static string GetFileText(string name)
////////////////    {
////////////////        string fileContents = String.Empty;

////////////////        // If the file has been deleted since we took 
////////////////        // the snapshot, ignore it and return the empty string.
////////////////        if (System.IO.File.Exists(name))
////////////////        {
////////////////            fileContents = System.IO.File.ReadAllText(name);
////////////////        }
////////////////        return fileContents;
////////////////    }
////////////////}

////////////////------------------------------------------------------------------------------------------------------------------------
////////////////------------------------------------------------------------------------------------------------------------------------
////////////////class FindFileByExtension
////////////////{
////////////////    // This query will produce the full path for all .txt files
////////////////    // under the specified folder including subfolders.
////////////////    // It orders the list according to the file name.
////////////////    static void Main()
////////////////    {
////////////////        string startFolder = @"c:\program files\Microsoft Visual Studio 9.0\";

////////////////        // Take a snapshot of the file system.
////////////////        System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo(startFolder);

////////////////        // This method assumes that the application has discovery permissions
////////////////        // for all folders under the specified path.
////////////////        IEnumerable<System.IO.FileInfo> fileList = dir.GetFiles("*.*", System.IO.SearchOption.AllDirectories);

////////////////        //Create the query
////////////////        IEnumerable<System.IO.FileInfo> fileQuery =
////////////////            from file in fileList
////////////////            where file.Extension == ".txt"
////////////////            orderby file.Name
////////////////            select file;

////////////////        //Execute the query. This might write out a lot of files!
////////////////        foreach (System.IO.FileInfo fi in fileQuery)
////////////////        {
////////////////            Console.WriteLine(fi.FullName);
////////////////        }

////////////////        // Create and execute a new query by using the previous 
////////////////        // query as a starting point. fileQuery is not 
////////////////        // executed again until the call to Last()
////////////////        var newestFile =
////////////////            (from file in fileQuery
////////////////             orderby file.CreationTime
////////////////             select new { file.FullName, file.CreationTime })
////////////////            .Last();

////////////////        Console.WriteLine("\r\nThe newest .txt file is {0}. Creation time: {1}",
////////////////            newestFile.FullName, newestFile.CreationTime);

////////////////        // Keep the console window open in debug mode.
////////////////        Console.WriteLine("Press any key to exit");
////////////////        Console.ReadKey();
////////////////    }
////////////////}

////////////////-------------------------------------------------------------------------------------------------------------------------
////////////////-------------------------------------------------------------------------------------------------------------------------

////////////////namespace QueryCompareTwoDirs
////////////////{
////////////////    class CompareDirs
////////////////    {

////////////////        static void Main(string[] args)
////////////////        {

////////////////            // Create two identical or different temporary folders 
////////////////            // on a local drive and change these file paths.
////////////////            string pathA = @"C:\TestDir";
////////////////            string pathB = @"C:\TestDir2";

////////////////            System.IO.DirectoryInfo dir1 = new System.IO.DirectoryInfo(pathA);
////////////////            System.IO.DirectoryInfo dir2 = new System.IO.DirectoryInfo(pathB);

////////////////            // Take a snapshot of the file system.
////////////////            IEnumerable<System.IO.FileInfo> list1 = dir1.GetFiles("*.*", System.IO.SearchOption.AllDirectories);
////////////////            IEnumerable<System.IO.FileInfo> list2 = dir2.GetFiles("*.*", System.IO.SearchOption.AllDirectories);

////////////////            //A custom file comparer defined below
////////////////            FileCompare myFileCompare = new FileCompare();

////////////////            // This query determines whether the two folders contain
////////////////            // identical file lists, based on the custom file comparer
////////////////            // that is defined in the FileCompare class.
////////////////            // The query executes immediately because it returns a bool.
////////////////            bool areIdentical = list1.SequenceEqual(list2, myFileCompare);

////////////////            if (areIdentical == true)
////////////////            {
////////////////                Console.WriteLine("the two folders are the same");
////////////////            }
////////////////            else
////////////////            {
////////////////                Console.WriteLine("The two folders are not the same");
////////////////            }

////////////////            // Find the common files. It produces a sequence and doesn't 
////////////////            // execute until the foreach statement.
////////////////            var queryCommonFiles = list1.Intersect(list2, myFileCompare);

////////////////            if (queryCommonFiles.Count() > 0)
////////////////            {
////////////////                Console.WriteLine("The following files are in both folders:");
////////////////                foreach (var v in queryCommonFiles)
////////////////                {
////////////////                    Console.WriteLine(v.FullName); //shows which items end up in result list
////////////////                }
////////////////            }
////////////////            else
////////////////            {
////////////////                Console.WriteLine("There are no common files in the two folders.");
////////////////            }

////////////////            // Find the set difference between the two folders.
////////////////            // For this example we only check one way.
////////////////            var queryList1Only = (from file in list1
////////////////                                  select file).Except(list2, myFileCompare);

////////////////            Console.WriteLine("The following files are in list1 but not list2:");
////////////////            foreach (var v in queryList1Only)
////////////////            {
////////////////                Console.WriteLine(v.FullName);
////////////////            }

////////////////            // Keep the console window open in debug mode.
////////////////            Console.WriteLine("Press any key to exit.");
////////////////            Console.ReadKey();
////////////////        }
////////////////    }

////////////////    // This implementation defines a very simple comparison
////////////////    // between two FileInfo objects. It only compares the name
////////////////    // of the files being compared and their length in bytes.
////////////////    class FileCompare : System.Collections.Generic.IEqualityComparer<System.IO.FileInfo>
////////////////    {
////////////////        public FileCompare() { }

////////////////        public bool Equals(System.IO.FileInfo f1, System.IO.FileInfo f2)
////////////////        {
////////////////            return (f1.Name == f2.Name &&
////////////////                    f1.Length == f2.Length);
////////////////        }

////////////////        // Return a hash that reflects the comparison criteria. According to the 
////////////////        // rules for IEqualityComparer<T>, if Equals is true, then the hash codes must
////////////////        // also be equal. Because equality as defined here is a simple value equality, not
////////////////        // reference identity, it is possible that two or more objects will produce the same
////////////////        // hash code.
////////////////        public int GetHashCode(System.IO.FileInfo fi)
////////////////        {
////////////////            string s = String.Format("{0}{1}", fi.Name, fi.Length);
////////////////            return s.GetHashCode();
////////////////        }
////////////////    }
////////////////}
////////////////-------------------------------------------------------------------------------------------------------------------------
////////////////-------------------------------------------------------------------------------------------------------------------------
////////////////class GroupByExtension
////////////////{
////////////////    // This query will sort all the files under the specified folder
////////////////    //  and subfolder into groups keyed by the file extension.
////////////////    private static void Main()
////////////////    {
////////////////        // Take a snapshot of the file system.
////////////////        string startFolder = @"c:\program files\Microsoft Visual Studio 9.0\Common7";

////////////////        // Used in WriteLine to trim output lines.
////////////////        int trimLength = startFolder.Length;

////////////////        // Take a snapshot of the file system.
////////////////        System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo(startFolder);

////////////////        // This method assumes that the application has discovery permissions
////////////////        // for all folders under the specified path.
////////////////        IEnumerable<System.IO.FileInfo> fileList = dir.GetFiles("*.*", System.IO.SearchOption.AllDirectories);

////////////////        // Create the query.
////////////////        var queryGroupByExt =
////////////////            from file in fileList
////////////////            group file by file.Extension.ToLower() into fileGroup
////////////////            orderby fileGroup.Key
////////////////            select fileGroup;

////////////////        // Display one group at a time. If the number of 
////////////////        // entries is greater than the number of lines
////////////////        // in the console window, then page the output.
////////////////        PageOutput(trimLength, queryGroupByExt);
////////////////    }

////////////////    // This method specifically handles group queries of FileInfo objects with string keys.
////////////////    // It can be modified to work for any long listings of data. Note that explicit typing
////////////////    // must be used in method signatures. The groupbyExtList parameter is a query that produces
////////////////    // groups of FileInfo objects with string keys.
////////////////    private static void PageOutput(int rootLength,
////////////////                                    IEnumerable<System.Linq.IGrouping<string, System.IO.FileInfo>> groupByExtList)
////////////////    {
////////////////        // Flag to break out of paging loop.
////////////////        bool goAgain = true;

////////////////        // "3" = 1 line for extension + 1 for "Press any key" + 1 for input cursor.
////////////////        int numLines = Console.WindowHeight - 3;

////////////////        // Iterate through the outer collection of groups.
////////////////        foreach (var filegroup in groupByExtList)
////////////////        {
////////////////            // Start a new extension at the top of a page.
////////////////            int currentLine = 0;

////////////////            // Output only as many lines of the current group as will fit in the window.
////////////////            do
////////////////            {
////////////////                Console.Clear();
////////////////                Console.WriteLine(filegroup.Key == String.Empty ? "[none]" : filegroup.Key);

////////////////                // Get 'numLines' number of items starting at number 'currentLine'.
////////////////                var resultPage = filegroup.Skip(currentLine).Take(numLines);

////////////////                //Execute the resultPage query
////////////////                foreach (var f in resultPage)
////////////////                {
////////////////                    Console.WriteLine("\t{0}", f.FullName.Substring(rootLength));
////////////////                }

////////////////                // Increment the line counter.
////////////////                currentLine += numLines;

////////////////                // Give the user a chance to escape.
////////////////                Console.WriteLine("Press any key to continue or the 'End' key to break...");
////////////////                ConsoleKey key = Console.ReadKey().Key;
////////////////                if (key == ConsoleKey.End)
////////////////                {
////////////////                    goAgain = false;
////////////////                    break;
////////////////                }
////////////////            } while (currentLine < filegroup.Count());

////////////////            if (goAgain == false)
////////////////                break;
////////////////        }
////////////////    }
////////////////}



////////////////-------------------------------------------------------------------------------------------------------------------------
////////////////-------------------------------------------------------------------------------------------------------------------------
////////////////class QuerySize
////////////////{
////////////////    public static void Main()
////////////////    {
////////////////        string startFolder = @"c:\program files\Microsoft Visual Studio 9.0\VC#";

////////////////        // Take a snapshot of the file system.
////////////////        // This method assumes that the application has discovery permissions
////////////////        // for all folders under the specified path.
////////////////        IEnumerable<string> fileList = System.IO.Directory.GetFiles(startFolder, "*.*", System.IO.SearchOption.AllDirectories);

////////////////        var fileQuery = from file in fileList
////////////////                        select GetFileLength(file);

////////////////        // Cache the results to avoid multiple trips to the file system.
////////////////        long[] fileLengths = fileQuery.ToArray();

////////////////        // Return the size of the largest file
////////////////        long largestFile = fileLengths.Max();

////////////////        // Return the total number of bytes in all the files under the specified folder.
////////////////        long totalBytes = fileLengths.Sum();

////////////////        Console.WriteLine("There are {0} bytes in {1} files under {2}",
////////////////            totalBytes, fileList.Count(), startFolder);
////////////////        Console.WriteLine("The largest files is {0} bytes.", largestFile);

////////////////        // Keep the console window open in debug mode.
////////////////        Console.WriteLine("Press any key to exit.");
////////////////        Console.ReadKey();
////////////////    }

////////////////    // This method is used to swallow the possible exception
////////////////    // that can be raised when accessing the System.IO.FileInfo.Length property.
////////////////    static long GetFileLength(string filename)
////////////////    {
////////////////        long retval;
////////////////        try
////////////////        {
////////////////            System.IO.FileInfo fi = new System.IO.FileInfo(filename);
////////////////            retval = fi.Length;
////////////////        }
////////////////        catch (System.IO.FileNotFoundException)
////////////////        {
////////////////            // If a file is no longer present,
////////////////            // just add zero bytes to the total.
////////////////            retval = 0;
////////////////        }
////////////////        return retval;
////////////////    }
////////////////}

////////////////-------------------------------------------------------------------------------------------------------------------------
////////////////-------------------------------------------------------------------------------------------------------------------------
////////////////class QueryBySize
////////////////{
////////////////    static void Main(string[] args)
////////////////    {
////////////////        QueryFilesBySize();
////////////////        Console.WriteLine("Press any key to exit");
////////////////        Console.ReadKey();
////////////////    }

////////////////    private static void QueryFilesBySize()
////////////////    {
////////////////        string startFolder = @"c:\program files\Microsoft Visual Studio 9.0\";

////////////////        // Take a snapshot of the file system.
////////////////        System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo(startFolder);

////////////////        // This method assumes that the application has discovery permissions
////////////////        // for all folders under the specified path.
////////////////        IEnumerable<System.IO.FileInfo> fileList = dir.GetFiles("*.*", System.IO.SearchOption.AllDirectories);

////////////////        //Return the size of the largest file
////////////////        long maxSize =
////////////////            (from file in fileList
////////////////             let len = GetFileLength(file)
////////////////             select len)
////////////////             .Max();

////////////////        Console.WriteLine("The length of the largest file under {0} is {1}",
////////////////            startFolder, maxSize);

////////////////        // Return the FileInfo object for the largest file
////////////////        // by sorting and selecting from beginning of list
////////////////        System.IO.FileInfo longestFile =
////////////////            (from file in fileList
////////////////             let len = GetFileLength(file)
////////////////             where len > 0
////////////////             orderby len descending
////////////////             select file)
////////////////            .First();

////////////////        Console.WriteLine("The largest file under {0} is {1} with a length of {2} bytes",
////////////////                            startFolder, longestFile.FullName, longestFile.Length);

////////////////        //Return the FileInfo of the smallest file
////////////////        System.IO.FileInfo smallestFile =
////////////////            (from file in fileList
////////////////             let len = GetFileLength(file)
////////////////             where len > 0
////////////////             orderby len ascending
////////////////             select file).First();

////////////////        Console.WriteLine("The smallest file under {0} is {1} with a length of {2} bytes",
////////////////                            startFolder, smallestFile.FullName, smallestFile.Length);

////////////////        //Return the FileInfos for the 10 largest files
////////////////        // queryTenLargest is an IEnumerable<System.IO.FileInfo>
////////////////        var queryTenLargest =
////////////////            (from file in fileList
////////////////             let len = GetFileLength(file)
////////////////             orderby len descending
////////////////             select file).Take(10);

////////////////        Console.WriteLine("The 10 largest files under {0} are:", startFolder);

////////////////        foreach (var v in queryTenLargest)
////////////////        {
////////////////            Console.WriteLine("{0}: {1} bytes", v.FullName, v.Length);
////////////////        }


////////////////        // Group the files according to their size, leaving out
////////////////        // files that are less than 200000 bytes. 
////////////////        var querySizeGroups =
////////////////            from file in fileList
////////////////            let len = GetFileLength(file)
////////////////            where len > 0
////////////////            group file by (len / 100000) into fileGroup
////////////////            where fileGroup.Key >= 2
////////////////            orderby fileGroup.Key descending
////////////////            select fileGroup;


////////////////        foreach (var filegroup in querySizeGroups)
////////////////        {
////////////////            Console.WriteLine(filegroup.Key.ToString() + "00000");
////////////////            foreach (var item in filegroup)
////////////////            {
////////////////                Console.WriteLine("\t{0}: {1}", item.Name, item.Length);
////////////////            }
////////////////        }
////////////////    }

////////////////    // This method is used to swallow the possible exception
////////////////    // that can be raised when accessing the FileInfo.Length property.
////////////////    // In this particular case, it is safe to swallow the exception.
////////////////    static long GetFileLength(System.IO.FileInfo fi)
////////////////    {
////////////////        long retval;
////////////////        try
////////////////        {
////////////////            retval = fi.Length;
////////////////        }
////////////////        catch (System.IO.FileNotFoundException)
////////////////        {
////////////////            // If a file is no longer present,
////////////////            // just add zero bytes to the total.
////////////////            retval = 0;
////////////////        }
////////////////        return retval;
////////////////    }

////////////////}

////////////////-------------------------------------------------------------------------------------------------------------------------
////////////////-------------------------------------------------------------------------------------------------------------------------

////////////////-------------------------------------------------------------------------------------------------------------------------
////////////////-------------------------------------------------------------------------------------------------------------------------

////////////////-------------------------------------------------------------------------------------------------------------------------
////////////////-------------------------------------------------------------------------------------------------------------------------

////////////////-------------------------------------------------------------------------------------------------------------------------
////////////////-------------------------------------------------------------------------------------------------------------------------

////////////////-------------------------------------------------------------------------------------------------------------------------
////////////////-------------------------------------------------------------------------------------------------------------------------

////////////////-------------------------------------------------------------------------------------------------------------------------
////////////////-------------------------------------------------------------------------------------------------------------------------

////////////////-------------------------------------------------------------------------------------------------------------------------
////////////////-------------------------------------------------------------------------------------------------------------------------



///////////////////////////////////////////////////////////////
///////////////////////////////////////////////////////////////


////////class StringSearch
////////{
////////    static void Main()
////////    {
////////        string str = "Extension methods have all the capabilities of regular static methods.";

////////        // Write the string and include the quotation marks.
////////        System.Console.WriteLine("\"{0}\"", str);

////////        // Simple comparisons are always case sensitive! 
////////        bool test1 = str.StartsWith("extension");
////////        System.Console.WriteLine("Starts with \"extension\"? {0}", test1);

////////        // For user input and strings that will be displayed to the end user,  
////////        // use the StringComparison parameter on methods that have it to specify how to match strings. 
////////        bool test2 = str.StartsWith("extension", System.StringComparison.CurrentCultureIgnoreCase);
////////        System.Console.WriteLine("Starts with \"extension\"? {0} (ignoring case)", test2);

////////        bool test3 = str.EndsWith(".", System.StringComparison.CurrentCultureIgnoreCase);
////////        System.Console.WriteLine("Ends with '.'? {0}", test3);

////////        // This search returns the substring between two strings, so  
////////        // the first index is moved to the character just after the first string. 
////////        int first = str.IndexOf("methods") + "methods".Length;
////////        int last = str.LastIndexOf("methods");
////////        string str2 = str.Substring(first, last - first);
////////        System.Console.WriteLine("Substring between \"methods\" and \"methods\": '{0}'", str2);

////////        // Keep the console window open in debug mode
////////        System.Console.WriteLine("Press any key to exit.");
////////        System.Console.ReadKey();
////////    }
////////}
/////////*
////////Output:
////////"Extension methods have all the capabilities of regular static methods."
////////Starts with "extension"? False
////////Starts with "extension"? True (ignoring case)
////////Ends with '.'? True
////////Substring between "methods" and "methods": ' have all the capabilities of regular static '
////////Press any key to exit.     
////////*/
//public string Replace(	string oldValue,	string newValue)
//string correctString = errString.Replace("docment", "document");
//LArqIdxQtdBytes[fi.Length.ToString()] = LArqIdxQtdBytes[fi.Length.ToString()] + "\t" + FirstFolder + "\t" + Arquivo.Substring(FirstFolder.Length + 1);
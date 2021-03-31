using System;
using System.Threading;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Globalization;

namespace Bank_Account_Control{
    public class Program
    {

          [DllImport("kernel32.dll", ExactSpelling = true)]  
          private static extern IntPtr GetConsoleWindow();  
          private static IntPtr ThisConsole = GetConsoleWindow();  
          [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]  
          private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);  
          private const int HIDE = 0;  
          private const int MAXIMIZE = 3;  
          private const int MINIMIZE = 6;  
          private const int RESTORE = 9;  
          
        protected static int origRow =  0;
        protected static int origCol = 0;

        protected static int contadorDeContas = 0;

   
        public static string EnteredVal = ""; 
        public static string LinhaBrancaText = new String(' ', Console.LargestWindowWidth);
        public static string LinhaBrancaCadastroText = new String(' ', Console.LargestWindowWidth/4);

        public static string LinhaBrancaLimpaCadastro = new String(' ',2 * Console.LargestWindowWidth/3);

        public static string GuestPasswordVal = "XXXXXX";
        public static string EndPasswordVal = "000000";

        static List<Account> AccountsFile = new List<Account>();
       
        public static void Main(String[] args)
        {
                    String CoverText = "Sistema de Transferências Bancárias";
                    String VersionText = "Versão DEMO!";
                    String GuestText = "Usuário: Guest User - Senha: XXXXXX ";
                    String PasswordText = "Entre com a senha do USUARIO!";
                    String EndingText = "Para Encerrar digite '000000' e pressione << ENTER >>!";
                    String SeparaText = new String('=', Console.LargestWindowWidth);
                    
                    
                    Console.SetWindowSize(Console.LargestWindowWidth, Console.LargestWindowHeight);  
                    ShowWindow(ThisConsole, MAXIMIZE);  
                    
                    ConsoleColor currentBackground = Console.BackgroundColor;
                    ConsoleColor currentForeground = Console.ForegroundColor;
                    
                    ConsoleColor newForeColor = ConsoleColor.White;
                    ConsoleColor newBackColor = ConsoleColor.DarkMagenta;
                    Console.ForegroundColor = newForeColor;
                    Console.BackgroundColor = newBackColor;
                    Console.Clear();
                    
                    int PosxCoverText = Console.LargestWindowWidth/2 - (CoverText.Length/2);
                    int PosyCoverText = Console.LargestWindowHeight/20;
                    WriteText(CoverText,PosxCoverText, PosyCoverText);

                    int PosxVersionText = Console.LargestWindowWidth/2 - (VersionText.Length/2);
                    int PosyVersionText = PosyCoverText + 3;
                    WriteText(VersionText,PosxVersionText, PosyVersionText);

                    int PosxGuestText = Console.LargestWindowWidth/2 - (GuestText.Length/2);
                    int PosyGuestText = PosyVersionText + 3;
                    WriteText(GuestText,PosxGuestText, PosyGuestText);
                    
                    int PosyPasswordText = PosyGuestText + 3;
                    WriteText(PasswordText,origCol, PosyPasswordText);

                    int PosxEndingText = Console.LargestWindowWidth/2 - (EndingText.Length/2);
                    WriteText(EndingText,PosxEndingText, PosyPasswordText+2);
                
                    int PosySeparaText = PosyPasswordText + 3;
                    WriteText(SeparaText,origCol, PosySeparaText);

                    bool success = CheckPassword(PasswordText,origCol,PosyPasswordText);  

                    if (success == false){
                        return;
                    }else{
                        MainMenu();
                        Console.ResetColor();
                        return;
                    }
    
        }

        static uint GetUserChoice(Action printMenu, int choiceMax, int posx, int posy){
            uint choice = 0;
            String SolicitaText = "Opçao Inválida, Por favor, tente novamente!";
            String OpcaoText = "Entre com sua opção: ";
            
            WriteText(OpcaoText,posx, posy);
            Action getInput = () =>
            {
                uint.TryParse(Console.ReadLine(), out choice);
            };
            getInput();
            while ( choice < 1 || choice > choiceMax )
            {
                Console.WriteLine();
                WriteText(SolicitaText,posx, posy + 1);
                Thread.Sleep(2000);
                WriteText(LinhaBrancaText,posx,posy+1);
                WriteText(LinhaBrancaText,posx,posy);
                WriteText(OpcaoText,posx, posy);
                getInput();
            }
            return choice;
        }


        static uint GetUserCadastro(Action printMenu, int choiceMax, int posx, int posy){
                uint cadastro = 0;
                String SolicitaText = "Opçao Inválida, tente novamente!";
                String OpcaoText = "Entre com sua opção: ";
                
                WriteText(OpcaoText,posx, posy);
                Action getInput = () =>
                {
                    uint.TryParse(Console.ReadLine(), out cadastro);
                };
                getInput();
                while ( cadastro < 1 || cadastro > choiceMax )
                {
                    Console.WriteLine();
                    WriteText(SolicitaText,posx, posy + 1);
                    Thread.Sleep(2000);
                    WriteText(LinhaBrancaCadastroText,posx,posy+1);
                    WriteText(LinhaBrancaCadastroText,posx,posy);
                    WriteText(OpcaoText,posx, posy);
                    getInput();
                }
                return cadastro;
                }
                            
                  

        static void MainMenu(){

                
                String MenuGeralText1 =  "Bem-Vindo ao Sistema de Controle de Contas Bancárias!";
                String MenuGeralText2 = "Você tem acesso a todas as operações rotineiras de sua conta!";
                String MenuGeralText3 = "Versão DEMO! Para utilizar cadastre, PRIMEIRO, pelo menos duas contas na opção Menu Completo!";
               
                String SeparaText = new String('-', MenuGeralText3.Length + 2);
               

                Action printMenu = () =>
                {
                    Console.WriteLine("Digite 1 Menu Completo");
                    Console.WriteLine("Digite 2 Menu Expresso << apenas saques >> SENHA NÃO HABILITADA!");
                    Console.WriteLine("Digite 3 Menu Expresso << apenas depósitos >> SENHA NÃO HABILITADA!");
                    Console.WriteLine("Digite 4 Menu Expresso << apenas transferências >> SENHA NÃO HABILITADA!");
                    Console.WriteLine("Digite 5 Menu Expresso << apenas extratos >> SENHA NÃO HABILITADA!");
                    Console.WriteLine("Digite 6 para sair");
                };
                
                int PosyMenuGeralText1 = origRow + 16;
                WriteText(SeparaText,origCol, PosyMenuGeralText1 - 1);
                WriteText(MenuGeralText1,origCol, PosyMenuGeralText1);
                WriteText(MenuGeralText2,origCol, PosyMenuGeralText1 + 1);
                WriteText(MenuGeralText3,origCol, PosyMenuGeralText1 + 2);
                WriteText(SeparaText,origCol, PosyMenuGeralText1 + 3);
                Console.WriteLine(" ");
                printMenu();
               
                uint choice = GetUserChoice(printMenu, 6, origCol, origRow + 27);
                switch ( choice )
                {
                    case 1:
                       FullMenu();
                       break;

                    case 2:
                       //ManagerMenu();
                       break;

                    case 4:
                       //CustomerMenu();
                       break;

                    case 5:
                       //ManagerMenu();
                       break;

                    case 6:
                       Console.WriteLine("Obrigado por acessar o Sistema de Controle de Contas Bancárias!, Até Breve!");
                       Thread.Sleep(2000);
                       Console.ResetColor();
                       Environment.Exit(1);
                       break;
                       

                    default:
                       throw new NotImplementedException();
                    }

                    
        }

        static void FullMenu(){

                String SubMenuText = "Abaixo a lista completa de funcionalidades!";
                String SeparaText = new String('-', SubMenuText.Length + 2);
                String OpcaoText = "Entre com sua opção: ";

                Action printMenu = () =>
                {
                    Console.WriteLine("Digite 1 para Cadastrar Contas!");
                    Console.WriteLine("Digite 2 para realizar Deposito!");
                    Console.WriteLine("Digite 3 para realizar Saque!");
                    Console.WriteLine("Digite 4 para realizar Transferência!");
                    Console.WriteLine("Digite 5 para Listar uma Conta!");
                    Console.WriteLine("Digite 6 para retornar ao Menu Anterior!");
                };

                int PosySubMenuText = origRow + 34;
                WriteText(SeparaText,origCol, PosySubMenuText - 1);
                WriteText(SubMenuText,origCol, PosySubMenuText);
                WriteText(SeparaText,origCol, PosySubMenuText + 2);
                Console.WriteLine(" ");
                printMenu();
               
                uint choice = GetUserChoice(printMenu, 6, origCol, origRow + 44);
                switch ( choice )
                {
                    case 1:
                        CadastrarContas();
                        break;
                    case 2:
                        DepositarContas();
                        break;
                    case 3:
                        SacarContas();
                         break;
                    case 4:
                         TransferirContas();
                         break;
                    case 5:
                         ListarContas();
                         break;     
                    case 6:

                        for (int indx_ln = -1; indx_ln < 11;indx_ln++){
                         
                         WriteText(LinhaBrancaText,origCol,PosySubMenuText + indx_ln);
                         
                        }

                         WriteText(LinhaBrancaText,origCol,origRow+27);
                         WriteText(OpcaoText,origCol, origRow+27);
                         MainMenu();
                         break;
                    default:
                         throw new NotImplementedException();
                }
         }

        private static void SacarContas(){
 
            List<Account> tmpAccount = new List<Account>();
   
            String ValorSaqueText = "Entre com o Valor a ser Sacado: ";
            String SolicitaText = "Valor Inválido, tente novamente!";
            String OpcaoText = "Entre com sua opção: ";

            bool isNumConta;          
            bool successValida = false;
            
            int inputNumeroDaConta = 0;
            int inputDigitoDaConta = 0;
            
            bool success = false;
            String msg = "Confirma realização de Saque? < S/s > ou < N/n >";
            Char[] vet = {'S','s','N','n'};
            do{
            
               char key =  GetKeyPress( msg, vet,87 , 28 );

                if ( ( key.Equals('S') )|| (key.Equals('s')) ){

                        WriteText(LinhaBrancaText,origCol,origRow+28);
                        inputNumeroDaConta = PegarNumeroDaConta(87,28);
                        break;

                }else if ( ( key.Equals('N') )|| ( key.Equals('n') ) ){

                        WriteText(LinhaBrancaText,origCol,origRow+28);
                        WriteText(LinhaBrancaText,origCol,origRow+44);
                        WriteText(OpcaoText,origCol, origRow+44);
                        FullMenu();
                
                }
            }while(success == false);

            Console.SetCursorPosition(133,28);
            
            do {

                   try {

                        String testaDigito = Console.ReadLine();
                        successValida = ValidaCampo(testaDigito, isNumConta = false);

                        if (successValida){

                           success = int.TryParse(testaDigito,out inputDigitoDaConta);

                        }

                     }catch (FormatException){
                    
                    Console.WriteLine("Unable to convert '{0}'.", inputDigitoDaConta);
                
                    }catch (OverflowException){
                    
                    Console.WriteLine("'{0}' is out of range of the double type.", inputDigitoDaConta);
                    }


                    if (successValida && success){
            
            
                       if(!inputDigitoDaConta.Equals(GeraDigito(inputNumeroDaConta))){;

                            WriteText(SolicitaText,87, 29);
                            Thread.Sleep(2000);
                            WriteText(LinhaBrancaCadastroText,87,29);
                            WriteText(LinhaBrancaCadastroText,87,28);
                            WriteText(LinhaBrancaCadastroText,97,28);
                            WriteText(LinhaBrancaText,origCol,origRow+44);
                            WriteText(OpcaoText,origCol, origRow+44);

                            FullMenu();

                        }
                    }else{
                    
                           success = true;

                    }

            }while(success == false);
                
            double inputValorDoSaque = 0;

            
            success = false;
            string testaValor = " ";

            WriteText (ValorSaqueText,87,30); // pega valor do saque
            
            do{
                    try{
                        testaValor = Console.ReadLine();
                        successValida = ValidaCampo(testaValor, isNumConta = false);

                        if(successValida){

                            success = double.TryParse(testaValor,out inputValorDoSaque);
                          
                        }


                    }catch (FormatException){
                    
                    Console.WriteLine("Unable to convert '{0}'.", inputValorDoSaque);
                
                    }catch (OverflowException){
                    
                    Console.WriteLine("'{0}' is out of range of the double type.", inputValorDoSaque);
                    }

                    if (successValida && success && inputValorDoSaque > 0){

                        
                        WriteText(LinhaBrancaCadastroText,87,30);
                        Console.SetCursorPosition(87 + ValorSaqueText.Length + 2,30);
                        WriteText (ValorSaqueText,87,30);
                        WriteText (inputValorDoSaque.ToString("N2"),87 + 
                                   ValorSaqueText.Length + 2,30);

                    }else{

                        successValida = false;
                        success = false;
                        WriteText(SolicitaText,87, 31);
                        Thread.Sleep(2000);
                        WriteText(LinhaBrancaCadastroText,87,31);
                        WriteText(LinhaBrancaCadastroText,87,30);
                        Console.SetCursorPosition(87 + ValorSaqueText.Length + 2,30);
                        WriteText (ValorSaqueText,87,30);
                        inputValorDoSaque = 0;
                    }
                    
             }while (successValida == false);
            
            var itemaccount = AccountsFile[inputNumeroDaConta-1].ToOpera(); 
           
            int[] vetIndx = new int[6]; 
            vetIndx = ImprimirItem(itemaccount);

            Console.SetCursorPosition(87,33);
            Console.WriteLine ("Conta-DV");
            Console.SetCursorPosition(87,35);
            Console.WriteLine(itemaccount.Substring(vetIndx[0]+1,(vetIndx[1]-vetIndx[0])-1));

            Console.SetCursorPosition(87,37);
            Console.Write ("Nome do Titular: ");
            Console.WriteLine(itemaccount.Substring(vetIndx[1]+1,(vetIndx[2]-vetIndx[1])-1));

            Console.SetCursorPosition(87,40);
            Console.Write("Valor depositado na Conta: ");
            Console.Write(itemaccount.Substring(vetIndx[2]+1,(vetIndx[3]-vetIndx[2])-1));

            Console.SetCursorPosition(127,40);
            Console.Write("Credito liberado na Conta: ");
            Console.WriteLine(itemaccount.Substring(vetIndx[3]+1,(vetIndx[4]-vetIndx[3])-1));

            Console.SetCursorPosition(87,43);
            Console.Write ("Valor disponivel para Saque: ");
            Console.Write(itemaccount.Substring(vetIndx[4]+1,(vetIndx[5]-vetIndx[4])-1));

            Console.SetCursorPosition(87,45);
          
            success = AccountsFile[inputNumeroDaConta-1].Sacar(inputValorDoSaque);

            if (success){

                itemaccount = AccountsFile[inputNumeroDaConta-1].ToOpera();
                vetIndx = ImprimirItem(itemaccount);

                WriteText(LinhaBrancaCadastroText,87,40);
                Console.SetCursorPosition(87,40);
                Console.Write("Valor depositado na Conta: ");
                Console.Write(itemaccount.Substring(vetIndx[2]+1,(vetIndx[3]-vetIndx[2])-1));

                Console.SetCursorPosition(127,40);
                Console.WriteLine("Credito liberado na Conta: ");

                WriteText(LinhaBrancaCadastroText,87,43);
                Console.SetCursorPosition(87,43);
                Console.Write ("Valor disponivel para Saque: ");
                Console.WriteLine(itemaccount.Substring(vetIndx[4]+1,(vetIndx[5]-vetIndx[4])-1));


                WriteText ("Saque realizado com Exito! ",87,45);
                WriteText(LinhaBrancaCadastroText,87,45);


            }else{

                WriteText ("Saldo insuficiente para esta operação! ",87,45);
                WriteText(LinhaBrancaCadastroText,87,45);
                            
            }      

            
            
            success = true;
            msg = "Digite < S ou s > para Voltar ao Menu Principal!";
        
            do{
            
               char key =  GetKeyPress( msg, vet, 77, 52);

                if ( ( key.Equals('S') )|| (key.Equals('s')) ){

                    
                        for (int indx_ln = 1; indx_ln < 28;indx_ln++){
                                
                                WriteText(LinhaBrancaLimpaCadastro,50, 27 + indx_ln);
                                
                                
                        }
            
                        success = false;
                }
            }while(success);



            for (int indx_ln = 1; indx_ln < 28;indx_ln++){
                         
                        WriteText(LinhaBrancaLimpaCadastro,87, 27 + indx_ln);
                    
                        
            } 

            WriteText(LinhaBrancaText,origCol,origRow+44);
            WriteText(OpcaoText,origCol, origRow+44);

            FullMenu();
        }

       

        private static void DepositarContas(){
 
            //List<Account> tmpAccount = new List<Account>();

            
            String ValorDepositoText = "Entre com o Valor a ser Depositado: ";
            String SolicitaText = "Valor Inválido, tente novamente!";
            String OpcaoText = "Entre com sua opção: ";

            bool successValida = false;
            bool isNumConta;


            int inputNumeroDaConta = 0;
            int inputDigitoDaConta = 0;

             
            bool success = false;
            String msg = "Confirma realização do Deposito? < S/s > ou < N/n >";
            Char[] vet = {'S','s','N','n'};
            do{
            
               char key =  GetKeyPress( msg, vet,87 , 28 );

                if ( ( key.Equals('S') )|| (key.Equals('s')) ){

                        WriteText(LinhaBrancaText,origCol,origRow+28);
                        inputNumeroDaConta = PegarNumeroDaConta(87,28);
                        break;

                }else if ( ( key.Equals('N') )|| ( key.Equals('n') ) ){

                        WriteText(LinhaBrancaText,origCol,origRow+28);
                        WriteText(LinhaBrancaText,origCol,origRow+44);
                        WriteText(OpcaoText,origCol, origRow+44);
                        FullMenu();
                
                }
            }while(success == false);

            Console.SetCursorPosition(133,28);
            
            do {

                   try {

                        String testaDigito = Console.ReadLine();
                        successValida = ValidaCampo(testaDigito, isNumConta = false);

                        if (successValida){

                           success = int.TryParse(testaDigito,out inputDigitoDaConta);

                        }

                     }catch (FormatException){
                    
                    Console.WriteLine("Unable to convert '{0}'.", inputDigitoDaConta);
                
                    }catch (OverflowException){
                    
                    Console.WriteLine("'{0}' is out of range of the double type.", inputDigitoDaConta);
                    }


                    if (successValida && success){
            
            
                       if(!inputDigitoDaConta.Equals(GeraDigito(inputNumeroDaConta))){;

                            WriteText(SolicitaText,87, 29);
                            Thread.Sleep(2000);
                            WriteText(LinhaBrancaCadastroText,87,29);
                            WriteText(LinhaBrancaCadastroText,87,28);
                            WriteText(LinhaBrancaCadastroText,97,28);
                            WriteText(LinhaBrancaText,origCol,origRow+44);
                            WriteText(OpcaoText,origCol, origRow+44);

                            FullMenu();

                        }
                    }else{
                    
                           success = true;

                    }

            }while(success == false);
                
                                   
            double inputValorDoDeposito = 0;
            String testaValor = " ";

            WriteText (ValorDepositoText,87,30); // pega valor do DEPOSITO
            do{
                    try{
                        testaValor = Console.ReadLine();   
                        successValida = ValidaCampo(testaValor, isNumConta = false);

                        if (successValida ){

                            success = double.TryParse(testaValor,out inputValorDoDeposito);
                        
                        }


                    }catch (FormatException){
                    
                    Console.WriteLine("Unable to convert '{0}'.", inputValorDoDeposito);
                
                    }catch (OverflowException){
                    
                    Console.WriteLine("'{0}' is out of range of the double type.", inputValorDoDeposito);
                    }

                     if (successValida && success && inputValorDoDeposito > 0){

                        WriteText(LinhaBrancaCadastroText,87,30);
                        Console.SetCursorPosition(87 + ValorDepositoText.Length + 2,30);
                        WriteText (ValorDepositoText,87,30);
                        WriteText (inputValorDoDeposito.ToString("N2"),87 + 
                                    ValorDepositoText.Length + 2,30);

                     }else{

                        successValida = false;
                        success = false;
                        WriteText(SolicitaText,87, 31);
                        Thread.Sleep(2000);
                        WriteText(LinhaBrancaCadastroText,87,31);
                        WriteText(LinhaBrancaCadastroText,87,30);
                        Console.SetCursorPosition(87 + ValorDepositoText.Length + 2,30);
                        WriteText (ValorDepositoText,87,30);
                        inputValorDoDeposito = 0;
                     }

                    
            }while(successValida == false); 

            
            var itemaccount = AccountsFile[inputNumeroDaConta-1].ToOpera(); 
           
            int[] vetIndx = new int[6]; 
            vetIndx = ImprimirItem(itemaccount);
                   
           
            Console.SetCursorPosition(87,33);
            Console.WriteLine ("Conta-DV");
            Console.SetCursorPosition(87,35);
            Console.WriteLine(itemaccount.Substring(vetIndx[0]+1,(vetIndx[1]-vetIndx[0])-1));

            Console.SetCursorPosition(87,37);
            Console.Write ("Nome do Titular: ");
            Console.WriteLine(itemaccount.Substring(vetIndx[1]+1,(vetIndx[2]-vetIndx[1])-1));

            Console.SetCursorPosition(87,40);
            Console.Write("Valor depositado na Conta: ");
            Console.Write(itemaccount.Substring(vetIndx[2]+1,(vetIndx[3]-vetIndx[2])-1));

            Console.SetCursorPosition(127,40);
            Console.Write("Credito liberado na Conta: ");
            Console.WriteLine(itemaccount.Substring(vetIndx[3]+1,(vetIndx[4]-vetIndx[3])-1));

            Console.SetCursorPosition(87,43);
            Console.Write ("Valor disponivel para Saque: ");
            Console.WriteLine(itemaccount.Substring(vetIndx[4]+1,(vetIndx[5]-vetIndx[4])-1));

            Console.SetCursorPosition(87,45);
          
            success = AccountsFile[inputNumeroDaConta-1].Depositar(inputValorDoDeposito);

            if (success){

                itemaccount = AccountsFile[inputNumeroDaConta-1].ToOpera();
                vetIndx = ImprimirItem(itemaccount);

                WriteText(LinhaBrancaCadastroText,87,40);
                Console.SetCursorPosition(87,40);
                Console.Write("Valor depositado na Conta: ");
                Console.Write(itemaccount.Substring(vetIndx[2]+1,(vetIndx[3]-vetIndx[2])-1));

                Console.SetCursorPosition(127,40);
                Console.WriteLine("Credito liberado na Conta: ");

                WriteText(LinhaBrancaCadastroText,87,43);
                Console.SetCursorPosition(87,43);
                Console.Write ("Valor disponivel para Saque: ");
                Console.Write(itemaccount.Substring(vetIndx[4]+1,(vetIndx[5]-vetIndx[4])-1));

                WriteText ("Deposito realizado com Exito! ",87,45);
                WriteText(LinhaBrancaCadastroText,87,45);


            }

            
            
            success = true;
            msg = "Digite < S ou s > para Voltar ao Menu Principal!";
           
            do{
            
               char key =  GetKeyPress( msg, vet, 77, 52);

                if ( ( key.Equals('S') )|| (key.Equals('s')) ){

                    
                        for (int indx_ln = 1; indx_ln < 28;indx_ln++){
                                
                                WriteText(LinhaBrancaLimpaCadastro,50, 27 + indx_ln);
                                
                                
                        }
            
                        success = false;
                }
            }while(success);

  
            for (int indx_ln = 1; indx_ln < 28;indx_ln++){
                         
                        WriteText(LinhaBrancaLimpaCadastro,87, 27 + indx_ln);
                    
                        
            } 

            WriteText(LinhaBrancaText,origCol,origRow+44);
            WriteText(OpcaoText,origCol, origRow+44);

            FullMenu();
        }


        private static void TransferirContas(){

            String NumeroContaSacadaText = "Conta para Saque => ";
            String NumeroContaCreditadaText = "Conta para Credito => ";
            String ValorSaqueText = "Entre com o Valor a ser Transferido: ";
            String SolicitaText = "Valor Inválido, tente novamente!";
            String OpcaoText = "Entre com sua opção: ";

            
            bool successValida = false;
            bool isNumConta;

            int inputNumeroDaContaSacada = 0;
            int inputDigitoDaContaSacada = 0;

            bool success = false;
            String msg = "Confirma realização da Transferência? < S/s > ou < N/n >";
            Char[] vet = {'S','s','N','n'};
            do{
            
               char key =  GetKeyPress( msg, vet,87 , 28 );

                if ( ( key.Equals('S') )|| (key.Equals('s')) ){

                        WriteText(LinhaBrancaText,origCol,origRow+28);
                        WriteText(NumeroContaSacadaText,50,28);
                        inputNumeroDaContaSacada = PegarNumeroDaConta(NumeroContaSacadaText.Length + 52,28);
                        break;

                }else if ( ( key.Equals('N') )|| ( key.Equals('n') ) ){
                
                        WriteText(LinhaBrancaText,origCol,origRow+28);
                        WriteText(LinhaBrancaText,origCol,origRow+44);
                        WriteText(OpcaoText,origCol, origRow+44);
                        FullMenu();
                
                }
            }while(success == false);

            Console.SetCursorPosition(118,28);
            
            do {

                   try {

                        String testaDigito = Console.ReadLine();
                        successValida = ValidaCampo(testaDigito, isNumConta = false);

                        if (successValida){

                           success = int.TryParse(testaDigito,out inputDigitoDaContaSacada);

                        }

                     }catch (FormatException){
                    
                    Console.WriteLine("Unable to convert '{0}'.", inputDigitoDaContaSacada);
                
                    }catch (OverflowException){
                    
                    Console.WriteLine("'{0}' is out of range of the double type.", inputDigitoDaContaSacada);
                    }


                    if (successValida && success){
            
            
                       if(!inputDigitoDaContaSacada.Equals(GeraDigito(inputNumeroDaContaSacada))){;

                            WriteText(SolicitaText,87, 29);
                            Thread.Sleep(2000);

                           
                            for (int indx_ln = 1; indx_ln < 28;indx_ln++){
                                        
                                        WriteText(LinhaBrancaLimpaCadastro,50 , 25 + indx_ln);
                            
                            }

                            WriteText(LinhaBrancaText,origCol,origRow+28);
                            WriteText(LinhaBrancaText,origCol,origRow+44);
                            WriteText(OpcaoText,origCol, origRow+44);

                            FullMenu();

                        }
                    }else{
                    
                           success = true;

                    }

            }while(success == false);



            string testaValor = " ";
            double inputValorDoSaque = 0;
                      
            var itemaccount = AccountsFile[inputNumeroDaContaSacada-1].ToOpera(); 
        
            int[] vetIndx = new int[6]; 
            vetIndx = ImprimirItem(itemaccount);

            Console.SetCursorPosition(50,33);
            Console.Write ("Nome do Titular: ");
            Console.WriteLine(itemaccount.Substring(vetIndx[1]+1,(vetIndx[2]-vetIndx[1])-1));

            Console.SetCursorPosition(50,34);
            Console.Write("Valor depositado na Conta: ");
            Console.Write(itemaccount.Substring(vetIndx[2]+1,(vetIndx[3]-vetIndx[2])-1));

            Console.SetCursorPosition(97,34);
            Console.Write("Credito liberado na Conta: ");
            Console.WriteLine(itemaccount.Substring(vetIndx[3]+1,(vetIndx[4]-vetIndx[3])-1));

            Console.SetCursorPosition(50,35);
            Console.Write ("Valor disponivel para Saque: ");
            Console.Write(itemaccount.Substring(vetIndx[4]+1,(vetIndx[5]-vetIndx[4])-1));

            Console.SetCursorPosition(50,36);


//////////////////////////////  DEPOSITO ///////////////////////

            int inputDigitoDaContaCreditada = 0;
            int inputNumeroDaContaCreditada = 0;

            WriteText(NumeroContaCreditadaText,50,42);
            inputNumeroDaContaCreditada = PegarNumeroDaConta(NumeroContaSacadaText.Length + 52,42);

            Console.SetCursorPosition(118,42);
            
            do {

                   try {

                        String testaDigito = Console.ReadLine();
                        successValida = ValidaCampo(testaDigito, isNumConta = false);

                        if (successValida){

                           success = int.TryParse(testaDigito,out inputDigitoDaContaCreditada);

                        }

                     }catch (FormatException){
                    
                    Console.WriteLine("Unable to convert '{0}'.", inputDigitoDaContaCreditada);
                
                    }catch (OverflowException){
                    
                    Console.WriteLine("'{0}' is out of range of the double type.", inputDigitoDaContaCreditada);
                    }


                    if (successValida && success){
            
            
                       if(!inputDigitoDaContaCreditada.Equals(GeraDigito(inputNumeroDaContaCreditada))){;

                            WriteText(SolicitaText,87, 43);
                            Thread.Sleep(2000);
                            
                            
                            for (int indx_ln = 1; indx_ln < 28;indx_ln++){
                                        
                                        WriteText(LinhaBrancaLimpaCadastro,50 , 25 + indx_ln);
                            
                            }
                 
                            WriteText(LinhaBrancaText,origCol,origRow+44);
                            WriteText(OpcaoText,origCol, origRow+44);

                            FullMenu();

                        }
                    }else{
                    
                           success = true;

                    }

            }while(success == false);



            itemaccount = AccountsFile[inputNumeroDaContaCreditada-1].ToOpera(); 
        
            vetIndx = new int[6]; 
            vetIndx = ImprimirItem(itemaccount);
            
            Console.SetCursorPosition(50,43);
            Console.Write ("Nome do Titular: ");
            Console.WriteLine(itemaccount.Substring(vetIndx[1]+1,(vetIndx[2]-vetIndx[1])-1));

            Console.SetCursorPosition(50,44);
            Console.Write("Valor depositado na Conta: ");
            Console.Write(itemaccount.Substring(vetIndx[2]+1,(vetIndx[3]-vetIndx[2])-1));

            Console.SetCursorPosition(90,44);
            Console.Write("Credito liberado na Conta: ");
            Console.WriteLine(itemaccount.Substring(vetIndx[3]+1,(vetIndx[4]-vetIndx[3])-1));

            Console.SetCursorPosition(50,45);
            Console.Write ("Valor disponivel para Saque: ");
            Console.WriteLine(itemaccount.Substring(vetIndx[4]+1,(vetIndx[5]-vetIndx[4])-1));


            WriteText (ValorSaqueText,50,47); // pega valor do saque
            do{
                    try{
                        testaValor = Console.ReadLine();
                        successValida = ValidaCampo(testaValor, isNumConta = false);

                        if(successValida){

                            success = double.TryParse(testaValor,out inputValorDoSaque);
                          
                        }


                    }catch (FormatException){
                    
                    Console.WriteLine("Unable to convert '{0}'.", inputValorDoSaque);
                
                    }catch (OverflowException){
                    
                    Console.WriteLine("'{0}' is out of range of the double type.", inputValorDoSaque);
                    }

                    if (successValida && success && inputValorDoSaque > 0){

                        
                        WriteText(LinhaBrancaCadastroText,50,47);
                        Console.SetCursorPosition(50 + ValorSaqueText.Length + 2,47);
                        WriteText (ValorSaqueText,50,47);
                        WriteText (inputValorDoSaque.ToString("N2"),50 + 
                                   ValorSaqueText.Length + 2,47);

                    }else{

                        successValida = false;
                        success = false;
                        WriteText(SolicitaText,50, 48);
                        Thread.Sleep(2000);
                        WriteText(LinhaBrancaCadastroText,50,58);
                        WriteText(LinhaBrancaCadastroText,50,47);
                        Console.SetCursorPosition(50 + ValorSaqueText.Length + 2,47);
                        WriteText (ValorSaqueText,50,47);
                        inputValorDoSaque = 0;
                    }
                    
             }while (successValida == false);



            success = AccountsFile[inputNumeroDaContaSacada-1].Transferir(inputValorDoSaque,
                                                            AccountsFile[inputNumeroDaContaCreditada-1]);
            if(success){
            
                itemaccount = AccountsFile[inputNumeroDaContaSacada-1].ToOpera(); 
                
                vetIndx = new int[6]; 
                vetIndx = ImprimirItem(itemaccount);

                Console.SetCursorPosition(50,34);
                Console.Write("Valor depositado na Conta: ");
                Console.Write(itemaccount.Substring(vetIndx[2]+1,(vetIndx[3]-vetIndx[2])-1));

                Console.SetCursorPosition(97,34);
                Console.Write("Credito liberado na Conta: ");
                Console.WriteLine(itemaccount.Substring(vetIndx[3]+1,(vetIndx[4]-vetIndx[3])-1));

                Console.SetCursorPosition(50,35);
                Console.Write ("Valor disponivel para Saque: ");
                Console.Write(itemaccount.Substring(vetIndx[4]+1,(vetIndx[5]-vetIndx[4])-1));

                Console.SetCursorPosition(50,36);
    //////////////////////////////  DEPOSITO ///////////////////////

                
                itemaccount = AccountsFile[inputNumeroDaContaCreditada-1].ToOpera(); 
            
                vetIndx = new int[6]; 
                vetIndx = ImprimirItem(itemaccount);

                Console.SetCursorPosition(50,44);
                Console.Write("Valor depositado na Conta: ");
                Console.Write(itemaccount.Substring(vetIndx[2]+1,(vetIndx[3]-vetIndx[2])-1));

                Console.SetCursorPosition(90,44);
                Console.Write("Credito liberado na Conta: ");
                Console.WriteLine(itemaccount.Substring(vetIndx[3]+1,(vetIndx[4]-vetIndx[3])-1));

                Console.SetCursorPosition(50,45);
                Console.Write ("Valor disponivel para Saque: ");
                Console.WriteLine(itemaccount.Substring(vetIndx[4]+1,(vetIndx[5]-vetIndx[4])-1));

            
                WriteText ("Transferência realizado com Exito! ",70,50);
                Thread.Sleep(2000);
                WriteText(LinhaBrancaCadastroText,70,50);
                WriteText(LinhaBrancaCadastroText,90,50);

            
            }else{
            
                WriteText ("Transferência não foi realizada! Saldo Insuficiente! ",70,50);
                Thread.Sleep(2000);
                WriteText(LinhaBrancaCadastroText,70,50);
                WriteText(LinhaBrancaCadastroText,90,50);
            
            
            }
        
            success = true;
            msg = "Digite < S ou s > para fechar Voltar ao Menu Principal!";
                        
            do{
            
                char key =  GetKeyPress( msg, vet, 77, 52);

                if ( ( key.Equals('S') )|| (key.Equals('s')) ){

                    
                        for (int indx_ln = 1; indx_ln < 28;indx_ln++){
                                
                                WriteText(LinhaBrancaLimpaCadastro,50, 27 + indx_ln);
                                
                                
                        }
            
                        break;
                }
            }while(success);

           
            WriteText(LinhaBrancaText,origCol,origRow+44);
            WriteText(OpcaoText,origCol, origRow+44);


            FullMenu();

        }

        private static void CadastrarContas()
        {
            String SolicitaText = "Valor Inválido, tente novamente!";
            String SolicitaNome = "Nome Inválido, tente novamente!";
            String OpcaoText = "Entre com sua opção: ";

            
            bool success = false;
            String msg = "Confirma realização do Cadastro? < S/s > ou < N/n >";
            Char[] vet = {'S','s','N','n'};
            do{
            
               char key =  GetKeyPress( msg, vet,87 , 28 );

                if ( ( key.Equals('S') )|| (key.Equals('s')) ){

                        WriteText(LinhaBrancaText,origCol,origRow+28);
                        break;

                }else if ( ( key.Equals('N') )|| ( key.Equals('n') ) ){
                
                        WriteText(LinhaBrancaText,origCol,origRow+28);
                        WriteText(LinhaBrancaText,origCol,origRow+44);
                        WriteText(OpcaoText,origCol, origRow+44);
                        FullMenu();
                        
                
                }
            }while(success == false);


             Action printCabecalhoCadastro = () =>
                {
                    WriteText ("Numero da Conta       Digito Verificador ",Console.LargestWindowWidth - 87,28);
                    WriteText("              Espécie da Conta                                    "+
                              "             Tipo da Conta ",50,32);
                    WriteText("<< Conta Corrente = 1,  Poupança = 2 >> " ,50,34);
                    WriteText("<< Pessoa Física = 1,  Pessoa Jurídica = 2 >> " ,110,34);
                    WriteText("                  Perfil da Conta                                     "+
                              "     Status da Conta ",50,39);
                    WriteText("<< Individual = 1,  Conjunta = 2,    Empresa = 3 >> " ,50,41);
                    WriteText("<< Ativa = 1,  Boqueada = 2,  Encerrada = 3 >> " ,110,41);
                    
                  };
                printCabecalhoCadastro();

                ++contadorDeContas;
                int inputNumeroDaConta = contadorDeContas;
                
                int inputDigitoDaConta = GeraDigito(inputNumeroDaConta);
                
                WriteText (inputNumeroDaConta.ToString("000000")+"                     "+inputDigitoDaConta.ToString("0"), Console.LargestWindowWidth - 77,29);

                uint cadastro = GetUserCadastro(printCabecalhoCadastro, 2, 60, origRow + 35);
                int inputEspecieDaConta = (int)cadastro;
                                
                cadastro = GetUserCadastro(printCabecalhoCadastro, 2, 110, origRow + 35);
                int inputTipoDaConta = (int)cadastro;

                cadastro = GetUserCadastro(printCabecalhoCadastro, 3, 60, origRow + 42);
                int inputPerfilDaConta = (int)cadastro;

                cadastro = GetUserCadastro(printCabecalhoCadastro, 3, 110, origRow + 42);
                int inputStatusDaConta = (int)cadastro;
                
                bool successNullOrEmpty = true;
                string inputNomeDoTitular = " ";
                string testaValor;

                WriteText ("Nome do Titular: ", 50,45);
                do {
                    testaValor = Console.ReadLine().ToUpper();
                    successNullOrEmpty = IsAnyNullOrEmpty(testaValor);

                    if ( successNullOrEmpty == true ){

                            WriteText(SolicitaNome,50, 46);
                            Thread.Sleep(2000);
                            WriteText(LinhaBrancaCadastroText,50,46);
                            WriteText(LinhaBrancaCadastroText,50,45);
                            WriteText ("Nome do Titular: ", 50,45);
                            
                    }else{
                    
                        inputNomeDoTitular = testaValor;
                    
                    }

                }while (successNullOrEmpty == true); 

                successNullOrEmpty = true;
                string inputSobreNomeDoTitular = " ";
                WriteText ("Sobrenome do Titular: ",110,45);

                do {
                    testaValor = Console.ReadLine().ToUpper();
                    successNullOrEmpty = IsAnyNullOrEmpty(testaValor);

                    if ( successNullOrEmpty == true ){

                            WriteText(SolicitaNome,110, 46);
                            Thread.Sleep(2000);
                            WriteText(LinhaBrancaCadastroText,110,46);
                            WriteText(LinhaBrancaCadastroText,110,45);
                            WriteText ("Sobrenome do Titular: ", 110,45);
                            
                    }else{
                    
                        inputSobreNomeDoTitular = testaValor;
                    
                    }

                }while (successNullOrEmpty == true); 


                successNullOrEmpty = IsAnyNullOrEmpty(inputSobreNomeDoTitular);

                success = true;
                successNullOrEmpty = true;
                bool successIsAnyPoint = true;
                double inputValorEmConta = 0;
                

                WriteText ("Valor depositado na Conta: ",50,48);
                do{
                    try{
                        testaValor = Console.ReadLine();   
                        successNullOrEmpty = IsAnyNullOrEmpty(testaValor);
                        successIsAnyPoint = testaValor.Contains(".");

                        if ( (successNullOrEmpty == false ) && (successIsAnyPoint == false ) ){

                            success = double.TryParse(testaValor,out inputValorEmConta);
                        
                        }else{
                        
                            WriteText(SolicitaText,50, 49);
                            Thread.Sleep(2000);
                            WriteText(LinhaBrancaCadastroText,50,49);
                            WriteText(LinhaBrancaCadastroText,50,48);
                            WriteText ("Valor depositado na Conta: ",50,48);
                            
                        }


                    }catch (FormatException){
                    
                    Console.WriteLine("Unable to convert '{0}'.", inputValorEmConta);
                
                    }catch (OverflowException){
                    
                    Console.WriteLine("'{0}' is out of range of the double type.", inputValorEmConta);
                    }

                    if (inputValorEmConta < 0){

                        WriteText(SolicitaText,50, 49);
                        Thread.Sleep(2000);
                        WriteText(LinhaBrancaCadastroText,50,49);
                        WriteText(LinhaBrancaCadastroText,50,48);
                        WriteText ("Valor depositado na Conta: ",50,48);
                    
                    }
                    
                }while( (successNullOrEmpty == true || successIsAnyPoint == true) || inputValorEmConta < 0 ); 



                WriteText ("Valor do Cheque Especial liberado na Conta: ",100,48);
                
                success = true;
                successNullOrEmpty = true;
                successIsAnyPoint = true;
                double inputValorChequeEspecial = 0;
                
                do{
                        try{
                        testaValor = Console.ReadLine();   
                        successNullOrEmpty = IsAnyNullOrEmpty(testaValor);
                        successIsAnyPoint = testaValor.Contains(".");

                        if ( (successNullOrEmpty == false ) && (successIsAnyPoint == false ) ){

                            success = double.TryParse(testaValor,out inputValorChequeEspecial);
                             
                        }else{


                            WriteText(SolicitaText,100, 49);
                            Thread.Sleep(2000);
                            WriteText(LinhaBrancaCadastroText,100,49);
                            WriteText(LinhaBrancaCadastroText,100,48);
                            WriteText(LinhaBrancaCadastroText,110,48);
                            WriteText ("Valor do Cheque Especial liberado na Conta: ",100,48);
                            
                        }


                        }catch (FormatException){
                        
                        Console.WriteLine("Unable to convert '{0}'.", inputValorChequeEspecial);
                    
                        }catch (OverflowException){
                        
                        Console.WriteLine("'{0}' is out of range of the double type.", inputValorChequeEspecial);
                        }

                        if (inputValorChequeEspecial < 0){

                            WriteText(SolicitaText,100, 49);
                            Thread.Sleep(2000);
                            WriteText(LinhaBrancaCadastroText,100,49);
                            WriteText(LinhaBrancaCadastroText,100,48);
                            WriteText(LinhaBrancaCadastroText,110,48);
                            WriteText ("Valor do Cheque Especial liberado na Conta: ",100,48);
                        
                        }
                        
                    }while( ( (successNullOrEmpty == true || successIsAnyPoint == true) || inputValorChequeEspecial < 0)  ); 



                WriteText ("Saldo Disponivel na Conta: ",80,50);
                double inputSaldoDaConta = inputValorEmConta + inputValorChequeEspecial;
                Console.WriteLine(inputSaldoDaConta.ToString("N2",new CultureInfo("pt-BR")));
                             
            Account newAccount = new Account(numeroDaConta: inputNumeroDaConta,  digitoDaConta: inputDigitoDaConta,
                         especieDaConta: (SpecieOfAccount) inputEspecieDaConta, 
                         tipoDaConta: (TypeOfAccount) inputTipoDaConta, 
                         perfilDaConta: (ProfileOfAccount) inputPerfilDaConta,
                         statusDaConta: (StatusOfAccount) inputStatusDaConta,
                         nomeDoTitular: inputNomeDoTitular, sobreNomeDoTitular: inputSobreNomeDoTitular,
                         valorEmConta: inputValorEmConta, valorChequeEspecial: inputValorChequeEspecial,
                         saldoDaConta: inputSaldoDaConta);

            AccountsFile.Add(newAccount);
           
            success = true;
            msg = "Digite < S ou s > para fechar Cadastro!";
            
            do{
            
               char key =  GetKeyPress( msg, vet,77,52);

                if ( ( key.Equals('S') )|| (key.Equals('s')) ){

                    
                        for (int indx_ln = 1; indx_ln < 28;indx_ln++){
                                
                                WriteText(LinhaBrancaLimpaCadastro,50, 27 + indx_ln);
                                
                                
                        }
            
                        success = false;
                }
            }while(success);

                
            WriteText ("Conta cadastrada com Exito! ",80,35);
            WriteText(LinhaBrancaText,origCol,origRow+44);
            WriteText(OpcaoText,origCol, origRow+44);


            FullMenu();
        }


        private static void ListarContas(){

            List<Account> tmpAccount = new List<Account>();

            String OpcaoText = "Entre com sua opção: ";
            String Cabec1 = "  Conta-DV |    Especie    |        Tipo      |       Perfil          |     Status    |";
            String Cabec2 = "  Titular                  |   Valor na Conta | Valor Cheque Especial |     Saldo     | ";  
            String SeparaText = new String('-', Cabec2.Length + 8);
            String SolicitaText = "Valor Inválido, tente novamente!";

            String msg = "Confirma Exibição de Dados da Conta? < S/s > ou < N/n >";
            Char[] vet = {'S','s','N','n'};

            int inputNumeroDaConta = 0;
            int inputDigitoDaConta = 0;
                
            bool success = false;
            bool successValida = false;
            bool isNumConta;
                        
            if (AccountsFile.Count == 0){

                WriteText ("Nenhuma Conta cadastrada! ",80,35);
                WriteText(LinhaBrancaText,origCol,origRow+44);
                WriteText(OpcaoText,origCol, origRow+44);

                FullMenu();
               
            }else{

               
                do{
                
                char key =  GetKeyPress( msg, vet,87 , 28 );

                    if ( ( key.Equals('S') )|| (key.Equals('s')) ){

                            WriteText(LinhaBrancaText,origCol,origRow+28);
                            inputNumeroDaConta = PegarNumeroDaConta(87,28);
                            break;

                    }else if ( ( key.Equals('N') )|| ( key.Equals('n') ) ){

                            WriteText(LinhaBrancaText,origCol,origRow+28);
                            WriteText(LinhaBrancaText,origCol,origRow+44);
                            WriteText(OpcaoText,origCol, origRow+44);
                            FullMenu();
                    
                    }
                }while(success == false);

                Console.SetCursorPosition(133,28);
                
                do {

                    try {

                            String testaDigito = Console.ReadLine();
                            successValida = ValidaCampo(testaDigito, isNumConta = false);

                            if (successValida){

                            success = int.TryParse(testaDigito,out inputDigitoDaConta);

                            }

                        }catch (FormatException){
                        
                        Console.WriteLine("Unable to convert '{0}'.", inputDigitoDaConta);
                    
                        }catch (OverflowException){
                        
                        Console.WriteLine("'{0}' is out of range of the double type.", inputDigitoDaConta);
                        }


                        if (successValida && success){
                
                
                        if(!inputDigitoDaConta.Equals(GeraDigito(inputNumeroDaConta))){;

                                WriteText(SolicitaText,87, 29);
                                Thread.Sleep(2000);
                                WriteText(LinhaBrancaCadastroText,87,29);
                                WriteText(LinhaBrancaCadastroText,87,28);
                                WriteText(LinhaBrancaCadastroText,97,28);
                                WriteText(LinhaBrancaText,origCol,origRow+44);
                                WriteText(OpcaoText,origCol, origRow+44);

                                FullMenu();

                            }
                        }else{
                        
                            success = true;

                        }

                }while(success == false);



                WriteText(SeparaText,70,30);
                WriteText(Cabec1,70,31);
                WriteText(Cabec2,70,32);
                WriteText(SeparaText,70,33);

                tmpAccount.Add(AccountsFile[inputNumeroDaConta-1]);
              
                int indx = 1;
                foreach (var indcount in tmpAccount){
                                       
                   WriteText(indcount.ToString(),71,33 + indx);
                   indx += 3;
                }
            }

            
            success = true;
            msg = "Digite < S ou s > para fechar Relatório!";
            
            do{
            
               char key =  GetKeyPress( msg, vet, 77, 52);

                if ( ( key.Equals('S') )|| (key.Equals('s')) ){

                    
                        for (int indx_ln = 1; indx_ln < 28;indx_ln++){
                                
                                WriteText(LinhaBrancaLimpaCadastro,50, 27 + indx_ln);
                                
                                
                        }
            
                        break;
                }
            }while(success);

            WriteText(LinhaBrancaText,origCol,origRow+44);
            WriteText(OpcaoText,origCol, origRow+44);
            
            FullMenu();
        }

        static bool CheckPassword(string InputText, int posx, int posy)  
                {  
                    try  
                    {  
                        WriteText(InputText,posx,posy);  
                        EnteredVal = "";  

                        do  
                        {  
                            ConsoleKeyInfo key = Console.ReadKey(true);  
                            // Backspace Should Not Work  
                            if (key.Key != ConsoleKey.Backspace && key.Key != ConsoleKey.Enter
                               && key.Key != ConsoleKey.Spacebar && key.Key != ConsoleKey.Escape)  
                            {  
                                EnteredVal += key.KeyChar;  
                                Console.Write("*");  
                            }  
                            else  
                            {  
                                if (key.Key == ConsoleKey.Backspace && EnteredVal.Length > 0)  
                                {  
                                    EnteredVal = EnteredVal.Substring(0, (EnteredVal.Length - 1));  
                                    Console.Write("\b \b");  
                                }  
                                else if (key.Key == ConsoleKey.Enter)  
                                {  
                                    if (string.IsNullOrWhiteSpace(EnteredVal))  
                                    {  
                                        Console.WriteLine("");  
                                        WriteText("Senha vazia, não permitido!.",posx,posy+1);
                                        Thread.Sleep(2000);  
                                        WriteText(LinhaBrancaText,posx,posy+1);
                                        WriteText(LinhaBrancaText,posx,posy);
                                        WriteText(InputText,posx,posy);
                                        EnteredVal = "";  
                                        continue;  
                                    }  
                                    else  
                                    {  
                                       
                                        if (EnteredVal.Length != 6 ){

                                            Console.WriteLine("");
                                            Console.WriteLine("Senha invalida! A senha deve ter 6 caracteres!");
                                            Thread.Sleep(2000);
                                            WriteText(LinhaBrancaText,posx,posy+1);
                                            WriteText(LinhaBrancaText,posx,posy);
                                            WriteText(InputText,posx,posy);
                                            EnteredVal = "";  
                                            continue;  

                                        }else if (EnteredVal.Equals(GuestPasswordVal) ){

                                            Console.WriteLine("");
                                            Console.WriteLine("Senha Validada! Aguarde a inicialização do sistema!");
                                            Thread.Sleep(2000);
                                            WriteText(LinhaBrancaText,posx,posy+1);
                                            WriteText(LinhaBrancaText,posx,posy);
                                            WriteText(InputText,posx,posy);
                                            EnteredVal = ""; 
                                            return true;

                                        }else{

                                            if (EnteredVal.Equals(EndPasswordVal) ){
                                                Console.WriteLine("");
                                                Console.WriteLine("Aguarde! Encerrando o sistema!");
                                                Thread.Sleep(2000);
                                                return false;
                                            }else { 

                                                Console.WriteLine("");
                                                Console.WriteLine("Senha Invalida! Não associada a este Usuário!");
                                                Thread.Sleep(2000);
                                                WriteText(LinhaBrancaText,posx,posy+1);
                                                WriteText(LinhaBrancaText,posx,posy);
                                                WriteText(InputText,posx,posy);
                                                EnteredVal = ""; 
                                                continue;

                                            }

                                        } 
                                    }  
                                }  
                            }  
                        } while (true);  
                    }  
                    catch (Exception ex)  
                    {  
                        throw ex;  
                    }  
                }  



        private static int PegarNumeroDaConta(int col, int row){
        
            String SolicitaConta  = "Conta Inválida, tente novamente!";
            String NumeroContaText = "Entre com o número da Conta e o Digito:      - ";
            String OpcaoText = "Entre com sua opção: ";
            
            bool successValida = false;
            bool success = false;
            bool isNumConta = true;

            int inputNumeroDaConta = 0;
           
            string testaValor;

            WriteText(NumeroContaText,col,row);
            WriteText(NumeroContaText.Substring(0,40),col,row);   // pega nunmero da conta

            do{
                try{
                    testaValor = Console.ReadLine();
                    successValida = ValidaCampo(testaValor, isNumConta = true);
              
                    if(successValida){
                        success = Int32.TryParse(testaValor,out inputNumeroDaConta);
                    }



                }catch (FormatException){
                
                Console.WriteLine("Unable to convert '{0}'.", inputNumeroDaConta);
            
                }catch (OverflowException){
                
                Console.WriteLine("'{0}' is out of range of the double type.", inputNumeroDaConta);
                }

                 if ( successValida && success && (inputNumeroDaConta <= contadorDeContas) ){

                        
                        WriteText(LinhaBrancaCadastroText,col,row);
                        WriteText (NumeroContaText,col,row);
                        WriteText (inputNumeroDaConta.ToString("00000"),col + 40 ,row);
                        break;   
                                    
                   }else{
                   
                    successValida = false;
                    success = false;
                    WriteText(SolicitaConta,col,row+1);
                    Thread.Sleep(2000);

                    for (int indx_ln = 1; indx_ln < 28;indx_ln++){
                                
                                WriteText(LinhaBrancaLimpaCadastro,50 , 25 + indx_ln);
                    
                    }
                  
                    WriteText(LinhaBrancaText,origCol,origRow+44);
                     WriteText(OpcaoText,origCol, origRow+44);
                    FullMenu();
                   
                   }

            }while (successValida == false);

            return inputNumeroDaConta;
        
        
        }

        
        private static bool ValidaCampo(string valcampo, bool isNumConta){
            
                    string testaCampo;
                    
                    bool successNullOrEmpty = false;
                    bool successIsAnyNotDigit = false;
                    bool successIsAnyPoint = false;


                    testaCampo = valcampo;   
                    successNullOrEmpty = IsAnyNullOrEmpty(testaCampo);
                    successIsAnyNotDigit = IsAnyNotDigit(testaCampo,isNumConta);
                    successIsAnyPoint = testaCampo.Contains(".");


                    if ( (successNullOrEmpty == false ) && (successIsAnyNotDigit == false )
                       && successIsAnyPoint == false){

                         return true; 

                    }else{
                    
                         return false;
                    }
   
        }



        private static int[] ImprimirItem(string item){

           int end = item.Length;
           int start = 0;
           int count = 0;
           int at = 0;

           int[] vet = new int[6]; 
           int i = 0;

            while((start <= end) && (at > -1))
            {
                // start+count must be a position within -str-.
                count = end - start;
                at = item.IndexOf("|", start, count);
                if (at == -1){
                  break;
                } 
                vet[i] = at;
                //Console.Write("{0} ", at);
                i++;
                start = at+1;
            }  
        
            return vet;
        
        
        }



        static void WriteText(String InputTextToDisplay, Int32 xLocation, Int32 yLocation ){

                       
            bool continueFlag = true;
              

            do {
                
               String textToDisplay = InputTextToDisplay;
               DisplayText(InputTextToDisplay,xLocation,yLocation);
               continueFlag = false;
                
                               
            } while (continueFlag);
        }


         static void DisplayText(String s, Int32 x, Int32 y){
            try
                {

                    Console.SetCursorPosition(origCol+x, origRow+y);
                    if (s.Equals("Conta cadastrada com Exito! ") || 
                        s.Equals("Nenhuma Conta cadastrada! ") ||
                        s.Equals("Deposito realizado com Exito! ") ||
                        s.Equals("Saldo insuficiente para esta operação! ") ||
                        s.Equals("Saque realizado com Exito! ") ){
                    
                        int numflash = 0;
                        while (numflash < 5){
                                
                                    WriteBlinkingText(s, 250, true);
                                    WriteBlinkingText(s, 250, false);
                                    ++numflash;
                            }
                     
                    }else{
                    
                        Console.Write(s);
                    }
                }
            catch (ArgumentOutOfRangeException e)
                {
                Console.Clear();
                Console.Write(e.Message);
                }
            }


        private static void WriteBlinkingText(string text, int delay, bool visible) {
            if (visible)
                Console.Write(text);
            else
                for (int i = 0; i < text.Length; i++)
                    Console.Write(" ");

            Console.CursorLeft -= text.Length;
            System.Threading.Thread.Sleep(delay);
        }




         static Char GetKeyPress(String msg, Char[] validChars,int col, int row)
        {
            ConsoleKeyInfo keyPressed;
            bool valid = false;

            Console.WriteLine();
            do {
                WriteText(msg, col, row);
                keyPressed = Console.ReadKey();
                Console.WriteLine();
                if (Array.Exists(validChars, ch => ch.Equals(Char.ToUpper(keyPressed.KeyChar))))
                    valid = true;
            } while (! valid);
            return keyPressed.KeyChar;
        }



        /// To check the properties of a class for Null/Empty values
        /// </summary>
        /// <param name="obj">The instance of the class</param>
        /// <returns>Result of the evaluation</returns>
        public static bool IsAnyNullOrEmpty(object obj)
        {
            //Step 1: Set the result variable to false;
            bool result = false;

            try
            {
                //Step 2: Check if the incoming object has values or not.
                if (obj != null)
                {
                    //Step 3: Iterate over the properties and check for null values based on the type.
                    foreach (System.Reflection.PropertyInfo pi in obj.GetType().GetProperties())
                    {
                        //Step 4: The null check condition only works if the value of the result is false, whenever the result gets true, the value is returned from the method.
                        if (result == false)
                        {
                            //Step 5: Different conditions to satisfy different types
                            dynamic value;
                            if (pi.PropertyType == typeof(string))
                            {
                                value = (string)pi.GetValue(obj);
                                result = (string.IsNullOrEmpty(value) ? true : false || string.IsNullOrWhiteSpace(value) ? true : false);
                            }
                            else if (pi.PropertyType == typeof(int))
                            {
                                value = (int)pi.GetValue(obj);
                                result = (value <= 0 ? true : false || value == null ? true : false);
                            }
                            else if (pi.PropertyType == typeof(bool))
                            {
                                value = pi.GetValue(obj);
                                result = (value == null ? true : false);
                            }
                            else if (pi.PropertyType == typeof(Guid))
                            {
                                value = pi.GetValue(obj);
                                result = (value == Guid.Empty ? true : false || value == null ? true : false);
                            }
                        }
                        //Step 6 - If the result becomes true, the value is returned from the method.
                        else
                            return result;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            //Step 7: If the value doesn't become true at the end of foreach loop, the value is returned.
            return result;
        }


        public static bool IsAnyNotDigit(string campo, bool isNumConta){
        
            //Step 1: Set the result variable to false;
            bool result = false;
            int contVirgula = 0;
           
            try {
                                                              
                    foreach(char c in campo) {    

                        if (c >= '0' && c <= '9'){    
                            continue;   
                        }else{

                            if ( (isNumConta == true) || (contVirgula == 1) ){
                                result = true;    
                                break; 
                            }

                            ++contVirgula;
                                                           
                        }     
                    }     

            }catch (Exception ex){
                throw ex;
            }

            //Step 7: If the value doesn't become true at the end of foreach loop, the value is returned.
            return result;
    
        }

        private static int GeraDigito(int numconta){
        
            int[] intPesos = { 2, 1, 2, 1, 2, 1};
            string strText = numconta.ToString("000000");
    
            if (strText.Length > 6)
                throw new Exception("Número não suportado pela função!");
    
            int intSoma = 0;
            int intIdx = 0;
            for (int intPos = strText.Length - 1; intPos >= 0; intPos--)
            {
                intSoma += Convert.ToInt32(strText[intPos].ToString()) * intPesos[intIdx];
                intIdx++;
            }
    
            intSoma = intSoma % 10;
            intSoma = 10 - intSoma;
            if (intSoma == 10)
            {
                intSoma = 0;
            }
    
            return intSoma;
        }
            
    }

}


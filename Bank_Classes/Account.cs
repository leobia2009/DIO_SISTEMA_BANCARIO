using System;
using System.Globalization;

namespace Bank_Account_Control{

        public class Account{
        
            private int NumeroDaConta {get; set;}
            private int DigitoDaConta {get; set;}
            private SpecieOfAccount EspecieDaConta {get; set;}
            private TypeOfAccount TipoDaConta {get; set;}
            private ProfileOfAccount PerfilDaConta {get; set;}
            private  StatusOfAccount StatusDaConta {get; set;}

            private string NomeDoTitular {get; set;}
            private  string SobreNomeDoTitular {get; set;}
            private double ValorChequeEspecial {get; set;}
            private double ValorEmConta {get; set;}
            private double SaldoDaConta {get; set;}

            public Account(int numeroDaConta, int digitoDaConta, SpecieOfAccount especieDaConta,
            TypeOfAccount tipoDaConta, ProfileOfAccount perfilDaConta, StatusOfAccount statusDaConta,
            string nomeDoTitular, string sobreNomeDoTitular, double valorChequeEspecial, 
            double valorEmConta, double saldoDaConta){

                this.NumeroDaConta = numeroDaConta;
                this.DigitoDaConta = digitoDaConta;
                this.EspecieDaConta = especieDaConta;
                this.TipoDaConta = tipoDaConta;
                this.PerfilDaConta = perfilDaConta;
                this.StatusDaConta = statusDaConta;
                this.NomeDoTitular = nomeDoTitular;
                this.SobreNomeDoTitular = sobreNomeDoTitular;
                this.ValorEmConta = valorEmConta;
                this.ValorChequeEspecial = valorChequeEspecial;
                this.SaldoDaConta = saldoDaConta;
            }


            public override string ToString(){

                var AcertaPosicao = new string('\t',9);
                var SeparaTexto = new string('-',82);

                var AccountItem = " ";
                AccountItem += this.NumeroDaConta.ToString("00000");
                AccountItem += " - "+this.DigitoDaConta.ToString("0") + " ";

                AccountItem +=  this.EspecieDaConta + "       ";
                AccountItem +=  this.TipoDaConta + "     ";
                AccountItem +=  this.PerfilDaConta + "          ";
                AccountItem +=  this.StatusDaConta + "    "+"\n";
                AccountItem +=  AcertaPosicao;

                int  position =  this.NomeDoTitular.IndexOf(" ");
                if (position == -1){
                
                    position = this.NomeDoTitular.Length;
                
                }
                String nome   =  this.NomeDoTitular.Substring(0, position);
                                
                AccountItem +=  nome + " " +
                                this.SobreNomeDoTitular.Trim();

                int lengthAccountItem = nome.Length;
                int valorOfSet  = 100 -  (lengthAccountItem + 72);
                var AcertaPosicaoValor = new String(' ',valorOfSet);

                AccountItem +=  AcertaPosicaoValor;
                AccountItem +=  this.ValorEmConta.ToString("N2",new CultureInfo("pt-BR"));

                var digitos = this.ValorEmConta.ToString("N2",new CultureInfo("pt-BR"));
                int numdigitos = digitos.Length;

                lengthAccountItem = numdigitos;
                valorOfSet  = 118 -  (lengthAccountItem + 100);
                var AcertaPosicaoCheque = new String(' ',valorOfSet);

                AccountItem +=  AcertaPosicaoCheque;
                AccountItem +=  this.ValorChequeEspecial.ToString("N2",new CultureInfo("pt-BR"));

                
                digitos = this.ValorChequeEspecial.ToString("N2",new CultureInfo("pt-BR"));
                numdigitos = digitos.Length;

                lengthAccountItem = numdigitos;
                valorOfSet  = 143 -  (lengthAccountItem + 118);
                var AcertaPosicaoSaldo = new String(' ',valorOfSet);

                AccountItem +=  AcertaPosicaoSaldo;
                AccountItem +=  this.SaldoDaConta.ToString("N2",new CultureInfo("pt-BR")) + "\n";

                AccountItem +=  AcertaPosicao;
                AccountItem +=  SeparaTexto + "\n";

                return AccountItem;

            }


            public string ToOpera(){

                var AccountItem = "|";
                AccountItem += this.NumeroDaConta.ToString("00000");
                AccountItem += "-"+this.DigitoDaConta.ToString("0") + "|";

                String nome   =  this.NomeDoTitular.Trim();
                                
                AccountItem +=  nome + " " +
                                this.SobreNomeDoTitular.Trim() + "|";

                AccountItem +=  this.ValorEmConta.ToString("N2",new CultureInfo("pt-BR"))
                                + "|";

                AccountItem +=  this.ValorChequeEspecial.ToString("N2",new CultureInfo("pt-BR"))
                                + "|";
                AccountItem +=  this.SaldoDaConta.ToString("N2",new CultureInfo("pt-BR")) + "|";
                return AccountItem;

            }


            public bool Sacar(double ValorDoSaque){

                double ValorDisponivelConta = this.ValorEmConta + this.ValorChequeEspecial;
                
                if ( ValorDoSaque > ValorDisponivelConta){
            
                    return false;
                
                }else{
              
                    this.ValorEmConta = this.ValorEmConta - ValorDoSaque;
                    this.SaldoDaConta = this.ValorEmConta + this.ValorChequeEspecial;
                    return true;
                
                }
                
                
                
            }
            
            public bool Depositar(double ValorDoDeposito){

                double ValorDisponivelConta = this.ValorEmConta + this.ValorChequeEspecial;
                
                            
                this.ValorEmConta += ValorDoDeposito;
                this.SaldoDaConta = this.ValorEmConta + this.ValorChequeEspecial;
                return true;
                
                
            }

             public bool Transferir(double ValorDaTransferencia, Account contaDeposito){
                
                bool success = true;

                if( this.Sacar(ValorDaTransferencia) ){
                
                  contaDeposito.Depositar(ValorDaTransferencia) ; 
                
                  return success;
                
                }else{
                
                success = false;
                return success;

                }
                
                
            }

            
        }
    
}



